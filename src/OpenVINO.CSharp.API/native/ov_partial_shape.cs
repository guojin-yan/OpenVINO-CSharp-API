// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        #region Partial Shape Creation

        /// <summary>
        /// Initialize a partial shape with static rank and dynamic dimension.
        /// </summary>
        /// <param name="rank">Support static rank.</param>
        /// <param name="dims">Support dynamic and static dimension.</param>
        /// <param name="partial_shape_obj">The partial shape object to initialize.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_partial_shape_create",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_partial_shape_create(
            long rank,
            [MarshalAs(UnmanagedType.LPArray)] ov_dimension_t[] dims,
            ref ov_partial_shape_t partial_shape_obj);

        /// <summary>
        /// Initialize a partial shape with dynamic rank and dynamic dimension.
        /// </summary>
        /// <param name="rank">Support dynamic and static rank.</param>
        /// <param name="dims">Support dynamic and static dimension.</param>
        /// <param name="partial_shape_obj">The partial shape object to initialize.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_partial_shape_create_dynamic",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_partial_shape_create_dynamic(
            ov_rank_t rank,
            [MarshalAs(UnmanagedType.LPArray)] ov_dimension_t[] dims,
            ref ov_partial_shape_t partial_shape_obj);

        /// <summary>
        /// Initialize a partial shape with static rank and static dimension.
        /// </summary>
        /// <param name="rank">Support static rank.</param>
        /// <param name="dims">Support static dimension.</param>
        /// <param name="partial_shape_obj">The partial shape object to initialize.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_partial_shape_create_static",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_partial_shape_create_static(
            long rank,
            [MarshalAs(UnmanagedType.LPArray)] long[] dims,
            ref ov_partial_shape_t partial_shape_obj);

        #endregion

        #region Partial Shape Conversion

        /// <summary>
        /// Convert partial shape without dynamic data to a static shape.
        /// </summary>
        /// <param name="partial_shape">The partial_shape pointer.</param>
        /// <param name="shape">The shape pointer.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_partial_shape_to_shape",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_partial_shape_to_shape(
            ov_partial_shape_t partial_shape,
            ref ov_shape_t shape);

        /// <summary>
        /// Convert shape to partial shape.
        /// </summary>
        /// <param name="shape">The shape pointer.</param>
        /// <param name="partial_shape">The partial_shape pointer.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_shape_to_partial_shape",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_shape_to_partial_shape(
            ov_shape_t shape,
            ref ov_partial_shape_t partial_shape);

        /// <summary>
        /// Helper function, convert a partial shape to readable string.
        /// </summary>
        /// <param name="partial_shape">The partial_shape pointer.</param>
        /// <returns>A string represents partial_shape's content.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_partial_shape_to_string",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr ov_partial_shape_to_string(ov_partial_shape_t partial_shape);

        #endregion

        #region Partial Shape Query

        /// <summary>
        /// Check this partial_shape whether is dynamic
        /// </summary>
        /// <param name="partial_shape">The partial_shape pointer.</param>
        /// <returns>True if partial shape is dynamic.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_partial_shape_is_dynamic",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool ov_partial_shape_is_dynamic(ov_partial_shape_t partial_shape);

        /// <summary>
        /// Release internal memory allocated in partial shape.
        /// </summary>
        /// <param name="partial_shape">The object's internal memory will be released.</param>
        [DllImport("openvino_c", EntryPoint = "ov_partial_shape_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_partial_shape_free(ref ov_partial_shape_t partial_shape);

        #endregion
    }

    /// <summary>
    /// Structure representing a partial shape with rank and dimensions
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ov_partial_shape_t
    {
        public ov_rank_t rank;
        public IntPtr dims;
    }
}
