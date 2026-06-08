//  ========================================================================
//  【项目名称】OpenVINO C# API
//  【项目描述】OpenVINO™ 的 C# 语言绑定库，提供高性能深度学习推理能力
//  【版权声明】© 2026-2025 Guojin Yan. All Rights Reserved.
//  【开源协议】Apache-2.0 License（请遵守许可证条款）
//  -----------------------------------------------------------------------
//  【功能简介】
//  1. 完整的 OpenVINO™ C API 封装，提供 C# 友好的面向对象接口。
//  2. 支持模型加载、编译、推理全流程操作。
//  3. 支持 CPU、GPU、VPU 等多种推理设备。
//  4. 支持同步推理和异步推理模式。
//  5. 支持预处理和后处理流水线配置。
//  6. 支持动态形状和批量推理。
//  7. 支持模型缓存和性能分析。
//  8. 支持远程上下文（Remote Context）和零拷贝推理。
//  9. 支持 .NET Framework 4.6.1+、.NET Core 2.0+、.NET 5/6/7/8/9+。
//  10. 提供推理请求对象池，优化高并发场景性能。
//  11. 提供完善的异常处理和日志记录机制。
//  12. 提供丰富的单元测试和集成测试用例。
//  -----------------------------------------------------------------------
//  【官方资源】
//  📌 GitHub仓库：https://github.com/guojin-yan/OpenVINO-CSharp-API
//  📌 NuGet包：https://www.nuget.org/packages/OpenVINO.CSharp.API
//  📌 在线文档：https://guojin-yan.github.io/OpenVINO-CSharp-API/index.html
//  📌 示例代码：https://github.com/guojin-yan/OpenVINO-CSharp-API/tree/csharp3.3/samples
//  -----------------------------------------------------------------------
//  【社区支持】
//  💬 QQ交流群：945057948（加入获取技术支持）
//  📱 微信公众号：CSharp与边缘模型部署（教程+案例）
//  📝 CSDN博客：https://guojin.blog.csdn.net（技术文章）
//  -----------------------------------------------------------------------
//  【联系我们】
//  ✉ 项目维护：guojin_yjs@cumt.edu.cn
//  💬 微信咨询：15253793309
//  🐛 Bug反馈：https://github.com/guojin-yan/OpenVINO-CSharp-API/issues
//  💡 功能建议：https://github.com/guojin-yan/OpenVINO-CSharp-API/discussions/landing
//  -----------------------------------------------------------------------
//  【致谢】
//  本项目基于 Intel® OpenVINO™ 工具包开发，感谢 Intel 提供的优秀开源项目。
//  OpenVINO™ 是 Intel Corporation 的商标。
//  ========================================================================
//  
//  【许可声明】
//  1. 本项目采用 Apache-2.0 License 开源协议，允许自由使用、修改和分发。
//  2. 使用本项目即表示您同意 Apache-2.0 License 许可证的所有条款。
//  3. 本项目按"原样"提供，不提供任何形式的担保。
//  4. 使用本项目产生的任何风险由使用者自行承担。
//  5. 修改或分发时请保留原始版权声明和许可声明。
//  ========================================================================
//

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
    /// OpenVINO 日志类 / OpenVINO OvLogger class
    /// <para>提供高性能、线程安全的日志记录功能，支持控制台输出和用户自定义回调。/ Provides high-performance, thread-safe logging with console output and custom callbacks.</para>
    /// <para>性能优化：在禁用低级别日志时，字符串格式化不会执行。/ Performance optimized: string formatting is skipped when low-level logs are disabled.</para>
    /// </summary>
    /// <example>
    /// 使用示例 / Usage example:
    /// <code>
    /// // 设置日志级别 / Set log level
    /// OvLogger.MinLevel = LogLevel.DEBUG;
    /// 
    /// // 输出日志 / Output logs
    /// OvLogger.Debug("调试信息 / Debug message");
    /// OvLogger.Info("应用启动 / Application started");
    /// OvLogger.Warn("警告信息 / Warning message");
    /// OvLogger.Error("错误信息 / Error message");
    /// 
    /// // 使用格式化 / Use formatting
    /// OvLogger.Info("处理完成，耗时 {0}ms / Processing completed, took {0}ms", elapsedTime);
    /// </code>
    /// </example>
    public static class OvLogger
    {
        private static readonly object _lock = new object();
        private static LogLevel _minLevel = LogLevel.INFO;
        private static LogCallback _customCallback;
        private static bool _useNativeCallback = false;
        
        // 原生回调委托实例（防止GC回收）/ Native callback delegate instance (prevent GC)
        private static LogCallbackDelegate _nativeCallback;

        /// <summary>
        /// 获取或设置最小日志级别 / Get or set the minimum log level
        /// <para>低于此级别的日志将被忽略。/ Logs below this level will be ignored.</para>
        /// </summary>
        /// <value>当前设置的最小日志级别。/ The current minimum log level setting.</value>
        /// <remarks>
        /// 在生产环境中建议设置为 INFO 或更高以提高性能。/ Set to INFO or higher in production for better performance.
        /// </remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// // 启用所有日志 / Enable all logs
        /// OvLogger.MinLevel = LogLevel.DEBUG;
        /// 
        /// // 仅显示警告和错误 / Show warnings and errors only
        /// OvLogger.MinLevel = LogLevel.WARNING;
        /// </code>
        /// </example>
        public static LogLevel MinLevel
        {
            get { return _minLevel; }
            set { _minLevel = value; }
        }

        /// <summary>
        /// 是否启用时间戳 / Whether to enable timestamps
        /// <para>默认值为 true。/ Default value is true.</para>
        /// </summary>
        /// <value>如果启用时间戳则为 true，否则为 false。/ true if timestamp is enabled; otherwise, false.</value>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// OvLogger.EnableTimestamp = false; // 禁用时间戳 / Disable timestamps
        /// OvLogger.Info("消息 / Message"); // 输出: [INFO] 消息 / Message
        /// </code>
        /// </example>
        public static bool EnableTimestamp { get; set; } = true;

        /// <summary>
        /// 是否启用日志级别前缀 / Whether to enable log level prefix
        /// <para>默认值为 true。/ Default value is true.</para>
        /// </summary>
        /// <value>如果启用级别前缀则为 true，否则为 false。/ true if level prefix is enabled; otherwise, false.</value>
        public static bool EnableLevelPrefix { get; set; } = true;

        /// <summary>
        /// 检查指定日志级别是否已启用 / Check if the specified log level is enabled
        /// </summary>
        /// <param name="level">要检查的日志级别。/ The log level to check.</param>
        /// <returns>如果级别已启用返回 true，否则返回 false。/ Returns true if the level is enabled.</returns>
        /// <remarks>
        /// 此方法用于条件日志记录，避免不必要的字符串格式化。/ Use this method for conditional logging to avoid unnecessary string formatting.
        /// </remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// if (OvLogger.IsEnabled(LogLevel.DEBUG))
        /// {
        ///     OvLogger.Debug($"复杂计算结果: {ExpensiveCalculation()}");
        /// }
        /// </code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEnabled(LogLevel level)
        {
            return level >= _minLevel && level != LogLevel.NONE;
        }

        /// <summary>
        /// 检查 DEBUG 级别是否启用 / Check if DEBUG level is enabled
        /// </summary>
        /// <value>如果 DEBUG 级别已启用则为 true。/ true if DEBUG level is enabled.</value>
        /// <remarks>
        /// 可用于条件编译或条件日志记录。/ Can be used for conditional compilation or conditional logging.
        /// </remarks>
        public static bool IsDebugEnabled => IsEnabled(LogLevel.DEBUG);

        /// <summary>
        /// 检查 INFO 级别是否启用 / Check if INFO level is enabled
        /// </summary>
        /// <value>如果 INFO 级别已启用则为 true。/ true if INFO level is enabled.</value>
        public static bool IsInfoEnabled => IsEnabled(LogLevel.INFO);

        /// <summary>
        /// 检查 WARNING 级别是否启用 / Check if WARNING level is enabled
        /// </summary>
        /// <value>如果 WARNING 级别已启用则为 true。/ true if WARNING level is enabled.</value>
        public static bool IsWarningEnabled => IsEnabled(LogLevel.WARNING);

        /// <summary>
        /// 检查 ERROR 级别是否启用 / Check if ERROR level is enabled
        /// </summary>
        /// <value>如果 ERROR 级别已启用则为 true。/ true if ERROR level is enabled.</value>
        public static bool IsErrorEnabled => IsEnabled(LogLevel.ERROR);

        /// <summary>
        /// 设置自定义日志回调 / Set custom log callback
        /// <para>设置后，日志将同时输出到控制台和回调函数。/ After setting, logs will be output to both console and callback.</para>
        /// </summary>
        /// <param name="callback">回调函数，接收日志级别和消息。/ Callback function receiving log level and message.</param>
        /// <remarks>
        /// 可用于将日志输出到文件、UI 控件或远程服务器。/ Can be used to output logs to file, UI controls, or remote server.
        /// <para>设置为 null 可清除回调（等效于调用 ClearCallback）。/ Set to null to clear callback (equivalent to calling ClearCallback).</para>
        /// </remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// // 设置文件日志回调 / Set file log callback
        /// OvLogger.SetCallback((level, message) =>
        /// {
        ///     File.AppendAllText("app.log", $"{DateTime.Now} [{level}] {message}\n");
        /// });
        /// </code>
        /// </example>
        public static void SetCallback(LogCallback callback)
        {
            lock (_lock)
            {
                _customCallback = callback;
            }
        }

        /// <summary>
        /// 清除自定义日志回调 / Clear custom log callback
        /// <para>清除后，日志仅输出到控制台。/ After clearing, logs are output to console only.</para>
        /// </summary>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// OvLogger.SetCallback(myCallback);
        /// // ... 使用回调 / Use callback ...
        /// OvLogger.ClearCallback(); // 恢复仅控制台输出 / Restore console-only output
        /// </code>
        /// </example>
        public static void ClearCallback()
        {
            lock (_lock)
            {
                _customCallback = null;
            }
        }

        /// <summary>
        /// 启用原生日志回调（与C API集成）/ Enable native log callback (integrate with C API)
        /// <para>将原生 OpenVINO C API 的日志重定向到 OvLogger。/ Redirects native OpenVINO C API logs to OvLogger.</para>
        /// </summary>
        /// <remarks>
        /// 启用后，来自原生库的消息会通过 OvLogger 输出。/ After enabling, messages from native library are output through OvLogger.
        /// <para>注意：需要在初始化 OpenVINO 之前调用。/ Note: Should be called before initializing OpenVINO.</para>
        /// </remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// // 启用原生回调 / Enable native callback
        /// OvLogger.EnableNativeCallback();
        /// 
        /// // 现在原生日志也会显示 / Now native logs will also appear
        /// using (var core = new Core())
        /// {
        ///     // 原生消息通过 OvLogger 输出 / Native messages output through OvLogger
        /// }
        /// </code>
        /// </example>
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
        /// <para>恢复原生库的默认日志处理。/ Restores default log handling for native library.</para>
        /// </summary>
        /// <remarks>
        /// 调用后，原生日志不再通过 OvLogger 输出。/ After calling, native logs are no longer output through OvLogger.
        /// </remarks>
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
        /// <para>处理来自原生 OpenVINO C API 的日志消息。/ Handles log messages from native OpenVINO C API.</para>
        /// </summary>
        /// <param name="message">原生日志消息。/ Native log message.</param>
        /// <remarks>
        /// 原生日志默认使用 INFO 级别，并添加 [Native] 前缀。/ Native logs use INFO level by default with [Native] prefix.
        /// </remarks>
        private static void NativeLogHandler(string message)
        {
            if (string.IsNullOrEmpty(message)) return;
            // 原生日志默认为 INFO 级别 / Native logs default to INFO level
            Log(LogLevel.INFO, "[Native] " + message);
        }

        /// <summary>
        /// 输出调试日志 / Output debug log
        /// <para>用于输出详细的调试信息，仅在 DEBUG 级别启用时记录。/ Used for detailed debug information, only logged when DEBUG level is enabled.</para>
        /// </summary>
        /// <param name="message">日志消息。/ Log message.</param>
        /// <remarks>
        /// 性能提示：如果 DEBUG 级别被禁用，此方法立即返回。/ Performance note: returns immediately if DEBUG level is disabled.
        /// </remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// OvLogger.Debug("进入函数 ProcessData / Entering function ProcessData");
        /// OvLogger.Debug($"变量值: {value} / Variable value: {value}");
        /// </code>
        /// </example>
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
        /// <param name="format">格式字符串。/ Format string.</param>
        /// <param name="args">格式参数。/ Format arguments.</param>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// OvLogger.Debug("处理 {0} 个项目，耗时 {1}ms / Processed {0} items in {1}ms", count, elapsed);
        /// </code>
        /// </example>
        public static void Debug(string format, params object[] args)
        {
            if (!IsEnabled(LogLevel.DEBUG)) return;
            Log(LogLevel.DEBUG, string.Format(format, args));
        }

        /// <summary>
        /// 输出信息日志 / Output info log
        /// <para>用于输出一般性信息，如应用状态、操作完成等。/ Used for general information like app status, operation completion, etc.</para>
        /// </summary>
        /// <param name="message">日志消息。/ Log message.</param>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// OvLogger.Info("模型加载成功 / Model loaded successfully");
        /// OvLogger.Info("服务已启动 / Service started");
        /// </code>
        /// </example>
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
        /// <param name="format">格式字符串。/ Format string.</param>
        /// <param name="args">格式参数。/ Format arguments.</param>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// OvLogger.Info("加载模型 {0} 完成，耗时 {1}s / Model {0} loaded in {1}s", modelName, seconds);
        /// </code>
        /// </example>
        public static void Info(string format, params object[] args)
        {
            if (!IsEnabled(LogLevel.INFO)) return;
            Log(LogLevel.INFO, string.Format(format, args));
        }

        /// <summary>
        /// 输出警告日志 / Output warning log
        /// <para>用于输出可能的问题或异常情况，不会导致程序失败。/ Used for potential issues or abnormal conditions that don't cause failure.</para>
        /// </summary>
        /// <param name="message">日志消息。/ Log message.</param>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// OvLogger.Warn("配置文件缺失，使用默认值 / Config file missing, using defaults");
        /// OvLogger.Warn("性能可能下降 / Performance may degrade");
        /// </code>
        /// </example>
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
        /// <param name="format">格式字符串。/ Format string.</param>
        /// <param name="args">格式参数。/ Format arguments.</param>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// OvLogger.Warn("参数 {0} 无效，使用默认值 {1} / Parameter {0} invalid, using default {1}", param, defaultValue);
        /// </code>
        /// </example>
        public static void Warn(string format, params object[] args)
        {
            if (!IsEnabled(LogLevel.WARNING)) return;
            Log(LogLevel.WARNING, string.Format(format, args));
        }

        /// <summary>
        /// 输出错误日志 / Output error log
        /// <para>用于输出错误信息，表示操作失败但程序可以继续运行。/ Used for error information indicating operation failure but program can continue.</para>
        /// </summary>
        /// <param name="message">日志消息。/ Log message.</param>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// OvLogger.Error("模型推理失败 / Model inference failed");
        /// OvLogger.Error("数据库连接超时 / Database connection timeout");
        /// </code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Error(string message)
        {
            if (!IsEnabled(LogLevel.ERROR)) return;
            Log(LogLevel.ERROR, message);
        }

        /// <summary>
        /// 输出错误日志（格式化）/ Output error log (formatted)
        /// <para>性能提示：如果 ERROR 级别被禁用，格式化不会执行。/ Performance note: formatting is skipped if ERROR level is disabled.</para>
        /// </summary>
        /// <param name="format">格式字符串。/ Format string.</param>
        /// <param name="args">格式参数。/ Format arguments.</param>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// OvLogger.Error("加载模型 {0} 失败: {1} / Failed to load model {0}: {1}", modelName, errorMessage);
        /// </code>
        /// </example>
        public static void Error(string format, params object[] args)
        {
            if (!IsEnabled(LogLevel.ERROR)) return;
            Log(LogLevel.ERROR, string.Format(format, args));
        }

        /// <summary>
        /// 输出严重错误日志 / Output fatal log
        /// <para>用于输出严重错误，表示程序无法继续运行。/ Used for fatal errors indicating program cannot continue.</para>
        /// </summary>
        /// <param name="message">日志消息。/ Log message.</param>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// OvLogger.Fatal("内存耗尽，程序即将终止 / Out of memory, application will terminate");
        /// OvLogger.Fatal("关键资源加载失败 / Critical resource loading failed");
        /// </code>
        /// </example>
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
        /// <param name="format">格式字符串。/ Format string.</param>
        /// <param name="args">格式参数。/ Format arguments.</param>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// OvLogger.Fatal("关键服务 {0} 启动失败: {1} / Critical service {0} failed to start: {1}", serviceName, error);
        /// </code>
        /// </example>
        public static void Fatal(string format, params object[] args)
        {
            if (!IsEnabled(LogLevel.FATAL)) return;
            Log(LogLevel.FATAL, string.Format(format, args));
        }

        /// <summary>
        /// 核心日志方法 / Core logging method
        /// <para>所有日志级别的方法最终都调用此方法。/ All log level methods eventually call this method.</para>
        /// </summary>
        /// <param name="level">日志级别。/ Log level.</param>
        /// <param name="message">日志消息。/ Log message.</param>
        /// <remarks>
        /// 此方法线程安全，会自动格式化消息并输出到控制台和回调。/ This method is thread-safe, automatically formats messages and outputs to console and callbacks.
        /// </remarks>
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
        /// <para>根据设置添加时间戳和级别前缀。/ Adds timestamp and level prefix based on settings.</para>
        /// </summary>
        /// <param name="level">日志级别。/ Log level.</param>
        /// <param name="message">原始日志消息。/ Raw log message.</param>
        /// <returns>格式化后的日志消息。/ Formatted log message.</returns>
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
        /// <para>将 LogLevel 枚举转换为字符串表示。/ Converts LogLevel enum to string representation.</para>
        /// </summary>
        /// <param name="level">日志级别。/ Log level.</param>
        /// <returns>级别字符串（如 DEBUG、INFO）。/ Level string (e.g., DEBUG, INFO).</returns>
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
        /// <para>根据日志级别设置不同的控制台颜色。/ Sets different console colors based on log level.</para>
        /// </summary>
        /// <param name="level">日志级别。/ Log level.</param>
        /// <param name="message">要输出的消息。/ Message to output.</param>
        /// <remarks>
        /// DEBUG - 灰色 / Gray
        /// <para>INFO - 白色 / White</para>
        /// <para>WARNING - 黄色 / Yellow</para>
        /// <para>ERROR - 红色 / Red</para>
        /// <para>FATAL - 深红色 / Dark Red</para>
        /// </remarks>
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
        /// <para>用于 OvLogger 内部错误处理，直接输出到控制台。/ Used for OvLogger internal error handling, outputs directly to console.</para>
        /// </summary>
        /// <param name="message">警告消息。/ Warning message.</param>
        /// <remarks>
        /// 此方法不使用日志锁，避免在日志系统故障时产生死锁。/ This method doesn't use logging lock to avoid deadlock when logging system fails.
        /// </remarks>
        private static void InternalWarn(string message)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[OvLogger Warning] {message}");
            Console.ForegroundColor = originalColor;
        }
    }
}
