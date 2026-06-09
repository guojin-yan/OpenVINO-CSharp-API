// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.UnitTests
{
    /// <summary>
    /// Ov 静态类测试 / Ov static class tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class OvTests
    {
        static OvTests()
        {
            TestInitialization.Initialize();
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Unit)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetOpenvinoVersion_ReturnsVersion()
        {
            // Act
            var version = Ov.get_openvino_version();

            // Assert
            Assert.False(string.IsNullOrEmpty(version.description));
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void ContentFromFile_WithInvalidPath_ThrowsException()
        {
            // Arrange
            string invalidPath = "nonexistent_file.bin";

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => Ov.content_from_file(invalidPath));
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Unit)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void Shutdown_DoesNotThrow()
        {
            // Act & Assert
            // 注意：shutdown 会影响所有 OpenVINO 实例，谨慎使用
            // Ov.shutdown();
        }
    }
}
