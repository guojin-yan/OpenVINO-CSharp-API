# OpenVINO GenAI C API Coverage / GenAI C API 覆盖矩阵

Source headers:

```text
E:\OpenVINOSharp\openvino\openvino.genai-master\src\c\include\openvino\genai\c
```

## Covered / 已覆盖

| Header | C# wrapper | Ownership and ABI notes |
| --- | --- | --- |
| `generation_config.h` | `GenerationConfig` | `size_t` maps to `UIntPtr`; C bool uses 1-byte marshalling; UTF-8 strings are explicitly allocated and freed. |
| `llm_pipeline.h` | `LLMPipeline`, `DecodedResults` | Pipeline and decoded results own native pointers; streaming callbacks are kept alive for the native call. |
| `perf_metrics.h` | `PerformanceMetrics` | Metrics returned by result objects are owned by managed wrappers and released with the matching native free function. |
| `json_container.h` | `JsonContainer` | JSON strings use the two-call UTF-8 buffer pattern. |
| `chat_history.h` | `ChatHistory` | Message JSON containers returned from native are owned by managed wrappers. |
| `whisper_generation_config.h` | `WhisperGenerationConfig` | Optional native strings map `NOT_FOUND` to `null`; token arrays are pinned only for the native call. |
| `whisper_pipeline.h` | `WhisperPipeline`, `WhisperDecodedResults`, `WhisperDecodedResultChunk` | Raw audio buffers are pinned for `generate`; result chunks and metrics are owned by returned wrappers. Whisper metrics use the exported decoded-results metrics free function because 2026.3 does not export a generic metrics free symbol. |
| `vlm_pipeline.h` | `VLMPipeline`, `VLMDecodedResults` | Image tensors are borrowed from callers; only the pointer array is pinned during native calls. |
| `visibility.h` | No managed wrapper required | Macro-only export/visibility header. |

## Runtime Loading / Runtime 加载

- 基础 OpenVINO API 不会主动加载 `openvino_genai_c`。
- GenAI runtime only loads when code calls `OpenVinoSharp.GenAI` APIs such as `GenAI.Initialize`, `GenerationConfig`, `LLMPipeline`, `WhisperPipeline`, or `VLMPipeline`.
- `OPENVINO_GENAI_RUNTIME_DIR` can point to a GenAI runtime root for local development, but official NuGet runtime packages are built from GitHub Actions downloads of Intel archives.

## ABI Rules / ABI 规则

- `size_t` maps to `UIntPtr`, with public `ulong` helpers where convenient.
- C `bool` maps to a 1-byte value (`byte`) at the P/Invoke boundary.
- `char*` and `const char*` are represented as `IntPtr` and converted with explicit UTF-8 helpers.
- `char**` and arrays are pinned or allocated for the shortest possible native call scope.
- Owned native pointers use `DisposableOvObject` and matching `*_free` functions.
- Borrowed native pointers, such as VLM input tensors, remain owned by the caller and are documented in XML comments.
- OpenVINO GenAI 2026.3 exports `ov_genai_vlm_pipeline_generate_with_history`; the managed VLM sample uses this history-based API. The older `start_chat`/`finish_chat` entry points remain available but are marked obsolete by the managed wrapper.

## Test Coverage / 测试覆盖

- Runtime-gated unit tests cover config objects, JSON containers, chat history, default decoded result objects, metrics release paths, and constructor validation.
- Model-gated integration tests use:
  - `OPENVINO_GENAI_WHISPER_MODEL_DIR`
  - `OPENVINO_GENAI_VLM_MODEL_DIR`
- When model variables are absent, xUnit marks those pipeline integration tests as skipped.
