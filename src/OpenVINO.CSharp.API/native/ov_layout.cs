// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Create a layout object.
        /// </summary>
        /// <param name="layout_desc">The description of layout.</param>
        /// <param name="layout">The layout input pointer.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_layout_create",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_layout_create(string layout_desc, ref IntPtr layout);

        /// <summary>
        /// Free layout object.
        /// </summary>
        /// <param name="layout">Layout to be released.</param>
        [DllImport("openvino_c", EntryPoint = "ov_layout_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_layout_free(IntPtr layout);

        /// <summary>
        /// Convert layout object to a readable string.
        /// </summary>
        /// <param name="layout">Layout to be converted.</param>
        /// <returns>String that describes the layout content.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_layout_to_string",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr ov_layout_to_string(IntPtr layout);
    }
}
