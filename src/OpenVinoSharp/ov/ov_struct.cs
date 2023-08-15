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
            public long min;
            /// <summary>
            /// The upper inclusive limit for the Rank.
            /// </summary>
            public long max;
        };

        /// <summary>
        /// This is a structure interface equal to ov::Dimension
        /// </summary>
        public struct ov_dimension
        {
            /// <summary>
            /// The lower inclusive limit for the dimension.
            /// </summary>
            public long min;
            /// <summary>
            /// The upper inclusive limit for the dimension.
            /// </summary>
            public long max;
        };

        /// <summary>
        /// Represents basic inference profiling information per operation.
        /// </summary>
        /// <remarks> 
        /// If the operation is executed using tiling, the sum time per each tile is indicated as the total execution time. 
        /// Due to parallel execution, the total execution time for all nodes might be greater than the total inference time.
        /// </remarks>
        public struct ProfilingInfo
        {
            /// <summary>
            /// Defines the general status of a node.
            /// </summary>
            public enum Status
            {
                /// <summary>
                /// A node is not executed.
                /// </summary>
                NOT_RUN,
                /// <summary>
                /// A node is optimized out during graph optimization phase.
                /// </summary>
                OPTIMIZED_OUT,
                /// <summary>
                /// A node is executed.
                /// </summary>
                EXECUTED
            };
            /// <summary>
            /// The absolute time, in microseconds, that the node ran (in total).
            /// </summary>
            public ulong real_time;
            /// <summary>
            /// The net host CPU time that the node ran.
            /// </summary>
            public ulong cpu_time;
            /// <summary>
            /// Name of a node.
            /// </summary>
            public string node_name;
            /// <summary>
            /// Execution type of a unit.
            /// </summary>
            public string exec_type;
            /// <summary>
            /// Node type.
            /// </summary>
            public string node_type;
        };
        /// <summary>
        /// A list of profiling info data
        /// </summary>
        public struct ov_profiling_info_list
        {
            /// <summary>
            /// The list of ProfilingInfo
            /// </summary>
            public IntPtr profiling_infos;
            /// <summary>
            /// he list size
            /// </summary>
            public ulong size;
        };

    }
}
