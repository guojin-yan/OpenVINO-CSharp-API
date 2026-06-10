#!/usr/bin/env python3
"""Discover official OpenVINO GenAI archive packages for runtime NuGet packaging.

Outputs (to $GITHUB_OUTPUT when present, otherwise stdout):
  version -- NuGet package version, e.g. "2026.2.0"
  matrix  -- JSON: {"include": [{id, archive_url, sha256_url, rid, kind}, ...]}
  skip    -- "true" if no package work is needed
  reason  -- human-readable explanation when skipping
"""

from __future__ import annotations

import json
import os
import re
import sys
import urllib.error
import urllib.request
from typing import Any

CDN_ROOT = "https://storage.openvinotoolkit.org"
FILETREE_URL = f"{CDN_ROOT}/filetree.json"
PACKAGES_PATH = ("repositories", "openvino_genai", "packages")
GH_RELEASES_API = "https://api.github.com/repos/openvinotoolkit/openvino.genai/releases?per_page=100"
LOCAL_TAG_PREFIX = "openvino-genai-runtime-v"

# Each entry produces one NuGet package:
# JYPPX.OpenVINO.GenAI.runtime.<id>
#
# `archive` is matched against filetree.json when the file tree lists the
# archive. Some OpenVINO GenAI 2026.2 files are published on storage but are
# missing from filetree.json, so `direct` contains deterministic filename
# templates that are verified by probing both the archive URL and its .sha256
# sibling.
#
# 每一项都会生成一个 GenAI runtime NuGet 包。包 ID 和基础 OpenVINO runtime
# 保持同样的平台命名习惯，只是包名前缀改为 JYPPX.OpenVINO.GenAI.runtime。
# `archive` 优先匹配 filetree.json；如果官方 storage 已发布但 filetree.json
# 漏列，则用 `direct` 中的确定性文件名模板探测 archive 和 .sha256。
PLATFORMS: list[dict[str, str]] = [
    {"id": "win", "os_dir": "windows", "archive": r"^openvino_genai_windows_{archive_ver}_x86_64\.zip$", "direct": "openvino_genai_windows_{archive_ver}_x86_64.zip", "rid": "win-x64", "kind": "zip"},
    {"id": "ubuntu.24-x86_64", "os_dir": "linux", "archive": r"^openvino_genai_ubuntu24_{archive_ver}_x86_64\.tar\.gz$", "direct": "openvino_genai_ubuntu24_{archive_ver}_x86_64.tar.gz", "rid": "linux-x64", "kind": "tgz"},
    {"id": "ubuntu.22-x86_64", "os_dir": "linux", "archive": r"^openvino_genai_ubuntu22_{archive_ver}_x86_64\.tar\.gz$", "direct": "openvino_genai_ubuntu22_{archive_ver}_x86_64.tar.gz", "rid": "linux-x64", "kind": "tgz"},
    {"id": "ubuntu.22-arm64", "os_dir": "linux", "archive": r"^openvino_genai_ubuntu22_{archive_ver}_arm64\.tar\.gz$", "direct": "openvino_genai_ubuntu22_{archive_ver}_arm64.tar.gz", "rid": "linux-arm64", "kind": "tgz"},
    {"id": "ubuntu.20-x86_64", "os_dir": "linux", "archive": r"^openvino_genai_ubuntu20_{archive_ver}_x86_64\.tar\.gz$", "direct": "openvino_genai_ubuntu20_{archive_ver}_x86_64.tar.gz", "rid": "linux-x64", "kind": "tgz"},
    {"id": "ubuntu.20-arm64", "os_dir": "linux", "archive": r"^openvino_genai_ubuntu20_{archive_ver}_arm64\.tar\.gz$", "direct": "openvino_genai_ubuntu20_{archive_ver}_arm64.tar.gz", "rid": "linux-arm64", "kind": "tgz"},
    {"id": "rhel8-x86_64", "os_dir": "linux", "archive": r"^openvino_genai_rhel8_{archive_ver}_x86_64\.tar\.gz$", "direct": "openvino_genai_rhel8_{archive_ver}_x86_64.tar.gz", "rid": "linux-x64", "kind": "tgz"},
    {"id": "macos-x86_64", "os_dir": "macos", "archive": r"^openvino_genai_macos_\d+_\d+_{archive_ver}_x86_64\.tar\.gz$", "direct": "openvino_genai_macos_12_6_{archive_ver}_x86_64.tar.gz", "rid": "osx-x64", "kind": "tgz"},
    {"id": "macos-arm64", "os_dir": "macos", "archive": r"^openvino_genai_macos_\d+_\d+_{archive_ver}_arm64\.tar\.gz$", "direct": "openvino_genai_macos_12_6_{archive_ver}_arm64.tar.gz", "rid": "osx-arm64", "kind": "tgz"},
]

