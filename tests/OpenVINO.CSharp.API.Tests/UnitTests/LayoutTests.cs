// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.UnitTests
{
    /// <summary>
    /// Layout 类单元测试 / Layout class unit tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class LayoutTests
    {
        static LayoutTests()
        {
            TestInitialization.Initialize();
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Constructor_WithNCHW_CreatesLayout()
        {
            // Act
            using var layout = new Layout("NCHW");

            // Assert
            Assert.NotNull(layout);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Constructor_WithString_CreatesLayout()
        {
            // Arrange
            string layoutStr = "NCHW";

            // Act
            using var layout = new Layout(layoutStr);

            // Assert
            Assert.NotNull(layout);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void NCHW_ReturnsStandardLayout()
        {
            // Act
            using var layout = Layout.NCHW;

            // Assert
            Assert.NotNull(layout);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void NHWC_ReturnsStandardLayout()
        {
            // Act
            using var layout = Layout.NHWC;

            // Assert
            Assert.NotNull(layout);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void C_ReturnsChannelLayout()
        {
            // Act
            using var layout = Layout.C;

            // Assert
            Assert.NotNull(layout);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void NC_ReturnsBatchChannelLayout()
        {
            // Act
            using var layout = Layout.NC;

            // Assert
            Assert.NotNull(layout);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void CHW_ReturnsChannelHeightWidthLayout()
        {
            // Act
            using var layout = Layout.CHW;

            // Assert
            Assert.NotNull(layout);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void HWC_ReturnsHeightWidthChannelLayout()
        {
            // Act
            using var layout = Layout.HWC;

            // Assert
            Assert.NotNull(layout);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void HW_ReturnsHeightWidthLayout()
        {
            // Act
            using var layout = Layout.HW;

            // Assert
            Assert.NotNull(layout);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void CN_ReturnsChannelBatchLayout()
        {
            // Act
            using var layout = Layout.CN;

            // Assert
            Assert.NotNull(layout);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void WH_ReturnsWidthHeightLayout()
        {
            // Act
            using var layout = Layout.WH;

            // Assert
            Assert.NotNull(layout);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Dispose_CleansUpResources()
        {
            // Arrange
            var layout = new Layout("NCHW");

            // Act
            layout.Dispose();

            // Assert
            Assert.True(layout.IsDisposed);
        }
    }
}
