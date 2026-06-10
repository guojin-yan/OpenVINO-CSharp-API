# Text Generation Samples / 文本生成示例

These projects cover the wrapped LLM pipeline scenarios: greedy decoding, beam search, multinomial sampling, streaming, chat, and benchmarking.

这些项目覆盖已封装的 LLM pipeline 场景：greedy 解码、beam search、multinomial 采样、流式输出、聊天和性能基准。

Prepare a model first:

先准备文本生成模型：

```powershell
$modelRoot = "E:\LlmModel"
$llm = Join-Path $modelRoot "TinyLlama-1.1B-Chat-v1.0-int4-ov"

conda run -n PaddleOCR hf download OpenVINO/TinyLlama-1.1B-Chat-v1.0-int4-ov --local-dir $llm
```

`PaddleOCR` is the conda environment that contains `hf` on this machine.
Replace it with your own Hugging Face CLI environment name if needed.

Run from the repository root:

在仓库根目录运行：

```powershell
dotnet run --project samples/GenAI/TextGeneration/Greedy/Greedy.csproj -- --model $llm --device CPU
dotnet run --project samples/GenAI/TextGeneration/BeamSearch/BeamSearch.csproj -- --model $llm --device CPU --beams 4
dotnet run --project samples/GenAI/TextGeneration/Multinomial/Multinomial.csproj -- --model $llm --device CPU --temperature 0.8 --top-p 0.95 --top-k 50
dotnet run --project samples/GenAI/TextGeneration/Streaming/Streaming.csproj -- --model $llm --device CPU
dotnet run --project samples/GenAI/TextGeneration/Chat/Chat.csproj -- --model $llm --device CPU --turn "请用中文列出三个 OpenVINO 关键词。"
dotnet run --project samples/GenAI/TextGeneration/Benchmark/Benchmark.csproj -- --model $llm --device CPU --iterations 3 --warmup 1
```

`Chat` also supports interactive input. For automated Chinese validation,
prefer repeated `--turn` arguments because command-line arguments preserve
Unicode text more reliably than redirected stdin on some Windows consoles.

`Chat` 也支持交互式输入。自动化中文验证建议使用可重复的 `--turn` 参数；在部分
Windows 控制台中，重定向 stdin 可能会影响中文编码，而命令行参数更稳定。
