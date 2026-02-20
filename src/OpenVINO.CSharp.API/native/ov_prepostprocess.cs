// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

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
