// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.UnitTests
{
    /// <summary>
    /// PartialShape 类单元测试 / PartialShape class unit tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class PartialShapeTests
    {
        static PartialShapeTests()
        {
            TestInitialization.Initialize();
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Constructor_Default_CreatesDynamicShape()
        {
            // Act
            using var shape = new PartialShape();

            // Assert
            Assert.NotNull(shape);
            Assert.True(shape.is_dynamic());
            Assert.True(shape.rank_is_dynamic());
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Constructor_WithDimensions_CreatesStaticShape()
        {
            // Arrange
            long[] dims = { 1, 3, 224, 224 };

            // Act
            using var shape = new PartialShape(dims);

            // Assert
            Assert.NotNull(shape);
            Assert.True(shape.is_static());
            Assert.Equal(dims.Length, shape.dims.Length);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void IsDynamic_WithStaticDimensions_ReturnsFalse()
        {
            // Arrange
            using var shape = new PartialShape(new long[] { 1, 3, 224, 224 });

            // Act
            bool isDynamic = shape.is_dynamic();

            // Assert
            Assert.False(isDynamic);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void IsStatic_WithStaticDimensions_ReturnsTrue()
        {
            // Arrange
            using var shape = new PartialShape(new long[] { 1, 3, 224, 224 });

            // Act
            bool isStatic = shape.is_static();

            // Assert
            Assert.True(isStatic);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void StaticShape_CreatesShapeWithCorrectDimensions()
        {
            // Arrange
            long[] dims = { 1, 3, 224, 224 };

            // Act
            using var shape = PartialShape.static_shape(dims);

            // Assert
            Assert.NotNull(shape);
            Assert.True(shape.is_static());
            Assert.Equal(dims.Length, shape.rank.get_length());
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void DynamicShape_CreatesFullyDynamicShape()
        {
            // Act
            using var shape = PartialShape.dynamic_shape();

            // Assert
            Assert.NotNull(shape);
            Assert.True(shape.is_dynamic());
            Assert.True(shape.rank_is_dynamic());
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Scalar_CreatesZeroDimensionalShape()
        {
            // Act
            using var shape = PartialShape.scalar();

            // Assert
            Assert.NotNull(shape);
            Assert.True(shape.is_static());
            Assert.Empty(shape.dims);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void ToString_WithStaticShape_ReturnsFormattedString()
        {
            // Arrange
            using var shape = new PartialShape(new long[] { 1, 3, 224, 224 });

            // Act
            string result = shape.ToString();

            // Assert
            Assert.NotNull(result);
            Assert.Contains("1", result);
            Assert.Contains("3", result);
            Assert.Contains("224", result);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void ToString_WithDynamicShape_ReturnsQuestionMark()
        {
            // Arrange
            using var shape = PartialShape.dynamic_shape();

            // Act
            string result = shape.ToString();

            // Assert
            Assert.Equal("?", result);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void ToShape_WithStaticShape_ReturnsShape()
        {
            // Arrange
            using var partialShape = new PartialShape(new long[] { 1, 3, 224, 224 });

            // Act
            using var shape = partialShape.to_shape();

            // Assert
            Assert.NotNull(shape);
            long[] dims = shape.get_dims();
            Assert.Equal(new long[] { 1, 3, 224, 224 }, dims);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void ToShape_WithDynamicShape_ThrowsInvalidOperationException()
        {
            // Arrange
            using var shape = PartialShape.dynamic_shape();

            // Act & Assert
            Assert.Throws<System.InvalidOperationException>(() => shape.to_shape());
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Dispose_CleansUpResources()
        {
            // Arrange
            var shape = new PartialShape(new long[] { 1, 3, 224, 224 });

            // Act
            shape.Dispose();

            // Assert
            Assert.True(shape.IsDisposed);
        }
    }
}
