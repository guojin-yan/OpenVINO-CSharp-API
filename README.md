# OpenVINO C# API

[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![NuGet](https://img.shields.io/nuget/v/JYPPX.OpenVINO.CSharp.API.svg)](https://www.nuget.org/packages/JYPPX.OpenVINO.CSharp.API/)
[![Downloads](https://img.shields.io/nuget/dt/JYPPX.OpenVINO.CSharp.API.svg)](https://www.nuget.org/packages/JYPPX.OpenVINO.CSharp.API/)
[![.NET](https://img.shields.io/badge/.NET-4.6%20%7C%205.0%20%7C%206.0%20%7C%207.0%20%7C%208.0%20%7C%209.0%20%7C%2010.0-blue)](https://dotnet.microsoft.com/)
[![OpenVINO](https://img.shields.io/badge/OpenVINO-2025.4-orange)](https://www.intel.com/content/www/us/en/developer/tools/openvino-toolkit/overview.html)

[English](README_EN.md) | 简体中文

OpenVINO C# API 是一个基于 OpenVINO C API 的 .NET 包装库，支持在 C# 中使用 Intel OpenVINO 进行深度学习模型推理。

## ✨ 特性

- 🚀 **多框架支持** - 支持 .NET Framework 4.6-4.8 和 .NET 5.0-10.0
- 🖥️ **跨平台** - Windows、Linux、macOS 全平台支持
- ⚡ **高性能** - 支持 `Span<T>`、`Memory<T>` 零拷贝内存操作
- 🔄 **异步推理** - 完整的 async/await 异步推理支持
- 💾 **模型缓存** - 自动缓存编译后的模型，避免重复编译
- 🏊 **对象池** - 推理请求对象池，减少频繁创建销毁的开销
- 📝 **完整日志** - 可配置的多级别日志系统
- 🌍 **双语注释** - 完整的中英文 XML 文档注释

## 📦 安装

### NuGet 包管理器

```bash
dotnet add package JYPPX.OpenVINO.CSharp.API
```

### PackageReference

```xml
<PackageReference Include="JYPPX.OpenVINO.CSharp.API" Version="4.0.0" />
```

## 🚀 快速开始

### 基本推理示例

```csharp
using OpenVinoSharp;

// 创建 OpenVINO 运行时核心
using var core = new Core();

// 加载并编译模型（自动缓存）
var compiledModel = core.compile_model("model.xml", "CPU");

// 创建推理请求
using var request = compiledModel.create_infer_request();

// 准备输入数据
float[] inputData = LoadInputData(); // 你的数据加载逻辑
var inputTensor = new Tensor(shape, inputData);
request.set_input_tensor(inputTensor);

// 执行推理
request.infer();

// 获取输出结果
var outputTensor = request.get_output_tensor();
float[] results = outputTensor.get_float_data();
```

### 异步推理示例

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

### 使用对象池（高并发场景）

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

### 零拷贝 Tensor 操作（.NET Core 2.1+ / .NET 5+）

```csharp
// 使用 Span<T> 直接访问底层内存，避免拷贝
Span<float> data = tensor.get_span<float>();
for (int i = 0; i < data.Length; i++)
{
    data[i] = data[i] * 2.0f; // 直接修改
}
```

## 📚 文档

- **[API 文档](https://guojin-yan.github.io/OpenVINO-CSharp-API)** - 完整的 API 参考文档
- **[使用示例](https://github.com/guojin-yan/OpenVINO-CSharp-API/tree/main/samples)** - 详细的使用示例代码
- **[NuGet 包](https://www.nuget.org/packages/JYPPX.OpenVINO.CSharp.API/)** - NuGet 包页面

## 🏗️ 项目结构

```
OpenVINO.CSharp.API/
├── src/OpenVINO.CSharp.API/    # 源代码
│   ├── core/                   # 核心类 (Core, Tensor, InferRequest等)
│   ├── preprocess/             # 预处理 (PrePostProcessor)
│   ├── extensions/             # 扩展功能 (Benchmark, Utils)
│   ├── native/                 # C API P/Invoke 声明
│   └── Internal/               # 内部工具类
├── docs/                        # 文档配置
├── .github/workflows/           # CI/CD 工作流
└── README.md                    # 本文件
```

## 🔧 构建

### 环境要求

- .NET SDK 5.0 或更高版本（或 Visual Studio 2019+）
- OpenVINO Runtime 2025.4+

### 构建步骤

```bash
# 克隆仓库
git clone https://github.com/guojin-yan/OpenVINO-CSharp-API.git
cd OpenVINO-CSharp-API/OpenVINO.CSharp.API

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
Logger.MinLevel = LogLevel.DEBUG;

// 启用时间戳
Logger.EnableTimestamp = true;

// 设置自定义日志回调（集成 NLog/Serilog 等）
Logger.SetCallback((level, message) =>
{
    // 你的日志处理逻辑
    Console.WriteLine($"[{level}] {message}");
});
```

## 🛠️ 支持的模型格式

- **OpenVINO IR** (.xml + .bin) - 推荐格式
- **ONNX** (.onnx)
- **TensorFlow** (.pb)
- **TensorFlow Lite** (.tflite)
- **PaddlePaddle** (.pdmodel)

## 💻 系统要求

| 平台 | 最低版本 | 架构 |
|-----|---------|------|
| Windows | Windows 7 SP1+ | x64, x86 |
| Linux | Ubuntu 18.04+ | x64, ARM64 |
| macOS | 10.15+ | x64, ARM64 |

## 🤝 贡献

欢迎提交 Issue 和 Pull Request！

1. Fork 本仓库
2. 创建你的特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 打开 Pull Request

## 📄 许可证

本项目采用 [MIT 许可证](LICENSE) 开源。

## 🙏 致谢

- [Intel OpenVINO](https://www.intel.com/content/www/us/en/developer/tools/openvino-toolkit/overview.html) - 强大的推理框架
- [OpenVINO C API](https://docs.openvino.ai/2025.4/api/c_cpp_api/group__ov__c__api.html) - C API 文档

## 📮 联系方式

- GitHub: [@guojin-yan](https://github.com/guojin-yan)
- NuGet: [JYPPX.OpenVINO.CSharp.API](https://www.nuget.org/packages/JYPPX.OpenVINO.CSharp.API/)

---

*Copyright © 2024 Guojin Yan. All Rights Reserved.*
