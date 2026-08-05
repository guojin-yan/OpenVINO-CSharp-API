# OpenVINO GenAI Samples Runbook

This runbook records the reproducible local workflow for `samples/GenAI`. It is a validation manual for maintainers and contributors: prepare runtime, prepare models and media, run every C# sample, and inspect the generated logs.

For publishable technical articles, see:

- `docs/articles/samples/genai-text-generation-tutorial.md`
- `docs/articles/samples/genai-whisper-tutorial.md`
- `docs/articles/samples/genai-vlm-tutorial.md`

## 1. Validated Environment

| Item | Value |
|---|---|
| Repository | `E:\GitSpace\OpenVINO-CSharp-API-csharp3.3\OpenVINO-CSharp-API` |
| Target framework | `net8.0` |
| Device | `CPU` |
| GenAI runtime package | `JYPPX.OpenVINO.GenAI.runtime.win` `2026.3.0` |
| Model root | `E:\LlmModel` |
| LLM model | `E:\LlmModel\TinyLlama-1.1B-Chat-v1.0-int4-ov` |
| Whisper model | `E:\LlmModel\whisper-tiny-int8-ov` |
| VLM model | `E:\LlmModel\InternVL2-1B-int4-ov` |
| Audio | `E:\LlmModel\assets\how_are_you_doing_today.wav` |
| Image | `E:\LlmModel\assets\color_blocks_30.ppm` |

Validation logs are written to:

```text
out\genai-samples-validation
```

## 2. Runtime Package

The C# samples restore the published Windows GenAI runtime package through `samples/GenAI/Directory.Build.props`. No local native runtime directory is required for the normal validation path.

Use a local native runtime only when debugging runtime packaging or native ABI changes. In that case, pass `-RuntimeDir` to `RunAllSamples.ps1` or set `OPENVINO_GENAI_RUNTIME_DIR`.

The runtime directory must contain the GenAI C API library and the dependent OpenVINO runtime libraries, including files such as:

```text
openvino_genai_c.dll
openvino_genai.dll
openvino_tokenizers.dll
openvino.dll
openvino_c.dll
tbb12.dll
openvino_intel_cpu_plugin.dll
```

## 3. Tools For Model And Media Preparation

The C# samples do not require Python at runtime. Python tools are used only to download/export models and convert media files.

```powershell
conda create -n ov-genai-samples python=3.11 -y
conda activate ov-genai-samples
python -m pip install --upgrade pip
python -m pip install --upgrade "huggingface_hub[cli]" pillow soundfile
python -m pip install --upgrade-strategy eager "optimum-intel[openvino]" openvino-genai transformers nncf
conda install -c conda-forge ffmpeg -y
```

Set common paths:

```powershell
cd E:\GitSpace\OpenVINO-CSharp-API-csharp3.3\OpenVINO-CSharp-API

$modelRoot = "E:\LlmModel"
$llm = Join-Path $modelRoot "TinyLlama-1.1B-Chat-v1.0-int4-ov"
$whisper = Join-Path $modelRoot "whisper-tiny-int8-ov"
$vlm = Join-Path $modelRoot "InternVL2-1B-int4-ov"
$audio = Join-Path $modelRoot "assets\how_are_you_doing_today.wav"
$image = Join-Path $modelRoot "assets\color_blocks_30.ppm"
```

## 4. Prepare Models

Download the LLM model:

```powershell
conda run -n PaddleOCR hf download OpenVINO/TinyLlama-1.1B-Chat-v1.0-int4-ov --local-dir $llm
```

Download the Whisper model:

```powershell
conda run -n PaddleOCR hf download OpenVINO/whisper-tiny-int8-ov --local-dir $whisper
```

Download the VLM model:

```powershell
conda run -n PaddleOCR hf download OpenVINO/InternVL2-1B-int4-ov --local-dir $vlm
```

The commands above use `PaddleOCR` because this machine has `hf` /
`huggingface-cli` installed in that conda environment. Replace it with your
own environment name when needed.

Expected LLM files include:

```text
openvino_model.xml
openvino_model.bin
openvino_tokenizer.xml
openvino_tokenizer.bin
openvino_detokenizer.xml
openvino_detokenizer.bin
tokenizer.json
generation_config.json
```

## 5. Prepare Media

Create the asset directory and download the validated Whisper audio:

```powershell
New-Item -ItemType Directory -Force (Split-Path $audio) | Out-Null

curl.exe -L `
  -o $audio `
  https://storage.openvinotoolkit.org/models_contrib/speech/2021.2/librispeech_s5/how_are_you_doing_today.wav
```

For custom audio, convert to mono 16 kHz WAV:

```powershell
ffmpeg -i input.mp3 -ac 1 -ar 16000 speech.wav
```

The VLM image loader supports RGB BMP and binary PPM/PNM. Convert JPG or PNG files with Pillow:

```powershell
python -c "from PIL import Image; Image.open(r'input.jpg').convert('RGB').save(r'input.bmp')"
$image = Join-Path $PWD "input.bmp"
```

## 6. Run All Samples

```powershell
powershell -ExecutionPolicy Bypass -File samples\GenAI\RunAllSamples.ps1 `
  -LlmModelDir $llm `
  -WhisperModelDir $whisper `
  -VlmModelDir $vlm `
  -AudioPath $audio `
  -ImagePath $image `
  -Device CPU
```

