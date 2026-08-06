# 版本说明 / Release Notes

本目录集中保存 OpenVINO C# API 的版本记录，自 3.3.1 起按统一格式维护。
README 只展示当前版本摘要；完整变更、兼容性、升级方法和已知限制以本目录中的单版本文件为准。

This directory contains the OpenVINO C# API release history, maintained in a
consistent format starting with version 3.3.1. The repository README contains
only the current release summary; use the per-version files for complete
changes, compatibility notes, migration guidance, and known limitations.

## 版本索引 / Release Index

| API 版本 | OpenVINO Core | OpenVINO GenAI | 发布日期 | 类型 | 详细说明 |
| --- | --- | --- | --- | --- | --- |
| 3.3.1 | 2026.3.0 | 2026.3.0.0 | 2026-08-05 | 兼容性更新 / Maintenance | [查看说明](3.3.1.md) |

## 维护要求 / Maintenance Rules

任何修改 API 包版本、OpenVINO 对齐版本或已发布兼容性的版本迭代，都必须同时完成：

1. 更新根目录 `README.md` 和 `README_EN.md` 中的当前版本摘要；
2. 复制 [版本说明模板](TEMPLATE.md)，新增 `docs/release-notes/<version>.md`；
3. 在上方版本索引中新增记录，最新版本放在最前；
4. 明确兼容性、弃用项、升级步骤、支持平台和已知限制；
5. 确认版本号、包名和文档内部链接一致。

版本发布 PR 缺少上述任一项时，视为尚未完成，不应合并或发布。

Every release that changes the managed package version, aligned OpenVINO
version, or published compatibility contract must update both root README
summaries, add a per-version note from the template, update this index, and
document compatibility, deprecations, migration, supported platforms, and
known limitations. A release PR is incomplete until all items are present.
