# OpenVINO C# API

[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![NuGet](https://img.shields.io/nuget/v/JYPPX.OpenVINO.CSharp.API.svg)](https://www.nuget.org/packages/JYPPX.OpenVINO.CSharp.API/)
[![Downloads](https://img.shields.io/nuget/dt/JYPPX.OpenVINO.CSharp.API.svg)](https://www.nuget.org/packages/JYPPX.OpenVINO.CSharp.API/)
[![.NET](https://img.shields.io/badge/.NET-4.6%20%7C%205.0%20%7C%206.0%20%7C%207.0%20%7C%208.0%20%7C%209.0%20%7C%2010.0-blue)](https://dotnet.microsoft.com/)
[![OpenVINO](https://img.shields.io/badge/OpenVINO-2025.4-orange)](https://www.intel.com/content/www/us/en/developer/tools/openvino-toolkit/overview.html)

English | [简体中文](README.md)

OpenVINO C# API is a .NET wrapper library based on OpenVINO C API, enabling deep learning model inference using Intel OpenVINO in C#.

## ✨ Features

- 🚀 **Multi-Framework Support** - Supports .NET Framework 4.6-4.8 and .NET 5.0-10.0
- 🖥️ **Cross-Platform** - Full support for Windows, Linux, and macOS
- ⚡ **High Performance** - Zero-copy memory operations with `Span<T>` and `Memory<T>`
- 🔄 **Async Inference** - Complete async/await support for asynchronous inference
- 💾 **Model Caching** - Automatic caching of compiled models to avoid recompilation
- 🏊 **Object Pooling** - Inference request pool to reduce allocation overhead
- 📝 **Complete Logging** - Configurable multi-level logging system
- 🌍 **Bilingual Documentation** - Complete Chinese and English XML documentation

## 📦 Installation

### NuGet Package Manager

```bash
dotnet add package JYPPX.OpenVINO.CSharp.API
```

### PackageReference

```xml
<PackageReference Include="JYPPX.OpenVINO.CSharp.API" Version="4.0.0" />
```

## 🚀 Quick Start

### Basic Inference Example

```csharp
using OpenVinoSharp;

// Create OpenVINO runtime core
using var core = new Core();

// Load and compile model (with automatic caching)
var compiledModel = core.compile_model("model.xml", "CPU");

// Create inference request
using var request = compiledModel.create_infer_request();

// Prepare input data
float[] inputData = LoadInputData(); // Your data loading logic
var inputTensor = new Tensor(shape, inputData);
request.set_input_tensor(inputTensor);

// Run inference
request.infer();

// Get output results
var outputTensor = request.get_output_tensor();
float[] results = outputTensor.get_float_data();
```

### Async Inference Example

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

### Using Object Pool (High Concurrency Scenarios)

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

### Zero-Copy Tensor Operations (.NET Core 2.1+ / .NET 5+)

```csharp
// Use Span<T> to directly access underlying memory without copying
Span<float> data = tensor.get_span<float>();
for (int i = 0; i < data.Length; i++)
{
    data[i] = data[i] * 2.0f; // Modify directly
}
```

## 📚 Documentation

- **[API Documentation](https://guojin-yan.github.io/OpenVINO-CSharp-API)** - Complete API reference
- **[Usage Samples](https://github.com/guojin-yan/OpenVINO-CSharp-API/tree/main/samples)** - Detailed usage examples
- **[NuGet Package](https://www.nuget.org/packages/JYPPX.OpenVINO.CSharp.API/)** - NuGet package page

## 🏗️ Project Structure

```
OpenVINO.CSharp.API/
├── src/OpenVINO.CSharp.API/    # Source code
│   ├── core/                   # Core classes (Core, Tensor, InferRequest, etc.)
│   ├── preprocess/             # Preprocessing (PrePostProcessor)
│   ├── extensions/             # Extensions (Benchmark, Utils)
│   ├── native/                 # C API P/Invoke declarations
│   └── Internal/               # Internal utility classes
├── docs/                        # Documentation configuration
├── .github/workflows/           # CI/CD workflows
└── README.md                    # This file
```

## 🔧 Building

### Requirements

- .NET SDK 5.0 or higher (or Visual Studio 2019+)
- OpenVINO Runtime 2025.4+

### Build Steps

```bash
# Clone repository
git clone https://github.com/guojin-yan/OpenVINO-CSharp-API.git
cd OpenVINO-CSharp-API/OpenVINO.CSharp.API

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
Logger.MinLevel = LogLevel.DEBUG;

// Enable timestamps
Logger.EnableTimestamp = true;

// Set custom log callback (integrate with NLog/Serilog, etc.)
Logger.SetCallback((level, message) =>
{
    // Your logging logic
    Console.WriteLine($"[{level}] {message}");
});
```

## 🛠️ Supported Model Formats

- **OpenVINO IR** (.xml + .bin) - Recommended format
- **ONNX** (.onnx)
- **TensorFlow** (.pb)
- **TensorFlow Lite** (.tflite)
- **PaddlePaddle** (.pdmodel)

## 💻 System Requirements

| Platform | Minimum Version | Architecture |
|---------|----------------|--------------|
| Windows | Windows 7 SP1+ | x64, x86 |
| Linux | Ubuntu 18.04+ | x64, ARM64 |
| macOS | 10.15+ | x64, ARM64 |

## 🤝 Contributing

Issues and Pull Requests are welcome!

1. Fork this repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the [MIT License](LICENSE).

## 🙏 Acknowledgements

- [Intel OpenVINO](https://www.intel.com/content/www/us/en/developer/tools/openvino-toolkit/overview.html) - Powerful inference toolkit
- [OpenVINO C API](https://docs.openvino.ai/2025.4/api/c_cpp_api/group__ov__c__api.html) - C API Documentation

## 📮 Contact

- GitHub: [@guojin-yan](https://github.com/guojin-yan)
- NuGet: [JYPPX.OpenVINO.CSharp.API](https://www.nuget.org/packages/JYPPX.OpenVINO.CSharp.API/)

---

*Copyright © 2024 Guojin Yan. All Rights Reserved.*
