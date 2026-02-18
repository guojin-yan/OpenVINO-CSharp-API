// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Print the error info.
        /// </summary>
        /// <param name="status">a status code.</param>
        /// <returns>error info.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_get_error_info",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static string ov_get_error_info(int status);

        /// <summary>
        /// free char
        /// </summary>
        /// <param name="content">The pointer to the char to free.</param>
        [DllImport("openvino_c", EntryPoint = "ov_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_free(IntPtr content);

        /// <summary>
        /// Get the last error msg.
        /// </summary>
        /// <returns>The last error msg.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_get_last_err_msg",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr ov_get_last_err_msg();
    }
}
