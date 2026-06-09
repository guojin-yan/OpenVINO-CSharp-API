// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using GenAI.Common;
using OpenVinoSharp.GenAI;

return GenAISample.Run(() =>
{
    GenAISample.PrintHeader(
        "Text generation - beam search / 文本生成 - beam search",
        "samples/cpp/text_generation/beam_search_causal_lm.cpp");

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
    string prompt = options.Get("prompt", "Explain what OpenVINO is in one paragraph.")!;
    ulong maxNewTokens = options.GetUInt64("max-new-tokens", 128);
    ulong beams = options.GetUInt64("beams", 4);
    float lengthPenalty = options.GetFloat("length-penalty", 1.0f);

    using GenerationConfig config = GenAISample.CreateTextConfig(maxNewTokens)
        .SetNumBeams(beams)
        .SetNumReturnSequences(1)
        .SetLengthPenalty(lengthPenalty)
        .SetDoSample(false);

    config.Validate();

    using LLMPipeline pipeline = new(model, device);
    string text = pipeline.GenerateText(prompt, config);
    Console.WriteLine(text);
    return 0;
});

static void PrintUsage()
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  dotnet run --project samples/GenAI/TextGeneration/BeamSearch/BeamSearch.csproj -- --model <MODEL_DIR> [--prompt <TEXT>] [--beams 4]");
}
