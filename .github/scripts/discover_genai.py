#!/usr/bin/env python3
"""Discover OpenVINO GenAI archive packages for runtime NuGet packaging.

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

VERSION_DIR_RE = re.compile(r"^(\d+)\.(\d+)(?:\.(\d+))?(?:\.(\d+))?$")
WINDOWS_ARCHIVE_RE = re.compile(r"^openvino_genai_windows_(?P<version>[\d.]+)_x86_64\.zip$")


def http_get(url: str) -> bytes:
    req = urllib.request.Request(url, headers={"User-Agent": "openvino-csharp-genai-runtime-bot"})
    token = os.environ.get("GITHUB_TOKEN")
    if token and url.startswith("https://api.github.com/"):
        req.add_header("Authorization", f"Bearer {token}")
        req.add_header("Accept", "application/vnd.github+json")
    with urllib.request.urlopen(req, timeout=60) as resp:
        return resp.read()


def normalize_version(name: str) -> str | None:
    match = VERSION_DIR_RE.match(name)
    if not match:
        return None
    parts = [match.group(1), match.group(2), match.group(3) or "0"]
    return ".".join(parts)


def version_key(version: str) -> tuple[int, int, int]:
    parts = [int(p) for p in version.split(".")]
    return (parts[0], parts[1], parts[2])


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
    return json.loads(http_get(FILETREE_URL))


def fetch_official_release_tags() -> set[str]:
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


def find_windows_archive(version_node: dict[str, Any], dir_name: str) -> dict[str, str] | None:
    windows_node = next(
        (c for c in (version_node.get("children") or []) if c.get("type") == "directory" and c.get("name") == "windows"),
        None,
    )
    if windows_node is None:
        return None

    files = [
        c.get("name") for c in (windows_node.get("children") or [])
        if c.get("type") == "file" and isinstance(c.get("name"), str)
    ]
    archive = next((f for f in files if WINDOWS_ARCHIVE_RE.match(f)), None)
    if archive is None or f"{archive}.sha256" not in files:
        return None

    archive_url = f"{CDN_ROOT}/repositories/openvino_genai/packages/{dir_name}/windows/{archive}"
    return {
        "id": "win",
        "archive_url": archive_url,
        "sha256_url": f"{archive_url}.sha256",
        "rid": "win-x64",
        "kind": "zip",
    }


def emit(outputs: dict[str, str]) -> None:
    output_path = os.environ.get("GITHUB_OUTPUT")
    if output_path:
        with open(output_path, "a", encoding="utf-8") as fh:
            for key, value in outputs.items():
                fh.write(f"{key}={value}\n")
    else:
        for key, value in outputs.items():
            print(f"{key}={value}")


def main() -> int:
    requested_raw = (os.environ.get("REQUESTED_VERSION") or "").strip()
    requested = normalize_version(requested_raw) if requested_raw else None
    force_republish = (os.environ.get("FORCE_REPUBLISH") or "").lower() == "true"
    repo = os.environ.get("GITHUB_REPOSITORY", "")

    tree = fetch_filetree()
    packages_node = find_node(tree, PACKAGES_PATH)
    if packages_node is None:
        emit({"skip": "true", "reason": "openvino_genai packages node not found", "version": "", "matrix": '{"include":[]}'})
        return 0

    official_tags = fetch_official_release_tags()
    versions = list_stable_versions(packages_node)
    if requested:
        versions = [v for v in versions if v[0] == requested]
        if not versions:
            sys.exit(f"requested OpenVINO GenAI version {requested_raw} was not found on the CDN")

    selected: tuple[str, str, dict[str, Any], dict[str, str]] | None = None
    for version, dir_name, node in versions:
        if official_tags and version not in official_tags:
            print(f"  skipping {version}: not found in official openvino.genai releases", file=sys.stderr)
            continue
        archive = find_windows_archive(node, dir_name)
        if archive is None:
            print(f"  skipping {version}: no Windows archive with .sha256 sibling", file=sys.stderr)
            continue
        selected = (version, dir_name, node, archive)
        break

    if selected is None:
        if requested:
            sys.exit(f"requested OpenVINO GenAI version {requested_raw} has no packageable Windows archive")
        emit({"skip": "true", "reason": "no packageable OpenVINO GenAI release found", "version": "", "matrix": '{"include":[]}'})
        return 0

    version, _dir_name, _node, archive = selected
    tag = f"{LOCAL_TAG_PREFIX}{version}"
    if repo and not force_republish and local_tag_exists(repo, tag):
        emit({
            "skip": "true",
            "reason": f"{tag} already exists",
            "version": version,
            "matrix": '{"include":[]}',
        })
        return 0

    emit({
        "skip": "false",
        "reason": "",
        "version": version,
        "matrix": json.dumps({"include": [archive]}, separators=(",", ":")),
    })
    return 0


if __name__ == "__main__":
    sys.exit(main())
