// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        // Port shape
        [DllImport("openvino_c", EntryPoint = "ov_const_port_get_shape",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_const_port_get_shape(IntPtr port, IntPtr shape);

        [DllImport("openvino_c", EntryPoint = "ov_port_get_shape",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_port_get_shape(IntPtr port, IntPtr shape);

        // Port partial shape
        [DllImport("openvino_c", EntryPoint = "ov_port_get_partial_shape",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_port_get_partial_shape(IntPtr port, IntPtr partial_shape);

        // Port element type
        [DllImport("openvino_c", EntryPoint = "ov_port_get_element_type",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_port_get_element_type(IntPtr port, ref uint tensor_type);

        // Port name
        [DllImport("openvino_c", EntryPoint = "ov_port_get_any_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_port_get_any_name(IntPtr port, ref IntPtr tensor_name);

        // Free ports
        [DllImport("openvino_c", EntryPoint = "ov_output_port_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_output_port_free(IntPtr port);

        [DllImport("openvino_c", EntryPoint = "ov_output_const_port_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_output_const_port_free(IntPtr port);

        // Node methods
        //[DllImport("openvino_c", EntryPoint = "ov_node_get_output_size",
        //    CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        //public extern static ExceptionStatus ov_node_get_output_size(IntPtr node, ref ulong size);

        //[DllImport("openvino_c", EntryPoint = "ov_node_get_output",
        //    CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        //public extern static ExceptionStatus ov_node_get_output(IntPtr node, ulong idx, ref IntPtr output_port);

        //[DllImport("openvino_c", EntryPoint = "ov_node_get_input_size",
        //    CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        //public extern static ExceptionStatus ov_node_get_input_size(IntPtr node, ref ulong size);

        //[DllImport("openvino_c", EntryPoint = "ov_node_get_input",
        //    CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        //public extern static ExceptionStatus ov_node_get_input(IntPtr node, ulong idx, ref IntPtr input_port);

        //[DllImport("openvino_c", EntryPoint = "ov_node_get_name",
        //    CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        //public extern static ExceptionStatus ov_node_get_name(IntPtr node, ref IntPtr name);

        //[DllImport("openvino_c", EntryPoint = "ov_node_get_friendly_name",
        //    CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        //public extern static ExceptionStatus ov_node_get_friendly_name(IntPtr node, ref IntPtr friendly_name);
    }
}
