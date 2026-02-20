// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

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
            ExceptionHandler.ThrowOnError(ov_layout_create(layout, ref ptr));
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
            string result = Marshal.PtrToStringAnsi(strPtr) ?? _layoutDesc ?? "<empty>";
            ov_free(strPtr);
            return result;
        }

        #endregion
    }
}
