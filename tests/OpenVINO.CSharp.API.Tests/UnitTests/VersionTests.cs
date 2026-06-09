// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.UnitTests
{
    /// <summary>
    /// Version 结构体测试 / Version structure tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class VersionTests
    {
        static VersionTests()
        {
            TestInitialization.Initialize();
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Unit)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetOpenVINOVersion_ReturnsValidVersion()
        {
            // Act
            var version = Ov.get_openvino_version();

            // Assert
            Assert.False(string.IsNullOrEmpty(version.description));
            Assert.False(string.IsNullOrEmpty(version.buildNumber));
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Unit)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void Version_Description_ContainsOpenVINO()
        {
            // Act
            var version = Ov.get_openvino_version();

            // Assert
            Assert.Contains("OpenVINO", version.description);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Unit)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void Version_BuildNumber_IsNotEmpty()
        {
            // Act
            var version = Ov.get_openvino_version();

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(version.buildNumber));
        }
    }
}
