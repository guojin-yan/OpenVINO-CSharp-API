// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Get the number of output ports of the node.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_node_get_output_size",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_node_get_output_size(IntPtr node, ref ulong size);

        /// <summary>
        /// Get the output port of the node by index.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_node_get_output",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_node_get_output(IntPtr node, ulong idx, ref IntPtr output_port);

        /// <summary>
        /// Get the number of input ports of the node.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_node_get_input_size",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_node_get_input_size(IntPtr node, ref ulong size);

        /// <summary>
        /// Get the input port of the node by index.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_node_get_input",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_node_get_input(IntPtr node, ulong idx, ref IntPtr input_port);

        /// <summary>
        /// Get the input port of the node by name.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_node_get_input_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_node_get_input_by_name(IntPtr node, ref sbyte name, ref IntPtr input_port);

        /// <summary>
        /// Get the output port of the node by name.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_node_get_output_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_node_get_output_by_name(IntPtr node, ref sbyte name, ref IntPtr output_port);

        /// <summary>
        /// Get the name of the node.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_node_get_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_node_get_name(IntPtr node, ref IntPtr name);

        /// <summary>
        /// Get the friendly name of the node.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_node_get_friendly_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_node_get_friendly_name(IntPtr node, ref IntPtr friendly_name);

        /// <summary>
        /// Get the element type of the output port.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_port_get_element_type",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_port_get_element_type(IntPtr output, ref uint type);


        /// <summary>
        /// Get the partial shape of the output port.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_output_get_partial_shape",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_output_get_partial_shape(IntPtr output, IntPtr partial_shape);

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

    }
}
