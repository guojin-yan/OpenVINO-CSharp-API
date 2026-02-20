---
uid: index
---

# OpenVINO C# API

[![License](https://img.shields.io/badge/License-Apache--2.0-blue.svg)](LICENSE)
[![NuGet](https://img.shields.io/nuget/v/OpenVINO.CSharp.API.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.API/)
[![Downloads](https://img.shields.io/nuget/dt/OpenVINO.CSharp.API.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.API/)
[![.NET](https://img.shields.io/badge/.NET-4.6%20%7C%205.0%20%7C%206.0%20%7C%207.0%20%7C%208.0%20%7C%209.0%20%7C%2010.0-blue)](https://dotnet.microsoft.com/)
[![OpenVINO](https://img.shields.io/badge/OpenVINO-2025.4-orange)](https://www.intel.com/content/www/us/en/developer/tools/openvino-toolkit/overview.html)

**OpenVINO C# API is a .NET wrapper library for Intel OpenVINO, enabling C# developers to run deep learning model inference with high performance on Windows, Linux, and macOS.**

```
┌─────────────────────────────────────────────────────────────────────────┐
│  One Sentence Summary                                                    │
│  ─────────────────────────────────────────────────────────────────────  │
│  Run AI model inference in C#, cross-platform, high-performance,         │
│  supporting .NET 4.6 to .NET 10.0                                        │
├─────────────────────────────────────────────────────────────────────────┤
│  Four Core Advantages                                                    │
│  ─────────────────────────────────────────────────────────────────────  │
│  🚀 High Performance  │  Span<T> zero-copy memory, speed rivals Python/C++│
│  🖥️ Cross-Platform    │  Windows/Linux/macOS, x64/ARM64 full support     │
│  🔄 Async Support     │  async/await async inference for high concurrency│
│  🔌 Multi-Device      │  Intel CPU/iGPU/GPU/NPU, AMD CPU (partial)       │
└─────────────────────────────────────────────────────────────────────────┘
```

## 🚀 Get Started in 30 Seconds

### 1. Install NuGet Packages

```bash
dotnet add package OpenVINO.CSharp.API
dotnet add package OpenVINO.runtime.win
```

### 2. Write Inference Code

```csharp
using OpenVinoSharp;

// Load model
using var core = new Core();
var model = core.compile_model("yolov8n.xml", "CPU");

// Create inference request and execute
using var request = model.create_infer_request();
request.set_input_tensor(new Tensor(shape, imageData));
request.infer();

// Get detection results
var output = request.get_output_tensor().get_data<float>();
```

### 3. Run the Program

```bash
dotnet run
```

📚 **[View Full Samples](articles/samples/index.md)** (Includes .NET 4.6/4.8/Core 3.1/10.0 versions)

## 📖 Documentation

- **[API Reference](api/OpenVinoSharp.yml)** - Complete class library reference
- **[Samples](articles/samples/index.md)** - YOLO detection and other examples

## ✨ Features

| Feature | Description | Supported Frameworks |
|---------|-------------|---------------------|
| 🚀 **Multi-Framework** | Supports .NET Framework 4.6-4.8 and .NET 5.0-10.0 | All |
| 🖥️ **Cross-Platform** | Full support for Windows, Linux, macOS | All |
| ⚡ **High Performance** | `Span<T>`/`Memory<T>` zero-copy memory operations | .NET Core 2.1+ / .NET 4.7.2+ |
| 🔄 **Async Inference** | Complete async/await async inference support | .NET Core 3.0+ |
| 💾 **Model Caching** | Automatic caching of compiled models | All |
| 🏊 **Object Pool** | Inference request object pool for high concurrency | All |
| 📝 **Logging** | Configurable multi-level logging system | All |
| 🌍 **Bilingual Docs** | Complete Chinese and English XML documentation | All |

## 📦 NuGet Packages

### Core Library

```bash
dotnet add package OpenVINO.CSharp.API
```

### Runtime Libraries (Choose by Platform)

- `OpenVINO.runtime.win` - Windows
- `OpenVINO.runtime.ubuntu.22-x86_64` - Ubuntu 22.04
- `OpenVINO.runtime.macos-arm64` - macOS ARM64

## 🛠️ Supported Model Formats

| Format | Extension | Description |
|--------|-----------|-------------|
| **OpenVINO IR** | .xml + .bin | Recommended, best Intel optimization |
| **ONNX** | .onnx | Universal format |
| **PaddlePaddle** | .pdmodel | Baidu PaddlePaddle |

## 💻 System Requirements

| Platform | Minimum Version | Architectures |
|----------|-----------------|---------------|
| Windows | Windows 10+ | x64, x86 |
| Linux | Ubuntu 18.04+ / CentOS 7+ | x64, ARM64 |
| macOS | 10.15+ | x64, ARM64 |

## 📄 License

[Apache-2.0 License](LICENSE) © 2026 Guojin Yan
