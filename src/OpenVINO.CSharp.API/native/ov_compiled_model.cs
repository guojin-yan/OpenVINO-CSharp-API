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

        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_inputs_size",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_compiled_model_inputs_size_native_size(IntPtr compiled_model, ref UIntPtr size);

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

        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_input_by_index",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_compiled_model_input_by_index_native_size(
            IntPtr compiled_model,
            UIntPtr index,
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

        /// <summary>
        /// 通过 UTF-8 名称获取编译模型输入端口 / Get compiled model input by UTF-8 tensor name.
        /// </summary>
        /// <param name="compiled_model">编译模型指针 / Compiled model pointer.</param>
        /// <param name="tensor_name">UTF-8 名称指针 / UTF-8 name pointer.</param>
        /// <param name="input_port">返回的输入端口 / Returned input port.</param>
        /// <returns>操作状态 / Operation status.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_input_by_name",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_compiled_model_input_by_name_utf8(
            IntPtr compiled_model,
            IntPtr tensor_name,
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

        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_outputs_size",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_compiled_model_outputs_size_native_size(IntPtr compiled_model, ref UIntPtr size);

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

        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_output_by_index",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_compiled_model_output_by_index_native_size(
            IntPtr compiled_model,
            UIntPtr index,
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

        /// <summary>
        /// 通过 UTF-8 名称获取编译模型输出端口 / Get compiled model output by UTF-8 tensor name.
        /// </summary>
        /// <param name="compiled_model">编译模型指针 / Compiled model pointer.</param>
        /// <param name="tensor_name">UTF-8 名称指针 / UTF-8 name pointer.</param>
        /// <param name="output_port">返回的输出端口 / Returned output port.</param>
        /// <returns>操作状态 / Operation status.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_output_by_name",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_compiled_model_output_by_name_utf8(
            IntPtr compiled_model,
            IntPtr tensor_name,
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
        /// 使用 UTF-8 文件路径导出编译模型 / Export compiled model to a UTF-8 file path.
        /// </summary>
        /// <param name="compiled_model">编译模型指针 / Compiled model pointer.</param>
        /// <param name="export_model_path">UTF-8 路径指针 / UTF-8 path pointer.</param>
        /// <returns>操作状态 / Operation status.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_export_model",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_compiled_model_export_model_utf8(
            IntPtr compiled_model,
            IntPtr export_model_path);

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
        /// 使用原生 size_t 设置编译模型属性 / Set compiled model property using native size_t.
        /// </summary>
        /// <param name="compiled_model">编译模型指针 / Compiled model pointer.</param>
        /// <param name="property_args_size">属性参数数量 / Property argument count.</param>
        /// <param name="property_key">属性键 / Property key.</param>
        /// <param name="property_value">属性值 / Property value.</param>
        /// <returns>操作状态 / Operation status.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_set_property",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_compiled_model_set_property_native_size(
            IntPtr compiled_model,
            UIntPtr property_args_size,
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

        /// <summary>
        /// 使用 UTF-8 属性键获取编译模型属性 / Get compiled model property by UTF-8 key.
        /// </summary>
        /// <param name="compiled_model">编译模型指针 / Compiled model pointer.</param>
        /// <param name="property_key">UTF-8 属性键指针 / UTF-8 property key pointer.</param>
        /// <param name="property_value">返回的属性值指针 / Returned property value pointer.</param>
        /// <returns>操作状态 / Operation status.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_get_property",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_compiled_model_get_property_utf8(
            IntPtr compiled_model,
            IntPtr property_key,
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
