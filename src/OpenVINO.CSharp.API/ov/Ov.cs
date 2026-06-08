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
using System.IO;
using System.Runtime.InteropServices;
using static OpenVinoSharp.native.NativeMethods;

namespace OpenVinoSharp
{
    /// <summary>
    /// Global functions under ov namespace
    /// </summary>
    public static partial class Ov
    {
        /// <summary>
        /// 初始化 OpenVINO 运行时环境，确保原生库已加载 / Initialize OpenVINO runtime environment and ensure native library is loaded
        /// </summary>
        /// <remarks>
        /// 此方法可选，原生库会在第一次使用 OpenVINO 功能时自动加载。
        /// 但如果您想提前控制加载过程或指定自定义路径，可以调用此方法。
        /// This method is optional, native library will be loaded automatically on first use.
        /// But you can call this to control the loading process early or specify custom paths.
        /// </remarks>
        /// <param name="libraryPath">原生库路径（可选，默认自动搜索）/ Native library path (optional, auto-search by default)</param>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// // 使用默认路径初始化 / Initialize with default path
        /// Ov.Initialize();
        /// 
        /// // 使用自定义路径初始化 / Initialize with custom path
        /// Ov.Initialize(@"C:\openvino\openvino_c.dll");
        /// </code>
        /// </example>
        public static void Initialize(string libraryPath = null)
        {
            if (!string.IsNullOrEmpty(libraryPath))
            {
                NativeLibraryLoader.Load(libraryPath);
            }
            else
            {
                NativeLibraryLoader.EnsureLoaded();
            }
        }

        /// <summary>
        /// 设置日志回调函数 / Set log callback function
        /// </summary>
        /// <param name="func">日志回调函数 / Log callback function</param>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// Ov.set_log_callback((msg) => {
        ///     Console.WriteLine($"[OpenVINO] {msg}");
        /// });
        /// </code>
        /// </example>
        public static void set_log_callback(LogCallbackDelegate func)
        {
            ov_util_set_log_callback(func);
        }


        /// <summary>
        /// 重置日志回调为默认状态 / Reset log callback to default
        /// </summary>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// // 先设置自定义回调 / Set custom callback first
        /// Ov.set_log_callback((msg) => Console.WriteLine(msg));
        /// 
        /// // 然后重置为默认 / Then reset to default
        /// Ov.reset_log_callback();
        /// </code>
        /// </example>
        public static void reset_log_callback()
        {
            ov_util_reset_log_callback();
        }

        /// <summary>
        /// 获取 OpenVINO 版本信息 / Get version of OpenVINO
        /// </summary>
        /// <returns>OpenVINO 版本信息 / Version of OpenVINO</returns>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// Version version = Ov.get_openvino_version();
        /// Console.WriteLine($"Build: {version.buildNumber}");
        /// Console.WriteLine($"Description: {version.description}");
        /// </code>
        /// </example>
        public static Version get_openvino_version()
        {
            int size = Marshal.SizeOf(typeof(OpenVinoSharp.native.ov_version_t));
            IntPtr ptr = Marshal.AllocHGlobal(size);
            bool versionAllocated = false;
            try
            {
                ExceptionStatus status = ov_get_openvino_version(ptr);
                if (status != ExceptionStatus.OK)
                {
                    System.Diagnostics.Debug.WriteLine("ov get_openvino_version() error!");
                    return new Version();
                }

                versionAllocated = true;
                OpenVinoSharp.native.ov_version_t version = Marshal.PtrToStructure<OpenVinoSharp.native.ov_version_t>(ptr);
                string build = StringUtils.Utf8PtrToString(version.buildNumber) ?? string.Empty;
                string description = StringUtils.Utf8PtrToString(version.description) ?? string.Empty;
                return new Version(build, description);
            }
            finally
            {
                if (versionAllocated)
                    ov_version_free(ptr);
                Marshal.FreeHGlobal(ptr);
            }
        }

