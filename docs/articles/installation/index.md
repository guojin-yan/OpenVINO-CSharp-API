# 安装方式 / Installation

本文档介绍如何在不同平台安装和配置 OpenVINO C# API。

This guide explains how to install and configure the OpenVINO C# API on supported platforms.

## 支持的平台 / Supported Platforms

| 平台 / Platform | 状态 / Status | 说明 / Description |
| --- | --- | --- |
| Windows | Supported / 已支持 | Windows 10/11, Windows Server 2019/2022 |
| Linux | Planned / 计划中 | Ubuntu 20.04/22.04, CentOS, and related distributions |
| macOS | Planned / 计划中 | macOS 11+ on Intel or Apple Silicon |

## 安装方式概览 / Installation Methods

### Windows

- [Windows 安装指南](windows.md) - 在 Windows 平台安装 OpenVINO C# API。

### Linux

- [Linux 安装指南](linux.md) - 在 Linux 平台安装 OpenVINO C# API。

### macOS

- [macOS 安装指南](macos.md) - 在 macOS 平台安装 OpenVINO C# API。

### OpenVINO GenAI Runtime

- [OpenVINO GenAI Runtime](genai-runtime.md) - 安装可选的 GenAI runtime 包，用于 `OpenVinoSharp.GenAI`。

## 快速开始 / Quick Start

### NuGet 包安装 / NuGet Packages

```bash
dotnet add package JYPPX.OpenVINO.CSharp.API
dotnet add package OpenVINO.runtime.win
```

If your application uses GenAI APIs, also install the GenAI runtime package:

如果应用使用 GenAI API，还需要安装 GenAI runtime 包：

```bash
dotnet add package JYPPX.OpenVINO.GenAI.runtime.win
```

### 基础用法 / Basic Usage

```csharp
using OpenVinoSharp;

using Core core = new Core();
Model model = core.read_model("model.xml");
CompiledModel compiled = core.compile_model(model, "CPU");
InferRequest request = compiled.create_infer_request();
```

## 系统要求 / System Requirements

### 最低配置 / Minimum

- .NET Framework 4.6.1+, .NET Core 3.1+, or .NET 5+
- 4 GB RAM
- 2 GB available disk space

### 推荐配置 / Recommended

- .NET 8.0 or newer
- 8 GB+ RAM
- 5 GB available disk space
- CPU with AVX2 support

## 获取帮助 / Getting Help

- [问题排查 / Troubleshooting](../troubleshooting/index.md)
- [GitHub Issues](https://github.com/guojin-yan/OpenVINO-CSharp-API/issues)
