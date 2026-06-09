#!/usr/bin/env python3
"""Download one OpenVINO platform archive from the official CDN, verify its
SHA-256, extract the native libraries, and pack them as a NuGet runtime
package.

Inputs come from environment variables (set by the GH Actions matrix):
  PKG_ID          NuGet ID suffix, e.g. "macos-arm64" -> OpenVINO.runtime.macos-arm64
  PKG_VERSION     NuGet package version, e.g. "2026.1.0"
  ARCHIVE_URL     Full URL to the .tgz / .zip on storage.openvinotoolkit.org
  SHA256_URL      Full URL to the .sha256 sibling
  RID             Standard .NET RID, e.g. "osx-arm64"
  KIND            "tgz" or "zip"
  REPO_ROOT       Path to the repository checkout (defaults to CWD)
  OUT_DIR         Where to drop the .nupkg (defaults to <REPO_ROOT>/out)
  AUTHORS         (optional) package authors string, defaults to "Guojin Yan"

Output: a single .nupkg at $OUT_DIR/OpenVINO.runtime.<PKG_ID>.<PKG_VERSION>.nupkg
"""

from __future__ import annotations

import hashlib
import os
import re
import shutil
import subprocess
import sys
import tarfile
import tempfile
import urllib.request
import zipfile
from pathlib import Path

NUGET_ID_PREFIX = "OpenVINO.runtime."

# Subdirectory globs we *exclude* when collecting native files from the
# extracted OpenVINO archive. Everything else is fair game if it has a
# native-library extension.
EXCLUDE_DIR_PARTS = {
    "python",
    "samples",
    "tools",
    "docs",
    "share",       # cmake configs, pkgconfig
    "include",     # header files
    "cmake",
    "pyopenvino",
    "tests",
    "test",
}

# Filename suffixes that count as native libraries.
NATIVE_PATTERNS = re.compile(
    r"\.(dll|so|dylib)(\.\d+)*$", re.IGNORECASE
)

PLATFORM_LABELS = {
    "win": "Windows (x86_64)",
    "ubuntu.24-x86_64": "Ubuntu 24.04 (x86_64)",
    "ubuntu.22-x86_64": "Ubuntu 22.04 (x86_64)",
    "ubuntu.22-arm64":  "Ubuntu 22.04 (arm64)",
    "ubuntu.20-x86_64": "Ubuntu 20.04 (x86_64)",
    "ubuntu.20-arm64":  "Ubuntu 20.04 (arm64)",
    "ubuntu.18-x86_64": "Ubuntu 18.04 (x86_64)",
    "ubuntu.18-arm64":  "Ubuntu 18.04 (arm64)",
    "debian10-armhf":   "Debian 10 (armhf)",
    "debian9-arm64":    "Debian 9 (arm64)",
    "debian9-armhf":    "Debian 9 (armhf)",
    "centos7-x86_64":   "CentOS 7 (x86_64)",
    "centos8-x86_64":   "CentOS 8 (x86_64)",
    "rhel8-x86_64":     "RHEL 8 (x86_64)",
    "macos-x86_64":     "macOS (x86_64)",
    "macos-arm64":      "macOS (arm64)",
}


def env(name: str, default: str | None = None) -> str:
    val = os.environ.get(name, default)
    if val is None:
        sys.exit(f"missing required env var: {name}")
    return val


def download(url: str, dest: Path) -> None:
    print(f"  GET  {url}", flush=True)
    req = urllib.request.Request(url, headers={"User-Agent": "openvino-csharp-runtime-bot"})
    with urllib.request.urlopen(req, timeout=300) as resp, open(dest, "wb") as fh:
        shutil.copyfileobj(resp, fh, length=1 << 20)


def sha256_of(path: Path) -> str:
    h = hashlib.sha256()
    with open(path, "rb") as fh:
        for chunk in iter(lambda: fh.read(1 << 20), b""):
            h.update(chunk)
    return h.hexdigest()


def parse_expected_sha256(sha_text: str, archive_name: str) -> str:
    """Parse a `sha256sum`-style file. Tolerates either '<hex> *<name>' or
    '<hex>  <name>' or just '<hex>'."""
    for line in sha_text.splitlines():
        line = line.strip()
        if not line:
            continue
        parts = line.split()
        if not re.fullmatch(r"[0-9a-fA-F]{64}", parts[0]):
            continue
        if len(parts) == 1 or any(archive_name in p for p in parts[1:]):
            return parts[0].lower()
    sys.exit(f"could not parse sha256 from:\n{sha_text}")


