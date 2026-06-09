// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System.Collections.Generic;
using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.IntegrationTests
{
    /// <summary>
    /// Core 类高级集成测试 / Core class advanced integration tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class CoreAdvancedTests
    {
        static CoreAdvancedTests()
        {
            // 确保 OpenVINO 原生库已加载
            // Ensure OpenVINO native library is loaded
            TestInitialization.Initialize();
        }
        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void CompileModel_WithProperties_Succeeds()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }

            var properties = new Dictionary<string, string>
            {
                { "PERFORMANCE_HINT", "LATENCY" }
            };

            // Act
            using var modelObj = core.read_model("model/yolo26n.xml");
            using var model = core.compile_model(modelObj, "CPU", properties);

            // Assert
            Assert.NotNull(model);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void ReadModel_FromFile_ReturnsModel()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }

            // Act
            using var model = core.read_model("model/yolo26n.xml");

            // Assert
            Assert.NotNull(model);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void SetProperty_WithValidProperty_Succeeds()
        {
            // Arrange
            using var core = new Core();

            // Act & Assert - 设置 CPU 设备的属性
            // 注意：具体属性取决于 OpenVINO 版本
            core.set_property("CPU", "CPU_THREADS_NUM", "4");
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetProperty_ReturnsPropertyValue()
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
        public void Constructor_WithConfigFile_ThrowsOrSucceeds()
        {
            // 测试带配置文件的构造函数
            // 如果没有配置文件，应该抛出异常或创建失败
            if (!System.IO.File.Exists("test_config.xml"))
            {
                // 没有配置文件时跳过
                return;
            }

            // Act
            using var core = new Core("test_config.xml");

            // Assert
            Assert.NotNull(core);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void CompileModel_FromModelObject_Succeeds()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var model = core.read_model("model/yolo26n.xml");

            // Act
            using var compiled = core.compile_model(model, "CPU", null!);

            // Assert
            Assert.NotNull(compiled);
        }
    }
}
