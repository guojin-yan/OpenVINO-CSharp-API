# 安装方式 / Installation

本文档介绍如何在不同平台上安装和配置 OpenVINO C# API。

## 支持的平台 / Supported Platforms

| 平台 / Platform | 状态 / Status | 说明 / Description |
|----------------|---------------|-------------------|
| Windows | ✅ 支持 | Windows 10/11, Windows Server 2019/2022 |
| Linux | 📝 待添加 | Ubuntu 20.04/22.04, CentOS 等 |
| macOS | 📝 待添加 | macOS 11+ (Intel/Apple Silicon) |

## 安装方式概览 / Installation Methods

### Windows

- [Windows 安装指南](windows.md) - 在 Windows 平台上安装 OpenVINO C# API

### Linux

- [Linux 安装指南](linux.md) - 在 Linux 平台上安装 OpenVINO C# API

### macOS

- [macOS 安装指南](macos.md) - 在 macOS 平台上安装 OpenVINO C# API

## 快速开始 / Quick Start

### NuGet 包安装

```bash
dotnet add package OpenVINO.CSharp.API
dotnet add package OpenVINO.CSharp.API.Extensions.OpenCvSharp4
dotnet add package OpenVINO.runtime.win
```

### 基础用法

```csharp
using OpenVinoSharp;

// 创建 Core
using Core core = new Core();

// 加载模型
Model model = core.read_model("model.xml");

// 编译模型
CompiledModel compiled = core.compile_model(model, "CPU");

// 创建推理请求
InferRequest request = compiled.create_infer_request();

// 准备输入数据并执行推理
// ...
```

## 系统要求 / System Requirements

### 最低配置

- **.NET 版本**: .NET Framework 4.6.1+ 或 .NET Core 3.1+ 或 .NET 5+
- **内存**: 4GB RAM
- **磁盘空间**: 2GB 可用空间

### 推荐配置

- **.NET 版本**: .NET 8.0 或 .NET 10.0
- **内存**: 8GB+ RAM
- **磁盘空间**: 5GB 可用空间
- **处理器**: 支持 AVX2 的 CPU

## 获取帮助 / Getting Help

如果在安装过程中遇到问题，请查阅：

- [问题排查 / Troubleshooting](../troubleshooting/)
- [GitHub Issues](https://github.com/guojin-yan/OpenVINO-CSharp-API/issues)
