# GenAI QuickStart (.NET 8.0) / GenAI 快速开始

This sample demonstrates optional OpenVINO GenAI loading and simple LLM, Whisper, and VLM entry points.

本示例演示 OpenVINO GenAI 的可选加载，以及 LLM、Whisper、VLM 的基础入口。

## Packages / 包

When consuming from NuGet, install the managed API and one GenAI runtime package:

使用 NuGet 包时，请安装托管 API 包和一个 GenAI runtime 包：

```bash
dotnet add package JYPPX.OpenVINO.CSharp.API
dotnet add package JYPPX.OpenVINO.GenAI.runtime.win
```

Inside this repository, the sample uses a `ProjectReference` to the local API project.

在本仓库内，示例使用 `ProjectReference` 引用本地 API 项目。

## Run / 运行

```powershell
cd samples\GenAIQuickStart-net8.0
dotnet run
```

If GenAI runtime is not installed, the sample prints diagnostics and exits without failing.

如果未安装 GenAI runtime，示例会打印诊断信息并正常退出。

## Optional Models / 可选模型

Set model directories to run specific pipelines:

设置模型目录后可运行对应 pipeline：

```powershell
$env:OPENVINO_GENAI_RUNTIME_DIR = "E:\OpenVINOSharp\openvino\openvino_genai_windows_2026.2.0.0_x86_64"
$env:OPENVINO_GENAI_DEVICE = "CPU"
$env:OPENVINO_GENAI_LLM_MODEL_DIR = "D:\models\llm-ov"
$env:OPENVINO_GENAI_WHISPER_MODEL_DIR = "D:\models\whisper-ov"
$env:OPENVINO_GENAI_VLM_MODEL_DIR = "D:\models\vlm-ov"
dotnet run
```

The sample only runs a pipeline when its environment variable is configured.

示例只会在对应环境变量已配置时运行该 pipeline。
