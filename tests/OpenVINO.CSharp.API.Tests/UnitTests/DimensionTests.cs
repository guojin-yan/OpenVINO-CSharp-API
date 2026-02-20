// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.UnitTests
{
    /// <summary>
    /// Dimension 类单元测试 / Dimension class unit tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class DimensionTests
    {
        static DimensionTests()
        {
            TestInitialization.Initialize();
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Constructor_WithStaticValue_CreatesStaticDimension()
        {
            // Arrange & Act
            var dim = new Dimension(224);

            // Assert
            Assert.True(dim.is_static());
            Assert.False(dim.is_dynamic());
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Constructor_WithMinMax_CreatesBoundedDimension()
        {
            // Arrange & Act
            var dim = new Dimension(1, 100);

            // Assert
            Assert.True(dim.is_dynamic());
            Assert.False(dim.is_static());
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Dynamic_CreatesDynamicDimension()
        {
            // Act
            var dim = Dimension.dynamic();

            // Assert
            Assert.True(dim.is_dynamic());
            Assert.False(dim.is_static());
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Bounded_CreatesBoundedDimension()
        {
            // Act
            var dim = Dimension.bounded(1, 100);

            // Assert
            Assert.True(dim.is_dynamic());
            Assert.Equal(1, dim.min);
            Assert.Equal(100, dim.max);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void GetLength_WithStaticDimension_ReturnsValue()
        {
            // Arrange
            var dim = new Dimension(512);

            // Act
            long length = dim.get_length();

            // Assert
            Assert.Equal(512, length);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void GetLength_WithDynamicDimension_ThrowsInvalidOperationException()
        {
            // Arrange
            var dim = Dimension.dynamic();

            // Act & Assert
            Assert.Throws<System.InvalidOperationException>(() => dim.get_length());
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void ToString_WithStaticDimension_ReturnsValue()
        {
            // Arrange
            var dim = new Dimension(224);

            // Act
            string result = dim.ToString();

            // Assert
            Assert.Equal("224", result);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void ToString_WithDynamicDimension_ReturnsQuestionMark()
        {
            // Arrange
            var dim = Dimension.dynamic();

            // Act
            string result = dim.ToString();

            // Assert
            Assert.Equal("?", result);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void ToString_WithBoundedDimension_ReturnsRange()
        {
            // Arrange
            var dim = Dimension.bounded(1, 100);

            // Act
            string result = dim.ToString();

            // Assert
            Assert.Equal("1..100", result);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void EqualityOperator_WithSameValues_ReturnsTrue()
        {
            // Arrange
            var dim1 = new Dimension(224);
            var dim2 = new Dimension(224);

            // Act & Assert
            Assert.True(dim1 == dim2);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void EqualityOperator_WithDifferentValues_ReturnsFalse()
        {
            // Arrange
            var dim1 = new Dimension(224);
            var dim2 = new Dimension(225);

            // Act & Assert
            Assert.False(dim1 == dim2);
        }
    }
}
