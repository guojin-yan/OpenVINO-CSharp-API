#!/usr/bin/env bash
# Publish all .nupkg files in $NUPKG_DIR to configured NuGet feeds.
#
# Destination selection:
#   1. If NUGET_API_KEY is set -> push to https://api.nuget.org/v3/index.json
#      (the public NuGet Gallery).
#   2. If GITHUB_TOKEN + OWNER are set -> push to GitHub Packages at the
#      current repo owner's feed.
#
# `--skip-duplicate` is set for both feeds so re-runs of the same version
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

published_any=false
publish_failed=false

push_with_retry() {
  local package="$1"
  local source="$2"
  local api_key="$3"
  local attempt=1
  local max_attempts=3

  while [ "$attempt" -le "$max_attempts" ]; do
    echo "Pushing $(basename "$package") (attempt $attempt/$max_attempts)"
    if dotnet nuget push "$package" \
      --api-key "$api_key" \
      --source "$source" \
      --skip-duplicate \
      --timeout 900; then
      return 0
    fi

    if [ "$attempt" -lt "$max_attempts" ]; then
      delay=$((attempt * 20))
      echo "Push failed; retrying in ${delay}s" >&2
      sleep "$delay"
    fi
    attempt=$((attempt + 1))
  done

  echo "Failed to publish $(basename "$package") after $max_attempts attempts" >&2
  return 1
}

if [ -n "${NUGET_API_KEY:-}" ]; then
  source="https://api.nuget.org/v3/index.json"
  echo
  echo "Publishing to NuGet.org ($source)"
  failed=()
  for p in "${pkgs[@]}"; do
    push_with_retry "$p" "$source" "$NUGET_API_KEY" || failed+=("$(basename "$p")")
  done
  if [ "${#failed[@]}" -gt 0 ]; then
    printf 'NuGet.org publish failures:\n  - %s\n' "${failed[@]}" >&2
    publish_failed=true
  fi
  published_any=true
else
  echo
  echo "NUGET_API_KEY is not set; skipping NuGet.org publish" >&2
fi

if [ -z "${GITHUB_TOKEN:-}" ] || [ -z "$OWNER" ]; then
  echo
  echo "GITHUB_TOKEN or OWNER is not set; skipping GitHub Packages publish" >&2
else
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

  failed=()
  for p in "${pkgs[@]}"; do
    push_with_retry "$p" "$source" "${GITHUB_TOKEN}" || failed+=("$(basename "$p")")
  done
  if [ "${#failed[@]}" -gt 0 ]; then
    printf 'GitHub Packages publish failures:\n  - %s\n' "${failed[@]}" >&2
    publish_failed=true
  fi
  published_any=true
fi

if [ "$published_any" != true ]; then
  echo "neither NUGET_API_KEY nor (GITHUB_TOKEN + OWNER) are set; cannot publish" >&2
  exit 1
fi

if [ "$publish_failed" = true ]; then
  exit 1
fi
