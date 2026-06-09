// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.IntegrationTests
{
    /// <summary>
    /// RemoteContext 远程上下文测试 / Remote context tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class RemoteContextTests
    {
        static RemoteContextTests()
        {
            // 确保 OpenVINO 原生库已加载
            // Ensure OpenVINO native library is loaded
            TestInitialization.Initialize();
        }
        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        [Trait("Category", "GPU")]
        public void GetDefaultContext_WithGPU_ReturnsContext()
        {
            // Arrange
            using var core = new Core();
            var devices = core.get_available_devices();
            
            if (!devices.Contains("GPU"))
            {
                return; // 跳过如果没有GPU / Skip if no GPU
            }

            // Act
            IntPtr contextPtr = core.get_default_context("GPU");

            // Assert
            Assert.NotEqual(IntPtr.Zero, contextPtr);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        [Trait("Category", "GPU")]
        public void CompiledModel_GetContext_WithGPU_ReturnsContext()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            
            var devices = core.get_available_devices();
            if (!devices.Contains("GPU"))
            {
                return; // 跳过如果没有GPU / Skip if no GPU
            }

            using var modelObj = core.read_model("model/yolo26n.xml");
            using var model = core.compile_model(modelObj, "GPU", null!);

            // Act
            using var context = model.get_context();

            // Assert
            Assert.NotNull(context);
            Assert.True(context.IsValid);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        [Trait("Category", "GPU")]
        public void RemoteContext_GetDeviceName_WithGPU_ReturnsGPU()
        {
            // Arrange
            using var core = new Core();
            var devices = core.get_available_devices();
            
            if (!devices.Contains("GPU"))
            {
                return; // 跳过如果没有GPU / Skip if no GPU
            }

            using var context = new RemoteContext(core, "GPU");

            // Act
            string deviceName = context.get_device_name();

            // Assert
            Assert.Contains("GPU", deviceName);
        }
    }
}
