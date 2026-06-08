// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using System.IO;
using OpenVinoSharp;
using OpenVinoSharp.GenAI;
using Xunit;

namespace OpenVinoSharp.Tests.UnitTests
{
    /// <summary>
    /// OpenVINO GenAI 托管封装测试 / Tests for managed OpenVINO GenAI wrappers.
    /// </summary>
    public class GenAIUnitTests
    {
        /// <summary>
        /// GenAI 枚举值必须与 C API 定义保持一致 / GenAI enum values must match C API definitions.
        /// </summary>
        [Fact]
        public void GenAIEnums_MatchNativeValues()
        {
            Assert.Equal(0, (int)StreamingStatus.Running);
            Assert.Equal(1, (int)StreamingStatus.Stop);
            Assert.Equal(2, (int)StreamingStatus.Cancel);

            Assert.Equal(0, (int)StopCriteria.Early);
            Assert.Equal(1, (int)StopCriteria.Heuristic);
            Assert.Equal(2, (int)StopCriteria.Never);
        }

        /// <summary>
        /// TryInitialize 在缺少 runtime 时应返回诊断信息 / TryInitialize should report diagnostics when runtime is missing.
        /// </summary>
        [Fact]
        public void TryInitialize_ReturnsDiagnosticState()
        {
            string error;
            bool available = OpenVinoSharp.GenAI.GenAI.TryInitialize(out error);

            if (available)
                Assert.True(OpenVinoSharp.GenAI.GenAI.IsAvailable);
            else
                Assert.False(string.IsNullOrWhiteSpace(error));
        }

        /// <summary>
        /// GenAI loader 应搜索 NuGet runtime native 布局 / GenAI loader should search the NuGet runtime native layout.
        /// </summary>
        [Fact]
        public void GenAINativeLibraryLoaderSearchPaths_IncludeNuGetRuntimeLayout()
        {
            string? oldRuntimeRoot = Environment.GetEnvironmentVariable("OPENVINO_GENAI_RUNTIME_DIR");
            string root = Path.Combine(Path.GetTempPath(), "ov-genai-runtime-" + Guid.NewGuid().ToString("N"));
            string expected = Path.Combine(
                root,
                "runtimes",
                NativeLibraryLoader.GetRuntimeIdentifier(),
                "native",
                GenAINativeLibraryLoader.GetLibraryName());

            Directory.CreateDirectory(Path.GetDirectoryName(expected)!);
            File.WriteAllBytes(expected, new byte[] { 0 });

            try
            {
                Environment.SetEnvironmentVariable("OPENVINO_GENAI_RUNTIME_DIR", root);
                string[] paths = GenAINativeLibraryLoader.GetPossibleLibraryPaths();

                Assert.Contains(paths, path => string.Equals(path, expected, StringComparison.OrdinalIgnoreCase));
            }
            finally
            {
                Environment.SetEnvironmentVariable("OPENVINO_GENAI_RUNTIME_DIR", oldRuntimeRoot);
                if (Directory.Exists(root))
                    Directory.Delete(root, recursive: true);
            }
        }

        /// <summary>
        /// GenerationConfig 应支持 UTF-8 停止词和 size_t 参数 / GenerationConfig should support UTF-8 stop strings and size_t parameters.
        /// </summary>
        [OpenVINOGenAIFact]
        public void GenerationConfig_SettersAndGetter_WorkWithUtf8()
        {
            using (var config = new GenerationConfig())
            {
                config
                    .SetMaxNewTokens(16)
                    .SetTemperature(0.7f)
                    .SetTopP(0.9f)
                    .SetTopK(40)
                    .SetDoSample(true)
                    .SetStopStrings("停止", "</s>")
                    .SetStopTokenIds(1, 2, 3)
                    .SetIncludeStopStringInOutput(false);

                Assert.Equal(16UL, config.GetMaxNewTokens());
                config.Validate();
            }
        }

        /// <summary>
        /// JsonContainer 应能处理 UTF-8 JSON 字符串 / JsonContainer should handle UTF-8 JSON strings.
        /// </summary>
        [OpenVINOGenAIFact]
        public void JsonContainer_RoundTripsJsonString()
        {
            using (var container = JsonContainer.FromJsonString("{\"role\":\"user\",\"content\":\"你好 OpenVINO\"}"))
            using (var copy = container.Copy())
            {
                string json = container.ToJsonString();
                string copiedJson = copy.ToJsonString();

                Assert.Contains("role", json);
                Assert.Contains("OpenVINO", json);
                Assert.Contains("OpenVINO", copiedJson);
            }
        }

        /// <summary>
        /// ChatHistory 应能保存中文消息 / ChatHistory should store Chinese messages.
        /// </summary>
        [OpenVINOGenAIFact]
        public void ChatHistory_AddsAndReadsUtf8Messages()
        {
            using (var history = new ChatHistory())
            {
                history.AddUserMessage("你好 OpenVINO");

                Assert.False(history.IsEmpty);
                Assert.Equal(1UL, history.Count);

                using (JsonContainer first = history.GetFirst())
                using (JsonContainer messages = history.GetMessages())
                {
                    Assert.Contains("user", first.ToJsonString());
                    Assert.Contains("OpenVINO", messages.ToJsonString());
                }
            }
        }
    }
}
