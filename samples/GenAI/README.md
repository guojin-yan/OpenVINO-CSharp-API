# OpenVINO GenAI Samples / OpenVINO GenAI 案例

This folder contains C# sample projects that mirror the currently wrapped OpenVINO GenAI C API scenarios.

本目录包含一组 C# 示例项目，用来复刻当前已经完成封装的 OpenVINO GenAI C API 场景。

Official reference: <https://github.com/openvinotoolkit/openvino.genai/tree/master/samples>

官方参考：<https://github.com/openvinotoolkit/openvino.genai/tree/master/samples>

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

The official GenAI repository also contains image generation, RAG, speech generation, and video generation samples. Those are intentionally not added as empty C# samples yet because the corresponding managed wrappers are not complete.

官方 GenAI 仓库还包含图像生成、RAG、语音生成和视频生成示例。当前不会添加空壳 C# 示例，因为这些 pipeline 的托管封装还没有完成。

## Prerequisites / 前置条件

- .NET 8 SDK.
- OpenVINO GenAI native runtime.
- A converted or downloaded OpenVINO GenAI model directory.
- Optional conda environment for model export and media conversion.

```powershell
cd E:\OpenVINOSharp\OpenVINO-CSharp-API-csharp3.3

# When running from source, point the loader to a native GenAI runtime directory.
# 从源码运行时，指向本机 GenAI runtime 原生库目录。
$env:OPENVINO_GENAI_RUNTIME_DIR = "E:\OpenVINOSharp\openvino\openvino_genai_windows_2026.2.0.0_x86_64\runtime\bin\intel64\Release"
$env:OPENVINO_GENAI_DEVICE = "CPU"
```

After the NuGet runtime packages are published, package consumers can install the matching runtime package instead of setting `OPENVINO_GENAI_RUNTIME_DIR`.

正式发布 NuGet runtime 包后，包使用者可以安装匹配 runtime 包，而不必手动设置 `OPENVINO_GENAI_RUNTIME_DIR`。

```powershell
dotnet add package JYPPX.OpenVINO.CSharp.API --version 3.3.0
dotnet add package JYPPX.OpenVINO.GenAI.runtime.win
```

## Conda Model Preparation / 使用 conda 准备模型

The samples themselves are C# projects. Conda is only used to prepare models, download assets, and convert images/audio.

示例本身是 C# 项目；conda 只用于准备模型、下载资源、转换图片或音频。

```powershell
conda create -n ov-genai-samples python=3.11 -y
conda activate ov-genai-samples
python -m pip install --upgrade pip
python -m pip install --upgrade-strategy eager "optimum-intel[openvino]" openvino-genai huggingface_hub transformers pillow soundfile nncf
```

### LLM Model / 文本生成模型

```powershell
mkdir models
optimum-cli export openvino --model TinyLlama/TinyLlama-1.1B-Chat-v1.0 --weight-format int4 --trust-remote-code models/TinyLlama-1.1B-Chat-v1.0-ov
$env:OPENVINO_GENAI_LLM_MODEL_DIR = "$PWD\models\TinyLlama-1.1B-Chat-v1.0-ov"
```

If Hugging Face already provides a converted OpenVINO model, you can download it directly:

如果 Hugging Face 已经提供转换好的 OpenVINO 模型，可以直接下载：

```powershell
hf download <openvino-model-id> --local-dir models\llm-ov
```

### Whisper Model and Audio / Whisper 模型和音频

```powershell
optimum-cli export openvino --trust-remote-code --model openai/whisper-tiny models/whisper-tiny
# Alternative / 也可以直接下载 OpenVINO 优化模型:
hf download OpenVINO/whisper-tiny-int8-ov --local-dir models/whisper-tiny-int8-ov

$env:OPENVINO_GENAI_WHISPER_MODEL_DIR = "$PWD\models\whisper-tiny"
```

Whisper samples expect WAV input. Use conda `ffmpeg` when you need to convert an audio file:

