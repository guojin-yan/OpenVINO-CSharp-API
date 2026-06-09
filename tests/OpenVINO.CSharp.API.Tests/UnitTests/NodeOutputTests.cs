// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.UnitTests
{
    /// <summary>
    /// NodeOutput 类单元测试 / NodeOutput class unit tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class NodeOutputTests
    {
        static NodeOutputTests()
        {
            TestInitialization.Initialize();
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Unit)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void Constructor_WithValidPointer_CreatesNodeOutput()
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
            Assert.True(output.IsValid);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Unit)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetElementType_ReturnsValidType()
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
        [Trait("Category", TestCategories.Unit)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetShape_ReturnsValidShape()
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
            Assert.True(shape.get_rank() > 0);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Unit)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetPartialShape_ReturnsValidPartialShape()
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
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var model = core.read_model("model/yolo26n.xml");
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
        public void Ptr_ReturnsValidPointer()
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
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var model = core.read_model("model/yolo26n.xml");
            var output = model.get_output(0);

            // Act
            output.Dispose();

            // Assert
            Assert.True(output.IsDisposed);
        }
    }
}
