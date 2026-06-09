// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using Xunit;
using OpenVinoSharp.Internal;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.IntegrationTests
{
    /// <summary>
    /// Core 类集成测试 / Core class integration tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class CoreIntegrationTests
    {
        static CoreIntegrationTests()
        {
            // 确保 OpenVINO 原生库已加载
            // Ensure OpenVINO native library is loaded
            TestInitialization.Initialize();
        }
        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void Constructor_CreatesCoreInstance()
        {
            // Act
            using var core = new Core();

            // Assert
            Assert.NotNull(core);
            Assert.True(core.IsValid);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetAvailableDevices_ReturnsDeviceList()
        {
            // Arrange
            using var core = new Core();

            // Act
            var devices = core.get_available_devices();

            // Assert
            Assert.NotNull(devices);
            Assert.True(devices.Count > 0, "At least one device should be available");
            Assert.Contains("CPU", devices); // CPU 应该始终可用
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetVersions_ReturnsVersionInfo()
        {
            // Arrange
            using var core = new Core();

            // Act
            var versionInfo = core.get_versions("CPU");

            // Assert
            Assert.NotNull(versionInfo.Key);
            Assert.False(string.IsNullOrEmpty(versionInfo.Value.description));
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetOpenVINOVersion_ReturnsVersion()
        {
            // Act
            var version = Ov.get_openvino_version();

            // Assert
            Assert.False(string.IsNullOrEmpty(version.description));
            Assert.False(string.IsNullOrEmpty(version.buildNumber));
        }
    }
}
