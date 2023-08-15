using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp
{ 
    public static partial class Ov
    {
        /// <summary>
        /// Reprents a static shape.
        /// </summary>
        public struct ov_shape
        {
            /// <summary>
            /// the rank of shape
            /// </summary>
            public long rank;
            /// <summary>
            /// the dims of shape
            /// </summary>
            public IntPtr dims_ptr;
            /// <summary>
            /// Get the dims of shape
            /// </summary>
            /// <returns>the dims of shape</returns>
            public long[] get_dims()
            {
                long[] dims = new long[rank];
                Marshal.Copy(dims_ptr, dims, 0, (int)rank);
                return dims;
            }
        }

        /// <summary>
        /// It represents a shape that may be partially or totally dynamic.
        /// </summary>
        /// <remarks>
        /// <para>Dynamic rank. (Informal notation: `?`)</para>
        /// <para>Static rank, but dynamic dimensions on some or all axes.
        /// (Informal notation examples: `{1,2,?,4}`, `{?,?,?}`)</para>
        /// <para> Static rank, and static dimensions on all axes.
        /// (Informal notation examples: `{1,2,3,4}`, `{6}`, `{}`)</para>
        /// </remarks>
        public struct ov_partial_shape
        {

#if NET7_0_OR_GREATER || NET6_0_OR_GREATER
            /// <summary>
            /// The rank
            /// </summary>
            public ov_rank rank;
            /// <summary>
            /// The dimension
            /// </summary>
            public IntPtr dims = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ov_dimension)));
            /// <summary>
            /// Default Constructor
            /// </summary>
            public ov_partial_shape()
            {
                rank = new ov_rank();
                dims = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ov_dimension)));
            }
#else
            public ov_rank rank;
            public IntPtr dims;
#endif
        }


        /// <summary>
        /// This is a structure interface equal to ov::Rank
        /// </summary>
        public struct ov_rank
        {
            /// <summary>
            /// The lower inclusive limit for the Rank.
            /// </summary>
            long min;
            /// <summary>
            /// The upper inclusive limit for the Rank.
            /// </summary>
            long max;
        };

        /// <summary>
        /// This is a structure interface equal to ov::Dimension
        /// </summary>
        public struct ov_dimension
        {
            /// <summary>
            /// The lower inclusive limit for the dimension.
            /// </summary>
            long min;
            /// <summary>
            /// The upper inclusive limit for the dimension.
            /// </summary>
            long max;
        };

    }
}
