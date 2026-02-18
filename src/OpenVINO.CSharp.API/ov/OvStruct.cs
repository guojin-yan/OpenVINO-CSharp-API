// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp
{
    /// <summary>
    /// Global structures under ov namespace
    /// </summary>
    public static partial class Ov
    {
        /// <summary>
        /// Reprents a static shape.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
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

#pragma warning disable CS1591
        /// <summary>
        /// It represents a shape that may be partially or totally dynamic.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ov_partial_shape
        {
            /// <summary>
            /// The rank
            /// </summary>
            public ov_dimension rank;
            /// <summary>
            /// The dimension
            /// </summary>
            public IntPtr dims;
        }

        /// <summary>
        /// This is a structure interface equal to ov::Rank
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
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
        [StructLayout(LayoutKind.Sequential)]
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

            public Status status;
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
        [StructLayout(LayoutKind.Sequential)]
        public struct ov_profiling_info_list
        {
            /// <summary>
            /// The list of ProfilingInfo
            /// </summary>
            public IntPtr profiling_infos;
            /// <summary>
            /// The list size
            /// </summary>
            public ulong size;
        };
    }
}
