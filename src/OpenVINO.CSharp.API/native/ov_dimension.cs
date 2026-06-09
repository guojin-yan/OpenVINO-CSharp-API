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
        /// Check this dimension whether is dynamic
        /// </summary>
        /// <param name="dim">The dimension pointer that will be checked.</param>
        /// <returns>Boolean, true is dynamic and false is static.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_dimension_is_dynamic",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public extern static bool ov_dimension_is_dynamic(ov_dimension_t dim);
    }

    /// <summary>
    /// Structure representing a dimension with min and max bounds
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ov_dimension_t
    {
        /// <summary>
        /// 维度下界；静态维度时与 <see cref="max"/> 相同。
        /// Minimum dimension bound. For static dimensions this equals <see cref="max"/>.
        /// </summary>
        public long min;

        /// <summary>
        /// 维度上界；完全动态维度通常为 -1。
        /// Maximum dimension bound. Fully dynamic dimensions commonly use -1.
        /// </summary>
        public long max;

        /// <summary>
        /// 使用指定上下界创建维度结构。
        /// Creates a dimension structure with the specified bounds.
        /// </summary>
        /// <param name="min">维度下界 / Minimum dimension bound.</param>
        /// <param name="max">维度上界 / Maximum dimension bound.</param>
        public ov_dimension_t(long min, long max)
        {
            this.min = min;
            this.max = max;
        }

        /// <summary>
        /// Check if this dimension is dynamic
        /// </summary>
        public bool is_dynamic => NativeMethods.ov_dimension_is_dynamic(this);

        /// <summary>
        /// Static dimension (fixed size)
        /// </summary>
        public static ov_dimension_t Static(long size) => new ov_dimension_t(size, size);

        /// <summary>
        /// Dynamic dimension with range
        /// </summary>
        public static ov_dimension_t Dynamic(long min, long max) => new ov_dimension_t(min, max);

        /// <summary>
        /// Fully dynamic dimension
        /// </summary>
        public static ov_dimension_t FullyDynamic => new ov_dimension_t(-1, -1);
    }
}
