using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp
{    /// <summary>
     /// Global functions under ov namespace
     /// </summary>
    public class Ov
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
            string build = String.Copy(version.buildNumber);
            string description = String.Copy(version.description);
            Version new_version = new Version(build, description);
            status = (ExceptionStatus)NativeMethods.ov_version_free(ptr);
            return new_version;
        }
    }
}