#!/usr/bin/env python3
"""Discover the latest OpenVINO release on Intel's official CDN and build a
matrix describing which runtime NuGet packages to produce.

Outputs (to $GITHUB_OUTPUT when present, otherwise stdout):
  version  -- OpenVINO release version (e.g. "2026.1.0")
  matrix   -- JSON: {"include": [{id, archive_url, sha256_url, rid, kind}, ...]}
  skip     -- "true" if no package work is needed
  reason   -- human-readable explanation when skipping

Selection criteria for "latest":
  1. Iterate version directories under
     https://storage.openvinotoolkit.org/repositories/openvino/packages/,
     newest semver first.
  2. Require the version to exist as a non-prerelease GitHub release tag
     at openvinotoolkit/openvino. This treats the upstream git tag as the
     authoritative "this is a real release" signal; the CDN is the binary
     mirror.
  3. Require at least one core platform archive (windows, ubuntu, macos)
     to be present in that version's directory so we don't pick up a
     Windows-only patch release.

The platform table mirrors the package IDs the upstream project has
already published to NuGet.org (see README.md). Archives that don't exist
for the chosen version are silently skipped -- e.g. macos-x86_64 was
dropped after 2025.4, so the macos-x86_64 NuGet just doesn't get a new
release for 2026.x.
"""

from __future__ import annotations

import json
import http.client
import os
import re
import sys
import time
import urllib.error
import urllib.request
from typing import Any

CDN_ROOT = "https://storage.openvinotoolkit.org"
FILETREE_URL = f"{CDN_ROOT}/filetree.json"
PACKAGES_PATH = ("repositories", "openvino", "packages")
GH_RELEASES_API = (
    "https://api.github.com/repos/openvinotoolkit/openvino/releases?per_page=100"
)
GH_TAGS_API = "https://api.github.com/repos/openvinotoolkit/openvino/tags?per_page=100"
# Tag the release job creates after a successful publish. Used as the
# "have we already shipped this version?" marker.
LOCAL_TAG_PREFIX = "openvino-runtime-v"