def extract(archive: Path, kind: str, dest: Path) -> Path:
    dest.mkdir(parents=True, exist_ok=True)
    if kind == "tgz":
        with tarfile.open(archive, "r:gz") as tf:
            # Python 3.12+ requires an explicit filter for safe extraction.
            try:
                tf.extractall(dest, filter="data")
            except TypeError:
                tf.extractall(dest)
    elif kind == "zip":
        with zipfile.ZipFile(archive) as zf:
            zf.extractall(dest)
    else:
        sys.exit(f"unknown archive kind: {kind}")

    # The archive typically has a single top-level directory; descend into it.
    entries = [p for p in dest.iterdir() if p.is_dir()]
    if len(entries) == 1:
        return entries[0]
    return dest


def is_native_file(path: Path) -> bool:
    name = path.name
    # Filter out python extension modules (.cpython-*.so etc.) — they live
    # under python/ anyway but be defensive.
    if ".cpython-" in name or ".python-" in name:
        return False
    # Reject Windows debug-symbol files.
    if name.lower().endswith(".pdb"):
        return False
    return bool(NATIVE_PATTERNS.search(name))


def collect_native_files(root: Path) -> list[Path]:
    """Walk the extracted toolkit and return every native library file
    outside the excluded subtrees."""
    out: list[Path] = []
    for path in root.rglob("*"):
        if not path.is_file() and not path.is_symlink():
            continue
        rel_parts = {p.lower() for p in path.relative_to(root).parts}
        if rel_parts & EXCLUDE_DIR_PARTS:
            continue
        if is_native_file(path):
            out.append(path)
    return out


def stage_files(files: list[Path], target_dir: Path) -> list[str]:
    """Copy native files into target_dir as flat regular files.

    Symlinks are dereferenced (the resulting nupkg cannot store symlinks
    reliably). Duplicate basenames are deduped — the last writer wins, but
    in practice OpenVINO archives don't produce conflicts because the
    versioned dylib siblings (`libopenvino.dylib`, `libopenvino.2026.1.0.dylib`,
    `libopenvino.2610.dylib`) all have distinct filenames.
    """
    target_dir.mkdir(parents=True, exist_ok=True)
    staged: dict[str, Path] = {}
    for src in files:
        name = src.name
        if name in staged:
            print(f"  WARN: duplicate basename {name} (keeping first)", file=sys.stderr)
            continue
        dst = target_dir / name
        # Follow symlinks: read the real file's bytes.
        shutil.copyfile(str(src), str(dst), follow_symlinks=True)
        try:
            shutil.copystat(str(src), str(dst), follow_symlinks=True)
        except OSError:
            pass
        staged[name] = dst
    return sorted(staged.keys())


def render(template_path: Path, mapping: dict[str, str]) -> str:
    text = template_path.read_text(encoding="utf-8")
    for key, value in mapping.items():
        text = text.replace("{" + key + "}", value)
    return text


def render_content_items(filenames: list[str], rid: str, link_subdir: str) -> str:
    """One Content/Link entry per native file, for the .NET Framework
    branch of the props file."""
    indent = "    "
    lines: list[str] = []
    for name in filenames:
        lines.extend([
            f'{indent}<Content Include="$(OpenVINORuntime)\\{rid}\\native\\{name}">',
            f'{indent}  <Link>dll\\{link_subdir}\\{name}</Link>',
            f'{indent}  <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>',
            f'{indent}</Content>',
        ])
    return "\n".join(lines)


