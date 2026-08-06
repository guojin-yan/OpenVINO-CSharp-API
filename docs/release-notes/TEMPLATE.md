# OpenVINO C# API `<API_VERSION>` 版本说明

> 复制本文件创建 `<API_VERSION>.md`，完成后删除所有提示文字和不适用的小节。

## 版本信息

| 项目 | 版本或内容 |
| --- | --- |
| OpenVINO C# API | `<API_VERSION>` |
| OpenVINO Core | `<OPENVINO_VERSION>` |
| OpenVINO GenAI | `<GENAI_VERSION 或 不适用>` |
| 发布日期 | `<YYYY-MM-DD>` |
| 发布类型 | `<Major / Minor / Patch / Maintenance>` |

## 更新摘要

- `<面向使用者的主要变化，建议 3 至 6 条>`

## 详细变更

### Core API

- `<新增、修复或行为变化>`

### GenAI

- `<新增、修复或行为变化；不适用时说明>`

### Runtime 与平台

- `<包版本、支持系统、架构或设备变化>`

### 示例与文档

- `<新增或更新的使用示例>`

## 兼容性与弃用项

- 破坏性变化：`<无或逐项列出>`
- 弃用 API：`<无或列出替代方案>`
- 行为变化：`<无或列出需要使用者关注的变化>`

## 升级方法

```xml
<PackageReference Include="JYPPX.OpenVINO.CSharp.API" Version="<API_VERSION>" />
<PackageReference Include="OpenVINO.runtime.<platform>" Version="<OPENVINO_VERSION>" />
```

1. `<升级步骤>`
2. `<代码迁移步骤>`
3. `<验证建议>`

## 支持平台

| 场景 | 平台或包 |
| --- | --- |
| Core | `<列出本版本实际发布的平台>` |
| GenAI | `<列出本版本实际发布的平台>` |

## 已知限制

- `<没有时明确写“无已知新增限制”>`

## 验证

- `<构建、测试、示例或兼容性验证结论>`

## English Summary

- API version: `<API_VERSION>`
- Aligned OpenVINO version: `<OPENVINO_VERSION>`
- Compatibility: `<short summary>`
- Migration: `<short summary>`
- Known limitations: `<short summary>`
