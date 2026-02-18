// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Check if model is dynamic.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_model_is_dynamic",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static byte ov_model_is_dynamic(IntPtr model);

        /// <summary>
        /// Reshape model with list of tensor names and partial shapes.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_model_reshape",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_reshape(IntPtr model, IntPtr tensor_names, IntPtr partial_shapes, ulong size);

        /// <summary>
        /// Reshape model input by name.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_model_reshape_input_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_reshape_input_by_name(IntPtr model, string tensor_name, IntPtr partial_shape);

        /// <summary>
        /// Reshape single input model.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_model_reshape_single_input",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_reshape_single_input(IntPtr model, IntPtr partial_shape);

        // Additional port methods
        //[DllImport("openvino_c", EntryPoint = "ov_model_const_input",
        //    CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        //public extern static ExceptionStatus ov_model_const_input(IntPtr model, ref IntPtr input_port);

        [DllImport("openvino_c", EntryPoint = "ov_model_const_input_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_const_input_by_name(IntPtr model, string tensor_name, ref IntPtr input_port);

        //[DllImport("openvino_c", EntryPoint = "ov_model_const_output",
        //    CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        //public extern static ExceptionStatus ov_model_const_output(IntPtr model, ref IntPtr output_port);

        [DllImport("openvino_c", EntryPoint = "ov_model_const_output_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_const_output_by_name(IntPtr model, string tensor_name, ref IntPtr output_port);

        //[DllImport("openvino_c", EntryPoint = "ov_model_output",
        //    CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        //public extern static ExceptionStatus ov_model_output(IntPtr model, ref IntPtr output_port);

        [DllImport("openvino_c", EntryPoint = "ov_model_output_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_output_by_name(IntPtr model, string tensor_name, ref IntPtr output_port);

        [DllImport("openvino_c", EntryPoint = "ov_model_input_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_input_by_name(IntPtr model, string tensor_name, ref IntPtr input_port);
    }
}