def build_package(
    pkg_id: str,
    version: str,
    archive_url: str,
    sha256_url: str,
    rid: str,
    kind: str,
    repo_root: Path,
    out_dir: Path,
    authors: str,
) -> Path:
    nuget_id = NUGET_ID_PREFIX + pkg_id
    label = PLATFORM_LABELS.get(pkg_id, pkg_id)
    templates = repo_root / "nuget" / "runtime" / "templates"
    license_path = repo_root / "LICENSE.txt"
    logo_path = repo_root / "nuget" / "logo.jpg"

    if not license_path.exists():
        sys.exit(f"missing {license_path}")
    if not logo_path.exists():
        sys.exit(f"missing {logo_path}")

    with tempfile.TemporaryDirectory(prefix="ov-pkg-") as workdir_str:
        workdir = Path(workdir_str)
        downloads = workdir / "downloads"
        downloads.mkdir()

        archive_name = archive_url.rsplit("/", 1)[-1]
        archive_path = downloads / archive_name
        sha_path = downloads / (archive_name + ".sha256")

        download(archive_url, archive_path)
        download(sha256_url, sha_path)

        expected = parse_expected_sha256(sha_path.read_text(encoding="utf-8"), archive_name)
        actual = sha256_of(archive_path)
        if expected.lower() != actual.lower():
            sys.exit(
                f"SHA-256 mismatch for {archive_name}\n"
                f"  expected: {expected}\n"
                f"  actual:   {actual}"
            )
        print(f"  SHA-256 OK: {actual}", flush=True)

        extract_root = workdir / "extracted"
        toolkit_root = extract(archive_path, kind, extract_root)
        print(f"  toolkit root: {toolkit_root}", flush=True)

        native_files = collect_native_files(toolkit_root)
        if not native_files:
            sys.exit(f"no native files found under {toolkit_root}")
        print(f"  found {len(native_files)} native files", flush=True)

        # Staging directory mirrors the nupkg layout.
        stage = workdir / "stage"
        runtime_dir = stage / "runtimes" / rid / "native"
        filenames = stage_files(native_files, runtime_dir)

        # Per-platform README, props, and nuspec.
        mapping = {
            "NUGET_ID":       nuget_id,
            "VERSION":        version,
            "AUTHORS":        authors,
            "PLATFORM_LABEL": label,
            "ARCHIVE_URL":    archive_url,
            "RID":            rid,
            "LINK_SUBDIR":    pkg_id,
        }

        # Render props with one Content entry per native file.
        content_block = render_content_items(filenames, rid, pkg_id)
        props_text = render(templates / "package.props.tmpl", {**mapping, "CONTENT_ITEMS": content_block})
        props_dir = stage / "build" / "net"
        props_dir.mkdir(parents=True)
        (props_dir / f"{nuget_id}.props").write_text(props_text, encoding="utf-8")
        # Runtime-only packages include a framework placeholder to silence
        # NU5127 and keep NuGet compatibility metadata aligned with build/net.
        # 运行时包没有托管程序集，因此放置空占位文件，明确该包与 build/net 对齐。
        lib_net_dir = stage / "lib" / "net"
        lib_net_dir.mkdir(parents=True)
        (lib_net_dir / "_._").write_text("", encoding="utf-8")

        # Readme + nuspec + license + logo at package root.
        (stage / "README.md").write_text(render(templates / "package.readme.tmpl.md", mapping), encoding="utf-8")
        (stage / "package.nuspec").write_text(render(templates / "package.nuspec.tmpl", mapping), encoding="utf-8")
        shutil.copyfile(license_path, stage / "LICENSE.txt")
        shutil.copyfile(logo_path, stage / "logo.jpg")

        # Stub csproj for `dotnet pack`.
        shutil.copyfile(templates / "pack.csproj.tmpl", stage / "pack.csproj")

        out_dir.mkdir(parents=True, exist_ok=True)
        cmd = [
            "dotnet", "pack", str(stage / "pack.csproj"),
            "--output", str(out_dir),
            "--configuration", "Release",
            "/p:NuspecFile=package.nuspec",
            "/p:NuspecBasePath=.",
        ]
        print(f"  running: {' '.join(cmd)}", flush=True)
        subprocess.run(cmd, check=True, cwd=stage)

        nupkg_name = f"{nuget_id}.{version}.nupkg"
        nupkg_path = out_dir / nupkg_name
        if not nupkg_path.exists():
            sys.exit(f"expected {nupkg_path} not produced")
        return nupkg_path


def main() -> int:
    pkg_id      = env("PKG_ID")
    version     = env("PKG_VERSION")
    archive_url = env("ARCHIVE_URL")
    sha256_url  = env("SHA256_URL")
    rid         = env("RID")
    kind        = env("KIND")
    repo_root   = Path(env("REPO_ROOT", os.getcwd())).resolve()
    out_dir     = Path(env("OUT_DIR", str(repo_root / "out"))).resolve()
    authors     = env("AUTHORS", "Guojin Yan")

    print(f"=== Building OpenVINO.runtime.{pkg_id} v{version} ===", flush=True)
    print(f"  archive: {archive_url}", flush=True)
    print(f"  RID:     {rid}", flush=True)
    print(f"  output:  {out_dir}", flush=True)

    nupkg = build_package(
        pkg_id=pkg_id,
        version=version,
        archive_url=archive_url,
        sha256_url=sha256_url,
        rid=rid,
        kind=kind,
        repo_root=repo_root,
        out_dir=out_dir,
        authors=authors,
    )
    print(f"  produced: {nupkg}", flush=True)
    return 0


if __name__ == "__main__":
    sys.exit(main())
