// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Shut down the OpenVINO.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_shutdown",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_shutdown();

        /// <summary>
        /// Adds an extension to the core.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_add_extension",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_add_extension(IntPtr core, string path);
    }
}
