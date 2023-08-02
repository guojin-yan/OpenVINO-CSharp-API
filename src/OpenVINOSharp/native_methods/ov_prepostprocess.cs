using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    public partial class NativeMethods
    {
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_prepostprocessor_create", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_prepostprocessor_create(
            IntPtr model, 
            ref IntPtr preprocess);
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_prepostprocessor_free", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_prepostprocessor_free(
            IntPtr preprocess);
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_prepostprocessor_get_input_info", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_prepostprocessor_get_input_info(
            IntPtr preprocess, 
            ref IntPtr preprocess_input_info);
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_prepostprocessor_get_input_info_by_name", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_prepostprocessor_get_input_info_by_name(
            IntPtr preprocess, 
            ref sbyte tensor_name, 
            ref IntPtr preprocess_input_info);
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_prepostprocessor_free", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_prepostprocessor_get_input_info_by_index(
            IntPtr preprocess,
            ulong tensor_index, 
            ref IntPtr preprocess_input_info);
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_prepostprocessor_free", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_info_free(
            IntPtr preprocess_input_info);
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_info_get_tensor_info", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_info_get_tensor_info(
            IntPtr preprocess_input_info, 
            ref IntPtr preprocess_input_tensor_info);

        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_tensor_info_free", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_tensor_info_free(
            IntPtr preprocess_input_tensor_info);


        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_info_get_preprocess_steps", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_info_get_preprocess_steps(
            IntPtr preprocess_input_info,
            ref IntPtr preprocess_input_steps);

        [DllImport(dll_extern, EntryPoint = "ov_preprocess_preprocess_steps_free",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_preprocess_steps_free(
            IntPtr preprocess_input_process_steps);

        [DllImport(dll_extern, EntryPoint = "ov_preprocess_preprocess_steps_resize", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_preprocess_steps_resize(
            IntPtr preprocess_input_process_steps,
            int resize_algorithm);

        [DllImport(dll_extern, EntryPoint = "ov_preprocess_preprocess_steps_scale", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_preprocess_steps_scale(
            IntPtr preprocess_input_process_steps, 
            float value);

        [DllImport(dll_extern, EntryPoint = "ov_preprocess_preprocess_steps_mean", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_preprocess_steps_mean(
            IntPtr preprocess_input_process_steps, 
            float value);

        [DllImport(dll_extern, EntryPoint = "ov_preprocess_preprocess_steps_crop", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_preprocess_steps_crop(
            IntPtr preprocess_input_process_steps,
            ref int begin,
            int begin_size,
            ref int end,
            int end_size);
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_preprocess_steps_convert_layout", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_preprocess_steps_convert_layout(
            IntPtr preprocess_input_process_steps,
            IntPtr layout);

        [DllImport(dll_extern, EntryPoint = "ov_preprocess_preprocess_steps_reverse_channels", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_preprocess_steps_reverse_channels(
            IntPtr preprocess_input_process_steps);

        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_tensor_info_set_element_type", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_tensor_info_set_element_type(
            IntPtr preprocess_input_tensor_info,
            uint element_type);

        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_tensor_info_set_color_format",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_tensor_info_set_color_format(
            IntPtr preprocess_input_tensor_info,
            uint color_format);

        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_tensor_info_set_color_format_with_subname",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_tensor_info_set_color_format_with_subname(
            IntPtr preprocess_input_tensor_info,
            uint color_format,
            ulong sub_names_size);

        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_tensor_info_set_spatial_static_shape", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_tensor_info_set_spatial_static_shape(
            IntPtr preprocess_input_tensor_info,
            ulong input_height,
            ulong input_width);

        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_tensor_info_set_memory_type", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_tensor_info_set_memory_type(
            IntPtr preprocess_input_tensor_info,
            ref sbyte mem_type);

        [DllImport(dll_extern, EntryPoint = "ov_preprocess_preprocess_steps_convert_element_type", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_preprocess_steps_convert_element_type(
            IntPtr preprocess_input_process_steps,
            uint element_type);

        [DllImport(dll_extern, EntryPoint = "ov_preprocess_preprocess_steps_convert_color", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_preprocess_steps_convert_color(
            IntPtr preprocess_input_process_steps,
           uint color_format);

        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_tensor_info_set_from", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_tensor_info_set_from(
            IntPtr preprocess_input_tensor_info,
            IntPtr tensor);
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_tensor_info_set_layout",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_tensor_info_set_layout(
            IntPtr preprocess_input_tensor_info,
            IntPtr layout);

        [DllImport(dll_extern, EntryPoint = "ov_preprocess_prepostprocessor_get_output_info", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_prepostprocessor_get_output_info(
            IntPtr preprocess,
            ref IntPtr preprocess_output_info);

        [DllImport(dll_extern, EntryPoint = "ov_preprocess_prepostprocessor_get_output_info_by_index", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_prepostprocessor_get_output_info_by_index(
            IntPtr preprocess,
            ulong tensor_index,
            ref IntPtr preprocess_output_info);

        [DllImport(dll_extern, EntryPoint = "ov_preprocess_prepostprocessor_get_output_info_by_name", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_prepostprocessor_get_output_info_by_name(
            IntPtr preprocess,
            ref sbyte tensor_name,
            ref IntPtr preprocess_output_info);

        [DllImport(dll_extern, EntryPoint = "ov_preprocess_output_info_free", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_output_info_free(IntPtr preprocess_output_info);

        [DllImport(dll_extern, EntryPoint = "ov_preprocess_output_info_get_tensor_info", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_output_info_get_tensor_info(
            IntPtr preprocess_output_info,
            ref IntPtr preprocess_output_tensor_info);
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_output_tensor_info_free", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_output_tensor_info_free(
            IntPtr preprocess_output_tensor_info);

        [DllImport(dll_extern, EntryPoint = "ov_preprocess_output_set_element_type", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_output_set_element_type(
            IntPtr preprocess_output_tensor_info,
            uint element_type);

        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_info_get_model_info", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_info_get_model_info(
            IntPtr preprocess_input_info,
            ref IntPtr preprocess_input_model_info);
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_model_info_free", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_model_info_free(
            IntPtr preprocess_input_model_info);
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_model_info_set_layout",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_model_info_set_layout(
            IntPtr preprocess_input_model_info,
            IntPtr layout);

        [DllImport(dll_extern, EntryPoint = "ov_preprocess_prepostprocessor_build", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_prepostprocessor_build(
            IntPtr preprocess,
            ref IntPtr model);

    }
}
