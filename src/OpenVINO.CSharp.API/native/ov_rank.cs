// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Check this rank whether is dynamic
        /// </summary>
        /// <param name="rank">The rank pointer that will be checked.</param>
        /// <returns>bool The return value.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_rank_is_dynamic",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool ov_rank_is_dynamic(ov_rank_t rank);
    }

    /// <summary>
    /// Structure representing a rank (same as ov_dimension_t)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ov_rank_t
    {
        public long min;
        public long max;

        public ov_rank_t(long min, long max)
        {
            this.min = min;
            this.max = max;
        }

        /// <summary>
        /// Check if this rank is dynamic
        /// </summary>
        public bool is_dynamic => NativeMethods.ov_rank_is_dynamic(this);

        /// <summary>
        /// Static rank (fixed)
        /// </summary>
        public static ov_rank_t Static(long size) => new ov_rank_t(size, size);

        /// <summary>
        /// Dynamic rank
        /// </summary>
        public static ov_rank_t Dynamic => new ov_rank_t(-1, -1);
    }
}
