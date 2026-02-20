// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.UnitTests
{
    /// <summary>
    /// ElementType 枚举测试 / ElementType enumeration tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class ElementTypeTests
    {
        static ElementTypeTests()
        {
            TestInitialization.Initialize();
        }

        [Theory]
        [InlineData(ElementType.UNDEFINED, 0U)]
        [InlineData(ElementType.BOOLEAN, 1U)]
        [InlineData(ElementType.BF16, 2U)]
        [InlineData(ElementType.F16, 3U)]
        [InlineData(ElementType.F32, 4U)]
        [InlineData(ElementType.F64, 5U)]
        [InlineData(ElementType.I4, 6U)]
        [InlineData(ElementType.I8, 7U)]
        [InlineData(ElementType.I16, 8U)]
        [InlineData(ElementType.I32, 9U)]
        [InlineData(ElementType.I64, 10U)]
        [InlineData(ElementType.U1, 11U)]
        [Trait("Category", TestCategories.Unit)]
        public void ElementType_HasCorrectValue(ElementType type, uint expectedValue)
        {
            // Assert
            Assert.Equal(expectedValue, (uint)type);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void DYNAMIC_Equals_UNDEFINED()
        {
            // Assert
            Assert.Equal(ElementType.UNDEFINED, ElementType.DYNAMIC);
        }

        [Theory]
        [InlineData(ElementType.F32, sizeof(float))]
        [InlineData(ElementType.F64, sizeof(double))]
        [InlineData(ElementType.I32, sizeof(int))]
        [InlineData(ElementType.I64, sizeof(long))]
        [Trait("Category", TestCategories.Unit)]
        public void ElementType_SizeOf_ReturnsCorrectSize(ElementType type, int expectedSize)
        {
            // 验证常见类型的预期大小
            // 注意：这只是验证我们的理解，不是测试 OpenVINO 本身
            int actualSize = GetElementTypeSize(type);
            Assert.Equal(expectedSize, actualSize);
        }

        private static int GetElementTypeSize(ElementType type)
        {
            return type switch
            {
                ElementType.F32 => 4,
                ElementType.F64 => 8,
                ElementType.I32 => 4,
                ElementType.I64 => 8,
                ElementType.I16 => 2,
                ElementType.I8 => 1,
                ElementType.U8 => 1,
                _ => 0
            };
        }
    }
}
