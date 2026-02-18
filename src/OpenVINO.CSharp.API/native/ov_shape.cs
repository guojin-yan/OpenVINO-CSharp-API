// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Create a static shape.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_shape_create",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_shape_create(long rank, ref long dims, ref IntPtr shape);

        /// <summary>
        /// Free a shape.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_shape_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_shape_free(IntPtr shape);

        /// <summary>
        /// Convert shape to string.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_shape_to_string",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_shape_to_string(IntPtr shape, ref IntPtr str);
    }
}
