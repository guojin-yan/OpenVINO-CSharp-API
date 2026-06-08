#!/usr/bin/env python3
"""Download, verify, extract, and pack an OpenVINO GenAI runtime NuGet package.

This script mirrors build_runtime_nupkg.py, but targets the OpenVINO GenAI
archive repository:

  https://storage.openvinotoolkit.org/repositories/openvino_genai/packages/

Inputs come from environment variables in GitHub Actions:
  PKG_ID          NuGet ID suffix, e.g. "win" -> JYPPX.OpenVINO.GenAI.runtime.win
  PKG_VERSION     NuGet package version, e.g. "2026.2.0"
  ARCHIVE_URL     Full URL to the .zip / .tar.gz archive
  SHA256_URL      Full URL to the .sha256 sibling
  RID             Standard .NET RID, e.g. "win-x64"
  KIND            "zip" or "tgz"
  REPO_ROOT       Path to the repository checkout (defaults to CWD)
  OUT_DIR         Where to drop the .nupkg (defaults to <REPO_ROOT>/out)
  AUTHORS         Optional package authors string, defaults to "Guojin Yan"

Output:
  A single .nupkg at $OUT_DIR/JYPPX.OpenVINO.GenAI.runtime.<PKG_ID>.<PKG_VERSION>.nupkg
"""

from __future__ import annotations

import argparse
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

NUGET_ID_PREFIX = "JYPPX.OpenVINO.GenAI.runtime."
DEFAULT_RID = "win-x64"
PLATFORM_LABELS = {
    "win": "Windows (x86_64)",
}

REQUIRED_RUNTIME_FILES = {
    "openvino.dll",
    "openvino_c.dll",
    "openvino_genai.dll",
    "openvino_genai_c.dll",
    "openvino_tokenizers.dll",
    "openvino_intel_cpu_plugin.dll",
    "tbb12.dll",
}


def env(name: str, default: str | None = None) -> str:
    val = os.environ.get(name, default)
    if val is None:
        sys.exit(f"missing required env var: {name}")
    return val


def truthy(value: str | None) -> bool:
    return value is not None and value.strip().lower() in {"1", "true", "yes", "on"}


def download(url: str, dest: Path) -> None:
    print(f"  GET  {url}", flush=True)
    req = urllib.request.Request(url, headers={"User-Agent": "openvino-csharp-genai-runtime-bot"})
    with urllib.request.urlopen(req, timeout=300) as resp, open(dest, "wb") as fh:
        shutil.copyfileobj(resp, fh, length=1 << 20)


def sha256_of(path: Path) -> str:
    h = hashlib.sha256()
    with open(path, "rb") as fh:
        for chunk in iter(lambda: fh.read(1 << 20), b""):
            h.update(chunk)
    return h.hexdigest()


def parse_expected_sha256(sha_text: str, archive_name: str) -> str:
    """Parse sha256sum text: '<hex> *<name>', '<hex>  <name>', or '<hex>'."""
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
    if kind == "zip":
        with zipfile.ZipFile(archive) as zf:
            zf.extractall(dest)
    elif kind == "tgz":
        with tarfile.open(archive, "r:gz") as tf:
            try:
                tf.extractall(dest, filter="data")
            except TypeError:
                tf.extractall(dest)
    else:
        sys.exit(f"unknown archive kind: {kind}")

    entries = [p for p in dest.iterdir() if p.is_dir()]
    if len(entries) == 1:
        return entries[0]
    return dest


def render(template_path: Path, mapping: dict[str, str]) -> str:
    text = template_path.read_text(encoding="utf-8")
    for key, value in mapping.items():
        text = text.replace("{" + key + "}", value)
    return text


def require_file(path: Path, label: str) -> None:
    if not path.exists() or not path.is_file():
        sys.exit(f"missing {label}: {path}")


def require_directory(path: Path, label: str) -> None:
    if not path.exists() or not path.is_dir():
        sys.exit(f"missing {label}: {path}")


def find_runtime_root(extracted_root: Path) -> Path:
    """Find the extracted OpenVINO GenAI runtime root.

    查找解压后的 OpenVINO GenAI runtime 根目录。官方归档通常只有一个顶层目录，
    但这里仍递归检查，便于兼容未来包结构。
    """
    candidates = [extracted_root]
    candidates.extend([p for p in extracted_root.rglob("*") if p.is_dir() and "openvino_genai" in p.name.lower()])
    for candidate in candidates:
        release_dir = candidate / "runtime" / "bin" / "intel64" / "Release"
        if (release_dir / "openvino_genai_c.dll").exists():
            return candidate
    sys.exit(f"could not locate OpenVINO GenAI runtime root under {extracted_root}")


