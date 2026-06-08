![OpenVINO™ C# API](https://socialify.git.ci/guojin-yan/OpenVINO-CSharp-API/image?description=1&descriptionEditable=💞%20Deploying%20Deep%20Learning%20Models%20On%20Multiple%20Platforms%20(OpenVINO/ONNX%20Runtime,%20etc.)%20💞%20&forks=1&issues=1&logo=https%3A%2F%2Fs2.loli.net%2F2023%2F01%2F26%2FylE1K5JPogMqGSW.png&name=1&owner=1&pattern=Circuit%20Board&pulls=1&stargazers=1&theme=Light)

# OpenVINO C# API

[![License](https://img.shields.io/badge/License-Apache--2.0-blue.svg)](LICENSE)
[![NuGet](https://img.shields.io/nuget/v/OpenVINO.CSharp.API.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.API/)
[![Downloads](https://img.shields.io/nuget/dt/OpenVINO.CSharp.API.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.API/)
[![.NET](https://img.shields.io/badge/.NET-4.6%20%7C%205.0%20%7C%206.0%20%7C%207.0%20%7C%208.0%20%7C%209.0%20%7C%2010.0-blue)](https://dotnet.microsoft.com/)
[![OpenVINO](https://img.shields.io/badge/OpenVINO-2025.4-orange)](https://www.intel.com/content/www/us/en/developer/tools/openvino-toolkit/overview.html)

English | [简体中文](README.md)

  Intel Distribution [OpenVINO™](www.openvino. ai) tool suite is developed based on the oneAPI and can accelerate the development speed of high-performance computer vision and deep learning visual applications. It is suitable for various Intel platforms from the edge to the cloud, helping users deploy more accurate real-world results to production systems faster. By simplifying the development workflow, OpenVINO™ Empowering developers to deploy high-performance applications and algorithms in the real world.

**OpenVINO C# API is a .NET wrapper library for Intel OpenVINO, enabling C# developers to run deep learning model inference with high performance on Windows, Linux, and macOS. Supports mainstream models like YOLO, ResNet, BERT, etc.**

The latest version currently released is **OpenVINO™ C# API 3.2 ** pre release version, which has been further updated on version 3.1, improving all test codes and addressing some errors. Relevant case projects and applications will be released based on the latest version in the future.

Finally, if you have any questions during use, you can communicate and contact me. We also welcome C # developers to join us in OpenVINO™ C# API development.

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
├─────────────────────────────────────────────────────────────────────────┤
│  Use Cases                                                               │
│  ─────────────────────────────────────────────────────────────────────  │
│  Object Detection(YOLO) │ Image Classification │ OCR │ Face Recognition │
│  Speech Synthesis │ NLP                                                  │
└─────────────────────────────────────────────────────────────────────────┘
```

## 🚀 Get Started in 30 Seconds

### 1. Install NuGet Packages

```bash
dotnet add package OpenVINO.CSharp.API
dotnet add package OpenVINO.runtime.win
(Install different runtime packages for different platforms/devices)
```

### 2. Write Inference Code

```csharp
using OpenVinoSharp;

// Load model (supports .xml/.onnx formats)
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

📚 **[View Full YOLO Samples](https://github.com/guojin-yan/OpenVINO-CSharp-API/tree/csharp3.3/samples)** (Includes four versions for .NET 4.6/4.8/Core 3.1/10.0)

---

## 📖 Detailed Documentation

| Resource | Link | Description |
|----------|------|-------------|
| **API Docs** | [guojin-yan.github.io/OpenVINO-CSharp-API](https://guojin-yan.github.io/OpenVINO-CSharp-API) | Complete class library reference |
| **Sample Code** | [samples/](https://github.com/guojin-yan/OpenVINO-CSharp-API/tree/csharp3.3/samples) | YOLO detection samples for 4 framework versions |
| **NuGet Package** | [nuget.org/packages/OpenVINO.CSharp.API](https://www.nuget.org/packages/OpenVINO.CSharp.API/) | Latest version download |

## ✨ Complete Feature List

| Feature | Description | Supported Frameworks |
|---------|-------------|---------------------|
| 🚀 **Multi-Framework** | Supports .NET Framework 4.6-4.8 and .NET 5.0-10.0 | All |
| 🖥️ **Cross-Platform** | Full support for Windows, Linux, macOS | All |
| ⚡ **High Performance** | `Span<T>`/`Memory<T>` zero-copy memory operations | .NET Core 2.1+ / .NET 4.7.2+ |
| 🔄 **Async Inference** | Complete async/await async inference support | .NET Core 3.0+ |
| 💾 **Model Caching** | Automatic caching of compiled models to avoid recompilation | All |
| 🏊 **Object Pool** | Inference request object pool to reduce creation/destruction overhead | All |
| 📝 **Complete Logging** | Configurable multi-level logging system | All |
| 🌍 **Bilingual Comments** | Complete Chinese and English XML documentation | All |

## 📦 NuGet Packages

### Core Managed Libraries

| Package | Description | Link |
| ------- | ----------- | ---- |
| **OpenVINO.CSharp.API** | OpenVINO C# API core libraries | [![NuGet Gallery](https://badge.fury.io/nu/OpenVINO.CSharp.API.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.API/) |

### Native Runtime Libraries

| Package | Description | Link |
| ------- | ----------- | ---- |
| **OpenVINO.runtime.win** | Native bindings for Windows | [![NuGet Gallery](https://badge.fury.io/nu/OpenVINO.runtime.win.svg)](https://www.nuget.org/packages/OpenVINO.runtime.win/) |
| **OpenVINO.runtime.ubuntu.24-x86_64** | Native bindings for ubuntu.24-x86_64 | [![NuGet Gallery](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.24-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.24-x86_64/) |
| **OpenVINO.runtime.ubuntu.22-x86_64** | Native bindings for ubuntu.22-x86_64 | [![NuGet Gallery](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.22-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.22-x86_64/) |
| **OpenVINO.runtime.ubuntu.20-x86_64** | Native bindings for ubuntu.20-x86_64 | [![NuGet Gallery](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.20-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.20-x86_64/) |
| **OpenVINO.runtime.ubuntu.20-arm64** | Native bindings for ubuntu.20-arm64 | [![NuGet Gallery](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.20-arm64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.20-arm64/) |
| **OpenVINO.runtime.ubuntu.18-x86_64** | Native bindings for ubuntu.18-x86_64 | [![NuGet Gallery](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.18-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.18-x86_64/) |
| **OpenVINO.runtime.ubuntu.18-arm64** | Native bindings for ubuntu.18-arm64 | [![NuGet Gallery](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.18-arm64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.18-arm64/) |
| **OpenVINO.runtime.debian10-armhf** | Native bindings for debian10-armhf | [![NuGet Gallery](https://badge.fury.io/nu/OpenVINO.runtime.debian10-armhf.svg)](https://www.nuget.org/packages/OpenVINO.runtime.debian10-armhf/) |
| **OpenVINO.runtime.debian9-arm64** | Native bindings for debian9-arm64 | [![NuGet Gallery](https://badge.fury.io/nu/OpenVINO.runtime.debian9-arm64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.debian9-arm64/) |
| **OpenVINO.runtime.debian9-armhf** | Native bindings for debian9-armhf | [![NuGet Gallery](https://badge.fury.io/nu/OpenVINO.runtime.debian9-armhf.svg)](https://www.nuget.org/packages/OpenVINO.runtime.debian9-armhf/) |
| **OpenVINO.runtime.centos7-x86_64** | Native bindings for centos7-x86_64 | [![NuGet Gallery](https://badge.fury.io/nu/OpenVINO.runtime.centos7-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.centos7-x86_64/) |
| **OpenVINO.runtime.rhel8-x86_64** | Native bindings for rhel8-x86_64 | [![NuGet Gallery](https://badge.fury.io/nu/OpenVINO.runtime.rhel8-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.rhel8-x86_64/) |
| **OpenVINO.runtime.macos-x86_64** | Native bindings for macos-x86_64 | [![NuGet Gallery](https://badge.fury.io/nu/OpenVINO.runtime.macos-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.macos-x86_64/) |
| **OpenVINO.runtime.macos-arm64** | Native bindings for macos-arm64 | [![NuGet Gallery](https://badge.fury.io/nu/OpenVINO.runtime.macos-arm64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.macos-arm64/) |

### Integration Library

| Package | Description | Link |
| ------- | ----------- | ---- |
| **OpenVINO.CSharp.Windows** | All-in-one package for Windows | [![NuGet Gallery](https://badge.fury.io/nu/OpenVINO.CSharp.Windows.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.Windows/) |

## 🚀 More Usage Examples

### Initialization and Dynamic Library Loading (Must Read)

```csharp
using OpenVinoSharp;

// Method 1: Auto-load (Recommended)
// Core will auto-load OpenVINO dynamic libraries, but you need to install runtime packages first:
//   NuGet: OpenVINO.runtime.win / OpenVINO.runtime.ubuntu / OpenVINO.runtime.macos etc.
using var core = new Core();

// Method 2: Manually specify dynamic library path (Linux/macOS custom installation)
// If runtime package not installed, or library not in default search path, initialize manually
// Linux example:
Ov.Initialize("/opt/intel/openvino/lib/openvino_c.so");
// Linux environment variable setup (add to ~/.bashrc for permanent effect):
//   export LD_LIBRARY_PATH=/opt/intel/openvino/lib:$LD_LIBRARY_PATH

// Windows example:
Ov.Initialize(".\dll\win-x64/openvino_c.dll");
```

> 💡 **Tip**: Windows usually auto-detects. For Linux/macOS, if you encounter `DllNotFoundException`, ensure the corresponding platform's `OpenVINO.runtime.xxx` NuGet package is installed, or manually specify the library path.

### Async Inference (Recommended for High Concurrency)

```csharp
// Start async inference
request.start_async();

// Wait for completion (with timeout)
bool completed = request.wait_for(5000); // 5 second timeout
if (completed)
{
    var output = request.get_output_tensor();
    // Process results...
}
```

### Using Object Pool (Batch Processing)

```csharp
// Create inference request pool
using var pool = new InferRequestPool(compiledModel, initialSize: 4, maxSize: 16);

// Execute inference using object pool
pool.RunInference(
    request => request.set_input_tensor(input),
    request => 
    {
        var output = request.get_output_tensor();
        ProcessResults(output);
    }
);
```

### Zero-Copy Tensor Operations (High Performance Mode)

```csharp
// Use Span<T> to directly access underlying memory, avoiding array copies
Span<float> data = tensor.get_span<float>();
for (int i = 0; i < data.Length; i++)
{
    data[i] = data[i] / 255.0f; // In-place normalization
}
```

## 🏗️ Project Structure

```
OpenVINO.CSharp.API/
├── src/OpenVINO.CSharp.API/    # Source code
│   ├── core/                   # Core classes (Core, Tensor, InferRequest, etc.)
│   ├── preprocess/             # Preprocessing (PrePostProcessor)
│   ├── extensions/             # Extensions (Benchmark, Utils)
│   ├── native/                 # C API P/Invoke declarations
│   └── Internal/               # Internal utility classes
├── samples/                     # Sample projects
│   ├── Yolo26Det-net4.6/       # .NET Framework 4.6 sample
│   ├── Yolo26Det-net4.8/       # .NET Framework 4.8 sample
│   ├── Yolo26Det-netcoreapp3.1/# .NET Core 3.1 sample
│   └── Yolo26Det-net10.0/      # .NET 10.0 sample
├── docs/                        # Documentation configuration
├── .github/workflows/           # CI/CD workflows
└── README.md                    # This file
```

## 🔧 Build Project

### Requirements

- .NET SDK 5.0 or higher (or Visual Studio 2019+)
- OpenVINO Runtime 2025.4+

### Build Steps

```bash
# Clone repository
git clone https://github.com/guojin-yan/OpenVINO-CSharp-API.git
cd OpenVINO-CSharp-API

# Restore dependencies
dotnet restore

# Build project
dotnet build -c Release

# Pack NuGet package
dotnet pack -c Release
```

## 📝 Logging Configuration

```csharp
using OpenVinoSharp.Internal;

// Set minimum log level
OvLogger.MinLevel = LogLevel.DEBUG;

// Enable timestamps
OvLogger.EnableTimestamp = true;

// Set custom log callback (integrate with NLog/Serilog, etc.)
OvLogger.SetCallback((level, message) =>
{
    Console.WriteLine($"[{level}] {message}");
});
```

## 🛠️ Supported Model Formats

| Format | Extension | Description |
|--------|-----------|-------------|
| **OpenVINO IR** | .xml + .bin | Recommended format, best Intel optimization |
| **ONNX** | .onnx | Universal format, supported by major frameworks |
| **PaddlePaddle** | .pdmodel | Baidu PaddlePaddle models |

## 💻 System Requirements

| Platform | Minimum Version | Supported Architectures |
|----------|-----------------|------------------------|
| Windows | Windows 10+ | x64, x86 |
| Linux | Ubuntu 18.04+ / CentOS 7+ | x64, ARM64 |
| macOS | 10.15+ | x64, ARM64 |

## 🤝 How to Contribute

Contributions via Issue and Pull Request are welcome!

1. Fork this repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is open-sourced under the [Apache-2.0 License](LICENSE).

## 🙏 Acknowledgments

- [Intel OpenVINO](https://www.intel.com/content/www/us/en/developer/tools/openvino-toolkit/overview.html) - Powerful inference framework

## 📮 Contact

- GitHub: [@guojin-yan](https://github.com/guojin-yan)
- NuGet: [OpenVINO.CSharp.API](https://www.nuget.org/packages/OpenVINO.CSharp.API/)

---

## 📢 Software Statement

**1. Open Source License Statement**

All open-source project code from the author is released under the **Apache License 2.0**.

*Special Note: This project integrates several third-party libraries. If any third-party library's license conflicts with or differs from the Apache 2.0 license, the original license of that third-party library shall prevail. This project does not contain nor represent authorization statements for these third-party libraries. Please be sure to read and comply with the relevant licenses of third-party libraries before use.*

**2. Code Development and Quality Statement**

- **AI-Assisted Development**: This code was generated and optimized using artificial intelligence (AI) assistance during development and was not entirely written manually line by line.
- **Security Commitment**: **The author solemnly declares that there are absolutely no intentionally set backdoors, viruses, trojans, or malicious code designed to damage user devices or steal data in this code.**
- **Technical Limitations**: Due to the author's personal technical level and capabilities, there may be low-level issues in the code caused by non-rigorous logic, insufficient optimization, or lack of experience (such as but not limited to memory leaks, occasional crashes, unreleased resources, etc.). These issues are purely due to insufficient capability and are not intentional.
- **Test Coverage**: Due to limited author energy, this software has not undergone comprehensive testing covering all edge cases.

**3. Disclaimer (Important)**

**Please be sure to conduct thorough and rigorous testing and verification before applying this code to any actual projects (especially commercial, industrial, or mission-critical environments).** Given the potential code defects and insufficient test coverage mentioned above, **the author is not responsible for any direct or indirect losses caused by using this code (including but not limited to equipment failure, data loss, system paralysis, or profit loss, etc.)**. Once you start using this code, it means you are aware of the above risks and agree to bear all consequences yourself. Related issues have nothing to do with this author.

**4. Code Open Source Scope**

This project promises that the core logic code is completely open source, but the binary files, source code, or related resources of the aforementioned "third-party libraries" are not within the open source obligations of this project. Please obtain them according to their respective guidelines.

**5. Community and Feedback**

Despite the above shortcomings, we still welcome everyone to download and use, submit Issues, or participate in testing to jointly improve the project. If you discover bugs, memory overflow, or have improvement suggestions during use, please contact the author through the contact information provided on the project homepage. We will try our best to provide assistance within limited time.

---

![image-20250224211044113](https://ygj-images-container.oss-cn-nanjing.aliyuncs.com/BlogGallery/202502242110187.png)

*Copyright © 2026 Guojin Yan. All Rights Reserved.*
