// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using OpenVinoSharp;
using Xunit;

namespace OpenVinoSharp.Tests
{
    /// <summary>
    /// 只在检测到 OpenVINO 运行时时才执行的测试特性
    /// Test attribute that only runs when OpenVINO runtime is detected
    /// </summary>
    public class OpenVINOFactAttribute : FactAttribute
    {
        private static readonly bool _isAvailable;
        private static readonly string _skipReason;

        static OpenVINOFactAttribute()
        {
            try
            {
                // 尝试获取版本信息来检测 OpenVINO 是否可用
                var version = Ov.get_openvino_version();
                _isAvailable = !string.IsNullOrEmpty(version.description);
                _skipReason = _isAvailable 
                    ? null 
                    : "OpenVINO runtime not available. Please install OpenVINO and ensure openvino_c.dll is in PATH.";
            }
            catch (Exception ex)
            {
                _isAvailable = false;
                _skipReason = $"OpenVINO runtime not available: {ex.Message}";
            }
        }

        public OpenVINOFactAttribute()
        {
            if (!_isAvailable)
            {
                Skip = _skipReason;
            }
        }

        /// <summary>
        /// 检查 OpenVINO 是否可用
        /// Check if OpenVINO is available
        /// </summary>
        public static bool IsAvailable => _isAvailable;
    }
}
