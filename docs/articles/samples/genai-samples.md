# GenAI Samples (.NET 8.0) / GenAI 示例

The old single QuickStart sample has been replaced by a structured `samples/GenAI` tree. The new samples mirror the currently wrapped OpenVINO GenAI C API scenarios and are easier to reproduce independently.

旧的单一 QuickStart 示例已替换为结构化的 `samples/GenAI` 目录。新的示例按当前已经封装的 OpenVINO GenAI C API 场景拆分，便于独立复现。

## Covered Samples / 覆盖示例

| Folder | Scenario |
|---|---|
| `samples/GenAI/TextGeneration/Greedy` | Greedy LLM text generation |
| `samples/GenAI/TextGeneration/BeamSearch` | Beam search decoding |
| `samples/GenAI/TextGeneration/Multinomial` | Sampling with temperature, top-p, top-k, seed |
| `samples/GenAI/TextGeneration/Streaming` | Streaming callback |
| `samples/GenAI/TextGeneration/Chat` | Multi-turn chat with `ChatHistory` |
| `samples/GenAI/TextGeneration/Benchmark` | Warmup, repeated generation, performance metrics |
| `samples/GenAI/WhisperSpeechRecognition` | Whisper speech recognition |
| `samples/GenAI/VisualLanguageChat` | VLM image chat |

Image generation, RAG, speech generation, and video generation are not added yet because their managed pipeline wrappers are not complete.

图像生成、RAG、语音生成和视频生成暂未添加，因为对应的托管 pipeline 封装还没有完成。

## Reproducible Setup / 可复现环境

The samples are C# projects. Use conda only for model export and media conversion:

示例是 C# 项目。conda 只用于模型导出和媒体转换：

```powershell
conda create -n ov-genai-samples python=3.11 -y
conda activate ov-genai-samples
python -m pip install --upgrade pip
python -m pip install --upgrade-strategy eager "optimum-intel[openvino]" openvino-genai huggingface_hub transformers pillow soundfile nncf
conda install -c conda-forge ffmpeg -y
```

When running from the repository source tree, set:

从源码目录运行时设置：

```powershell
$env:OPENVINO_GENAI_RUNTIME_DIR = "E:\OpenVINOSharp\openvino\openvino_genai_windows_2026.2.0.0_x86_64\runtime\bin\intel64\Release"
$env:OPENVINO_GENAI_DEVICE = "CPU"
```

## Models / 模型

```powershell
# LLM
optimum-cli export openvino --model TinyLlama/TinyLlama-1.1B-Chat-v1.0 --weight-format int4 --trust-remote-code models/TinyLlama-1.1B-Chat-v1.0-ov
$env:OPENVINO_GENAI_LLM_MODEL_DIR = "$PWD\models\TinyLlama-1.1B-Chat-v1.0-ov"

# Whisper
optimum-cli export openvino --trust-remote-code --model openai/whisper-tiny models/whisper-tiny
hf download OpenVINO/whisper-tiny-int8-ov --local-dir models/whisper-tiny-int8-ov
$env:OPENVINO_GENAI_WHISPER_MODEL_DIR = "$PWD\models\whisper-tiny"

# VLM
optimum-cli export openvino --model Qwen/Qwen3-VL-2B-Instruct --trust-remote-code models/Qwen3-VL-2B-Instruct
$env:OPENVINO_GENAI_VLM_MODEL_DIR = "$PWD\models\Qwen3-VL-2B-Instruct"
```

## Media / 媒体文件

```powershell
# Audio to mono 16 kHz WAV
ffmpeg -i input.mp3 -ac 1 -ar 16000 speech.wav
$env:OPENVINO_GENAI_AUDIO_PATH = "$PWD\speech.wav"

# JPG/PNG to RGB BMP for the dependency-free C# VLM loader
python -c "from PIL import Image; Image.open(r'input.jpg').convert('RGB').save(r'input.bmp')"
$env:OPENVINO_GENAI_IMAGE_PATH = "$PWD\input.bmp"
```

## Commands / 运行命令

```powershell
dotnet run --project samples/GenAI/TextGeneration/Greedy/Greedy.csproj -- --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR"
dotnet run --project samples/GenAI/TextGeneration/BeamSearch/BeamSearch.csproj -- --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR" --beams 4
dotnet run --project samples/GenAI/TextGeneration/Multinomial/Multinomial.csproj -- --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR" --temperature 0.8 --top-p 0.95 --top-k 50
dotnet run --project samples/GenAI/TextGeneration/Streaming/Streaming.csproj -- --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR"
dotnet run --project samples/GenAI/TextGeneration/Chat/Chat.csproj -- --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR"
dotnet run --project samples/GenAI/TextGeneration/Benchmark/Benchmark.csproj -- --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR" --iterations 3 --warmup 1
dotnet run --project samples/GenAI/WhisperSpeechRecognition/WhisperSpeechRecognition.csproj -- --model "$env:OPENVINO_GENAI_WHISPER_MODEL_DIR" --audio "$env:OPENVINO_GENAI_AUDIO_PATH" --timestamps true
dotnet run --project samples/GenAI/VisualLanguageChat/VisualLanguageChat.csproj -- --model "$env:OPENVINO_GENAI_VLM_MODEL_DIR" --image "$env:OPENVINO_GENAI_IMAGE_PATH" --interactive true
```

More detailed notes are in `samples/GenAI/README.md`.

更多细节见 `samples/GenAI/README.md`。
