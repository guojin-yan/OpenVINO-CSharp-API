// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using Xunit;

namespace OpenVinoSharp.Tests.IntegrationTests
{
    /// <summary>
    /// CompiledModel 集成测试 / CompiledModel integration tests
    /// </summary>
    public class CompiledModelIntegrationTests
    {
        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetInputsSize_ReturnsCorrectCount()
        {
            // Arrange
            using var core = new Core();
            // Note: 这里需要一个实际的模型文件才能测试
            // 如果没有模型文件，测试会被跳过
            if (!System.IO.File.Exists("test_model.xml"))
            {
                return;
            }
            using var modelObj = core.read_model("test_model.xml");
            using var model = core.compile_model(modelObj, "CPU", null);

            // Act
            ulong inputSize = model.get_inputs_size();

            // Assert
            Assert.True(inputSize > 0);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetOutputsSize_ReturnsCorrectCount()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("test_model.xml"))
            {
                return;
            }
            using var modelObj = core.read_model("test_model.xml");
            using var model = core.compile_model(modelObj, "CPU", null);

            // Act
            ulong outputSize = model.get_outputs_size();

            // Assert
            Assert.True(outputSize > 0);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void CreateInferRequest_ReturnsValidRequest()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("test_model.xml"))
            {
                return;
            }
            using var modelObj = core.read_model("test_model.xml");
            using var model = core.compile_model(modelObj, "CPU", null);

            // Act
            using var request = model.create_infer_request();

            // Assert
            Assert.NotNull(request);
            Assert.True(request.IsValid);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetInput_WithValidIndex_ReturnsNodeInput()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("test_model.xml"))
            {
                return;
            }
            using var modelObj = core.read_model("test_model.xml");
            using var model = core.compile_model(modelObj, "CPU", null);

            // Act
            using var input = model.get_input(0);

            // Assert
            Assert.NotNull(input);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetOutput_WithValidIndex_ReturnsNodeOutput()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("test_model.xml"))
            {
                return;
            }
            using var modelObj = core.read_model("test_model.xml");
            using var model = core.compile_model(modelObj, "CPU", null);

            // Act
            using var output = model.get_output(0);

            // Assert
            Assert.NotNull(output);
        }
    }
}
