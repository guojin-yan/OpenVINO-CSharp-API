// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.UnitTests
{
    /// <summary>
    /// 异常处理测试 / Exception handling tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class ExceptionTests
    {
        static ExceptionTests()
        {
            TestInitialization.Initialize();
        }
        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void OVException_Constructor_SetsProperties()
        {
            // Arrange
            var status = ExceptionStatus.GENERAL_ERROR;
            string message = "Test error message";

            // Act
            var exception = new OVException(status, message);

            // Assert
            Assert.Equal(status, exception.Status);
            Assert.Equal(message, exception.Message);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Shape_Constructor_WithNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Shape(null!));
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Tensor_Constructor_WithNullShape_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                new Tensor(null!, ElementType.F32));
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Tensor_Constructor_WithNullData_ThrowsArgumentNullException()
        {
            // Arrange
            using var shape = new Shape(new long[] { 1, 3, 224, 224 });

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                new Tensor(shape, (float[])null!));
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Shape_GetDim_WithInvalidIndex_ThrowsException()
        {
            // Arrange
            using var shape = new Shape(new long[] { 1, 3, 224 });

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => shape.get_dim(10));
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void DisposableObject_ThrowIfDisposed_AfterDispose_ThrowsObjectDisposedException()
        {
            // Arrange
            var shape = new Shape(new long[] { 1, 3 });
            shape.Dispose();

            // Act & Assert
            Assert.Throws<ObjectDisposedException>(() => shape.ThrowIfDisposed());
        }
    }
}
