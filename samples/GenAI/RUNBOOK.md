# OpenVINO GenAI Samples Runbook / OpenVINO GenAI 示例运行手册

This runbook records the complete local reproduction flow for the C# GenAI
samples under `samples/GenAI`. It is written as source material for later
technical articles: each sample has its own purpose, command line, expected
output, and troubleshooting notes.

本文档记录 `samples/GenAI` 下 C# GenAI 示例的完整本地复现流程。它可以作为后续
技术文章的底稿：每个示例都包含用途、运行命令、预期输出和排错说明。

Official OpenVINO GenAI sample reference:
<https://github.com/openvinotoolkit/openvino.genai/tree/master/samples>

官方 OpenVINO GenAI 示例参考：
<https://github.com/openvinotoolkit/openvino.genai/tree/master/samples>

## 1. Local Validation Matrix / 本地验证矩阵

The following setup was used to verify that the sample code path is runnable
without publishing NuGet packages.

以下环境已经用于验证示例代码路径可以在本地直接跑通，不依赖正式发布 NuGet 包。

| Item / 项目 | Local value / 本地值 |
|---|---|
| Repository / 仓库 | `E:\OpenVINOSharp\OpenVINO-CSharp-API-csharp3.3` |
| Native runtime / 原生运行时 | `E:\OpenVINOSharp\openvino\openvino_genai_windows_2026.2.0.0_x86_64\runtime\bin\intel64\Release` |
| LLM model / 文本模型 | `E:\OpenVINOSharp\models\genai-samples\TinyLlama-1.1B-Chat-v1.0-int4-ov` |
| Whisper model / Whisper 模型 | `E:\OpenVINOSharp\models\genai-smoke\whisper-tiny-int8-ov` |
| VLM smoke model / VLM 烟测模型 | `E:\OpenVINOSharp\models\genai-smoke\tiny-random-llava-ov` |
| Audio / 音频 | `E:\OpenVINOSharp\models\genai-samples\assets\how_are_you_doing_today.wav` |
| Image / 图片 | `E:\OpenVINOSharp\models\genai-samples\assets\color_blocks_30.ppm` |
| Device / 推理设备 | `CPU` |

Validation logs are written by `RunAllSamples.ps1` to
`out\genai-samples-validation`.

批量验证脚本会把日志写入 `out\genai-samples-validation`。

## 2. Prepare Native Runtime / 准备原生运行时

When running from the repository source tree, point the GenAI loader to the
directory that contains `openvino_genai_c.dll`.

从源码目录运行时，需要把 GenAI loader 指向包含 `openvino_genai_c.dll` 的目录。

```powershell
cd E:\OpenVINOSharp\OpenVINO-CSharp-API-csharp3.3

$env:OPENVINO_GENAI_RUNTIME_DIR = "E:\OpenVINOSharp\openvino\openvino_genai_windows_2026.2.0.0_x86_64\runtime\bin\intel64\Release"
$env:OPENVINO_GENAI_DEVICE = "CPU"
Remove-Item Env:OPENVINO_GENAI_C_LIBRARY -ErrorAction SilentlyContinue
```

The runtime directory should contain at least:

运行时目录至少应包含：

- `openvino_genai_c.dll`
- `openvino_genai.dll`
- `openvino_tokenizers.dll`
- `openvino.dll`
- `openvino_c.dll`
- `tbb12.dll`
- OpenVINO plugins and frontends such as `openvino_intel_cpu_plugin.dll`.

After the GenAI runtime NuGet packages are officially published, application
developers can install `JYPPX.OpenVINO.GenAI.runtime.win` instead of setting
`OPENVINO_GENAI_RUNTIME_DIR` manually.

GenAI runtime NuGet 包正式发布后，应用开发者可以安装
`JYPPX.OpenVINO.GenAI.runtime.win`，不必再手动设置 `OPENVINO_GENAI_RUNTIME_DIR`。

## 3. Prepare Conda Tools / 准备 Conda 工具环境

