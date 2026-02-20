// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Check this dimension whether is dynamic
        /// </summary>
        /// <param name="dim">The dimension pointer that will be checked.</param>
        /// <returns>Boolean, true is dynamic and false is static.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_dimension_is_dynamic",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool ov_dimension_is_dynamic(ov_dimension_t dim);
    }

    /// <summary>
    /// Structure representing a dimension with min and max bounds
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ov_dimension_t
    {
        public long min;
        public long max;

        public ov_dimension_t(long min, long max)
        {
            this.min = min;
            this.max = max;
        }

        /// <summary>
        /// Check if this dimension is dynamic
        /// </summary>
        public bool is_dynamic => NativeMethods.ov_dimension_is_dynamic(this);

        /// <summary>
        /// Static dimension (fixed size)
        /// </summary>
        public static ov_dimension_t Static(long size) => new ov_dimension_t(size, size);

        /// <summary>
        /// Dynamic dimension with range
        /// </summary>
        public static ov_dimension_t Dynamic(long min, long max) => new ov_dimension_t(min, max);

        /// <summary>
        /// Fully dynamic dimension
        /// </summary>
        public static ov_dimension_t FullyDynamic => new ov_dimension_t(-1, -1);
    }
}
