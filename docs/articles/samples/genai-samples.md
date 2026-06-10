# GenAI Samples (.NET 8.0) / GenAI 示例

The old single `GenAIQuickStart-net8.0` sample has been replaced by a structured
`samples/GenAI` tree. The new samples mirror the currently wrapped OpenVINO
GenAI C API scenarios and are designed for independent reproduction, testing,
and article writing.

旧的单一 `GenAIQuickStart-net8.0` 示例已替换为结构化的 `samples/GenAI` 目录。新的
示例按当前已经封装的 OpenVINO GenAI C API 场景拆分，便于独立复现、测试和撰写技术
文章。

The full step-by-step runbook is in:

完整运行手册见：

```text
samples/GenAI/RUNBOOK.md
```

## Covered Samples / 覆盖示例

| Folder | Scenario |
|---|---|
| `samples/GenAI/TextGeneration/Greedy` | Greedy LLM text generation |
| `samples/GenAI/TextGeneration/BeamSearch` | Beam search decoding |
| `samples/GenAI/TextGeneration/Multinomial` | Sampling with temperature, top-p, top-k, seed |
| `samples/GenAI/TextGeneration/Streaming` | Streaming callback |
| `samples/GenAI/TextGeneration/Chat` | Multi-turn chat |
| `samples/GenAI/TextGeneration/Benchmark` | Warmup, repeated generation, performance metrics |
| `samples/GenAI/WhisperSpeechRecognition` | Whisper speech recognition and timestamp chunks |
| `samples/GenAI/VisualLanguageChat` | VLM image question answering and interactive chat flow |

Image generation, RAG, speech generation, and video generation are not added yet
because their managed pipeline wrappers are not complete.

图像生成、RAG、语音生成和视频生成暂未添加，因为对应的托管 pipeline 封装还没有完成。

## Reproducible Setup / 可复现环境

The samples are C# projects. Conda is only used for model download/export and
media conversion:

示例本身是 C# 项目。conda 只用于模型下载、模型导出和媒体转换：

```powershell
conda create -n ov-genai-samples python=3.11 -y
conda activate ov-genai-samples
python -m pip install --upgrade pip
python -m pip install --upgrade "huggingface_hub[cli]" pillow soundfile
python -m pip install --upgrade-strategy eager "optimum-intel[openvino]" openvino-genai transformers nncf
conda install -c conda-forge ffmpeg -y
```

When running from the repository source tree, set:

从源码目录运行时设置：

```powershell
$env:OPENVINO_GENAI_RUNTIME_DIR = "E:\OpenVINOSharp\openvino\openvino_genai_windows_2026.2.0.0_x86_64\runtime\bin\intel64\Release"
$env:OPENVINO_GENAI_DEVICE = "CPU"
```

## Validated Local Assets / 已验证本地资源

| Asset | Suggested path |
|---|---|
| LLM model | `E:\OpenVINOSharp\models\genai-samples\TinyLlama-1.1B-Chat-v1.0-int4-ov` |
| Whisper model | `E:\OpenVINOSharp\models\genai-smoke\whisper-tiny-int8-ov` |
| VLM smoke model | `E:\OpenVINOSharp\models\genai-smoke\tiny-random-llava-ov` |
| Whisper audio | `E:\OpenVINOSharp\models\genai-samples\assets\how_are_you_doing_today.wav` |
| VLM image | `E:\OpenVINOSharp\models\genai-samples\assets\color_blocks_30.ppm` |

The VLM smoke model validates the API flow but may return empty text. Use a real
VLM model, such as a Qwen VL OpenVINO export, for article-quality semantic
output.

VLM 烟测模型用于验证 API 流程，但可能返回空文本。文章级语义效果展示建议使用真实
VLM 模型，例如 Qwen VL 的 OpenVINO 导出。

## Batch Validation / 批量验证

Run all samples with:

使用下面命令运行全部示例：

```powershell
powershell -ExecutionPolicy Bypass -File samples\GenAI\RunAllSamples.ps1 `
  -RuntimeDir "E:\OpenVINOSharp\openvino\openvino_genai_windows_2026.2.0.0_x86_64\runtime\bin\intel64\Release" `
  -LlmModelDir "E:\OpenVINOSharp\models\genai-samples\TinyLlama-1.1B-Chat-v1.0-int4-ov" `
  -WhisperModelDir "E:\OpenVINOSharp\models\genai-smoke\whisper-tiny-int8-ov" `
  -VlmModelDir "E:\OpenVINOSharp\models\genai-smoke\tiny-random-llava-ov" `
  -AudioPath "E:\OpenVINOSharp\models\genai-samples\assets\how_are_you_doing_today.wav" `
  -ImagePath "E:\OpenVINOSharp\models\genai-samples\assets\color_blocks_30.ppm" `
  -Device CPU
```

The script writes logs to `out\genai-samples-validation`:

脚本会把日志写入 `out\genai-samples-validation`：

```text
01-greedy.log
02-beam-search.log
03-multinomial.log
04-streaming.log
05-benchmark.log
06-chat.log
07-whisper.log
08-vlm-single.log
09-vlm-interactive.log
```

## Individual Commands / 单个示例命令

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

## Important Notes / 重要说明

- `WhisperSpeechRecognition` accepts `--language en`; it normalizes the value to
  `<|en|>` before calling the native C API.
- `VisualLanguageChat` uses non-streaming `Generate`. The validated OpenVINO
  GenAI 2026.2 Windows C runtime does not export
  `ov_genai_vlm_pipeline_generate_with_history`, and the tiny random VLM smoke
  model returned native `-17` through the streamer path.
- The GenAI runtime remains optional. Core OpenVINO API scenarios do not load
  `openvino_genai_c`.

- `WhisperSpeechRecognition` 支持 `--language en`；示例会在调用原生 C API 前转换为
  `<|en|>`。
- `VisualLanguageChat` 使用非流式 `Generate`。已验证的 OpenVINO GenAI 2026.2
  Windows C runtime 未导出 `ov_genai_vlm_pipeline_generate_with_history`，且 tiny
  random VLM 烟测模型通过 streamer 路径返回 native `-17`。
- GenAI runtime 仍保持可选加载。只使用基础 OpenVINO API 时不会加载
  `openvino_genai_c`。
