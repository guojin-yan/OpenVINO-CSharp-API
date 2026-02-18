// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using Xunit;

namespace OpenVinoSharp.Tests.UnitTests
{
    /// <summary>
    /// NodeOutput 类单元测试 / NodeOutput class unit tests
    /// </summary>
    public class NodeOutputTests
    {
        [OpenVINOFact]
        [Trait("Category", TestCategories.Unit)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void Constructor_WithValidPointer_CreatesNodeOutput()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("test_model.xml"))
            {
                return;
            }
            using var model = core.read_model("test_model.xml");
            
            // Act
            using var output = model.get_output(0);
            
            // Assert
            Assert.NotNull(output);
            Assert.True(output.IsValid);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Unit)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetElementType_ReturnsValidType()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("test_model.xml"))
            {
                return;
            }
            using var model = core.read_model("test_model.xml");
            using var output = model.get_output(0);

            // Act
            var elementType = output.get_element_type();

            // Assert
            Assert.NotNull(elementType);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Unit)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetShape_ReturnsValidShape()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("test_model.xml"))
            {
                return;
            }
            using var model = core.read_model("test_model.xml");
            using var output = model.get_output(0);

            // Act
            using var shape = output.get_shape();

            // Assert
            Assert.NotNull(shape);
            Assert.True(shape.get_rank() > 0);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Unit)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetPartialShape_ReturnsValidPartialShape()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("test_model.xml"))
            {
                return;
            }
            using var model = core.read_model("test_model.xml");
            using var output = model.get_output(0);

            // Act
            using var partialShape = output.get_partial_shape();

            // Assert
            Assert.NotNull(partialShape);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Unit)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetAnyName_ReturnsValidName()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("test_model.xml"))
            {
                return;
            }
            using var model = core.read_model("test_model.xml");
            using var output = model.get_output(0);

            // Act
            string name = output.get_any_name();

            // Assert
            Assert.NotNull(name);
            Assert.False(string.IsNullOrEmpty(name));
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Unit)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetIndex_ReturnsValidIndex()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("test_model.xml"))
            {
                return;
            }
            using var model = core.read_model("test_model.xml");
            using var output = model.get_output(0);

            // Act
            ulong index = output.get_index();

            // Assert
            Assert.Equal(0UL, index);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Unit)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetIndex_WithMultipleOutputs_ReturnsCorrectIndex()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("test_model.xml"))
            {
                return;
            }
            using var model = core.read_model("test_model.xml");
            
            // 获取第一个输出
            using var output0 = model.get_output(0);
            ulong index0 = output0.get_index();
            
            // 如果有多于一个输出，测试第二个
            ulong outputSize = model.get_outputs_size();
            if (outputSize > 1)
            {
                using var output1 = model.get_output(1);
                ulong index1 = output1.get_index();
                
                // Assert - 索引应该是递增的
                Assert.Equal(0UL, index0);
                Assert.Equal(1UL, index1);
            }
            else
            {
                // 只有一个输出时，索引应该是0
                Assert.Equal(0UL, index0);
            }
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Unit)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void Ptr_ReturnsValidPointer()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("test_model.xml"))
            {
                return;
            }
            using var model = core.read_model("test_model.xml");
            using var output = model.get_output(0);

            // Act
            System.IntPtr ptr = output.Ptr;

            // Assert
            Assert.NotEqual(System.IntPtr.Zero, ptr);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Unit)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void Dispose_CleansUpResources()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("test_model.xml"))
            {
                return;
            }
            using var model = core.read_model("test_model.xml");
            var output = model.get_output(0);

            // Act
            output.Dispose();

            // Assert
            Assert.True(output.IsDisposed);
        }
    }
}
