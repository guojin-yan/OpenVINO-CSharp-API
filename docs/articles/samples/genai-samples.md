# OpenVINO GenAI C# 示例总览

OpenVINO C# API 3.3 将 OpenVINO GenAI 的核心 pipeline 引入 .NET：文本生成、流式输出、多轮聊天、Whisper 语音识别和视觉语言问答都可以直接从 C# 调用。`samples/GenAI` 不再是一个简单的 quick start，而是一组按场景拆分的可运行示例，方便开发者从单个能力开始验证，再逐步组合到自己的应用中。

这组示例的目标很明确：用 NuGet 管理托管 API 和 native runtime，用 C# 编写应用逻辑，用 OpenVINO GenAI 执行本地生成式 AI 推理。传统 OpenVINO 推理项目不需要加载 GenAI runtime；只有调用 `OpenVinoSharp.GenAI` 时才会进入这条路径。

## 示例覆盖范围

| 目录 | 场景 | 价值 |
|---|---|---|
| `TextGeneration/Greedy` | 贪心解码 | 最稳定的 LLM smoke test |
| `TextGeneration/BeamSearch` | Beam Search | 确定性候选搜索 |
| `TextGeneration/Multinomial` | temperature、top-p、top-k | 更开放的采样式生成 |
| `TextGeneration/Streaming` | token 流式输出 | 适合聊天窗口和实时控制台 |
| `TextGeneration/Chat` | 中文/英文对话 | 支持交互输入和 `--turn` scripted turns |
| `TextGeneration/Benchmark` | 预热、重复迭代、性能指标 | 快速观察延迟和吞吐 |
| `WhisperSpeechRecognition` | Whisper ASR | 本地语音识别和时间戳 |
| `VisualLanguageChat` | VLM 图文问答 | 图片 Tensor + 文本 prompt 的多模态推理 |

图像生成、RAG、语音生成和视频生成没有放进当前示例集，因为对应的托管 pipeline 尚未形成完整稳定封装。当前文档只覆盖已经能在 C# 中明确复现的能力。

## 推荐阅读路径

第一次接触这组示例时，建议按下面顺序阅读：

