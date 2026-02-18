// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Gets runtime model information from a device.
        /// </summary>
        //[DllImport("openvino_c", EntryPoint = "ov_compiled_model_get_runtime_model",
        //    CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        //public extern static ExceptionStatus ov_compiled_model_get_runtime_model(IntPtr compiled_model, ref IntPtr model);

        /// <summary>
        /// Exports the compiled model to a file.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_export_model",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_export_model(IntPtr compiled_model, string export_model_path);

        /// <summary>
        /// Returns pointer to device-specific shared context.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_get_context",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_get_context(IntPtr compiled_model, ref IntPtr context);

        // Additional input/output methods
        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_inputs_size",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_inputs_size(IntPtr compiled_model, ref ulong size);

        //[DllImport("openvino_c", EntryPoint = "ov_compiled_model_input",
        //    CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        //public extern static ExceptionStatus ov_compiled_model_input(IntPtr compiled_model, ref IntPtr input_port);

        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_input_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_input_by_name(IntPtr compiled_model, string name, ref IntPtr input_port);

        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_outputs_size",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_outputs_size(IntPtr compiled_model, ref ulong size);

        //[DllImport("openvino_c", EntryPoint = "ov_compiled_model_output",
        //    CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        //public extern static ExceptionStatus ov_compiled_model_output(IntPtr compiled_model, ref IntPtr output_port);

        [DllImport("openvino_c", EntryPoint = "ov_compiled_model_output_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_output_by_name(IntPtr compiled_model, string name, ref IntPtr output_port);
    }
}
