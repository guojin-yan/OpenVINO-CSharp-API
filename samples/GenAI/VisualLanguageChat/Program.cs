// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using GenAI.Common;
using OpenVinoSharp;
using OpenVinoSharp.GenAI;
using System.Text;

return GenAISample.Run(() =>
{
    GenAISample.PrintHeader(
        "Visual language chat / 视觉语言聊天",
        "samples/c/visual_language_chat/vlm_pipeline.c");

    SampleOptions options = SampleOptions.Parse(args);
    if (options.Has("help"))
    {
        PrintUsage();
        return 0;
    }

    if (!GenAISample.EnsureGenAIAvailable())
        return 2;

    string model = GenAISample.RequireModelDirectory(options.Require("model", "OPENVINO_GENAI_VLM_MODEL_DIR"));
    string image = options.Require("image", "OPENVINO_GENAI_IMAGE_PATH");
    string device = options.Get("device", "CPU", "OPENVINO_GENAI_DEVICE")!;
    string prompt = options.Get("prompt", "Describe this image in detail.")!;
    ulong maxNewTokens = options.GetUInt64("max-new-tokens", 120);
    bool interactive = options.GetBool("interactive", false);
    bool stream = options.GetBool("stream", true);
    bool allowEmpty = options.GetBool("allow-empty", false);

    using Tensor imageTensor = ImageTensorLoader.LoadRgbTensor(image);
    using GenerationConfig config = GenAISample.CreateTextConfig(maxNewTokens);
    using VLMPipeline pipeline = new(model, device);

    if (interactive)
        return RunInteractive(pipeline, config, imageTensor, stream, allowEmpty);

    Console.WriteLine("Answer / 回答:");
    using ChatHistory history = new ChatHistory().AddUserMessage(prompt);
    using VLMDecodedResults results = Generate(pipeline, history, new[] { imageTensor }, config, stream, out string answer);
    if (!stream)
        Console.WriteLine(answer);
    else
        Console.WriteLine();

    using PerformanceMetrics metrics = results.GetPerformanceMetrics();
    GenAISample.PrintMetrics(metrics);

    if (!allowEmpty && string.IsNullOrWhiteSpace(answer))
    {
        Console.Error.WriteLine("No VLM text was generated. Use a real VLM model or pass --allow-empty true for ABI smoke tests.");
        Console.Error.WriteLine("VLM 未生成文本。请使用真实 VLM 模型，或在 ABI 烟测时传入 --allow-empty true。");
        return 3;
    }

    return 0;
});

static int RunInteractive(VLMPipeline pipeline, GenerationConfig config, Tensor imageTensor, bool stream, bool allowEmpty)
{
    bool firstTurn = true;
    bool hasEmptyAnswer = false;
    using ChatHistory history = new();

    Console.WriteLine("Type questions about the image, empty line, or /exit to quit.");
    Console.WriteLine("输入关于图片的问题，空行或 /exit 退出。");

    while (true)
    {
        Console.WriteLine();
        Console.Write("question> ");
        string? prompt = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(prompt) || prompt.Equals("/exit", StringComparison.OrdinalIgnoreCase))
            break;

        history.AddUserMessage(prompt);
        Tensor[]? turnImages = firstTurn ? new[] { imageTensor } : null;

        Console.Write("answer> ");
        using VLMDecodedResults results = Generate(pipeline, history, turnImages, config, stream, out string answer);
        if (!stream)
            Console.Write(answer);
        Console.WriteLine();

        if (string.IsNullOrWhiteSpace(answer))
            hasEmptyAnswer = true;
        history.AddAssistantMessage(answer);
        firstTurn = false;
    }

    if (!allowEmpty && hasEmptyAnswer)
    {
        Console.Error.WriteLine("At least one VLM turn generated empty text. Use a real VLM model or pass --allow-empty true for ABI smoke tests.");
        Console.Error.WriteLine("至少一轮 VLM 对话未生成文本。请使用真实 VLM 模型，或在 ABI 烟测时传入 --allow-empty true。");
        return 3;
    }

    return 0;
}

static VLMDecodedResults Generate(VLMPipeline pipeline, ChatHistory history, Tensor[]? images, GenerationConfig config, bool stream, out string answer)
{
    if (!stream)
    {
        VLMDecodedResults results = pipeline.GenerateWithHistory(history, images, config);
        answer = results.GetText();
        return results;
    }

    StringBuilder builder = new();
    VLMDecodedResults streamedResults = pipeline.GenerateWithHistory(history, images, config, text =>
    {
        Console.Write(text);
        builder.Append(text);
        return StreamingStatus.Running;
    });

    string streamedText = builder.ToString();
    answer = string.IsNullOrEmpty(streamedText) ? streamedResults.GetText() : streamedText;
    return streamedResults;
}

static void PrintUsage()
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  dotnet run --project samples/GenAI/VisualLanguageChat/VisualLanguageChat.csproj -- --model <MODEL_DIR> --image <BMP_OR_PPM> [--prompt <TEXT>] [--interactive true] [--stream true] [--allow-empty false]");
    Console.WriteLine();
    Console.WriteLine("Environment fallback / 环境变量:");
    Console.WriteLine("  OPENVINO_GENAI_VLM_MODEL_DIR, OPENVINO_GENAI_IMAGE_PATH, OPENVINO_GENAI_DEVICE");
}
