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
        /// 初始化 OpenVINO 运行时环境，确保原生库已加载
        /// Initialize OpenVINO runtime environment and ensure native library is loaded
        /// </summary>
        /// <remarks>
        /// 此方法可选，原生库会在第一次使用 OpenVINO 功能时自动加载。
        /// 但如果您想提前控制加载过程或指定自定义路径，可以调用此方法。
        /// This method is optional, native library will be loaded automatically on first use.
        /// But you can call this to control the loading process early or specify custom paths.
        /// </remarks>
        /// <param name="libraryPath">原生库路径（可选，默认自动搜索）/ Native library path (optional, auto-search by default)</param>
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
        /// Set log callback function
        /// </summary>
        /// <param name="func">Log callback function</param>
        public static void set_log_callback(LogCallbackDelegate func)
        {
            ov_util_set_log_callback(func);
        }


        /// <summary>
        /// Reset log callback to default
        /// </summary>
        public static void reset_log_callback()
        {
            ov_util_reset_log_callback();
        }

        /// <summary>
        /// Get version of OpenVINO.
        /// </summary>
        /// <returns>Version of OpenVINO</returns>
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
        /// Read content from file as byte array
        /// </summary>
        /// <param name="file">File path</param>
        /// <returns>File content as byte array</returns>
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
        /// Read content from file as ReadOnlyMemory (more efficient, .NET Core 2.1+ / .NET 5+)
        /// </summary>
        /// <param name="file">File path</param>
        /// <returns>File content as ReadOnlyMemory</returns>
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
        /// Read content from file as ReadOnlySpan (efficient, no allocation after initial read)
        /// </summary>
        public static ReadOnlySpan<byte> content_from_file_span_memory(string file)
        {
            return content_from_file(file).AsSpan();
        }
#endif

        /// <summary>
        /// Get the last error message from OpenVINO
        /// </summary>
        /// <returns></returns>
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
        /// Get error info from status code
        /// </summary>
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
