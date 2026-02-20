// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using OpenVinoSharp.Internal;
using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.UnitTests
{
    /// <summary>
    /// OvLogger 类单元测试 / OvLogger class unit tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class OvLoggerTests : IDisposable
    {
        static OvLoggerTests()
        {
            TestInitialization.Initialize();
        }

        private LogLevel _originalLevel;

        public OvLoggerTests()
        {
            // 保存原始日志级别
            _originalLevel = OvLogger.MinLevel;
        }

        public void Dispose()
        {
            // 恢复原始日志级别
            OvLogger.MinLevel = _originalLevel;
            OvLogger.ClearCallback();
        }

        [Theory]
        [InlineData(LogLevel.DEBUG, LogLevel.DEBUG, true)]
        [InlineData(LogLevel.DEBUG, LogLevel.INFO, true)]
        [InlineData(LogLevel.INFO, LogLevel.DEBUG, false)]
        [InlineData(LogLevel.ERROR, LogLevel.WARNING, false)]
        [Trait("Category", TestCategories.Unit)]
        public void IsEnabled_ReturnsCorrectValue(LogLevel minLevel, LogLevel testLevel, bool expected)
        {
            // Arrange
            OvLogger.MinLevel = minLevel;

            // Act
            bool actual = OvLogger.IsEnabled(testLevel);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void SetCallback_InvokesCallbackOnLog()
        {
            // Arrange
            LogLevel? receivedLevel = null;
            string? receivedMessage = null;
            
            OvLogger.MinLevel = LogLevel.DEBUG;
            OvLogger.SetCallback((level, msg) =>
            {
                receivedLevel = level;
                receivedMessage = msg;
            });

            // Act
            OvLogger.Info("Test message");

            // Assert
            Assert.Equal(LogLevel.INFO, receivedLevel);
            Assert.Equal("Test message", receivedMessage);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void ClearCallback_RemovesCallback()
        {
            // Arrange
            bool callbackInvoked = false;
            OvLogger.SetCallback((level, msg) => callbackInvoked = true);
            OvLogger.ClearCallback();

            // Act
            OvLogger.Info("Test message");

            // Assert
            Assert.False(callbackInvoked);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Debug_WithDisabledLevel_DoesNotInvokeCallback()
        {
            // Arrange
            bool callbackInvoked = false;
            OvLogger.MinLevel = LogLevel.INFO; // 禁用 DEBUG
            OvLogger.SetCallback((level, msg) => callbackInvoked = true);

            // Act
            OvLogger.Debug("Debug message");

            // Assert
            Assert.False(callbackInvoked);
        }

        [Theory]
        [InlineData(LogLevel.DEBUG)]
        [InlineData(LogLevel.INFO)]
        [InlineData(LogLevel.WARNING)]
        [InlineData(LogLevel.ERROR)]
        [InlineData(LogLevel.FATAL)]
        [Trait("Category", TestCategories.Unit)]
        public void Log_WithFormatString_WorksCorrectly(LogLevel level)
        {
            // Arrange
            string? receivedMessage = null;
            OvLogger.MinLevel = LogLevel.DEBUG;
            OvLogger.SetCallback((lvl, msg) => 
            {
                if (lvl == level) receivedMessage = msg;
            });

            // Act
            switch (level)
            {
                case LogLevel.DEBUG:
                    OvLogger.Debug("Value: {0}", 42);
                    break;
                case LogLevel.INFO:
                    OvLogger.Info("Value: {0}", 42);
                    break;
                case LogLevel.WARNING:
                    OvLogger.Warn("Value: {0}", 42);
                    break;
                case LogLevel.ERROR:
                    OvLogger.Error("Value: {0}", 42);
                    break;
                case LogLevel.FATAL:
                    OvLogger.Fatal("Value: {0}", 42);
                    break;
            }

            // Assert
            Assert.Equal("Value: 42", receivedMessage);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void IsDebugEnabled_WhenMinLevelIsDebug_ReturnsTrue()
        {
            // Arrange
            OvLogger.MinLevel = LogLevel.DEBUG;

            // Act & Assert
            Assert.True(OvLogger.IsDebugEnabled);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void IsInfoEnabled_WhenMinLevelIsWarning_ReturnsFalse()
        {
            // Arrange
            OvLogger.MinLevel = LogLevel.WARNING;

            // Act & Assert
            Assert.False(OvLogger.IsInfoEnabled);
        }
    }
}
