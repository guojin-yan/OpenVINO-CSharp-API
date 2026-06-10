# Whisper Speech Recognition / Whisper 语音识别

This project mirrors the official C Whisper sample and demonstrates
`WhisperPipeline`, `WhisperGenerationConfig`, decoded text, timestamp chunks,
and performance metrics.

本项目复刻官方 C Whisper 示例，演示 `WhisperPipeline`、
`WhisperGenerationConfig`、识别文本、时间戳分段和性能指标。

## Prepare Model and Audio / 准备模型和音频

```powershell
$modelRoot = "E:\LlmModel"
$whisper = Join-Path $modelRoot "whisper-tiny-int8-ov"
$audio = Join-Path $modelRoot "assets\how_are_you_doing_today.wav"
New-Item -ItemType Directory -Force (Split-Path $audio) | Out-Null

conda run -n PaddleOCR hf download OpenVINO/whisper-tiny-int8-ov `
  --local-dir $whisper

curl.exe -L `
  -o $audio `
  https://storage.openvinotoolkit.org/models_contrib/speech/2021.2/librispeech_s5/how_are_you_doing_today.wav
```

`PaddleOCR` is the conda environment that contains `hf` on this machine.
Replace it with your own Hugging Face CLI environment name if needed.

For your own audio, convert it to mono 16 kHz WAV:

如果使用自己的音频，先转换为 mono 16 kHz WAV：

```powershell
conda install -c conda-forge ffmpeg -y
ffmpeg -i input.mp3 -ac 1 -ar 16000 speech.wav
```

## Run / 运行

```powershell
dotnet run --project samples/GenAI/WhisperSpeechRecognition/WhisperSpeechRecognition.csproj --framework net8.0 -- `
  --model $whisper `
  --audio $audio `
  --device CPU `
  --language en `
  --task transcribe `
  --timestamps true
```

The sample accepts `--language`, `--task`, `--initial-prompt`, `--hotwords`, and
`--timestamps`.

示例支持 `--language`、`--task`、`--initial-prompt`、`--hotwords` 和
`--timestamps`。

## Expected Output / 预期输出

The validated sample audio should produce text similar to:

已验证音频的输出应类似：

```text
Result / 识别结果:
 How are you doing today?

Timestamp chunks / 时间戳分段:
[0.00, 2.00] How are you doing today?
```

## Notes / 说明

- You may pass `--language en`; the sample normalizes it to `<|en|>` before
  calling the native C API.
- You may also pass the native Whisper token form directly, for example
  `--language "<|en|>"`.
- `--timestamps true` prints decoded chunks when the model/runtime returns
  timestamp information.

- 可以传入 `--language en`；示例会在调用原生 C API 前转换为 `<|en|>`。
- 也可以直接传入原生 Whisper token 写法，例如 `--language "<|en|>"`。
- `--timestamps true` 会在模型和 runtime 返回时间戳信息时输出分段结果。
