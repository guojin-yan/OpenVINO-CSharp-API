// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

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
