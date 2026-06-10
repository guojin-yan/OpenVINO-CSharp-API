# 安装方式 / Installation

This guide explains how to install and configure OpenVINO C# API packages.

本文介绍如何安装和配置 OpenVINO C# API 相关 NuGet 包。

## Supported Platforms / 支持平台

| Platform / 平台 | Core runtime / 基础 runtime | GenAI runtime / GenAI runtime |
| --- | --- | --- |
| Windows x64 | Supported / 已支持 | Supported / 已支持 |
| Ubuntu 24 x64 | Supported / 已支持 | Supported / 已支持 |
| Ubuntu 22 x64 | Supported / 已支持 | Supported / 已支持 |
| Ubuntu 22 ARM64 | Supported / 已支持 | Supported / 已支持 |
| Ubuntu 20 x64 | Supported / 已支持 | Supported / 已支持 |
| Ubuntu 20 ARM64 | Supported / 已支持 | Supported / 已支持 |
| RHEL 8 x64 | Supported / 已支持 | Supported / 已支持 |
| macOS x64 | Supported / 已支持 | Supported / 已支持 |
| macOS ARM64 | Supported / 已支持 | Supported / 已支持 |

## Package Choice / 包选择

For normal OpenVINO inference, install the managed API package and one platform runtime package:

普通 OpenVINO 推理需要安装托管 API 包和一个平台 runtime 包：

```bash
dotnet add package JYPPX.OpenVINO.CSharp.API
dotnet add package OpenVINO.runtime.win
```

If your application calls `OpenVinoSharp.GenAI`, install the matching GenAI runtime package. The GenAI runtime package already carries the OpenVINO native dependencies needed by GenAI, so do not reference both the normal runtime package and the GenAI runtime package in the same project:

如果应用调用 `OpenVinoSharp.GenAI`，请安装对应平台的 GenAI runtime 包。GenAI runtime 包已经包含 GenAI 所需的 OpenVINO native 依赖，不要在同一个项目里同时引用普通 runtime 包和 GenAI runtime 包：

```bash
dotnet add package JYPPX.OpenVINO.CSharp.API
dotnet add package JYPPX.OpenVINO.GenAI.runtime.win
```

Core OpenVINO APIs do not load `openvino_genai_c`, so applications that only use `Core`, `Model`, `Tensor`, `CompiledModel`, or `InferRequest` do not need GenAI runtime.

基础 OpenVINO API 不会加载 `openvino_genai_c`，只使用 `Core`、`Model`、`Tensor`、`CompiledModel` 或 `InferRequest` 的应用不需要 GenAI runtime。

## Quick Start / 快速开始

```csharp
using OpenVinoSharp;

using Core core = new Core();
using Model model = core.ReadModel("model.xml");
using CompiledModel compiled = core.CompileModel(model, "CPU");
using InferRequest request = compiled.CreateInferRequest();
```

## Related Guides / 相关文档

- [Windows 安装指南](windows.md)
- [Linux 安装指南](linux.md)
- [macOS 安装指南](macos.md)
- [OpenVINO GenAI Runtime](genai-runtime.md)
- [问题排查 / Troubleshooting](../troubleshooting/index.md)
