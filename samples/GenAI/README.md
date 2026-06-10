# OpenVINO GenAI Samples / OpenVINO GenAI 示例

This folder contains C# sample projects that mirror the currently wrapped
OpenVINO GenAI C API scenarios. The structure follows the official GenAI sample
idea, but keeps the projects grouped under `samples/GenAI` for easier browsing.

本目录包含一组 C# 示例项目，用来复刻当前已经完成封装的 OpenVINO GenAI C API
场景。目录结构参考官方 GenAI samples，同时统一放在 `samples/GenAI` 下，便于开发者
查找和复现。

Official reference / 官方参考：
<https://github.com/openvinotoolkit/openvino.genai/tree/master/samples>

For the complete local reproduction flow, read [RUNBOOK.md](RUNBOOK.md).

完整本地复现流程见 [RUNBOOK.md](RUNBOOK.md)。

## Project Layout / 项目结构

| Project | Official inspiration | Scenario |
|---|---|---|
| `Common` | Shared helpers | Argument parsing, runtime diagnostics, WAV loading, BMP/PPM image loading |
| `TextGeneration/Greedy` | `samples/c/text_generation/greedy_causal_lm_c.c` | Deterministic text generation |
| `TextGeneration/BeamSearch` | `samples/cpp/text_generation/beam_search_causal_lm.cpp` | Beam search decoding |
| `TextGeneration/Multinomial` | `samples/cpp/text_generation/multinomial_causal_lm.cpp` | Sampling with top-k, top-p, temperature, seed |
| `TextGeneration/Streaming` | `samples/c/text_generation/chat_sample_c.c` streamer callback | Streaming callback |
| `TextGeneration/Chat` | `samples/c/text_generation/chat_sample_c.c` | Multi-turn chat |
| `TextGeneration/Benchmark` | `samples/c/text_generation/benchmark_genai_c.c` | Warmup, repeated generation, performance metrics |
| `WhisperSpeechRecognition` | `samples/c/whisper_speech_recognition/whisper_speech_recognition.c` | Whisper automatic speech recognition |
| `VisualLanguageChat` | `samples/c/visual_language_chat/vlm_pipeline.c` | Image + text VLM chat |

The official GenAI repository also contains image generation, RAG, speech
generation, and video generation samples. They are intentionally not added as
empty C# samples yet because the corresponding managed pipeline wrappers are
not complete.

官方 GenAI 仓库还包含图像生成、RAG、语音生成和视频生成示例。当前不会添加空壳
C# 示例，因为这些 pipeline 的托管封装还没有完成。

## Quick Local Validation / 快速本地验证

Set the native runtime and prepared model paths:

设置原生运行时和准备好的模型路径：

```powershell
cd E:\OpenVINOSharp\OpenVINO-CSharp-API-csharp3.3

$runtime = "E:\OpenVINOSharp\openvino\openvino_genai_windows_2026.2.0.0_x86_64\runtime\bin\intel64\Release"
$llm = "E:\OpenVINOSharp\models\genai-samples\TinyLlama-1.1B-Chat-v1.0-int4-ov"
$whisper = "E:\OpenVINOSharp\models\genai-smoke\whisper-tiny-int8-ov"
$vlm = "E:\OpenVINOSharp\models\genai-smoke\tiny-random-llava-ov"
$audio = "E:\OpenVINOSharp\models\genai-samples\assets\how_are_you_doing_today.wav"
$image = "E:\OpenVINOSharp\models\genai-samples\assets\color_blocks_30.ppm"
```

Run all samples and write one log per scenario:

运行全部示例，并为每个场景保存一份日志：

```powershell
powershell -ExecutionPolicy Bypass -File samples\GenAI\RunAllSamples.ps1 `
  -RuntimeDir $runtime `
  -LlmModelDir $llm `
  -WhisperModelDir $whisper `
  -VlmModelDir $vlm `
  -AudioPath $audio `
  -ImagePath $image `
  -Device CPU
```

Logs are written to `out\genai-samples-validation`.

日志会写入 `out\genai-samples-validation`。

## Minimal Commands / 最小运行命令

When running from source, set:

从源码运行时设置：

```powershell
$env:OPENVINO_GENAI_RUNTIME_DIR = "E:\OpenVINOSharp\openvino\openvino_genai_windows_2026.2.0.0_x86_64\runtime\bin\intel64\Release"
$env:OPENVINO_GENAI_DEVICE = "CPU"
```

Then run individual samples:

然后运行单个示例：

```powershell
dotnet run --project samples/GenAI/TextGeneration/Greedy/Greedy.csproj --framework net8.0 -- --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR"
dotnet run --project samples/GenAI/TextGeneration/BeamSearch/BeamSearch.csproj --framework net8.0 -- --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR" --beams 4
dotnet run --project samples/GenAI/TextGeneration/Multinomial/Multinomial.csproj --framework net8.0 -- --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR" --temperature 0.8 --top-p 0.95 --top-k 50
dotnet run --project samples/GenAI/TextGeneration/Streaming/Streaming.csproj --framework net8.0 -- --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR"
dotnet run --project samples/GenAI/TextGeneration/Chat/Chat.csproj --framework net8.0 -- --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR"
dotnet run --project samples/GenAI/TextGeneration/Benchmark/Benchmark.csproj --framework net8.0 -- --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR" --iterations 3 --warmup 1
dotnet run --project samples/GenAI/WhisperSpeechRecognition/WhisperSpeechRecognition.csproj --framework net8.0 -- --model "$env:OPENVINO_GENAI_WHISPER_MODEL_DIR" --audio "$env:OPENVINO_GENAI_AUDIO_PATH" --timestamps true
dotnet run --project samples/GenAI/VisualLanguageChat/VisualLanguageChat.csproj --framework net8.0 -- --model "$env:OPENVINO_GENAI_VLM_MODEL_DIR" --image "$env:OPENVINO_GENAI_IMAGE_PATH" --interactive true
```

## Notes / 说明

- `RunAllSamples.ps1` is the recommended validation entry point before changing
  sample code.
- `WhisperSpeechRecognition` accepts `--language en`; the sample normalizes it
  to the Whisper token form `<|en|>` before calling the C API.
- `VisualLanguageChat` uses non-streaming `Generate` because the validated
  OpenVINO GenAI 2026.2 Windows C runtime does not export
  `ov_genai_vlm_pipeline_generate_with_history`, and the VLM streamer path
  returned native error `-17` with the tiny random smoke model.
- The tiny random VLM model validates the pipeline flow but may return empty
  text. Use a real VLM model for article-quality output.

- 修改示例代码前，建议先通过 `RunAllSamples.ps1` 完整验证。
- `WhisperSpeechRecognition` 支持 `--language en`；示例会在调用 C API 前自动转成
  Whisper token 写法 `<|en|>`。
- `VisualLanguageChat` 使用非流式 `Generate`，因为已验证的 OpenVINO GenAI 2026.2
  Windows C runtime 未导出 `ov_genai_vlm_pipeline_generate_with_history`，并且 tiny
  random 烟测模型下 VLM streamer 路径返回 native `-17`。
- tiny random VLM 模型用于验证 pipeline 流程，可能返回空文本。文章级效果展示请使用
  真实 VLM 模型。
