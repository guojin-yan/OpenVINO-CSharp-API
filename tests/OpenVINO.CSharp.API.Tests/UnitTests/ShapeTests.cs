// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.UnitTests
{
    /// <summary>
    /// Shape 类单元测试 / Shape class unit tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class ShapeTests
    {
        static ShapeTests()
        {
            TestInitialization.Initialize();
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Constructor_WithValidDimensions_CreatesShape()
        {
            // Arrange
            long[] dims = { 1, 3, 224, 224 };

            // Act
            using var shape = new Shape(dims);

            // Assert
            Assert.NotNull(shape);
            Assert.Equal(4L, shape.get_rank());
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void GetDims_ReturnsCorrectDimensions()
        {
            // Arrange
            long[] expectedDims = { 1, 3, 224, 224 };
            using var shape = new Shape(expectedDims);

            // Act
            long[] actualDims = shape.get_dims();

            // Assert
            Assert.Equal(expectedDims, actualDims);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 3)]
        [InlineData(2, 224)]
        [InlineData(3, 224)]
        [Trait("Category", TestCategories.Unit)]
        public void GetDim_ReturnsCorrectValue(int index, long expectedValue)
        {
            // Arrange
            using var shape = new Shape(new long[] { 1, 3, 224, 224 });

            // Act
            long actualValue = shape.get_dim(index);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(new long[] { 1, 3, 224, 224 }, 150528)]
        [InlineData(new long[] { 1, 512 }, 512)]
        [InlineData(new long[] { 2, 3, 4 }, 24)]
        [Trait("Category", TestCategories.Unit)]
        public void GetTotalElements_ReturnsCorrectCount(long[] dims, long expectedCount)
        {
            // Arrange
            using var shape = new Shape(dims);

            // Act
            long actualCount = shape.get_total_elements();

            // Assert
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Scalar_CreatesZeroDimensionalShape()
        {
            // Act
            using var shape = Shape.scalar();

            // Assert
            Assert.Equal(1L, shape.get_rank());
            Assert.Equal(-1, shape.get_total_elements());
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void NCHW_CreatesCorrectShape()
        {
            // Act
            using var shape = Shape.nchw(1, 3, 224, 224);

            // Assert
            long[] dims = shape.get_dims();
            Assert.Equal(new long[] { 1, 3, 224, 224 }, dims);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void NHWC_CreatesCorrectShape()
        {
            // Act
            using var shape = Shape.nhwc(1, 224, 224, 3);

            // Assert
            long[] dims = shape.get_dims();
            Assert.Equal(new long[] { 1, 224, 224, 3 }, dims);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Dispose_CleansUpResources()
        {
            // Arrange
            var shape = new Shape(new long[] { 1, 3, 224, 224 });

            // Act
            shape.Dispose();

            // Assert
            Assert.True(shape.IsDisposed);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void ToString_ReturnsFormattedString()
        {
            // Arrange
            using var shape = new Shape(new long[] { 1, 3, 224, 224 });

            // Act
            string result = shape.ToString();

            // Assert
            Assert.Contains("Shape", result);
            Assert.Contains("1", result);
            Assert.Contains("224", result);
        }
    }
}
