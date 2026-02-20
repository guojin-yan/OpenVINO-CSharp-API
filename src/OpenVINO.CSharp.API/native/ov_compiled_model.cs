// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        #region Compiled Model Destruction

        /// <summary>
        /// 释放 ov_compiled_model_t 分配的内存 / Release the memory allocated by ov_compiled_model_t
        /// </summary>
        /// <param name="compiled_model">编译模型指针 / Compiled model pointer</param>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_compiled_model_free(IntPtr compiled_model);

        #endregion

        #region Input Methods

        /// <summary>
        /// 获取 ov_compiled_model_t 的输入数量 / Get the input size of ov_compiled_model_t
        /// </summary>
        /// <param name="compiled_model">编译模型指针 / Compiled model pointer</param>
        /// <param name="size">返回的输入数量 / Returned input size</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_inputs_size",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_inputs_size(IntPtr compiled_model, ref ulong size);

        /// <summary>
        /// 获取 ov_compiled_model_t 的单个常量输入端口 / Get the single const input port of ov_compiled_model_t
        /// </summary>
        /// <param name="compiled_model">编译模型指针 / Compiled model pointer</param>
        /// <param name="input_port">返回的输入端口指针 / Returned input port pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_input",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_input(IntPtr compiled_model, ref IntPtr input_port);

        /// <summary>
        /// 通过端口索引获取 ov_compiled_model_t 的常量输入端口 / Get the const input port of ov_compiled_model_t by port index
        /// </summary>
        /// <param name="compiled_model">编译模型指针 / Compiled model pointer</param>
        /// <param name="index">端口索引 / Port index</param>
        /// <param name="input_port">返回的输入端口指针 / Returned input port pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_input_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_input_by_index(
            IntPtr compiled_model,
            ulong index,
            ref IntPtr input_port);

        /// <summary>
        /// 通过名称获取 ov_compiled_model_t 的常量输入端口 / Get the const input port of ov_compiled_model_t by name
        /// </summary>
        /// <param name="compiled_model">编译模型指针 / Compiled model pointer</param>
        /// <param name="tensor_name">张量名称 / Tensor name</param>
        /// <param name="input_port">返回的输入端口指针 / Returned input port pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_input_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_input_by_name(
            IntPtr compiled_model,
            [MarshalAs(UnmanagedType.LPStr)] string tensor_name,
            ref IntPtr input_port);

        #endregion

        #region Output Methods

        /// <summary>
        /// 获取 ov_compiled_model_t 的输出数量 / Get the output size of ov_compiled_model_t
        /// </summary>
        /// <param name="compiled_model">编译模型指针 / Compiled model pointer</param>
        /// <param name="size">返回的输出数量 / Returned output size</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_outputs_size",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_outputs_size(IntPtr compiled_model, ref ulong size);

        /// <summary>
        /// 获取 ov_compiled_model_t 的单个常量输出端口 / Get the single const output port of ov_compiled_model_t
        /// </summary>
        /// <param name="compiled_model">编译模型指针 / Compiled model pointer</param>
        /// <param name="output_port">返回的输出端口指针 / Returned output port pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_output",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_output(IntPtr compiled_model, ref IntPtr output_port);

        /// <summary>
        /// 通过端口索引获取 ov_compiled_model_t 的常量输出端口 / Get the const output port of ov_compiled_model_t by port index
        /// </summary>
        /// <param name="compiled_model">编译模型指针 / Compiled model pointer</param>
        /// <param name="index">端口索引 / Port index</param>
        /// <param name="output_port">返回的输出端口指针 / Returned output port pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_output_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_output_by_index(
            IntPtr compiled_model,
            ulong index,
            ref IntPtr output_port);

        /// <summary>
        /// 通过名称获取 ov_compiled_model_t 的常量输出端口 / Get the const output port of ov_compiled_model_t by name
        /// </summary>
        /// <param name="compiled_model">编译模型指针 / Compiled model pointer</param>
        /// <param name="tensor_name">张量名称 / Tensor name</param>
        /// <param name="output_port">返回的输出端口指针 / Returned output port pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_output_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_output_by_name(
            IntPtr compiled_model,
            [MarshalAs(UnmanagedType.LPStr)] string tensor_name,
            ref IntPtr output_port);

        #endregion

        #region Runtime Model and Inference Request

        /// <summary>
        /// 从设备获取运行时模型信息 / Gets runtime model information from a device
        /// </summary>
        /// <param name="compiled_model">编译模型指针 / Compiled model pointer</param>
        /// <param name="model">返回的模型指针 / Returned model pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_get_runtime_model",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_get_runtime_model(
            IntPtr compiled_model,
            ref IntPtr model);

        /// <summary>
        /// 创建推理请求对象 / Creates an inference request object
        /// </summary>
        /// <param name="compiled_model">编译模型指针 / Compiled model pointer</param>
        /// <param name="infer_request">返回的推理请求指针 / Returned inference request pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_create_infer_request",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_create_infer_request(
            IntPtr compiled_model,
            ref IntPtr infer_request);

        #endregion

        #region Export and Properties

        /// <summary>
        /// 将编译模型导出到指定文件路径 / Exports the compiled model to the specified file path
        /// </summary>
        /// <param name="compiled_model">编译模型指针 / Compiled model pointer</param>
        /// <param name="export_model_path">导出模型文件路径 / Export model file path</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_export_model",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_export_model(
            IntPtr compiled_model,
            [MarshalAs(UnmanagedType.LPStr)] string export_model_path);

        /// <summary>
        /// 为编译模型设置属性 / Sets properties for the compiled model
        /// </summary>
        /// <param name="compiled_model">编译模型指针 / Compiled model pointer</param>
        /// <param name="property_args_size">属性参数数量 / Property arguments size</param>
        /// <param name="property_key">属性键 / Property key</param>
        /// <param name="property_value">属性值 / Property value</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_set_property",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_set_property(
            IntPtr compiled_model,
            ulong property_args_size,
            IntPtr property_key,
            IntPtr property_value);

        /// <summary>
        /// 获取编译模型的属性 / Gets properties for the compiled model
        /// </summary>
        /// <param name="compiled_model">编译模型指针 / Compiled model pointer</param>
        /// <param name="property_key">属性键 / Property key</param>
        /// <param name="property_value">返回的属性值指针 / Returned property value pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_get_property",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_get_property(
            IntPtr compiled_model,
            [MarshalAs(UnmanagedType.LPStr)] string property_key,
            ref IntPtr property_value);

        #endregion

        #region Remote Context

        /// <summary>
        /// 返回指向设备特定共享上下文的指针 / Returns pointer to device-specific shared context
        /// </summary>
        /// <param name="compiled_model">编译模型指针 / Compiled model pointer</param>
        /// <param name="context">返回的上下文指针 / Returned context pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_get_context",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_get_context(
            IntPtr compiled_model,
            ref IntPtr context);

        #endregion
    }
}
