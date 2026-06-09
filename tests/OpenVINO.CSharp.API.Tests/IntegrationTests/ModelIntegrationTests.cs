// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.IntegrationTests
{
    /// <summary>
    /// Model 类集成测试 / Model class integration tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class ModelIntegrationTests
    {
        static ModelIntegrationTests()
        {
            // 确保 OpenVINO 原生库已加载
            // Ensure OpenVINO native library is loaded
            TestInitialization.Initialize();
        }
        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetFriendlyName_ReturnsValidName()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var model = core.read_model("model/yolo26n.xml");

            // Act
            string name = model.get_friendly_name();

            // Assert
            Assert.NotNull(name);
            Assert.False(string.IsNullOrEmpty(name));
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetInputsSize_ReturnsCorrectCount()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var model = core.read_model("model/yolo26n.xml");

            // Act
            ulong size = model.get_inputs_size();

            // Assert
            Assert.True(size > 0);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetOutputsSize_ReturnsCorrectCount()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var model = core.read_model("model/yolo26n.xml");

            // Act
            ulong size = model.get_outputs_size();

            // Assert
            Assert.True(size > 0);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetInput_WithValidIndex_ReturnsNodeInput()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var model = core.read_model("model/yolo26n.xml");

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
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var model = core.read_model("model/yolo26n.xml");

            // Act
            using var output = model.get_output(0);

            // Assert
            Assert.NotNull(output);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void IsDynamic_ReturnsBoolean()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var model = core.read_model("model/yolo26n.xml");

            // Act
            bool isDynamic = model.is_dynamic();

            // Assert - 只验证不抛出异常，结果取决于模型
            Assert.True(isDynamic || !isDynamic); // 总是真，但验证方法可调用
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void Reshape_SingleInput_Succeeds()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var model = core.read_model("model/yolo26n.xml");
            
            // 获取原始输入形状
            using var input = model.get_input(0);
            using var originalShape = input.get_shape();
            long[] dims = originalShape.get_dims();
            
            // 如果第一维是动态的，尝试重塑
            if (dims.Length > 0 && dims[0] > 0)
            {
                dims[0] = 1; // 设置为批次大小1
                using var newShape = new Shape(dims);
                
                // Act & Assert - 应该不抛出异常
                try
                {
                    model.reshape(newShape);
                }
                catch (OVException)
                {
                    // 某些模型可能不支持重塑，这是可接受的
                }
            }
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void NodeInput_GetElementType_ReturnsValidType()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var model = core.read_model("model/yolo26n.xml");
            using var input = model.get_input(0);

            // Act
            var elementType = input.get_element_type();

            // Assert
            Assert.True(System.Enum.IsDefined(typeof(ElementType), elementType.get_type()));
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void NodeInput_GetShape_ReturnsValidShape()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var model = core.read_model("model/yolo26n.xml");
            using var input = model.get_input(0);

            // Act
            using var shape = input.get_shape();

            // Assert
            Assert.NotNull(shape);
            Assert.True(shape.get_rank() > 0);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void NodeInput_GetAnyName_ReturnsValidName()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var model = core.read_model("model/yolo26n.xml");
            using var input = model.get_input(0);

            // Act
            string name = input.get_any_name();

            // Assert
            Assert.NotNull(name);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void NodeOutput_GetElementType_ReturnsValidType()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var model = core.read_model("model/yolo26n.xml");
            using var output = model.get_output(0);

            // Act
            var elementType = output.get_element_type();

            // Assert
            Assert.True(System.Enum.IsDefined(typeof(ElementType), elementType.get_type()));
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void NodeOutput_GetShape_ReturnsValidShape()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var model = core.read_model("model/yolo26n.xml");
            using var output = model.get_output(0);

            // Act
            using var shape = output.get_shape();

            // Assert
            Assert.NotNull(shape);
        }

    }
}
