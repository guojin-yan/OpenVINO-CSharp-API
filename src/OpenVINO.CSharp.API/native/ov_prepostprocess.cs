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
        #region PrePostProcessor Creation and Destruction

        /// <summary>
        /// 为指定模型创建预处理/后处理配置句柄，返回的句柄由调用方释放。
        /// Creates a pre/postprocessor handle for the specified model. The returned handle is owned by the caller.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_prepostprocessor_create",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_prepostprocessor_create(IntPtr model, ref IntPtr preprocess);

        /// <summary>
        /// 释放预处理/后处理配置句柄。
        /// Releases a pre/postprocessor handle.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_prepostprocessor_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_preprocess_prepostprocessor_free(IntPtr preprocess);

        /// <summary>
        /// 将预处理/后处理配置应用到模型并返回新的模型句柄。
        /// Builds a model with the configured pre/postprocessing steps and returns a new model handle.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_prepostprocessor_build",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_prepostprocessor_build(IntPtr preprocess, ref IntPtr model);

        #endregion

        #region Input Info

        /// <summary>
        /// 获取默认输入信息句柄，返回的句柄由调用方释放。
        /// Gets the default input info handle. The returned handle is owned by the caller.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_prepostprocessor_get_input_info",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_prepostprocessor_get_input_info(IntPtr preprocess, ref IntPtr input_info);

        /// <summary>
        /// 按张量名称获取输入信息句柄。旧兼容入口按 ANSI 字符串编组。
        /// Gets an input info handle by tensor name. This legacy-compatible entry marshals the string as ANSI.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_prepostprocessor_get_input_info_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_prepostprocessor_get_input_info_by_name(IntPtr preprocess, string tensor_name, ref IntPtr input_info);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_prepostprocessor_get_input_info_by_name",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_preprocess_prepostprocessor_get_input_info_by_name_utf8(
            IntPtr preprocess,
            IntPtr tensor_name,
            ref IntPtr input_info);

        /// <summary>
        /// 按张量索引获取输入信息句柄。
        /// Gets an input info handle by tensor index.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_prepostprocessor_get_input_info_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_prepostprocessor_get_input_info_by_index(IntPtr preprocess, ulong tensor_index, ref IntPtr input_info);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_prepostprocessor_get_input_info_by_index",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_preprocess_prepostprocessor_get_input_info_by_index_native_size(
            IntPtr preprocess,
            UIntPtr tensor_index,
            ref IntPtr input_info);

        /// <summary>
        /// 释放输入信息句柄。
        /// Releases an input info handle.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_info_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_preprocess_input_info_free(IntPtr input_info);

        #endregion

        #region Input Tensor Info

        /// <summary>
        /// 从输入信息中获取输入张量信息句柄，返回的句柄由调用方释放。
        /// Gets an input tensor info handle from input info. The returned handle is owned by the caller.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_info_get_tensor_info",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_input_info_get_tensor_info(IntPtr input_info, ref IntPtr tensor_info);

        /// <summary>
        /// 释放输入张量信息句柄。
        /// Releases an input tensor info handle.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_tensor_info_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_preprocess_input_tensor_info_free(IntPtr tensor_info);

        /// <summary>
        /// 设置输入张量元素类型。
        /// Sets the input tensor element type.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_tensor_info_set_element_type",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_input_tensor_info_set_element_type(IntPtr tensor_info, uint element_type);

        /// <summary>
        /// 设置输入张量颜色格式。
        /// Sets the input tensor color format.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_tensor_info_set_color_format",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_input_tensor_info_set_color_format(IntPtr tensor_info, uint color_format);

        /// <summary>
        /// 设置输入张量颜色格式及其子平面名称。旧兼容入口按 ANSI 字符串数组指针传递。
        /// Sets the input tensor color format and sub-plane names. This legacy-compatible entry passes ANSI string pointers.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_tensor_info_set_color_format_with_subname",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_input_tensor_info_set_color_format_with_subname(
            IntPtr tensor_info,
            uint color_format,
            ulong sub_names_size,
            IntPtr sub_names);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_tensor_info_set_color_format_with_subname",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_preprocess_input_tensor_info_set_color_format_with_subname_utf8_1(
            IntPtr tensor_info,
            uint color_format,
            UIntPtr sub_names_size,
            IntPtr sub_name1);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_tensor_info_set_color_format_with_subname",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_preprocess_input_tensor_info_set_color_format_with_subname_utf8_2(
            IntPtr tensor_info,
            uint color_format,
            UIntPtr sub_names_size,
            IntPtr sub_name1,
            IntPtr sub_name2);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_tensor_info_set_color_format_with_subname",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_preprocess_input_tensor_info_set_color_format_with_subname_utf8_3(
            IntPtr tensor_info,
            uint color_format,
            UIntPtr sub_names_size,
            IntPtr sub_name1,
            IntPtr sub_name2,
            IntPtr sub_name3);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_tensor_info_set_color_format_with_subname",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_preprocess_input_tensor_info_set_color_format_with_subname_utf8_4(
            IntPtr tensor_info,
            uint color_format,
            UIntPtr sub_names_size,
            IntPtr sub_name1,
            IntPtr sub_name2,
            IntPtr sub_name3,
            IntPtr sub_name4);

        /// <summary>
        /// 设置输入张量布局，layout 句柄由调用方管理。
        /// Sets the input tensor layout. The layout handle lifetime is managed by the caller.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_tensor_info_set_layout",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_input_tensor_info_set_layout(IntPtr tensor_info, IntPtr layout);

        /// <summary>
        /// 设置输入张量的静态空间尺寸。
        /// Sets the static spatial shape of the input tensor.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_tensor_info_set_spatial_static_shape",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_input_tensor_info_set_spatial_static_shape(IntPtr tensor_info, ulong input_height, ulong input_width);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_tensor_info_set_spatial_static_shape",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_preprocess_input_tensor_info_set_spatial_static_shape_native_size(
            IntPtr tensor_info,
            UIntPtr input_height,
            UIntPtr input_width);

        /// <summary>
        /// 设置输入张量内存类型。旧兼容入口按 ANSI 字符串编组。
        /// Sets the input tensor memory type. This legacy-compatible entry marshals the string as ANSI.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_tensor_info_set_memory_type",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_input_tensor_info_set_memory_type(IntPtr tensor_info, string mem_type);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_tensor_info_set_memory_type",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_preprocess_input_tensor_info_set_memory_type_utf8(
            IntPtr tensor_info,
            IntPtr mem_type);

        /// <summary>
        /// 使用已有张量描述输入张量信息，tensor 句柄由调用方管理。
        /// Sets input tensor info from an existing tensor. The tensor handle lifetime is managed by the caller.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_tensor_info_set_from",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_input_tensor_info_set_from(IntPtr tensor_info, IntPtr tensor);

        #endregion

        #region Preprocess Steps

        /// <summary>
        /// 获取输入预处理步骤句柄，返回的句柄由调用方释放。
        /// Gets the input preprocess steps handle. The returned handle is owned by the caller.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_info_get_preprocess_steps",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_input_info_get_preprocess_steps(IntPtr input_info, ref IntPtr steps);

        /// <summary>
        /// 释放预处理步骤句柄。
        /// Releases a preprocess steps handle.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_preprocess_steps_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_preprocess_preprocess_steps_free(IntPtr steps);

        /// <summary>
        /// 添加 resize 预处理步骤。
        /// Adds a resize preprocess step.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_preprocess_steps_resize",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_preprocess_steps_resize(IntPtr steps, uint resize_algorithm);

        /// <summary>
        /// 添加单值 scale 预处理步骤。
        /// Adds a scalar scale preprocess step.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_preprocess_steps_scale",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_preprocess_steps_scale(IntPtr steps, float value);

        /// <summary>
        /// 添加多通道 scale 预处理步骤。
        /// Adds a per-channel scale preprocess step.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_preprocess_steps_scale_multi_channels",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_preprocess_steps_scale_multi_channels(
            IntPtr steps,
            [MarshalAs(UnmanagedType.LPArray)] float[] values,
            int value_size);

        /// <summary>
        /// 添加单值 mean 预处理步骤。
        /// Adds a scalar mean preprocess step.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_preprocess_steps_mean",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_preprocess_steps_mean(IntPtr steps, float value);

        /// <summary>
        /// 添加多通道 mean 预处理步骤。
        /// Adds a per-channel mean preprocess step.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_preprocess_steps_mean_multi_channels",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_preprocess_steps_mean_multi_channels(
            IntPtr steps,
            [MarshalAs(UnmanagedType.LPArray)] float[] values,
            int value_size);

        /// <summary>
        /// 添加裁剪预处理步骤。
        /// Adds a crop preprocess step.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_preprocess_steps_crop",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_preprocess_steps_crop(
            IntPtr steps,
            [MarshalAs(UnmanagedType.LPArray)] int[] begin,
            int begin_size,
            [MarshalAs(UnmanagedType.LPArray)] int[] end,
            int end_size);

        /// <summary>
        /// 添加布局转换预处理步骤。
        /// Adds a layout conversion preprocess step.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_preprocess_steps_convert_layout",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_preprocess_steps_convert_layout(IntPtr steps, IntPtr layout);

        /// <summary>
        /// 添加元素类型转换预处理步骤。
        /// Adds an element type conversion preprocess step.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_preprocess_steps_convert_element_type",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_preprocess_steps_convert_element_type(IntPtr steps, uint element_type);

        /// <summary>
        /// 添加颜色格式转换预处理步骤。
        /// Adds a color format conversion preprocess step.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_preprocess_steps_convert_color",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_preprocess_steps_convert_color(IntPtr steps, uint color_format);

        /// <summary>
        /// 添加通道反转预处理步骤。
        /// Adds a channel reversal preprocess step.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_preprocess_steps_reverse_channels",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_preprocess_steps_reverse_channels(IntPtr steps);

        /// <summary>
        /// 添加填充预处理步骤。
        /// Adds a padding preprocess step.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_preprocess_steps_pad",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_preprocess_steps_pad(
            IntPtr steps,
            [MarshalAs(UnmanagedType.LPArray)] int[] pads_begin,
            ulong pads_begin_size,
            [MarshalAs(UnmanagedType.LPArray)] int[] pads_end,
            ulong pads_end_size,
            float value,
            uint mode);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_preprocess_steps_pad",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_preprocess_preprocess_steps_pad_native_size(
            IntPtr steps,
            [MarshalAs(UnmanagedType.LPArray)] int[] pads_begin,
            UIntPtr pads_begin_size,
            [MarshalAs(UnmanagedType.LPArray)] int[] pads_end,
            UIntPtr pads_end_size,
            float value,
            uint mode);

        #endregion

        #region Output Info

        /// <summary>
        /// 获取默认输出信息句柄，返回的句柄由调用方释放。
        /// Gets the default output info handle. The returned handle is owned by the caller.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_prepostprocessor_get_output_info",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_prepostprocessor_get_output_info(IntPtr preprocess, ref IntPtr output_info);

        /// <summary>
        /// 按张量索引获取输出信息句柄。
        /// Gets an output info handle by tensor index.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_prepostprocessor_get_output_info_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_prepostprocessor_get_output_info_by_index(IntPtr preprocess, ulong tensor_index, ref IntPtr output_info);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_prepostprocessor_get_output_info_by_index",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_preprocess_prepostprocessor_get_output_info_by_index_native_size(
            IntPtr preprocess,
            UIntPtr tensor_index,
            ref IntPtr output_info);

        /// <summary>
        /// 按张量名称获取输出信息句柄。旧兼容入口按 ANSI 字符串编组。
        /// Gets an output info handle by tensor name. This legacy-compatible entry marshals the string as ANSI.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_prepostprocessor_get_output_info_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_prepostprocessor_get_output_info_by_name(IntPtr preprocess, string tensor_name, ref IntPtr output_info);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_prepostprocessor_get_output_info_by_name",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_preprocess_prepostprocessor_get_output_info_by_name_utf8(
            IntPtr preprocess,
            IntPtr tensor_name,
            ref IntPtr output_info);

        /// <summary>
        /// 释放输出信息句柄。
        /// Releases an output info handle.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_output_info_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_preprocess_output_info_free(IntPtr output_info);

        /// <summary>
        /// 从输出信息中获取输出张量信息句柄，返回的句柄由调用方释放。
        /// Gets an output tensor info handle from output info. The returned handle is owned by the caller.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_output_info_get_tensor_info",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_output_info_get_tensor_info(IntPtr output_info, ref IntPtr tensor_info);

        /// <summary>
        /// 释放输出张量信息句柄。
        /// Releases an output tensor info handle.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_output_tensor_info_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_preprocess_output_tensor_info_free(IntPtr tensor_info);

        /// <summary>
        /// 设置输出张量元素类型。
        /// Sets the output tensor element type.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_output_set_element_type",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_output_set_element_type(IntPtr tensor_info, uint element_type);

        #endregion

        #region Model Info

        /// <summary>
        /// 从输入信息中获取模型信息句柄，返回的句柄由调用方释放。
        /// Gets an input model info handle from input info. The returned handle is owned by the caller.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_info_get_model_info",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_input_info_get_model_info(IntPtr input_info, ref IntPtr model_info);

        /// <summary>
        /// 释放输入模型信息句柄。
        /// Releases an input model info handle.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_model_info_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_preprocess_input_model_info_free(IntPtr model_info);

        /// <summary>
        /// 设置模型输入布局，layout 句柄由调用方管理。
        /// Sets the model input layout. The layout handle lifetime is managed by the caller.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_model_info_set_layout",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_input_model_info_set_layout(IntPtr model_info, IntPtr layout);

        #endregion
    }
}
