// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp
{
    /// <summary>
    /// Profiling information for a node
    /// </summary>
    public class ProfilingInfo
    {
        /// <summary>
        /// Defines the general status of a node.
        /// </summary>
        public enum Status
        {
            /// <summary>
            /// A node is not executed.
            /// </summary>
            NOT_RUN = 0,
            /// <summary>
            /// A node is optimized out during graph optimization phase.
            /// </summary>
            OPTIMIZED_OUT = 1,
            /// <summary>
            /// A node is executed.
            /// </summary>
            EXECUTED = 2
        }

        /// <summary>
        /// The status of the node
        /// </summary>
        public Status status { get; set; }

        /// <summary>
        /// The absolute time, in microseconds, that the node ran (in total).
        /// </summary>
        public long real_time { get; set; }

        /// <summary>
        /// The net host CPU time that the node ran.
        /// </summary>
        public long cpu_time { get; set; }

        /// <summary>
        /// Name of a node.
        /// </summary>
        public string node_name { get; set; } = string.Empty;

        /// <summary>
        /// Execution type of a unit.
        /// </summary>
        public string exec_type { get; set; } = string.Empty;

        /// <summary>
        /// Node type.
        /// </summary>
        public string node_type { get; set; } = string.Empty;

        /// <summary>
        /// Convert to string representation
        /// </summary>
        public override string ToString()
        {
            return $"{node_name}: status={status}, real_time={real_time}us, cpu_time={cpu_time}us, exec_type={exec_type}";
        }
    }
}
