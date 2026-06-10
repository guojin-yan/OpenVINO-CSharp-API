// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using GenAI.Common;
using OpenVinoSharp;
using OpenVinoSharp.GenAI;

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

    using Tensor imageTensor = ImageTensorLoader.LoadRgbTensor(image);
    using GenerationConfig config = GenAISample.CreateTextConfig(maxNewTokens);
    using VLMPipeline pipeline = new(model, device);

    if (interactive)
        return RunInteractive(pipeline, config, imageTensor);

    using VLMDecodedResults results = pipeline.Generate(prompt, new[] { imageTensor }, config);
    Console.WriteLine("Answer / 回答:");
    Console.WriteLine(results.GetText());

    using PerformanceMetrics metrics = results.GetPerformanceMetrics();
    GenAISample.PrintMetrics(metrics);
    return 0;
});

static int RunInteractive(VLMPipeline pipeline, GenerationConfig config, Tensor imageTensor)
{
    bool firstTurn = true;

    pipeline.StartChat();
    try
    {
        Console.WriteLine("Type questions about the image, empty line, or /exit to quit.");
        Console.WriteLine("输入关于图片的问题，空行或 /exit 退出。");

        while (true)
        {
            Console.WriteLine();
            Console.Write("question> ");
            string? prompt = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(prompt) || prompt.Equals("/exit", StringComparison.OrdinalIgnoreCase))
                break;

            Tensor[]? turnImages = firstTurn ? new[] { imageTensor } : null;

            // OpenVINO GenAI 2026.2 Windows runtime does not export the C
            // generate_with_history entry point yet. Use chat mode plus
            // Generate so the sample stays runnable with the released runtime.
            // OpenVINO GenAI 2026.2 Windows runtime 尚未导出 C 版
            // generate_with_history，因此这里使用 chat mode + Generate。
            using VLMDecodedResults results = pipeline.Generate(prompt, turnImages, config);
            string answer = results.GetText();
            Console.WriteLine("answer> " + answer);
            firstTurn = false;
        }
    }
    finally
    {
        pipeline.FinishChat();
    }

    return 0;
}

static void PrintUsage()
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  dotnet run --project samples/GenAI/VisualLanguageChat/VisualLanguageChat.csproj -- --model <MODEL_DIR> --image <BMP_OR_PPM> [--prompt <TEXT>] [--interactive true]");
    Console.WriteLine();
    Console.WriteLine("Environment fallback / 环境变量:");
    Console.WriteLine("  OPENVINO_GENAI_VLM_MODEL_DIR, OPENVINO_GENAI_IMAGE_PATH, OPENVINO_GENAI_DEVICE");
}
