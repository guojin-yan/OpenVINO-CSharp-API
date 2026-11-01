#!/usr/bin/env python3
"""Exercise publishing and failure handling with fake dotnet; never upload packages."""

from __future__ import annotations

import os
import pathlib
import shutil
import subprocess
import tempfile
import unittest


ROOT = pathlib.Path(__file__).resolve().parent
BASH = os.environ.get("BASH_PATH") or shutil.which("bash")


def shell_path(path: pathlib.Path) -> str:
    value = path.as_posix()
    return f"/{value[0].lower()}{value[2:]}" if os.name == "nt" else value


class PublishTests(unittest.TestCase):
    def setUp(self) -> None:
        self.assertIsNotNone(BASH, "Bash is required; set BASH_PATH when testing on Windows.")
        temporary = tempfile.TemporaryDirectory(prefix="openvino-publish-test-")
        self.addCleanup(temporary.cleanup)
        self.workspace = pathlib.Path(temporary.name)
        self.bin_dir = self.workspace / "bin"
        self.bin_dir.mkdir()
        self.packages = self.workspace / "packages"
        self.packages.mkdir()
        (self.packages / "example.1.0.0.nupkg").touch()
        self.log = self.workspace / "commands.log"
        self.log.touch()
        mock_dotnet = self.bin_dir / "dotnet"
        mock_dotnet.write_text(
            '#!/usr/bin/env bash\n'
            'printf "%s\\n" "$*" >> "$PUBLISH_TEST_LOG"\n'
            'if [[ "$1 $2" == "nuget push" && -n "${FAIL_SOURCE:-}" && "$*" == *"$FAIL_SOURCE"* ]]; then\n'
            '  exit 1\n'
            'fi\n',
            encoding="utf-8",
            newline="\n",
        )
        mock_dotnet.chmod(0o755)
        mock_sleep = self.bin_dir / "sleep"
        mock_sleep.write_text("#!/usr/bin/env bash\nexit 0\n", encoding="utf-8", newline="\n")
        mock_sleep.chmod(0o755)

    def publish(self, *, packages: bool = True, **settings: str) -> subprocess.CompletedProcess[str]:
        env = os.environ.copy()
        for name in (
            "NUGET_API_KEY", "GITHUB_TOKEN", "OWNER", "GITHUB_REPOSITORY_OWNER",
            "REQUIRE_NUGET", "FAIL_SOURCE", "BASH_ENV",
        ):
            env.pop(name, None)
        env.update(
            NUPKG_DIR=shell_path(self.packages if packages else self.workspace / "missing"),
            PUBLISH_TEST_BIN=shell_path(self.bin_dir),
            PUBLISH_TEST_LOG=shell_path(self.log),
            **settings,
        )
        # Bash constructs its own PATH, also making these mocks work in Git Bash.
        return subprocess.run(
            [BASH, "-c", 'export PATH="$PUBLISH_TEST_BIN:$PATH"; exec bash "$1"', "publish-test", shell_path(ROOT / "publish.sh")],
            env=env,
            capture_output=True,
            text=True,
            timeout=20,
            check=False,
        )

    def commands(self) -> str:
        return self.log.read_text(encoding="utf-8")

    def test_required_oidc_key_cannot_fall_back_to_github(self) -> None:
        result = self.publish(REQUIRE_NUGET="true", GITHUB_TOKEN="test-github-token", OWNER="example")
        self.assertNotEqual(result.returncode, 0)
        self.assertIn("Trusted Publishing", result.stderr)
        self.assertEqual(self.commands(), "")

    def test_oidc_credential_publishes_all_packages_to_both_feeds(self) -> None:
        (self.packages / "second.1.0.0.nupkg").touch()
        result = self.publish(
            REQUIRE_NUGET="true", NUGET_API_KEY="test-temporary-key",
            GITHUB_TOKEN="test-github-token", OWNER="example",
        )
        self.assertEqual(result.returncode, 0, result.stdout + result.stderr)
        pushes = [line for line in self.commands().splitlines() if line.startswith("nuget push ")]
        self.assertEqual(len(pushes), 4)
        self.assertEqual(sum("api.nuget.org" in line for line in pushes), 2)
        self.assertEqual(sum("nuget.pkg.github.com/example" in line for line in pushes), 2)
        self.assertTrue(all("--skip-duplicate" in line for line in pushes))

    def test_unconfigured_fork_can_publish_only_to_github(self) -> None:
        result = self.publish(GITHUB_TOKEN="test-github-token", OWNER="fork-owner")
        self.assertEqual(result.returncode, 0, result.stdout + result.stderr)
        self.assertIn("nuget.pkg.github.com/fork-owner", self.commands())
        self.assertNotIn("api.nuget.org", self.commands())

    def test_missing_all_credentials_fails(self) -> None:
        result = self.publish()
        self.assertNotEqual(result.returncode, 0)
        self.assertEqual(self.commands(), "")

    def test_empty_artifact_directory_fails(self) -> None:
        result = self.publish(packages=False, NUGET_API_KEY="test-temporary-key")
        self.assertNotEqual(result.returncode, 0)
        self.assertIn("no .nupkg files found", result.stderr)
        self.assertEqual(self.commands(), "")

    def test_nuget_failure_is_not_hidden_by_github_success(self) -> None:
        result = self.publish(
            REQUIRE_NUGET="true", NUGET_API_KEY="test-temporary-key",
            GITHUB_TOKEN="test-github-token", OWNER="example", FAIL_SOURCE="api.nuget.org",
        )
        self.assertNotEqual(result.returncode, 0)
        self.assertIn("NuGet.org publish failures", result.stderr)
        self.assertIn("nuget.pkg.github.com/example", self.commands())

    def test_github_failure_is_not_hidden_by_nuget_success(self) -> None:
        result = self.publish(
            REQUIRE_NUGET="true", NUGET_API_KEY="test-temporary-key",
            GITHUB_TOKEN="test-github-token", OWNER="example", FAIL_SOURCE="nuget.pkg.github.com",
        )
        self.assertNotEqual(result.returncode, 0)
        self.assertIn("GitHub Packages publish failures", result.stderr)


if __name__ == "__main__":
    unittest.main()
