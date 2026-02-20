// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using OpenVinoSharp.Internal;
using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.UnitTests
{
    /// <summary>
    /// Logger 类单元测试 / Logger class unit tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class LoggerTests : IDisposable
    {
        static LoggerTests()
        {
            TestInitialization.Initialize();
        }

        private LogLevel _originalLevel;

        public LoggerTests()
        {
            // 保存原始日志级别
            _originalLevel = Logger.MinLevel;
        }

        public void Dispose()
        {
            // 恢复原始日志级别
            Logger.MinLevel = _originalLevel;
            Logger.ClearCallback();
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
            Logger.MinLevel = minLevel;

            // Act
            bool actual = Logger.IsEnabled(testLevel);

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
            
            Logger.MinLevel = LogLevel.DEBUG;
            Logger.SetCallback((level, msg) =>
            {
                receivedLevel = level;
                receivedMessage = msg;
            });

            // Act
            Logger.Info("Test message");

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
            Logger.SetCallback((level, msg) => callbackInvoked = true);
            Logger.ClearCallback();

            // Act
            Logger.Info("Test message");

            // Assert
            Assert.False(callbackInvoked);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Debug_WithDisabledLevel_DoesNotInvokeCallback()
        {
            // Arrange
            bool callbackInvoked = false;
            Logger.MinLevel = LogLevel.INFO; // 禁用 DEBUG
            Logger.SetCallback((level, msg) => callbackInvoked = true);

            // Act
            Logger.Debug("Debug message");

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
            Logger.MinLevel = LogLevel.DEBUG;
            Logger.SetCallback((lvl, msg) => 
            {
                if (lvl == level) receivedMessage = msg;
            });

            // Act
            switch (level)
            {
                case LogLevel.DEBUG:
                    Logger.Debug("Value: {0}", 42);
                    break;
                case LogLevel.INFO:
                    Logger.Info("Value: {0}", 42);
                    break;
                case LogLevel.WARNING:
                    Logger.Warn("Value: {0}", 42);
                    break;
                case LogLevel.ERROR:
                    Logger.Error("Value: {0}", 42);
                    break;
                case LogLevel.FATAL:
                    Logger.Fatal("Value: {0}", 42);
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
            Logger.MinLevel = LogLevel.DEBUG;

            // Act & Assert
            Assert.True(Logger.IsDebugEnabled);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void IsInfoEnabled_WhenMinLevelIsWarning_ReturnsFalse()
        {
            // Arrange
            Logger.MinLevel = LogLevel.WARNING;

            // Act & Assert
            Assert.False(Logger.IsInfoEnabled);
        }
    }
}
