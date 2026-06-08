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
using static OpenVinoSharp.native.NativeMethods;
using OpenVinoSharp.Internal;

namespace OpenVinoSharp
{
    /// <summary>
    /// 布局类 / Layout class
    /// <para>表示张量的维度布局。/ Represents the dimension layout of a tensor.</para>
    /// </summary>
    /// <example>
    /// 使用示例 / Usage example:
    /// <code>
    /// // 使用预定义布局 / Use predefined layouts
    /// Layout nchw = Layout.NCHW;
    /// Layout nhwc = Layout.NHWC;
    /// 
    /// // 创建自定义布局 / Create custom layout
    /// Layout custom = new Layout("NHWC");
    /// 
    /// // 输出布局字符串 / Output layout string
    /// Console.WriteLine(nchw.ToString()); // "NCHW"
    /// </code>
    /// </example>
    public class Layout : DisposableOvObject
    {
        #region 字段 / Fields

        private string? _layoutDesc;

        #endregion

        #region 构造函数 / Constructors

        /// <summary>
        /// 从布局字符串构造 / Construct from layout string
        /// </summary>
        /// <param name="layout">布局字符串 / Layout string</param>
        /// <exception cref="ArgumentException">当布局字符串为空时抛出 / Thrown when layout string is null or empty</exception>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// Layout layout = new Layout("NCHW");
        /// Console.WriteLine(layout); // "NCHW"
        /// </code>
        /// </example>
        public Layout(string layout) : base()
        {
            if (string.IsNullOrEmpty(layout))
                throw new ArgumentException("Layout string cannot be null or empty / 布局字符串不能为空", nameof(layout));

            _layoutDesc = layout;
            IntPtr ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(StringUtils.WithUtf8Ptr(
                layout,
                layoutPtr => ov_layout_create_utf8(layoutPtr, ref ptr)));
            _ptr = ptr;
        }

        /// <summary>
        /// 从原生指针构造 / Construct from native pointer
        /// </summary>
        /// <param name="ptr">原生布局指针 / Native layout pointer</param>
        public Layout(IntPtr ptr) : base(ptr) { }

        #endregion

        #region 资源释放 / Resource Disposal

        /// <inheritdoc/>
        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero && IsEnabledDispose)
            {
                ov_layout_free(_ptr);
            }
            base.DisposeUnmanaged();
        }

        #endregion

        #region 静态属性 / Static Properties

        /// <summary>
        /// NCHW布局：批次、通道、高度、宽度 / NCHW: Batch, Channels, Height, Width
        /// </summary>
        /// <remarks>常用于图像处理模型 / Commonly used in image processing models</remarks>
        public static Layout NCHW => new Layout("NCHW");

        /// <summary>
        /// NHWC布局：批次、高度、宽度、通道 / NHWC: Batch, Height, Width, Channels
        /// </summary>
        /// <remarks>TensorFlow常用格式 / Common format in TensorFlow</remarks>
        public static Layout NHWC => new Layout("NHWC");

        /// <summary>
        /// NCDHW布局：3D数据的NCHW变体 / NCDHW: 3D data NCHW variant
        /// </summary>
        /// <remarks>用于3D卷积（视频/体积数据）/ Used for 3D convolution (video/volumetric data)</remarks>
        public static Layout NCDHW => new Layout("NCDHW");

        /// <summary>
        /// NDHWC布局：3D数据的NHWC变体 / NDHWC: 3D data NHWC variant
        /// </summary>
        /// <remarks>用于3D卷积的替代格式 / Alternative format for 3D convolution</remarks>
        public static Layout NDHWC => new Layout("NDHWC");

        /// <summary>
        /// NC布局：批次、通道 / NC: Batch, Channels
        /// </summary>
        /// <remarks>用于全连接层输入 / Used for fully connected layer input</remarks>
        public static Layout NC => new Layout("NC");

        /// <summary>
        /// CN布局：通道、批次 / CN: Channels, Batch
        /// </summary>
        public static Layout CN => new Layout("CN");

        /// <summary>
        /// HW布局：高度、宽度 / HW: Height, Width
        /// </summary>
        /// <remarks>用于2D图像或掩码 / Used for 2D images or masks</remarks>
        public static Layout HW => new Layout("HW");

        /// <summary>
        /// WH布局：宽度、高度 / WH: Width, Height
        /// </summary>
        public static Layout WH => new Layout("WH");

        /// <summary>
        /// CHW布局：通道、高度、宽度 / CHW: Channels, Height, Width
        /// </summary>
        public static Layout CHW => new Layout("CHW");

        /// <summary>
        /// HWC布局：高度、宽度、通道 / HWC: Height, Width, Channels
        /// </summary>
        public static Layout HWC => new Layout("HWC");

        /// <summary>
        /// C布局：通道 / C: Channels
        /// </summary>
        /// <remarks>用于1D数据 / Used for 1D data</remarks>
        public static Layout C => new Layout("C");

        /// <summary>
        /// H布局：高度 / H: Height
        /// </summary>
        public static Layout H => new Layout("H");

        /// <summary>
        /// W布局：宽度 / W: Width
        /// </summary>
        public static Layout W => new Layout("W");

        /// <summary>
        /// DHW布局：深度、高度、宽度 / DHW: Depth, Height, Width
        /// </summary>
        /// <remarks>用于3D体积数据 / Used for 3D volumetric data</remarks>
        public static Layout DHW => new Layout("DHW");

        #endregion

        #region 对象方法 / Object Methods

        /// <inheritdoc/>
        /// <returns>布局字符串 / Layout string</returns>
        public override string ToString()
        {
            if (_ptr == IntPtr.Zero)
                return _layoutDesc ?? "<empty>";

            IntPtr strPtr = ov_layout_to_string(_ptr);
            try
            {
                string result = StringUtils.Utf8PtrToString(strPtr);
                return string.IsNullOrEmpty(result) ? _layoutDesc ?? "<empty>" : result;
            }
            finally
            {
                if (strPtr != IntPtr.Zero)
                    ov_free(strPtr);
            }
        }

        #endregion
    }
}
