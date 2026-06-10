# Visual Language Chat / 视觉语言聊天

This project mirrors the official VLM chat sample and demonstrates image tensor
loading, single-turn VLM generation, interactive chat mode, and metrics.

本项目复刻官方 VLM 聊天示例，演示图片 Tensor 加载、单轮 VLM 生成、交互式聊天模式和
性能指标。

## Prepare Model / 准备模型

For smoke tests, use a tiny random OpenVINO LLaVA model:

烟测可以使用 tiny random OpenVINO LLaVA 模型：

```powershell
hf download katuni4ka/tiny-random-llava-ov `
  --local-dir E:\OpenVINOSharp\models\genai-smoke\tiny-random-llava-ov
```

This tiny model validates ABI and sample flow, but it may return empty or
meaningless text. For a technical article, use a real VLM model:

该 tiny 模型用于验证 ABI 和示例流程，但可能返回空文本或无意义文本。写技术文章时建议
使用真实 VLM 模型：

```powershell
conda activate ov-genai-samples

optimum-cli export openvino `
  --model Qwen/Qwen3-VL-2B-Instruct `
  --trust-remote-code `
  E:\OpenVINOSharp\models\genai-samples\Qwen3-VL-2B-Instruct-ov
```

## Prepare Image / 准备图片

The built-in image loader supports RGB BMP and binary PPM/PNM to keep the sample
free from image-processing NuGet packages.

内置图片读取器支持 RGB BMP 和二进制 PPM/PNM，这样示例不需要额外图片处理 NuGet 包。

Convert JPG/PNG with Pillow:

使用 Pillow 转换 JPG/PNG：

```powershell
python -c "from PIL import Image; Image.open(r'input.jpg').convert('RGB').save(r'input.bmp')"
```

## Run Single Turn / 运行单轮问答

```powershell
$env:OPENVINO_GENAI_RUNTIME_DIR = "E:\OpenVINOSharp\openvino\openvino_genai_windows_2026.2.0.0_x86_64\runtime\bin\intel64\Release"

dotnet run --project samples/GenAI/VisualLanguageChat/VisualLanguageChat.csproj --framework net8.0 -- `
  --model E:\OpenVINOSharp\models\genai-smoke\tiny-random-llava-ov `
  --image E:\OpenVINOSharp\models\genai-samples\assets\color_blocks_30.ppm `
  --device CPU `
  --prompt "What colors are visible?" `
  --max-new-tokens 8
```

## Run Interactive Chat / 运行交互式聊天

```powershell
dotnet run --project samples/GenAI/VisualLanguageChat/VisualLanguageChat.csproj --framework net8.0 -- `
  --model E:\OpenVINOSharp\models\genai-smoke\tiny-random-llava-ov `
  --image E:\OpenVINOSharp\models\genai-samples\assets\color_blocks_30.ppm `
  --device CPU `
  --interactive true `
  --max-new-tokens 8
```

Interactive commands:

交互命令：

- Type a question about the image.
- Type `/exit` or an empty line to quit.
- 输入关于图片的问题。
- 输入 `/exit` 或空行退出。

## Runtime Notes / 运行时说明

- The validated OpenVINO GenAI 2026.2 Windows C runtime does not export
  `ov_genai_vlm_pipeline_generate_with_history`.
- The sample therefore uses `StartChat()` plus `Generate()` instead of the C
  history entry point.
- The VLM streamer path returned native error `-17` with the tiny random smoke
  model during validation, so this sample uses non-streaming generation.
- Empty text from `tiny-random-llava-ov` is acceptable for smoke validation.
  Use a real model for screenshots and article-quality answers.

- 已验证的 OpenVINO GenAI 2026.2 Windows C runtime 未导出
  `ov_genai_vlm_pipeline_generate_with_history`。
- 因此示例使用 `StartChat()` 加 `Generate()`，不依赖 C history 入口点。
- tiny random 烟测模型验证时，VLM streamer 路径返回 native `-17`，因此该示例使用
  非流式生成。
- `tiny-random-llava-ov` 返回空文本是可接受的烟测结果。文章截图和语义效果展示请使用
  真实模型。
