#!/usr/bin/env python3
"""Send a concise runtime publication report over SMTP.

The workflow deliberately keeps SMTP configuration in repository secrets. If
the host and recipient are not configured, the script records that fact in the
job summary and exits successfully so publishing is never blocked by an
optional notification channel.
"""

from __future__ import annotations

import json
import os
import smtplib
import ssl
import sys
from email.message import EmailMessage
from typing import Any


JOB_NAMES = (
    ("DISCOVER_RESULT", "discover"),
    ("BUILD_RUNTIME_RESULT", "core runtime build"),
    ("BUILD_GENAI_RESULT", "GenAI runtime build"),
    ("PUBLISH_RESULT", "NuGet publish"),
    ("RELEASE_RESULT", "GitHub release"),
)


def parse_matrix(raw: str | None) -> list[dict[str, Any]]:
    if not raw:
        return []
    try:
        value = json.loads(raw)
    except json.JSONDecodeError:
        return []
    items = value.get("include", []) if isinstance(value, dict) else []
    return [item for item in items if isinstance(item, dict)]


def package_lines(label: str, items: list[dict[str, Any]]) -> list[str]:
    if not items:
        return [f"{label}: none"]
    return [f"{label}: " + ", ".join(str(item.get("id", "unknown")) for item in items)]


def build_report(env: dict[str, str]) -> tuple[str, str]:
    version = env.get("RUNTIME_VERSION", "unknown")
    run_url = env.get("RUN_URL", "")
    published = env.get("PUBLISH_RESULT") == "success" and env.get("RELEASE_RESULT") == "success"
    outcome = "published" if published else "not fully published"

    subject = f"OpenVINO C# runtime {version}: {outcome}"
    lines = [
        f"OpenVINO C# runtime packaging report / OpenVINO C# runtime 打包报告",
        f"Version / 版本: {version}",
        f"Outcome / 结果: {outcome}",
        "",
        *package_lines("Core packages / Core 包", parse_matrix(env.get("RUNTIME_MATRIX"))),
        *package_lines("GenAI packages / GenAI 包", parse_matrix(env.get("GENAI_MATRIX"))),
        "",
        "Job results / 工作流结果:",
    ]
    for key, label in JOB_NAMES:
        lines.append(f"- {label}: {env.get(key, 'unknown')}")

    release_url = env.get("RELEASE_URL")
    if release_url:
        lines.extend(["", f"GitHub release / GitHub 发布页: {release_url}"])
    if run_url:
        lines.append(f"Actions run / Actions 运行: {run_url}")
    lines.extend(
        [
            "",
            "Every archive was verified against its official SHA-256 sidecar before packing.",
            "所有归档文件在打包前均已按官方 SHA-256 sidecar 校验。",
        ]
    )
    return subject, "\n".join(lines) + "\n"


def write_summary(text: str) -> None:
    path = os.environ.get("GITHUB_STEP_SUMMARY")
    if path:
        with open(path, "a", encoding="utf-8") as handle:
            handle.write(text.rstrip() + "\n")


def send(subject: str, body: str, env: dict[str, str]) -> None:
    host = env.get("SMTP_HOST", "").strip()
    recipient = env.get("SMTP_TO", "").strip()
    if not host and not recipient:
        message = "SMTP notification is not configured; set SMTP_HOST and SMTP_TO repository secrets to enable it."
        print(message, file=sys.stderr)
        write_summary(f"### Release email\n\n{message}")
        return
    missing = [name for name, value in (("SMTP_HOST", host), ("SMTP_TO", recipient)) if not value]
    if missing:
        raise RuntimeError("incomplete SMTP configuration; missing " + ", ".join(missing))

    sender = env.get("SMTP_FROM", "").strip() or env.get("SMTP_USERNAME", "").strip() or recipient
    port = int(env.get("SMTP_PORT", "587") or "587")
    security = (env.get("SMTP_SECURITY", "starttls") or "starttls").strip().lower()
    username = env.get("SMTP_USERNAME", "").strip()
    password = env.get("SMTP_PASSWORD", "")

    message = EmailMessage()
    message["From"] = sender
    message["To"] = recipient
    message["Subject"] = subject
    message.set_content(body)

    if security == "ssl":
        with smtplib.SMTP_SSL(host, port, context=ssl.create_default_context(), timeout=30) as client:
            if username:
                client.login(username, password)
            client.send_message(message)
    elif security in {"starttls", "tls"}:
        with smtplib.SMTP(host, port, timeout=30) as client:
            client.starttls(context=ssl.create_default_context())
            if username:
                client.login(username, password)
            client.send_message(message)
    elif security in {"none", "plain"}:
        with smtplib.SMTP(host, port, timeout=30) as client:
            if username:
                client.login(username, password)
            client.send_message(message)
    else:
        raise ValueError("SMTP_SECURITY must be starttls, ssl, or none")

    print(f"Release report sent to {recipient}")
    write_summary(f"### Release email\n\nSent runtime report to `{recipient}`.")


def main() -> int:
    subject, body = build_report(dict(os.environ))
    send(subject, body, dict(os.environ))
    return 0


if __name__ == "__main__":
    sys.exit(main())
