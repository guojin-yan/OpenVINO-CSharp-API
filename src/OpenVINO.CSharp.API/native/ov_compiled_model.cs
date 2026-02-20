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
        /// Release the memory allocated by ov_compiled_model_t.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_compiled_model_free(IntPtr compiled_model);

        #endregion

        #region Input Methods

        /// <summary>
        /// Get the input size of ov_compiled_model_t.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_inputs_size",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_inputs_size(IntPtr compiled_model, ref ulong size);

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
        public extern static ExceptionStatus ov_compiled_model_input_by_index(
            IntPtr compiled_model,
            ulong index,
            ref IntPtr input_port);

        /// <summary>
        /// Get the const input port of ov_compiled_model_t by name.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_input_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_input_by_name(
            IntPtr compiled_model,
            [MarshalAs(UnmanagedType.LPStr)] string tensor_name,
            ref IntPtr input_port);

        #endregion

        #region Output Methods

        /// <summary>
        /// Get the output size of ov_compiled_model_t.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_outputs_size",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_outputs_size(IntPtr compiled_model, ref ulong size);

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
        public extern static ExceptionStatus ov_compiled_model_output_by_index(
            IntPtr compiled_model,
            ulong index,
            ref IntPtr output_port);

        /// <summary>
        /// Get the const output port of ov_compiled_model_t by name.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_output_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_output_by_name(
            IntPtr compiled_model,
            [MarshalAs(UnmanagedType.LPStr)] string tensor_name,
            ref IntPtr output_port);

        #endregion

        #region Runtime Model and Inference Request

        /// <summary>
        /// Gets runtime model information from a device.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_get_runtime_model",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_get_runtime_model(
            IntPtr compiled_model,
            ref IntPtr model);

        /// <summary>
        /// Creates an inference request object.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_create_infer_request",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_create_infer_request(
            IntPtr compiled_model,
            ref IntPtr infer_request);

        #endregion

        #region Export and Properties

        /// <summary>
        /// Exports the compiled model to the specified file path.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_export_model",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_export_model(
            IntPtr compiled_model,
            [MarshalAs(UnmanagedType.LPStr)] string export_model_path);

        /// <summary>
        /// Sets properties for the compiled model.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_set_property",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_set_property(
            IntPtr compiled_model,
            ulong property_args_size,
            IntPtr property_key,
            IntPtr property_value);

        /// <summary>
        /// Gets properties for the compiled model.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_get_property",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_get_property(
            IntPtr compiled_model,
            [MarshalAs(UnmanagedType.LPStr)] string property_key,
            ref IntPtr property_value);

        #endregion

        #region Remote Context

        /// <summary>
        /// Returns pointer to device-specific shared context.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_get_context",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_get_context(
            IntPtr compiled_model,
            ref IntPtr context);

        #endregion
    }
}