# Each entry produces one NuGet package: OpenVINO.runtime.<id>
# `archive` is a regex against the filename in the CDN's per-version dir;
# `{ver}` is substituted with the OpenVINO release version (e.g. "2026.1.0").
PLATFORMS: list[dict[str, str]] = [
    {"id": "win",              "os_dir": "windows", "archive": r"^openvino_toolkit_windows_{ver}\.\d+\.[0-9a-f]+_x86_64\.zip$", "rid": "win-x64",     "kind": "zip"},
    {"id": "ubuntu.24-x86_64", "os_dir": "linux",   "archive": r"^openvino_toolkit_ubuntu24_{ver}\.\d+\.[0-9a-f]+_x86_64\.tgz$", "rid": "linux-x64",   "kind": "tgz"},
    {"id": "ubuntu.22-x86_64", "os_dir": "linux",   "archive": r"^openvino_toolkit_ubuntu22_{ver}\.\d+\.[0-9a-f]+_x86_64\.tgz$", "rid": "linux-x64",   "kind": "tgz"},
    {"id": "ubuntu.22-arm64",  "os_dir": "linux",   "archive": r"^openvino_toolkit_ubuntu22_{ver}\.\d+\.[0-9a-f]+_arm64\.tgz$",  "rid": "linux-arm64", "kind": "tgz"},
    {"id": "ubuntu.20-x86_64", "os_dir": "linux",   "archive": r"^openvino_toolkit_ubuntu20_{ver}\.\d+\.[0-9a-f]+_x86_64\.tgz$", "rid": "linux-x64",   "kind": "tgz"},
    {"id": "ubuntu.20-arm64",  "os_dir": "linux",   "archive": r"^openvino_toolkit_ubuntu20_{ver}\.\d+\.[0-9a-f]+_arm64\.tgz$",  "rid": "linux-arm64", "kind": "tgz"},
    {"id": "ubuntu.18-x86_64", "os_dir": "linux",   "archive": r"^openvino_toolkit_ubuntu18_{ver}\.\d+\.[0-9a-f]+_x86_64\.tgz$", "rid": "linux-x64",   "kind": "tgz"},
    {"id": "ubuntu.18-arm64",  "os_dir": "linux",   "archive": r"^openvino_toolkit_ubuntu18_{ver}\.\d+\.[0-9a-f]+_arm64\.tgz$",  "rid": "linux-arm64", "kind": "tgz"},
    {"id": "debian10-armhf",   "os_dir": "linux",   "archive": r"^openvino_toolkit_debian10_{ver}\.\d+\.[0-9a-f]+_armhf\.tgz$",  "rid": "linux-arm",   "kind": "tgz"},
    {"id": "debian9-arm64",    "os_dir": "linux",   "archive": r"^openvino_toolkit_debian9_{ver}\.\d+\.[0-9a-f]+_arm64\.tgz$",   "rid": "linux-arm64", "kind": "tgz"},
    {"id": "debian9-armhf",    "os_dir": "linux",   "archive": r"^openvino_toolkit_debian9_{ver}\.\d+\.[0-9a-f]+_armhf\.tgz$",   "rid": "linux-arm",   "kind": "tgz"},
    {"id": "centos7-x86_64",   "os_dir": "linux",   "archive": r"^openvino_toolkit_centos7_{ver}\.\d+\.[0-9a-f]+_x86_64\.tgz$",  "rid": "linux-x64",   "kind": "tgz"},
    {"id": "centos8-x86_64",   "os_dir": "linux",   "archive": r"^openvino_toolkit_centos8_{ver}\.\d+\.[0-9a-f]+_x86_64\.tgz$",  "rid": "linux-x64",   "kind": "tgz"},
    {"id": "rhel8-x86_64",     "os_dir": "linux",   "archive": r"^openvino_toolkit_rhel8_{ver}\.\d+\.[0-9a-f]+_x86_64\.tgz$",    "rid": "linux-x64",   "kind": "tgz"},
    {"id": "macos-x86_64",     "os_dir": "macos",   "archive": r"^openvino_toolkit_macos_\d+_\d+_{ver}\.\d+\.[0-9a-f]+_x86_64\.tgz$", "rid": "osx-x64",  "kind": "tgz"},
    {"id": "macos-arm64",      "os_dir": "macos",   "archive": r"^openvino_toolkit_macos_\d+_\d+_{ver}\.\d+\.[0-9a-f]+_arm64\.tgz$",  "rid": "osx-arm64", "kind": "tgz"},
]

# Core platforms that must all be present before we promote a version
# directory to "this is a complete release we want to package".
# (Windows-only or Linux-only patch directories should be skipped.)
CORE_PRESENCE_OS_DIRS = {"windows", "linux", "macos"}

VERSION_DIR_RE = re.compile(r"^(\d+)\.(\d+)(?:\.(\d+))?$")


def http_get(url: str, attempts: int = 4) -> bytes:
    """Read a discovery endpoint with bounded retries for transient failures."""
    transient_statuses = {408, 429, 500, 502, 503, 504}
    for attempt in range(1, attempts + 1):
        req = urllib.request.Request(url, headers={"User-Agent": "openvino-csharp-runtime-bot"})
        token = os.environ.get("GITHUB_TOKEN")
        if token and url.startswith("https://api.github.com/"):
            req.add_header("Authorization", f"Bearer {token}")
            req.add_header("Accept", "application/vnd.github+json")
        try:
            with urllib.request.urlopen(req, timeout=60) as resp:
                return resp.read()
        except urllib.error.HTTPError as exc:
            if exc.code not in transient_statuses or attempt == attempts:
                raise
            error: Exception = exc
        except (urllib.error.URLError, TimeoutError, http.client.IncompleteRead) as exc:
            if attempt == attempts:
                raise
            error = exc

        delay = min(2 ** (attempt - 1), 8)
        print(
            f"  transient read failure ({attempt}/{attempts}) for {url}: {error}; "
            f"retrying in {delay}s",
            file=sys.stderr,
        )
        time.sleep(delay)

    raise RuntimeError(f"failed to read {url}")


