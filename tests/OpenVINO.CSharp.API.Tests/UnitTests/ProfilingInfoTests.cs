// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.UnitTests
{
    /// <summary>
    /// ProfilingInfo 类单元测试 / ProfilingInfo class unit tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class ProfilingInfoTests
    {
        static ProfilingInfoTests()
        {
            TestInitialization.Initialize();
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Constructor_Default_SetsDefaultValues()
        {
            // Act
            var info = new ProfilingInfo();

            // Assert
            Assert.Equal(ProfilingInfo.Status.NOT_RUN, info.status);
            Assert.Equal(0, info.real_time);
            Assert.Equal(0, info.cpu_time);
            Assert.Equal(string.Empty, info.node_name);
            Assert.Equal(string.Empty, info.exec_type);
            Assert.Equal(string.Empty, info.node_type);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Status_CanBeSetAndGet()
        {
            // Arrange
            var info = new ProfilingInfo();

            // Act & Assert
            info.status = ProfilingInfo.Status.NOT_RUN;
            Assert.Equal(ProfilingInfo.Status.NOT_RUN, info.status);

            info.status = ProfilingInfo.Status.OPTIMIZED_OUT;
            Assert.Equal(ProfilingInfo.Status.OPTIMIZED_OUT, info.status);

            info.status = ProfilingInfo.Status.EXECUTED;
            Assert.Equal(ProfilingInfo.Status.EXECUTED, info.status);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void RealTime_CanBeSetAndGet()
        {
            // Arrange
            var info = new ProfilingInfo();

            // Act
            info.real_time = 1000;

            // Assert
            Assert.Equal(1000, info.real_time);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void CpuTime_CanBeSetAndGet()
        {
            // Arrange
            var info = new ProfilingInfo();

            // Act
            info.cpu_time = 500;

            // Assert
            Assert.Equal(500, info.cpu_time);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void NodeName_CanBeSetAndGet()
        {
            // Arrange
            var info = new ProfilingInfo();

            // Act
            info.node_name = "TestNode";

            // Assert
            Assert.Equal("TestNode", info.node_name);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void ExecType_CanBeSetAndGet()
        {
            // Arrange
            var info = new ProfilingInfo();

            // Act
            info.exec_type = "Convolution";

            // Assert
            Assert.Equal("Convolution", info.exec_type);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void NodeType_CanBeSetAndGet()
        {
            // Arrange
            var info = new ProfilingInfo();

            // Act
            info.node_type = "Op";

            // Assert
            Assert.Equal("Op", info.node_type);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void ToString_WithExecutedStatus_ReturnsFormattedString()
        {
            // Arrange
            var info = new ProfilingInfo
            {
                status = ProfilingInfo.Status.EXECUTED,
                real_time = 100,
                cpu_time = 80,
                node_name = "Conv1",
                exec_type = "Convolution",
                node_type = "Op"
            };

            // Act
            string result = info.ToString();

            // Assert
            Assert.Contains("Conv1", result);
            Assert.Contains("status=EXECUTED", result);
            Assert.Contains("real_time=100us", result);
            Assert.Contains("cpu_time=80us", result);
            Assert.Contains("exec_type=Convolution", result);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void ToString_WithNotRunStatus_ReturnsFormattedString()
        {
            // Arrange
            var info = new ProfilingInfo
            {
                status = ProfilingInfo.Status.NOT_RUN,
                real_time = 0,
                cpu_time = 0,
                node_name = "TestNode",
                exec_type = "None"
            };

            // Act
            string result = info.ToString();

            // Assert
            Assert.Contains("TestNode", result);
            Assert.Contains("status=NOT_RUN", result);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void ToString_WithOptimizedOutStatus_ReturnsFormattedString()
        {
            // Arrange
            var info = new ProfilingInfo
            {
                status = ProfilingInfo.Status.OPTIMIZED_OUT,
                node_name = "DeadNode"
            };

            // Act
            string result = info.ToString();

            // Assert
            Assert.Contains("DeadNode", result);
            Assert.Contains("status=OPTIMIZED_OUT", result);
        }
    }
}
