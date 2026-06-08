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
        /// Allocates memory tensor in device memory or wraps user-supplied memory handle
        /// using the specified tensor description and low-level device-specific parameters.
        /// </summary>
        /// <param name="context">A pointer to the ov_remote_context_t instance.</param>
        /// <param name="type">Defines the element type of the tensor.</param>
        /// <param name="shape">Defines the shape of the tensor.</param>
        /// <param name="object_args_size">Size of the low-level tensor object parameters.</param>
        /// <param name="remote_tensor">Pointer to returned ov_tensor_t that contains remote tensor instance.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_remote_context_create_tensor",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_remote_context_create_tensor(
            IntPtr context,
            uint type,
            ov_shape_t shape,
            ulong object_args_size,
            ref IntPtr remote_tensor);

        /// <summary>
        /// 使用原生 size_t 创建远程张量 / Create a remote tensor using native size_t.
        /// </summary>
        /// <param name="context">远程上下文指针 / Remote context pointer.</param>
        /// <param name="type">元素类型 / Element type.</param>
        /// <param name="shape">张量形状 / Tensor shape.</param>
        /// <param name="object_args_size">参数数量 / Argument count.</param>
        /// <param name="remote_tensor">返回的远程张量 / Returned remote tensor.</param>
        /// <returns>操作状态 / Operation status.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_remote_context_create_tensor",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_remote_context_create_tensor_native_size(
            IntPtr context,
            uint type,
            ov_shape_t shape,
            UIntPtr object_args_size,
            ref IntPtr remote_tensor);

        /// <summary>
        /// Returns name of a device on which underlying object is allocated.
        /// </summary>
        /// <param name="context">A pointer to the ov_remote_context_t instance.</param>
        /// <param name="device_name">Device name will be returned.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_remote_context_get_device_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_remote_context_get_device_name(
            IntPtr context,
            ref IntPtr device_name);

        /// <summary>
        /// Returns a string contains device-specific parameters required for low-level
        /// operations with the underlying object.
        /// </summary>
        /// <param name="context">A pointer to the ov_remote_context_t instance.</param>
        /// <param name="size">The size of param pairs.</param>
        /// <param name="params">Param name:value list.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_remote_context_get_params",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_remote_context_get_params(
            IntPtr context,
            ref ulong size,
            ref IntPtr @params);

        [DllImport("openvino_c", EntryPoint = "ov_remote_context_get_params",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_remote_context_get_params_native_size(
            IntPtr context,
            ref UIntPtr size,
            ref IntPtr @params);

        /// <summary>
        /// This method is used to create a host tensor object friendly for the device in current context.
        /// </summary>
        /// <param name="context">A pointer to the ov_remote_context_t instance.</param>
        /// <param name="type">Defines the element type of the tensor.</param>
        /// <param name="shape">Defines the shape of the tensor.</param>
        /// <param name="tensor">Pointer to ov_tensor_t that contains host tensor.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_remote_context_create_host_tensor",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_remote_context_create_host_tensor(
            IntPtr context,
            uint type,
            ov_shape_t shape,
            ref IntPtr tensor);

        /// <summary>
        /// Release the memory allocated by ov_remote_context_t.
        /// </summary>
        /// <param name="context">A pointer to the ov_remote_context_t to free memory.</param>
        [DllImport("openvino_c", EntryPoint = "ov_remote_context_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_remote_context_free(IntPtr context);

        /// <summary>
        /// Returns a string contains device-specific parameters required for low-level
        /// operations with underlying object.
        /// </summary>
        /// <param name="tensor">Pointer to ov_tensor_t that contains host tensor.</param>
        /// <param name="size">The size of param pairs.</param>
        /// <param name="params">Param name:value list.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_remote_tensor_get_params",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_remote_tensor_get_params(
            IntPtr tensor,
            ref ulong size,
            ref IntPtr @params);

        [DllImport("openvino_c", EntryPoint = "ov_remote_tensor_get_params",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_remote_tensor_get_params_native_size(
            IntPtr tensor,
            ref UIntPtr size,
            ref IntPtr @params);

        /// <summary>
        /// Returns name of a device on which underlying object is allocated.
        /// </summary>
        /// <param name="remote_tensor">A pointer to the remote tensor instance.</param>
        /// <param name="device_name">Device name will be return.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_remote_tensor_get_device_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_remote_tensor_get_device_name(
            IntPtr remote_tensor,
            ref IntPtr device_name);
    }
}
