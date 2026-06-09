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
        /// WhisperGenerationConfig 应支持基础 token、size_t 和 bool getter/setter / WhisperGenerationConfig should support basic token, size_t, and bool getters/setters.
        /// </summary>
        [OpenVINOGenAIFact]
        public void WhisperGenerationConfig_DefaultsAndSetters_Work()
        {
            using (var config = new WhisperGenerationConfig())
            {
                Assert.Equal(50258L, config.GetDecoderStartTokenId());
                Assert.Equal(50257L, config.GetPadTokenId());
                Assert.Equal(50358L, config.GetTranslateTokenId());
                Assert.Equal(50359L, config.GetTranscribeTokenId());
                Assert.Equal(50361L, config.GetPrevSotTokenId());
                Assert.Equal(50363L, config.GetNoTimestampsTokenId());

                config
                    .SetDecoderStartTokenId(60001)
                    .SetPadTokenId(60002)
                    .SetTranslateTokenId(60003)
                    .SetTranscribeTokenId(60004)
                    .SetPrevSotTokenId(60005)
                    .SetNoTimestampsTokenId(60006)
                    .SetMaxInitialTimestampIndex(24)
                    .SetIsMultilingual(true)
                    .SetReturnTimestamps(true);

                Assert.Equal(60001L, config.DecoderStartTokenId);
                Assert.Equal(60002L, config.PadTokenId);
                Assert.Equal(60003L, config.TranslateTokenId);
                Assert.Equal(60004L, config.TranscribeTokenId);
                Assert.Equal(60005L, config.PrevSotTokenId);
                Assert.Equal(60006L, config.NoTimestampsTokenId);
                Assert.Equal(24UL, config.MaxInitialTimestampIndex);
                Assert.True(config.IsMultilingual);
                Assert.True(config.ReturnTimestamps);

                using (GenerationConfig generationConfig = config.GetGenerationConfig())
                {
                    generationConfig.SetMaxNewTokens(8);
                    Assert.Equal(8UL, generationConfig.GetMaxNewTokens());
                }
            }
        }

        /// <summary>
        /// WhisperGenerationConfig 应正确处理 UTF-8 可选字符串 / WhisperGenerationConfig should handle optional UTF-8 strings.
        /// </summary>
        [OpenVINOGenAIFact]
        public void WhisperGenerationConfig_OptionalStrings_WorkWithUtf8AndUnset()
        {
            using (var config = new WhisperGenerationConfig())
            {
                Assert.Null(config.GetLanguage());
                Assert.Null(config.GetTask());
                Assert.Null(config.GetInitialPrompt());
                Assert.Null(config.GetHotwords());

                config
                    .SetLanguage("zh")
                    .SetTask("transcribe")
                    .SetInitialPrompt("你好 OpenVINO")
                    .SetHotwords("OpenVINO 热词");

                Assert.Equal("zh", config.Language);
                Assert.Equal("transcribe", config.Task);
                Assert.Equal("你好 OpenVINO", config.InitialPrompt);
                Assert.Equal("OpenVINO 热词", config.Hotwords);

                config
                    .SetLanguage(null)
                    .SetTask(null)
                    .SetInitialPrompt(null)
                    .SetHotwords(null);

                Assert.Null(config.Language);
                Assert.Null(config.Task);
                Assert.Null(config.InitialPrompt);
                Assert.Null(config.Hotwords);
            }
        }

        /// <summary>
        /// WhisperGenerationConfig 应正确读写 token 数组 / WhisperGenerationConfig should read and write token arrays.
        /// </summary>
        [OpenVINOGenAIFact]
        public void WhisperGenerationConfig_TokenArrays_RoundTrip()
        {
            using (var config = new WhisperGenerationConfig())
            {
                config
                    .SetBeginSuppressTokens(1, 2, 3)
                    .SetSuppressTokens(4, 5, 6, 7);

                Assert.Equal(3UL, config.GetBeginSuppressTokensCount());
                Assert.Equal(new long[] { 1, 2, 3 }, config.GetBeginSuppressTokens());
                Assert.Equal(4UL, config.GetSuppressTokensCount());
                Assert.Equal(new long[] { 4, 5, 6, 7 }, config.GetSuppressTokens());

                config.BeginSuppressTokens = new long[0];
                config.SuppressTokens = new long[0];

                Assert.Empty(config.BeginSuppressTokens);
                Assert.Empty(config.SuppressTokens);
            }
        }

        /// <summary>
        /// JsonContainer should handle UTF-8 JSON strings. / JsonContainer 应能处理 UTF-8 JSON 字符串。
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

        /// <summary>
        /// Whisper 分段结果应能安全创建、读取默认值并释放。
        /// Whisper result chunks should be safely created, read with default values, and released.
        /// </summary>
        [OpenVINOGenAIFact]
        public void WhisperDecodedResultChunk_DefaultObject_IsReadable()
        {
            using (var chunk = new WhisperDecodedResultChunk())
            {
                Assert.False(float.IsNaN(chunk.StartTimestamp));
                Assert.False(float.IsNaN(chunk.EndTimestamp));
                Assert.Equal(string.Empty, chunk.Text);
                Assert.Equal(string.Empty, chunk.ToString());
            }
        }

        /// <summary>
        /// Whisper 解码结果应能安全创建、读取计数、处理空分段并释放 metrics。
        /// Whisper decoded results should safely expose counts, empty chunks, and owned metrics.
        /// </summary>
        [OpenVINOGenAIFact]
        public void WhisperDecodedResults_DefaultObject_IsReadable()
        {
            using (var results = new WhisperDecodedResults())
            {
                Assert.Equal(0UL, results.TextCount);
                Assert.False(results.HasChunks);
                Assert.Equal(0UL, results.ChunkCount);
                Assert.Null(results.GetChunkAt(0));
                Assert.ThrowsAny<Exception>(() => results.GetTextAt(0));
                Assert.ThrowsAny<Exception>(() => results.GetScoreAt(0));

                using (PerformanceMetrics metrics = results.GetPerformanceMetrics())
                {
                    Assert.NotNull(metrics);
                }

                Assert.NotNull(results.GetString());
            }
        }

        /// <summary>
        /// VLM 解码结果应能安全创建、读取默认文本并释放 metrics。
        /// VLM decoded results should safely expose default text and owned metrics.
        /// </summary>
        [OpenVINOGenAIFact]
        public void VLMDecodedResults_DefaultObject_IsReadable()
        {
            using (var results = new VLMDecodedResults())
            {
                Assert.NotNull(results.Text);
                Assert.Equal(results.Text, results.ToString());

                using (PerformanceMetrics metrics = results.GetPerformanceMetrics())
                {
                    Assert.NotNull(metrics);
                }
            }
        }

        /// <summary>
        /// Pipeline 构造函数应在进入 native 前验证托管参数。
        /// Pipeline constructors should validate managed arguments before entering native code.
        /// </summary>
        [Fact]
        public void GenAIPipelines_ValidateConstructorArguments()
        {
            Assert.Throws<ArgumentException>(() => new WhisperPipeline(string.Empty, "CPU"));
            Assert.Throws<ArgumentException>(() => new WhisperPipeline("model", string.Empty));
            Assert.Throws<ArgumentException>(() => new VLMPipeline(string.Empty, "CPU"));
            Assert.Throws<ArgumentException>(() => new VLMPipeline("model", string.Empty));
        }
    }
}