The C# samples do not require Python at runtime. Conda is used only for model
download, model export, and media conversion.

C# 示例运行时不依赖 Python。这里使用 conda 只是为了下载模型、导出模型和转换媒体文件。

```powershell
conda create -n ov-genai-samples python=3.11 -y
conda activate ov-genai-samples
python -m pip install --upgrade pip
python -m pip install --upgrade "huggingface_hub[cli]" pillow soundfile
python -m pip install --upgrade-strategy eager "optimum-intel[openvino]" openvino-genai transformers nncf
conda install -c conda-forge ffmpeg -y
```

If `conda` is not available, install Miniconda or Anaconda first. The repository
does not commit conda environments, downloaded models, or generated media.

如果本机没有 `conda`，请先安装 Miniconda 或 Anaconda。仓库不会提交 conda 环境、
下载的模型或生成的媒体文件。

## 4. Prepare Models / 准备模型

### 4.1 LLM Model / 文本生成模型

For fast local validation, use the converted TinyLlama INT4 model:

为了快速本地验证，可以使用已经转换好的 TinyLlama INT4 模型：

```powershell
hf download OpenVINO/TinyLlama-1.1B-Chat-v1.0-int4-ov `
  --local-dir E:\OpenVINOSharp\models\genai-samples\TinyLlama-1.1B-Chat-v1.0-int4-ov

$env:OPENVINO_GENAI_LLM_MODEL_DIR = "E:\OpenVINOSharp\models\genai-samples\TinyLlama-1.1B-Chat-v1.0-int4-ov"
```

Expected files include `openvino_model.xml`, `openvino_model.bin`,
`openvino_tokenizer.xml`, `openvino_tokenizer.bin`,
`openvino_detokenizer.xml`, `openvino_detokenizer.bin`, `tokenizer.json`, and
`generation_config.json`.

目录中应包含 `openvino_model.xml`、`openvino_model.bin`、
`openvino_tokenizer.xml`、`openvino_tokenizer.bin`、`openvino_detokenizer.xml`、
`openvino_detokenizer.bin`、`tokenizer.json` 和 `generation_config.json` 等文件。

You can also export a model yourself:

也可以自行导出模型：

```powershell
optimum-cli export openvino `
  --model TinyLlama/TinyLlama-1.1B-Chat-v1.0 `
  --weight-format int4 `
  --trust-remote-code `
  E:\OpenVINOSharp\models\genai-samples\TinyLlama-1.1B-Chat-v1.0-int4-ov
```

### 4.2 Whisper Model / Whisper 语音识别模型

Use the converted Whisper tiny INT8 model for validation:

验证时可以使用已经转换好的 Whisper tiny INT8 模型：

```powershell
hf download OpenVINO/whisper-tiny-int8-ov `
  --local-dir E:\OpenVINOSharp\models\genai-smoke\whisper-tiny-int8-ov

$env:OPENVINO_GENAI_WHISPER_MODEL_DIR = "E:\OpenVINOSharp\models\genai-smoke\whisper-tiny-int8-ov"
```

Alternative export command:

也可以自行导出：

```powershell
optimum-cli export openvino `
  --trust-remote-code `
  --model openai/whisper-tiny `
  E:\OpenVINOSharp\models\genai-samples\whisper-tiny-ov
```

### 4.3 VLM Model / 视觉语言模型

For API smoke tests, the local validation uses a tiny random LLaVA OpenVINO
model. This model is useful for checking ABI, tensor shape, image loading, and
pipeline lifetime. It is not suitable for article-quality semantic answers.

API 烟测使用 tiny random LLaVA OpenVINO 模型。该模型适合验证 ABI、Tensor 形状、
图片加载和 pipeline 生命周期，但不适合用于文章中的语义效果展示。

```powershell
hf download katuni4ka/tiny-random-llava-ov `
  --local-dir E:\OpenVINOSharp\models\genai-smoke\tiny-random-llava-ov

$env:OPENVINO_GENAI_VLM_MODEL_DIR = "E:\OpenVINOSharp\models\genai-smoke\tiny-random-llava-ov"
```

