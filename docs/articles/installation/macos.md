# macOS 安装指南

本文介绍如何在 macOS 上使用 OpenVINO C# API。macOS 用户通常只需要安装托管 API 包和对应架构的 runtime NuGet 包；调用 GenAI API 时，再安装 GenAI runtime 包。

## 系统要求

- macOS 12 或更高版本。
- .NET SDK 8.0 或更高版本用于开发和构建。
- Apple Silicon 使用 ARM64 runtime；Intel Mac 使用 x64 runtime。

| 平台 | 基础 runtime 包 | GenAI runtime 包 |
|---|---|---|
| Apple Silicon | `OpenVINO.runtime.macos-arm64` | `JYPPX.OpenVINO.GenAI.runtime.macos-arm64` |
| Intel Mac | `OpenVINO.runtime.macos-x86_64` | 按已发布 NuGet 包选择 |

## 普通 OpenVINO 推理

Apple Silicon：

```bash
dotnet new console -n OvMacDemo
cd OvMacDemo

dotnet add package JYPPX.OpenVINO.CSharp.API
dotnet add package OpenVINO.runtime.macos-arm64
```

Intel Mac：

```bash
dotnet add package JYPPX.OpenVINO.CSharp.API
dotnet add package OpenVINO.runtime.macos-x86_64
```

最小代码：

```csharp
using OpenVinoSharp;

using Core core = new Core();
using Model model = core.ReadModel("model.xml");
using CompiledModel compiled = core.CompileModel(model, "CPU");
using InferRequest request = compiled.CreateInferRequest();
```

## GenAI 推理

Apple Silicon 上可以安装 GenAI runtime 包：

```bash
dotnet add package JYPPX.OpenVINO.CSharp.API
dotnet add package JYPPX.OpenVINO.GenAI.runtime.macos-arm64
```

文本生成示例：

```bash
dotnet run --project samples/GenAI/TextGeneration/Greedy/Greedy.csproj --framework net8.0 -- \
  --model /models/TinyLlama-1.1B-Chat-v1.0-int4-ov \
  --device CPU \
  --prompt "What is OpenVINO?" \
  --max-new-tokens 64
```

如果 NuGet.org 上已有对应架构的 GenAI runtime 包，Intel Mac 使用同名 x64 包；否则请使用基础 OpenVINO runtime 做传统推理，或自行准备 native GenAI runtime。

## 使用本地 native runtime

普通用户优先使用 NuGet runtime 包。本地 native runtime 适合调试或验证未发布平台包：

```bash
export OPENVINO_RUNTIME_DIR=/opt/openvino
export OPENVINO_GENAI_RUNTIME_DIR=/opt/openvino_genai
```

macOS 上 native 库通常是 `.dylib`。目录中需要包含 OpenVINO 主库、C API 库、GenAI 库、插件和依赖库。

## Apple Silicon 与 Rosetta

Apple Silicon 项目应优先使用 ARM64 runtime，并用 ARM64 .NET SDK 运行。不要在 ARM64 进程中加载 x64 native runtime，也不要在 Rosetta x64 进程中加载 ARM64 runtime。架构不一致会导致 native library 加载失败。

可以用下面命令确认当前进程架构：

```bash
dotnet --info
uname -m
```

## 排错

`DllNotFoundException` 通常是 runtime 包架构不匹配或 native 库未能从 NuGet runtime 目录复制/解析。先确认包名和系统架构一致，再执行：

```bash
dotnet restore
dotnet nuget locals global-packages --list
```

如果模型加载失败，检查模型目录是否包含 OpenVINO IR 文件和 tokenizer/detokenizer 文件。GenAI 模型不能只放 `openvino_model.xml` 和 `.bin`，还需要生成式 pipeline 所需的 tokenizer 相关文件。

## 相关资源

- [安装方式](index.md)
- [OpenVINO GenAI Runtime](genai-runtime.md)
- [问题排查](../troubleshooting/index.md)
