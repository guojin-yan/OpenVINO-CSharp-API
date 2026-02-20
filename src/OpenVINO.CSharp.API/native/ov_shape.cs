// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Initialize a fully shape object, allocate space for its dimensions and set its content if dims is not null.
        /// </summary>
        /// <param name="rank">The rank value for this object, it should be more than 0(>0)</param>
        /// <param name="dims">The dimensions data for this shape object, it's size should be equal to rank.</param>
        /// <param name="shape">The input/output shape object pointer.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_shape_create",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_shape_create(
            long rank,
            [MarshalAs(UnmanagedType.LPArray)] long[] dims,
            ref ov_shape_t shape);

        /// <summary>
        /// Free a shape object's internal memory.
        /// </summary>
        /// <param name="shape">The input shape object pointer.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_shape_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_shape_free(ref ov_shape_t shape);
    }

    /// <summary>
    /// Structure representing a static shape
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ov_shape_t
    {
        public long rank;
        public IntPtr dims;

        /// <summary>
        /// Get dimensions as array
        /// </summary>
        public long[] GetDims()
        {
            if (rank <= 0 || dims == IntPtr.Zero)
                return Array.Empty<long>();

            long[] result = new long[rank];
            Marshal.Copy(dims, result, 0, (int)rank);
            return result;
        }
    }
}