# A complete GenAI release should have all major OS directories. Individual
# platform archives can still be absent and will be skipped.
#
# 一个完整的 GenAI 版本至少应同时包含 windows/linux/macos 目录。某个具体平台
# archive 缺失时只跳过该平台，不影响其他平台打包。
CORE_PRESENCE_OS_DIRS = {"windows", "linux", "macos"}

VERSION_DIR_RE = re.compile(r"^(\d+)\.(\d+)(?:\.(\d+))?(?:\.(\d+))?$")


def http_get(url: str) -> bytes:
    req = urllib.request.Request(url, headers={"User-Agent": "openvino-csharp-genai-runtime-bot"})
    token = os.environ.get("GITHUB_TOKEN")
    if token and url.startswith("https://api.github.com/"):
        req.add_header("Authorization", f"Bearer {token}")
        req.add_header("Accept", "application/vnd.github+json")
    with urllib.request.urlopen(req, timeout=60) as resp:
        return resp.read()


def http_url_exists(url: str) -> bool:
    """Return True if the official CDN URL exists.

    HEAD is used first because these archives are large. If a CDN endpoint does
    not allow HEAD, retry with a tiny GET range.
    """
    headers = {"User-Agent": "openvino-csharp-genai-runtime-bot"}
    try:
        req = urllib.request.Request(url, headers=headers, method="HEAD")
        with urllib.request.urlopen(req, timeout=30) as resp:
            return 200 <= resp.status < 400
    except urllib.error.HTTPError as e:
        if e.code == 405:
            try:
                req = urllib.request.Request(url, headers={**headers, "Range": "bytes=0-0"})
                with urllib.request.urlopen(req, timeout=30) as resp:
                    return 200 <= resp.status < 400
            except urllib.error.HTTPError:
                return False
            except urllib.error.URLError:
                return False
        return False
    except urllib.error.URLError:
        return False


def normalize_version(name: str) -> str | None:
    match = VERSION_DIR_RE.match(name)
    if not match:
        return None
    parts = [match.group(1), match.group(2), match.group(3) or "0"]
    return ".".join(parts)


def archive_version(version: str) -> str:
    """Convert a 3-component NuGet version to GenAI's 4-component file version."""
    return f"{version}.0"


def version_key(version: str) -> tuple[int, int, int]:
    major, minor, patch = (int(p) for p in version.split("."))
    return major, minor, patch


def find_node(tree: dict[str, Any], path: tuple[str, ...]) -> dict[str, Any] | None:
    node = tree
    for segment in path:
        children = node.get("children") or []
        match = next((c for c in children if c.get("name") == segment), None)
        if match is None:
            return None
        node = match
    return node


def fetch_filetree() -> dict[str, Any]:
    print(f"fetching {FILETREE_URL}", file=sys.stderr)
    return json.loads(http_get(FILETREE_URL))


def fetch_official_release_tags() -> set[str]:
    print("fetching openvinotoolkit/openvino.genai release tags", file=sys.stderr)
    raw = http_get(GH_RELEASES_API)
    releases = json.loads(raw)
    tags: set[str] = set()
    for release in releases:
        if release.get("draft") or release.get("prerelease"):
            continue
        tag = release.get("tag_name")
        if isinstance(tag, str):
            normalized = normalize_version(tag.lstrip("vV"))
            if normalized:
                tags.add(normalized)
    print(f"  found {len(tags)} non-prerelease release tags", file=sys.stderr)
    return tags


def local_tag_exists(repo: str, tag: str) -> bool:
    url = f"https://api.github.com/repos/{repo}/git/refs/tags/{tag}"
    req = urllib.request.Request(url, headers={"User-Agent": "openvino-csharp-genai-runtime-bot"})
    token = os.environ.get("GITHUB_TOKEN")
    if token:
        req.add_header("Authorization", f"Bearer {token}")
        req.add_header("Accept", "application/vnd.github+json")
    try:
        with urllib.request.urlopen(req, timeout=30) as resp:
            return resp.status == 200
    except urllib.error.HTTPError as e:
        if e.code == 404:
            return False
        raise


def list_stable_versions(packages_node: dict[str, Any]) -> list[tuple[str, str, dict[str, Any]]]:
    versions: list[tuple[str, str, dict[str, Any]]] = []
    for child in packages_node.get("children") or []:
        if child.get("type") != "directory":
            continue
        name = child.get("name", "")
        if name in {"nightly", "latest", "master", "pre-release"}:
            continue
        normalized = normalize_version(name)
        if normalized is None:
            continue
        versions.append((normalized, name, child))
    versions.sort(key=lambda item: version_key(item[0]), reverse=True)
    return versions


def os_dirs_present(version_node: dict[str, Any]) -> set[str]:
    return {
        c.get("name") for c in (version_node.get("children") or [])
        if c.get("type") == "directory" and c.get("name") in CORE_PRESENCE_OS_DIRS
    }


