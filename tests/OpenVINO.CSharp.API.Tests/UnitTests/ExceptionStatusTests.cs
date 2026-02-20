// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.UnitTests
{
    /// <summary>
    /// ExceptionStatus 枚举测试 / ExceptionStatus enumeration tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class ExceptionStatusTests
    {
        static ExceptionStatusTests()
        {
            TestInitialization.Initialize();
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void OK_HasValueZero()
        {
            // Assert
            Assert.Equal(0, (int)ExceptionStatus.OK);
        }

        [Theory]
        [InlineData(ExceptionStatus.GENERAL_ERROR, -1)]
        [InlineData(ExceptionStatus.NOT_IMPLEMENTED, -2)]
        [InlineData(ExceptionStatus.NETWORK_NOT_LOADED, -3)]
        [InlineData(ExceptionStatus.PARAMETER_MISMATCH, -4)]
        [InlineData(ExceptionStatus.NOT_FOUND, -5)]
        [InlineData(ExceptionStatus.OUT_OF_BOUNDS, -6)]
        [InlineData(ExceptionStatus.UNEXPECTED, -7)]
        [InlineData(ExceptionStatus.REQUEST_BUSY, -8)]
        [InlineData(ExceptionStatus.RESULT_NOT_READY, -9)]
        [InlineData(ExceptionStatus.NOT_ALLOCATED, -10)]
        [InlineData(ExceptionStatus.INFER_NOT_STARTED, -11)]
        [InlineData(ExceptionStatus.NETWORK_NOT_READ, -12)]
        [InlineData(ExceptionStatus.INFER_CANCELLED, -13)]
        [InlineData(ExceptionStatus.INVALID_C_PARAM, -14)]
        [InlineData(ExceptionStatus.UNKNOWN_C_ERROR, -15)]
        [InlineData(ExceptionStatus.NOT_IMPLEMENT_C_METHOD, -16)]
        [InlineData(ExceptionStatus.UNKNOW_EXCEPTION, -17)]
        [InlineData(ExceptionStatus.PTR_NULL, -100)]
        [Trait("Category", TestCategories.Unit)]
        public void ExceptionStatus_HasCorrectValue(ExceptionStatus status, int expectedValue)
        {
            // Assert
            Assert.Equal(expectedValue, (int)status);
        }
    }
}
