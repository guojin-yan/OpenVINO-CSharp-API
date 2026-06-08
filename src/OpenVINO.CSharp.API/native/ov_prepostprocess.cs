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

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_prepostprocessor_create",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_prepostprocessor_create(IntPtr model, ref IntPtr preprocess);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_prepostprocessor_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_preprocess_prepostprocessor_free(IntPtr preprocess);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_prepostprocessor_build",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_prepostprocessor_build(IntPtr preprocess, ref IntPtr model);

        #endregion

        #region Input Info

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_prepostprocessor_get_input_info",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_prepostprocessor_get_input_info(IntPtr preprocess, ref IntPtr input_info);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_prepostprocessor_get_input_info_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_prepostprocessor_get_input_info_by_name(IntPtr preprocess, string tensor_name, ref IntPtr input_info);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_prepostprocessor_get_input_info_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_prepostprocessor_get_input_info_by_index(IntPtr preprocess, ulong tensor_index, ref IntPtr input_info);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_info_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_preprocess_input_info_free(IntPtr input_info);

        #endregion

        #region Input Tensor Info

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_info_get_tensor_info",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_input_info_get_tensor_info(IntPtr input_info, ref IntPtr tensor_info);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_tensor_info_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_preprocess_input_tensor_info_free(IntPtr tensor_info);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_tensor_info_set_element_type",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_input_tensor_info_set_element_type(IntPtr tensor_info, uint element_type);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_tensor_info_set_color_format",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_input_tensor_info_set_color_format(IntPtr tensor_info, uint color_format);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_tensor_info_set_color_format_with_subname",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_input_tensor_info_set_color_format_with_subname(
            IntPtr tensor_info,
            uint color_format,
            ulong sub_names_size,
            IntPtr sub_names);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_tensor_info_set_layout",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_input_tensor_info_set_layout(IntPtr tensor_info, IntPtr layout);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_tensor_info_set_spatial_static_shape",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_input_tensor_info_set_spatial_static_shape(IntPtr tensor_info, ulong input_height, ulong input_width);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_tensor_info_set_memory_type",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_input_tensor_info_set_memory_type(IntPtr tensor_info, string mem_type);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_tensor_info_set_from",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_input_tensor_info_set_from(IntPtr tensor_info, IntPtr tensor);

        #endregion

        #region Preprocess Steps

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_info_get_preprocess_steps",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_input_info_get_preprocess_steps(IntPtr input_info, ref IntPtr steps);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_preprocess_steps_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_preprocess_preprocess_steps_free(IntPtr steps);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_preprocess_steps_resize",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_preprocess_steps_resize(IntPtr steps, uint resize_algorithm);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_preprocess_steps_scale",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_preprocess_steps_scale(IntPtr steps, float value);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_preprocess_steps_scale_multi_channels",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_preprocess_steps_scale_multi_channels(
            IntPtr steps,
            [MarshalAs(UnmanagedType.LPArray)] float[] values,
            int value_size);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_preprocess_steps_mean",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_preprocess_steps_mean(IntPtr steps, float value);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_preprocess_steps_mean_multi_channels",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_preprocess_steps_mean_multi_channels(
            IntPtr steps,
            [MarshalAs(UnmanagedType.LPArray)] float[] values,
            int value_size);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_preprocess_steps_crop",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_preprocess_steps_crop(
            IntPtr steps,
            [MarshalAs(UnmanagedType.LPArray)] int[] begin,
            int begin_size,
            [MarshalAs(UnmanagedType.LPArray)] int[] end,
            int end_size);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_preprocess_steps_convert_layout",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_preprocess_steps_convert_layout(IntPtr steps, IntPtr layout);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_preprocess_steps_convert_element_type",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_preprocess_steps_convert_element_type(IntPtr steps, uint element_type);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_preprocess_steps_convert_color",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_preprocess_steps_convert_color(IntPtr steps, uint color_format);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_preprocess_steps_reverse_channels",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_preprocess_steps_reverse_channels(IntPtr steps);

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

        #endregion

        #region Output Info

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_prepostprocessor_get_output_info",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_prepostprocessor_get_output_info(IntPtr preprocess, ref IntPtr output_info);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_prepostprocessor_get_output_info_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_prepostprocessor_get_output_info_by_index(IntPtr preprocess, ulong tensor_index, ref IntPtr output_info);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_prepostprocessor_get_output_info_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_prepostprocessor_get_output_info_by_name(IntPtr preprocess, string tensor_name, ref IntPtr output_info);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_output_info_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_preprocess_output_info_free(IntPtr output_info);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_output_info_get_tensor_info",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_output_info_get_tensor_info(IntPtr output_info, ref IntPtr tensor_info);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_output_tensor_info_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_preprocess_output_tensor_info_free(IntPtr tensor_info);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_output_set_element_type",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_output_set_element_type(IntPtr tensor_info, uint element_type);

        #endregion

        #region Model Info

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_info_get_model_info",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_input_info_get_model_info(IntPtr input_info, ref IntPtr model_info);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_model_info_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_preprocess_input_model_info_free(IntPtr model_info);

        [DllImport("openvino_c", EntryPoint = "ov_preprocess_input_model_info_set_layout",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_preprocess_input_model_info_set_layout(IntPtr model_info, IntPtr layout);

        #endregion
    }
}
