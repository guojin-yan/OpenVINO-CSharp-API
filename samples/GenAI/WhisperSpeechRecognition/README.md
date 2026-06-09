# Whisper Speech Recognition / Whisper 语音识别

This project mirrors the official C Whisper sample and demonstrates `WhisperPipeline`, `WhisperGenerationConfig`, decoded text, timestamp chunks, and performance metrics.

本项目复刻官方 C Whisper 示例，演示 `WhisperPipeline`、`WhisperGenerationConfig`、识别文本、时间戳分段和性能指标。

```powershell
conda activate ov-genai-samples
optimum-cli export openvino --trust-remote-code --model openai/whisper-tiny models/whisper-tiny
hf download OpenVINO/whisper-tiny-int8-ov --local-dir models/whisper-tiny-int8-ov

conda install -c conda-forge ffmpeg -y
ffmpeg -i input.mp3 -ac 1 -ar 16000 speech.wav

dotnet run --project samples/GenAI/WhisperSpeechRecognition/WhisperSpeechRecognition.csproj -- --model models/whisper-tiny --audio speech.wav --timestamps true
```

The sample accepts `--language`, `--task`, `--initial-prompt`, `--hotwords`, and `--timestamps`.

示例支持 `--language`、`--task`、`--initial-prompt`、`--hotwords` 和 `--timestamps`。
