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
        /// <summary>
        /// Create a ov_preprocess_prepostprocessor_t instance.
        /// </summary>
        /// <param name="model">A pointer to the ov_model_t.</param>
        /// <param name="preprocess">A pointer to the ov_preprocess_prepostprocessor_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_prepostprocessor_create", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_prepostprocessor_create(
            IntPtr model, 
            ref IntPtr preprocess);

        /// <summary>
        /// Release the memory allocated by ov_preprocess_prepostprocessor_t.
        /// </summary>
        /// <param name="preprocess">A pointer to the ov_preprocess_prepostprocessor_t to free memory.</param>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_prepostprocessor_free", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_preprocess_prepostprocessor_free(
            IntPtr preprocess);

        /// <summary>
        /// Get the input info of ov_preprocess_prepostprocessor_t instance.
        /// </summary>
        /// <param name="preprocess">A pointer to the ov_preprocess_prepostprocessor_t.</param>
        /// <param name="preprocess_input_info">A pointer to the ov_preprocess_input_info_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_prepostprocessor_get_input_info", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_prepostprocessor_get_input_info(
            IntPtr preprocess, 
            ref IntPtr preprocess_input_info);

        /// <summary>
        /// Get the input info of ov_preprocess_prepostprocessor_t instance by tensor name.
        /// </summary>
        /// <param name="preprocess">A pointer to the ov_preprocess_prepostprocessor_t.</param>
        /// <param name="tensor_name">The name of input.</param>
        /// <param name="preprocess_input_info">A pointer to the ov_preprocess_input_info_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_prepostprocessor_get_input_info_by_name", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_prepostprocessor_get_input_info_by_name(
            IntPtr preprocess, 
            ref sbyte tensor_name, 
            ref IntPtr preprocess_input_info);

        /// <summary>
        /// Get the input info of ov_preprocess_prepostprocessor_t instance by tensor order.
        /// </summary>
        /// <param name="preprocess">A pointer to the ov_preprocess_prepostprocessor_t.</param>
        /// <param name="tensor_index">The order of input.</param>
        /// <param name="preprocess_input_info">A pointer to the ov_preprocess_input_info_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_prepostprocessor_free", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_prepostprocessor_get_input_info_by_index(
            IntPtr preprocess,
            ulong tensor_index, 
            ref IntPtr preprocess_input_info);

        /// <summary>
        /// Release the memory allocated by ov_preprocess_input_info_t.
        /// </summary>
        /// <param name="preprocess_input_info">A pointer to the ov_preprocess_input_info_t to free memory.</param>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_prepostprocessor_free", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_preprocess_input_info_free(
            IntPtr preprocess_input_info);

        /// <summary>
        /// Get a ov_preprocess_input_tensor_info_t.
        /// </summary>
        /// <param name="preprocess_input_info">A pointer to the ov_preprocess_input_info_t.</param>
        /// <param name="preprocess_input_tensor_info">A pointer to ov_preprocess_input_tensor_info_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_info_get_tensor_info", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_info_get_tensor_info(
            IntPtr preprocess_input_info, 
            ref IntPtr preprocess_input_tensor_info);

        /// <summary>
        /// Release the memory allocated by ov_preprocess_input_tensor_info_t.
        /// </summary>
        /// <param name="preprocess_input_tensor_info">A pointer to the ov_preprocess_input_tensor_info_t to free memory.</param>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_tensor_info_free", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_preprocess_input_tensor_info_free(
            IntPtr preprocess_input_tensor_info);

        /// <summary>
        /// Get a ov_preprocess_preprocess_steps_t.
        /// </summary>
        /// <param name="preprocess_input_info">A pointer to the ov_preprocess_input_info_t.</param>
        /// <param name="preprocess_input_steps">A pointer to ov_preprocess_preprocess_steps_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_info_get_preprocess_steps", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_info_get_preprocess_steps(
            IntPtr preprocess_input_info,
            ref IntPtr preprocess_input_steps);


        /// <summary>
        /// Release the memory allocated by ov_preprocess_preprocess_steps_t.
        /// </summary>
        /// <param name="preprocess_input_process_steps">A pointer to the ov_preprocess_preprocess_steps_t to free memory.</param>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_preprocess_steps_free",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_preprocess_preprocess_steps_free(
            IntPtr preprocess_input_process_steps);


        /// <summary>
        /// Add resize operation to model's dimensions.
        /// </summary>
        /// <param name="preprocess_input_process_steps">A pointer to ov_preprocess_preprocess_steps_t.</param>
        /// <param name="resize_algorithm">A ov_preprocess_resizeAlgorithm instance</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_preprocess_steps_resize", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_preprocess_steps_resize(
            IntPtr preprocess_input_process_steps,
            int resize_algorithm);


        /// <summary>
        /// Add scale preprocess operation. Divide each element of input by specified value.
        /// </summary>
        /// <param name="preprocess_input_process_steps">A pointer to ov_preprocess_preprocess_steps_t.</param>
        /// <param name="value">Scaling value.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_preprocess_steps_scale", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_preprocess_steps_scale(
            IntPtr preprocess_input_process_steps, 
            float value);


        /// <summary>
        /// Add mean preprocess operation. Subtract specified value from each element of input.
        /// </summary>
        /// <param name="preprocess_input_process_steps">A pointer to ov_preprocess_preprocess_steps_t.</param>
        /// <param name="value">Value to subtract from each element.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_preprocess_steps_mean", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_preprocess_steps_mean(
            IntPtr preprocess_input_process_steps, 
            float value);

        /// <summary>
        /// Crop input tensor between begin and end coordinates.
        /// </summary>
        /// <param name="preprocess_input_process_steps">A pointer to ov_preprocess_preprocess_steps_t.</param>
        /// <param name="begin">Pointer to begin indexes for input tensor cropping. 
        /// Negative values represent counting elements from the end of input tensor</param>
        /// <param name="begin_size">The size of begin array.</param>
        /// <param name="end">Pointer to end indexes for input tensor cropping.  
        /// End indexes are exclusive, which means values including end edge are not included in the output slice.     
        /// Negative values represent counting elements from the end of input tensor</param>
        /// <param name="end_size">The size of end array</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_preprocess_steps_crop", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_preprocess_steps_crop(
            IntPtr preprocess_input_process_steps,
            ref int begin,
            int begin_size,
            ref int end,
            int end_size);

        /// <summary>
        /// Add 'convert layout' operation to specified layout.
        /// </summary>
        /// <param name="preprocess_input_process_steps">A pointer to ov_preprocess_preprocess_steps_t.</param>
        /// <param name="layout">A point to ov_layout_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_preprocess_steps_convert_layout", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_preprocess_steps_convert_layout(
            IntPtr preprocess_input_process_steps,
            IntPtr layout);


        /// <summary>
        /// Reverse channels operation.
        /// </summary>
        /// <param name="preprocess_input_process_steps">A pointer to ov_preprocess_preprocess_steps_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_preprocess_steps_reverse_channels", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_preprocess_steps_reverse_channels(
            IntPtr preprocess_input_process_steps);

        /// <summary>
        /// Set ov_preprocess_input_tensor_info_t precesion.
        /// </summary>
        /// <param name="preprocess_input_tensor_info">A pointer to the ov_preprocess_input_tensor_info_t.</param>
        /// <param name="element_type">A point to element_type.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_tensor_info_set_element_type", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_tensor_info_set_element_type(
            IntPtr preprocess_input_tensor_info,
            uint element_type);

        /// <summary>
        /// Set ov_preprocess_input_tensor_info_t color format.
        /// </summary>
        /// <param name="preprocess_input_tensor_info">A pointer to the ov_preprocess_input_tensor_info_t.</param>
        /// <param name="color_format"> The enumerate of colorFormat</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_tensor_info_set_color_format",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_tensor_info_set_color_format(
            IntPtr preprocess_input_tensor_info,
            uint color_format);


        /// <summary>
        /// Set ov_preprocess_input_tensor_info_t color format with subname.
        /// </summary>
        /// <param name="preprocess_input_tensor_info">A pointer to the ov_preprocess_input_tensor_info_t.</param>
        /// <param name="color_format">The enumerate of colorFormat</param>
        /// <param name="sub_names_size">The size of sub_names.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_tensor_info_set_color_format_with_subname",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_tensor_info_set_color_format_with_subname(
            IntPtr preprocess_input_tensor_info,
            uint color_format,
            ulong sub_names_size);


        /// <summary>
        /// Set ov_preprocess_input_tensor_info_t spatial_static_shape.
        /// </summary>
        /// <param name="preprocess_input_tensor_info">A pointer to the ov_preprocess_input_tensor_info_t.</param>
        /// <param name="input_height">The height of input</param>
        /// <param name="input_width">The width of input</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_tensor_info_set_spatial_static_shape", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_tensor_info_set_spatial_static_shape(
            IntPtr preprocess_input_tensor_info,
            ulong input_height,
            ulong input_width);


        /// <summary>
        /// Set ov_preprocess_input_tensor_info_t memory type.
        /// </summary>
        /// <param name="preprocess_input_tensor_info">A pointer to the ov_preprocess_input_tensor_info_t.</param>
        /// <param name="mem_type"> Memory type. Refer to ov_remote_context.h to get memory type string info.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_tensor_info_set_memory_type", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_tensor_info_set_memory_type(
            IntPtr preprocess_input_tensor_info,
            ref sbyte mem_type);


        /// <summary>
        /// Convert ov_preprocess_preprocess_steps_t element type.
        /// </summary>
        /// <param name="preprocess_input_process_steps">A pointer to the ov_preprocess_preprocess_steps_t.</param>
        /// <param name="element_type">preprocess input element type.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_preprocess_steps_convert_element_type", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_preprocess_steps_convert_element_type(
            IntPtr preprocess_input_process_steps,
            uint element_type);


        /// <summary>
        /// onvert ov_preprocess_preprocess_steps_t color.
        /// </summary>
        /// <param name="preprocess_input_process_steps">A pointer to the ov_preprocess_preprocess_steps_t.</param>
        /// <param name="color_format">The enumerate of colorFormat.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_preprocess_steps_convert_color", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_preprocess_steps_convert_color(
            IntPtr preprocess_input_process_steps,
           uint color_format);


        /// <summary>
        /// Helper function to reuse element type and shape from user's created tensor.
        /// </summary>
        /// <param name="preprocess_input_tensor_info">A pointer to the ov_preprocess_input_tensor_info_t.</param>
        /// <param name="tensor">A point to ov_tensor_t</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_tensor_info_set_from", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_tensor_info_set_from(
            IntPtr preprocess_input_tensor_info,
            IntPtr tensor);

        /// <summary>
        /// Set ov_preprocess_input_tensor_info_t layout.
        /// </summary>
        /// <param name="preprocess_input_tensor_info">A pointer to the ov_preprocess_input_tensor_info_t.</param>
        /// <param name="layout">A point to ov_layout_t</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_tensor_info_set_layout",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_tensor_info_set_layout(
            IntPtr preprocess_input_tensor_info,
            IntPtr layout);


        /// <summary>
        /// Get the output info of ov_preprocess_output_info_t instance.
        /// </summary>
        /// <param name="preprocess">A pointer to the ov_preprocess_prepostprocessor_t.</param>
        /// <param name="preprocess_output_info">A pointer to the ov_preprocess_output_info_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_prepostprocessor_get_output_info", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_prepostprocessor_get_output_info(
            IntPtr preprocess,
            ref IntPtr preprocess_output_info);


        /// <summary>
        /// Get the output info of ov_preprocess_output_info_t instance.
        /// </summary>
        /// <param name="preprocess">A pointer to the ov_preprocess_prepostprocessor_t.</param>
        /// <param name="tensor_index">The tensor index.</param>
        /// <param name="preprocess_output_info">A pointer to the ov_preprocess_output_info_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_prepostprocessor_get_output_info_by_index", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_prepostprocessor_get_output_info_by_index(
            IntPtr preprocess,
            ulong tensor_index,
            ref IntPtr preprocess_output_info);


        /// <summary>
        /// Get the output info of ov_preprocess_output_info_t instance.
        /// </summary>
        /// <param name="preprocess">A pointer to the ov_preprocess_prepostprocessor_t.</param>
        /// <param name="tensor_name">The name of input.</param>
        /// <param name="preprocess_output_info">A pointer to the ov_preprocess_output_info_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_prepostprocessor_get_output_info_by_name", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_prepostprocessor_get_output_info_by_name(
            IntPtr preprocess,
            ref sbyte tensor_name,
            ref IntPtr preprocess_output_info);


        /// <summary>
        /// Release the memory allocated by ov_preprocess_output_info_t.
        /// </summary>
        /// <param name="preprocess_output_info">A pointer to the ov_preprocess_output_info_t to free memory.</param>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_output_info_free", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_preprocess_output_info_free(IntPtr preprocess_output_info);


        /// <summary>
        /// Get a ov_preprocess_input_tensor_info_t.
        /// </summary>
        /// <param name="preprocess_output_info">A pointer to the ov_preprocess_output_info_t.</param>
        /// <param name="preprocess_output_tensor_info">A pointer to the ov_preprocess_output_tensor_info_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_output_info_get_tensor_info", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_output_info_get_tensor_info(
            IntPtr preprocess_output_info,
            ref IntPtr preprocess_output_tensor_info);

        /// <summary>
        /// Release the memory allocated by ov_preprocess_output_tensor_info_t.
        /// </summary>
        /// <param name="preprocess_output_tensor_info">A pointer to the ov_preprocess_output_tensor_info_t to free memory.</param>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_output_tensor_info_free", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_preprocess_output_tensor_info_free(
            IntPtr preprocess_output_tensor_info);


        /// <summary>
        /// Set ov_preprocess_input_tensor_info_t precesion.
        /// </summary>
        /// <param name="preprocess_output_tensor_info">A pointer to the ov_preprocess_output_tensor_info_t.</param>
        /// <param name="element_type">A point to element_type</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_output_set_element_type", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_output_set_element_type(
            IntPtr preprocess_output_tensor_info,
            uint element_type);


        /// <summary>
        /// Get current input model information.
        /// </summary>
        /// <param name="preprocess_input_info">A pointer to the ov_preprocess_input_info_t.</param>
        /// <param name="preprocess_input_model_info">A pointer to the ov_preprocess_input_model_info_t</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_info_get_model_info", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_info_get_model_info(
            IntPtr preprocess_input_info,
            ref IntPtr preprocess_input_model_info);

        /// <summary>
        /// Release the memory allocated by ov_preprocess_input_model_info_t.
        /// </summary>
        /// <param name="preprocess_input_model_info">A pointer to the ov_preprocess_input_model_info_t to free memory.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_model_info_free", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_model_info_free(
            IntPtr preprocess_input_model_info);

        /// <summary>
        /// Set layout for model's input tensor.
        /// </summary>
        /// <param name="preprocess_input_model_info">A pointer to the ov_preprocess_input_model_info_t</param>
        /// <param name="layout">A point to ov_layout_t</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_input_model_info_set_layout",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_input_model_info_set_layout(
            IntPtr preprocess_input_model_info,
            IntPtr layout);


        /// <summary>
        /// Adds pre/post-processing operations to function passed in constructor.
        /// </summary>
        /// <param name="preprocess">A pointer to the ov_preprocess_prepostprocessor_t.</param>
        /// <param name="model">A pointer to the ov_model_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_preprocess_prepostprocessor_build", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_preprocess_prepostprocessor_build(
            IntPtr preprocess,
            ref IntPtr model);

    }
}