        /// <summary>
        /// 从文件读取内容作为字节数组 / Read content from file as byte array
        /// </summary>
        /// <param name="file">文件路径 / File path</param>
        /// <returns>文件内容的字节数组 / File content as byte array</returns>
        /// <exception cref="FileNotFoundException">当文件不存在时抛出 / Thrown when file does not exist</exception>
        /// <exception cref="IOException">当读取文件失败时抛出 / Thrown when file read fails</exception>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// try
        /// {
        ///     byte[] modelData = Ov.content_from_file(@"model.xml");
        ///     Console.WriteLine($"Read {modelData.Length} bytes");
        /// }
        /// catch (FileNotFoundException ex)
        /// {
        ///     Console.WriteLine($"File not found: {ex.Message}");
        /// }
        /// </code>
        /// </example>
        public static byte[] content_from_file(string file)
        {
            if (!File.Exists(file))
            {
                throw new FileNotFoundException($"File not found: {file}");
            }

            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                long len = fs.Length;
                byte[] data = new byte[len + 1]; // +1 for null terminator if needed

                int bytesRead = fs.Read(data, 0, (int)len);
                if (bytesRead != len)
                {
                    throw new IOException($"Failed to read complete file: {file}");
                }

                return data;
            }
        }

#if HAS_SPAN
        /// <summary>
        /// 从文件读取内容作为 ReadOnlyMemory（更高效，适用于 .NET Core 2.1+ / .NET 5+）/ Read content from file as ReadOnlyMemory (more efficient, .NET Core 2.1+ / .NET 5+)
        /// </summary>
        /// <param name="file">文件路径 / File path</param>
        /// <returns>文件内容的 ReadOnlyMemory / File content as ReadOnlyMemory</returns>
        /// <exception cref="FileNotFoundException">当文件不存在时抛出 / Thrown when file does not exist</exception>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// ReadOnlyMemory&lt;byte&gt; modelData = Ov.content_from_file_span(@"model.xml");
        /// // 可以直接用于模型加载 / Can be used directly for model loading
        /// </code>
        /// </example>
        public static ReadOnlyMemory<byte> content_from_file_span(string file)
        {
            if (!File.Exists(file))
            {
                throw new FileNotFoundException($"File not found: {file}");
            }

            byte[] data = File.ReadAllBytes(file);
            return new ReadOnlyMemory<byte>(data);
        }

        /// <summary>
        /// 从文件读取内容作为 ReadOnlySpan（高效，初始读取后无额外分配）/ Read content from file as ReadOnlySpan (efficient, no allocation after initial read)
        /// </summary>
        /// <param name="file">文件路径 / File path</param>
        /// <returns>文件内容的 ReadOnlySpan / File content as ReadOnlySpan</returns>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// ReadOnlySpan&lt;byte&gt; modelData = Ov.content_from_file_span_memory(@"model.xml");
        /// // 适合在性能敏感场景使用 / Suitable for performance-sensitive scenarios
        /// </code>
        /// </example>
        public static ReadOnlySpan<byte> content_from_file_span_memory(string file)
        {
            return content_from_file(file).AsSpan();
        }
#endif

        /// <summary>
        /// 获取 OpenVINO 的最后错误信息 / Get the last error message from OpenVINO
        /// </summary>
        /// <returns>错误信息字符串，如果没有错误则返回 "Unknown error" / Error message string, returns "Unknown error" if no error</returns>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// string errorMsg = Ov.get_last_error_message();
        /// if (errorMsg != "Unknown error")
        /// {
        ///     Console.WriteLine($"OpenVINO Error: {errorMsg}");
        /// }
        /// </code>
        /// </example>
        public static string get_last_error_message()
        {
            try
            {
                IntPtr msgPtr = ov_get_last_err_msg();
                if (msgPtr != IntPtr.Zero)
                {
                    return StringUtils.Utf8PtrToString(msgPtr) ?? "Unknown error";
                }
            }
            catch
            {
            }
            return "Unknown error";
        }

        /// <summary>
        /// 从状态码获取错误信息 / Get error info from status code
        /// </summary>
        /// <param name="status">错误状态码 / Error status code</param>
        /// <returns>错误信息字符串 / Error message string</returns>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// ExceptionStatus status = some_openvino_operation();
        /// if (status != ExceptionStatus.OK)
        /// {
        ///     string errorInfo = Ov.get_error_info((int)status);
        ///     Console.WriteLine($"Operation failed: {errorInfo}");
        /// }
        /// </code>
        /// </example>
        public static string get_error_info(int status)
        {
            try
            {
                IntPtr infoPtr = ov_get_error_info_ptr(status);
                string info = StringUtils.Utf8PtrToString(infoPtr);
                return string.IsNullOrEmpty(info) ? "Unknown error" : info;
            }
            catch
            {
                return "Unknown error";
            }
        }
    }
}