def collect_archives(version_node: dict[str, Any], version: str, dir_name: str) -> list[dict[str, str]]:
    by_os: dict[str, list[str]] = {}
    for os_child in version_node.get("children") or []:
        if os_child.get("type") != "directory":
            continue
        name = os_child.get("name")
        files = [
            c.get("name") for c in (os_child.get("children") or [])
            if c.get("type") == "file" and isinstance(c.get("name"), str)
        ]
        by_os[name] = files

    items: list[dict[str, str]] = []
    archive_ver = re.escape(archive_version(version))
    archive_ver_text = archive_version(version)
    for platform in PLATFORMS:
        files = by_os.get(platform["os_dir"], [])
        pattern = re.compile(platform["archive"].format(archive_ver=archive_ver))
        match = next((f for f in files if pattern.match(f)), None)
        if match is not None and f"{match}.sha256" not in files:
            print(f"  skipping {platform['id']}: no .sha256 sibling for {match}", file=sys.stderr)
            continue

        if match is None:
            direct_template = platform.get("direct")
            direct_match = direct_template.format(archive_ver=archive_ver_text) if direct_template else ""
            archive_url = f"{CDN_ROOT}/repositories/openvino_genai/packages/{dir_name}/{platform['os_dir']}/{direct_match}"
            if not direct_match or not http_url_exists(archive_url) or not http_url_exists(f"{archive_url}.sha256"):
                print(f"  skipping {platform['id']}: archive not found", file=sys.stderr)
                continue
            match = direct_match
            print(f"  using direct CDN probe for {platform['id']}: {match}", file=sys.stderr)
        else:
            archive_url = f"{CDN_ROOT}/repositories/openvino_genai/packages/{dir_name}/{platform['os_dir']}/{match}"

        items.append({
            "id": platform["id"],
            "archive_url": archive_url,
            "sha256_url": f"{archive_url}.sha256",
            "rid": platform["rid"],
            "kind": platform["kind"],
        })
    return items


def write_output(key: str, value: str) -> None:
    output_path = os.environ.get("GITHUB_OUTPUT")
    if output_path:
        with open(output_path, "a", encoding="utf-8") as fh:
            fh.write(f"{key}<<__EOF__\n{value}\n__EOF__\n")
    else:
        print(f"{key}={value}")


def emit_skip(reason: str, version: str = "") -> None:
    print(f"SKIP: {reason}", file=sys.stderr)
    write_output("skip", "true")
    write_output("reason", reason)
    write_output("version", version)
    write_output("matrix", json.dumps({"include": []}))


def main() -> int:
    requested_raw = (os.environ.get("REQUESTED_VERSION") or "").strip()
    requested = normalize_version(requested_raw.lstrip("vV")) if requested_raw else None
    force_republish = (os.environ.get("FORCE_REPUBLISH") or "").lower() == "true"
    repo = os.environ.get("GITHUB_REPOSITORY", "")

    tree = fetch_filetree()
    packages_node = find_node(tree, PACKAGES_PATH)
    if packages_node is None:
        emit_skip("openvino_genai packages node not found")
        return 0

    official_tags = fetch_official_release_tags()
    versions = list_stable_versions(packages_node)
    if requested:
        versions = [v for v in versions if v[0] == requested]
        if not versions:
            sys.exit(f"requested OpenVINO GenAI version {requested_raw} was not found on the CDN")

    selected: tuple[str, str, dict[str, Any], list[dict[str, str]]] | None = None
    for version, dir_name, node in versions:
        if official_tags and version not in official_tags:
            print(f"  skipping {version}: not found in official openvino.genai releases", file=sys.stderr)
            continue
        present = os_dirs_present(node)
        if not CORE_PRESENCE_OS_DIRS.issubset(present):
            missing = sorted(CORE_PRESENCE_OS_DIRS - present)
            print(f"  skipping {version}: incomplete release, missing {missing}", file=sys.stderr)
            continue
        archives = collect_archives(node, version, dir_name)
        if not archives:
            print(f"  skipping {version}: no packageable archives", file=sys.stderr)
            continue
        selected = (version, dir_name, node, archives)
        break

    if selected is None:
        if requested:
            sys.exit(f"requested OpenVINO GenAI version {requested_raw} has no packageable archives")
        emit_skip("no packageable OpenVINO GenAI release found")
        return 0

    version, _dir_name, _node, archives = selected
    tag = f"{LOCAL_TAG_PREFIX}{version}"
    if repo and not force_republish and local_tag_exists(repo, tag):
        emit_skip(f"{tag} already exists", version=version)
        return 0

    print(f"chosen version: {version}", file=sys.stderr)
    print(f"matrix items ({len(archives)}):", file=sys.stderr)
    for item in archives:
        print(f"  - {item['id']} <- {item['archive_url']}", file=sys.stderr)

    write_output("skip", "false")
    write_output("reason", "")
    write_output("version", version)
    write_output("matrix", json.dumps({"include": archives}, separators=(",", ":")))
    return 0


if __name__ == "__main__":
    sys.exit(main())
