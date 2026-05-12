#!/usr/bin/env bash
# Publish all .nupkg files in $NUPKG_DIR to a NuGet feed.
#
# Destination selection (one of):
#   1. If NUGET_API_KEY is set -> push to https://api.nuget.org/v3/index.json
#      (the public NuGet Gallery). This is the upstream maintainer's path.
#   2. Else -> push to GitHub Packages at the current repo owner's feed,
#      authenticated by the workflow's built-in GITHUB_TOKEN. This is the
#      fork path; no secrets to configure.
#
# `--skip-duplicate` is set in both cases so re-runs of the same version
# fail soft instead of failing the whole workflow.
set -euo pipefail

NUPKG_DIR="${NUPKG_DIR:-artifacts}"
OWNER="${OWNER:-${GITHUB_REPOSITORY_OWNER:-}}"

shopt -s nullglob
pkgs=( "$NUPKG_DIR"/*.nupkg )
shopt -u nullglob

if [ "${#pkgs[@]}" -eq 0 ]; then
  echo "no .nupkg files found in $NUPKG_DIR; nothing to publish" >&2
  exit 0
fi

echo "Found ${#pkgs[@]} packages in $NUPKG_DIR:"
for p in "${pkgs[@]}"; do
  echo "  - $(basename "$p")"
done

if [ -n "${NUGET_API_KEY:-}" ]; then
  source="https://api.nuget.org/v3/index.json"
  echo
  echo "Publishing to NuGet.org ($source)"
  for p in "${pkgs[@]}"; do
    dotnet nuget push "$p" \
      --api-key "$NUGET_API_KEY" \
      --source "$source" \
      --skip-duplicate
  done
  exit 0
fi

if [ -z "${GITHUB_TOKEN:-}" ] || [ -z "$OWNER" ]; then
  echo "neither NUGET_API_KEY nor (GITHUB_TOKEN + OWNER) are set; cannot publish" >&2
  exit 1
fi

# GitHub Packages NuGet feed. The owner is the user/org that hosts the
# repo running this workflow -- so a fork publishes to its own namespace.
source="https://nuget.pkg.github.com/${OWNER}/index.json"
echo
echo "Publishing to GitHub Packages ($source)"

# Register the source once. If it already exists from a previous step,
# `add source` fails -- that's fine, swallow it.
dotnet nuget add source "$source" \
  --name "github-${OWNER}" \
  --username "${OWNER}" \
  --password "${GITHUB_TOKEN}" \
  --store-password-in-clear-text \
  >/dev/null 2>&1 || true

for p in "${pkgs[@]}"; do
  dotnet nuget push "$p" \
    --api-key "${GITHUB_TOKEN}" \
    --source "$source" \
    --skip-duplicate
done
