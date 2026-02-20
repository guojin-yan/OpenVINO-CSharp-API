// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    /// <summary>
    /// OpenVINO C API 原生 P/Invoke 方法 / Native P/Invoke methods for OpenVINO C API
    /// <para>提供与 OpenVINO C 库的底层互操作。/ Provides low-level interop with OpenVINO C library.</para>
    /// </summary>
    public static partial class NativeMethods
    {
        // 静态构造函数 - 确保在使用任何 P/Invoke 方法之前加载原生库
        // Static constructor - ensure native library is loaded before any P/Invoke call
        static NativeMethods()
        {
            NativeLibraryLoader.EnsureLoaded();
        }

        // 动态 DLL 名称 - 允许运行时配置
        private static string _dllName = "openvino_c";
        
        /// <summary>
        /// 获取或设置 DLL 名称 / Gets or sets the DLL name
        /// </summary>
        public static string DllName
        {
            get => _dllName;
            set => _dllName = value ?? "openvino_c";
        }

        /// <summary>
        /// 获取完整 DLL 名称（带平台特定扩展名）/ Gets the full DLL name with platform-specific extension
        /// </summary>
        /// <returns>完整的 DLL 文件名 / Full DLL file name</returns>
        public static string GetFullDllName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return $"{_dllName}.dll";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return $"lib{_dllName}.so";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return $"lib{_dllName}.dylib";
            else
                return _dllName;
        }
    }
}
