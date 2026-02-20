// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.UnitTests
{
    /// <summary>
    /// Rank 结构单元测试 / Rank structure unit tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class RankTests
    {
        static RankTests()
        {
            TestInitialization.Initialize();
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Constructor_WithStaticValue_CreatesStaticRank()
        {
            // Arrange & Act
            var rank = new Rank(4);

            // Assert
            Assert.True(rank.is_static());
            Assert.False(rank.is_dynamic());
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Constructor_WithMinMax_CreatesBoundedRank()
        {
            // Arrange & Act
            var rank = new Rank(1, 4);

            // Assert
            Assert.True(rank.is_dynamic());
            Assert.False(rank.is_static());
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Dynamic_CreatesDynamicRank()
        {
            // Act
            var rank = Rank.dynamic();

            // Assert
            Assert.True(rank.is_dynamic());
            Assert.False(rank.is_static());
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void GetLength_WithStaticRank_ReturnsValue()
        {
            // Arrange
            var rank = new Rank(4);

            // Act
            long length = rank.get_length();

            // Assert
            Assert.Equal(4, length);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void GetLength_WithDynamicRank_ThrowsInvalidOperationException()
        {
            // Arrange
            var rank = Rank.dynamic();

            // Act & Assert
            Assert.Throws<System.InvalidOperationException>(() => rank.get_length());
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void ToString_WithStaticRank_ReturnsValue()
        {
            // Arrange
            var rank = new Rank(4);

            // Act
            string result = rank.ToString();

            // Assert
            Assert.Equal("4", result);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void ToString_WithDynamicRank_ReturnsQuestionMark()
        {
            // Arrange
            var rank = Rank.dynamic();

            // Act
            string result = rank.ToString();

            // Assert
            Assert.Equal("?", result);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void EqualityOperator_WithSameValues_ReturnsTrue()
        {
            // Arrange
            var rank1 = new Rank(4);
            var rank2 = new Rank(4);

            // Act & Assert
            Assert.True(rank1 == rank2);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void EqualityOperator_WithDifferentValues_ReturnsFalse()
        {
            // Arrange
            var rank1 = new Rank(4);
            var rank2 = new Rank(5);

            // Act & Assert
            Assert.False(rank1 == rank2);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void InequalityOperator_WithDifferentValues_ReturnsTrue()
        {
            // Arrange
            var rank1 = new Rank(4);
            var rank2 = new Rank(5);

            // Act & Assert
            Assert.True(rank1 != rank2);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Equals_WithSameValues_ReturnsTrue()
        {
            // Arrange
            var rank1 = new Rank(4);
            var rank2 = new Rank(4);

            // Act
            bool result = rank1.Equals(rank2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void GetHashCode_WithSameValues_ReturnsSameHash()
        {
            // Arrange
            var rank1 = new Rank(4);
            var rank2 = new Rank(4);

            // Act
            int hash1 = rank1.GetHashCode();
            int hash2 = rank2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }
    }
}
