// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.IntegrationTests
{
    /// <summary>
    /// Tensor 高级集成测试 / Tensor advanced integration tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class TensorAdvancedTests
    {
        static TensorAdvancedTests()
        {
            // 确保 OpenVINO 原生库已加载
            // Ensure OpenVINO native library is loaded
            TestInitialization.Initialize();
        }
        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void SetData_WithFloatArray_UpdatesData()
        {
            // Arrange
            using var shape = new Shape(new long[] { 2, 3 });
            float[] data = new float[] { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f };
            using var tensor = new Tensor(shape, ElementType.F32);

            // Act
            tensor.set_data(data);

            // Assert
            float[] retrieved = tensor.get_float_data();
            Assert.Equal(data, retrieved);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void SetData_WithIntArray_UpdatesData()
        {
            // Arrange
            using var shape = new Shape(new long[] { 2, 3 });
            int[] data = new int[] { 1, 2, 3, 4, 5, 6 };
            using var tensor = new Tensor(shape, ElementType.I32);

            // Act
            tensor.set_data(data);

            // Assert
            int[] retrieved = tensor.get_int_data();
            Assert.Equal(data, retrieved);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void ElementType_AfterCreation_ReturnsCorrectType()
        {
            // Arrange
            using var shape = new Shape(new long[] { 2, 2 });

            // Act & Assert
            using (var tensorF32 = new Tensor(shape, ElementType.F32))
            {
                Assert.Equal(ElementType.F32, tensorF32.element_type);
            }

            using (var tensorI32 = new Tensor(shape, ElementType.I32))
            {
                Assert.Equal(ElementType.I32, tensorI32.element_type);
            }

            using (var tensorF64 = new Tensor(shape, ElementType.F64))
            {
                Assert.Equal(ElementType.F64, tensorF64.element_type);
            }
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void ByteSize_ForF32_Returns4xElementCount()
        {
            // Arrange
            using var shape = new Shape(new long[] { 2, 3 });
            using var tensor = new Tensor(shape, ElementType.F32);
            ulong expectedSize = (ulong)(6 * 4); // 6 elements * 4 bytes

            // Act
            ulong actualSize = tensor.byte_size;

            // Assert
            Assert.Equal(expectedSize, actualSize);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void Size_ReturnsTotalElementCount()
        {
            // Arrange
            using var shape = new Shape(new long[] { 2, 3, 4 });
            using var tensor = new Tensor(shape, ElementType.F32);
            ulong expectedSize = 24; // 2*3*4

            // Act
            ulong actualSize = tensor.size;

            // Assert
            Assert.Equal(expectedSize, actualSize);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void Shape_ReturnsCorrectShape()
        {
            // Arrange
            long[] expectedDims = { 1, 3, 224, 224 };
            using var shape = new Shape(expectedDims);
            using var tensor = new Tensor(shape, ElementType.F32);

            // Act
            var tensorShape = tensor.shape;
            long[] actualDims = tensorShape.get_dims();

            // Assert
            Assert.Equal(expectedDims, actualDims);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void Data_ReturnsValidPointer()
        {
            // Arrange
            using var shape = new Shape(new long[] { 2, 2 });
            using var tensor = new Tensor(shape, ElementType.F32);

            // Act
            System.IntPtr dataPtr = tensor.data();

            // Assert
            Assert.NotEqual(System.IntPtr.Zero, dataPtr);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetByteData_ReturnsCorrectData()
        {
            // Arrange
            using var shape = new Shape(new long[] { 2, 2 });
            float[] inputData = new float[] { 1.0f, 2.0f, 3.0f, 4.0f };
            using var tensor = new Tensor(shape, inputData);

            // Act
            byte[] byteData = tensor.get_byte_data();

            // Assert
            Assert.Equal(16, byteData.Length); // 4 floats * 4 bytes
        }

    }
}
