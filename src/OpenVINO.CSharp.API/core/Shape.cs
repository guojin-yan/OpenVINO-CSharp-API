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
using OpenVinoSharp.native;

namespace OpenVinoSharp
{
    /// <summary>
    /// 形状结构 / Shape structure
    /// <para>表示张量的多维形状。/ Represents the multi-dimensional shape of a tensor.</para>
    /// </summary>
    public class Shape : DisposableOvObject
    {
        #region 结构体定义 / Structure Definitions


        #endregion

        #region 字段 / Fields

        ///// <summary>
        ///// 内部形状指针 / Internal shape pointer
        ///// </summary>
        //internal new IntPtr _ptr;

        /// <summary>
        /// 获取底层 OpenVINO native shape 指针，主要用于低层互操作场景。
        /// Gets the underlying OpenVINO native shape pointer, primarily for low-level interop scenarios.
        /// </summary>
        public IntPtr Ptr => OvPtr;
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

            // 使用原生 API 创建形状 / Use native API to create shape
            OpenVinoSharp.native.ov_shape_t shapeStruct = new OpenVinoSharp.native.ov_shape_t();
            ExceptionHandler.ThrowOnError(ov_shape_create(dims.Length, dims, ref shapeStruct));
            
            // 保存指针和维度数据 / Save pointer and dimension data
            _ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ov_shape_t)));
            Marshal.StructureToPtr(shapeStruct, _ptr, false);
            _dims_ptr = shapeStruct.dims;
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
            // 使用原生 API 释放形状 / Use native API to free shape
            if (_ptr != IntPtr.Zero && IsEnabledDispose)
            {
                OpenVinoSharp.native.ov_shape_t shape = Marshal.PtrToStructure<OpenVinoSharp.native.ov_shape_t>(_ptr);
                ov_shape_free(ref shape);
                Marshal.FreeHGlobal(_ptr);
                _ptr = IntPtr.Zero;
                _dims_ptr = IntPtr.Zero;
            }

            base.DisposeUnmanaged();
        }

        #endregion

        #region 索引器 / Indexer

        /// <summary>
        /// 通过索引获取维度值 / Get dimension value by index
        /// <para>允许像数组一样访问形状维度，如 shape[0]、shape[1] 等。/ Allows array-like access to shape dimensions, e.g., shape[0], shape[1], etc.</para>
        /// </summary>
        /// <param name="index">维度索引 / Dimension index</param>
        /// <returns>维度值 / Dimension value</returns>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// var shape = new Shape(new long[] { 1, 3, 224, 224 });
        /// 
        /// // 使用索引器访问各维度 / Access dimensions using indexer
        /// long batch = shape[0];    // 1
        /// long channels = shape[1]; // 3
        /// long height = shape[2];   // 224
        /// long width = shape[3];    // 224
        /// 
        /// // 遍历所有维度 / Iterate all dimensions
        /// for (int i = 0; i &lt; shape.get_rank(); i++)
        /// {
        ///     Console.WriteLine($"Dim[{i}] = {shape[i]}");
        /// }
        /// </code>
        /// </example>
        /// <exception cref="ObjectDisposedException">当形状已被释放时抛出 / Thrown when shape has been disposed</exception>
        /// <exception cref="IndexOutOfRangeException">当索引越界时抛出 / Thrown when index is out of range</exception>
        public long this[int index]
        {
            get
            {
                ThrowIfDisposed();
                
                long rank = (long)get_rank();
                if (index < 0 || index >= rank)
                    throw new IndexOutOfRangeException($"Index {index} is out of range for shape with rank {rank}");
                
                return get_dim(index);
            }
        }

        /// <summary>
        /// 通过索引获取维度值（ulong 重载）/ Get dimension value by index (ulong overload)
        /// </summary>
        /// <param name="index">维度索引 / Dimension index</param>
        /// <returns>维度值 / Dimension value</returns>
        public long this[ulong index]
        {
            get => this[(int)index];
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
        public long get_rank()
        {
            ThrowIfDisposed();
            return _dims_array != null
                ? (long)_dims_array.Length
                : Marshal.PtrToStructure<Ov.ov_shape>(_ptr).rank;
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

        #region 转换方法 / Conversion Methods

        /// <summary>
        /// 转换为 ov_partial_shape_t 结构体 / Convert to ov_partial_shape_t structure
        /// </summary>
        /// <returns>ov_partial_shape_t 结构体 / ov_partial_shape_t structure</returns>
        internal ov_partial_shape_t to_partial_shape_struct()
        {
            // 使用原生 API 将 shape 转换为 partial_shape / Use native API to convert shape to partial_shape
            OpenVinoSharp.native.ov_shape_t shape = Marshal.PtrToStructure<OpenVinoSharp.native.ov_shape_t>(_ptr);
            ov_partial_shape_t partialShape = new ov_partial_shape_t();
            ExceptionHandler.ThrowOnError(ov_shape_to_partial_shape(shape, ref partialShape));
            return partialShape;
        }

        /// <summary>
        /// 获取原生形状结构体的引用（不安全）/ Get reference to native shape structure (Unsafe).
        /// <para>
        /// 直接读取当前指针指向的结构体。注意：返回的结构体中的 dims 指针指向 OpenVINO 托管对象的内部内存。
        /// 直接读取当前指针指向的结构体。注意：返回的结构体中的 dims 指针指向 OpenVINO 托管对象的内部内存。
        /// </para>
        /// </summary>
        /// <returns>ov_shape_t 结构体引用 / The ov_shape_t structure reference.</returns>
        public ov_shape_t get_native_shape()
        {
            ThrowIfDisposed();
            return Marshal.PtrToStructure<ov_shape_t>(_ptr);
        }

        #endregion

        #region 工厂方法 / Factory Methods

        /// <summary>
        /// 创建标量形状 / Create scalar shape
        /// </summary>
        /// <returns>标量形状 / Scalar shape</returns>
        public static Shape scalar() => new Shape(new long[1] { 0});

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
