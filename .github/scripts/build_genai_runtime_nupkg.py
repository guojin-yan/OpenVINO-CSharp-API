#!/usr/bin/env python3
"""Download, verify, extract, and pack an OpenVINO GenAI runtime NuGet package.

This intentionally mirrors build_runtime_nupkg.py. The only functional
difference is the NuGet ID prefix and the official OpenVINO GenAI archive
source.

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
import datetime as _datetime
import hashlib
import http.client
import os
import re
import shutil
import sys
import tarfile
import tempfile
import time
import urllib.error
import urllib.request
import uuid
import zipfile
from pathlib import Path
from xml.sax.saxutils import escape as xml_escape

NUGET_ID_PREFIX = "JYPPX.OpenVINO.GenAI.runtime."
DEFAULT_RID = "win-x64"

# Match the platform labels used by the core OpenVINO runtime packaging flow.
# 平台显示名称与基础 OpenVINO runtime 打包流程保持一致，便于用户理解和检索。
PLATFORM_LABELS = {
    "win": "Windows (x86_64)",
    "ubuntu.24-x86_64": "Ubuntu 24.04 (x86_64)",
    "ubuntu.22-x86_64": "Ubuntu 22.04 (x86_64)",
    "ubuntu.22-arm64": "Ubuntu 22.04 (arm64)",
    "ubuntu.20-x86_64": "Ubuntu 20.04 (x86_64)",
    "ubuntu.20-arm64": "Ubuntu 20.04 (arm64)",
    "rhel8-x86_64": "RHEL 8 (x86_64)",
    "macos-x86_64": "macOS (x86_64)",
    "macos-arm64": "macOS (arm64)",
}

EXCLUDE_DIR_PARTS = {
    "python",
    "samples",
    "tools",
    "docs",
    "share",
    "include",
    "cmake",
    "pyopenvino",
    "tests",
    "test",
    "debug",
}

NATIVE_PATTERNS = re.compile(r"\.(dll|so|dylib)(\.\d+)*$", re.IGNORECASE)


def truthy(value: str | None) -> bool:
    return value is not None and value.strip().lower() in {"1", "true", "yes", "on"}


def download(url: str, dest: Path) -> None:
    print(f"  GET  {url}", flush=True)
    partial = dest.with_name(dest.name + ".part")
    for attempt in range(1, 4):
        req = urllib.request.Request(url, headers={"User-Agent": "openvino-csharp-genai-runtime-bot"})
        try:
            with urllib.request.urlopen(req, timeout=300) as resp, open(partial, "wb") as fh:
                shutil.copyfileobj(resp, fh, length=1 << 20)
            os.replace(partial, dest)
            return
        except (urllib.error.URLError, TimeoutError, http.client.IncompleteRead, OSError):
            if partial.exists():
                partial.unlink()
            if attempt == 3:
                raise
            delay = 2 ** (attempt - 1)
            print(f"  transient download failure; retrying in {delay}s", file=sys.stderr, flush=True)
            time.sleep(delay)


def sha256_of(path: Path) -> str:
    h = hashlib.sha256()
    with open(path, "rb") as fh:
        for chunk in iter(lambda: fh.read(1 << 20), b""):
            h.update(chunk)
    return h.hexdigest()


def parse_expected_sha256(sha_text: str, archive_name: str) -> str:
    """Parse sha256sum text: '<hex> *<name>', '<hex>  <name>', or '<hex>'."""
    if "<html" in sha_text.lower() or "<!doctype html" in sha_text.lower():
        sys.exit(
            f"sha256 sidecar for {archive_name} is not a checksum file; "
            "the server returned an HTML directory page instead"
        )

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


def is_native_file(path: Path) -> bool:
    name = path.name.lower()
    if ".cpython-" in name or ".python-" in name:
        return False
    if name.endswith(".pdb"):
        return False
    if "_debug" in name:
        return False
    return bool(NATIVE_PATTERNS.search(path.name))


def collect_native_files(root: Path) -> list[Path]:
    """Collect native runtime libraries using the same shape as the core package.

    按基础 OpenVINO runtime 包的方式递归收集原生动态库。只打包运行时库，
    不把 headers、docs、samples、Python 扩展或 debug 库放进 NuGet。
    """
    files: list[Path] = []
    for path in root.rglob("*"):
        if not path.is_file() and not path.is_symlink():
            continue
        rel_parts = {part.lower() for part in path.relative_to(root).parts}
        if rel_parts & EXCLUDE_DIR_PARTS:
            continue
        if is_native_file(path):
            files.append(path)
    return files


def stage_files(files: list[Path], target_dir: Path) -> list[str]:
    target_dir.mkdir(parents=True, exist_ok=True)
    staged: dict[str, Path] = {}
    for src in files:
        name = src.name
        if name in staged:
            print(f"  WARN: duplicate basename {name} (keeping first)", file=sys.stderr)
            continue
        dst = target_dir / name
        shutil.copyfile(str(src), str(dst), follow_symlinks=True)
        try:
            shutil.copystat(str(src), str(dst), follow_symlinks=True)
        except OSError:
            pass
        staged[name] = dst
    return sorted(staged.keys(), key=str.lower)


def copy_notice_file(source: Path, target: Path) -> None:
    target.parent.mkdir(parents=True, exist_ok=True)
    shutil.copyfile(str(source), str(target), follow_symlinks=True)
    try:
        shutil.copystat(str(source), str(target), follow_symlinks=True)
    except OSError:
        pass


def stage_notice_files(runtime_root: Path, target_dir: Path) -> int:
    """Stage upstream license, notice, and version files under licenses/.

    将上游 runtime 中的许可证、第三方声明和版本文件复制到 licenses/，
    让 NuGet 包保留清晰的官方来源和许可信息。
    """
    count = 0
    notice_dirs = [
        "docs/licensing",
        "docs/openvino_tokenizers",
    ]
    notice_files = [
        "runtime/version.txt",
        "runtime/3rdparty/tbb/TBB-LICENSE",
    ]

    for rel_dir in notice_dirs:
        source_dir = runtime_root.joinpath(*rel_dir.split("/"))
        if not source_dir.exists() or not source_dir.is_dir():
            continue

        for source in source_dir.rglob("*"):
            if not source.is_file():
                continue
            target = target_dir.joinpath(*rel_dir.split("/")) / source.relative_to(source_dir)
            copy_notice_file(source, target)
            count += 1

    for rel_file in notice_files:
        source = runtime_root.joinpath(*rel_file.split("/"))
        if not source.exists() or not source.is_file():
            continue

        target = target_dir.joinpath(*rel_file.split("/"))
        copy_notice_file(source, target)
        count += 1

    return count


def render(template_path: Path, mapping: dict[str, str]) -> str:
    text = template_path.read_text(encoding="utf-8")
    for key, value in mapping.items():
        text = text.replace("{" + key + "}", value)
    return text


def require_file(path: Path, label: str) -> None:
    if not path.exists() or not path.is_file():
        sys.exit(f"missing {label}: {path}")


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


def normalized_package_file(out_dir: Path, nuget_id: str, version: str) -> Path:
    exact = out_dir / f"{nuget_id}.{version}.nupkg"
    if exact.exists():
        return exact

    candidates = sorted(out_dir.glob(f"{nuget_id}.*.nupkg"), key=lambda p: p.stat().st_mtime, reverse=True)
    if candidates:
        return candidates[0]

    sys.exit(f"expected package for {nuget_id} {version} was not produced in {out_dir}")


def strip_nuspec_files_section(nuspec_text: str) -> str:
    """Remove the transient <files> section before embedding the nuspec."""
    return re.sub(r"\s*<files>.*?</files>", "", nuspec_text, flags=re.DOTALL)


def content_type_for_extension(extension: str) -> str:
    if extension == ".rels":
        return "application/vnd.openxmlformats-package.relationships+xml"
    if extension == ".psmdcp":
        return "application/vnd.openxmlformats-package.core-properties+xml"
    return "application/octet"


def write_content_types(package_paths: list[str]) -> str:
    extensions = sorted({Path(path).suffix.lower() for path in package_paths if Path(path).suffix})
    no_extension_paths = sorted(path for path in package_paths if not Path(path).suffix)

    lines = [
        '<?xml version="1.0" encoding="utf-8"?>',
        '<Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">',
        '  <Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml" />',
        '  <Default Extension="psmdcp" ContentType="application/vnd.openxmlformats-package.core-properties+xml" />',
    ]
    for extension in extensions:
        ext = extension.lstrip(".")
        if ext in {"rels", "psmdcp"}:
            continue
        lines.append(f'  <Default Extension="{xml_escape(ext)}" ContentType="{content_type_for_extension(extension)}" />')
    for path in no_extension_paths:
        lines.append(f'  <Override PartName="/{xml_escape(path)}" ContentType="application/octet" />')
    lines.append("</Types>")
    return "\n".join(lines) + "\n"


def write_relationships(core_properties_path: str) -> str:
    return (
        '<?xml version="1.0" encoding="utf-8"?>\n'
        '<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">\n'
        '  <Relationship Type="http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties" '
        f'Target="/{xml_escape(core_properties_path)}" Id="R{uuid.uuid4().hex}" />\n'
        '</Relationships>\n'
    )


def write_core_properties(nuget_id: str, version: str, authors: str) -> str:
    now = _datetime.datetime.now(_datetime.timezone.utc).replace(microsecond=0).isoformat().replace("+00:00", "Z")
    return (
        '<?xml version="1.0" encoding="utf-8"?>\n'
        '<coreProperties xmlns:dc="http://purl.org/dc/elements/1.1/" '
        'xmlns:dcterms="http://purl.org/dc/terms/" '
        'xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" '
        'xmlns="http://schemas.openxmlformats.org/package/2006/metadata/core-properties">\n'
        f'  <dc:creator>{xml_escape(authors)}</dc:creator>\n'
        f'  <dc:description>{xml_escape(nuget_id)}</dc:description>\n'
        f'  <version>{xml_escape(version)}</version>\n'
        f'  <dcterms:created xsi:type="dcterms:W3CDTF">{now}</dcterms:created>\n'
        f'  <dcterms:modified xsi:type="dcterms:W3CDTF">{now}</dcterms:modified>\n'
        '</coreProperties>\n'
    )


def add_text(zip_file: zipfile.ZipFile, package_path: str, text: str) -> None:
    info = zipfile.ZipInfo(package_path.replace("\\", "/"))
    info.compress_type = zipfile.ZIP_DEFLATED
    zip_file.writestr(info, text.encode("utf-8"))


def add_file(zip_file: zipfile.ZipFile, source: Path, package_path: str) -> None:
    zip_file.write(source, package_path.replace("\\", "/"), compress_type=zipfile.ZIP_DEFLATED)


def pack_stage_as_nupkg(stage: Path, out_dir: Path, nuget_id: str, version: str, authors: str) -> Path:
    """Write a nupkg directly so runtimes/build/lib paths are preserved."""
    out_dir.mkdir(parents=True, exist_ok=True)
    package_path = out_dir / f"{nuget_id}.{version}.nupkg"
    if package_path.exists():
        package_path.unlink()

    stage_files = [
        path for path in stage.rglob("*")
        if path.is_file() and path.name not in {"package.nuspec", "pack.csproj"}
    ]
    package_entries = [path.relative_to(stage).as_posix() for path in stage_files]
    nuspec_entry = f"{nuget_id}.nuspec"
    core_properties_entry = f"package/services/metadata/core-properties/{uuid.uuid4().hex}.psmdcp"
    content_type_entries = sorted(package_entries + [nuspec_entry, "_rels/.rels", core_properties_entry])

    with zipfile.ZipFile(package_path, "w", compression=zipfile.ZIP_DEFLATED) as zf:
        add_text(zf, "[Content_Types].xml", write_content_types(content_type_entries))
        add_text(zf, "_rels/.rels", write_relationships(core_properties_entry))
        add_text(zf, core_properties_entry, write_core_properties(nuget_id, version, authors))
        add_text(
            zf,
            nuspec_entry,
            strip_nuspec_files_section((stage / "package.nuspec").read_text(encoding="utf-8")),
        )

        for path in sorted(stage_files, key=lambda p: p.relative_to(stage).as_posix()):
            add_file(zf, path, path.relative_to(stage).as_posix())

    return package_path


def write_manifest(
    stage: Path,
    nuget_id: str,
    version: str,
    rid: str,
    archive_url: str,
    sha256_url: str,
    archive_sha256: str,
    filenames: list[str],
) -> None:
    lines = [
        f"PackageId: {nuget_id}",
        f"Version: {version}",
        f"RID: {rid}",
        f"ArchiveUrl: {archive_url}",
        f"Sha256Url: {sha256_url}",
        f"ArchiveSha256: {archive_sha256}",
        "",
        "NativeFiles:",
    ]
    lines.extend(f"- runtimes/{rid}/native/{name}" for name in filenames)
    (stage / "manifest.txt").write_text("\n".join(lines) + "\n", encoding="utf-8")


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

        runtime_root = extract(archive_path, kind, workdir / "extracted")
        print(f"  runtime root: {runtime_root}", flush=True)

        native_files = collect_native_files(runtime_root)
        if not native_files:
            sys.exit(f"no native files found under {runtime_root}")
        print(f"  found {len(native_files)} native files", flush=True)

        stage = stage_dir.resolve() if stage_dir else workdir / "stage"
        if stage.exists():
            shutil.rmtree(stage)

        runtime_dir = stage / "runtimes" / rid / "native"
        filenames = stage_files(native_files, runtime_dir)
        notice_count = stage_notice_files(runtime_root, stage / "licenses")

        mapping = {
            "NUGET_ID": nuget_id,
            "VERSION": version,
            "AUTHORS": authors,
            "PLATFORM_LABEL": platform_label,
            "ARCHIVE_URL": archive_url,
            "SHA256_URL": sha256_url,
            "RID": rid,
            "LINK_SUBDIR": pkg_id,
        }

        props_text = render(
            templates / "genai.package.props.tmpl",
            {**mapping, "CONTENT_ITEMS": render_content_items(filenames, rid, pkg_id)},
        )
        for tfm in ("net46", "netstandard2.0"):
            props_dir = stage / "build" / tfm
            props_dir.mkdir(parents=True, exist_ok=True)
            (props_dir / f"{nuget_id}.props").write_text(props_text, encoding="utf-8")

            # Runtime-only packages include empty lib placeholders so NuGet can
            # select compatible assets without falling back to legacy framework
            # compatibility. This avoids NU1701 in SDK-style consumers.
            # 运行时包没有托管程序集，用空 lib 占位明确兼容目标，避免 SDK 项目恢复时触发 NU1701。
            lib_dir = stage / "lib" / tfm
            lib_dir.mkdir(parents=True, exist_ok=True)
            (lib_dir / "_._").write_text("", encoding="utf-8")

        (stage / "README.md").write_text(render(templates / "genai.package.readme.tmpl.md", mapping), encoding="utf-8")
        (stage / "package.nuspec").write_text(render(templates / "genai.package.nuspec.tmpl", mapping), encoding="utf-8")
        write_manifest(stage, nuget_id, version, rid, archive_url, sha256_url, actual, filenames)
        shutil.copyfile(license_path, stage / "LICENSE.txt")
        shutil.copyfile(logo_path, stage / "logo.jpg")
        shutil.copyfile(templates / "pack.csproj.tmpl", stage / "pack.csproj")

        print(f"  staged native files: {len(filenames)}", flush=True)
        print(f"  staged notice files: {notice_count}", flush=True)
        print(f"  stage: {stage}", flush=True)

        if dry_run:
            return stage

        print("  packing nupkg with preserved runtime layout", flush=True)
        return pack_stage_as_nupkg(stage, out_dir, nuget_id, version, authors)


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
