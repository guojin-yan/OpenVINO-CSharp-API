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

The sample projects restore `JYPPX.OpenVINO.GenAI.runtime.win` 2026.2.0 by
default on Windows. Set `OPENVINO_GENAI_RUNTIME_DIR` only when validating a
local native runtime build instead of the published NuGet package.

示例项目在 Windows 上默认安装 `JYPPX.OpenVINO.GenAI.runtime.win` 2026.2.0。只有在
验证本地 native runtime 构建、而不是已发布 NuGet 包时，才需要设置
`OPENVINO_GENAI_RUNTIME_DIR`。

## Validated Local Assets / 已验证本地资源

| Asset | Suggested path |
|---|---|
| LLM model | `E:\OpenVINOSharp\models\genai-samples\TinyLlama-1.1B-Chat-v1.0-int4-ov` |
| Whisper model | `E:\OpenVINOSharp\models\genai-smoke\whisper-tiny-int8-ov` |
| VLM model | `E:\OpenVINOSharp\models\genai-samples\InternVL2-1B-int4-ov` |
| Whisper audio | `E:\OpenVINOSharp\models\genai-samples\assets\how_are_you_doing_today.wav` |
| VLM image | `E:\OpenVINOSharp\models\genai-samples\assets\color_blocks_30.ppm` |

The VLM validation uses the real `OpenVINO/InternVL2-1B-int4-ov` model and
requires non-empty generated text. Tiny random VLM models can still be useful
for ABI smoke tests, but they should not be used as article-quality validation.

VLM 验证使用真实的 `OpenVINO/InternVL2-1B-int4-ov` 模型，并要求生成非空文本。
tiny random VLM 模型仍可用于 ABI 烟测，但不能作为文章级验证依据。

## Batch Validation / 批量验证

Run all samples with:

使用下面命令运行全部示例：

```powershell
powershell -ExecutionPolicy Bypass -File samples\GenAI\RunAllSamples.ps1 `
  -LlmModelDir "E:\OpenVINOSharp\models\genai-samples\TinyLlama-1.1B-Chat-v1.0-int4-ov" `
  -WhisperModelDir "E:\OpenVINOSharp\models\genai-smoke\whisper-tiny-int8-ov" `
  -VlmModelDir "E:\OpenVINOSharp\models\genai-samples\InternVL2-1B-int4-ov" `
  -AudioPath "E:\OpenVINOSharp\models\genai-samples\assets\how_are_you_doing_today.wav" `
  -ImagePath "E:\OpenVINOSharp\models\genai-samples\assets\color_blocks_30.ppm" `
  -Device CPU
```

The script publishes each sample first, runs the generated exe, and writes logs
to `out\genai-samples-validation`:

脚本会先 publish 每个 sample，再运行生成的 exe，并把日志写入
`out\genai-samples-validation`：

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

On Windows machines with application control policies, direct `dotnet run` can
be blocked for newly built DLLs. The batch script avoids that local issue by
publishing each sample and running the generated exe.

在启用应用控制策略的 Windows 机器上，`dotnet run` 直接加载新构建 DLL 可能被拦截。
批量脚本通过先 publish 再运行 exe 来规避该本地问题。

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
- `VisualLanguageChat` uses `StartChat()` plus streamed `Generate()`. The
  validated OpenVINO GenAI 2026.2 Windows C runtime does not export
  `ov_genai_vlm_pipeline_generate_with_history`.
- `VisualLanguageChat` treats empty output as a failure by default. Pass
  `--allow-empty true` only for explicit ABI smoke tests.
- The GenAI runtime remains optional. Core OpenVINO API scenarios do not load
  `openvino_genai_c`.

- `WhisperSpeechRecognition` 支持 `--language en`；示例会在调用原生 C API 前转换为
  `<|en|>`。
- `VisualLanguageChat` 使用 `StartChat()` 加流式 `Generate()`。已验证的
  OpenVINO GenAI 2026.2 Windows C runtime 未导出
  `ov_genai_vlm_pipeline_generate_with_history`。
- `VisualLanguageChat` 默认把空输出视为失败。只有明确做 ABI 烟测时才传入
  `--allow-empty true`。
- GenAI runtime 仍保持可选加载。只使用基础 OpenVINO API 时不会加载
  `openvino_genai_c`。