def collect_runtime_files(runtime_root: Path) -> list[Path]:
    """Collect native runtime files for runtimes/<rid>/native.

    收集进入 runtimes/<rid>/native 的运行时文件。仅包含 Release 运行时、
    GenAI/tokenizers/frontends/plugins、cache.json 和 TBB release DLL。
    Debug DLLs are deliberately excluded to avoid debug/release ABI mixing.
    """
    release_dir = runtime_root / "runtime" / "bin" / "intel64" / "Release"
    tbb_dir = runtime_root / "runtime" / "3rdparty" / "tbb" / "bin"
    require_directory(release_dir, "OpenVINO Release binary directory")
    require_directory(tbb_dir, "TBB binary directory")

    files: list[Path] = []
    files.extend(sorted(release_dir.glob("*.dll")))

    cache_json = release_dir / "cache.json"
    if cache_json.exists():
        files.append(cache_json)

    for path in sorted(tbb_dir.glob("*.dll")):
        if "_debug" in path.name.lower():
            continue
        files.append(path)

    by_name: dict[str, Path] = {}
    for path in files:
        if path.name in by_name:
            sys.exit(f"duplicate runtime file basename '{path.name}': {by_name[path.name]} and {path}")
        by_name[path.name] = path

    missing = sorted(REQUIRED_RUNTIME_FILES - set(by_name))
    if missing:
        sys.exit("missing required GenAI runtime files: " + ", ".join(missing))

    return sorted(by_name.values(), key=lambda p: p.name.lower())


def collect_license_files(runtime_root: Path) -> list[Path]:
    """Collect upstream license and notice files.

    收集上游许可和 third-party notice 文件，保留在 nupkg 的 licenses/ 目录。
    """
    roots = [
        runtime_root / "docs" / "licensing",
        runtime_root / "docs" / "openvino_tokenizers",
    ]
    files: list[Path] = []
    for root in roots:
        if root.exists():
            files.extend([p for p in root.rglob("*") if p.is_file()])

    tbb_license = runtime_root / "runtime" / "3rdparty" / "tbb" / "TBB-LICENSE"
    if tbb_license.exists():
        files.append(tbb_license)

    version_file = runtime_root / "runtime" / "version.txt"
    if version_file.exists():
        files.append(version_file)

    return sorted(set(files), key=lambda p: str(p).lower())


def copy_runtime_files(files: list[Path], target_dir: Path) -> list[str]:
    target_dir.mkdir(parents=True, exist_ok=True)
    names: list[str] = []
    for src in files:
        dst = target_dir / src.name
        shutil.copy2(src, dst)
        names.append(src.name)
    return sorted(names, key=str.lower)


def copy_license_files(files: list[Path], runtime_root: Path, target_dir: Path) -> list[str]:
    copied: list[str] = []
    for src in files:
        rel = src.relative_to(runtime_root)
        dst = target_dir / rel
        dst.parent.mkdir(parents=True, exist_ok=True)
        shutil.copy2(src, dst)
        copied.append(str(rel).replace("\\", "/"))
    return sorted(copied, key=str.lower)


def render_content_items(filenames: list[str], rid: str, link_subdir: str) -> str:
    indent = "    "
    lines: list[str] = []
    for name in filenames:
        lines.extend([
            f'{indent}<Content Include="$(OpenVINOGenAIRuntime)\\{rid}\\native\\{name}">',
            f'{indent}  <Link>dll\\{link_subdir}\\{name}</Link>',
            f'{indent}  <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>',
            f'{indent}</Content>',
        ])
    return "\n".join(lines)


def write_manifest(stage: Path, archive_url: str, runtime_names: list[str], license_names: list[str]) -> None:
    lines = [
        "OpenVINO GenAI runtime package manifest",
        "",
        f"Source archive: {archive_url}",
        "",
        "Runtime files:",
        *[f"  runtimes/*/native/{name}" for name in runtime_names],
        "",
        "License and notice files:",
        *[f"  licenses/{name}" for name in license_names],
        "",
    ]
    (stage / "manifest.txt").write_text("\n".join(lines), encoding="utf-8")


