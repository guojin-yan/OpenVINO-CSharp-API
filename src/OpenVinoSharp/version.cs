using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    /// <summary>
    /// [struct] Represents version information that describes plugins and the OpemVINO library
    /// </summary>
    /// <ingroup>ov_runtime_c#_api</ingroup>
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
    public struct CoreVersion 
    {
        /// <summary>
        /// A device name
        /// </summary>
        public string device_name;
        public Version version;
    }
    /// <summary>
    /// [struct] Represents version information that describes all devices and ov runtime library
    /// </summary>
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

    /// <summary>
    /// Global functions under ov namespace
    /// </summary>
    public static partial class ov
    {
        /// <summary>
        /// Get version of OpenVINO.
        /// </summary>
        /// <returns>Version of OpenVINO</returns>
        public static Version get_openvino_version()
        {
            int l = Marshal.SizeOf(typeof(Version));
            IntPtr ptr = Marshal.AllocHGlobal(l);
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_get_openvino_version(ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("ov get_openvino_version() error!");
                return new Version();
            }
            var temp = Marshal.PtrToStructure(ptr, typeof(Version));
            Version version = (Version)temp;
            status = (ExceptionStatus)NativeMethods.ov_version_free(ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("ov get_openvino_version() error!");
                return new Version();
            }
            return version;
        }
    };



}
