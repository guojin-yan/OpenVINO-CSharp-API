#!/usr/bin/env python3
"""Small stdlib-only regression tests for release automation helpers."""

from __future__ import annotations

import importlib.util
import json
import pathlib
import unittest
from unittest import mock


ROOT = pathlib.Path(__file__).resolve().parent


def load(name: str):
    spec = importlib.util.spec_from_file_location(name, ROOT / f"{name}.py")
    assert spec and spec.loader
    module = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(module)
    return module


discover = load("discover")
discover_genai = load("discover_genai")
email_report = load("send_release_email")


class DiscoveryTests(unittest.TestCase):
    def test_core_tags_include_release_and_stable_git_tag(self) -> None:
        releases = json.dumps([{"tag_name": "2026.2.0", "draft": False, "prerelease": False}]).encode()
        tags = json.dumps([{"name": "2026.3.0"}]).encode()
        with mock.patch.object(discover, "http_get", side_effect=[releases, tags]):
            self.assertEqual(discover.fetch_official_tags(), {"2026.2.0", "2026.3.0"})

    def test_genai_tags_include_release_and_stable_git_tag(self) -> None:
        releases = json.dumps([{"tag_name": "2026.2.0.0", "draft": False, "prerelease": False}]).encode()
        tags = json.dumps([{"name": "2026.3.0.0"}]).encode()
        with mock.patch.object(discover_genai, "http_get", side_effect=[releases, tags]):
            self.assertEqual(discover_genai.fetch_official_tags(), {"2026.2.0", "2026.3.0"})

    def test_checksum_parser_rejects_html_directory_listing(self) -> None:
        self.assertIsNone(discover_genai.parse_sha256_text("<!doctype html><html>", "archive.zip"))


class EmailReportTests(unittest.TestCase):
    def test_report_contains_package_ids_and_links(self) -> None:
        subject, body = email_report.build_report(
            {
                "RUNTIME_VERSION": "2026.3.0",
                "RUNTIME_MATRIX": json.dumps({"include": [{"id": "win"}]}),
                "GENAI_MATRIX": json.dumps({"include": [{"id": "ubuntu.24-x86_64"}]}),
                "PUBLISH_RESULT": "success",
                "RELEASE_RESULT": "success",
                "RELEASE_URL": "https://github.com/example/releases/tag/openvino-runtime-v2026.3.0",
            }
        )
        self.assertIn("2026.3.0", subject)
        self.assertIn("win", body)
        self.assertIn("ubuntu.24-x86_64", body)
        self.assertIn("openvino-runtime-v2026.3.0", body)

    def test_unconfigured_email_is_a_noop(self) -> None:
        with mock.patch.object(email_report, "write_summary") as summary:
            email_report.send("subject", "body", {})
        summary.assert_called_once()


if __name__ == "__main__":
    unittest.main()
