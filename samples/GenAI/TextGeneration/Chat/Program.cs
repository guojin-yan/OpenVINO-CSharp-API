// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using GenAI.Common;
using OpenVinoSharp.GenAI;

return GenAISample.Run(() =>
{
    GenAISample.PrintHeader(
        "Text generation - chat / 文本生成 - 聊天",
        "samples/c/text_generation/chat_sample_c.c");

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
    ulong maxNewTokens = options.GetUInt64("max-new-tokens", 120);

    using GenerationConfig config = GenAISample.CreateTextConfig(maxNewTokens);
    using LLMPipeline pipeline = new(model, device);
    using ChatHistory history = new();

    pipeline.StartChat();
    try
    {
        Console.WriteLine("Type a question, empty line, or /exit to quit.");
        Console.WriteLine("输入问题，空行或 /exit 退出。");

        while (true)
        {
            Console.WriteLine();
            Console.Write("question> ");
            string? prompt = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(prompt) || prompt.Equals("/exit", StringComparison.OrdinalIgnoreCase))
                break;

            history.AddUserMessage(prompt);
            Console.Write("answer> ");

            using DecodedResults results = pipeline.GenerateWithHistory(history, config, text =>
            {
                Console.Write(text);
                return StreamingStatus.Running;
            });

            string answer = results.GetText();
            history.AddAssistantMessage(answer);
            Console.WriteLine();
        }
    }
    finally
    {
        pipeline.FinishChat();
    }

    return 0;
});

static void PrintUsage()
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  dotnet run --project samples/GenAI/TextGeneration/Chat/Chat.csproj -- --model <MODEL_DIR> [--device CPU] [--max-new-tokens 120]");
    Console.WriteLine();
    Console.WriteLine("Environment fallback / 环境变量:");
    Console.WriteLine("  OPENVINO_GENAI_LLM_MODEL_DIR, OPENVINO_GENAI_DEVICE");
}
