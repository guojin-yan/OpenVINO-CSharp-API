<picture>
  <source media="(prefers-color-scheme: dark)" srcset="docs/images/readme/hero-dark.svg">
  <source media="(prefers-color-scheme: light)" srcset="docs/images/readme/hero-light.svg">
  <img alt="OpenVINO C# API native inference" src="docs/images/readme/hero-light.svg">
</picture>

# OpenVINO C# API

[![License](https://img.shields.io/badge/License-Apache--2.0-blue.svg)](https://github.com/guojin-yan/OpenVINO-CSharp-API/blob/csharp3.3/LICENSE.txt)
[![NuGet](https://img.shields.io/nuget/v/JYPPX.OpenVINO.CSharp.API.svg)](https://www.nuget.org/packages/JYPPX.OpenVINO.CSharp.API/)
[![Downloads](https://img.shields.io/nuget/dt/JYPPX.OpenVINO.CSharp.API.svg)](https://www.nuget.org/packages/JYPPX.OpenVINO.CSharp.API/)
[![Build](https://github.com/guojin-yan/OpenVINO-CSharp-API/actions/workflows/build.yml/badge.svg?branch=csharp3.3)](https://github.com/guojin-yan/OpenVINO-CSharp-API/actions/workflows/build.yml)
[![Stars](https://img.shields.io/github/stars/guojin-yan/OpenVINO-CSharp-API?style=flat&logo=github)](https://github.com/guojin-yan/OpenVINO-CSharp-API/stargazers)
[![.NET](https://img.shields.io/badge/.NET-4.6%20%7C%205.0%20%7C%206.0%20%7C%207.0%20%7C%208.0%20%7C%209.0%20%7C%2010.0-blue)](https://dotnet.microsoft.com/)
[![OpenVINO](https://img.shields.io/badge/OpenVINO-2026.3-orange)](https://www.intel.com/content/www/us/en/developer/tools/openvino-toolkit/overview.html)

[English](README_EN.md) | 简体中文

英特尔发行版 [OpenVINO™](https://www.intel.com/content/www/us/en/developer/tools/openvino-toolkit/overview.html)工具套件基于oneAPI 而开发，可以加快高性能计算机视觉和深度学习视觉应用开发速度工具套件，适用于从边缘到云的各种英特尔平台上，帮助用户更快地将更准确的真实世界结果部署到生产系统中。通过简化的开发工作流程， OpenVINO™可赋能开发者在现实世界中部署高性能应用程序和算法。

**OpenVINO C# API 是 Intel OpenVINO 的 .NET 封装库，让 C# 开发者能够在 Windows、Linux、macOS 上高性能运行深度学习模型推理，支持 YOLO、ResNet、BERT 等主流模型。**

当前稳定版本为 **OpenVINO™ C# API 3.3.1**，配套 OpenVINO 2026.3 runtime。

## 📢 3.3.1 更新摘要

- 对齐 OpenVINO Core 2026.3，现有 Core 推理 API 保持兼容；
- 提供可选 `OpenVinoSharp.GenAI` 封装，覆盖 LLM、Whisper 和 VLM；
- VLM 多轮对话使用 `ChatHistory` 与 `GenerateWithHistory()`；
- 提供 Windows、Linux 和 macOS 对应的 Core/GenAI 2026.3 runtime 包。

[查看 3.3.1 完整版本说明](https://github.com/guojin-yan/OpenVINO-CSharp-API/blob/csharp3.3/docs/release-notes/3.3.1.md) · [查看全部版本记录](https://github.com/guojin-yan/OpenVINO-CSharp-API/tree/csharp3.3/docs/release-notes)

最后，如果各位在使用中有什么问题，可以与我沟通联系，也欢迎广大C#开发者加入到OpenVINO™ C# API 开发中。

```
┌─────────────────────────────────────────────────────────────────────────┐
│  一句话了解                                                              │
│  ─────────────────────────────────────────────────────────────────────  │
│  用 C# 跑 AI 模型推理，跨平台、高性能、支持 .NET 4.6 到 .NET 10.0        │
├─────────────────────────────────────────────────────────────────────────┤
│  四大核心优势                                                            │
│  ─────────────────────────────────────────────────────────────────────  │
│  🚀 高性能  │  Span<T> 零拷贝内存，推理速度媲美 Python/C++              │
│  🖥️ 跨平台  │  Windows/Linux/macOS，x64/ARM64 全支持                   │
│  🔄 异步化  │  async/await 异步推理，轻松应对高并发场景                  │
│  🔌 多设备  │  Intel CPU/iGPU/GPU/NPU，AMD CPU（部分支持）              │
├─────────────────────────────────────────────────────────────────────────┤
│  适用场景                                                                │
│  ─────────────────────────────────────────────────────────────────────  │
│  目标检测(YOLO) │ 图像分类(ResNet) │ OCR │ 人脸识别 │ 语音合成 │ NLP     │
└─────────────────────────────────────────────────────────────────────────┘
```

## 🚀 30 秒上手

### 1. 安装 NuGet 包

```bash
dotnet add package JYPPX.OpenVINO.CSharp.API
dotnet add package OpenVINO.runtime.win
(第二个包在不同平台设备需要安装不同的包)
```

如果需要使用 `OpenVinoSharp.GenAI`，请安装对应平台的 GenAI runtime 包，例如：

```bash
dotnet add package JYPPX.OpenVINO.GenAI.runtime.win
```

### 2. 写推理代码

```csharp
using OpenVinoSharp;

// 加载模型（支持 .xml/.onnx等格式）
using var core = new Core();
var model = core.compile_model("yolov8n.xml", "CPU");

// 创建推理请求并执行
using var request = model.create_infer_request();
request.set_input_tensor(new Tensor(shape, imageData));
request.infer();

// 获取检测结果
var output = request.get_output_tensor().get_data<float>();
```

### 3. 运行程序

```bash
dotnet run
```

📚 **[查看完整 YOLO 案例](https://github.com/guojin-yan/OpenVINO-CSharp-API/tree/csharp3.3/samples)**（包含 .NET 4.6/4.8/Core 3.1/10.0 四个版本）

---

## 📖 详细文档

| 资源 | 链接 | 说明 |
|------|------|------|
| **API 文档** | [guojin-yan.github.io/OpenVINO-CSharp-API](https://guojin-yan.github.io/OpenVINO-CSharp-API) | 完整的类库参考 |
| **案例源码** | [samples/](https://github.com/guojin-yan/OpenVINO-CSharp-API/tree/csharp3.3/samples) | 4 个框架版本的 YOLO 检测案例 |
| **NuGet 包** | [nuget.org/packages/JYPPX.OpenVINO.CSharp.API](https://www.nuget.org/packages/JYPPX.OpenVINO.CSharp.API/) | 最新版本下载 |



## ✨ 完整特性列表

| 特性 | 说明 | 适用框架 |
|------|------|----------|
| 🚀 **多框架支持** | 支持 .NET Framework 4.6-4.8 和 .NET 5.0-10.0 | 全部 |
| 🖥️ **跨平台** | Windows、Linux、macOS 全平台支持 | 全部 |
| ⚡ **高性能** | `Span<T>`/`Memory<T>` 零拷贝内存操作 | .NET Core 2.1+ / .NET 4.7.2+ |
| 🔄 **异步推理** | 完整的 async/await 异步推理支持 | .NET Core 3.0+ |
| 💾 **模型缓存** | 自动缓存编译后的模型，避免重复编译 | 全部 |
| 🏊 **对象池** | 推理请求对象池，减少频繁创建销毁的开销 | 全部 |
| 📝 **完整日志** | 可配置的多级别日志系统 | 全部 |
| 🌍 **双语注释** | 完整的中英文 XML 文档注释 | 全部 |

## 📦 NuGet Package

### Core Managed Libraries

| Package                 | Description                    | Link                                                         |
| ----------------------- | ------------------------------ | ------------------------------------------------------------ |
| **JYPPX.OpenVINO.CSharp.API** | OpenVINO C# API core libraries | [![NuGet Gallery ](https://badge.fury.io/nu/JYPPX.OpenVINO.CSharp.API.svg)](https://www.nuget.org/packages/JYPPX.OpenVINO.CSharp.API/) |

### Native Runtime Libraries

| Package                               | Description                                           | Link                                                         |
| ------------------------------------- | ----------------------------------------------------- | ------------------------------------------------------------ |
| **OpenVINO.runtime.win**              | Native bindings for Windows                           | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.win.svg)](https://www.nuget.org/packages/OpenVINO.runtime.win/) |
| **JYPPX.OpenVINO.GenAI.runtime.win**  | Native OpenVINO GenAI runtime for Windows             | GitHub workflow: `docs/articles/installation/genai-runtime.md` |
| **JYPPX.OpenVINO.GenAI.runtime.ubuntu.24-x86_64** | Native OpenVINO GenAI runtime for Ubuntu 24 x86_64 | GitHub workflow: `docs/articles/installation/genai-runtime.md` |
| **JYPPX.OpenVINO.GenAI.runtime.ubuntu.22-x86_64** | Native OpenVINO GenAI runtime for Ubuntu 22 x86_64 | GitHub workflow: `docs/articles/installation/genai-runtime.md` |
| **JYPPX.OpenVINO.GenAI.runtime.ubuntu.22-arm64** | Native OpenVINO GenAI runtime for Ubuntu 22 ARM64 | GitHub workflow: `docs/articles/installation/genai-runtime.md` |
| **JYPPX.OpenVINO.GenAI.runtime.rhel8-x86_64** | Native OpenVINO GenAI runtime for RHEL 8 x86_64 | GitHub workflow: `docs/articles/installation/genai-runtime.md` |
| **JYPPX.OpenVINO.GenAI.runtime.macos-arm64** | Native OpenVINO GenAI runtime for macOS ARM64 | GitHub workflow: `docs/articles/installation/genai-runtime.md` |
| **OpenVINO.runtime.ubuntu.24-x86_64** | Native bindings for ubuntu.24-x86_64                  | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.24-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.24-x86_64/) |
| **OpenVINO.runtime.ubuntu.22-x86_64** | Native bindings for ubuntu.22-x86_64                  | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.22-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.22-x86_64/) |
| **OpenVINO.runtime.ubuntu.20-x86_64** | Native bindings for ubuntu.20-x86_64                  | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.20-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.20-x86_64/) |
| **OpenVINO.runtime.ubuntu.20-arm64**  | Native bindings for ubuntu.20-arm64                   | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.20-arm64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.20-arm64/) |
| **OpenVINO.runtime.ubuntu.18-x86_64** | Native bindings for ubuntu.18-x86_64                  | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.18-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.18-x86_64/) |
| **OpenVINO.runtime.ubuntu.18-arm64**  | Native bindings for uOpenVINO.runtime.ubuntu.18-arm64 | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.18-arm64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.18-arm64/) |
| **OpenVINO.runtime.debian10-armhf**   | Native bindings for debian10-armhf                    | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.debian10-armhf.svg)](https://www.nuget.org/packages/OpenVINO.runtime.debian10-armhf/) |
| **OpenVINO.runtime.debian9-arm64**    | Native bindings for debian9-arm64                     | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.debian9-arm64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.debian9-arm64/) |
| **OpenVINO.runtime.debian9-armhf**    | Native bindings for debian9-armhf                     | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.debian9-armhf.svg)](https://www.nuget.org/packages/OpenVINO.runtime.debian9-armhf/) |
| **OpenVINO.runtime.centos7-x86_64**   | Native bindings for centos7-x86_64                    | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.centos7-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.centos7-x86_64/) |
| **OpenVINO.runtime.rhel8-x86_64**     | Native bindings for rhel8-x86_64                      | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.rhel8-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.rhel8-x86_64/) |
| **OpenVINO.runtime.macos-x86_64**     | Native bindings for macos-x86_64                      | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.macos-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.macos-x86_64/) |
| **OpenVINO.runtime.macos-arm64**      | Native bindings for macos-arm64                       | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.macos-arm64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.macos-arm64/) |

GenAI runtime packages are published only for official archives that also have
a valid `.sha256` sidecar. For OpenVINO 2026.3, the core matrix currently
contains Windows, Ubuntu 24/22 x86_64, Ubuntu 22 ARM64, CentOS 8, RHEL 8, and
macOS ARM64. The GenAI matrix contains Windows, Ubuntu 24/22 x86_64, Ubuntu 22
ARM64, RHEL 8, and macOS ARM64; platforms without a valid official checksum are
intentionally skipped.


### Integration Library

| Package                     | Description                    | Link                                                         |
| --------------------------- | ------------------------------ | ------------------------------------------------------------ |
| **OpenVINO.CSharp.Windows** | All-in-one package for Windows | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.CSharp.Windows.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.Windows/) |

## 🚀 更多使用示例

### 初始化与动态库加载（必看）

```csharp
using OpenVinoSharp;

// 方式1：自动加载（推荐）
// Core 会自动加载 OpenVINO 动态库，但需要先安装运行时包：
//   NuGet: OpenVINO.runtime.win / OpenVINO.runtime.ubuntu / OpenVINO.runtime.macos等，具体根据自己设备安装
using var core = new Core();

// 方式2：手动指定动态库路径（Linux/macOS 等自定义安装场景）
// 如果未安装运行时包，或动态库不在默认搜索路径，需手动初始化
// Linux 示例：
Ov.Initialize("/opt/intel/openvino/lib/openvino_c.so");  
// Linux 环境变量设置（如需永久生效，添加到 ~/.bashrc）：
//   export LD_LIBRARY_PATH=/opt/intel/openvino/lib:$LD_LIBRARY_PATH

// Windows 示例：
Ov.Initialize(".\dll\win-x64/openvino_c.dll");  
```

> 💡 **提示**：Windows 通常自动识别，Linux/macOS 若遇到 `DllNotFoundException`，请确保已安装对应平台的 `OpenVINO.runtime.xxx` NuGet 包，或手动指定库路径。

### 异步推理（推荐用于高并发）

```csharp
// 启动异步推理
request.start_async();

// 等待完成（带超时）
bool completed = request.wait_for(5000); // 5秒超时
if (completed)
{
    var output = request.get_output_tensor();
    // 处理结果...
}
```

### 使用对象池（批量处理场景）

```csharp
// 创建推理请求池
using var pool = new InferRequestPool(compiledModel, initialSize: 4, maxSize: 16);

// 使用对象池执行推理
pool.RunInference(
    request => request.set_input_tensor(input),
    request => 
    {
        var output = request.get_output_tensor();
        ProcessResults(output);
    }
);
```

### 零拷贝 Tensor 操作（高性能模式）

```csharp
// 使用 Span<T> 直接访问底层内存，避免数组拷贝
Span<float> data = tensor.get_span<float>();
for (int i = 0; i < data.Length; i++)
{
    data[i] = data[i] / 255.0f; // 原地归一化
}
```

## 🏗️ 项目结构

```
OpenVINO.CSharp.API/
├── src/OpenVINO.CSharp.API/    # 源代码
│   ├── core/                   # 核心类 (Core, Tensor, InferRequest等)
│   ├── preprocess/             # 预处理 (PrePostProcessor)
│   ├── extensions/             # 扩展功能 (Benchmark, Utils)
│   ├── native/                 # C API P/Invoke 声明
│   └── Internal/               # 内部工具类
├── samples/                     # 示例项目
│   ├── Yolo26Det-net4.6/       # .NET Framework 4.6 示例
│   ├── Yolo26Det-net4.8/       # .NET Framework 4.8 示例
│   ├── Yolo26Det-netcoreapp3.1/# .NET Core 3.1 示例
│   └── Yolo26Det-net10.0/      # .NET 10.0 示例
├── docs/                        # 文档配置
├── .github/workflows/           # CI/CD 工作流
└── README.md                    # 本文件
```

## 🔧 构建项目

### 环境要求

- .NET SDK 5.0 或更高版本（或 Visual Studio 2019+）
- OpenVINO Runtime 2026.3+

### 构建步骤

```bash
# 克隆仓库
git clone https://github.com/guojin-yan/OpenVINO-CSharp-API.git
cd OpenVINO-CSharp-API

# 还原依赖
dotnet restore

# 构建项目
dotnet build -c Release

# 打包 NuGet 包
dotnet pack -c Release
```

## 📝 日志配置

```csharp
using OpenVinoSharp.Internal;

// 设置最小日志级别
OvLogger.MinLevel = LogLevel.DEBUG;

// 启用时间戳
OvLogger.EnableTimestamp = true;

// 设置自定义日志回调（集成 NLog/Serilog 等）
OvLogger.SetCallback((level, message) =>
{
    Console.WriteLine($"[{level}] {message}");
});
```

## 🛠️ 支持的模型格式

| 格式 | 扩展名 | 说明 |
|------|--------|------|
| **OpenVINO IR** | .xml + .bin | 推荐格式，Intel 优化最佳 |
| **ONNX** | .onnx | 通用格式，主流框架都支持导出 |
| **PaddlePaddle** | .pdmodel | 百度飞桨模型 |

## 💻 系统要求

| 平台 | 最低版本 | 支持架构 |
|-----|---------|----------|
| Windows | Windows 10+ | x64, x86 |
| Linux | Ubuntu 18.04+ / CentOS 7+ | x64, ARM64 |
| macOS | 10.15+ | x64, ARM64 |

## 🤝 如何贡献

欢迎提交 Issue 和 Pull Request！

1. Fork 本仓库
2. 创建你的特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 打开 Pull Request

## 📄 许可证

本项目采用 [Apache-2.0 License](https://github.com/guojin-yan/OpenVINO-CSharp-API/blob/csharp3.3/LICENSE.txt) 开源。

## 🙏 致谢

- [Intel OpenVINO](https://www.intel.com/content/www/us/en/developer/tools/openvino-toolkit/overview.html) - 强大的推理框架

## 📮 联系与赞助

- GitHub: [@guojin-yan](https://github.com/guojin-yan)
- NuGet: [JYPPX.OpenVINO.CSharp.API](https://www.nuget.org/packages/JYPPX.OpenVINO.CSharp.API/)

<p align="center">
  <img src="docs/images/readme/contact-support-zh.png" width="100%" alt="开发者联系地图、社区入口与微信和支付宝赞助二维码">
</p>

---

## ⚠️ 软件声明与免责声明

### 📜 1. 开源协议声明

作者所有开源项目代码均遵循 **Apache License 2.0** 开源协议。 

*特别说明：本项目集成了若干第三方库。若任何第三方库的许可协议与 Apache 2.0 协议存在冲突或不一致，均以该第三方库的原始许可协议为准。本项目不包含也不代表这些第三方库的授权声明，使用前请务必阅读并遵守第三方库的相关许可。*

### 🤖 2. 代码开发与质量说明

- **AI 辅助开发**：本代码在开发过程中使用了人工智能（AI）辅助生成与优化，并非完全由人工逐行编写。
- **安全性承诺**：**作者郑重声明，本代码中绝无任何有意设置的后门、病毒、木马或旨在破坏用户设备、窃取数据的恶意代码。**
- **技术局限性**：受限于作者个人的技术水平与能力，代码中可能存在因逻辑不严谨、优化不足或经验欠缺导致的低级问题（例如但不限于内存泄漏、偶发崩溃、资源未释放等）。这些问题纯属能力不足所致，并非主观故意。
- **测试范围**：由于作者精力有限，未对本软件进行全方位、覆盖所有边缘场景的完整测试。

### 🚨 3. 免责声明（重要）

**请在将本代码应用于任何实际项目（特别是商业、工业或关键任务环境）之前，务必进行详尽、严格的自行测试与验证。** 鉴于上述可能存在的代码缺陷及测试覆盖不足，**因使用本代码而导致的任何直接或间接损失（包括但不限于设备故障、数据丢失、系统瘫痪或利润损失等），本作者概不负责。** 一旦您开始使用本代码，即表示您已知晓上述风险并同意自行承担一切后果，相关问题与本作者无关。

### 🔓 4. 代码开源范围

本项目承诺核心逻辑代码完全开源，但上述提到的“第三方库”的二进制文件、源代码或相关资源不在本项目的开源义务范围内，请根据其各自的指引获取。

### 🤝 5. 社区与反馈

尽管存在上述不足，我们仍欢迎大家下载使用、提交 Issue 或参与测试，共同完善项目。如果您在使用过程中发现 Bug、内存溢出或有改进建议，欢迎通过项目主页提供的联系方式与作者取得联系，我们将尽力在有限的时间内提供协助。

*Copyright © 2026 Guojin Yan. All Rights Reserved.*
