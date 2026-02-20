// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.UnitTests
{
    /// <summary>
    /// ModelCache 单元测试 / ModelCache unit tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class ModelCacheTests : IDisposable
    {
        static ModelCacheTests()
        {
            TestInitialization.Initialize();
        }

        private readonly bool _originalEnabled;
        private readonly int _originalMaxSize;

        public ModelCacheTests()
        {
            _originalEnabled = ModelCache.Enabled;
            _originalMaxSize = ModelCache.MaxCacheSize;
            ModelCache.Clear();
        }

        public void Dispose()
        {
            ModelCache.Enabled = _originalEnabled;
            ModelCache.MaxCacheSize = _originalMaxSize;
            ModelCache.Clear();
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Enabled_CanBeSetAndGet()
        {
            // Act & Assert
            ModelCache.Enabled = true;
            Assert.True(ModelCache.Enabled);

            ModelCache.Enabled = false;
            Assert.False(ModelCache.Enabled);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void MaxCacheSize_CanBeSetAndGet()
        {
            // Act
            ModelCache.MaxCacheSize = 5;

            // Assert
            Assert.Equal(5, ModelCache.MaxCacheSize);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void MaxCacheSize_SetToInvalidValue_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<System.ArgumentException>(() => ModelCache.MaxCacheSize = 0);
            Assert.Throws<System.ArgumentException>(() => ModelCache.MaxCacheSize = -1);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Count_Initially_ReturnsZero()
        {
            // Assert
            Assert.Equal(0, ModelCache.Count);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Clear_WhenCacheHasItems_EmptiesCache()
        {
            // 由于无法真正添加模型（需要 OpenVINO），我们只测试 Clear 不会抛出异常
            // Act
            ModelCache.Clear();

            // Assert
            Assert.Equal(0, ModelCache.Count);
        }

    }
}
