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

namespace OpenVinoSharp.preprocess
{
    /// <summary>
    /// 颜色格式枚举。
    /// Color format enumeration.
    /// </summary>
    public enum ColorFormat : uint
    {
        /// <summary>
        /// 未定义颜色格式。
        /// Undefined color format.
        /// </summary>
        UNDEFINE = 0U,

        /// <summary>
        /// 单平面 NV12 图像格式，Y 与 UV 数据位于同一个平面。
        /// Single-plane NV12 image format with Y and UV data in one plane.
        /// </summary>
        NV12_SINGLE_PLANE,

        /// <summary>
        /// 双平面 NV12 图像格式，Y 与 UV 数据位于两个独立平面。
        /// Two-plane NV12 image format with Y and UV data in separate planes.
        /// </summary>
        NV12_TWO_PLANES,

        /// <summary>
        /// 单平面 I420 图像格式，Y、U、V 数据位于同一个平面。
        /// Single-plane I420 image format with Y, U, and V data in one plane.
        /// </summary>
        I420_SINGLE_PLANE,

        /// <summary>
        /// 三平面 I420 图像格式，Y、U、V 数据位于三个独立平面。
        /// Three-plane I420 image format with Y, U, and V data in separate planes.
        /// </summary>
        I420_THREE_PLANES,

        /// <summary>
        /// RGB 三通道颜色格式。
        /// Three-channel RGB color format.
        /// </summary>
        RGB,

        /// <summary>
        /// BGR 三通道颜色格式。
        /// Three-channel BGR color format.
        /// </summary>
        BGR,

        /// <summary>
        /// 单通道灰度颜色格式。
        /// Single-channel grayscale color format.
        /// </summary>
        GRAY,

        /// <summary>
        /// RGBX 四通道颜色格式，第四通道通常作为填充通道。
        /// Four-channel RGBX color format where the fourth channel is usually padding.
        /// </summary>
        RGBX,

        /// <summary>
        /// BGRX 四通道颜色格式，第四通道通常作为填充通道。
        /// Four-channel BGRX color format where the fourth channel is usually padding.
        /// </summary>
        BGRX
    }

    /// <summary>
    /// 尺寸调整算法枚举。
    /// Resize algorithm enumeration.
    /// </summary>
    public enum ResizeAlgorithm : uint
    {
        /// <summary>
        /// 线性插值。
        /// Linear interpolation.
        /// </summary>
        RESIZE_LINEAR,

        /// <summary>
        /// 三次插值。
        /// Cubic interpolation.
        /// </summary>
        RESIZE_CUBIC,

        /// <summary>
        /// 最近邻插值。
        /// Nearest-neighbor interpolation.
        /// </summary>
        RESIZE_NEAREST
    }

    /// <summary>
    /// 填充模式枚举。
    /// Padding mode enumeration.
    /// </summary>
    public enum PaddingMode : uint
    {
        /// <summary>
        /// 使用常量值填充。
        /// Pads with a constant value.
        /// </summary>
        CONSTANT = 0,

        /// <summary>
        /// 使用边缘值填充。
        /// Pads with edge values.
        /// </summary>
        EDGE,

        /// <summary>
        /// 使用反射值填充，不重复边界元素。
        /// Pads with reflected values without repeating border elements.
        /// </summary>
        REFLECT,

        /// <summary>
        /// 使用对称反射值填充，包含边界元素。
        /// Pads with symmetric reflected values including border elements.
        /// </summary>
        SYMMETRIC
    }
}
