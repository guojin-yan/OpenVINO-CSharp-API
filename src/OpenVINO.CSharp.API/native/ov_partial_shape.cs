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
        #region Partial Shape Creation

        /// <summary>
        /// Initialize a partial shape with static rank and dynamic dimension.
        /// </summary>
        /// <param name="rank">Support static rank.</param>
        /// <param name="dims">Support dynamic and static dimension.</param>
        /// <param name="partial_shape_obj">The partial shape object to initialize.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_partial_shape_create",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_partial_shape_create(
            long rank,
            [MarshalAs(UnmanagedType.LPArray)] ov_dimension_t[] dims,
            ref ov_partial_shape_t partial_shape_obj);

        /// <summary>
        /// Initialize a partial shape with dynamic rank and dynamic dimension.
        /// </summary>
        /// <param name="rank">Support dynamic and static rank.</param>
        /// <param name="dims">Support dynamic and static dimension.</param>
        /// <param name="partial_shape_obj">The partial shape object to initialize.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_partial_shape_create_dynamic",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_partial_shape_create_dynamic(
            ov_rank_t rank,
            [MarshalAs(UnmanagedType.LPArray)] ov_dimension_t[] dims,
            ref ov_partial_shape_t partial_shape_obj);

        /// <summary>
        /// Initialize a partial shape with static rank and static dimension.
        /// </summary>
        /// <param name="rank">Support static rank.</param>
        /// <param name="dims">Support static dimension.</param>
        /// <param name="partial_shape_obj">The partial shape object to initialize.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_partial_shape_create_static",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_partial_shape_create_static(
            long rank,
            [MarshalAs(UnmanagedType.LPArray)] long[] dims,
            ref ov_partial_shape_t partial_shape_obj);

        #endregion

        #region Partial Shape Conversion

        /// <summary>
        /// Convert partial shape without dynamic data to a static shape.
        /// </summary>
        /// <param name="partial_shape">The partial_shape pointer.</param>
        /// <param name="shape">The shape pointer.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_partial_shape_to_shape",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_partial_shape_to_shape(
            ov_partial_shape_t partial_shape,
            ref ov_shape_t shape);

        /// <summary>
        /// Convert shape to partial shape.
        /// </summary>
        /// <param name="shape">The shape pointer.</param>
        /// <param name="partial_shape">The partial_shape pointer.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_shape_to_partial_shape",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_shape_to_partial_shape(
            ov_shape_t shape,
            ref ov_partial_shape_t partial_shape);

        /// <summary>
        /// Helper function, convert a partial shape to readable string.
        /// </summary>
        /// <param name="partial_shape">The partial_shape pointer.</param>
        /// <returns>A string represents partial_shape's content.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_partial_shape_to_string",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr ov_partial_shape_to_string(ov_partial_shape_t partial_shape);

        #endregion

        #region Partial Shape Query

        /// <summary>
        /// Check this partial_shape whether is dynamic
        /// </summary>
        /// <param name="partial_shape">The partial_shape pointer.</param>
        /// <returns>True if partial shape is dynamic.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_partial_shape_is_dynamic",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool ov_partial_shape_is_dynamic(ov_partial_shape_t partial_shape);

        /// <summary>
        /// Release internal memory allocated in partial shape.
        /// </summary>
        /// <param name="partial_shape">The object's internal memory will be released.</param>
        [DllImport("openvino_c", EntryPoint = "ov_partial_shape_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_partial_shape_free(ref ov_partial_shape_t partial_shape);

        #endregion
    }

    /// <summary>
    /// Structure representing a partial shape with rank and dimensions
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ov_partial_shape_t
    {
        /// <summary>
        /// 部分形状的 rank，可为静态或动态。
        /// Partial shape rank, which may be static or dynamic.
        /// </summary>
        public ov_rank_t rank;

        /// <summary>
        /// 指向 native 维度数组的指针，该内存由 OpenVINO C API 管理。
        /// Pointer to the native dimension array. The memory is managed by the OpenVINO C API.
        /// </summary>
        public IntPtr dims;
    }
}
