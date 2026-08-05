// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using System.IO;
using Xunit;

namespace OpenVinoSharp.Tests
{
    /// <summary>
    /// 仅在检测到 OpenVINO GenAI runtime 时执行的测试特性。
    /// Test attribute that only runs when OpenVINO GenAI runtime is detected.
    /// </summary>
    public class OpenVINOGenAIFactAttribute : FactAttribute
    {
        private static readonly bool _isAvailable;
        private static readonly string? _skipReason;

        static OpenVINOGenAIFactAttribute()
        {
            try
            {
                ConfigureLocalRuntimeHint();
                OpenVinoSharp.GenAI.GenAI.Initialize();
                _isAvailable = true;
                _skipReason = null;
            }
            catch (Exception ex)
            {
                _isAvailable = false;
                _skipReason = $"OpenVINO GenAI runtime not available: {ex.Message}";
            }
        }

        /// <summary>
        /// 创建 GenAI runtime 条件测试特性 / Creates a GenAI runtime-gated fact attribute.
        /// </summary>
        public OpenVINOGenAIFactAttribute()
        {
            if (!_isAvailable)
                Skip = _skipReason;
        }

        /// <summary>
        /// GenAI runtime 是否可用 / Whether GenAI runtime is available.
        /// </summary>
        public static bool IsAvailable => _isAvailable;

        /// <summary>
        /// GenAI runtime 不可用时的诊断信息 / Diagnostic message when GenAI runtime is unavailable.
        /// </summary>
        public static string? AvailabilityError => _skipReason;

        private static void ConfigureLocalRuntimeHint()
        {
            const string localRuntimeRoot = @"E:\OpenVINOSharp\openvino\openvino_genai_windows_2026.3.0.0_x86_64";
            string? current = Environment.GetEnvironmentVariable("OPENVINO_GENAI_RUNTIME_DIR");
            if (string.IsNullOrEmpty(current) && Directory.Exists(localRuntimeRoot))
                Environment.SetEnvironmentVariable("OPENVINO_GENAI_RUNTIME_DIR", localRuntimeRoot);
        }
    }

    /// <summary>
    /// 仅在 GenAI runtime 和指定模型目录都可用时执行的测试特性。
    /// Test attribute that only runs when GenAI runtime and the requested model directory are available.
    /// </summary>
    public sealed class OpenVINOGenAIModelFactAttribute : FactAttribute
    {
        /// <summary>
        /// 创建模型条件测试特性 / Creates a model-gated GenAI fact attribute.
        /// </summary>
        /// <param name="environmentVariable">保存模型目录的环境变量 / Environment variable that stores the model directory.</param>
        /// <param name="modelName">用于 skip 消息的模型名称 / Model name used in skip messages.</param>
        public OpenVINOGenAIModelFactAttribute(string environmentVariable, string modelName)
        {
            if (!OpenVINOGenAIFactAttribute.IsAvailable)
            {
                Skip = OpenVINOGenAIFactAttribute.AvailabilityError;
                return;
            }

            string? modelPath = Environment.GetEnvironmentVariable(environmentVariable);
            if (string.IsNullOrWhiteSpace(modelPath) || !Directory.Exists(modelPath))
                Skip = $"{modelName} model directory is not configured. Set {environmentVariable} to run this test.";
        }

        /// <summary>
        /// 获取必需模型目录 / Gets a required model directory.
        /// </summary>
        public static string GetModelPath(string environmentVariable)
        {
            string? modelPath = Environment.GetEnvironmentVariable(environmentVariable);
            if (string.IsNullOrWhiteSpace(modelPath))
                throw new InvalidOperationException($"Environment variable {environmentVariable} is not set.");
            return modelPath;
        }
    }
}
