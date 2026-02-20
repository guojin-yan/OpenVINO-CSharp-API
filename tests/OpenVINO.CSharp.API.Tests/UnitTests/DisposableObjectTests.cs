// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.UnitTests
{
    /// <summary>
    /// DisposableObject 基类测试 / DisposableObject base class tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class DisposableObjectTests
    {
        static DisposableObjectTests()
        {
            TestInitialization.Initialize();
        }
        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void IsDisposed_AfterCreation_ReturnsFalse()
        {
            // Arrange
            using var shape = new Shape(new long[] { 1, 3, 224, 224 });

            // Assert
            Assert.False(shape.IsDisposed);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void IsDisposed_AfterDispose_ReturnsTrue()
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
        public void IsEnabledDispose_DefaultValue_IsTrue()
        {
            // Arrange
            using var shape = new Shape(new long[] { 1, 3, 224, 224 });

            // Assert
            Assert.True(shape.IsEnabledDispose);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void IsEnabledDispose_CanBeSetToFalse()
        {
            // Arrange
            using var shape = new Shape(new long[] { 1, 3, 224, 224 });

            // Act
            shape.IsEnabledDispose = false;

            // Assert
            Assert.False(shape.IsEnabledDispose);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void IsValid_AfterCreation_ReturnsTrue()
        {
            // Arrange
            using var shape = new Shape(new long[] { 1, 3, 224, 224 });

            // Assert
            Assert.True(shape.IsValid);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void IsValid_AfterDispose_ReturnsFalse()
        {
            // Arrange
            var shape = new Shape(new long[] { 1, 3, 224, 224 });

            // Act
            shape.Dispose();

            // Assert
            Assert.False(shape.IsValid);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void MultipleDispose_DoesNotThrow()
        {
            // Arrange
            var shape = new Shape(new long[] { 1, 3, 224, 224 });

            // Act & Assert - 多次 Dispose 不应抛出异常
            shape.Dispose();
            shape.Dispose();
            shape.Dispose();
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void OvPtr_AfterCreation_ReturnsValidPointer()
        {
            // Arrange
            using var shape = new Shape(new long[] { 1, 3, 224, 224 });

            // Act
            IntPtr ptr = shape.OvPtr;

            // Assert
            Assert.NotEqual(IntPtr.Zero, ptr);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void OvPtr_AfterDispose_ThrowsObjectDisposedException()
        {
            // Arrange
            var shape = new Shape(new long[] { 1, 3, 224, 224 });
            shape.Dispose();

            // Act & Assert
            Assert.Throws<ObjectDisposedException>(() => shape.OvPtr);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Release_DisposesObject()
        {
            // Arrange
            var shape = new Shape(new long[] { 1, 3, 224, 224 });

            // Act
            shape.Release();

            // Assert
            Assert.True(shape.IsDisposed);
        }
    }
}
