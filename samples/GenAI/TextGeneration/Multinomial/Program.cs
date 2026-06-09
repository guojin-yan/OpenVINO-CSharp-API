// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using GenAI.Common;
using OpenVinoSharp.GenAI;

return GenAISample.Run(() =>
{
    GenAISample.PrintHeader(
        "Text generation - multinomial sampling / 文本生成 - multinomial 采样",
        "samples/cpp/text_generation/multinomial_causal_lm.cpp");

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
    string prompt = options.Get("prompt", "Write a short creative story about efficient AI inference.")!;
    ulong maxNewTokens = options.GetUInt64("max-new-tokens", 160);
    ulong topK = options.GetUInt64("top-k", 50);
    float topP = options.GetFloat("top-p", 0.95f);
    float temperature = options.GetFloat("temperature", 0.8f);
    ulong seed = options.GetUInt64("seed", 42);

    using GenerationConfig config = GenAISample.CreateTextConfig(maxNewTokens)
        .SetDoSample(true)
        .SetTopK(topK)
        .SetTopP(topP)
        .SetTemperature(temperature)
        .SetRngSeed(seed);

    config.Validate();

    using LLMPipeline pipeline = new(model, device);
    Console.WriteLine(pipeline.GenerateText(prompt, config));
    return 0;
});

static void PrintUsage()
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  dotnet run --project samples/GenAI/TextGeneration/Multinomial/Multinomial.csproj -- --model <MODEL_DIR> [--temperature 0.8] [--top-p 0.95] [--top-k 50]");
}
