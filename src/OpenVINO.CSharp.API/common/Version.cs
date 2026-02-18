// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp
{
    /// <summary>
    /// [struct] Represents version information that describes plugins and the OpenVINO library
    /// </summary>
    /// <ingroup>ov_runtime_c#_api</ingroup>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct Version
    {
        /// <summary>
        /// A null terminated string with build number
        /// </summary>
        public string buildNumber;
        /// <summary>
        /// A null terminated description string
        /// </summary>
        public string description;
        /// <summary>
        /// Constructs a Version.
        /// </summary>
        /// <param name="buildNumber"></param>
        /// <param name="description"></param>
        public Version(string buildNumber, string description)
        {
            this.buildNumber = buildNumber;
            this.description = description;
        }

        /// <summary>
        /// Convert Version to output string
        /// </summary>
        /// <returns>Output string</returns>
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
    /// [struct] Represents version information that describes device and ov runtime library
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct CoreVersion
    {
        /// <summary>
        /// A device name
        /// </summary>
        public string device_name;
        /// <summary>
        /// The OpenVINO version.
        /// </summary>
        public Version version;
    }

    /// <summary>
    /// [struct] Represents version information that describes all devices and ov runtime library
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CoreVersionList
    {
        /// <summary>
        /// An array of device versions
        /// </summary>
        public IntPtr core_version;
        /// <summary>
        /// A number of versions in the array
        /// </summary>
        public ulong size;
    }
}
