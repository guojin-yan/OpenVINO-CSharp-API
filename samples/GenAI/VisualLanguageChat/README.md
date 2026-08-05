# Visual Language Chat / 视觉语言聊天

This project mirrors the official VLM chat sample and demonstrates image tensor
loading, single-turn VLM generation, interactive chat mode, and metrics.

本项目复刻官方 VLM 聊天示例，演示图片 Tensor 加载、单轮 VLM 生成、交互式聊天模式和
性能指标。

## Prepare Model / 准备模型

For full local validation, use the real `OpenVINO/InternVL2-1B-int4-ov` model:

完整本地验证使用真实的 `OpenVINO/InternVL2-1B-int4-ov` 模型：

```powershell
$modelRoot = "E:\LlmModel"
$vlm = Join-Path $modelRoot "InternVL2-1B-int4-ov"

conda run -n PaddleOCR hf download OpenVINO/InternVL2-1B-int4-ov `
  --local-dir $vlm
```

`PaddleOCR` is the conda environment that contains `hf` on this machine.
Replace it with your own Hugging Face CLI environment name if needed.

If direct Hugging Face access is slow, use the same model from
`https://hf-mirror.com/OpenVINO/InternVL2-1B-int4-ov`.

如果 Hugging Face 直连较慢，可以从
`https://hf-mirror.com/OpenVINO/InternVL2-1B-int4-ov` 下载同一模型。

For larger article demos, you can also export Qwen VL:

如需更大的文章演示模型，也可以导出 Qwen VL：

```powershell
conda activate ov-genai-samples

optimum-cli export openvino `
  --model Qwen/Qwen3-VL-2B-Instruct `
  --trust-remote-code `
  (Join-Path $modelRoot "Qwen3-VL-2B-Instruct-ov")
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
$image = Join-Path "E:\LlmModel" "assets\color_blocks_30.ppm"

dotnet run --project samples/GenAI/VisualLanguageChat/VisualLanguageChat.csproj --framework net8.0 -- `
  --model $vlm `
  --image $image `
  --device CPU `
  --prompt "What colors are visible in this image? Answer with color names only." `
  --max-new-tokens 48
```

## Run Interactive Chat / 运行交互式聊天

```powershell
dotnet run --project samples/GenAI/VisualLanguageChat/VisualLanguageChat.csproj --framework net8.0 -- `
  --model $vlm `
  --image $image `
  --device CPU `
  --interactive true `
  --max-new-tokens 48
```

Interactive commands:

交互命令：

- Type a question about the image.
- Type `/exit` or an empty line to quit.
- 输入关于图片的问题。
- 输入 `/exit` 或空行退出。

## Runtime Notes / 运行时说明

- OpenVINO GenAI 2026.3 Windows C runtime exports
  `ov_genai_vlm_pipeline_generate_with_history`.
- The sample therefore uses `ChatHistory` plus streamed `GenerateWithHistory()`;
  the deprecated stateful chat entry points are not required.
- Empty output is treated as a failure by default. Pass `--allow-empty true`
  only when intentionally running tiny random ABI smoke models.
- Full local validation uses `OpenVINO/InternVL2-1B-int4-ov` and produces
  non-empty text.

- OpenVINO GenAI 2026.3 Windows C runtime 已导出
  `ov_genai_vlm_pipeline_generate_with_history`。
- 因此示例使用 `ChatHistory` 加流式 `GenerateWithHistory()`，不依赖已弃用的状态聊天入口点。
- 默认会把空输出视为失败。只有明确运行 tiny random ABI 烟测模型时才传入
  `--allow-empty true`。
- 完整本地验证使用 `OpenVINO/InternVL2-1B-int4-ov`，并能生成非空文本。

Validated output / 已验证输出：

```text
This image is in the color spectrum of the visible colors.
```
