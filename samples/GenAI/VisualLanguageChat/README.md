# Visual Language Chat / 视觉语言聊天

This project mirrors the official VLM chat sample and demonstrates image input, chat history, streaming output, and metrics.

本项目复刻官方 VLM 聊天示例，演示图片输入、聊天历史、流式输出和性能指标。

```powershell
conda activate ov-genai-samples
optimum-cli export openvino --model Qwen/Qwen3-VL-2B-Instruct --trust-remote-code models/Qwen3-VL-2B-Instruct
python -c "from PIL import Image; Image.open(r'input.jpg').convert('RGB').save(r'input.bmp')"

dotnet run --project samples/GenAI/VisualLanguageChat/VisualLanguageChat.csproj -- --model models/Qwen3-VL-2B-Instruct --image input.bmp --prompt "What is on the image?"
dotnet run --project samples/GenAI/VisualLanguageChat/VisualLanguageChat.csproj -- --model models/Qwen3-VL-2B-Instruct --image input.bmp --interactive true
```

The built-in loader supports RGB BMP and binary PPM/PNM to keep the sample free from image-processing NuGet packages.

内置图片读取器支持 RGB BMP 和二进制 PPM/PNM，避免示例额外依赖图片处理 NuGet 包。
