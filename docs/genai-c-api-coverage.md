# OpenVINO GenAI C API Coverage / GenAI C API 覆盖矩阵

Source headers:

```text
E:\OpenVINOSharp\openvino\openvino.genai-master\src\c\include\openvino\genai\c
```

## Covered In Phase 2 / Phase 2 已覆盖

| Header | C# wrapper | Notes |
| --- | --- | --- |
| `generation_config.h` | `GenerationConfig` | UTF-8 JSON path, size_t as `UIntPtr`, C bool as 1 byte. |
| `llm_pipeline.h` | `LLMPipeline`, `DecodedResults` | Prompt generation, chat history generation, streaming callback, config get/set. |
| `perf_metrics.h` | `PerformanceMetrics` | Returned metrics are owned and released through the matching GenAI free function. |
| `json_container.h` | `JsonContainer` | Two-call UTF-8 JSON string bridge. |
| `chat_history.h` | `ChatHistory` | Message list, tools, extra context, JSON helper methods. |

## Audited In Phase 3 / Phase 3 已审计

| Header | Status | Decision |
| --- | --- | --- |
| `vlm_pipeline.h` | Not wrapped yet | ABI uses `ov_tensor_t**` image arrays plus text/history and streaming. It should be implemented with image tensor tests in a media-focused phase. |
| `whisper_generation_config.h` | Not wrapped yet | Low-risk config API, good candidate for the next API expansion. Needs careful optional string handling for NOT_FOUND and bool marshalling. |
| `whisper_pipeline.h` | Not wrapped yet | ABI uses raw `float*` speech buffers and result chunks. It should be implemented together with audio buffer tests and model-gated integration tests. |
| Tokenizer C headers | No standalone header found | Tokenizer runtime is packaged through `openvino_tokenizers.dll`; no separate C wrapper header exists under `src\c`. |

## ABI Rules / ABI 规则

- `size_t` maps to `UIntPtr`.
- C `bool` must use 1-byte marshalling.
- `char*` and `const char*` are explicit UTF-8 `IntPtr`.
- Two-call string APIs should use managed temporary buffers and trim the trailing null.
- Owned native objects must inherit or follow `DisposableOvObject` ownership patterns.
- Borrowed pointers must be documented in XML comments.

## Next API Work / 下一步 API 工作

Suggested order:

1. Add `WhisperGenerationConfig`, because it can be validated without media/model files.
2. Add `WhisperDecodedResults` and chunk wrappers.
3. Add `WhisperPipeline.Generate(float[])` with audio buffer tests.
4. Add `VLMDecodedResults` and `VLMPipeline` with `Tensor[]` image input tests.
