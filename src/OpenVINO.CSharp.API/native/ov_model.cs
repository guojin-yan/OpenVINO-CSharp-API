// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        #region Model Destruction

        /// <summary>
        /// 释放 ov_model_t 分配的内存 / Release the memory allocated by ov_model_t
        /// </summary>
        /// <param name="model">模型指针 / Model pointer</param>
        [DllImport("openvino_c", EntryPoint = "ov_model_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_model_free(IntPtr model);

        #endregion

        #region Input Size

        /// <summary>
        /// 获取 ov_model_t 的输入数量 / Get the input size of ov_model_t
        /// </summary>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <param name="size">返回的输入数量 / Returned input size</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_model_inputs_size",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_inputs_size(IntPtr model, ref ulong size);

        /// <summary>
        /// 获取 ov_model_t 的输出数量 / Get the output size of ov_model_t
        /// </summary>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <param name="size">返回的输出数量 / Returned output size</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_model_outputs_size",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_outputs_size(IntPtr model, ref ulong size);

        #endregion

        #region Const Input Ports

        /// <summary>
        /// 获取 ov_model_t 的常量输入端口（仅支持单输入模型）/ Get a const input port of ov_model_t, which only support single input model
        /// </summary>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <param name="input_port">返回的输入端口指针 / Returned input port pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_model_const_input",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_const_input(IntPtr model, ref IntPtr input_port);

        /// <summary>
        /// 通过名称获取 ov_model_t 的常量输入端口 / Get a const input port of ov_model_t by name
        /// </summary>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <param name="tensor_name">张量名称 / Tensor name</param>
        /// <param name="input_port">返回的输入端口指针 / Returned input port pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_model_const_input_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_const_input_by_name(
            IntPtr model,
            [MarshalAs(UnmanagedType.LPStr)] string tensor_name,
            ref IntPtr input_port);

        /// <summary>
        /// 通过端口索引获取 ov_model_t 的常量输入端口 / Get a const input port of ov_model_t by port index
        /// </summary>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <param name="index">端口索引 / Port index</param>
        /// <param name="input_port">返回的输入端口指针 / Returned input port pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_model_const_input_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_const_input_by_index(
            IntPtr model,
            ulong index,
            ref IntPtr input_port);

        #endregion

        #region Input Ports (Non-const)

        /// <summary>
        /// 获取 ov_model_t 的单个输入端口（仅支持单输入模型）/ Get single input port of ov_model_t, which only support single input model
        /// </summary>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <param name="input_port">返回的输入端口指针 / Returned input port pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_model_input",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_input(IntPtr model, ref IntPtr input_port);

        /// <summary>
        /// 通过名称获取 ov_model_t 的输入端口 / Get an input port of ov_model_t by name
        /// </summary>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <param name="tensor_name">张量名称 / Tensor name</param>
        /// <param name="input_port">返回的输入端口指针 / Returned input port pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_model_input_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_input_by_name(
            IntPtr model,
            [MarshalAs(UnmanagedType.LPStr)] string tensor_name,
            ref IntPtr input_port);

        /// <summary>
        /// 通过端口索引获取 ov_model_t 的输入端口 / Get an input port of ov_model_t by port index
        /// </summary>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <param name="index">端口索引 / Port index</param>
        /// <param name="input_port">返回的输入端口指针 / Returned input port pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_model_input_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_input_by_index(
            IntPtr model,
            ulong index,
            ref IntPtr input_port);

        #endregion

        #region Const Output Ports

        /// <summary>
        /// 获取 ov_model_t 的单个常量输出端口（仅支持单输出模型）/ Get a single const output port of ov_model_t, which only support single output model
        /// </summary>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <param name="output_port">返回的输出端口指针 / Returned output port pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_model_const_output",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_const_output(IntPtr model, ref IntPtr output_port);

        /// <summary>
        /// 通过端口索引获取 ov_model_t 的常量输出端口 / Get a const output port of ov_model_t by port index
        /// </summary>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <param name="index">端口索引 / Port index</param>
        /// <param name="output_port">返回的输出端口指针 / Returned output port pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_model_const_output_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_const_output_by_index(
            IntPtr model,
            ulong index,
            ref IntPtr output_port);

        /// <summary>
        /// 通过名称获取 ov_model_t 的常量输出端口 / Get a const output port of ov_model_t by name
        /// </summary>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <param name="tensor_name">张量名称 / Tensor name</param>
        /// <param name="output_port">返回的输出端口指针 / Returned output port pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_model_const_output_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_const_output_by_name(
            IntPtr model,
            [MarshalAs(UnmanagedType.LPStr)] string tensor_name,
            ref IntPtr output_port);

        #endregion

        #region Output Ports (Non-const)

        /// <summary>
        /// 获取 ov_model_t 的单个输出端口（仅支持单输出模型）/ Get a single output port of ov_model_t, which only support single output model
        /// </summary>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <param name="output_port">返回的输出端口指针 / Returned output port pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_model_output",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_output(IntPtr model, ref IntPtr output_port);

        /// <summary>
        /// 通过端口索引获取 ov_model_t 的输出端口 / Get an output port of ov_model_t by port index
        /// </summary>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <param name="index">端口索引 / Port index</param>
        /// <param name="output_port">返回的输出端口指针 / Returned output port pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_model_output_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_output_by_index(
            IntPtr model,
            ulong index,
            ref IntPtr output_port);

        /// <summary>
        /// 通过名称获取 ov_model_t 的输出端口 / Get an output port of ov_model_t by name
        /// </summary>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <param name="tensor_name">张量名称 / Tensor name</param>
        /// <param name="output_port">返回的输出端口指针 / Returned output port pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_model_output_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_output_by_name(
            IntPtr model,
            [MarshalAs(UnmanagedType.LPStr)] string tensor_name,
            ref IntPtr output_port);

        #endregion

        #region Model Properties

        /// <summary>
        /// 获取模型的友好名称 / Gets the friendly name for a model
        /// </summary>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <param name="friendly_name">返回的友好名称指针 / Returned friendly name pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_model_get_friendly_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_get_friendly_name(IntPtr model, ref IntPtr friendly_name);

        /// <summary>
        /// 检查模型中是否有动态形状的算子 / Returns true if any of the ops defined in the model is dynamic shape
        /// </summary>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <returns>如果有动态形状返回 true / True if dynamic shape exists</returns>
        [DllImport("openvino_c", EntryPoint = "ov_model_is_dynamic",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool ov_model_is_dynamic(IntPtr model);

        #endregion

        #region Reshape Methods

        /// <summary>
        /// 使用名称和部分形状列表对模型进行 reshape / Do reshape in model with a list of &lt;name, partial shape&gt;
        /// </summary>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <param name="tensor_names">张量名称数组 / Tensor names array</param>
        /// <param name="partial_shapes">部分形状数组 / Partial shapes array</param>
        /// <param name="size">数组大小 / Array size</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_model_reshape",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_reshape(
            IntPtr model,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] tensor_names,
            [MarshalAs(UnmanagedType.LPArray)] ov_partial_shape_t[] partial_shapes,
            ulong size);

        /// <summary>
        /// 为指定名称的输入进行 reshape / Do reshape in model with partial shape for a specified name
        /// </summary>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <param name="tensor_name">张量名称 / Tensor name</param>
        /// <param name="partial_shape">部分形状 / Partial shape</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_model_reshape_input_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_reshape_input_by_name(
            IntPtr model,
            [MarshalAs(UnmanagedType.LPStr)] string tensor_name,
            ov_partial_shape_t partial_shape);

        /// <summary>
        /// 对模型的单个输入进行 reshape（端口 0）/ Do reshape in model for one node(port 0)
        /// </summary>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <param name="partial_shape">部分形状 / Partial shape</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_model_reshape_single_input",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_reshape_single_input(
            IntPtr model,
            ov_partial_shape_t partial_shape);

        /// <summary>
        /// 使用端口索引和部分形状列表对模型进行 reshape / Do reshape in model with a list of &lt;port id, partial shape&gt;
        /// </summary>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <param name="port_indexes">端口索引数组 / Port indexes array</param>
        /// <param name="partial_shapes">部分形状数组 / Partial shapes array</param>
        /// <param name="size">数组大小 / Array size</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_model_reshape_by_port_indexes",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_reshape_by_port_indexes(
            IntPtr model,
            [MarshalAs(UnmanagedType.LPArray)] ulong[] port_indexes,
            [MarshalAs(UnmanagedType.LPArray)] ov_partial_shape_t[] partial_shapes,
            ulong size);

        /// <summary>
        /// 使用输出端口和部分形状列表对模型进行 reshape / Do reshape in model with a list of &lt;ov_output_port_t, partial shape&gt;
        /// </summary>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <param name="output_ports">输出端口数组 / Output ports array</param>
        /// <param name="partial_shapes">部分形状数组 / Partial shapes array</param>
        /// <param name="size">数组大小 / Array size</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_model_reshape_by_ports",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_reshape_by_ports(
            IntPtr model,
            [MarshalAs(UnmanagedType.LPArray)] IntPtr[] output_ports,
            [MarshalAs(UnmanagedType.LPArray)] ov_partial_shape_t[] partial_shapes,
            ulong size);

        #endregion
    }
}
