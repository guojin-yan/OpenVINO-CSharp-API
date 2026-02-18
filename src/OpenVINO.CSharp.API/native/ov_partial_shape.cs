// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Create a partial shape from static shape.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_partial_shape_from_shape",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_partial_shape_from_shape(IntPtr shape, IntPtr partial_shape);

        /// <summary>
        /// Convert partial shape to static shape.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_partial_shape_to_shape",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_partial_shape_to_shape(IntPtr partial_shape, IntPtr shape);

        /// <summary>
        /// Check if partial shape is static.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_partial_shape_is_static",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_partial_shape_is_static(IntPtr partial_shape, ref int is_static);

        /// <summary>
        /// Check if partial shape is dynamic.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_partial_shape_is_dynamic",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_partial_shape_is_dynamic(IntPtr partial_shape, ref int is_dynamic);

        /// <summary>
        /// Get partial shape rank.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_partial_shape_get_rank",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_partial_shape_get_rank(IntPtr partial_shape, IntPtr rank);

        /// <summary>
        /// Get partial shape dimension by index.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_partial_shape_get_dimension",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_partial_shape_get_dimension(IntPtr partial_shape, ulong idx, IntPtr dimension);

        /// <summary>
        /// Free partial shape.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_partial_shape_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_partial_shape_free(IntPtr partial_shape);
    }
}
