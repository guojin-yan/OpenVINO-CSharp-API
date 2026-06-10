// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using GenAI.Common;
using OpenVinoSharp.GenAI;

return GenAISample.Run(() =>
{
    GenAISample.PrintHeader(
        "Whisper speech recognition / Whisper 语音识别",
        "samples/c/whisper_speech_recognition/whisper_speech_recognition.c");

    SampleOptions options = SampleOptions.Parse(args);
    if (options.Has("help"))
    {
        PrintUsage();
        return 0;
    }

    if (!GenAISample.EnsureGenAIAvailable())
        return 2;

    string model = GenAISample.RequireModelDirectory(options.Require("model", "OPENVINO_GENAI_WHISPER_MODEL_DIR"));
    string audio = options.Require("audio", "OPENVINO_GENAI_AUDIO_PATH");
    string device = options.Get("device", "CPU", "OPENVINO_GENAI_DEVICE")!;
    string? language = options.Get("language", env: "OPENVINO_GENAI_WHISPER_LANGUAGE");
    string? task = options.Get("task", "transcribe");
    string? initialPrompt = options.Get("initial-prompt");
    string? hotwords = options.Get("hotwords");
    bool timestamps = options.GetBool("timestamps", false);

    float[] samples = WavFile.ReadMonoFloat(audio, 16000);
    Console.WriteLine($"Loaded audio samples / 已加载音频采样: {samples.Length}");

    using WhisperPipeline pipeline = new(model, device);
    using WhisperGenerationConfig config = pipeline.GetGenerationConfig();

    if (!string.IsNullOrWhiteSpace(task))
        config.SetTask(task);
    if (!string.IsNullOrWhiteSpace(language))
        config.SetLanguage(NormalizeWhisperLanguage(language));
    if (!string.IsNullOrWhiteSpace(initialPrompt))
        config.SetInitialPrompt(initialPrompt);
    if (!string.IsNullOrWhiteSpace(hotwords))
        config.SetHotwords(hotwords);
    config.SetReturnTimestamps(timestamps);

    using WhisperDecodedResults results = pipeline.Generate(samples, config);

    Console.WriteLine();
    Console.WriteLine("Result / 识别结果:");
    Console.WriteLine(results.GetString());

    for (ulong i = 0; i < results.TextCount; i++)
        Console.WriteLine($"Text[{i}] score={results.GetScoreAt(i):F4}: {results.GetTextAt(i)}");

    if (results.HasChunks)
    {
        Console.WriteLine();
        Console.WriteLine("Timestamp chunks / 时间戳分段:");
        for (ulong i = 0; i < results.ChunkCount; i++)
        {
            using WhisperDecodedResultChunk? chunk = results.GetChunkAt(i);
            if (chunk != null)
                Console.WriteLine($"[{chunk.StartTimestamp:F2}, {chunk.EndTimestamp:F2}] {chunk.Text}");
        }
    }

    using PerformanceMetrics metrics = results.GetPerformanceMetrics();
    GenAISample.PrintMetrics(metrics);
    return 0;
});

static void PrintUsage()
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  dotnet run --project samples/GenAI/WhisperSpeechRecognition/WhisperSpeechRecognition.csproj -- --model <MODEL_DIR> --audio <WAV_PATH> [--language en] [--task transcribe] [--timestamps true]");
    Console.WriteLine();
    Console.WriteLine("Environment fallback / 环境变量:");
    Console.WriteLine("  OPENVINO_GENAI_WHISPER_MODEL_DIR, OPENVINO_GENAI_AUDIO_PATH, OPENVINO_GENAI_DEVICE");
}

static string NormalizeWhisperLanguage(string language)
{
    string value = language.Trim();
    if (value.StartsWith("<|", StringComparison.Ordinal) && value.EndsWith("|>", StringComparison.Ordinal))
        return value;

    return "<|" + value.Trim('<', '|', '>') + "|>";
}
