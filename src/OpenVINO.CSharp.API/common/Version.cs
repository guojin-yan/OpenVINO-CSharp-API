// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp
{
    /// <summary>
    /// [结构体] 版本信息，描述插件和 OpenVINO 库 / [struct] Represents version information that describes plugins and the OpenVINO library
    /// </summary>
    /// <ingroup>ov_runtime_c#_api</ingroup>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct Version
    {
        /// <summary>
        /// 构建号，以 null 结尾的字符串 / A null terminated string with build number
        /// </summary>
        public string buildNumber;
        /// <summary>
        /// 描述信息，以 null 结尾的字符串 / A null terminated description string
        /// </summary>
        public string description;
        /// <summary>
        /// 构造 Version 结构体 / Constructs a Version
        /// </summary>
        /// <param name="buildNumber">构建号 / Build number</param>
        /// <param name="description">描述信息 / Description</param>
        public Version(string buildNumber, string description)
        {
            this.buildNumber = buildNumber;
            this.description = description;
        }

        /// <summary>
        /// 将 Version 转换为输出字符串 / Convert Version to output string
        /// </summary>
        /// <returns>输出字符串 / Output string</returns>
        public string to_string()
        {
            string str = "";
            str += description;
            str += "\r\n    Version : ";
            str += buildNumber.Substring(0, buildNumber.IndexOf("-"));
            str += "\r\n    Build   : ";
            str += buildNumber;
            return str;
        }
    }

    /// <summary>
    /// [结构体] 版本信息，描述设备和 OV 运行时库 / [struct] Represents version information that describes device and ov runtime library
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct CoreVersion
    {
        /// <summary>
        /// 设备名称 / A device name
        /// </summary>
        public string device_name;
        /// <summary>
        /// OpenVINO 版本 / The OpenVINO version
        /// </summary>
        public Version version;
    }

    /// <summary>
    /// [结构体] 版本信息列表，描述所有设备和 OV 运行时库 / [struct] Represents version information that describes all devices and ov runtime library
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CoreVersionList
    {
        /// <summary>
        /// 设备版本数组指针 / An array of device versions
        /// </summary>
        public IntPtr core_version;
        /// <summary>
        /// 数组中的版本数量 / A number of versions in the array
        /// </summary>
        public ulong size;
    }
}
