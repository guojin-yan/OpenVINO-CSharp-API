// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using System.Collections.Generic;
using System.IO;
using OpenVinoSharp.Tests.TestHelpers;
using Xunit;

namespace OpenVinoSharp.Tests.IntegrationTests
{
    /// <summary>
    /// Windows Unicode 路径集成测试 / Integration tests for Windows Unicode path APIs.
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class CoreUnicodePathTests
    {
        private const string SourceModelPath = "model/yolo26n.xml";
        private const string SourceWeightsPath = "model/yolo26n.bin";

        static CoreUnicodePathTests()
        {
            TestInitialization.Initialize();
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void CreateWithConfigUnicode_WithEmptyPath_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Core.CreateWithConfigUnicode(string.Empty));
            Assert.Throws<ArgumentException>(() => Core.create_with_config_unicode(null!));
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void ReadModelUnicode_WithEmptyPath_ThrowsArgumentException()
        {
            using var core = new Core();

            Assert.Throws<ArgumentException>(() => core.ReadModelUnicode(string.Empty));
            Assert.Throws<ArgumentException>(() => core.read_model_unicode(null!));
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void ReadModelUnicode_FromUnicodeDirectory_ReturnsModelWhenRuntimeSupportsIt()
        {
            if (!File.Exists(SourceModelPath))
                return;

            using var core = new Core();
            string modelPath = CreateUnicodeModelCopy();
            try
            {
                using var model = core.ReadModelUnicode(modelPath);

                Assert.NotNull(model);
                Assert.True(model.IsValid);
            }
            catch (PlatformNotSupportedException)
            {
                // Older OpenVINO runtimes may not export ov_core_read_model_unicode.
                // 旧版 OpenVINO runtime 可能未导出 ov_core_read_model_unicode。
            }
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void CompileModelUnicode_FromUnicodeDirectory_ReturnsCompiledModelWhenRuntimeSupportsIt()
        {
            if (!File.Exists(SourceModelPath))
                return;

            using var core = new Core();
            string modelPath = CreateUnicodeModelCopy();
            try
            {
                using var compiled = core.CompileModelUnicode(modelPath, "CPU");

                Assert.NotNull(compiled);
                Assert.True(compiled.IsValid);
            }
            catch (PlatformNotSupportedException)
            {
                // Older OpenVINO runtimes may not export ov_core_compile_model_from_file_unicode.
                // 旧版 OpenVINO runtime 可能未导出 ov_core_compile_model_from_file_unicode。
            }
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void CompileModelUnicode_WithTooManyProperties_ThrowsArgumentException()
        {
            if (!File.Exists(SourceModelPath))
                return;

            using var core = new Core();
            string modelPath = CreateUnicodeModelCopy();
            var properties = new Dictionary<string, string>
            {
                { "KEY1", "VALUE1" },
                { "KEY2", "VALUE2" },
                { "KEY3", "VALUE3" },
                { "KEY4", "VALUE4" }
            };

            Assert.Throws<ArgumentException>(() => core.CompileModelUnicode(modelPath, "CPU", properties));
        }

        private static string CreateUnicodeModelCopy()
        {
            string directory = Path.Combine(Path.GetTempPath(), "OpenVINOSharp-路径测试-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(directory);

            string modelPath = Path.Combine(directory, "yolo26n.xml");
            File.Copy(SourceModelPath, modelPath, overwrite: true);

            if (File.Exists(SourceWeightsPath))
            {
                string weightsPath = Path.Combine(directory, "yolo26n.bin");
                File.Copy(SourceWeightsPath, weightsPath, overwrite: true);
            }

            return modelPath;
        }
    }
}