The script publishes each sample, runs the generated executable, and writes:

```text
01-greedy.log
02-beam-search.log
03-multinomial.log
04-streaming.log
05-benchmark.log
06-chat.log
07-greedy-zh.log
08-chat-zh.log
09-whisper.log
10-vlm-single.log
11-vlm-interactive.log
12-vlm-zh.log
```

On Windows machines with application control policies, direct `dotnet run` can be blocked for newly built DLLs. The batch script avoids that local issue by publishing each sample and running the generated executable.

## 7. Individual Commands

Greedy:

```powershell
dotnet run --project samples/GenAI/TextGeneration/Greedy/Greedy.csproj --framework net8.0 -- `
  --model $llm `
  --prompt "What is OpenVINO?" `
  --device CPU `
  --max-new-tokens 64
```

Beam Search:

```powershell
dotnet run --project samples/GenAI/TextGeneration/BeamSearch/BeamSearch.csproj --framework net8.0 -- `
  --model $llm `
  --prompt "OpenVINO is" `
  --device CPU `
  --max-new-tokens 64 `
  --beams 4
```

Multinomial:

```powershell
dotnet run --project samples/GenAI/TextGeneration/Multinomial/Multinomial.csproj --framework net8.0 -- `
  --model $llm `
  --prompt "OpenVINO helps developers" `
  --device CPU `
  --max-new-tokens 64 `
  --temperature 0.8 `
  --top-p 0.95 `
  --top-k 50 `
  --seed 7
```

Streaming:

```powershell
dotnet run --project samples/GenAI/TextGeneration/Streaming/Streaming.csproj --framework net8.0 -- `
  --model $llm `
  --prompt "List three OpenVINO benefits." `
  --device CPU `
  --max-new-tokens 96
```

Benchmark:

```powershell
dotnet run --project samples/GenAI/TextGeneration/Benchmark/Benchmark.csproj --framework net8.0 -- `
  --model $llm `
  --prompt "OpenVINO is" `
  --device CPU `
  --max-new-tokens 64 `
  --iterations 3 `
  --warmup 1
```

Chat:

```powershell
dotnet run --project samples/GenAI/TextGeneration/Chat/Chat.csproj --framework net8.0 -- `
  --model $llm `
  --device CPU `
  --max-new-tokens 96 `
  --turn "OpenVINO 适合部署在哪些设备上？请用中文回答。" `
  --turn "请用中文列出三个 OpenVINO 关键词。"
```

Whisper:

```powershell
dotnet run --project samples/GenAI/WhisperSpeechRecognition/WhisperSpeechRecognition.csproj --framework net8.0 -- `
  --model $whisper `
  --audio $audio `
  --device CPU `
  --language en `
  --task transcribe `
  --timestamps true
```

VLM single-turn:

```powershell
dotnet run --project samples/GenAI/VisualLanguageChat/VisualLanguageChat.csproj --framework net8.0 -- `
  --model $vlm `
  --image $image `
  --device CPU `
  --prompt "Describe the image in one sentence." `
  --max-new-tokens 96
```

VLM Chinese prompt:

```powershell
dotnet run --project samples/GenAI/VisualLanguageChat/VisualLanguageChat.csproj --framework net8.0 -- `
  --model $vlm `
  --image $image `
  --device CPU `
  --prompt "请用中文描述这张图片。" `
  --max-new-tokens 96
```

VLM interactive:

```powershell
dotnet run --project samples/GenAI/VisualLanguageChat/VisualLanguageChat.csproj --framework net8.0 -- `
  --model $vlm `
  --image $image `
  --device CPU `
  --interactive true `
  --max-new-tokens 96
```

## 8. Expected Checks

After a successful validation run:

- Text generation logs contain generated text and metrics.
- Chinese text generation logs contain readable Chinese output when the selected model supports it.
- Streaming logs show incremental token output.
- Benchmark logs contain warmup and measured iterations.
- Whisper logs contain `How are you doing today?`.
- VLM logs contain non-empty generated text when using a real VLM model.
- `12-vlm-zh.log` contains a Chinese image description with `InternVL2-1B-int4-ov`.

Tiny random VLM models are valid only for ABI smoke tests. Use `--allow-empty true` only when intentionally testing that path.

## 9. Troubleshooting

- `openvino_genai_c.dll` not found: confirm the GenAI runtime package is restored or set `OPENVINO_GENAI_RUNTIME_DIR`.
- `DllNotFoundException` for `openvino.dll` or plugins: use the full runtime directory, not only the folder containing `openvino_genai_c.dll`.
- Model load failure: verify the model directory contains OpenVINO IR files and tokenizer/detokenizer files.
- Whisper language error: use `--language en`; the sample normalizes it to `<|en|>`.
- Whisper empty output: confirm the WAV is mono 16 kHz and contains clear speech.
- VLM image load failure: convert the image to RGB BMP or binary PPM/PNM.
- VLM empty output: use a real VLM model and a meaningful image.
- Slow first run: model compilation and CPU cache warmup can dominate the first iteration.
- Chinese LLM quality is model-dependent. TinyLlama is useful for lightweight smoke tests, but production Chinese chat should use a Chinese or multilingual model converted to OpenVINO format.
