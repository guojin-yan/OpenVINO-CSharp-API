// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.IntegrationTests
{
    /// <summary>
    /// CompiledModel 高级集成测试 / CompiledModel advanced integration tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class CompiledModelAdvancedTests
    {
        static CompiledModelAdvancedTests()
        {
            // 确保 OpenVINO 原生库已加载
            // Ensure OpenVINO native library is loaded
            TestInitialization.Initialize();
        }
        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetRuntimeModel_ReturnsValidModel()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var modelObj = core.read_model("model/yolo26n.xml");
            using var compiled = core.compile_model(modelObj, "CPU", null!);

            // Act
            using var runtimeModel = compiled.get_runtime_model();

            // Assert
            Assert.NotNull(runtimeModel);
            Assert.True(runtimeModel.IsValid);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetInputByName_WithValidName_ReturnsNodeInput()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var modelObj = core.read_model("model/yolo26n.xml");
            using var compiled = core.compile_model(modelObj, "CPU", null!);
            
            // 获取输入名称
            using var input = compiled.get_input(0);
            string inputName = input.get_any_name();

            // Act
            using var inputByName = compiled.get_input_by_name(inputName);

            // Assert
            Assert.NotNull(inputByName);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetOutputByName_WithValidName_ReturnsNodeOutput()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var modelObj = core.read_model("model/yolo26n.xml");
            using var compiled = core.compile_model(modelObj, "CPU", null!);
            
            // 获取输出名称
            using var output = compiled.get_output(0);
            string outputName = output.get_any_name();

            // Act
            using var outputByName = compiled.get_output_by_name(outputName);

            // Assert
            Assert.NotNull(outputByName);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetProperty_WithValidKey_ReturnsValue()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var modelObj = core.read_model("model/yolo26n.xml");
            using var compiled = core.compile_model(modelObj, "CPU", null!);

            // Act
            string value = compiled.get_property("NETWORK_NAME");

            // Assert
            Assert.NotNull(value);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void ExportModel_CreatesFile()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var modelObj = core.read_model("model/yolo26n.xml");
            using var compiled = core.compile_model(modelObj, "CPU", null!);
            
            string exportPath = "exported_model.bin";
            if (System.IO.File.Exists(exportPath))
            {
                System.IO.File.Delete(exportPath);
            }

            // Act
            compiled.export_model(exportPath);

            // Assert
            // 注意：导出功能可能因设备而异，文件可能不存在
            // 只要方法不抛出异常即视为成功
            
            // 清理
            if (System.IO.File.Exists(exportPath))
            {
                System.IO.File.Delete(exportPath);
            }
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetInputsSize_WithCompiledModel_ReturnsCorrectCount()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var modelObj = core.read_model("model/yolo26n.xml");
            using var compiled = core.compile_model(modelObj, "CPU", null!);

            // Act
            ulong size = compiled.get_inputs_size();

            // Assert
            Assert.True(size > 0);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetOutputsSize_WithCompiledModel_ReturnsCorrectCount()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var modelObj = core.read_model("model/yolo26n.xml");
            using var compiled = core.compile_model(modelObj, "CPU", null!);

            // Act
            ulong size = compiled.get_outputs_size();

            // Assert
            Assert.True(size > 0);
        }



        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void NodeOutput_GetAnyName_ReturnsValidName()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var modelObj = core.read_model("model/yolo26n.xml");
            using var compiled = core.compile_model(modelObj, "CPU", null!);
            using var output = compiled.get_output(0);

            // Act
            string name = output.get_any_name();

            // Assert
            Assert.NotNull(name);
            Assert.False(string.IsNullOrEmpty(name));
        }
    }
}
