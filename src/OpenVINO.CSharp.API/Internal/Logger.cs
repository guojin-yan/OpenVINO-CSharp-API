// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static OpenVinoSharp.native.NativeMethods;

namespace OpenVinoSharp.Internal
{
    /// <summary>
    /// 日志级别 / Log level
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 调试信息 / Debug information
        /// </summary>
        DEBUG = 0,
        
        /// <summary>
        /// 普通信息 / General information
        /// </summary>
        INFO = 1,
        
        /// <summary>
        /// 警告信息 / Warning information
        /// </summary>
        WARNING = 2,
        
        /// <summary>
        /// 错误信息 / Error information
        /// </summary>
        ERROR = 3,
        
        /// <summary>
        /// 严重错误 / Fatal error
        /// </summary>
        FATAL = 4,
        
        /// <summary>
        /// 无日志 / No logging
        /// </summary>
        NONE = 5
    }

    /// <summary>
    /// 日志回调委托 / Log callback delegate
    /// </summary>
    /// <param name="level">日志级别 / Log level</param>
    /// <param name="message">日志消息 / Log message</param>
    public delegate void LogCallback(LogLevel level, string message);

    /// <summary>
    /// OpenVINO 日志类 / OpenVINO Logger class
    /// <para>提供高性能、线程安全的日志记录功能，支持控制台输出和用户自定义回调。/ Provides high-performance, thread-safe logging with console output and custom callbacks.</para>
    /// <para>性能优化：在禁用低级别日志时，字符串格式化不会执行。/ Performance optimized: string formatting is skipped when low-level logs are disabled.</para>
    /// </summary>
    public static class Logger
    {
        private static readonly object _lock = new object();
        private static LogLevel _minLevel = LogLevel.INFO;
        private static LogCallback _customCallback;
        private static bool _useNativeCallback = false;
        
        // 原生回调委托实例（防止GC回收）/ Native callback delegate instance (prevent GC)
        private static LogCallbackDelegate _nativeCallback;

        /// <summary>
        /// 获取或设置最小日志级别 / Get or set the minimum log level
        /// </summary>
        public static LogLevel MinLevel
        {
            get { return _minLevel; }
            set { _minLevel = value; }
        }

        /// <summary>
        /// 是否启用时间戳 / Whether to enable timestamps
        /// </summary>
        public static bool EnableTimestamp { get; set; } = true;

        /// <summary>
        /// 是否启用日志级别前缀 / Whether to enable log level prefix
        /// </summary>
        public static bool EnableLevelPrefix { get; set; } = true;

        /// <summary>
        /// 检查指定日志级别是否已启用 / Check if the specified log level is enabled
        /// </summary>
        /// <param name="level">要检查的日志级别 / The log level to check</param>
        /// <returns>如果级别已启用返回 true / Returns true if the level is enabled</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEnabled(LogLevel level)
        {
            return level >= _minLevel && level != LogLevel.NONE;
        }

        /// <summary>
        /// 检查 DEBUG 级别是否启用 / Check if DEBUG level is enabled
        /// </summary>
        public static bool IsDebugEnabled => IsEnabled(LogLevel.DEBUG);

        /// <summary>
        /// 检查 INFO 级别是否启用 / Check if INFO level is enabled
        /// </summary>
        public static bool IsInfoEnabled => IsEnabled(LogLevel.INFO);

        /// <summary>
        /// 设置自定义日志回调 / Set custom log callback
        /// <para>设置后，日志将同时输出到控制台和回调函数。/ After setting, logs will be output to both console and callback.</para>
        /// </summary>
        /// <param name="callback">回调函数 / Callback function</param>
        public static void SetCallback(LogCallback callback)
        {
            lock (_lock)
            {
                _customCallback = callback;
            }
        }

        /// <summary>
        /// 清除自定义日志回调 / Clear custom log callback
        /// </summary>
        public static void ClearCallback()
        {
            lock (_lock)
            {
                _customCallback = null;
            }
        }

        /// <summary>
        /// 启用原生日志回调（与C API集成）/ Enable native log callback (integrate with C API)
        /// </summary>
        public static void EnableNativeCallback()
        {
            lock (_lock)
            {
                if (!_useNativeCallback)
                {
                    _nativeCallback = new LogCallbackDelegate(NativeLogHandler);
                    try
                    {
                        ov_util_set_log_callback(_nativeCallback);
                        _useNativeCallback = true;
                    }
                    catch (Exception ex)
                    {
                        InternalWarn("Failed to set native log callback: " + ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 重置原生日志回调 / Reset native log callback
        /// </summary>
        public static void ResetNativeCallback()
        {
            lock (_lock)
            {
                if (_useNativeCallback)
                {
                    try
                    {
                        ov_util_reset_log_callback();
                    }
                    catch { }
                    _nativeCallback = null;
                    _useNativeCallback = false;
                }
            }
        }

        /// <summary>
        /// 原生日志处理函数 / Native log handler
        /// </summary>
        private static void NativeLogHandler(string message)
        {
            if (string.IsNullOrEmpty(message)) return;
            // 原生日志默认为 INFO 级别 / Native logs default to INFO level
            Log(LogLevel.INFO, "[Native] " + message);
        }

        /// <summary>
        /// 输出调试日志 / Output debug log
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Debug(string message)
        {
            if (!IsEnabled(LogLevel.DEBUG)) return;
            Log(LogLevel.DEBUG, message);
        }

        /// <summary>
        /// 输出调试日志（格式化）/ Output debug log (formatted)
        /// <para>性能提示：如果 DEBUG 级别被禁用，格式化不会执行。/ Performance note: formatting is skipped if DEBUG level is disabled.</para>
        /// </summary>
        public static void Debug(string format, params object[] args)
        {
            if (!IsEnabled(LogLevel.DEBUG)) return;
            Log(LogLevel.DEBUG, string.Format(format, args));
        }

        /// <summary>
        /// 输出信息日志 / Output info log
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Info(string message)
        {
            if (!IsEnabled(LogLevel.INFO)) return;
            Log(LogLevel.INFO, message);
        }

        /// <summary>
        /// 输出信息日志（格式化）/ Output info log (formatted)
        /// <para>性能提示：如果 INFO 级别被禁用，格式化不会执行。/ Performance note: formatting is skipped if INFO level is disabled.</para>
        /// </summary>
        public static void Info(string format, params object[] args)
        {
            if (!IsEnabled(LogLevel.INFO)) return;
            Log(LogLevel.INFO, string.Format(format, args));
        }

        /// <summary>
        /// 输出警告日志 / Output warning log
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Warn(string message)
        {
            if (!IsEnabled(LogLevel.WARNING)) return;
            Log(LogLevel.WARNING, message);
        }

        /// <summary>
        /// 输出警告日志（格式化）/ Output warning log (formatted)
        /// <para>性能提示：如果 WARNING 级别被禁用，格式化不会执行。/ Performance note: formatting is skipped if WARNING level is disabled.</para>
        /// </summary>
        public static void Warn(string format, params object[] args)
        {
            if (!IsEnabled(LogLevel.WARNING)) return;
            Log(LogLevel.WARNING, string.Format(format, args));
        }

        /// <summary>
        /// 输出错误日志 / Output error log
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Error(string message)
        {
            if (!IsEnabled(LogLevel.ERROR)) return;
            Log(LogLevel.ERROR, message);
        }

        /// <输出错误日志（格式化）/ Output error log (formatted)
        /// <para>性能提示：如果 ERROR 级别被禁用，格式化不会执行。/ Performance note: formatting is skipped if ERROR level is disabled.</para>
        /// </summary>
        public static void Error(string format, params object[] args)
        {
            if (!IsEnabled(LogLevel.ERROR)) return;
            Log(LogLevel.ERROR, string.Format(format, args));
        }

        /// <summary>
        /// 输出严重错误日志 / Output fatal log
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Fatal(string message)
        {
            if (!IsEnabled(LogLevel.FATAL)) return;
            Log(LogLevel.FATAL, message);
        }

        /// <summary>
        /// 输出严重错误日志（格式化）/ Output fatal log (formatted)
        /// <para>性能提示：如果 FATAL 级别被禁用，格式化不会执行。/ Performance note: formatting is skipped if FATAL level is disabled.</para>
        /// </summary>
        public static void Fatal(string format, params object[] args)
        {
            if (!IsEnabled(LogLevel.FATAL)) return;
            Log(LogLevel.FATAL, string.Format(format, args));
        }

        /// <summary>
        /// 核心日志方法 / Core logging method
        /// </summary>
        public static void Log(LogLevel level, string message)
        {
            if (level < _minLevel || level == LogLevel.NONE)
                return;

            string formattedMessage = FormatMessage(level, message);
            
            lock (_lock)
            {
                // 输出到控制台 / Output to console
                ConsoleWrite(level, formattedMessage);
                
                // 调用自定义回调 / Call custom callback
                _customCallback?.Invoke(level, message);
            }
        }

        /// <summary>
        /// 格式化日志消息 / Format log message
        /// </summary>
        private static string FormatMessage(LogLevel level, string message)
        {
            var sb = new System.Text.StringBuilder();
            
            if (EnableTimestamp)
            {
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] ");
            }
            
            if (EnableLevelPrefix)
            {
                sb.Append($"[{GetLevelString(level)}] ");
            }
            
            sb.Append(message);
            return sb.ToString();
        }

        /// <summary>
        /// 获取日志级别字符串 / Get log level string
        /// </summary>
        private static string GetLevelString(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.DEBUG: return "DEBUG";
                case LogLevel.INFO: return "INFO";
                case LogLevel.WARNING: return "WARN";
                case LogLevel.ERROR: return "ERROR";
                case LogLevel.FATAL: return "FATAL";
                default: return "UNKNOWN";
            }
        }

        /// <summary>
        /// 控制台输出（带颜色）/ Console output (with color)
        /// </summary>
        private static void ConsoleWrite(LogLevel level, string message)
        {
            var originalColor = Console.ForegroundColor;
            
            try
            {
                switch (level)
                {
                    case LogLevel.DEBUG:
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case LogLevel.INFO:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case LogLevel.WARNING:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case LogLevel.ERROR:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case LogLevel.FATAL:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        break;
                }
                
                Console.WriteLine(message);
            }
            finally
            {
                Console.ForegroundColor = originalColor;
            }
        }

        /// <summary>
        /// 内部警告（不经过普通日志系统，避免循环）/ Internal warning (bypass normal logging to avoid recursion)
        /// </summary>
        private static void InternalWarn(string message)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[Logger Warning] {message}");
            Console.ForegroundColor = originalColor;
        }
    }
}
