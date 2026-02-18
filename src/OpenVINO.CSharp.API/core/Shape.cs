// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using static OpenVinoSharp.native.NativeMethods;
using OpenVinoSharp.Internal;

namespace OpenVinoSharp
{
    /// <summary>
    /// 形状结构 / Shape structure
    /// <para>表示张量的多维形状。/ Represents the multi-dimensional shape of a tensor.</para>
    /// </summary>
    public class Shape : DisposableOvObject
    {
        #region 结构体定义 / Structure Definitions

        /// <summary>
        /// 原生形状结构体 / Native shape structure
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct ov_shape_t
        {
            /// <summary>维度数量 / Number of dimensions</summary>
            public ulong rank;
            /// <summary>维度指针 / Dimensions pointer</summary>
            public IntPtr dims;
        }

        #endregion

        #region 字段 / Fields

        /// <summary>
        /// 内部形状指针 / Internal shape pointer
        /// </summary>
        internal new IntPtr _ptr;

        /// <summary>
        /// 维度数据指针 / Dimension data pointer
        /// </summary>
        private IntPtr _dims_ptr = IntPtr.Zero;

        /// <summary>
        /// 维度数组 / Dimension array
        /// </summary>
        private long[] _dims_array;

        #endregion

        #region 构造函数 / Constructors

        /// <summary>
        /// 从原生指针构造 / Construct from native pointer
        /// </summary>
        /// <param name="ptr">原生形状指针 / Native shape pointer</param>
        internal Shape(IntPtr ptr) : base()
        {
            _ptr = ptr;
        }

