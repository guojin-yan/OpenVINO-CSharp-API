// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Create a tensor.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_create",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_create(uint type, IntPtr shape, ref IntPtr tensor);

        /// <summary>
        /// Create a tensor from host pointer.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_create_from_host_ptr",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_create_from_host_ptr(uint type, IntPtr shape, IntPtr host_ptr, ref IntPtr tensor);

        /// <summary>
        /// Release the memory allocated by ov_tensor_t.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_tensor_free(IntPtr tensor);

        /// <summary>
        /// Set new shape for tensor.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_set_shape",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_set_shape(IntPtr tensor, IntPtr shape);

        /// <summary>
        /// Get the tensor shape.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_get_shape",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_get_shape(IntPtr tensor, IntPtr shape);

        /// <summary>
        /// Get the tensor element type.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_get_element_type",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_get_element_type(IntPtr tensor, out uint type);

        /// <summary>
        /// Get the tensor size (number of elements).
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_get_size",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_get_size(IntPtr tensor, ref ulong size);

        /// <summary>
        /// Get the tensor byte size.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_get_byte_size",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_get_byte_size(IntPtr tensor, ref ulong size);

        /// <summary>
        /// Get the tensor data pointer.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_data",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_data(IntPtr tensor, ref IntPtr data);
    }
}
