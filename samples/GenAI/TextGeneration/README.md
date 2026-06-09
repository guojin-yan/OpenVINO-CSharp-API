# Text Generation Samples / 文本生成示例

These projects cover the wrapped LLM pipeline scenarios: greedy decoding, beam search, multinomial sampling, streaming, chat, and benchmarking.

这些项目覆盖已封装的 LLM pipeline 场景：greedy 解码、beam search、multinomial 采样、流式输出、聊天和性能基准。

Prepare a model first:

先准备文本生成模型：

```powershell
conda activate ov-genai-samples
optimum-cli export openvino --model TinyLlama/TinyLlama-1.1B-Chat-v1.0 --weight-format int4 --trust-remote-code models/TinyLlama-1.1B-Chat-v1.0-ov
$env:OPENVINO_GENAI_LLM_MODEL_DIR = "$PWD\models\TinyLlama-1.1B-Chat-v1.0-ov"
```

Run from the repository root:

在仓库根目录运行：

```powershell
dotnet run --project samples/GenAI/TextGeneration/Greedy/Greedy.csproj -- --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR"
dotnet run --project samples/GenAI/TextGeneration/BeamSearch/BeamSearch.csproj -- --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR" --beams 4
dotnet run --project samples/GenAI/TextGeneration/Multinomial/Multinomial.csproj -- --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR" --temperature 0.8 --top-p 0.95 --top-k 50
dotnet run --project samples/GenAI/TextGeneration/Streaming/Streaming.csproj -- --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR"
dotnet run --project samples/GenAI/TextGeneration/Chat/Chat.csproj -- --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR"
dotnet run --project samples/GenAI/TextGeneration/Benchmark/Benchmark.csproj -- --model "$env:OPENVINO_GENAI_LLM_MODEL_DIR" --iterations 3 --warmup 1
```
