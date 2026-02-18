// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Create a tensor from string array.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_create_from_string_array",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_create_from_string_array(
            IntPtr string_array,
            ulong array_size,
            IntPtr shape,
            ref IntPtr tensor);

        /// <summary>
        /// Set string data for tensor.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_set_string_data",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_set_string_data(
            IntPtr tensor,
            IntPtr string_array,
            ulong array_size);
    }
}
