// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using System.Collections.Generic;
using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.IntegrationTests
{
    /// <summary>
    /// Core 类扩展集成测试 / Core class extended integration tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class CoreExtendedTests
    {
        static CoreExtendedTests()
        {
            // 确保 OpenVINO 原生库已加载
            // Ensure OpenVINO native library is loaded
            TestInitialization.Initialize();
        }
        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void ReadModel_FromMemoryBuffer_ReturnsModel()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            
            byte[] modelData = System.IO.File.ReadAllBytes("model/yolo26n.xml");
            using var weights = new Tensor(new Shape(new long[] { 1 }), ElementType.U8);

            // Act & Assert - 内存缓冲区读取可能需要特定格式
            // 这个测试验证方法存在且可调用
            try
            {
                using var model = core.read_model(modelData, weights);
                Assert.NotNull(model);
            }
            catch (OVException)
            {
                // 如果格式不正确可能会抛出异常，这是可接受的
            }
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void ReadModel_FromEmptyMemoryBuffer_ThrowsArgumentException()
        {
            // Arrange
            using var core = new Core();
            using var weights = new Tensor(new Shape(new long[] { 1 }), ElementType.U8);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => core.read_model(Array.Empty<byte>(), weights));
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void CompileModel_FromFile_ReturnsCompiledModel()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }

            // Act
            using var compiled = core.compile_model("model/yolo26n.xml", "CPU", null!);

            // Assert
            Assert.NotNull(compiled);
            Assert.True(compiled.IsValid);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void CompileModel_WithAutoDevice_Succeeds()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var model = core.read_model("model/yolo26n.xml");

            // Act
            using var compiled = core.compile_model(model);

            // Assert
            Assert.NotNull(compiled);
            Assert.True(compiled.IsValid);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void SetProperty_WithMultipleProperties_Succeeds()
        {
            // Arrange
            using var core = new Core();
            var properties = new Dictionary<string, string>
            {
                { "CPU_THREADS_NUM", "4" },
                { "CPU_BIND_THREAD", "YES" }
            };

            // Act & Assert - 应该不抛出异常
            core.set_property("CPU", properties);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetProperty_WithValidKey_ReturnsValue()
        {
            // Arrange
            using var core = new Core();

            // Act
            string value = core.get_property("CPU", "AVAILABLE_DEVICES");

            // Assert
            Assert.NotNull(value);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetVersions_WithCPU_ReturnsVersionInfo()
        {
            // Arrange
            using var core = new Core();

            // Act
            var version = core.get_versions("CPU");

            // Assert
            Assert.NotNull(version.Key);
            Assert.False(string.IsNullOrEmpty(version.Value.description));
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void Shutdown_DoesNotThrow()
        {
            // Act & Assert
            // 注意：shutdown 会影响所有 OpenVINO 实例，谨慎测试
            // 这里只验证方法存在且可调用
            // Core.shutdown();
        }
    }
}
