//  ========================================================================
//  【项目名称】OpenVINO C# API
//  【项目描述】OpenVINO™ 的 C# 语言绑定库，提供高性能深度学习推理能力
//  【版权声明】© 2026-2025 Guojin Yan. All Rights Reserved.
//  【开源协议】Apache-2.0 License（请遵守许可证条款）
//  -----------------------------------------------------------------------
//  【功能简介】
//  1. 完整的 OpenVINO™ C API 封装，提供 C# 友好的面向对象接口。
//  2. 支持模型加载、编译、推理全流程操作。
//  3. 支持 CPU、GPU、VPU 等多种推理设备。
//  4. 支持同步推理和异步推理模式。
//  5. 支持预处理和后处理流水线配置。
//  6. 支持动态形状和批量推理。
//  7. 支持模型缓存和性能分析。
//  8. 支持远程上下文（Remote Context）和零拷贝推理。
//  9. 支持 .NET Framework 4.6.1+、.NET Core 2.0+、.NET 5/6/7/8/9+。
//  10. 提供推理请求对象池，优化高并发场景性能。
//  11. 提供完善的异常处理和日志记录机制。
//  12. 提供丰富的单元测试和集成测试用例。
//  -----------------------------------------------------------------------
//  【官方资源】
//  📌 GitHub仓库：https://github.com/guojin-yan/OpenVINO-CSharp-API
//  📌 NuGet包：https://www.nuget.org/packages/OpenVINO.CSharp.API
//  📌 在线文档：https://guojin-yan.github.io/OpenVINO-CSharp-API/index.html
//  📌 示例代码：https://github.com/guojin-yan/OpenVINO-CSharp-API/tree/csharp3.3/samples
//  -----------------------------------------------------------------------
//  【社区支持】
//  💬 QQ交流群：945057948（加入获取技术支持）
//  📱 微信公众号：CSharp与边缘模型部署（教程+案例）
//  📝 CSDN博客：https://guojin.blog.csdn.net（技术文章）
//  -----------------------------------------------------------------------
//  【联系我们】
//  ✉ 项目维护：guojin_yjs@cumt.edu.cn
//  💬 微信咨询：15253793309
//  🐛 Bug反馈：https://github.com/guojin-yan/OpenVINO-CSharp-API/issues
//  💡 功能建议：https://github.com/guojin-yan/OpenVINO-CSharp-API/discussions/landing
//  -----------------------------------------------------------------------
//  【致谢】
//  本项目基于 Intel® OpenVINO™ 工具包开发，感谢 Intel 提供的优秀开源项目。
//  OpenVINO™ 是 Intel Corporation 的商标。
//  ========================================================================
//  
//  【许可声明】
//  1. 本项目采用 Apache-2.0 License 开源协议，允许自由使用、修改和分发。
//  2. 使用本项目即表示您同意 Apache-2.0 License 许可证的所有条款。
//  3. 本项目按"原样"提供，不提供任何形式的担保。
//  4. 使用本项目产生的任何风险由使用者自行承担。
//  5. 修改或分发时请保留原始版权声明和许可声明。
//  ========================================================================
//

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

        /// <summary>
        /// 通过 UTF-8 张量名称设置输入/输出张量 / Set an input/output tensor by UTF-8 tensor name.
        /// </summary>
        /// <param name="infer_request">推理请求指针 / Inference request pointer.</param>
        /// <param name="tensor_name">UTF-8 名称指针 / UTF-8 name pointer.</param>
        /// <param name="tensor">张量指针 / Tensor pointer.</param>
        /// <returns>操作状态 / Operation status.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_tensor",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_infer_request_set_tensor_utf8(
            IntPtr infer_request,
            IntPtr tensor_name,
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

        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_input_tensor_by_index",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_infer_request_set_input_tensor_by_index_native_size(
            IntPtr infer_request,
            UIntPtr idx,
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

        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_output_tensor_by_index",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_infer_request_set_output_tensor_by_index_native_size(
            IntPtr infer_request,
            UIntPtr idx,
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

        /// <summary>
        /// 通过 UTF-8 张量名称获取输入/输出张量 / Get an input/output tensor by UTF-8 tensor name.
        /// </summary>
        /// <param name="infer_request">推理请求指针 / Inference request pointer.</param>
        /// <param name="tensor_name">UTF-8 名称指针 / UTF-8 name pointer.</param>
        /// <param name="tensor">返回的张量指针 / Returned tensor pointer.</param>
        /// <returns>操作状态 / Operation status.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_tensor",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_infer_request_get_tensor_utf8(
            IntPtr infer_request,
            IntPtr tensor_name,
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

        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_input_tensor_by_index",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_infer_request_get_input_tensor_by_index_native_size(
            IntPtr infer_request,
            UIntPtr idx,
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

        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_output_tensor_by_index",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_infer_request_get_output_tensor_by_index_native_size(
            IntPtr infer_request,
            UIntPtr idx,
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

        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_profiling_info",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_infer_request_get_profiling_info_native(
            IntPtr infer_request,
            ref ov_profiling_info_list_native_t profiling_infos);

        /// <summary>
        /// 释放 ov_profiling_info_list_t 分配的内存 / Release the memory allocated by ov_profiling_info_list_t
        /// </summary>
        /// <param name="profiling_infos">性能分析信息列表指针 / Profiling information list pointer</param>
        [DllImport("openvino_c", EntryPoint = "ov_profiling_info_list_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_profiling_info_list_free(ref ov_profiling_info_list_t profiling_infos);

        [DllImport("openvino_c", EntryPoint = "ov_profiling_info_list_free",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static void ov_profiling_info_list_free_native(ref ov_profiling_info_list_native_t profiling_infos);

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

    [StructLayout(LayoutKind.Sequential)]
    internal struct ov_profiling_info_list_native_t
    {
        public IntPtr profiling_infos;
        public UIntPtr size;
    }
}