Whisper 示例需要 WAV 输入。如果需要转换音频，请用 conda 安装 `ffmpeg`：

```powershell
conda install -c conda-forge ffmpeg -y
ffmpeg -i input.mp3 -ac 1 -ar 16000 speech.wav
$env:OPENVINO_GENAI_AUDIO_PATH = "$PWD\speech.wav"
```

### VLM Model and Image / VLM 模型和图片

```powershell
optimum-cli export openvino --model Qwen/Qwen3-VL-2B-Instruct --trust-remote-code models/Qwen3-VL-2B-Instruct
$env:OPENVINO_GENAI_VLM_MODEL_DIR = "$PWD\models\Qwen3-VL-2B-Instruct"
```

The C# sample image loader is dependency-free and supports BMP or binary PPM/PNM. Convert JPG/PNG with Pillow:

C# 示例的图片读取器不引入额外 NuGet 依赖，支持 BMP 或二进制 PPM/PNM。JPG/PNG 可用 Pillow 转换：

```powershell
python -c "from PIL import Image; Image.open(r'input.jpg').convert('RGB').save(r'input.bmp')"
$env:OPENVINO_GENAI_IMAGE_PATH = "$PWD\input.bmp"
```

## Run Samples / 运行示例

Run from the repository root.

请在仓库根目录运行。

```powershell
dotnet run --project samples/GenAI/TextGeneration/Greedy/Greedy.csproj -- --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR" --prompt "The sky is blue because"
dotnet run --project samples/GenAI/TextGeneration/BeamSearch/BeamSearch.csproj -- --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR" --beams 4
dotnet run --project samples/GenAI/TextGeneration/Multinomial/Multinomial.csproj -- --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR" --temperature 0.8 --top-p 0.95 --top-k 50
dotnet run --project samples/GenAI/TextGeneration/Streaming/Streaming.csproj -- --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR"
dotnet run --project samples/GenAI/TextGeneration/Chat/Chat.csproj -- --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR"
dotnet run --project samples/GenAI/TextGeneration/Benchmark/Benchmark.csproj -- --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR" --iterations 3 --warmup 1
dotnet run --project samples/GenAI/WhisperSpeechRecognition/WhisperSpeechRecognition.csproj -- --model "$env:OPENVINO_GENAI_WHISPER_MODEL_DIR" --audio "$env:OPENVINO_GENAI_AUDIO_PATH" --timestamps true
dotnet run --project samples/GenAI/VisualLanguageChat/VisualLanguageChat.csproj -- --model "$env:OPENVINO_GENAI_VLM_MODEL_DIR" --image "$env:OPENVINO_GENAI_IMAGE_PATH" --interactive true
```

## Troubleshooting / 排错

- If `openvino_genai_c` cannot be found, set `OPENVINO_GENAI_RUNTIME_DIR` to the directory that contains `openvino_genai_c.dll`, `openvino_genai.dll`, `openvino_tokenizers.dll`, `openvino.dll`, and `openvino_c.dll`.
- If a model fails to load, verify the model directory contains a GenAI-compatible OpenVINO export and tokenizer files.
- If VLM image loading fails, convert the image to RGB BMP or binary PPM first.
- If Whisper output is empty or incorrect, confirm the audio contains clear speech and was converted to mono 16 kHz WAV.

- 如果找不到 `openvino_genai_c`，请把 `OPENVINO_GENAI_RUNTIME_DIR` 指向包含 `openvino_genai_c.dll`、`openvino_genai.dll`、`openvino_tokenizers.dll`、`openvino.dll`、`openvino_c.dll` 的目录。
- 如果模型加载失败，请确认模型目录是 GenAI 兼容的 OpenVINO 导出，并包含 tokenizer 文件。
- 如果 VLM 图片读取失败，请先把图片转换为 RGB BMP 或二进制 PPM。
- 如果 Whisper 输出为空或错误，请确认音频包含清晰语音，并已转换为 mono 16 kHz WAV。
