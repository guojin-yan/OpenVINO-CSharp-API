// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Release the memory allocated by ov_compiled_model_t.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_compiled_model_free(IntPtr compiled_model);

        /// <summary>
        /// Get the single const input port of ov_compiled_model_t.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_input",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_input(IntPtr compiled_model, ref IntPtr input_port);

        /// <summary>
        /// Get the const input port of ov_compiled_model_t by port index.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_input_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_input_by_index(IntPtr compiled_model, ulong index, ref IntPtr input_port);

        /// <summary>
        /// Get the const input port of ov_compiled_model_t by name.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_input_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_input_by_name(IntPtr compiled_model, ref sbyte tensor_name, ref IntPtr input_port);

        /// <summary>
        /// Get the single const output port of ov_compiled_model_t.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_output",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_output(IntPtr compiled_model, ref IntPtr output_port);

        /// <summary>
        /// Get the const output port of ov_compiled_model_t by port index.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_output_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_output_by_index(IntPtr compiled_model, ulong index, ref IntPtr output_port);

        /// <summary>
        /// Get the const output port of ov_compiled_model_t by name.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_output_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_output_by_name(IntPtr compiled_model, ref sbyte tensor_name, ref IntPtr output_port);

        /// <summary>
        /// Creates an inference request object.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_create_infer_request",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_create_infer_request(IntPtr compiled_model, ref IntPtr infer_request);

        /// <summary>
        /// Get runtime model information from a device.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_get_runtime_model",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_get_runtime_model(IntPtr compiled_model, ref IntPtr model);

        /// <summary>
        /// Exports the compiled model to the specified file path.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_export_model",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_export_model(IntPtr compiled_model, ref sbyte export_path);

        /// <summary>
        /// Sets properties for the compiled model.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_set_property",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_set_property(IntPtr compiled_model,
            ulong property_args_size, IntPtr varg1, IntPtr varg2);

        /// <summary>
        /// Gets properties for the compiled model.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_get_property",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_get_property(IntPtr compiled_model,
            ref sbyte property_key, ref IntPtr property_value);
    }
}
