// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// 创建张量 / Create a tensor
        /// </summary>
        /// <param name="type">元素类型 / Element type</param>
        /// <param name="shape">张量形状 / Tensor shape</param>
        /// <param name="tensor">返回的张量指针 / Returned tensor pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_create",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_create(uint type, ov_shape_t shape, ref IntPtr tensor);

        /// <summary>
        /// 从主机指针创建张量 / Create a tensor from host pointer
        /// </summary>
        /// <param name="type">元素类型 / Element type</param>
        /// <param name="shape">张量形状 / Tensor shape</param>
        /// <param name="host_ptr">主机数据指针 / Host data pointer</param>
        /// <param name="tensor">返回的张量指针 / Returned tensor pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        /// <remarks>
        /// 此函数不会复制数据，而是直接使用主机指针指向的内存。
        /// This function does not copy data, but directly uses the memory pointed to by the host pointer.
        /// </remarks>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_create_from_host_ptr",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_create_from_host_ptr(uint type, ov_shape_t shape, IntPtr host_ptr, ref IntPtr tensor);

        /// <summary>
        /// 释放 ov_tensor_t 分配的内存 / Release the memory allocated by ov_tensor_t
        /// </summary>
        /// <param name="tensor">张量指针 / Tensor pointer</param>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_tensor_free(IntPtr tensor);

        /// <summary>
        /// 为张量设置新形状 / Set new shape for tensor
        /// </summary>
        /// <param name="tensor">张量指针 / Tensor pointer</param>
        /// <param name="shape">新形状 / New shape</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_set_shape",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_set_shape(IntPtr tensor, ov_shape_t shape);

        /// <summary>
        /// 获取张量形状 / Get the tensor shape
        /// </summary>
        /// <param name="tensor">张量指针 / Tensor pointer</param>
        /// <param name="shape">返回的形状指针 / Returned shape pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_get_shape",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_get_shape(IntPtr tensor, IntPtr shape);

        /// <summary>
        /// 获取张量元素类型 / Get the tensor element type
        /// </summary>
        /// <param name="tensor">张量指针 / Tensor pointer</param>
        /// <param name="type">返回的元素类型 / Returned element type</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_get_element_type",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_get_element_type(IntPtr tensor, out uint type);

        /// <summary>
        /// 获取张量大小（元素数量）/ Get the tensor size (number of elements)
        /// </summary>
        /// <param name="tensor">张量指针 / Tensor pointer</param>
        /// <param name="size">返回的元素数量 / Returned number of elements</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_get_size",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_get_size(IntPtr tensor, ref ulong size);

        /// <summary>
        /// 获取张量字节大小 / Get the tensor byte size
        /// </summary>
        /// <param name="tensor">张量指针 / Tensor pointer</param>
        /// <param name="size">返回的字节大小 / Returned byte size</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_get_byte_size",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_get_byte_size(IntPtr tensor, ref ulong size);

        /// <summary>
        /// 获取张量数据指针 / Get the tensor data pointer
        /// </summary>
        /// <param name="tensor">张量指针 / Tensor pointer</param>
        /// <param name="data">返回的数据指针 / Returned data pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_data",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_data(IntPtr tensor, ref IntPtr data);

        /// <summary>
        /// 从字符串数组创建张量 / Create a tensor from string array
        /// </summary>
        /// <param name="string_array">字符串数组指针 / String array pointer</param>
        /// <param name="array_size">数组大小 / Array size</param>
        /// <param name="shape">张量形状 / Tensor shape</param>
        /// <param name="tensor">返回的张量指针 / Returned tensor pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_create_from_string_array",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_create_from_string_array(
            IntPtr string_array,
            ulong array_size,
            ov_shape_t shape,
            ref IntPtr tensor);

        /// <summary>
        /// 为张量设置字符串数据 / Set string data for tensor
        /// </summary>
        /// <param name="tensor">张量指针 / Tensor pointer</param>
        /// <param name="string_array">字符串数组指针 / String array pointer</param>
        /// <param name="array_size">数组大小 / Array size</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_set_string_data",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_set_string_data(
            IntPtr tensor,
            IntPtr string_array,
            ulong array_size);
    }
}
