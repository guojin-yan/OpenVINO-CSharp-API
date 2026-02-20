// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

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
        public Layout(string layout) : base()
        {
            if (string.IsNullOrEmpty(layout))
                throw new ArgumentException("Layout string cannot be null or empty", nameof(layout));

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
        public static Layout NCHW => new Layout("NCHW");

        /// <summary>
        /// NHWC布局：批次、高度、宽度、通道 / NHWC: Batch, Height, Width, Channels
        /// </summary>
        public static Layout NHWC => new Layout("NHWC");

        /// <summary>
        /// NCDHW布局：3D数据的NCHW变体 / NCDHW: 3D data NCHW variant
        /// </summary>
        public static Layout NCDHW => new Layout("NCDHW");

        /// <summary>
        /// NDHWC布局：3D数据的NHWC变体 / NDHWC: 3D data NHWC variant
        /// </summary>
        public static Layout NDHWC => new Layout("NDHWC");

        /// <summary>
        /// NC布局：批次、通道 / NC: Batch, Channels
        /// </summary>
        public static Layout NC => new Layout("NC");

        /// <summary>
        /// CN布局：通道、批次 / CN: Channels, Batch
        /// </summary>
        public static Layout CN => new Layout("CN");

        /// <summary>
        /// HW布局：高度、宽度 / HW: Height, Width
        /// </summary>
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
        public static Layout DHW => new Layout("DHW");

        #endregion

        #region 对象方法 / Object Methods

        /// <inheritdoc/>
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
