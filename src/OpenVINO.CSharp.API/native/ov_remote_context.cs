// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Allocates memory tensor in device memory or wraps user-supplied memory handle
        /// using the specified tensor description and low-level device-specific parameters.
        /// </summary>
        /// <param name="context">A pointer to the ov_remote_context_t instance.</param>
        /// <param name="type">Defines the element type of the tensor.</param>
        /// <param name="shape">Defines the shape of the tensor.</param>
        /// <param name="object_args_size">Size of the low-level tensor object parameters.</param>
        /// <param name="remote_tensor">Pointer to returned ov_tensor_t that contains remote tensor instance.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_remote_context_create_tensor",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_remote_context_create_tensor(
            IntPtr context,
            uint type,
            ov_shape_t shape,
            ulong object_args_size,
            ref IntPtr remote_tensor);

        /// <summary>
        /// Returns name of a device on which underlying object is allocated.
        /// </summary>
        /// <param name="context">A pointer to the ov_remote_context_t instance.</param>
        /// <param name="device_name">Device name will be returned.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_remote_context_get_device_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_remote_context_get_device_name(
            IntPtr context,
            ref IntPtr device_name);

        /// <summary>
        /// Returns a string contains device-specific parameters required for low-level
        /// operations with the underlying object.
        /// </summary>
        /// <param name="context">A pointer to the ov_remote_context_t instance.</param>
        /// <param name="size">The size of param pairs.</param>
        /// <param name="params">Param name:value list.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_remote_context_get_params",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_remote_context_get_params(
            IntPtr context,
            ref ulong size,
            ref IntPtr @params);

        /// <summary>
        /// This method is used to create a host tensor object friendly for the device in current context.
        /// </summary>
        /// <param name="context">A pointer to the ov_remote_context_t instance.</param>
        /// <param name="type">Defines the element type of the tensor.</param>
        /// <param name="shape">Defines the shape of the tensor.</param>
        /// <param name="tensor">Pointer to ov_tensor_t that contains host tensor.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_remote_context_create_host_tensor",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_remote_context_create_host_tensor(
            IntPtr context,
            uint type,
            ov_shape_t shape,
            ref IntPtr tensor);

        /// <summary>
        /// Release the memory allocated by ov_remote_context_t.
        /// </summary>
        /// <param name="context">A pointer to the ov_remote_context_t to free memory.</param>
        [DllImport("openvino_c", EntryPoint = "ov_remote_context_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_remote_context_free(IntPtr context);

        /// <summary>
        /// Returns a string contains device-specific parameters required for low-level
        /// operations with underlying object.
        /// </summary>
        /// <param name="tensor">Pointer to ov_tensor_t that contains host tensor.</param>
        /// <param name="size">The size of param pairs.</param>
        /// <param name="params">Param name:value list.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_remote_tensor_get_params",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_remote_tensor_get_params(
            IntPtr tensor,
            ref ulong size,
            ref IntPtr @params);

        /// <summary>
        /// Returns name of a device on which underlying object is allocated.
        /// </summary>
        /// <param name="remote_tensor">A pointer to the remote tensor instance.</param>
        /// <param name="device_name">Device name will be return.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_remote_tensor_get_device_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_remote_tensor_get_device_name(
            IntPtr remote_tensor,
            ref IntPtr device_name);
    }
}
