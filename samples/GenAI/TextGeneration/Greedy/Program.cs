// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using GenAI.Common;
using OpenVinoSharp.GenAI;

return GenAISample.Run(() =>
{
    GenAISample.PrintHeader(
        "Text generation - greedy decoding / 文本生成 - greedy 解码",
        "samples/c/text_generation/greedy_causal_lm_c.c");

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
    string prompt = options.Get("prompt", "The sky is blue because")!;
    ulong maxNewTokens = options.GetUInt64("max-new-tokens", 100);

    using GenerationConfig config = GenAISample.CreateTextConfig(maxNewTokens);
    using LLMPipeline pipeline = new(model, device);
    using DecodedResults results = pipeline.Generate(prompt, config);

    Console.WriteLine("Prompt / 提示词:");
    Console.WriteLine(prompt);
    Console.WriteLine();
    Console.WriteLine("Output / 输出:");
    Console.WriteLine(results.GetText());

    using PerformanceMetrics metrics = results.GetPerformanceMetrics();
    GenAISample.PrintMetrics(metrics);
    return 0;
});

static void PrintUsage()
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  dotnet run --project samples/GenAI/TextGeneration/Greedy/Greedy.csproj -- --model <MODEL_DIR> [--prompt <TEXT>] [--device CPU] [--max-new-tokens 100]");
    Console.WriteLine();
    Console.WriteLine("Environment fallback / 环境变量:");
    Console.WriteLine("  OPENVINO_GENAI_LLM_MODEL_DIR, OPENVINO_GENAI_DEVICE");
}
