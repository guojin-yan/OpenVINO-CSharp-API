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

        public OpenVINOGenAIFactAttribute()
        {
            if (!_isAvailable)
                Skip = _skipReason;
        }

        /// <summary>
        /// GenAI runtime 是否可用 / Whether GenAI runtime is available.
        /// </summary>
        public static bool IsAvailable => _isAvailable;

        private static void ConfigureLocalRuntimeHint()
        {
            const string localRuntimeRoot = @"E:\OpenVINOSharp\openvino\openvino_genai_windows_2026.2.0.0_x86_64";
            string? current = Environment.GetEnvironmentVariable("OPENVINO_GENAI_RUNTIME_DIR");
            if (string.IsNullOrEmpty(current) && Directory.Exists(localRuntimeRoot))
                Environment.SetEnvironmentVariable("OPENVINO_GENAI_RUNTIME_DIR", localRuntimeRoot);
        }
    }
}
