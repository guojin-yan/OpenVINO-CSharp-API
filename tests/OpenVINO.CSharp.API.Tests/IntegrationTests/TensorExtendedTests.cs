// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.IntegrationTests
{
    /// <summary>
    /// Tensor 扩展集成测试 / Tensor extended integration tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class TensorExtendedTests
    {
        static TensorExtendedTests()
        {
            // 确保 OpenVINO 原生库已加载
            // Ensure OpenVINO native library is loaded
            TestInitialization.Initialize();
        }
        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetIntData_ReturnsCorrectData()
        {
            // Arrange
            using var shape = new Shape(new long[] { 2, 3 });
            int[] inputData = new int[] { 1, 2, 3, 4, 5, 6 };
            using var tensor = new Tensor(shape, ElementType.I32);
            tensor.set_data(inputData);

            // Act
            int[] outputData = tensor.get_int_data();

            // Assert
            Assert.Equal(inputData, outputData);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetLongData_ReturnsCorrectData()
        {
            // Arrange
            using var shape = new Shape(new long[] { 2, 2 });
            long[] inputData = new long[] { 10000000000L, 20000000000L, 30000000000L, 40000000000L };
            using var tensor = new Tensor(shape, ElementType.I64);
            tensor.set_data(inputData);

            // Act
            long[] outputData = tensor.get_long_data();

            // Assert
            Assert.Equal(inputData, outputData);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetByteData_ReturnsCorrectData()
        {
            // Arrange
            using var shape = new Shape(new long[] { 4 });
            using var tensor = new Tensor(shape, ElementType.F32);
            float[] floatData = new float[] { 1.0f, 2.0f, 3.0f, 4.0f };
            tensor.set_float_data(floatData);

            // Act
            byte[] byteData = tensor.get_byte_data();

            // Assert
            Assert.Equal(16, byteData.Length); // 4 floats * 4 bytes
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetUintData_ReturnsCorrectData()
        {
            // Arrange
            using var shape = new Shape(new long[] { 2, 2 });
            uint[] inputData = new uint[] { 1, 2, 3, 4 };
            using var tensor = new Tensor(shape, ElementType.U32);
            tensor.set_data(inputData);

            // Act
            uint[] outputData = tensor.get_uint_data();

            // Assert
            Assert.Equal(inputData, outputData);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void SetData_WithGenericArray_UpdatesData()
        {
            // Arrange
            using var shape = new Shape(new long[] { 2, 2 });
            float[] inputData = new float[] { 1.0f, 2.0f, 3.0f, 4.0f };
            using var tensor = new Tensor(shape, ElementType.F32);

            // Act
            tensor.set_data(inputData);
            float[] outputData = tensor.get_float_data();

            // Assert
            Assert.Equal(inputData, outputData);
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
            System.IntPtr ptr = tensor.data();

            // Assert
            Assert.NotEqual(System.IntPtr.Zero, ptr);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void FromShape_CreatesTensor()
        {
            // Arrange
            using var shape = new Shape(new long[] { 1, 3, 224, 224 });

            // Act
            using var tensor = Tensor.from_shape(shape, ElementType.F32);

            // Assert
            Assert.NotNull(tensor);
            Assert.Equal(ElementType.F32, tensor.element_type);
            Assert.Equal((ulong)(1 * 3 * 224 * 224), tensor.size);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void Size_ReturnsTotalElementCount()
        {
            // Arrange
            using var shape = new Shape(new long[] { 2, 3, 4 });
            using var tensor = new Tensor(shape, ElementType.F32);

            // Act
            ulong size = tensor.size;

            // Assert
            Assert.Equal((ulong)(2 * 3 * 4), size);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void ByteSize_ReturnsCorrectByteCount()
        {
            // Arrange
            using var shape = new Shape(new long[] { 2, 2 });
            using var tensor = new Tensor(shape, ElementType.F32);

            // Act
            ulong byteSize = tensor.byte_size;

            // Assert
            Assert.Equal((ulong)(4 * 4), byteSize); // 4 elements * 4 bytes each
        }
    }
}
