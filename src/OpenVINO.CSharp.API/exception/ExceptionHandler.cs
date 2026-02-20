// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static OpenVinoSharp.native.NativeMethods;

namespace OpenVinoSharp
{
    /// <summary>
    /// OpenVINO C API返回值异常检测处理器 / OpenVINO C API return value anomaly detection handler
    /// <para>高性能实现：缓存异常消息，减少字符串分配。/ High-performance implementation: caches exception messages to reduce string allocations.</para>
    /// </summary>
    internal static class ExceptionHandler
    {
        // 缓存状态描述，避免重复创建字符串 / Cache status descriptions to avoid repeated string creation
        private static readonly string[] StatusDescriptions = new string[]
        {
            "成功 / OK",                           // OK = 0
            "一般错误 / General error",            // GENERAL_ERROR
            "未实现 / Not implemented",            // NOT_IMPLEMENTED
            "网络未加载 / Network not loaded",     // NETWORK_NOT_LOADED
            "参数不匹配 / Parameter mismatch",     // PARAMETER_MISMATCH
            "未找到 / Not found",                  // NOT_FOUND
            "越界 / Out of bounds",                // OUT_OF_BOUNDS
            "意外错误 / Unexpected error",         // UNEXPECTED
            "请求繁忙 / Request busy",             // REQUEST_BUSY
            "结果未就绪 / Result not ready",       // RESULT_NOT_READY
            "未分配 / Not allocated",              // NOT_ALLOCATED
            "推理未开始 / Inference not started",  // INFER_NOT_STARTED
            "网络未读取 / Network not read",       // NETWORK_NOT_READ
            "推理已取消 / Inference cancelled",    // INFER_CANCELLED
            "无效的C参数 / Invalid C parameter",   // INVALID_C_PARAM
            "未知的C错误 / Unknown C error",       // UNKNOWN_C_ERROR
            "C方法未实现 / C method not implemented", // NOT_IMPLEMENT_C_METHOD
            "未知异常 / Unknown exception",        // UNKNOW_EXCEPTION
            "指针为空 / Pointer is null"           // PTR_NULL
        };

        /// <summary>
        /// 检查返回值是否有异常，如果有则根据异常值返回相应的异常 / Check if return value has exception and throw corresponding exception
        /// <para>性能优化：使用 AggressiveInlining 减少调用开销。/ Performance optimization: uses AggressiveInlining to reduce call overhead.</para>
        /// </summary>
        /// <param name="status">异常状态码 / Exception status code</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowOnError(ExceptionStatus status)
        {
            if (status == ExceptionStatus.OK)
            {
                return;
            }

            ThrowExceptionCore(status);
        }

        /// <summary>
        /// 核心异常抛出逻辑（分离以优化内联）/ Core exception throwing logic (separated for inlining optimization)
        /// </summary>
        private static void ThrowExceptionCore(ExceptionStatus status)
        {
            string errorMessage = GetLastErrorMessage();
            string statusDescription = GetStatusDescriptionFast(status);

            throw new OVException(status, $"{statusDescription}: {errorMessage}");
        }

        /// <summary>
        /// 从OpenVINO获取最后的错误消息 / Get the last error message from OpenVINO
        /// <para>性能优化：使用 Span 和栈分配减少堆分配（.NET Core 2.1+）。</para>
        /// </summary>
        private static string GetLastErrorMessage()
        {
            try
            {
                IntPtr msgPtr = ov_get_last_err_msg();
                if (msgPtr != IntPtr.Zero)
                {
#if HAS_SPAN
                    // 使用 Span 高效读取 ANSI 字符串
                    unsafe
                    {
                        byte* ptr = (byte*)msgPtr;
                        int len = 0;
                        while (ptr[len] != 0) len++;
                        return System.Text.Encoding.UTF8.GetString(ptr, len);
                    }
#else
                    return Marshal.PtrToStringAnsi(msgPtr) ?? "未知错误 / Unknown error";
#endif
                }
            }
            catch
            {
                // 获取错误消息时忽略异常 / Ignore exceptions when trying to get error message
            }
            return "未知错误 / Unknown error";
        }

        /// <summary>
        /// 快速获取状态码描述（使用缓存数组）/ Get status code description quickly (using cached array)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetStatusDescriptionFast(ExceptionStatus status)
        {
            int index = (int)status;
            if (index >= 0 && index < StatusDescriptions.Length)
            {
                return StatusDescriptions[index];
            }
            return $"未知错误 / Unknown error ({index})";
        }
    }
}
