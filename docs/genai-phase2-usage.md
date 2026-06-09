# OpenVINO GenAI Phase 2 Usage

## Overview / 概览

Phase 2 adds the first C# wrapper for the OpenVINO GenAI C API under `OpenVinoSharp.GenAI`.

本阶段在 `OpenVinoSharp.GenAI` 命名空间下加入 OpenVINO GenAI C API 的第一版 C# 封装。

Covered in this phase:

- `GenerationConfig`
- `LLMPipeline`
- `DecodedResults`
- `PerformanceMetrics`
- `JsonContainer`
- `ChatHistory`
- `WhisperGenerationConfig`

Covered after Phase 2:

- GenAI runtime NuGet packaging: see `docs/articles/installation/genai-runtime.md`.

Not covered yet:

- VLM pipeline
- Whisper pipeline

## Runtime Loading / Runtime 加载

Set `OPENVINO_GENAI_RUNTIME_DIR` to the extracted GenAI runtime root, or pass the full `openvino_genai_c` path:

设置 `OPENVINO_GENAI_RUNTIME_DIR` 到 GenAI runtime 解压目录，或显式传入 `openvino_genai_c` 完整路径：

```csharp
using OpenVinoSharp.GenAI;

OpenVinoSharp.GenAI.GenAI.Initialize();

// Or:
OpenVinoSharp.GenAI.GenAI.Initialize(@"E:\OpenVINOSharp\openvino\openvino_genai_windows_2026.2.0.0_x86_64\runtime\bin\intel64\Release\openvino_genai_c.dll");
```

## Text Generation / 文本生成

```csharp
using OpenVinoSharp.GenAI;

OpenVinoSharp.GenAI.GenAI.Initialize();

using var config = new GenerationConfig()
    .SetMaxNewTokens(128)
    .SetTemperature(0.7f)
    .SetTopP(0.9f)
    .SetStopStrings("</s>");

using var pipe = new LLMPipeline(@"D:\models\qwen2.5-ov", "CPU");
using DecodedResults results = pipe.Generate("你好，请介绍 OpenVINO。", config);

Console.WriteLine(results.Text);
```

## Whisper Generation Config / Whisper 生成配置

`WhisperGenerationConfig` wraps `ov_genai_whisper_generation_config`. It is a configuration-only wrapper in this phase;
Whisper audio pipeline inference will be added separately.

`WhisperGenerationConfig` 封装 `ov_genai_whisper_generation_config`。本阶段只提供配置对象封装，Whisper 音频推理
pipeline 会在后续阶段单独实现。

```csharp
using OpenVinoSharp.GenAI;

OpenVinoSharp.GenAI.GenAI.Initialize();

using var whisperConfig = new WhisperGenerationConfig()
    .SetLanguage("zh")
    .SetTask("transcribe")
    .SetReturnTimestamps(true)
    .SetInitialPrompt("你好 OpenVINO")
    .SetHotwords("OpenVINO 热词")
    .SetBeginSuppressTokens(220, 50257)
    .SetSuppressTokens(1, 2, 3);

using GenerationConfig textConfig = whisperConfig.GetGenerationConfig();
textConfig.SetMaxNewTokens(128);

Console.WriteLine(whisperConfig.Language);
```

## Streaming / 流式输出

```csharp
using var pipe = new LLMPipeline(@"D:\models\qwen2.5-ov", "GPU");

using DecodedResults results = pipe.Generate(
    "Write a short introduction for OpenVINO GenAI.",
    text =>
    {
        Console.Write(text);
        return StreamingStatus.Running;
    });
```

## Chat History / 聊天历史

```csharp
using var history = new ChatHistory()
    .AddUserMessage("你好")
    .AddAssistantMessage("你好，请问有什么可以帮你？")
    .AddUserMessage("请用一句话解释 OpenVINO。");

using var pipe = new LLMPipeline(@"D:\models\chat-model-ov", "CPU");
using DecodedResults results = pipe.GenerateWithHistory(history);
Console.WriteLine(results.Text);
```
