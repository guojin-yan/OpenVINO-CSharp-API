// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using GenAI.Common;
using OpenVinoSharp.GenAI;

return GenAISample.Run(() =>
{
    GenAISample.PrintHeader(
        "Text generation - benchmark / 文本生成 - 性能基准",
        "samples/c/text_generation/benchmark_genai_c.c");

    SampleOptions options = SampleOptions.Parse(args);
    if (options.Has("help"))
    {
        PrintUsage();
        return 0;
    }

    if (!GenAISample.EnsureGenAIAvailable())
        return 2;

    string model = GenAISample.RequireModelDirectory(options.Require("model", "OPENVINO_GENAI_LLM_MODEL_DIR"));
    string device = options.Get("device", "CPU", "OPENVINO_GENAI_DEVICE")!;
    string prompt = options.Get("prompt", "What is OpenVINO GenAI?")!;
    ulong maxNewTokens = options.GetUInt64("max-new-tokens", 64);
    int warmup = options.GetInt("warmup", 1);
    int iterations = Math.Max(1, options.GetInt("iterations", 3));

    using GenerationConfig config = GenAISample.CreateTextConfig(maxNewTokens);
    using LLMPipeline pipeline = new(model, device);

    for (int i = 0; i < warmup; i++)
    {
        using DecodedResults warmupResults = pipeline.Generate(prompt, config);
        Console.WriteLine($"Warmup {i + 1}/{warmup}: {warmupResults.GetText().Length} chars");
    }

    using DecodedResults firstResults = pipeline.Generate(prompt, config);
    using PerformanceMetrics totalMetrics = firstResults.GetPerformanceMetrics();
    Console.WriteLine($"Iteration 1/{iterations}: {firstResults.GetText().Length} chars");

    for (int i = 1; i < iterations; i++)
    {
        using DecodedResults results = pipeline.Generate(prompt, config);
        using PerformanceMetrics metrics = results.GetPerformanceMetrics();
        totalMetrics.AddInPlace(metrics);
        Console.WriteLine($"Iteration {i + 1}/{iterations}: {results.GetText().Length} chars");
    }

    Console.WriteLine();
    Console.WriteLine($"Aggregated metrics for {iterations} iteration(s) / {iterations} 次迭代聚合指标");
    GenAISample.PrintMetrics(totalMetrics);
    return 0;
});

static void PrintUsage()
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  dotnet run --project samples/GenAI/TextGeneration/Benchmark/Benchmark.csproj -- --model <MODEL_DIR> [--iterations 3] [--warmup 1]");
}
