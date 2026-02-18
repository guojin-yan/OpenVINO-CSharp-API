// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Release the memory allocated by ov_model_t.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_model_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_model_free(IntPtr model);

        /// <summary>
        /// Get the input size of ov_model_t.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_model_inputs_size",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_inputs_size(IntPtr model, ref ulong size);

        /// <summary>
        /// Get the output size of ov_model_t.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_model_outputs_size",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_outputs_size(IntPtr model, ref ulong size);

        /// <summary>
        /// Get the single const input port of ov_model_t.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_model_const_input",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_const_input(IntPtr model, ref IntPtr input_port);

        /// <summary>
        /// Get the const input port of ov_model_t by port index.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_model_const_input_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_const_input_by_index(IntPtr model, ulong index, ref IntPtr input_port);

        /// <summary>
        /// Get the const input port of ov_model_t by name.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_model_const_input_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_const_input_by_name(IntPtr model, ref sbyte tensor_name, ref IntPtr input_port);

        /// <summary>
        /// Get the single const output port of ov_model_t.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_model_const_output",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_const_output(IntPtr model, ref IntPtr output_port);

        /// <summary>
        /// Get the const output port of ov_model_t by port index.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_model_const_output_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_const_output_by_index(IntPtr model, ulong index, ref IntPtr output_port);

        /// <summary>
        /// Get the const output port of ov_model_t by name.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_model_const_output_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_const_output_by_name(IntPtr model, ref sbyte tensor_name, ref IntPtr output_port);

        /// <summary>
        /// Get the single input port of ov_model_t.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_model_input",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_input(IntPtr model, ref IntPtr input_port);

        /// <summary>
        /// Get the input port of ov_model_t by port index.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_model_input_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_input_by_index(IntPtr model, ulong index, ref IntPtr input_port);

        /// <summary>
        /// Get the input port of ov_model_t by name.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_model_input_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_input_by_name(IntPtr model, ref sbyte tensor_name, ref IntPtr input_port);

        /// <summary>
        /// Get the single output port of ov_model_t.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_model_output",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_output(IntPtr model, ref IntPtr output_port);

        /// <summary>
        /// Get the output port of ov_model_t by port index.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_model_output_by_index",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_output_by_index(IntPtr model, ulong index, ref IntPtr output_port);

        /// <summary>
        /// Get the output port of ov_model_t by name.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_model_output_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_output_by_name(IntPtr model, ref sbyte tensor_name, ref IntPtr output_port);

        /// <summary>
        /// Get the friend name of ov_model_t.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_model_get_friendly_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_get_friendly_name(IntPtr model, ref IntPtr friendly_name);
    }
}
