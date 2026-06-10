// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using OpenVinoSharp.GenAI;
using System.Text;

namespace GenAI.Common;

/// <summary>
/// Shared helpers for OpenVINO GenAI samples.
/// OpenVINO GenAI 示例共享工具。
/// </summary>
public static class GenAISample
{
    /// <summary>
    /// Prints a simple bilingual sample header.
    /// 打印中英文示例标题。
    /// </summary>
    public static void PrintHeader(string title, string officialSample)
    {
        Console.WriteLine("OpenVINO GenAI C# Sample");
        Console.WriteLine(title);
        Console.WriteLine($"Inspired by official sample: {officialSample}");
        Console.WriteLine(new string('-', 72));
    }

    /// <summary>
    /// Initializes GenAI runtime and prints diagnostic information on failure.
    /// 初始化 GenAI runtime，失败时打印诊断信息。
    /// </summary>
    public static bool EnsureGenAIAvailable()
    {
        if (OpenVinoSharp.GenAI.GenAI.TryInitialize(out string error))
            return true;

        Console.Error.WriteLine("OpenVINO GenAI runtime is not available.");
        Console.Error.WriteLine("OpenVINO GenAI runtime 不可用。");
        Console.Error.WriteLine(error);
        Console.Error.WriteLine();
        Console.Error.WriteLine("Install a JYPPX.OpenVINO.GenAI.runtime.* package or set OPENVINO_GENAI_RUNTIME_DIR.");
        Console.Error.WriteLine("请安装 JYPPX.OpenVINO.GenAI.runtime.* 包，或设置 OPENVINO_GENAI_RUNTIME_DIR。");
        return false;
    }

    /// <summary>
    /// Ensures a model directory exists.
    /// 检查模型目录是否存在。
    /// </summary>
    public static string RequireModelDirectory(string path)
    {
        if (!Directory.Exists(path))
            throw new DirectoryNotFoundException($"Model directory was not found: {path}");

        return Path.GetFullPath(path);
    }

    /// <summary>
    /// Creates a common text generation configuration.
    /// 创建常用文本生成配置。
    /// </summary>
    public static GenerationConfig CreateTextConfig(ulong maxNewTokens, bool echo = false)
    {
        return new GenerationConfig()
            .SetMaxNewTokens(maxNewTokens)
            .SetEcho(echo);
    }

    /// <summary>
    /// Prints OpenVINO GenAI performance metrics.
    /// 打印 OpenVINO GenAI 性能指标。
    /// </summary>
    public static void PrintMetrics(PerformanceMetrics metrics)
    {
        Console.WriteLine();
        Console.WriteLine("Performance metrics / 性能指标");
        Console.WriteLine($"  Load time: {metrics.LoadTime:F2} ms");
        Console.WriteLine($"  Input tokens: {metrics.NumInputTokens}");
        Console.WriteLine($"  Generated tokens: {metrics.NumGenerationTokens}");
        Console.WriteLine($"  Generate duration: {Format(metrics.GenerateDuration)} ms");
        Console.WriteLine($"  Tokenization: {Format(metrics.TokenizationDuration)} ms");
        Console.WriteLine($"  Detokenization: {Format(metrics.DetokenizationDuration)} ms");
        Console.WriteLine($"  TTFT: {Format(metrics.TimeToFirstToken)} ms");
        Console.WriteLine($"  TPOT: {Format(metrics.TimePerOutputToken)} ms/token");
        Console.WriteLine($"  Throughput: {Format(metrics.Throughput)} tokens/s");
    }

    /// <summary>
    /// Runs an action and returns a process exit code.
    /// 执行动作并转换为进程退出码。
    /// </summary>
    public static int Run(Func<int> action)
    {
        try
        {
            ConfigureConsoleUtf8();
            return action();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.GetType().Name + ": " + ex.Message);
            return 1;
        }
    }

    /// <summary>
    /// Uses UTF-8 console input/output so Chinese prompts survive redirected and interactive runs.
    /// 使用 UTF-8 控制台输入输出，保证中文 prompt 在重定向和交互运行中不乱码。
    /// </summary>
    public static void ConfigureConsoleUtf8()
    {
        try
        {
            Console.OutputEncoding = new UTF8Encoding(false);
            if (!Console.IsInputRedirected)
                Console.InputEncoding = new UTF8Encoding(false);
        }
        catch
        {
            // Some hosted consoles do not allow changing encodings. The samples can still run.
        }
    }

    private static string Format(MetricStatistics value)
    {
        return $"{value.Mean:F2} +/- {value.StandardDeviation:F2}";
    }
}