1. [用 C# 跑通 OpenVINO GenAI 文本生成](genai-text-generation-tutorial.md)
2. [用 C# 调用 OpenVINO GenAI Whisper](genai-whisper-tutorial.md)
3. [用 C# 构建 OpenVINO GenAI 视觉语言问答](genai-vlm-tutorial.md)

这三篇文章分别对应文本、语音和视觉语言三条主线。每篇都包含模型准备、运行命令、核心代码、参数说明和排错路径，可以直接作为技术宣发文章或开发者教程发布。

## 环境准备

示例本身是 C# 项目。Python/Conda 只用于下载模型、导出模型或转换媒体文件，不参与 C# 程序运行。

```powershell
conda create -n ov-genai-samples python=3.11 -y
conda activate ov-genai-samples
python -m pip install --upgrade pip
python -m pip install --upgrade "huggingface_hub[cli]" pillow soundfile
python -m pip install --upgrade-strategy eager "optimum-intel[openvino]" openvino-genai transformers nncf
conda install -c conda-forge ffmpeg -y
```

Windows 源码运行时，示例项目默认恢复 `JYPPX.OpenVINO.GenAI.runtime.win`。只有在验证本地编译的 native runtime 时，才需要设置 `OPENVINO_GENAI_RUNTIME_DIR`。

## 已验证资源

| 资源 | 推荐路径 |
|---|---|
| 模型根目录 | `E:\LlmModel` |
| LLM 模型 | `E:\LlmModel\TinyLlama-1.1B-Chat-v1.0-int4-ov` |
| Whisper 模型 | `E:\LlmModel\whisper-tiny-int8-ov` |
| VLM 模型 | `E:\LlmModel\InternVL2-1B-int4-ov` |
| Whisper 音频 | `E:\LlmModel\assets\how_are_you_doing_today.wav` |
| VLM 图片 | `E:\LlmModel\assets\color_blocks_30.ppm` |

`color_blocks_30.ppm` 适合验证图片 Tensor 通路。正式展示 VLM 能力时，建议换成语义明确的真实图片。

## 一次运行全部示例

```powershell
$modelRoot = "E:\LlmModel"
$llm = Join-Path $modelRoot "TinyLlama-1.1B-Chat-v1.0-int4-ov"
$whisper = Join-Path $modelRoot "whisper-tiny-int8-ov"
$vlm = Join-Path $modelRoot "InternVL2-1B-int4-ov"
$audio = Join-Path $modelRoot "assets\how_are_you_doing_today.wav"
$image = Join-Path $modelRoot "assets\color_blocks_30.ppm"

powershell -ExecutionPolicy Bypass -File samples\GenAI\RunAllSamples.ps1 `
  -LlmModelDir $llm `
  -WhisperModelDir $whisper `
  -VlmModelDir $vlm `
  -AudioPath $audio `
  -ImagePath $image `
  -Device CPU
```

脚本会先 publish 每个 sample，再运行生成的 exe，并将日志写入 `out\genai-samples-validation`。这种方式可以规避部分 Windows 机器上应用控制策略拦截 `dotnet run` 直接加载新 DLL 的问题。

## 单个示例命令

```powershell
dotnet run --project samples/GenAI/TextGeneration/Greedy/Greedy.csproj --framework net8.0 -- --model $llm --device CPU
dotnet run --project samples/GenAI/TextGeneration/BeamSearch/BeamSearch.csproj --framework net8.0 -- --model $llm --device CPU --beams 4
dotnet run --project samples/GenAI/TextGeneration/Multinomial/Multinomial.csproj --framework net8.0 -- --model $llm --device CPU --temperature 0.8 --top-p 0.95 --top-k 50
dotnet run --project samples/GenAI/TextGeneration/Streaming/Streaming.csproj --framework net8.0 -- --model $llm --device CPU
dotnet run --project samples/GenAI/TextGeneration/Chat/Chat.csproj --framework net8.0 -- --model $llm --device CPU --turn "请用中文列出三个 OpenVINO 关键词。"
dotnet run --project samples/GenAI/TextGeneration/Benchmark/Benchmark.csproj --framework net8.0 -- --model $llm --device CPU --iterations 3 --warmup 1
dotnet run --project samples/GenAI/WhisperSpeechRecognition/WhisperSpeechRecognition.csproj --framework net8.0 -- --model $whisper --audio $audio --device CPU --timestamps true
dotnet run --project samples/GenAI/VisualLanguageChat/VisualLanguageChat.csproj --framework net8.0 -- --model $vlm --image $image --device CPU --prompt "请用中文描述这张图片。"
```

## 关键实现说明

`TextGeneration/Chat` 支持 `--turn`，可重复传入多轮问题，用于稳定复现中文或英文对话。C# 字符串到 native GenAI 的链路使用 UTF-8，中文 prompt 可以正常传递；最终回答质量取决于模型本身，正式中文对话建议使用中文或多语模型。

`WhisperSpeechRecognition` 支持 `--language en` 这类普通语言代码，示例会在调用 native C API 前转换为 `<|en|>`。`VisualLanguageChat` 使用 `ChatHistory` 加 `GenerateWithHistory()` 的方式实现交互流程，适配 OpenVINO GenAI 2026.3 新增的 VLM history 导出。VLM 示例已验证中文 prompt，可以返回中文图片描述。

VLM 示例默认把空输出视为失败。只有明确做 ABI smoke test 时，才使用 `--allow-empty true`。

## 小结

`samples/GenAI` 展示的是 OpenVINO C# API 3.3 的新增应用边界：不仅能做传统推理，也能把生成式 AI pipeline 纳入 .NET 工程。开发者可以从文本生成开始验证本地 LLM，再扩展到语音识别和视觉语言问答，最终把这些能力组合到桌面、服务端或边缘应用中。
