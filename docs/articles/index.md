# 文章

这里汇总 OpenVINO C# API 的安装、升级、案例和排错内容。3.3 版本的重点是两条线并行推进：继续保持传统 OpenVINO C API 的 C# 封装兼容，同时加入可选的 OpenVINO GenAI 托管封装，让 .NET 应用可以运行本地 LLM、Whisper 和视觉语言模型。

## 内容导航

| 分类 | 说明 |
|---|---|
| [安装方式](installation/index.md) | Windows、Linux、macOS 的 NuGet runtime 安装和 native runtime 说明 |
| [案例应用](samples/index.md) | YOLO 传统推理示例，以及 GenAI 文本、语音、视觉语言示例 |
| [升级指南](upgrade/openvino-csharp-3.3.md) | 从 3.2 升级到 3.3 的 API、runtime 和示例变化 |
| [问题排查](troubleshooting/index.md) | native library 加载、模型路径、平台差异等常见问题 |

## 技术文章入口

如果你关注 OpenVINO GenAI 在 C# 中的落地，可以从下面三篇开始：

- [用 C# 跑通 OpenVINO GenAI 文本生成](samples/genai-text-generation-tutorial.md)
- [用 C# 调用 OpenVINO GenAI Whisper](samples/genai-whisper-tutorial.md)
- [用 C# 构建 OpenVINO GenAI 视觉语言问答](samples/genai-vlm-tutorial.md)

如果你关注传统模型推理和跨框架兼容，可以阅读 YOLO 系列示例。它们覆盖 .NET Framework 4.6/4.8、.NET Core 3.1 和 .NET 10.0，展示同一推理任务在不同运行时上的工程写法。
