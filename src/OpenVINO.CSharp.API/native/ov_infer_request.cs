// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Release the memory allocated by ov_infer_request_t.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_infer_request_free(IntPtr infer_request);

        /// <summary>
        /// Set the input tensor of the infer request by port.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_input_tensor",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_input_tensor(IntPtr infer_request, IntPtr tensor);

        /// <summary>
        /// Set the input tensor of the infer request by index.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_input_tensor_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_input_tensor_by_index(IntPtr infer_request, ulong idx, IntPtr tensor);

        /// <summary>
        /// Set the input tensor of the infer request by tensor name.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_input_tensor_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_input_tensor_by_name(IntPtr infer_request, ref sbyte tensor_name, IntPtr tensor);

        /// <summary>
        /// Set the output tensor of the infer request by port.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_output_tensor",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_output_tensor(IntPtr infer_request, IntPtr tensor);

        /// <summary>
        /// Set the output tensor of the infer request by index.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_output_tensor_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_output_tensor_by_index(IntPtr infer_request, ulong idx, IntPtr tensor);

        /// <summary>
        /// Get the input tensor of the infer request by port.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_input_tensor",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_input_tensor(IntPtr infer_request, ref IntPtr tensor);

        /// <summary>
        /// Get the input tensor of the infer request by index.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_input_tensor_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_input_tensor_by_index(IntPtr infer_request, ulong idx, ref IntPtr tensor);

        /// <summary>
        /// Get the output tensor of the infer request by port.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_output_tensor",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_output_tensor(IntPtr infer_request, ref IntPtr tensor);

        /// <summary>
        /// Get the output tensor of the infer request by index.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_output_tensor_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_output_tensor_by_index(IntPtr infer_request, ulong idx, ref IntPtr tensor);

        /// <summary>
        /// Inference the infer request.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_infer",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_infer(IntPtr infer_request);

        /// <summary>
        /// Cancel the infer request.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_cancel",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_cancel(IntPtr infer_request);

        /// <summary>
        /// Start inference of the infer request asynchronously.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_start_async",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_start_async(IntPtr infer_request);

        /// <summary>
        /// Wait for the result of the infer request.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_wait",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_wait(IntPtr infer_request);

        /// <summary>
        /// Wait for the result of the infer request with timeout.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_wait_for",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_wait_for(IntPtr infer_request, long timeout);

        /// <summary>
        /// Set callback function for the infer request.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_callback",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_callback(IntPtr infer_request, IntPtr callback);
    }
}