        /// <summary>
        /// 从维度数组构造 / Construct from dimension array
        /// </summary>
        /// <param name="dims">维度数组 / Dimension array</param>
        public Shape(long[] dims) : base()
        {
            _dims_array = dims ?? throw new ArgumentNullException(nameof(dims));
            int size = dims.Length;

            // 分配非托管内存 / Allocate unmanaged memory
            _dims_ptr = Marshal.AllocHGlobal(sizeof(long) * size);
            unsafe
            {
                fixed (void* srcPtr = dims)
                {
                    Buffer.MemoryCopy(
                        srcPtr,
                        _dims_ptr.ToPointer(),
                        sizeof(long) * size,
                        sizeof(long) * size);
                }
            }

            // 分配形状结构体内存 / Allocate shape structure memory
            _ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ov_shape_t)));
            Marshal.StructureToPtr(new ov_shape_t
            {
                rank = (ulong)size,
                dims = _dims_ptr
            }, _ptr, false);
        }

        /// <summary>
        /// 从尺寸向量构造 / Construct from size vector
        /// </summary>
        /// <param name="dims">尺寸数组 / Size array</param>
        public static Shape FromIntArray(int[] dims)
        {
            if (dims == null)
                throw new ArgumentNullException(nameof(dims));

            long[] longDims = new long[dims.Length];
            for (int i = 0; i < dims.Length; i++)
                longDims[i] = dims[i];

            return new Shape(longDims);
        }

        #endregion

        #region 资源释放 / Resource Disposal

        /// <inheritdoc/>
        protected override void DisposeUnmanaged()
        {
            // 释放形状结构体 / Free shape structure
            if (_ptr != IntPtr.Zero && _dims_array != null)
            {
                Marshal.FreeHGlobal(_ptr);
                _ptr = IntPtr.Zero;
            }

            // 释放维度数组内存 / Free dimension array memory
            if (_dims_ptr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_dims_ptr);
                _dims_ptr = IntPtr.Zero;
            }

            base.DisposeUnmanaged();
        }

        #endregion

        #region 维度查询 / Dimension Queries

        /// <summary>
        /// 获取维度数组 / Get dimension array
        /// </summary>
        /// <returns>维度数组 / Dimension array</returns>
        public long[] get_dims()
        {
            ThrowIfDisposed();

            if (_dims_array != null)
                return _dims_array;

            ov_shape_t shape = Marshal.PtrToStructure<ov_shape_t>(_ptr);
            long[] dims = new long[shape.rank];
            Marshal.Copy(shape.dims, dims, 0, (int)shape.rank);
            return dims;
        }

        /// <summary>
        /// 获取维度数量 / Get number of dimensions (rank)
        /// </summary>
        /// <returns>维度数量 / Number of dimensions</returns>
        public ulong get_rank()
        {
            ThrowIfDisposed();
            return _dims_array != null
                ? (ulong)_dims_array.Length
                : Marshal.PtrToStructure<ov_shape_t>(_ptr).rank;
        }

        /// <summary>
        /// 获取指定索引的维度 / Get dimension at specified index
        /// </summary>
        /// <param name="index">维度索引 / Dimension index</param>
        /// <returns>维度值 / Dimension value</returns>
        public long get_dim(int index)
        {
            ThrowIfDisposed();

            if (_dims_array != null)
                return _dims_array[index];

            ov_shape_t shape = Marshal.PtrToStructure<ov_shape_t>(_ptr);
            unsafe
            {
                long* dims = (long*)shape.dims.ToPointer();
                return dims[index];
            }
        }

        /// <summary>
        /// 获取元素总数 / Get total number of elements
        /// </summary>
        /// <returns>元素总数 / Total element count</returns>
        public long get_total_elements()
        {
            ThrowIfDisposed();
            long[] dims = get_dims();
            long total = 1;
            foreach (var dim in dims)
            {
                if (dim <= 0)
                    return -1; // 动态维度 / Dynamic dimension
                total *= dim;
            }
            return total;
        }

        /// <summary>
        /// 转换为字符串表示 / Convert to string representation
        /// </summary>
        /// <returns>形状字符串 / Shape string</returns>
        public override string ToString()
        {
            ThrowIfDisposed();
            return string.Format("Shape({0})", string.Join(", ", get_dims()));
        }

        #endregion

        #region 工厂方法 / Factory Methods

        /// <summary>
        /// 创建标量形状 / Create scalar shape
        /// </summary>
        /// <returns>标量形状 / Scalar shape</returns>
        public static Shape scalar() => new Shape(new long[0]);

        /// <summary>
        /// 创建一维形状 / Create one-dimensional shape
        /// </summary>
        /// <param name="dim0">第一维大小 / First dimension size</param>
        /// <returns>一维形状 / One-dimensional shape</returns>
        public static Shape one_dim(long dim0) => new Shape(new long[] { dim0 });

        /// <summary>
        /// 创建二维形状 / Create two-dimensional shape
        /// </summary>
        /// <param name="dim0">第一维大小 / First dimension size</param>
        /// <param name="dim1">第二维大小 / Second dimension size</param>
        /// <returns>二维形状 / Two-dimensional shape</returns>
        public static Shape two_dim(long dim0, long dim1) => new Shape(new long[] { dim0, dim1 });

        /// <summary>
        /// 创建三维形状 / Create three-dimensional shape
        /// </summary>
        /// <param name="dim0">第一维大小 / First dimension size</param>
        /// <param name="dim1">第二维大小 / Second dimension size</param>
        /// <param name="dim2">第三维大小 / Third dimension size</param>
        /// <returns>三维形状 / Three-dimensional shape</returns>
        public static Shape three_dim(long dim0, long dim1, long dim2) => new Shape(new long[] { dim0, dim1, dim2 });

        /// <summary>
        /// 创建四维形状 / Create four-dimensional shape
        /// </summary>
        /// <param name="dim0">第一维大小 / First dimension size</param>
        /// <param name="dim1">第二维大小 / Second dimension size</param>
        /// <param name="dim2">第三维大小 / Third dimension size</param>
        /// <param name="dim3">第四维大小 / Fourth dimension size</param>
        /// <returns>四维形状 / Four-dimensional shape</returns>
        public static Shape four_dim(long dim0, long dim1, long dim2, long dim3) => new Shape(new long[] { dim0, dim1, dim2, dim3 });

        /// <summary>
        /// 创建NCHW形状（批次、通道、高度、宽度）/ Create NCHW shape (batch, channels, height, width)
        /// </summary>
        /// <param name="batch">批次大小 / Batch size</param>
        /// <param name="channels">通道数 / Number of channels</param>
        /// <param name="height">高度 / Height</param>
        /// <param name="width">宽度 / Width</param>
        /// <returns>NCHW形状 / NCHW shape</returns>
        public static Shape nchw(long batch, long channels, long height, long width)
            => new Shape(new long[] { batch, channels, height, width });

        /// <summary>
        /// 创建NHWC形状（批次、高度、宽度、通道）/ Create NHWC shape (batch, height, width, channels)
        /// </summary>
        /// <param name="batch">批次大小 / Batch size</param>
        /// <param name="height">高度 / Height</param>
        /// <param name="width">宽度 / Width</param>
        /// <param name="channels">通道数 / Number of channels</param>
        /// <returns>NHWC形状 / NHWC shape</returns>
        public static Shape nhwc(long batch, long height, long width, long channels)
            => new Shape(new long[] { batch, height, width, channels });

        /// <summary>
        /// 创建CHW形状（通道、高度、宽度）/ Create CHW shape (channels, height, width)
        /// </summary>
        /// <param name="channels">通道数 / Number of channels</param>
        /// <param name="height">高度 / Height</param>
        /// <param name="width">宽度 / Width</param>
        /// <returns>CHW形状 / CHW shape</returns>
        public static Shape chw(long channels, long height, long width)
            => new Shape(new long[] { channels, height, width });

        /// <summary>
        /// 创建HWC形状（高度、宽度、通道）/ Create HWC shape (height, width, channels)
        /// </summary>
        /// <param name="height">高度 / Height</param>
        /// <param name="width">宽度 / Width</param>
        /// <param name="channels">通道数 / Number of channels</param>
        /// <returns>HWC形状 / HWC shape</returns>
        public static Shape hwc(long height, long width, long channels)
            => new Shape(new long[] { height, width, channels });

        /// <summary>
        /// 创建HW形状（高度、宽度）/ Create HW shape (height, width)
        /// </summary>
        /// <param name="height">高度 / Height</param>
        /// <param name="width">宽度 / Width</param>
        /// <returns>HW形状 / HW shape</returns>
        public static Shape hw(long height, long width)
            => new Shape(new long[] { height, width });

        #endregion
    }
}
