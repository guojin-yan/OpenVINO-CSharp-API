// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using GenAI.Common;
using OpenVinoSharp.GenAI;
using System.Text;

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
    string instruction = options.Get(
        "system-prompt",
        string.Empty)!;

    using GenerationConfig config = GenAISample.CreateTextConfig(maxNewTokens)
        .SetStopStrings("\n问题：", "\r\n问题：", "\n用户：", "\r\n用户：", "\nUser:", "\r\nUser:")
        .SetIncludeStopStringInOutput(false);
    using LLMPipeline pipeline = new(model, device);
    List<(string Role, string Content)> messages = new();
    IReadOnlyList<string> scriptedTurns = options.GetAll("turn");

    if (scriptedTurns.Count > 0)
    {
        foreach (string turn in scriptedTurns)
        {
            if (string.IsNullOrWhiteSpace(turn) || turn.Equals("/exit", StringComparison.OrdinalIgnoreCase))
                break;
            RunTurn(pipeline, config, instruction, messages, turn);
        }
        return 0;
    }

    Console.WriteLine("Type a question, empty line, or /exit to quit.");
    Console.WriteLine("输入问题，空行或 /exit 退出。");

    while (true)
    {
        Console.WriteLine();
        Console.Write("question> ");
        string? prompt = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(prompt) || prompt.Equals("/exit", StringComparison.OrdinalIgnoreCase))
            break;

        RunTurn(pipeline, config, instruction, messages, prompt);
    }

    return 0;
});

static void RunTurn(
    LLMPipeline pipeline,
    GenerationConfig config,
    string instruction,
    List<(string Role, string Content)> messages,
    string prompt)
{
    messages.Add(("User", prompt));
    string chatPrompt = BuildChatPrompt(instruction, messages, prompt);

    Console.WriteLine();
    Console.WriteLine("question> " + prompt);
    Console.Write("answer> ");

    using DecodedResults results = pipeline.Generate(chatPrompt, config);
    string answer = CleanAnswer(results.GetText(), prompt);
    Console.Write(answer);
    messages.Add(("Assistant", answer));
    Console.WriteLine();
}

static string BuildChatPrompt(string instruction, IReadOnlyList<(string Role, string Content)> messages, string latestPrompt)
{
    bool chinese = ContainsCjk(latestPrompt);
    if (chinese)
    {
        string prefix = string.IsNullOrWhiteSpace(instruction) ? string.Empty : instruction.Trim() + "\n";
        return prefix + "请用中文回答：" + latestPrompt.Trim();
    }

    return string.IsNullOrWhiteSpace(instruction)
        ? latestPrompt.Trim()
        : instruction.Trim() + "\n" + latestPrompt.Trim();
}

static string CleanAnswer(string answer, string prompt)
{
    string cleaned = answer.Trim();
    string[] markers =
    {
        "中文回答：",
        "中文回答:",
        "回答：",
        "回答:",
        "Answer:",
        "answer:"
    };

    foreach (string marker in markers)
    {
        int index = cleaned.LastIndexOf(marker, StringComparison.Ordinal);
        if (index >= 0)
        {
            cleaned = cleaned.Substring(index + marker.Length).Trim();
            break;
        }
    }

    string questionPrefix = "问题：" + prompt.Trim();
    if (cleaned.StartsWith(questionPrefix, StringComparison.Ordinal))
        cleaned = cleaned.Substring(questionPrefix.Length).Trim();

    return cleaned.Trim().TrimEnd('\uFFFD').Trim();
}

static bool ContainsCjk(string value)
{
    foreach (char ch in value)
    {
        if (ch >= '\u4e00' && ch <= '\u9fff')
            return true;
    }

    return false;
}

static void PrintUsage()
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  dotnet run --project samples/GenAI/TextGeneration/Chat/Chat.csproj -- --model <MODEL_DIR> [--device CPU] [--max-new-tokens 120] [--system-prompt <TEXT>] [--turn <TEXT> ...]");
    Console.WriteLine();
    Console.WriteLine("Environment fallback / 环境变量:");
    Console.WriteLine("  OPENVINO_GENAI_LLM_MODEL_DIR, OPENVINO_GENAI_DEVICE");
}
