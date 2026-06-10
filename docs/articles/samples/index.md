# 案例应用

本节汇总 OpenVINO C# API 的案例应用，覆盖两类主线：传统视觉推理和 OpenVINO GenAI。YOLO 示例展示跨 .NET Framework、.NET Core 和现代 .NET 的推理工程写法；GenAI 示例展示文本生成、语音识别和视觉语言问答如何进入 C# 应用。

## 案例列表

| 案例 | 框架 | 特性 | 说明 |
|---|---|---|---|
| [OpenVINO GenAI C# 示例总览](genai-samples.md) | .NET 8.0 | LLM、Streaming、Chat、Benchmark、Whisper、VLM | GenAI 能力入口和批量复现 |
| [GenAI 文本生成](genai-text-generation-tutorial.md) | .NET 8.0 | TinyLlama、Greedy、Beam Search、Sampling、Streaming | 本地 LLM 文本生成技术文章 |
| [GenAI Whisper 语音识别](genai-whisper-tutorial.md) | .NET 8.0 | Whisper、WAV、timestamps | 本地 ASR 和时间戳输出 |
| [GenAI 视觉语言问答](genai-vlm-tutorial.md) | .NET 8.0 | InternVL2、image tensor、VLM chat | 图片理解和交互式问答 |
| [YOLO Detection (.NET 10.0)](yolo-net10.md) | .NET 10.0 | Span<T>、IAsyncEnumerable、Parallel.ForEachAsync | 最新 .NET 版本的高性能实现 |
| [YOLO Detection (.NET 4.8)](yolo-net48.md) | .NET Framework 4.8 | Span<T>、async/await | 完整功能的 .NET Framework 实现 |
| [YOLO Detection (.NET 4.6)](yolo-net46.md) | .NET Framework 4.6 | 传统异步模式 | 兼容旧版 .NET Framework 的实现 |
| [YOLO Detection (.NET Core 3.1)](yolo-netcoreapp31.md) | .NET Core 3.1 | Span<T>、Memory<T>、IAsyncEnumerable | 跨平台 .NET Core 实现 |

## GenAI 示例亮点

OpenVINO C# API 3.3 的 GenAI 示例使用 `OpenVinoSharp.GenAI` 命名空间，覆盖 `LLMPipeline`、`WhisperPipeline` 和 `VLMPipeline`。这些示例强调可部署性：C# 项目通过 NuGet 恢复托管 API 和 native runtime，模型通过本地目录传入，命令行参数明确，适合从文章直接复制到本地验证。

GenAI runtime 是可选依赖。只使用 `Core`、`Model`、`Tensor`、`CompiledModel`、`InferRequest` 等传统推理 API 时，不会加载 `openvino_genai_c`。

## YOLO 示例亮点

YOLO 示例展示 OpenVINO C# API 在不同 .NET 版本中的使用方式：

- 加载 OpenVINO IR 格式模型。
- 使用 OpenCvSharp 进行图片预处理。
- 执行同步推理、异步推理和批量推理。
- 输出检测结果和性能计时。
- 对比 .NET Framework、.NET Core 和现代 .NET 的语言特性差异。

## 框架特性对比

```text
Feature                net4.6   net4.8   netcoreapp3.1   net10.0
Span<T>                no       yes      yes             yes
Memory<T>              no       yes      yes             yes
async/await            yes      yes      yes             yes
IAsyncEnumerable       no       no       yes             yes
Parallel.ForEachAsync  no       no       no              yes
NativeLibrary          no       no       yes             yes
```

## 推荐阅读路径

想了解 3.3 新能力，先读 GenAI 总览，再分别阅读文本生成、Whisper 和 VLM 三篇文章。想了解传统推理和跨框架兼容，按 .NET 10.0、.NET 4.8、.NET Core 3.1、.NET 4.6 的顺序阅读 YOLO 示例。

## 了解更多

- [API 参考文档](../../api/OpenVinoSharp.yml)
- [安装指南](../installation/index.md)
- [问题排查](../troubleshooting/index.md)
- [GitHub 仓库](https://github.com/guojin-yan/OpenVINO-CSharp-API)
