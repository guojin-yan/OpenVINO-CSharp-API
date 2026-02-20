// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.IntegrationTests
{
    /// <summary>
    /// Tensor 类集成测试 / Tensor class integration tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class TensorIntegrationTests
    {
        static TensorIntegrationTests()
        {
            // 确保 OpenVINO 原生库已加载
            // Ensure OpenVINO native library is loaded
            TestInitialization.Initialize();
        }
        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void Constructor_WithShapeAndType_CreatesTensor()
        {
            // Arrange
            using var shape = new Shape(new long[] { 1, 3, 224, 224 });

            // Act
            using var tensor = new Tensor(shape, ElementType.F32);

            // Assert
            Assert.NotNull(tensor);
            Assert.Equal(ElementType.F32, tensor.element_type);
            Assert.Equal((ulong)150528, tensor.size); // 1*3*224*224
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void Constructor_WithFloatArray_CreatesTensor()
        {
            // Arrange
            using var shape = new Shape(new long[] { 1, 3, 4, 4 });
            float[] data = TestDataGenerator.GenerateRandomFloatArray(48);

            // Act
            using var tensor = new Tensor(shape, data);

            // Assert
            Assert.NotNull(tensor);
            Assert.Equal(ElementType.F32, tensor.element_type);
            Assert.Equal((ulong)48, tensor.size);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetFloatData_ReturnsCorrectData()
        {
            // Arrange
            using var shape = new Shape(new long[] { 1, 3, 2, 2 });
            float[] inputData = TestDataGenerator.GenerateSequentialFloatArray(12);
            using var tensor = new Tensor(shape, inputData);

            // Act
            float[] outputData = tensor.get_float_data();

            // Assert
            Assert.Equal(inputData, outputData);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void SetFloatData_UpdatesTensorData()
        {
            // Arrange
            using var shape = new Shape(new long[] { 2, 2 });
            float[] initialData = new float[] { 1.0f, 2.0f, 3.0f, 4.0f };
            using var tensor = new Tensor(shape, initialData);

            float[] newData = new float[] { 10.0f, 20.0f, 30.0f, 40.0f };

            // Act
            tensor.set_float_data(newData);
            float[] retrievedData = tensor.get_float_data();

            // Assert
            Assert.Equal(newData, retrievedData);
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
        public void ByteSize_ReturnsCorrectSize()
        {
            // Arrange
            using var shape = new Shape(new long[] { 1, 3, 224, 224 });
            using var tensor = new Tensor(shape, ElementType.F32);

            // Act
            ulong byteSize = tensor.byte_size;

            // Assert
            // F32 = 4 bytes per element, total = 1*3*224*224*4
            Assert.Equal((ulong)(150528 * 4), byteSize);
        }
    }
}