For technical articles, use a real VLM model. Examples:

写技术文章时建议换成真实 VLM 模型。例如：

```powershell
optimum-cli export openvino `
  --model Qwen/Qwen3-VL-2B-Instruct `
  --trust-remote-code `
  E:\OpenVINOSharp\models\genai-samples\Qwen3-VL-2B-Instruct-ov
```

Real VLM models are much larger and may require more memory and disk space.
Keep the tiny model for CI or local smoke tests, and use a real model for
screenshots and article output.

真实 VLM 模型体积较大，对内存和磁盘空间要求更高。建议保留 tiny 模型用于 CI 或本地
烟测，用真实模型生成文章截图和效果输出。

## 5. Prepare Media / 准备音频和图片

### 5.1 Audio / 音频

The validated sample audio says "How are you doing today?".

已验证音频内容为 "How are you doing today?"。

```powershell
New-Item -ItemType Directory -Force E:\OpenVINOSharp\models\genai-samples\assets | Out-Null

curl.exe -L `
  -o E:\OpenVINOSharp\models\genai-samples\assets\how_are_you_doing_today.wav `
  https://storage.openvinotoolkit.org/models_contrib/speech/2021.2/librispeech_s5/how_are_you_doing_today.wav

$env:OPENVINO_GENAI_AUDIO_PATH = "E:\OpenVINOSharp\models\genai-samples\assets\how_are_you_doing_today.wav"
```

For your own audio, convert it to mono 16 kHz WAV:

如果使用自己的音频，先转换为 mono 16 kHz WAV：

```powershell
ffmpeg -i input.mp3 -ac 1 -ar 16000 speech.wav
```

### 5.2 Image / 图片

The sample image loader intentionally avoids image NuGet dependencies and
supports RGB BMP and binary PPM/PNM. Convert JPG or PNG with Pillow:

示例图片加载器刻意不引入图片处理 NuGet 依赖，支持 RGB BMP 和二进制 PPM/PNM。
JPG 或 PNG 可用 Pillow 转换：

```powershell
python -c "from PIL import Image; Image.open(r'input.jpg').convert('RGB').save(r'input.bmp')"
$env:OPENVINO_GENAI_IMAGE_PATH = "$PWD\input.bmp"
```

For smoke tests, `color_blocks_30.ppm` is enough:

烟测时可以使用 `color_blocks_30.ppm`：

```powershell
$env:OPENVINO_GENAI_IMAGE_PATH = "E:\OpenVINOSharp\models\genai-samples\assets\color_blocks_30.ppm"
```

## 6. Run All Samples / 一次性运行全部示例

Use the validation script after all paths are ready:

所有路径准备好之后，运行批量验证脚本：

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

The script runs:

脚本会运行：

1. Greedy text generation.
2. Beam search text generation.
3. Multinomial sampling.
4. Streaming text generation.
5. Benchmark text generation.
6. Interactive chat with scripted input.
7. Whisper speech recognition with timestamps.
8. VLM single-turn image question.
9. VLM interactive chat with scripted input.

Logs are saved in `out\genai-samples-validation`:

日志保存在 `out\genai-samples-validation`：

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

## 7. Individual Sample Commands / 单个示例命令

Run commands from the repository root.

以下命令均在仓库根目录运行。

### 7.1 Greedy

Purpose: deterministic LLM generation with a small number of new tokens.

用途：使用 greedy 解码进行确定性文本生成。

```powershell
dotnet run --project samples/GenAI/TextGeneration/Greedy/Greedy.csproj --framework net8.0 -- `
  --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR" `
  --prompt "What is OpenVINO?" `
  --device CPU `
  --max-new-tokens 8
```

Expected output includes a short continuation, for example:

预期输出包含短文本续写，例如：

```text
OpenVINO is a software development
```

### 7.2 Beam Search

Purpose: compare deterministic beam search decoding with greedy decoding.

用途：对比 beam search 解码和 greedy 解码。

```powershell
dotnet run --project samples/GenAI/TextGeneration/BeamSearch/BeamSearch.csproj --framework net8.0 -- `
  --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR" `
  --prompt "OpenVINO is" `
  --device CPU `
  --max-new-tokens 8 `
  --beams 2
```

