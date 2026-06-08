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
                    return StringUtils.Utf8PtrToString(msgPtr) ?? "未知错误 / Unknown error";
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
