# OpenVINO GenAI Usage / GenAI 使用说明

## Overview / 概览

The `OpenVinoSharp.GenAI` namespace wraps the OpenVINO GenAI C API in the same repository as the core OpenVINO C# API. The GenAI native library remains optional: applications that only use `Core`, `Model`, `Tensor`, `CompiledModel`, or `InferRequest` do not load `openvino_genai_c`.

`OpenVinoSharp.GenAI` 命名空间封装 OpenVINO GenAI C API，并与基础 OpenVINO C# API 放在同一仓库中。GenAI 原生库保持可选加载：只使用 `Core`、`Model`、`Tensor`、`CompiledModel` 或 `InferRequest` 的应用不会加载 `openvino_genai_c`。

Covered wrappers:

- `GenerationConfig`
- `LLMPipeline`
- `DecodedResults`
- `PerformanceMetrics`
- `JsonContainer`
- `ChatHistory`
- `WhisperGenerationConfig`
- `WhisperPipeline`
- `WhisperDecodedResults`
- `WhisperDecodedResultChunk`
- `VLMPipeline`
- `VLMDecodedResults`

## Runtime Loading / Runtime 加载

Install a GenAI runtime package when using `OpenVinoSharp.GenAI`:

```bash
dotnet add package JYPPX.OpenVINO.CSharp.API
dotnet add package JYPPX.OpenVINO.GenAI.runtime.win
```

Use the platform-specific GenAI runtime package for normal applications. Set
`OPENVINO_GENAI_RUNTIME_DIR` or initialize with an explicit library path only
when validating a local native runtime build.

正常应用请安装对应平台的 GenAI runtime NuGet 包。只有在验证本地 native runtime 构建时，
才需要设置 `OPENVINO_GENAI_RUNTIME_DIR` 或传入显式库路径。

```csharp
using OpenVinoSharp.GenAI;

GenAI.Initialize();

// Optional local native runtime override for diagnostics only.
GenAI.Initialize(
    @"D:\local-runtimes\openvino_genai\runtime\bin\intel64\Release\openvino_genai_c.dll");
```

## Text Generation / 文本生成

```csharp
using OpenVinoSharp.GenAI;

GenAI.Initialize();

using var config = new GenerationConfig()
    .SetMaxNewTokens(128)
    .SetTemperature(0.7f)
    .SetTopP(0.9f)
    .SetStopStrings("</s>");

using var pipe = new LLMPipeline(@"D:\models\qwen2.5-ov", "CPU");
using DecodedResults results = pipe.Generate("你好，请介绍 OpenVINO。", config);

Console.WriteLine(results.Text);
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

## Whisper / 语音识别

`WhisperGenerationConfig` configures Whisper decoding. `WhisperPipeline` accepts raw `float` speech samples and returns `WhisperDecodedResults`.

`WhisperGenerationConfig` 用于配置 Whisper 解码行为。`WhisperPipeline` 接收原始 `float` 语音采样并返回 `WhisperDecodedResults`。

```csharp
using OpenVinoSharp.GenAI;

GenAI.Initialize();

using var whisperConfig = new WhisperGenerationConfig()
    .SetLanguage("zh")
    .SetTask("transcribe")
    .SetReturnTimestamps(true)
    .SetInitialPrompt("你好 OpenVINO")
    .SetHotwords("OpenVINO 热词");

using var pipe = new WhisperPipeline(@"D:\models\whisper-ov", "CPU");
using WhisperDecodedResults results = pipe.Generate(rawSpeechFloatArray, whisperConfig);

Console.WriteLine(results.GetString());

if (results.HasChunks)
{
    for (ulong i = 0; i < results.ChunkCount; i++)
    {
        using WhisperDecodedResultChunk? chunk = results.GetChunkAt(i);
        Console.WriteLine($"{chunk?.StartTimestamp}-{chunk?.EndTimestamp}: {chunk?.Text}");
    }
}
```

## VLM / 视觉语言模型

`VLMPipeline` supports text-only prompts, optional image tensors, chat-history generation, and streaming callbacks. Image tensors are borrowed during the call; callers still own and dispose the `Tensor` instances.

`VLMPipeline` 支持纯文本 prompt、可选图像 Tensor、聊天历史生成和流式回调。图像 Tensor 在调用期间是借用关系，仍由调用方负责释放。

```csharp
using OpenVinoSharp;
using OpenVinoSharp.GenAI;

GenAI.Initialize();

using var config = new GenerationConfig().SetMaxNewTokens(64);
using var pipe = new VLMPipeline(@"D:\models\vlm-ov", "CPU");

// Optional image tensors. They remain owned by the caller.
Tensor[] images = LoadImageTensors();

using VLMDecodedResults results = pipe.Generate("Describe this image.", images, config);
Console.WriteLine(results.Text);
```

## Model-Gated Tests / 模型门控测试

Pipeline integration tests are skipped unless model directories are configured:

```powershell
$env:OPENVINO_GENAI_WHISPER_MODEL_DIR = "D:\models\whisper-ov"
$env:OPENVINO_GENAI_VLM_MODEL_DIR = "D:\models\vlm-ov"
dotnet test tests/OpenVINO.CSharp.API.Tests/OpenVINO.CSharp.API.Tests.csproj --framework net8.0
```
