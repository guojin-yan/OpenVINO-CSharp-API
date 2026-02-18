# OpenVINO C# API 文档

[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-4.6%20%7C%205.0%20%7C%206.0%20%7C%207.0%20%7C%208.0%20%7C%209.0%20%7C%2010.0-blue)](https://dotnet.microsoft.com/)
[![OpenVINO](https://img.shields.io/badge/OpenVINO-2025.4-orange)](https://www.intel.com/content/www/us/en/developer/tools/openvino-toolkit/overview.html)

OpenVINO C# API 是一个基于 OpenVINO C API 的 .NET 包装库，支持在 C# 中使用 Intel OpenVINO 进行深度学习模型推理。

## 特性

- **多框架支持** - 支持 .NET Framework 4.6-4.8 和 .NET 5.0-10.0
- **跨平台** - Windows、Linux、macOS
- **高性能** - 支持 Span&lt;T&gt;、Memory&lt;T&gt; 零拷贝操作
- **异步支持** - async/await 异步推理
- **模型缓存** - 自动缓存编译后的模型
- **对象池** - 推理请求对象池减少分配开销

## 快速开始

### 安装 NuGet 包

```bash
dotnet add package JYPPX.OpenVINO.CSharp.API
```

### 基本使用

```csharp
using OpenVinoSharp;

// 创建 Core
using var core = new Core();

// 加载并编译模型
var model = core.compile_model("model.xml", "CPU");

// 创建推理请求
using var request = model.create_infer_request();

// 准备输入数据
var input = new Tensor(shape, data);
request.set_input_tensor(input);

// 执行推理
request.infer();

// 获取输出
var output = request.get_output_tensor();
var result = output.get_float_data();
```

## 文档结构

- [API 参考](api/OpenVinoSharp.yml) - 完整的 API 文档
- [示例代码](https://github.com/guojin-yan/OpenVINO-CSharp-API/tree/main/samples) - 使用示例

## 相关链接

- [GitHub 仓库](https://github.com/guojin-yan/OpenVINO-CSharp-API)
- [OpenVINO 官方文档](https://docs.openvino.ai/)
- [NuGet 包](https://www.nuget.org/packages/JYPPX.OpenVINO.CSharp.API/)

## 许可证

本项目采用 [MIT 许可证](LICENSE) 开源。

---

*Copyright © 2024 Guojin Yan*