def local_tag_exists(repo: str, tag: str) -> bool:
    """Return True if `tag` exists in the GitHub repo `<owner>/<name>`.

    Used to short-circuit the workflow when we've already published the
    target OpenVINO version -- the release job creates this tag on every
    successful run, so its presence means "nothing new to do here".
    """
    url = f"https://api.github.com/repos/{repo}/git/refs/tags/{tag}"
    req = urllib.request.Request(url, headers={"User-Agent": "openvino-csharp-runtime-bot"})
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


def parse_version(name: str) -> tuple[int, int, int] | None:
    m = VERSION_DIR_RE.match(name)
    if not m:
        return None
    major, minor, patch = m.group(1), m.group(2), m.group(3) or "0"
    return (int(major), int(minor), int(patch))


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
    raw = http_get(FILETREE_URL)
    return json.loads(raw)


def fetch_official_tags() -> set[str]:
    """Return stable upstream tags, including tags awaiting a Release page."""
    releases = json.loads(http_get(GH_RELEASES_API))
    tags: set[str] = set()
    for r in releases:
        if r.get("draft") or r.get("prerelease"):
            continue
        tag = r.get("tag_name")
        if isinstance(tag, str):
            # Normalize "v2026.1.0" -> "2026.1.0"
            normalized = normalize_version(tag.lstrip("vV"))
            if parse_version(normalized) is not None:
                tags.add(normalized)

    # Upstream sometimes pushes a stable tag before GitHub creates the Release
    # page. The CDN archive plus SHA-256 sidecar still remain mandatory.
    for item in json.loads(http_get(GH_TAGS_API)):
        tag = item.get("name")
        if not isinstance(tag, str):
            continue
        normalized = normalize_version(tag.lstrip("vV"))
        if parse_version(normalized) is not None:
            tags.add(normalized)
    return tags


def normalize_version(name: str) -> str:
    """Pad a 2-component version to 3 components: '2026.1' -> '2026.1.0'."""
    parts = name.split(".")
    while len(parts) < 3:
        parts.append("0")
    return ".".join(parts)


def list_version_dirs(packages_node: dict[str, Any]) -> list[tuple[tuple[int, int, int], str, dict[str, Any]]]:
    out: list[tuple[tuple[int, int, int], str, dict[str, Any]]] = []
    for child in packages_node.get("children") or []:
        if child.get("type") != "directory":
            continue
        v = parse_version(child.get("name", ""))
        if v is None:
            continue
        out.append((v, child["name"], child))
    out.sort(key=lambda t: t[0], reverse=True)
    return out


def os_dirs_present(version_node: dict[str, Any]) -> set[str]:
    return {
        c.get("name") for c in (version_node.get("children") or [])
        if c.get("type") == "directory" and c.get("name") in CORE_PRESENCE_OS_DIRS
    }


def collect_archives(version_node: dict[str, Any], version: str, dir_name: str) -> list[dict[str, str]]:
    """For each PLATFORMS entry, find the matching archive file (if any).

    `version` is the normalized 3-component release string ("2026.1.0") used
    to match the filename pattern; `dir_name` is the actual CDN directory
    name ("2026.1") used to build URLs. They are NOT always the same -- the
    CDN drops trailing zero patch components from directory names but keeps
    them in filenames.
    """
    items: list[dict[str, str]] = []
    # Index files by os_dir for cheap lookup
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

    for plat in PLATFORMS:
        files = by_os.get(plat["os_dir"], [])
        pattern = re.compile(plat["archive"].format(ver=re.escape(version)))
        match = next((f for f in files if pattern.match(f)), None)
        if match is None:
            continue
        if f"{match}.sha256" not in files:
            print(f"  skipping {plat['id']}: no .sha256 sibling for {match}", file=sys.stderr)
            continue
        archive_url = f"{CDN_ROOT}/repositories/openvino/packages/{dir_name}/{plat['os_dir']}/{match}"
        items.append({
            "id": plat["id"],
            "archive_url": archive_url,
            "sha256_url": f"{archive_url}.sha256",
            "rid": plat["rid"],
            "kind": plat["kind"],
        })
    return items


