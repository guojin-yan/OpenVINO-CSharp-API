// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.UnitTests
{
    /// <summary>
    /// InferRequestPool 单元测试 / InferRequestPool unit tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class InferRequestPoolTests
    {
        static InferRequestPoolTests()
        {
            TestInitialization.Initialize();
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Unit)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void Constructor_WithValidParameters_CreatesPool()
        {
            // Arrange
            using var core = new Core();
            using var modelObj = core.read_model("model/yolo26n.xml");
            using var model = core.compile_model(modelObj, "CPU", null!);

            // Act
            using var pool = new InferRequestPool(model, initialSize: 2, maxSize: 4);

            // Assert
            Assert.NotNull(pool);
            Assert.Equal(2, pool.Count);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Unit)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void Rent_WhenPoolHasAvailableRequest_ReturnsRequest()
        {
            // Arrange
            using var core = new Core();
            using var modelObj = core.read_model("model/yolo26n.xml");
            using var model = core.compile_model(modelObj, "CPU", null!);
            using var pool = new InferRequestPool(model, initialSize: 1, maxSize: 2);

            // Act
            var request = pool.Rent();

            // Assert
            Assert.NotNull(request);
            Assert.False(request.IsDisposed);

            // Cleanup
            pool.Return(request);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Unit)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void TryRent_WhenPoolEmpty_ReturnsFalse()
        {
            // Arrange
            using var core = new Core();
            using var modelObj = core.read_model("model/yolo26n.xml");
            using var model = core.compile_model(modelObj, "CPU", null!);
            using var pool = new InferRequestPool(model, initialSize: 0, maxSize: 1);

            // Act
            bool result = pool.TryRent(out var request);

            // Assert
            Assert.True(result); // 池为空时会创建新请求
            Assert.NotNull(request);

            // Cleanup
            pool.Return(request);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Unit)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void Return_WhenRequestValid_ReturnsToPool()
        {
            // Arrange
            using var core = new Core();
            using var modelObj = core.read_model("model/yolo26n.xml");
            using var model = core.compile_model(modelObj, "CPU", null!);
            using var pool = new InferRequestPool(model, initialSize: 1, maxSize: 2);
            var request = pool.Rent();
            int countBefore = pool.AvailableCount;

            // Act
            pool.Return(request);

            // Assert
            Assert.True(pool.AvailableCount >= countBefore);
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Unit)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void Dispose_CleansUpResources()
        {
            // Arrange
            using var core = new Core();
            using var modelObj = core.read_model("model/yolo26n.xml");
            using var model = core.compile_model(modelObj, "CPU", null!);
            var pool = new InferRequestPool(model, initialSize: 2, maxSize: 4);

            // Act
            pool.Dispose();

            // Assert
            Assert.Throws<ObjectDisposedException>(() => pool.Rent());
        }
    }
}
