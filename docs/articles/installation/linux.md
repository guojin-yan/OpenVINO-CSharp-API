# Linux 安装指南

本文介绍如何在 Linux 上使用 OpenVINO C# API。普通推理只需要托管 API 包和基础 OpenVINO runtime 包；调用 `OpenVinoSharp.GenAI` 时，再安装对应平台的 GenAI runtime 包。

## 系统要求

- .NET SDK 8.0 或更高版本用于开发和构建。
- x64 或 ARM64 Linux 发行版。
- NuGet 包版本与项目目标 OpenVINO 版本保持一致。

常见平台包：

| 平台 | 基础 runtime 包 | GenAI runtime 包 |
|---|---|---|
| Ubuntu 24 x64 | `OpenVINO.runtime.ubuntu.24-x86_64` | `JYPPX.OpenVINO.GenAI.runtime.ubuntu.24-x86_64` |
| Ubuntu 22 x64 | `OpenVINO.runtime.ubuntu.22-x86_64` | `JYPPX.OpenVINO.GenAI.runtime.ubuntu.22-x86_64` |
| Ubuntu 22 ARM64 | `OpenVINO.runtime.ubuntu.22-arm64` | `JYPPX.OpenVINO.GenAI.runtime.ubuntu.22-arm64` |
| Ubuntu 20 x64 | `OpenVINO.runtime.ubuntu.20-x86_64` | 按已发布 NuGet 包选择 |
| Ubuntu 20 ARM64 | `OpenVINO.runtime.ubuntu.20-arm64` | 按已发布 NuGet 包选择 |
| RHEL 8 x64 | `OpenVINO.runtime.rhel8-x86_64` | `JYPPX.OpenVINO.GenAI.runtime.rhel8-x86_64` |

## 普通 OpenVINO 推理

```bash
dotnet new console -n OvLinuxDemo
cd OvLinuxDemo

dotnet add package JYPPX.OpenVINO.CSharp.API
dotnet add package OpenVINO.runtime.ubuntu.22-x86_64
```

根据系统替换 runtime 包名。例如 Ubuntu 24 x64 使用 `OpenVINO.runtime.ubuntu.24-x86_64`，RHEL 8 x64 使用 `OpenVINO.runtime.rhel8-x86_64`。

最小代码：

```csharp
using OpenVinoSharp;

using Core core = new Core();
using Model model = core.ReadModel("model.xml");
using CompiledModel compiled = core.CompileModel(model, "CPU");
using InferRequest request = compiled.CreateInferRequest();
```

## GenAI 推理

如果应用调用 `OpenVinoSharp.GenAI`，安装 GenAI runtime 包：

```bash
dotnet add package JYPPX.OpenVINO.CSharp.API
dotnet add package JYPPX.OpenVINO.GenAI.runtime.ubuntu.22-x86_64
```

文本生成示例：

```bash
dotnet run --project samples/GenAI/TextGeneration/Greedy/Greedy.csproj --framework net8.0 -- \
  --model /models/TinyLlama-1.1B-Chat-v1.0-int4-ov \
  --device CPU \
  --prompt "What is OpenVINO?" \
  --max-new-tokens 64
```

GenAI runtime 是可选依赖。只使用 `Core`、`Model`、`Tensor`、`CompiledModel`、`InferRequest` 等传统 API 时，不会加载 `openvino_genai_c`。

## 使用本地 native runtime

通常推荐使用 NuGet runtime 包。只有调试本地 native 构建或验证新 runtime 时，才设置环境变量：

```bash
export OPENVINO_RUNTIME_DIR=/opt/openvino
export OPENVINO_GENAI_RUNTIME_DIR=/opt/openvino_genai
```

目录中需要包含 OpenVINO native 库、插件和依赖库。Linux 上常见库名包括 `libopenvino.so`、`libopenvino_c.so`、`libopenvino_genai.so`、`libopenvino_genai_c.so` 和 CPU plugin。

## 排错

`DllNotFoundException` 通常说明 runtime 包没有恢复成功，或系统平台与包名不匹配。先执行：

```bash
dotnet restore
dotnet nuget locals global-packages --list
```

确认 NuGet 全局缓存中存在对应 runtime 包。若使用本地 runtime，确认环境变量指向的是 runtime 根目录，而不是只包含单个 `.so` 文件的目录。

如果模型加载失败，检查模型目录是否包含 `openvino_model.xml`、`openvino_model.bin`，以及 GenAI 模型所需 tokenizer/detokenizer 文件。

## 相关资源

- [安装方式](index.md)
- [OpenVINO GenAI Runtime](genai-runtime.md)
- [问题排查](../troubleshooting/index.md)
