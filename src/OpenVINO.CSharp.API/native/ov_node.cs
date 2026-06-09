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

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Get the number of output ports of the node.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_node_get_output_size",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_node_get_output_size(IntPtr node, ref ulong size);

        [DllImport("openvino_c", EntryPoint = "ov_node_get_output_size",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_node_get_output_size_native_size(IntPtr node, ref UIntPtr size);

        /// <summary>
        /// Get the output port of the node by index.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_node_get_output",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_node_get_output(IntPtr node, ulong idx, ref IntPtr output_port);

        [DllImport("openvino_c", EntryPoint = "ov_node_get_output",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_node_get_output_native_size(IntPtr node, UIntPtr idx, ref IntPtr output_port);

        /// <summary>
        /// Get the number of input ports of the node.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_node_get_input_size",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_node_get_input_size(IntPtr node, ref ulong size);

        [DllImport("openvino_c", EntryPoint = "ov_node_get_input_size",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_node_get_input_size_native_size(IntPtr node, ref UIntPtr size);

        /// <summary>
        /// Get the input port of the node by index.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_node_get_input",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_node_get_input(IntPtr node, ulong idx, ref IntPtr input_port);

        [DllImport("openvino_c", EntryPoint = "ov_node_get_input",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_node_get_input_native_size(IntPtr node, UIntPtr idx, ref IntPtr input_port);

        /// <summary>
        /// Get the input port of the node by name.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_node_get_input_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_node_get_input_by_name(IntPtr node, ref sbyte name, ref IntPtr input_port);

        [DllImport("openvino_c", EntryPoint = "ov_node_get_input_by_name",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_node_get_input_by_name_utf8(IntPtr node, IntPtr name, ref IntPtr input_port);

        /// <summary>
        /// Get the output port of the node by name.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_node_get_output_by_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_node_get_output_by_name(IntPtr node, ref sbyte name, ref IntPtr output_port);

        [DllImport("openvino_c", EntryPoint = "ov_node_get_output_by_name",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_node_get_output_by_name_utf8(IntPtr node, IntPtr name, ref IntPtr output_port);

        /// <summary>
        /// Get the name of the node.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_node_get_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_node_get_name(IntPtr node, ref IntPtr name);

        /// <summary>
        /// Get the friendly name of the node.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_node_get_friendly_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_node_get_friendly_name(IntPtr node, ref IntPtr friendly_name);

        /// <summary>
        /// Get the element type of the output port.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_port_get_element_type",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_port_get_element_type(IntPtr output, ref uint type);


        /// <summary>
        /// Get the partial shape of the output port.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_output_get_partial_shape",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_output_get_partial_shape(IntPtr output, IntPtr partial_shape);

        /// <summary>
        /// 获取只读端口的静态形状，调用方传入的 shape 结构由 native 函数填充。
        /// Gets the static shape of a const port. The caller-provided shape structure is filled by native code.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_const_port_get_shape",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_const_port_get_shape(IntPtr port, IntPtr shape);

        /// <summary>
        /// 获取端口的静态形状，调用方传入的 shape 结构由 native 函数填充。
        /// Gets the static shape of a port. The caller-provided shape structure is filled by native code.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_port_get_shape",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_port_get_shape(IntPtr port, IntPtr shape);

        /// <summary>
        /// 获取端口的部分形状，调用方传入的 partial shape 结构由 native 函数填充。
        /// Gets the partial shape of a port. The caller-provided partial shape structure is filled by native code.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_port_get_partial_shape",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_port_get_partial_shape(IntPtr port, IntPtr partial_shape);


        /// <summary>
        /// 获取端口任意名称。返回的 native 字符串指针需要按 OpenVINO C API 约定释放。
        /// Gets any name of a port. The returned native string pointer must be released according to OpenVINO C API rules.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_port_get_any_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_port_get_any_name(IntPtr port, ref IntPtr tensor_name);

        /// <summary>
        /// 释放由 OpenVINO C API 返回的输出端口句柄。
        /// Releases an output port handle returned by the OpenVINO C API.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_output_port_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_output_port_free(IntPtr port);

        /// <summary>
        /// 释放由 OpenVINO C API 返回的只读输出端口句柄。
        /// Releases a const output port handle returned by the OpenVINO C API.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_output_const_port_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_output_const_port_free(IntPtr port);

    }
}
