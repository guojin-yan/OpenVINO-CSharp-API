// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp
{
    /// <summary>
    /// 性能分析信息 / Profiling information for a node
    /// <para>提供神经网络节点执行的性能分析数据。/ Provides profiling data for neural network node execution.</para>
    /// </summary>
    /// <example>
    /// 使用示例 / Usage example:
    /// <code>
    /// // 假设从推理请求获取分析信息 / Assume getting profiling info from inference request
    /// var profilingInfo = inferRequest.GetProfilingInfo();
    /// foreach (var info in profilingInfo)
    /// {
    ///     Console.WriteLine(info);
    ///     // 输出: node_name: status=EXECUTED, real_time=100us, cpu_time=95us, exec_type=Convolution
    /// }
    /// </code>
    /// </example>
    public class ProfilingInfo
    {
        /// <summary>
        /// 节点状态枚举 / Defines the general status of a node
        /// </summary>
        public enum Status
        {
            /// <summary>
            /// 节点未执行 / A node is not executed
            /// </summary>
            NOT_RUN = 0,
            /// <summary>
            /// 节点在图优化阶段被优化掉 / A node is optimized out during graph optimization phase
            /// </summary>
            OPTIMIZED_OUT = 1,
            /// <summary>
            /// 节点已执行 / A node is executed
            /// </summary>
            EXECUTED = 2
        }

        /// <summary>
        /// 节点状态 / The status of the node
        /// </summary>
        /// <value>执行状态 / Execution status</value>
        public Status status { get; set; }

        /// <summary>
        /// 节点执行的总实际时间（微秒）/ The absolute time, in microseconds, that the node ran (in total)
        /// </summary>
        /// <value>实际时间（微秒）/ Real time in microseconds</value>
        public long real_time { get; set; }

        /// <summary>
        /// 节点在主机CPU上运行的净时间 / The net host CPU time that the node ran
        /// </summary>
        /// <value>CPU时间（微秒）/ CPU time in microseconds</value>
        public long cpu_time { get; set; }

        /// <summary>
        /// 节点名称 / Name of a node
        /// </summary>
        /// <value>节点名称 / Node name</value>
        public string node_name { get; set; } = string.Empty;

        /// <summary>
        /// 执行单元类型 / Execution type of a unit
        /// </summary>
        /// <value>执行类型（如Convolution, ReLU等）/ Execution type (e.g., Convolution, ReLU, etc.)</value>
        public string exec_type { get; set; } = string.Empty;

        /// <summary>
        /// 节点类型 / Node type
        /// </summary>
        /// <value>节点类型 / Node type</value>
        public string node_type { get; set; } = string.Empty;

        /// <summary>
        /// 转换为字符串表示 / Convert to string representation
        /// </summary>
        /// <returns>格式化的性能信息字符串 / Formatted profiling information string</returns>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// ProfilingInfo info = new ProfilingInfo
        /// {
        ///     node_name = "conv1",
        ///     status = ProfilingInfo.Status.EXECUTED,
        ///     real_time = 150,
        ///     cpu_time = 140,
        ///     exec_type = "Convolution"
        /// };
        /// string str = info.ToString();
        /// // 结果: "conv1: status=EXECUTED, real_time=150us, cpu_time=140us, exec_type=Convolution"
        /// </code>
        /// </example>
        public override string ToString()
        {
            return $"{node_name}: status={status}, real_time={real_time}us, cpu_time={cpu_time}us, exec_type={exec_type}";
        }
    }
}
