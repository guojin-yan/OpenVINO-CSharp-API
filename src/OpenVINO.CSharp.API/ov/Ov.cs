// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

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
            int size = Marshal.SizeOf(typeof(Version));
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                ExceptionStatus status = ov_get_openvino_version(ptr);
                if (status != ExceptionStatus.OK)
                {
                    System.Diagnostics.Debug.WriteLine("ov get_openvino_version() error!");
                    return new Version();
                }

                Version version = Marshal.PtrToStructure<Version>(ptr);
                string build = string.Copy(version.buildNumber);
                string description = string.Copy(version.description);
                Version new_version = new Version(build, description);
                ov_version_free(ptr);
                return new_version;
            }
            finally
            {
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
                    return Marshal.PtrToStringAnsi(msgPtr) ?? "Unknown error";
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
                string info = ov_get_error_info(status);
                return info ?? "Unknown error";
            }
            catch
            {
                return "Unknown error";
            }
        }
    }
}
