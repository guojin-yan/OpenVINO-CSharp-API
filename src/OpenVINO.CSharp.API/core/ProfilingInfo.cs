//  ========================================================================
//  【项目名称】OpenVINO C# API
//  【项目描述】OpenVINO™ 的 C# 语言绑定库，提供高性能深度学习推理能力
//  【版权声明】© 2026-2025 Guojin Yan. All Rights Reserved.
//  【开源协议】Apache-2.0 License（请遵守许可证条款）
//  -----------------------------------------------------------------------
//  【功能简介】
//  1. 完整的 OpenVINO™ C API 封装，提供 C# 友好的面向对象接口。
//  2. 支持模型加载、编译、推理全流程操作。
//  3. 支持 CPU、GPU、VPU 等多种推理设备。
//  4. 支持同步推理和异步推理模式。
//  5. 支持预处理和后处理流水线配置。
//  6. 支持动态形状和批量推理。
//  7. 支持模型缓存和性能分析。
//  8. 支持远程上下文（Remote Context）和零拷贝推理。
//  9. 支持 .NET Framework 4.6.1+、.NET Core 2.0+、.NET 5/6/7/8/9+。
//  10. 提供推理请求对象池，优化高并发场景性能。
//  11. 提供完善的异常处理和日志记录机制。
//  12. 提供丰富的单元测试和集成测试用例。
//  -----------------------------------------------------------------------
//  【官方资源】
//  📌 GitHub仓库：https://github.com/guojin-yan/OpenVINO-CSharp-API
//  📌 NuGet包：https://www.nuget.org/packages/OpenVINO.CSharp.API
//  📌 在线文档：https://guojin-yan.github.io/OpenVINO-CSharp-API/index.html
//  📌 示例代码：https://github.com/guojin-yan/OpenVINO-CSharp-API/tree/csharp3.3/samples
//  -----------------------------------------------------------------------
//  【社区支持】
//  💬 QQ交流群：945057948（加入获取技术支持）
//  📱 微信公众号：CSharp与边缘模型部署（教程+案例）
//  📝 CSDN博客：https://guojin.blog.csdn.net（技术文章）
//  -----------------------------------------------------------------------
//  【联系我们】
//  ✉ 项目维护：guojin_yjs@cumt.edu.cn
//  💬 微信咨询：15253793309
//  🐛 Bug反馈：https://github.com/guojin-yan/OpenVINO-CSharp-API/issues
//  💡 功能建议：https://github.com/guojin-yan/OpenVINO-CSharp-API/discussions/landing
//  -----------------------------------------------------------------------
//  【致谢】
//  本项目基于 Intel® OpenVINO™ 工具包开发，感谢 Intel 提供的优秀开源项目。
//  OpenVINO™ 是 Intel Corporation 的商标。
//  ========================================================================
//  
//  【许可声明】
//  1. 本项目采用 Apache-2.0 License 开源协议，允许自由使用、修改和分发。
//  2. 使用本项目即表示您同意 Apache-2.0 License 许可证的所有条款。
//  3. 本项目按"原样"提供，不提供任何形式的担保。
//  4. 使用本项目产生的任何风险由使用者自行承担。
//  5. 修改或分发时请保留原始版权声明和许可声明。
//  ========================================================================
//

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
