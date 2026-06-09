// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using OpenVinoSharp;
using OpenVinoSharp.GenAI;

namespace GenAIQuickStart;

internal static class Program
{
    private const string DeviceEnvironment = "OPENVINO_GENAI_DEVICE";
    private const string LlmModelEnvironment = "OPENVINO_GENAI_LLM_MODEL_DIR";
    private const string WhisperModelEnvironment = "OPENVINO_GENAI_WHISPER_MODEL_DIR";
    private const string VlmModelEnvironment = "OPENVINO_GENAI_VLM_MODEL_DIR";

    private static int Main()
    {
        var version = Ov.get_openvino_version();
        Console.WriteLine($"OpenVINO Runtime: {version.description} {version.buildNumber}");

        if (!GenAI.TryInitialize(out string error))
        {
            Console.WriteLine("OpenVINO GenAI runtime is not available.");
            Console.WriteLine(error);
            Console.WriteLine("Install a JYPPX.OpenVINO.GenAI.runtime.* package or set OPENVINO_GENAI_RUNTIME_DIR.");
            return 0;
        }

        string device = Environment.GetEnvironmentVariable(DeviceEnvironment) ?? "CPU";
        Console.WriteLine($"OpenVINO GenAI runtime is available. Device: {device}");

        RunLlmIfConfigured(device);
        RunWhisperIfConfigured(device);
        RunVlmIfConfigured(device);

        Console.WriteLine("Done.");
        return 0;
    }

    private static void RunLlmIfConfigured(string device)
    {
        string? modelPath = Environment.GetEnvironmentVariable(LlmModelEnvironment);
        if (string.IsNullOrWhiteSpace(modelPath))
        {
            Console.WriteLine($"Skip LLM: set {LlmModelEnvironment} to run text generation.");
            return;
        }

        try
        {
            using var config = new GenerationConfig().SetMaxNewTokens(64);
            using var pipeline = new LLMPipeline(modelPath, device);

            string prompt = Environment.GetEnvironmentVariable("OPENVINO_GENAI_PROMPT")
                ?? "Explain OpenVINO GenAI in one short paragraph.";

            Console.WriteLine("LLM result:");
            Console.WriteLine(pipeline.GenerateText(prompt, config));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"LLM sample failed: {ex.Message}");
        }
    }

    private static void RunWhisperIfConfigured(string device)
    {
        string? modelPath = Environment.GetEnvironmentVariable(WhisperModelEnvironment);
        if (string.IsNullOrWhiteSpace(modelPath))
        {
            Console.WriteLine($"Skip Whisper: set {WhisperModelEnvironment} to run speech recognition.");
            return;
        }

        try
        {
            using var config = new WhisperGenerationConfig()
                .SetLanguage("en")
                .SetTask("transcribe")
                .SetReturnTimestamps(false);

            using var pipeline = new WhisperPipeline(modelPath, device);
            float[] oneSecondSilence = new float[16000];

            using WhisperDecodedResults results = pipeline.Generate(oneSecondSilence, config);
            Console.WriteLine("Whisper result:");
            Console.WriteLine(results.GetString());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Whisper sample failed: {ex.Message}");
        }
    }

    private static void RunVlmIfConfigured(string device)
    {
        string? modelPath = Environment.GetEnvironmentVariable(VlmModelEnvironment);
        if (string.IsNullOrWhiteSpace(modelPath))
        {
            Console.WriteLine($"Skip VLM: set {VlmModelEnvironment} to run vision-language generation.");
            return;
        }

        try
        {
            using var config = new GenerationConfig().SetMaxNewTokens(64);
            using var pipeline = new VLMPipeline(modelPath, device);

            Console.WriteLine("VLM text-only result:");
            Console.WriteLine(pipeline.GenerateText("Describe what you can do.", images: null, config));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"VLM sample failed: {ex.Message}");
        }
    }
}