def write_output(key: str, value: str) -> None:
    out_path = os.environ.get("GITHUB_OUTPUT")
    if out_path:
        with open(out_path, "a", encoding="utf-8") as fh:
            # Use heredoc form so JSON with newlines/quotes is safe.
            fh.write(f"{key}<<__EOF__\n{value}\n__EOF__\n")
    else:
        print(f"{key}={value}")


def emit_skip(reason: str) -> None:
    print(f"SKIP: {reason}", file=sys.stderr)
    write_output("skip", "true")
    write_output("reason", reason)
    write_output("version", "")
    write_output("matrix", json.dumps({"include": []}))


def main() -> int:
    requested = (os.environ.get("REQUESTED_VERSION") or "").strip()
    if requested:
        requested = normalize_version(requested.lstrip("vV"))
        print(f"requested version: {requested}", file=sys.stderr)

    print(f"fetching {FILETREE_URL}", file=sys.stderr)
    tree = fetch_filetree()
    packages_node = find_node(tree, PACKAGES_PATH)
    if packages_node is None:
        emit_skip(f"packages path not found in filetree")
        return 0

    print("fetching openvinotoolkit/openvino stable tags", file=sys.stderr)
    official_tags = fetch_official_tags()
    print(f"  found {len(official_tags)} stable tags", file=sys.stderr)

    version_dirs = list_version_dirs(packages_node)
    if not version_dirs:
        emit_skip("no version directories under packages/")
        return 0

    chosen_version: str | None = None
    chosen_dir_name: str | None = None
    chosen_node: dict[str, Any] | None = None

    if requested:
        match = next((t for t in version_dirs if normalize_version(t[1]) == requested), None)
        if match is None:
            emit_skip(f"requested version {requested} not found under packages/")
            return 0
        if requested not in official_tags:
            emit_skip(
                f"requested version {requested} has no non-prerelease release tag "
                f"at openvinotoolkit/openvino; refusing to package an unofficial build"
            )
            return 0
        chosen_version = requested
        chosen_dir_name = match[1]
        chosen_node = match[2]
    else:
        for _, name, node in version_dirs:
            v = normalize_version(name)
            if v not in official_tags:
                print(f"  skip {v}: no matching release tag at openvinotoolkit/openvino", file=sys.stderr)
                continue
            present = os_dirs_present(node)
            if not CORE_PRESENCE_OS_DIRS.issubset(present):
                missing = CORE_PRESENCE_OS_DIRS - present
                print(f"  skip {v}: incomplete release, missing {sorted(missing)}", file=sys.stderr)
                continue
            chosen_version = v
            chosen_dir_name = name
            chosen_node = node
            break

    if chosen_version is None or chosen_dir_name is None or chosen_node is None:
        emit_skip("no version directory satisfies both 'official release tag' and 'all core OSes present'")
        return 0

    # Short-circuit if this version was already shipped in the current
    # repo. The release job tags every successful run as
    # `openvino-runtime-v<version>` -- its presence means nothing has
    # changed since last time and we'd just be re-packing identical
    # archives. Use FORCE_REPUBLISH=true to override.
    repo = os.environ.get("GITHUB_REPOSITORY", "").strip()
    force = (os.environ.get("FORCE_REPUBLISH") or "").strip().lower() == "true"
    if repo and not force:
        tag = f"{LOCAL_TAG_PREFIX}{chosen_version}"
        if local_tag_exists(repo, tag):
            emit_skip(
                f"tag {tag} already exists in {repo}; nothing new to publish "
                f"(set force_republish=true to rebuild and re-push)"
            )
            return 0
        print(f"  tag {tag} not yet present in {repo}; proceeding", file=sys.stderr)

    items = collect_archives(chosen_node, chosen_version, chosen_dir_name)
    if not items:
        emit_skip(f"version {chosen_version} matched no platform archives")
        return 0

    print(f"chosen version: {chosen_version}", file=sys.stderr)
    print(f"matrix items ({len(items)}):", file=sys.stderr)
    for it in items:
        print(f"  - {it['id']} <- {it['archive_url']}", file=sys.stderr)

    write_output("version", chosen_version)
    write_output("matrix", json.dumps({"include": items}))
    write_output("skip", "false")
    write_output("reason", "")
    return 0


if __name__ == "__main__":
    sys.exit(main())