### 7.3 Multinomial

Purpose: demonstrate sampling with temperature, top-p, top-k, and seed.

用途：演示 temperature、top-p、top-k 和 seed 采样参数。

```powershell
dotnet run --project samples/GenAI/TextGeneration/Multinomial/Multinomial.csproj --framework net8.0 -- `
  --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR" `
  --prompt "OpenVINO helps developers" `
  --device CPU `
  --max-new-tokens 8 `
  --temperature 0.7 `
  --top-p 0.9 `
  --top-k 20 `
  --seed 7
```

### 7.4 Streaming

Purpose: verify the LLM streamer callback and print tokens as they arrive.

用途：验证 LLM streamer 回调，并边生成边输出 token。

```powershell
dotnet run --project samples/GenAI/TextGeneration/Streaming/Streaming.csproj --framework net8.0 -- `
  --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR" `
  --prompt "List one OpenVINO benefit." `
  --device CPU `
  --max-new-tokens 8
```

### 7.5 Benchmark

Purpose: run warmup and measured iterations, then print performance metrics.

用途：运行 warmup 和正式迭代，并输出性能指标。

```powershell
dotnet run --project samples/GenAI/TextGeneration/Benchmark/Benchmark.csproj --framework net8.0 -- `
  --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR" `
  --prompt "OpenVINO is" `
  --device CPU `
  --max-new-tokens 8 `
  --iterations 1 `
  --warmup 0
```

### 7.6 Chat

Purpose: demonstrate chat mode and multi-turn LLM state.

用途：演示 LLM chat mode 和多轮对话状态。

```powershell
dotnet run --project samples/GenAI/TextGeneration/Chat/Chat.csproj --framework net8.0 -- `
  --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR" `
  --device CPU `
  --max-new-tokens 8
```

Interactive commands:

交互命令：

- Enter a question to generate a response.
- Type `/exit` or an empty line to quit.
- 输入问题后生成回答。
- 输入 `/exit` 或空行退出。

### 7.7 Whisper Speech Recognition

Purpose: run Whisper ASR, optional language/task settings, and timestamp chunks.

用途：运行 Whisper 语音识别，演示语言、任务和时间戳分段配置。

```powershell
dotnet run --project samples/GenAI/WhisperSpeechRecognition/WhisperSpeechRecognition.csproj --framework net8.0 -- `
  --model "$env:OPENVINO_GENAI_WHISPER_MODEL_DIR" `
  --audio "$env:OPENVINO_GENAI_AUDIO_PATH" `
  --device CPU `
  --language en `
  --task transcribe `
  --timestamps true
```

The sample accepts both plain language codes such as `en` and Whisper token
syntax such as `<|en|>`. Plain language codes are normalized before being sent
to the native C API.

示例同时支持 `en` 这样的普通语言代码，以及 `<|en|>` 这样的 Whisper token 写法。
普通语言代码会在调用 C API 前自动转换。

Expected output for the validated audio:

已验证音频的预期输出：

```text
How are you doing today?
[0.00, 2.00] How are you doing today?
```

### 7.8 Visual Language Chat

Purpose: load an image as a tensor and ask a VLM pipeline a text question.

用途：把图片加载为 Tensor，然后向 VLM pipeline 提问。

```powershell
dotnet run --project samples/GenAI/VisualLanguageChat/VisualLanguageChat.csproj --framework net8.0 -- `
  --model "$env:OPENVINO_GENAI_VLM_MODEL_DIR" `
  --image "$env:OPENVINO_GENAI_IMAGE_PATH" `
  --device CPU `
  --prompt "What colors are visible?" `
  --max-new-tokens 8
