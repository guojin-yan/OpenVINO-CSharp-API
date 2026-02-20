// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp
{
    /// <summary>
    /// ov命名空间下的全局结构体 / Global structures under ov namespace
    /// </summary>
    public static partial class Ov
    {
        /// <summary>
        /// 静态形状结构 / Represents a static shape
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ov_shape
        {
            /// <summary>
            /// 形状维度数量 / The rank of shape
            /// </summary>
            public long rank;
            /// <summary>
            /// 形状维度数组指针 / The dims of shape
            /// </summary>
            public IntPtr dims_ptr;
            /// <summary>
            /// 获取形状的维度数组 / Get the dims of shape
            /// </summary>
            /// <returns>形状维度数组 / The dims of shape</returns>
            public long[] get_dims()
            {
                long[] dims = new long[rank];
                Marshal.Copy(dims_ptr, dims, 0, (int)rank);
                return dims;
            }
        }

#pragma warning disable CS1591
        /// <summary>
        /// 部分形状结构，表示可能部分或完全动态的形状 / It represents a shape that may be partially or totally dynamic
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ov_partial_shape
        {
            /// <summary>
            /// 维度等级 / The rank
            /// </summary>
            public ov_dimension rank;
            /// <summary>
            /// 维度数据指针 / The dimension
            /// </summary>
            public IntPtr dims;
        }

        /// <summary>
        /// 等级结构体，等同于ov::Rank / This is a structure interface equal to ov::Rank
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ov_rank
        {
            /// <summary>
            /// 等级的下限（包含） / The lower inclusive limit for the Rank
            /// </summary>
            public long min;
            /// <summary>
            /// 等级的上限（包含） / The upper inclusive limit for the Rank
            /// </summary>
            public long max;
        };

        /// <summary>
        /// 维度结构体，等同于ov::Dimension / This is a structure interface equal to ov::Dimension
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ov_dimension
        {
            /// <summary>
            /// 维度的下限（包含） / The lower inclusive limit for the dimension
            /// </summary>
            public long min;
            /// <summary>
            /// 维度的上限（包含） / The upper inclusive limit for the dimension
            /// </summary>
            public long max;
        };

        /// <summary>
        /// 性能分析信息结构体，表示每个操作的基本推理分析信息 / Represents basic inference profiling information per operation
        /// </summary>
        public struct ProfilingInfo
        {
            /// <summary>
            /// 节点状态枚举 / Defines the general status of a node
            /// </summary>
            public enum Status
            {
                /// <summary>
                /// 节点未执行 / A node is not executed
                /// </summary>
                NOT_RUN,
                /// <summary>
                /// 节点在图优化阶段被优化掉 / A node is optimized out during graph optimization phase
                /// </summary>
                OPTIMIZED_OUT,
                /// <summary>
                /// 节点已执行 / A node is executed
                /// </summary>
                EXECUTED
            };

            /// <summary>
            /// 节点状态 / The status of node
            /// </summary>
            public Status status;
            /// <summary>
            /// 节点运行的绝对时间（微秒） / The absolute time, in microseconds, that the node ran (in total)
            /// </summary>
            public ulong real_time;
            /// <summary>
            /// 节点运行的净主机CPU时间 / The net host CPU time that the node ran
            /// </summary>
            public ulong cpu_time;
            /// <summary>
            /// 节点名称 / Name of a node
            /// </summary>
            public string node_name;
            /// <summary>
            /// 执行单元类型 / Execution type of a unit
            /// </summary>
            public string exec_type;
            /// <summary>
            /// 节点类型 / Node type
            /// </summary>
            public string node_type;
        };

        /// <summary>
        /// 性能分析信息列表结构体 / A list of profiling info data
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ov_profiling_info_list
        {
            /// <summary>
            /// 性能分析信息数组指针 / The list of ProfilingInfo
            /// </summary>
            public IntPtr profiling_infos;
            /// <summary>
            /// 列表大小 / The list size
            /// </summary>
            public ulong size;
        };
    }
}
