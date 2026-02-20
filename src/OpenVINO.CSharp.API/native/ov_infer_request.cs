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
        /// 释放 ov_infer_request_t 分配的内存 / Release the memory allocated by ov_infer_request_t
        /// </summary>
        /// <param name="infer_request">推理请求指针 / Inference request pointer</param>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_infer_request_free(IntPtr infer_request);

        #endregion

        #region Set Tensor by Port

        /// <summary>
        /// 为端口设置输入/输出张量到推理请求 / Set an input/output tensor to infer request for the port
        /// </summary>
        /// <param name="infer_request">推理请求指针 / Inference request pointer</param>
        /// <param name="port">端口指针 / Port pointer</param>
        /// <param name="tensor">张量指针 / Tensor pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_tensor_by_port",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_tensor_by_port(
            IntPtr infer_request,
            IntPtr port,
            IntPtr tensor);

        /// <summary>
        /// 为常量端口设置输入/输出张量到推理请求 / Set an input/output tensor to infer request for the const port
        /// </summary>
        /// <param name="infer_request">推理请求指针 / Inference request pointer</param>
        /// <param name="const_port">常量端口指针 / Const port pointer</param>
        /// <param name="tensor">张量指针 / Tensor pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_tensor_by_const_port",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_tensor_by_const_port(
            IntPtr infer_request,
            IntPtr const_port,
            IntPtr tensor);

        /// <summary>
        /// 通过张量名称设置输入/输出张量到推理请求 / Set an input/output tensor to infer on by the name of tensor
        /// </summary>
        /// <param name="infer_request">推理请求指针 / Inference request pointer</param>
        /// <param name="tensor_name">张量名称 / Tensor name</param>
        /// <param name="tensor">张量指针 / Tensor pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_tensor",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_tensor(
            IntPtr infer_request,
            string tensor_name,
            IntPtr tensor);

        #endregion

        #region Set Input Tensor

        /// <summary>
        /// 通过张量索引设置输入张量到推理请求 / Set an input tensor to infer on by the index of tensor
        /// </summary>
        /// <param name="infer_request">推理请求指针 / Inference request pointer</param>
        /// <param name="idx">张量索引 / Tensor index</param>
        /// <param name="tensor">张量指针 / Tensor pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_input_tensor_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_input_tensor_by_index(
            IntPtr infer_request,
            ulong idx,
            IntPtr tensor);

        /// <summary>
        /// 为单输入模型设置输入张量 / Set an input tensor for the model with single input to infer on
        /// </summary>
        /// <param name="infer_request">推理请求指针 / Inference request pointer</param>
        /// <param name="tensor">张量指针 / Tensor pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_input_tensor",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_input_tensor(
            IntPtr infer_request,
            IntPtr tensor);

        #endregion

        #region Set Output Tensor

        /// <summary>
        /// 通过输出张量索引设置输出张量 / Set an output tensor to infer by the index of output tensor
        /// </summary>
        /// <param name="infer_request">推理请求指针 / Inference request pointer</param>
        /// <param name="idx">输出张量索引 / Output tensor index</param>
        /// <param name="tensor">张量指针 / Tensor pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_output_tensor_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_output_tensor_by_index(
            IntPtr infer_request,
            ulong idx,
            IntPtr tensor);

        /// <summary>
        /// 为单输出模型设置输出张量 / Set an output tensor to infer models with single output
        /// </summary>
        /// <param name="infer_request">推理请求指针 / Inference request pointer</param>
        /// <param name="tensor">张量指针 / Tensor pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_output_tensor",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_output_tensor(
            IntPtr infer_request,
            IntPtr tensor);

        #endregion

        #region Get Tensor by Name

        /// <summary>
        /// 通过张量名称获取输入/输出张量 / Get an input/output tensor by the name of tensor
        /// </summary>
        /// <param name="infer_request">推理请求指针 / Inference request pointer</param>
        /// <param name="tensor_name">张量名称 / Tensor name</param>
        /// <param name="tensor">返回的张量指针 / Returned tensor pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_tensor",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_tensor(
            IntPtr infer_request,
            string tensor_name,
            ref IntPtr tensor);

        #endregion

        #region Get Tensor by Port

        /// <summary>
        /// 通过端口获取输入/输出张量 / Get an input/output tensor by port
        /// </summary>
        /// <param name="infer_request">推理请求指针 / Inference request pointer</param>
        /// <param name="port">端口指针 / Port pointer</param>
        /// <param name="tensor">返回的张量指针 / Returned tensor pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_tensor_by_port",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_tensor_by_port(
            IntPtr infer_request,
            IntPtr port,
            ref IntPtr tensor);

        /// <summary>
        /// 通过常量端口获取输入/输出张量 / Get an input/output tensor by const port
        /// </summary>
        /// <param name="infer_request">推理请求指针 / Inference request pointer</param>
        /// <param name="const_port">常量端口指针 / Const port pointer</param>
        /// <param name="tensor">返回的张量指针 / Returned tensor pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_tensor_by_const_port",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_tensor_by_const_port(
            IntPtr infer_request,
            IntPtr const_port,
            ref IntPtr tensor);

        #endregion

        #region Get Input Tensor

        /// <summary>
        /// 通过输入张量索引获取输入张量 / Get an input tensor by the index of input tensor
        /// </summary>
        /// <param name="infer_request">推理请求指针 / Inference request pointer</param>
        /// <param name="idx">输入张量索引 / Input tensor index</param>
        /// <param name="tensor">返回的张量指针 / Returned tensor pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_input_tensor_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_input_tensor_by_index(
            IntPtr infer_request,
            ulong idx,
            ref IntPtr tensor);

        /// <summary>
        /// 从单输入模型获取输入张量 / Get an input tensor from the model with only one input tensor
        /// </summary>
        /// <param name="infer_request">推理请求指针 / Inference request pointer</param>
        /// <param name="tensor">返回的张量指针 / Returned tensor pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_input_tensor",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_input_tensor(
            IntPtr infer_request,
            ref IntPtr tensor);

        #endregion

        #region Get Output Tensor

        /// <summary>
        /// 通过输出张量索引获取输出张量 / Get an output tensor by the index of output tensor
        /// </summary>
        /// <param name="infer_request">推理请求指针 / Inference request pointer</param>
        /// <param name="idx">输出张量索引 / Output tensor index</param>
        /// <param name="tensor">返回的张量指针 / Returned tensor pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_output_tensor_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_output_tensor_by_index(
            IntPtr infer_request,
            ulong idx,
            ref IntPtr tensor);

        /// <summary>
        /// 从单输出模型获取输出张量 / Get an output tensor from the model with only one output tensor
        /// </summary>
        /// <param name="infer_request">推理请求指针 / Inference request pointer</param>
        /// <param name="tensor">返回的张量指针 / Returned tensor pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_output_tensor",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_output_tensor(
            IntPtr infer_request,
            ref IntPtr tensor);

        #endregion

        #region Inference Execution

        /// <summary>
        /// 以同步模式执行推理 / Infer specified input(s) in synchronous mode
        /// </summary>
        /// <param name="infer_request">推理请求指针 / Inference request pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        /// <remarks>
        /// 此函数会阻塞直到推理完成。
        /// This function blocks until inference is complete.
        /// </remarks>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_infer",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_infer(IntPtr infer_request);

        /// <summary>
        /// 取消推理请求 / Cancel inference request
        /// </summary>
        /// <param name="infer_request">推理请求指针 / Inference request pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_cancel",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_cancel(IntPtr infer_request);

        /// <summary>
        /// 以异步模式开始推理 / Start inference of specified input(s) in asynchronous mode
        /// </summary>
        /// <param name="infer_request">推理请求指针 / Inference request pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        /// <remarks>
        /// 使用 ov_infer_request_wait 或 ov_infer_request_wait_for 等待推理完成。
        /// Use ov_infer_request_wait or ov_infer_request_wait_for to wait for completion.
        /// </remarks>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_start_async",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_start_async(IntPtr infer_request);

        /// <summary>
        /// 等待推理结果可用 / Wait for the result to become available
        /// </summary>
        /// <param name="infer_request">推理请求指针 / Inference request pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_wait",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_wait(IntPtr infer_request);

        /// <summary>
        /// 等待推理结果可用（带超时）/ Waits for the result to become available with timeout
        /// </summary>
        /// <param name="infer_request">推理请求指针 / Inference request pointer</param>
        /// <param name="timeout">超时时间（毫秒）/ Timeout in milliseconds</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_wait_for",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_wait_for(
            IntPtr infer_request,
            long timeout);

        #endregion

        #region Callback

        /// <summary>
        /// 设置回调函数，推理完成时调用 / Set callback function, which will be called when inference is done
        /// </summary>
        /// <param name="infer_request">推理请求指针 / Inference request pointer</param>
        /// <param name="callback">回调结构体指针 / Callback structure pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_callback",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_callback(
            IntPtr infer_request,
            ref ov_callback_t callback);

        /// <summary>
        /// 异步推理完成的回调委托 / Callback delegate for async inference completion
        /// </summary>
        /// <param name="args">回调参数 / Callback arguments</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ov_infer_request_callback_func(IntPtr args);

        #endregion

        #region Profiling Info

        /// <summary>
        /// 查询每层性能测量以识别最耗时的操作 / Query performance measures per layer to identify the most time consuming operation
        /// </summary>
        /// <param name="infer_request">推理请求指针 / Inference request pointer</param>
        /// <param name="profiling_infos">返回的性能分析信息列表 / Returned profiling information list</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_profiling_info",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_profiling_info(
            IntPtr infer_request,
            ref ov_profiling_info_list_t profiling_infos);

        /// <summary>
        /// 释放 ov_profiling_info_list_t 分配的内存 / Release the memory allocated by ov_profiling_info_list_t
        /// </summary>
        /// <param name="profiling_infos">性能分析信息列表指针 / Profiling information list pointer</param>
        [DllImport("openvino_c", EntryPoint = "ov_profiling_info_list_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_profiling_info_list_free(ref ov_profiling_info_list_t profiling_infos);

        #endregion
    }

    /// <summary>
    /// 异步推理的回调结构体 / Callback structure for async inference
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ov_callback_t
    {
        /// <summary>
        /// 回调函数指针 / Callback function pointer
        /// </summary>
        public IntPtr callback_func;
        /// <summary>
        /// 回调参数 / Arguments for callback
        /// </summary>
        public IntPtr args;
    }

    /// <summary>
    /// 节点的性能分析信息 / Profiling information for a node
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ov_profiling_info_t
    {
        /// <summary>
        /// 性能分析状态 / Profiling status
        /// </summary>
        public ProfilingStatus status;
        /// <summary>
        /// 实际时间（纳秒）/ Real time in nanoseconds
        /// </summary>
        public long real_time;
        /// <summary>
        /// CPU 时间（纳秒）/ CPU time in nanoseconds
        /// </summary>
        public long cpu_time;
        /// <summary>
        /// 节点名称指针 / Node name pointer
        /// </summary>
        public IntPtr node_name;
        /// <summary>
        /// 执行类型指针 / Execution type pointer
        /// </summary>
        public IntPtr exec_type;
        /// <summary>
        /// 节点类型指针 / Node type pointer
        /// </summary>
        public IntPtr node_type;
    }

    /// <summary>
    /// 性能分析状态枚举 / Profiling status enum
    /// </summary>
    public enum ProfilingStatus : int
    {
        /// <summary>
        /// 未运行 / Not run
        /// </summary>
        NOT_RUN = 0,
        /// <summary>
        /// 已优化掉 / Optimized out
        /// </summary>
        OPTIMIZED_OUT = 1,
        /// <summary>
        /// 已执行 / Executed
        /// </summary>
        EXECUTED = 2
    }

    /// <summary>
    /// 性能分析信息列表 / List of profiling information
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ov_profiling_info_list_t
    {
        /// <summary>
        /// 性能分析信息数组指针 / Profiling info array pointer
        /// </summary>
        public IntPtr profiling_infos;
        /// <summary>
        /// 信息数量 / Number of infos
        /// </summary>
        public ulong size;
    }
}
