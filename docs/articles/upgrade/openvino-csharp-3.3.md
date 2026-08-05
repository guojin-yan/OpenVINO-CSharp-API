# OpenVINO C# API 3.3 Upgrade Guide

OpenVINO C# API 3.3 keeps the core inference APIs compatible with 3.2 and adds
managed OpenVINO GenAI support for .NET applications.

The 3.3.1 maintenance update aligns runtime automation and documentation with
OpenVINO 2026.3. The core C ABI is unchanged. OpenVINO GenAI 2026.3 adds the
VLM history export already exposed by `GenerateWithHistory`; `StartChat()` and
`FinishChat()` remain compatibility helpers but are now marked obsolete.

## What changed

| Area | 3.2 | 3.3 |
|---|---|---|
| Core inference | OpenVINO C API wrapper | Broader Core, Model, Tensor, InferRequest, preprocessing coverage |
| C# API style | Existing compatible naming | Additional PascalCase convenience APIs |
| GenAI | Not covered as a structured API | `OpenVinoSharp.GenAI` for LLM, Whisper, and VLM flows |
| Runtime packages | Core OpenVINO runtime packages | Optional GenAI runtime packages |
| Samples | Traditional inference samples | Structured `samples/GenAI` projects |

## GenAI is optional

Applications that only use `Core`, `Model`, `Tensor`, `CompiledModel`, and
`InferRequest` do not load `openvino_genai_c`. Add the GenAI runtime package
only when calling APIs under `OpenVinoSharp.GenAI`.

## Package guidance

For core inference:

```xml
<PackageReference Include="JYPPX.OpenVINO.CSharp.API" Version="3.3.1" />
<PackageReference Include="OpenVINO.runtime.win" Version="2026.3.0" />
```

For GenAI on Windows:

```xml
<PackageReference Include="JYPPX.OpenVINO.CSharp.API" Version="3.3.1" />
<PackageReference Include="JYPPX.OpenVINO.GenAI.runtime.win" Version="2026.3.0" />
```

## Model paths in samples

The GenAI samples accept explicit command-line paths. A portable tutorial setup
can define one model root and pass paths to each sample:

```powershell
$modelRoot = "E:\LlmModel"
$llm = Join-Path $modelRoot "TinyLlama-1.1B-Chat-v1.0-int4-ov"

dotnet run --project samples/GenAI/TextGeneration/Greedy/Greedy.csproj --framework net8.0 -- `
  --model $llm `
  --device CPU
```

## Migration checklist

1. Upgrade `JYPPX.OpenVINO.CSharp.API` to `3.3.1` and the native runtime to `2026.3.0`.
2. Keep existing core inference code unchanged and run your current tests.
3. Use the new PascalCase APIs for new code where convenient.
4. Add `JYPPX.OpenVINO.GenAI.runtime.*` only for GenAI scenarios. Prefer
   `ChatHistory` plus `GenerateWithHistory()` for multi-turn VLM/LLM code.
5. Pass model and media paths explicitly in sample commands or application
   configuration; avoid hard-coded developer-machine paths.
