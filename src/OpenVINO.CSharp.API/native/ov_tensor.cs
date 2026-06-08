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
        /// 创建张量 / Create a tensor
        /// </summary>
        /// <param name="type">元素类型 / Element type</param>
        /// <param name="shape">张量形状 / Tensor shape</param>
        /// <param name="tensor">返回的张量指针 / Returned tensor pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_create",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_create(uint type, ov_shape_t shape, ref IntPtr tensor);

        /// <summary>
        /// 从主机指针创建张量 / Create a tensor from host pointer
        /// </summary>
        /// <param name="type">元素类型 / Element type</param>
        /// <param name="shape">张量形状 / Tensor shape</param>
        /// <param name="host_ptr">主机数据指针 / Host data pointer</param>
        /// <param name="tensor">返回的张量指针 / Returned tensor pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        /// <remarks>
        /// 此函数不会复制数据，而是直接使用主机指针指向的内存。
        /// This function does not copy data, but directly uses the memory pointed to by the host pointer.
        /// </remarks>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_create_from_host_ptr",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_create_from_host_ptr(uint type, ov_shape_t shape, IntPtr host_ptr, ref IntPtr tensor);

        /// <summary>
        /// 释放 ov_tensor_t 分配的内存 / Release the memory allocated by ov_tensor_t
        /// </summary>
        /// <param name="tensor">张量指针 / Tensor pointer</param>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_tensor_free(IntPtr tensor);

        /// <summary>
        /// 为张量设置新形状 / Set new shape for tensor
        /// </summary>
        /// <param name="tensor">张量指针 / Tensor pointer</param>
        /// <param name="shape">新形状 / New shape</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_set_shape",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_set_shape(IntPtr tensor, ov_shape_t shape);

        /// <summary>
        /// 获取张量形状 / Get the tensor shape
        /// </summary>
        /// <param name="tensor">张量指针 / Tensor pointer</param>
        /// <param name="shape">返回的形状指针 / Returned shape pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_get_shape",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_get_shape(IntPtr tensor, IntPtr shape);

        /// <summary>
        /// 获取张量元素类型 / Get the tensor element type
        /// </summary>
        /// <param name="tensor">张量指针 / Tensor pointer</param>
        /// <param name="type">返回的元素类型 / Returned element type</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_get_element_type",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_get_element_type(IntPtr tensor, out uint type);

        /// <summary>
        /// 获取张量大小（元素数量）/ Get the tensor size (number of elements)
        /// </summary>
        /// <param name="tensor">张量指针 / Tensor pointer</param>
        /// <param name="size">返回的元素数量 / Returned number of elements</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_get_size",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_get_size(IntPtr tensor, ref ulong size);

        [DllImport("openvino_c", EntryPoint = "ov_tensor_get_size",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_tensor_get_size_native_size(IntPtr tensor, ref UIntPtr size);

        /// <summary>
        /// 获取张量字节大小 / Get the tensor byte size
        /// </summary>
        /// <param name="tensor">张量指针 / Tensor pointer</param>
        /// <param name="size">返回的字节大小 / Returned byte size</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_get_byte_size",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_get_byte_size(IntPtr tensor, ref ulong size);

        [DllImport("openvino_c", EntryPoint = "ov_tensor_get_byte_size",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_tensor_get_byte_size_native_size(IntPtr tensor, ref UIntPtr size);

        /// <summary>
        /// 获取张量数据指针 / Get the tensor data pointer
        /// </summary>
        /// <param name="tensor">张量指针 / Tensor pointer</param>
        /// <param name="data">返回的数据指针 / Returned data pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_data",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_data(IntPtr tensor, ref IntPtr data);

        /// <summary>
        /// 从字符串数组创建张量 / Create a tensor from string array
        /// </summary>
        /// <param name="string_array">字符串数组指针 / String array pointer</param>
        /// <param name="array_size">数组大小 / Array size</param>
        /// <param name="shape">张量形状 / Tensor shape</param>
        /// <param name="tensor">返回的张量指针 / Returned tensor pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_create_from_string_array",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_create_from_string_array(
            IntPtr string_array,
            ulong array_size,
            ov_shape_t shape,
            ref IntPtr tensor);

        [DllImport("openvino_c", EntryPoint = "ov_tensor_create_from_string_array",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_tensor_create_from_string_array_native_size(
            IntPtr string_array,
            UIntPtr array_size,
            ov_shape_t shape,
            ref IntPtr tensor);

        /// <summary>
        /// 为张量设置字符串数据 / Set string data for tensor
        /// </summary>
        /// <param name="tensor">张量指针 / Tensor pointer</param>
        /// <param name="string_array">字符串数组指针 / String array pointer</param>
        /// <param name="array_size">数组大小 / Array size</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_tensor_set_string_data",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_set_string_data(
            IntPtr tensor,
            IntPtr string_array,
            ulong array_size);

        [DllImport("openvino_c", EntryPoint = "ov_tensor_set_string_data",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_tensor_set_string_data_native_size(
            IntPtr tensor,
            IntPtr string_array,
            UIntPtr array_size);
    }
}