```

Interactive mode:

交互模式：

```powershell
dotnet run --project samples/GenAI/VisualLanguageChat/VisualLanguageChat.csproj --framework net8.0 -- `
  --model "$env:OPENVINO_GENAI_VLM_MODEL_DIR" `
  --image "$env:OPENVINO_GENAI_IMAGE_PATH" `
  --device CPU `
  --interactive true `
  --max-new-tokens 8
```

Note: OpenVINO GenAI 2026.2 Windows C runtime used in local validation does not
export `ov_genai_vlm_pipeline_generate_with_history`. The C# sample uses
`StartChat()` plus `Generate()` so it remains runnable with the released C
runtime. The VLM streamer path also returned a native `-17` error with the tiny
random model during validation, so this sample uses non-streaming generation.

注意：本地验证使用的 OpenVINO GenAI 2026.2 Windows C runtime 未导出
`ov_genai_vlm_pipeline_generate_with_history`。C# 示例使用 `StartChat()` 加
`Generate()`，以保证可在当前发布版 C runtime 上运行。tiny random VLM 模型验证时
streamer 路径返回 native `-17`，因此该示例使用非流式生成。

The tiny random VLM model may return empty text. That is acceptable for ABI and
sample-flow validation. Use a real VLM model for article screenshots and
meaningful answers.

tiny random VLM 模型可能返回空文本。这对 ABI 和示例流程验证是可接受的。文章截图和
语义效果展示应使用真实 VLM 模型。

## 8. Article Planning / 技术文章拆分建议

The current sample set can be split into the following articles:

当前示例集可以拆成以下文章：

1. OpenVINO GenAI C# runtime setup and NuGet/native runtime selection.
2. Greedy and beam search text generation with C#.
3. Sampling parameters: temperature, top-p, top-k, seed.
4. Streaming token output and callback design.
5. Building a multi-turn LLM chat console.
6. Measuring generation latency and token throughput.
7. Whisper speech recognition with timestamps.
8. VLM image chat and image tensor preparation.
9. Batch validation and reproducible GenAI sample workflows.

For each article, include:

每篇文章建议包含：

- Environment and runtime package selection.
- Model download or export command.
- Complete `dotnet run` command.
- A short explanation of the key wrapper APIs.
- Expected output and metrics.
- Known limitations and troubleshooting.

## 9. Troubleshooting Checklist / 排错清单

- `openvino_genai_c.dll` not found: check `OPENVINO_GENAI_RUNTIME_DIR`.
- `DllNotFoundException` for `openvino.dll` or plugins: use the GenAI runtime
  `runtime\bin\intel64\Release` directory, not only the folder containing
  `openvino_genai_c.dll`.
- Model load failure: verify the model directory contains OpenVINO IR files and
  tokenizer/detokenizer files.
- Whisper language error: use `--language en`; the sample normalizes it to
  `<|en|>`.
- Whisper empty output: confirm WAV is mono 16 kHz and contains clear speech.
- VLM image load failure: convert images to RGB BMP or binary PPM/PNM.
- VLM empty output with tiny random model: use a real VLM model for semantic
  results.
- Very slow first run: model compilation and CPU cache warmup can dominate the
  first iteration.

- 找不到 `openvino_genai_c.dll`：检查 `OPENVINO_GENAI_RUNTIME_DIR`。
- `openvino.dll` 或插件 `DllNotFoundException`：请使用 GenAI runtime 的
  `runtime\bin\intel64\Release` 目录，不要只复制 `openvino_genai_c.dll`。
- 模型加载失败：确认模型目录包含 OpenVINO IR 文件和 tokenizer/detokenizer 文件。
- Whisper 语言参数错误：使用 `--language en`，示例会自动转换为 `<|en|>`。
- Whisper 输出为空：确认 WAV 是 mono 16 kHz 且包含清晰语音。
- VLM 图片加载失败：把图片转换为 RGB BMP 或二进制 PPM/PNM。
- tiny random VLM 输出为空：语义效果展示请使用真实 VLM 模型。
- 首次运行很慢：模型编译和 CPU cache warmup 会影响第一次迭代。
