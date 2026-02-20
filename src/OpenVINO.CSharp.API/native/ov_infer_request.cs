// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        #region Infer Request Destruction

        /// <summary>
        /// Release the memory allocated by ov_infer_request_t.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_infer_request_free(IntPtr infer_request);

        #endregion

        #region Set Tensor by Port

        /// <summary>
        /// Set an input/output tensor to infer request for the port.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_tensor_by_port",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_tensor_by_port(
            IntPtr infer_request,
            IntPtr port,
            IntPtr tensor);

        /// <summary>
        /// Set an input/output tensor to infer request for the const port.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_tensor_by_const_port",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_tensor_by_const_port(
            IntPtr infer_request,
            IntPtr const_port,
            IntPtr tensor);

        /// <summary>
        /// Set an input/output tensor to infer on by the name of tensor.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_tensor",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_tensor(
            IntPtr infer_request,
            string tensor_name,
            IntPtr tensor);

        #endregion

        #region Set Input Tensor

        /// <summary>
        /// Set an input tensor to infer on by the index of tensor.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_input_tensor_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_input_tensor_by_index(
            IntPtr infer_request,
            ulong idx,
            IntPtr tensor);

        /// <summary>
        /// Set an input tensor for the model with single input to infer on.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_input_tensor",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_input_tensor(
            IntPtr infer_request,
            IntPtr tensor);

        #endregion

        #region Set Output Tensor

        /// <summary>
        /// Set an output tensor to infer by the index of output tensor.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_output_tensor_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_output_tensor_by_index(
            IntPtr infer_request,
            ulong idx,
            IntPtr tensor);

        /// <summary>
        /// Set an output tensor to infer models with single output.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_output_tensor",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_output_tensor(
            IntPtr infer_request,
            IntPtr tensor);

        #endregion

        #region Get Tensor by Name

        /// <summary>
        /// Get an input/output tensor by the name of tensor.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_tensor",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_tensor(
            IntPtr infer_request,
            string tensor_name,
            ref IntPtr tensor);

        #endregion

        #region Get Tensor by Port

        /// <summary>
        /// Get an input/output tensor by port.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_tensor_by_port",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_tensor_by_port(
            IntPtr infer_request,
            IntPtr port,
            ref IntPtr tensor);

        /// <summary>
        /// Get an input/output tensor by const port.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_tensor_by_const_port",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_tensor_by_const_port(
            IntPtr infer_request,
            IntPtr const_port,
            ref IntPtr tensor);

        #endregion

        #region Get Input Tensor

        /// <summary>
        /// Get an input tensor by the index of input tensor.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_input_tensor_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_input_tensor_by_index(
            IntPtr infer_request,
            ulong idx,
            ref IntPtr tensor);

        /// <summary>
        /// Get an input tensor from the model with only one input tensor.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_input_tensor",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_input_tensor(
            IntPtr infer_request,
            ref IntPtr tensor);

        #endregion

        #region Get Output Tensor

        /// <summary>
        /// Get an output tensor by the index of output tensor.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_output_tensor_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_output_tensor_by_index(
            IntPtr infer_request,
            ulong idx,
            ref IntPtr tensor);

        /// <summary>
        /// Get an output tensor from the model with only one output tensor.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_output_tensor",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_output_tensor(
            IntPtr infer_request,
            ref IntPtr tensor);

        #endregion

        #region Inference Execution

        /// <summary>
        /// Infer specified input(s) in synchronous mode.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_infer",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_infer(IntPtr infer_request);

        /// <summary>
        /// Cancel inference request.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_cancel",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_cancel(IntPtr infer_request);

        /// <summary>
        /// Start inference of specified input(s) in asynchronous mode.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_start_async",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_start_async(IntPtr infer_request);

        /// <summary>
        /// Wait for the result to become available.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_wait",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_wait(IntPtr infer_request);

        /// <summary>
        /// Waits for the result to become available with timeout.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_wait_for",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_wait_for(
            IntPtr infer_request,
            long timeout);

        #endregion

        #region Callback

        /// <summary>
        /// Set callback function, which will be called when inference is done.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_callback",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_callback(
            IntPtr infer_request,
            ref ov_callback_t callback);

        /// <summary>
        /// Callback delegate for async inference completion
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ov_infer_request_callback_func(IntPtr args);

        #endregion

        #region Profiling Info

        /// <summary>
        /// Query performance measures per layer to identify the most time consuming operation.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_profiling_info",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_profiling_info(
            IntPtr infer_request,
            ref ov_profiling_info_list_t profiling_infos);

        /// <summary>
        /// Release the memory allocated by ov_profiling_info_list_t.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_profiling_info_list_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_profiling_info_list_free(ref ov_profiling_info_list_t profiling_infos);

        #endregion
    }

    /// <summary>
    /// Callback structure for async inference
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ov_callback_t
    {
        /// <summary>
        /// Callback function pointer
        /// </summary>
        public IntPtr callback_func;
        /// <summary>
        /// Arguments for callback
        /// </summary>
        public IntPtr args;
    }

    /// <summary>
    /// Profiling information for a node
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ov_profiling_info_t
    {
        public ProfilingStatus status;
        public long real_time;
        public long cpu_time;
        public IntPtr node_name;
        public IntPtr exec_type;
        public IntPtr node_type;
    }

    /// <summary>
    /// Profiling status enum
    /// </summary>
    public enum ProfilingStatus : int
    {
        NOT_RUN = 0,
        OPTIMIZED_OUT = 1,
        EXECUTED = 2
    }

    /// <summary>
    /// List of profiling information
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ov_profiling_info_list_t
    {
        public IntPtr profiling_infos;
        public ulong size;
    }
}
