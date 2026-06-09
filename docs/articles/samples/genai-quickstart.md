# GenAI QuickStart (.NET 8.0) / GenAI 快速开始

The `samples/GenAIQuickStart-net8.0` project demonstrates the optional GenAI runtime path.

`samples/GenAIQuickStart-net8.0` 项目演示 GenAI runtime 的可选加载路径。

## What It Covers / 覆盖内容

- `GenAI.TryInitialize(out error)` diagnostics.
- Optional LLM text generation with `LLMPipeline`.
- Optional Whisper speech recognition with `WhisperPipeline`.
- Optional VLM text-only generation with `VLMPipeline`.
- Environment-variable model gating so the sample can compile and run without bundled models.

## Run / 运行

```powershell
cd samples\GenAIQuickStart-net8.0
dotnet run
```

To run pipelines, configure model directories:

如果要运行具体 pipeline，请配置模型目录：

```powershell
$env:OPENVINO_GENAI_RUNTIME_DIR = "E:\OpenVINOSharp\openvino\openvino_genai_windows_2026.2.0.0_x86_64"
$env:OPENVINO_GENAI_DEVICE = "CPU"
$env:OPENVINO_GENAI_LLM_MODEL_DIR = "D:\models\llm-ov"
$env:OPENVINO_GENAI_WHISPER_MODEL_DIR = "D:\models\whisper-ov"
$env:OPENVINO_GENAI_VLM_MODEL_DIR = "D:\models\vlm-ov"
dotnet run
```

Core OpenVINO applications do not need this runtime package unless they call `OpenVinoSharp.GenAI`.

基础 OpenVINO 应用只有调用 `OpenVinoSharp.GenAI` 时才需要安装 GenAI runtime 包。
