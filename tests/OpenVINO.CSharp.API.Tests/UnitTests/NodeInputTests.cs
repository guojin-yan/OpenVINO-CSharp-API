// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.UnitTests
{
    /// <summary>
    /// NodeInput 类单元测试 / NodeInput class unit tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class NodeInputTests
    {
        static NodeInputTests()
        {
            TestInitialization.Initialize();
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Unit)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void Constructor_WithValidPointer_CreatesNodeInput()
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
            Assert.True(input.IsValid);
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
            using var input = model.get_input(0);

            // Act
            var elementType = input.get_element_type();

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
            using var input = model.get_input(0);

            // Act
            using var shape = input.get_shape();

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
            using var input = model.get_input(0);

            // Act
            using var partialShape = input.get_partial_shape();

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
            using var input = model.get_input(0);

            // Act
            string name = input.get_any_name();

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
            using var input = model.get_input(0);

            // Act
            System.IntPtr ptr = input.Ptr;

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
            var input = model.get_input(0);

            // Act
            input.Dispose();

            // Assert
            Assert.True(input.IsDisposed);
        }
    }
}
