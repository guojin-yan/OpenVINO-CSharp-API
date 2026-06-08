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