def normalized_package_file(out_dir: Path, nuget_id: str, version: str) -> Path:
    exact = out_dir / f"{nuget_id}.{version}.nupkg"
    if exact.exists():
        return exact

    candidates = sorted(out_dir.glob(f"{nuget_id}.*.nupkg"), key=lambda p: p.stat().st_mtime, reverse=True)
    if candidates:
        return candidates[0]

    sys.exit(f"expected package for {nuget_id} {version} was not produced in {out_dir}")


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
    dry_run: bool,
    stage_dir: Path | None,
) -> Path:
    nuget_id = NUGET_ID_PREFIX + pkg_id
    platform_label = PLATFORM_LABELS.get(pkg_id, pkg_id)
    templates = repo_root / "nuget" / "runtime" / "templates"
    license_path = repo_root / "LICENSE.txt"
    logo_path = repo_root / "nuget" / "logo.jpg"

    require_file(templates / "genai.package.nuspec.tmpl", "GenAI nuspec template")
    require_file(templates / "genai.package.props.tmpl", "GenAI props template")
    require_file(templates / "genai.package.readme.tmpl.md", "GenAI readme template")
    require_file(templates / "pack.csproj.tmpl", "pack csproj template")
    require_file(license_path, "repository license")
    require_file(logo_path, "NuGet logo")

    with tempfile.TemporaryDirectory(prefix="ov-genai-pkg-") as workdir_str:
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

        extracted = extract(archive_path, kind, workdir / "extracted")
        runtime_root = find_runtime_root(extracted)
        print(f"  runtime root: {runtime_root}", flush=True)

        runtime_files = collect_runtime_files(runtime_root)
        license_files = collect_license_files(runtime_root)
        if not license_files:
            sys.exit(f"no license or notice files found under {runtime_root}")

        stage = stage_dir.resolve() if stage_dir else workdir / "stage"
        if stage.exists():
            shutil.rmtree(stage)

        runtime_dir = stage / "runtimes" / rid / "native"
        runtime_names = copy_runtime_files(runtime_files, runtime_dir)
        license_names = copy_license_files(license_files, runtime_root, stage / "licenses")

        mapping = {
            "NUGET_ID": nuget_id,
            "VERSION": version,
            "AUTHORS": authors,
            "PLATFORM_LABEL": platform_label,
            "ARCHIVE_URL": archive_url,
            "SHA256_URL": sha256_url,
            "RID": rid,
            "LINK_SUBDIR": "win-x64",
        }

        props_text = render(
            templates / "genai.package.props.tmpl",
            {**mapping, "CONTENT_ITEMS": render_content_items(runtime_names, rid, "win-x64")},
        )
        # Keep the same build asset layout as the core runtime package.
        # 与基础 runtime 包保持一致，使用 build/net 放置 props。
        props_dir = stage / "build" / "net"
        props_dir.mkdir(parents=True, exist_ok=True)
        (props_dir / f"{nuget_id}.props").write_text(props_text, encoding="utf-8")

        (stage / "README.md").write_text(render(templates / "genai.package.readme.tmpl.md", mapping), encoding="utf-8")
        (stage / "package.nuspec").write_text(render(templates / "genai.package.nuspec.tmpl", mapping), encoding="utf-8")
        shutil.copyfile(license_path, stage / "LICENSE.txt")
        shutil.copyfile(logo_path, stage / "logo.jpg")
        shutil.copyfile(templates / "pack.csproj.tmpl", stage / "pack.csproj")
        write_manifest(stage, archive_url, runtime_names, license_names)

        print(f"  staged runtime files: {len(runtime_names)}", flush=True)
        print(f"  staged license files: {len(license_names)}", flush=True)
        print(f"  stage: {stage}", flush=True)

        if dry_run:
            return stage

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

        return normalized_package_file(out_dir, nuget_id, version)


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(description="Build OpenVINO GenAI runtime NuGet package from an official archive.")
    parser.add_argument("--pkg-id", default=os.environ.get("PKG_ID", "win"))
    parser.add_argument("--version", default=os.environ.get("PKG_VERSION"))
    parser.add_argument("--archive-url", default=os.environ.get("ARCHIVE_URL"))
    parser.add_argument("--sha256-url", default=os.environ.get("SHA256_URL"))
    parser.add_argument("--rid", default=os.environ.get("RID", DEFAULT_RID))
    parser.add_argument("--kind", default=os.environ.get("KIND", "zip"))
    parser.add_argument("--repo-root", default=os.environ.get("REPO_ROOT", os.getcwd()))
    parser.add_argument("--out-dir", default=os.environ.get("OUT_DIR"))
    parser.add_argument("--authors", default=os.environ.get("AUTHORS", "Guojin Yan"))
    parser.add_argument("--dry-run", action="store_true", default=truthy(os.environ.get("DRY_RUN")))
    parser.add_argument("--stage-dir", default=os.environ.get("STAGE_DIR"))
    return parser.parse_args()


def main() -> int:
    args = parse_args()
    if not args.version:
        sys.exit("missing required PKG_VERSION or --version")
    if not args.archive_url:
        sys.exit("missing required ARCHIVE_URL or --archive-url")
    if not args.sha256_url:
        sys.exit("missing required SHA256_URL or --sha256-url")

    repo_root = Path(args.repo_root).resolve()
    out_dir = Path(args.out_dir).resolve() if args.out_dir else repo_root / "out"
    stage_dir = Path(args.stage_dir).resolve() if args.stage_dir else None
    if args.dry_run and stage_dir is None:
        stage_dir = out_dir / "genai-runtime-stage"

    print(f"=== Building {NUGET_ID_PREFIX}{args.pkg_id} v{args.version} ===", flush=True)
    print(f"  archive: {args.archive_url}", flush=True)
    print(f"  RID:     {args.rid}", flush=True)
    print(f"  output:  {out_dir}", flush=True)
    print(f"  dry run: {args.dry_run}", flush=True)

    result = build_package(
        pkg_id=args.pkg_id,
        version=args.version,
        archive_url=args.archive_url,
        sha256_url=args.sha256_url,
        rid=args.rid,
        kind=args.kind,
        repo_root=repo_root,
        out_dir=out_dir,
        authors=args.authors,
        dry_run=args.dry_run,
        stage_dir=stage_dir,
    )
    print(f"  produced: {result}", flush=True)
    return 0


if __name__ == "__main__":
    sys.exit(main())
