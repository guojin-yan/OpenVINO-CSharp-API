// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using GenAI.Common;
using OpenVinoSharp.GenAI;

return GenAISample.Run(() =>
{
    GenAISample.PrintHeader(
        "Text generation - streaming callback / 文本生成 - 流式回调",
        "samples/c/text_generation/chat_sample_c.c streamer_callback");

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
    string prompt = options.Get("prompt", "List three reasons developers use OpenVINO GenAI.")!;
    ulong maxNewTokens = options.GetUInt64("max-new-tokens", 120);

    using GenerationConfig config = GenAISample.CreateTextConfig(maxNewTokens);
    using LLMPipeline pipeline = new(model, device);

    Console.WriteLine("Streaming output / 流式输出:");
    using DecodedResults results = pipeline.Generate(prompt, config, text =>
    {
        Console.Write(text);
        return StreamingStatus.Running;
    });

    Console.WriteLine();
    using PerformanceMetrics metrics = results.GetPerformanceMetrics();
    GenAISample.PrintMetrics(metrics);
    return 0;
});

static void PrintUsage()
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  dotnet run --project samples/GenAI/TextGeneration/Streaming/Streaming.csproj -- --model <MODEL_DIR> [--prompt <TEXT>]");
}
