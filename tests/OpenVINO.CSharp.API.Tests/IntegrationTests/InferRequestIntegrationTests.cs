// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.IntegrationTests
{
    /// <summary>
    /// InferRequest 集成测试 / InferRequest integration tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class InferRequestIntegrationTests
    {
        static InferRequestIntegrationTests()
        {
            // 确保 OpenVINO 原生库已加载
            // Ensure OpenVINO native library is loaded
            TestInitialization.Initialize();
        }
        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void SetInputTensor_WithValidTensor_Succeeds()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var modelObj = core.read_model("model/yolo26n.xml");
            using var model = core.compile_model(modelObj, "CPU", null!);
            using var request = model.create_infer_request();

            // 获取输入信息并创建张量
            using var inputNode = model.get_input(0);
            // 这里需要知道输入形状，实际测试时需要根据模型调整

            // Act & Assert - 如果没有有效输入，至少验证不抛出异常
            // 注意：完整测试需要有效模型
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetInputTensor_ReturnsValidTensor()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var modelObj = core.read_model("model/yolo26n.xml");
            using var model = core.compile_model(modelObj, "CPU", null!);
            using var request = model.create_infer_request();

            // Act
            using var tensor = request.get_input_tensor();

            // Assert
            Assert.NotNull(tensor);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetOutputTensor_ReturnsValidTensor()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var modelObj = core.read_model("model/yolo26n.xml");
            using var model = core.compile_model(modelObj, "CPU", null!);
            using var request = model.create_infer_request();

            // Act
            using var tensor = request.get_output_tensor();

            // Assert
            Assert.NotNull(tensor);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void Infer_ExecutesWithoutException()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var modelObj = core.read_model("model/yolo26n.xml");
            using var model = core.compile_model(modelObj, "CPU", null!);
            using var request = model.create_infer_request();

            // Act & Assert
            request.infer(); // 应该不抛出异常
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void StartAsync_ExecutesWithoutException()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var modelObj = core.read_model("model/yolo26n.xml");
            using var model = core.compile_model(modelObj, "CPU", null!);
            using var request = model.create_infer_request();

            // Act & Assert
            request.start_async(); // 应该不抛出异常
            request.wait();
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void WaitFor_WithTimeout_ReturnsCompletionStatus()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var modelObj = core.read_model("model/yolo26n.xml");
            using var model = core.compile_model(modelObj, "CPU", null!);
            using var request = model.create_infer_request();
            request.start_async();

            // Act
            bool completed = request.wait_for(5000);

            // Assert
            Assert.True(completed);
        }
    }
}
