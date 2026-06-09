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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static OpenVinoSharp.native.NativeMethods;
using OpenVinoSharp.Internal;
using OpenVinoSharp.native;


#if HAS_SPAN
using System.Buffers;
#endif

namespace OpenVinoSharp
{
    /// <summary>
    /// 张量类 / Tensor class
    /// <para>OpenVINO张量，用于存储和管理多维数组数据。/ OpenVINO tensor for storing and managing multi-dimensional array data.</para>
    /// <para>性能优化：支持 Span&lt;T&gt;、Memory&lt;T&gt; 和 ArrayPool 减少内存分配。/ Performance optimized: supports Span&lt;T&gt;, Memory&lt;T&gt; and ArrayPool to reduce allocations.</para>
    /// </summary>
    public partial class Tensor : DisposableOvObject
    {
        #region 构造函数 / Constructors

        /// <summary>
        /// 从原生指针构造 / Construct from native pointer
        /// </summary>
        /// <param name="ptr">原生张量指针 / Native tensor pointer</param>
        public Tensor(IntPtr ptr) : base(ptr) { }

        /// <summary>
        /// 从形状和数据类型构造（空分配）/ Construct from shape and element type (empty allocation)
        /// </summary>
        /// <param name="shape">张量形状 / Tensor shape</param>
        /// <param name="type">元素类型 / Element type</param>
        public Tensor(Shape shape, ElementType type) : base()
        {
            if (shape == null)
                throw new ArgumentNullException(nameof(shape));
            IntPtr ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_tensor_create((uint)type, Marshal.PtrToStructure<ov_shape_t>(shape.OvPtr), ref ptr));
            _ptr = ptr;
        }

        /// <summary>
        /// 从形状和浮点数组构造 / Construct from shape and float array
        /// </summary>
        /// <param name="shape">张量形状 / Tensor shape</param>
        /// <param name="input_data">输入数据 / Input data</param>
        public Tensor(Shape shape, float[] input_data) : base()
        {
            if (shape == null)
                throw new ArgumentNullException(nameof(shape));
            if (input_data == null)
                throw new ArgumentNullException(nameof(input_data));
            
            unsafe
            {
                fixed (void* dataPtr = input_data)
                {
                    ExceptionHandler.ThrowOnError(
                        ov_tensor_create_from_host_ptr((uint)ElementType.F32, Marshal.PtrToStructure<ov_shape_t>(shape.OvPtr), (IntPtr)dataPtr, ref _ptr));
                }
            }
        }

        /// <summary>
        /// 从形状和 UTF-8 字符串数组构造字符串张量 / Construct a string tensor from shape and UTF-8 string array
        /// </summary>
        /// <param name="shape">张量形状 / Tensor shape</param>
        /// <param name="input_data">字符串数据 / String data</param>
        public Tensor(Shape shape, string[] input_data) : base()
        {
            if (shape == null)
                throw new ArgumentNullException(nameof(shape));
            if (input_data == null)
                throw new ArgumentNullException(nameof(input_data));

            ov_shape_t nativeShape = Marshal.PtrToStructure<ov_shape_t>(shape.OvPtr);
            WithUtf8StringArray(input_data, (arrayPtr, arraySize) =>
            {
                ExceptionHandler.ThrowOnError(
                    ov_tensor_create_from_string_array_native_size(arrayPtr, arraySize, nativeShape, ref _ptr));
            });
        }

#if HAS_SPAN
        /// <summary>
        /// 从形状和浮点Span构造（.NET Core 2.1+）/ Construct from shape and float Span (.NET Core 2.1+)
        /// </summary>
        /// <param name="shape">张量形状 / Tensor shape</param>
        /// <param name="input_data">输入数据 / Input data</param>
        public Tensor(Shape shape, ReadOnlySpan<float> input_data) : base()
        {
            if (shape == null)
                throw new ArgumentNullException(nameof(shape));
            
            unsafe
            {
                fixed (float* dataPtr = input_data)
                {
                    ExceptionHandler.ThrowOnError(
                        ov_tensor_create_from_host_ptr((uint)ElementType.F32, Marshal.PtrToStructure<ov_shape_t>(shape.OvPtr), (IntPtr)dataPtr, ref _ptr));
                }
            }
        }
#endif

        /// <summary>
        /// 从外部内存指针构造 / Construct from external memory pointer
        /// </summary>
        /// <param name="element_type">元素类型 / Element type</param>
        /// <param name="shape">张量形状 / Tensor shape</param>
        /// <param name="data">数据指针 / Data pointer</param>
        /// <param name="byte_size">字节大小 / Byte size</param>
        public Tensor(ElementType element_type, Shape shape, IntPtr data, ulong byte_size) : base()
        {
            if (shape == null)
                throw new ArgumentNullException(nameof(shape));
            ExceptionHandler.ThrowOnError(
                ov_tensor_create_from_host_ptr((uint)element_type, Marshal.PtrToStructure<ov_shape_t>(shape.OvPtr), data, ref _ptr));
        }

        /// <summary>
        /// 从形状和数据类型构造通用张量 / Construct generic tensor from shape and element type
        /// </summary>
        /// <param name="shape">张量形状 / Tensor shape</param>
        /// <param name="element_type">元素类型 / Element type</param>
        /// <returns>张量对象 / Tensor object</returns>
        public static Tensor from_shape(Shape shape, ElementType element_type)
        {
            if (shape == null)
                throw new ArgumentNullException(nameof(shape));
            IntPtr ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_tensor_create((uint)element_type, Marshal.PtrToStructure<ov_shape_t>(shape.OvPtr), ref ptr));
            return new Tensor(ptr);
        }

        /// <summary>
        /// 从形状和元素类型创建张量 / Creates a tensor from shape and element type.
        /// </summary>
        /// <param name="shape">张量形状 / Tensor shape.</param>
        /// <param name="elementType">元素类型 / Element type.</param>
        /// <returns>张量对象 / Tensor object.</returns>
        public static Tensor FromShape(Shape shape, ElementType elementType)
        {
            return from_shape(shape, elementType);
        }

        /// <summary>
        /// 从字符串数组创建字符串张量 / Create a string tensor from a string array
        /// </summary>
        /// <param name="shape">张量形状 / Tensor shape</param>
        /// <param name="inputData">字符串数据 / String data</param>
        /// <returns>字符串张量 / String tensor</returns>
        public static Tensor from_strings(Shape shape, string[] inputData)
        {
            return new Tensor(shape, inputData);
        }

        /// <summary>
        /// 从字符串数组创建字符串张量 / Creates a string tensor from a string array.
        /// </summary>
        /// <param name="shape">张量形状 / Tensor shape.</param>
        /// <param name="inputData">字符串数据 / String data.</param>
        /// <returns>字符串张量 / String tensor.</returns>
        public static Tensor FromStrings(Shape shape, string[] inputData)
        {
            return from_strings(shape, inputData);
        }

        /// <summary>
        /// 从固定指针构造张量 / Construct tensor from fixed pointer
        /// </summary>
        /// <param name="shape">张量形状 / Tensor shape</param>
        /// <param name="element_type">元素类型 / Element type</param>
        /// <param name="data">数据指针 / Data pointer</param>
        public unsafe Tensor(Shape shape, ElementType element_type, void* data) : base()
        {
            if (shape == null)
                throw new ArgumentNullException(nameof(shape));
            ExceptionHandler.ThrowOnError(ov_tensor_create_from_host_ptr((uint)element_type, Marshal.PtrToStructure<ov_shape_t>(shape.OvPtr), (IntPtr)data, ref _ptr));
        }

        #endregion

        #region 资源释放 / Resource Disposal

        /// <inheritdoc/>
        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero && IsEnabledDispose)
            {
                ov_tensor_free(_ptr);
            }
            base.DisposeUnmanaged();
        }

        #endregion

        #region 形状和大小属性 / Shape and Size Properties

        /// <summary>
        /// 获取张量形状 / Get tensor shape
        /// </summary>
        public Shape shape
        {
            get
            {
                ThrowIfDisposed();
                IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ov_shape_t)));
                ExceptionHandler.ThrowOnError(ov_tensor_get_shape(_ptr, ptr));
                return new Shape(ptr);
            }
            set 
            {
                ExceptionHandler.ThrowOnError(ov_tensor_set_shape(_ptr, value.get_native_shape()));
            }
        }

        /// <summary>
        /// 获取元素数量 / Get number of elements
        /// </summary>
        public ulong size
        {
            get
            {
                ThrowIfDisposed();
                UIntPtr sizeValue = UIntPtr.Zero;
                ExceptionHandler.ThrowOnError(ov_tensor_get_size_native_size(_ptr, ref sizeValue));
                return StringUtils.FromNativeSize(sizeValue);
            }
        }

        /// <summary>
        /// 获取字节大小 / Get byte size
        /// </summary>
        public ulong byte_size
        {
            get
            {
                ThrowIfDisposed();
                UIntPtr sizeValue = UIntPtr.Zero;
                ExceptionHandler.ThrowOnError(ov_tensor_get_byte_size_native_size(_ptr, ref sizeValue));
                return StringUtils.FromNativeSize(sizeValue);
            }
        }

        /// <summary>
        /// 获取元素类型 / Get element type
        /// </summary>
        public ElementType element_type
        {
            get
            {
                ThrowIfDisposed();
                uint type = 0;
                ExceptionHandler.ThrowOnError(ov_tensor_get_element_type(_ptr, out type));
                return (ElementType)type;
            }
        }

        /// <summary>
        /// 获取或设置张量形状 / Gets or sets the tensor shape.
        /// </summary>
        public Shape Shape
        {
            get { return shape; }
            set { shape = value; }
        }

        /// <summary>
        /// 获取张量元素数量 / Gets the number of tensor elements.
        /// </summary>
        public ulong ElementCount
        {
            get { return size; }
        }

        /// <summary>
        /// 获取张量字节数 / Gets the tensor byte size.
        /// </summary>
        public ulong ByteSize
        {
            get { return byte_size; }
        }

        /// <summary>
        /// 获取张量元素类型 / Gets the tensor element type.
        /// </summary>
        public ElementType ElementTypeValue
        {
            get { return element_type; }
        }

        #endregion

        #region 数据访问方法 / Data Access Methods

        /// <summary>
        /// 获取数据指针 / Get data pointer
        /// </summary>
        /// <returns>数据指针 / Data pointer</returns>
        public IntPtr data()
        {
            ThrowIfDisposed();
            IntPtr data = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_tensor_data(_ptr, ref data));
            return data;
        }

        /// <summary>
        /// 获取张量数据指针 / Gets the tensor data pointer.
        /// </summary>
        public IntPtr Data
        {
            get { return data(); }
        }

        /// <summary>
        /// 将数据拷贝到数组 / Copy data to array
        /// <para>高性能实现，使用Span进行内存拷贝。/ High-performance implementation using Span for memory copy.</para>
        /// </summary>
        /// <param name="dst">目标数组 / Destination array</param>
        /// <param name="dst_size">目标大小 / Destination size</param>
        public unsafe void copy_to(byte* dst, ulong dst_size)
        {
            ThrowIfDisposed();
            if (dst == null)
                throw new ArgumentNullException(nameof(dst));

            ulong byteSize = byte_size;
            if (dst_size < byteSize)
                throw new ArgumentException("目标缓冲区太小。/ Destination buffer is too small.");

            void* src = data().ToPointer();
            Buffer.MemoryCopy(src, dst, (long)dst_size, (long)byteSize);
        }

#if HAS_SPAN
        /// <summary>
        /// 获取可写的Span视图（高性能，零拷贝）/ Get writable Span view (high performance, zero-copy)
        /// <para>.NET Core 2.1+ / .NET 5+ 支持 / Supported on .NET Core 2.1+ / .NET 5+</para>
        /// </summary>
        /// <typeparam name="T">元素类型 / Element type</typeparam>
        /// <returns>Span视图 / Span view</returns>
        public unsafe Span<T> get_span<T>() where T : unmanaged
        {
            ThrowIfDisposed();
            ulong byteSize = byte_size;
            void* ptr = data().ToPointer();
            return new Span<T>(ptr, CheckedArrayLength(byteSize / (ulong)sizeof(T), nameof(byte_size)));
        }

        /// <summary>
        /// 获取可写 Span 视图 / Gets a writable Span view.
        /// </summary>
        /// <typeparam name="T">元素类型 / Element type.</typeparam>
        /// <returns>Span 视图 / Span view.</returns>
        public Span<T> AsSpan<T>() where T : unmanaged
        {
            return get_span<T>();
        }

        /// <summary>
        /// 获取只读的ReadOnlySpan视图（高性能，零拷贝）/ Get readonly ReadOnlySpan view (high performance, zero-copy)
        /// <para>.NET Core 2.1+ / .NET 5+ 支持 / Supported on .NET Core 2.1+ / .NET 5+</para>
        /// </summary>
        /// <typeparam name="T">元素类型 / Element type</typeparam>
        /// <returns>ReadOnlySpan视图 / ReadOnlySpan view</returns>
        public unsafe ReadOnlySpan<T> get_readonly_span<T>() where T : unmanaged
        {
            ThrowIfDisposed();
            ulong byteSize = byte_size;
            void* ptr = data().ToPointer();
            return new ReadOnlySpan<T>(ptr, CheckedArrayLength(byteSize / (ulong)sizeof(T), nameof(byte_size)));
        }

        /// <summary>
        /// 获取只读 Span 视图 / Gets a read-only Span view.
        /// </summary>
        /// <typeparam name="T">元素类型 / Element type.</typeparam>
        /// <returns>只读 Span 视图 / Read-only Span view.</returns>
        public ReadOnlySpan<T> AsReadOnlySpan<T>() where T : unmanaged
        {
            return get_readonly_span<T>();
        }
#endif

#if HAS_MEMORY
        /// <summary>
        /// 获取Memory视图（使用不安全指针包装）/ Get Memory view (wrapped using unsafe pointer)
        /// <para>.NET Core 2.1+ / .NET 5+ 支持 / Supported on .NET Core 2.1+ / .NET 5+</para>
        /// </summary>
        /// <typeparam name="T">元素类型 / Element type</typeparam>
        /// <returns>Memory视图 / Memory view</returns>
        public unsafe Memory<T> get_memory<T>() where T : unmanaged
        {
            ThrowIfDisposed();
            ulong byteSize = byte_size;
            void* ptr = data().ToPointer();
            int length = CheckedArrayLength(byteSize / (ulong)sizeof(T), nameof(byte_size));
            return new PointerMemoryManager<T>(ptr, length).Memory;
        }
#endif

        #endregion

        private static int CheckedArrayLength(ulong length, string paramName)
        {
            if (length > int.MaxValue)
                throw new OverflowException($"{paramName} is too large to copy into a managed array.");

            return (int)length;
        }

        private static void WithUtf8StringArray(string[] values, Action<IntPtr, UIntPtr> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (values.Length == 0)
            {
                action(IntPtr.Zero, UIntPtr.Zero);
                return;
            }

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == null)
                    throw new ArgumentException("String tensor values cannot be null. / 字符串张量元素不能为 null。", nameof(values));
            }

            IntPtr[] stringPtrs = StringUtils.StringArrayToUtf8PtrArray(values);
            GCHandle arrayHandle = default;
            try
            {
                arrayHandle = GCHandle.Alloc(stringPtrs, GCHandleType.Pinned);
                action(arrayHandle.AddrOfPinnedObject(), StringUtils.ToNativeSize((ulong)values.Length));
            }
            finally
            {
                if (arrayHandle.IsAllocated)
                    arrayHandle.Free();
                StringUtils.FreeUtf8PtrArray(stringPtrs);
            }
        }

        #region 类型特定数据获取 / Type-Specific Data Getters

        /// <summary>
        /// 获取浮点数组数据 / Get float array data
        /// <para>性能提示：对于大数据，考虑使用 get_span&lt;float&gt;() 避免拷贝。</para>
        /// </summary>
        /// <returns>浮点数组 / Float array</returns>
        public float[] get_float_data()
        {
            ThrowIfDisposed();
            int elemCount = CheckedArrayLength(size, nameof(size));
            float[] result = new float[elemCount];
            IntPtr dataPtr = data();
            Marshal.Copy(dataPtr, result, 0, elemCount);
            return result;
        }

        /// <summary>
        /// 获取字节数组数据 / Get byte array data
        /// </summary>
        /// <returns>字节数组 / Byte array</returns>
        public byte[] get_byte_data()
        {
            ThrowIfDisposed();
            int byteCount = CheckedArrayLength(byte_size, nameof(byte_size));
            byte[] result = new byte[byteCount];
            IntPtr dataPtr = data();
            Marshal.Copy(dataPtr, result, 0, byteCount);
            return result;
        }

        /// <summary>
        /// 获取整数数组数据 / Get int array data
        /// </summary>
        /// <returns>整数数组 / Int array</returns>
        public int[] get_int_data()
        {
            ThrowIfDisposed();
            ulong byteCount = byte_size;
            int length = CheckedArrayLength(byteCount / (ulong)sizeof(int), nameof(byte_size));
            int[] result = new int[length];
            IntPtr dataPtr = data();
            Marshal.Copy(dataPtr, result, 0, result.Length);
            return result;
        }

        /// <summary>
        /// 获取长整数数组数据 / Get long array data
        /// </summary>
        /// <returns>长整数数组 / Long array</returns>
        public long[] get_long_data()
        {
            ThrowIfDisposed();
            ulong byteCount = byte_size;
            int length = CheckedArrayLength(byteCount / (ulong)sizeof(long), nameof(byte_size));
            long[] result = new long[length];
            IntPtr dataPtr = data();
            Marshal.Copy(dataPtr, result, 0, result.Length);
            return result;
        }

        /// <summary>
        /// 获取无符号整数数组数据 / Get uint array data
        /// </summary>
        /// <returns>无符号整数数组 / Uint array</returns>
        public uint[] get_uint_data()
        {
            ThrowIfDisposed();
            ulong byteCount = byte_size;
            int length = CheckedArrayLength(byteCount / (ulong)sizeof(uint), nameof(byte_size));
            uint[] result = new uint[length];
            IntPtr dataPtr = data();
            int[] temp = new int[result.Length];
            Marshal.Copy(dataPtr, temp, 0, result.Length);
            Buffer.BlockCopy(temp, 0, result, 0, CheckedArrayLength(byteCount, nameof(byte_size)));
            return result;
        }

        /// <summary>
        /// 获取 float 数组数据 / Gets tensor data as a float array.
        /// </summary>
        /// <returns>float 数组 / Float array.</returns>
        public float[] GetFloatData()
        {
            return get_float_data();
        }

        /// <summary>
        /// 获取 byte 数组数据 / Gets tensor data as a byte array.
        /// </summary>
        /// <returns>byte 数组 / Byte array.</returns>
        public byte[] GetByteData()
        {
            return get_byte_data();
        }

        /// <summary>
        /// 获取 int 数组数据 / Gets tensor data as an int array.
        /// </summary>
        /// <returns>int 数组 / Int array.</returns>
        public int[] GetIntData()
        {
            return get_int_data();
        }

        /// <summary>
        /// 获取 long 数组数据 / Gets tensor data as a long array.
        /// </summary>
        /// <returns>long 数组 / Long array.</returns>
        public long[] GetLongData()
        {
            return get_long_data();
        }

        /// <summary>
        /// 获取 uint 数组数据 / Gets tensor data as a uint array.
        /// </summary>
        /// <returns>uint 数组 / UInt array.</returns>
        public uint[] GetUIntData()
        {
            return get_uint_data();
        }

        /// <summary>
        /// 获取通用类型数据（使用 ArrayPool 减少分配）/ Get generic type data (uses ArrayPool to reduce allocations)
        /// <para>性能优化：使用 ArrayPool 复用数组缓冲区。</para>
        /// </summary>
        /// <typeparam name="T">元素类型 / Element type</typeparam>
        /// <param name="length">元素数量 / Number of elements</param>
        /// <returns>元素数组 / Element array</returns>
        public unsafe T[] get_data<T>(int length) where T : unmanaged
        {
            ThrowIfDisposed();
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            T[] result = new T[length];
            void* src = data().ToPointer();
            ulong srcSize = checked((ulong)length * (ulong)sizeof(T));
            if (srcSize > byte_size)
                throw new ArgumentException("Requested data length is larger than the tensor byte size.", nameof(length));

            fixed (void* dst = result)
            {
                Buffer.MemoryCopy(src, dst, (long)srcSize, (long)srcSize);
            }
            return result;
        }

        /// <summary>
        /// 获取指定数量的泛型数据 / Gets typed tensor data with the specified element count.
        /// </summary>
        /// <typeparam name="T">元素类型 / Element type.</typeparam>
        /// <param name="length">元素数量 / Element count.</param>
        /// <returns>元素数组 / Element array.</returns>
        public T[] GetData<T>(int length) where T : unmanaged
        {
            return get_data<T>(length);
        }

#if HAS_SPAN
        /// <summary>
        /// 将张量数据复制到已有缓冲区，避免额外分配。
        /// Copies tensor data into an existing buffer without extra allocation.
        /// </summary>
        /// <typeparam name="T">元素类型 / Element type.</typeparam>
        /// <param name="buffer">目标缓冲区 / Destination buffer.</param>
        public unsafe void get_data_to<T>(Span<T> buffer) where T : unmanaged
        {
            ThrowIfDisposed();
            if (buffer.IsEmpty)
                throw new ArgumentException("缓冲区不能为空", nameof(buffer));

            void* src = data().ToPointer();
            ulong srcSize = checked((ulong)buffer.Length * (ulong)sizeof(T));
            if (srcSize > byte_size)
                throw new ArgumentException("Buffer length is larger than the tensor byte size.", nameof(buffer));

            fixed (void* dst = buffer)
            {
                Buffer.MemoryCopy(src, dst, (long)srcSize, (long)srcSize);
            }
        }
#else
        /// <summary>
        /// 将张量数据复制到已有数组缓冲区，避免额外分配。
        /// Copies tensor data into an existing array buffer without extra allocation.
        /// </summary>
        /// <typeparam name="T">元素类型 / Element type.</typeparam>
        /// <param name="buffer">目标缓冲区 / Destination buffer.</param>
        /// <param name="length">要复制的元素数量 / Number of elements to copy.</param>
        public unsafe void get_data_to<T>(T[] buffer, int length) where T : unmanaged
        {
            ThrowIfDisposed();
            if (buffer == null || buffer.Length == 0)
                throw new ArgumentException("缓冲区不能为空", nameof(buffer));

            void* src = data().ToPointer();
            if (length < 0 || length > buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(length));

            ulong srcSize = checked((ulong)length * (ulong)sizeof(T));
            if (srcSize > byte_size)
                throw new ArgumentException("Buffer length is larger than the tensor byte size.", nameof(length));

            fixed (void* dst = buffer)
            {
                Buffer.MemoryCopy(src, dst, (long)srcSize, (long)srcSize);
            }
        }
#endif

        #endregion

        #region 数据设置方法 / Data Set Methods

        /// <summary>
        /// 设置数据（泛型数组）/ Set data (generic array)
        /// </summary>
        /// <typeparam name="T">元素类型 / Element type</typeparam>
        /// <param name="input_data">输入数据 / Input data</param>
        public unsafe void set_data<T>(T[] input_data) where T : unmanaged
        {
            ThrowIfDisposed();
            if (input_data == null)
                throw new ArgumentNullException(nameof(input_data));

            void* destPtr = data().ToPointer();
            ulong destSize = byte_size;
            ulong srcSize = checked((ulong)input_data.Length * (ulong)sizeof(T));

            if (srcSize > destSize)
                throw new ArgumentException("输入数据太大。/ Input data is too large.");

            fixed (void* srcPtr = input_data)
            {
                Buffer.MemoryCopy(srcPtr, destPtr, (long)destSize, (long)srcSize);
            }
        }

        /// <summary>
        /// 设置泛型数组数据 / Sets tensor data from a typed array.
        /// </summary>
        /// <typeparam name="T">元素类型 / Element type.</typeparam>
        /// <param name="inputData">输入数据 / Input data.</param>
        public void SetData<T>(T[] inputData) where T : unmanaged
        {
            set_data(inputData);
        }

#if HAS_SPAN
        /// <summary>
        /// 设置数据（Span，高性能）/ Set data (Span, high performance)
        /// <para>.NET Core 2.1+ / .NET 5+ 支持 / Supported on .NET Core 2.1+ / .NET 5+</para>
        /// </summary>
        /// <typeparam name="T">元素类型 / Element type</typeparam>
        /// <param name="input_data">输入数据 / Input data</param>
        public unsafe void set_data<T>(ReadOnlySpan<T> input_data) where T : unmanaged
        {
            ThrowIfDisposed();

            void* destPtr = data().ToPointer();
            ulong destSize = byte_size;
            ulong srcSize = checked((ulong)input_data.Length * (ulong)sizeof(T));

            if (srcSize > destSize)
                throw new ArgumentException("输入数据太大。/ Input data is too large.");

            fixed (void* srcPtr = input_data)
            {
                Buffer.MemoryCopy(srcPtr, destPtr, (long)destSize, (long)srcSize);
            }
        }

        /// <summary>
        /// 设置 Span 数据 / Sets tensor data from a read-only span.
        /// </summary>
        /// <typeparam name="T">元素类型 / Element type.</typeparam>
        /// <param name="inputData">输入数据 / Input data.</param>
        public void SetData<T>(ReadOnlySpan<T> inputData) where T : unmanaged
        {
            set_data(inputData);
        }
#endif

        /// <summary>
        /// 设置浮点数据 / Set float data
        /// </summary>
        /// <param name="input_data">浮点数组 / Float array</param>
        public void set_float_data(float[] input_data)
        {
            ThrowIfDisposed();
            if (input_data == null)
                throw new ArgumentNullException(nameof(input_data));
            set_data(input_data);
        }

        /// <summary>
        /// 设置 float 数组数据 / Sets tensor data from a float array.
        /// </summary>
        /// <param name="inputData">输入数据 / Input data.</param>
        public void SetFloatData(float[] inputData)
        {
            set_float_data(inputData);
        }

        /// <summary>
        /// 设置字符串数据 / Set string data
        /// </summary>
        /// <param name="input_data">字符串数组 / String array</param>
        public void set_string_data(string[] input_data)
        {
            ThrowIfDisposed();
            if (input_data == null)
                throw new ArgumentNullException(nameof(input_data));

            WithUtf8StringArray(input_data, (arrayPtr, arraySize) =>
            {
                ExceptionHandler.ThrowOnError(
                    ov_tensor_set_string_data_native_size(_ptr, arrayPtr, arraySize));
            });
        }

        /// <summary>
        /// 设置字符串数据 / Sets string data.
        /// </summary>
        /// <param name="inputData">字符串数组 / String array.</param>
        public void SetStringData(string[] inputData)
        {
            set_string_data(inputData);
        }

        /// <summary>
        /// 批量设置数据（高性能内存拷贝）/ Batch set data (high-performance memory copy)
        /// </summary>
        /// <param name="source">源数据指针 / Source data pointer</param>
        /// <param name="byteLength">字节长度 / Byte length</param>
        public unsafe void set_raw_data(void* source, ulong byteLength)
        {
            ThrowIfDisposed();
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            void* destPtr = data().ToPointer();
            ulong destSize = byte_size;

            if (byteLength > destSize)
                throw new ArgumentException("输入数据太大。/ Input data is too large.");

            Buffer.MemoryCopy(source, destPtr, (long)destSize, (long)byteLength);
        }

        #endregion

        /// <summary>
        /// 获取原生指针（兼容属性）/ Get native pointer (compatibility property)
        /// </summary>
        public IntPtr Ptr => OvPtr;

        /// <summary>
        /// 获取远程张量参数字符串 / Get remote tensor parameter string
        /// </summary>
        /// <returns>远程张量参数字符串 / Remote tensor parameter string.</returns>
        /// <remarks>仅适用于远程张量 / Only valid for remote tensors.</remarks>
        public string get_remote_params()
        {
            ThrowIfDisposed();
            UIntPtr size = UIntPtr.Zero;
            IntPtr paramsPtr = IntPtr.Zero;
            try
            {
                ExceptionHandler.ThrowOnError(
                    ov_remote_tensor_get_params_native_size(_ptr, ref size, ref paramsPtr));
                return StringUtils.Utf8PtrToString(paramsPtr) ?? string.Empty;
            }
            finally
            {
                if (paramsPtr != IntPtr.Zero)
                    ov_free(paramsPtr);
            }
        }

        /// <summary>
        /// 获取远程张量参数字符串 / Gets remote tensor parameters.
        /// </summary>
        /// <returns>远程张量参数字符串 / Remote tensor parameter string.</returns>
        public string GetRemoteParams()
        {
            return get_remote_params();
        }

        /// <summary>
        /// 获取远程张量所在设备名称 / Get the device name for a remote tensor
        /// </summary>
        /// <returns>设备名称 / Device name.</returns>
        /// <remarks>仅适用于远程张量 / Only valid for remote tensors.</remarks>
        public string get_remote_device_name()
        {
            ThrowIfDisposed();
            IntPtr deviceNamePtr = IntPtr.Zero;
            try
            {
                ExceptionHandler.ThrowOnError(
                    ov_remote_tensor_get_device_name(_ptr, ref deviceNamePtr));
                return StringUtils.Utf8PtrToString(deviceNamePtr) ?? string.Empty;
            }
            finally
            {
                if (deviceNamePtr != IntPtr.Zero)
                    ov_free(deviceNamePtr);
            }
        }

        /// <summary>
        /// 获取远程张量所在设备名称 / Gets the device name for a remote tensor.
        /// </summary>
        /// <returns>设备名称 / Device name.</returns>
        public string GetRemoteDeviceName()
        {
            return get_remote_device_name();
        }

#if HAS_MEMORY
        /// <summary>
        /// 用于将不安全指针包装为 Memory&lt;T&gt; 的 MemoryManager
        /// </summary>
        private unsafe class PointerMemoryManager<T> : MemoryManager<T> where T : unmanaged
        {
            private readonly void* _pointer;
            private readonly int _length;

            public PointerMemoryManager(void* pointer, int length)
            {
                _pointer = pointer;
                _length = length;
            }

            public override Span<T> GetSpan()
            {
                return new Span<T>(_pointer, _length);
            }

            public override MemoryHandle Pin(int elementIndex = 0)
            {
                return new MemoryHandle((T*)_pointer + elementIndex);
            }

            public override void Unpin()
            {
                // 不需要解固定，因为原生内存本来就是固定的
            }

            protected override void Dispose(bool disposing)
            {
                // 不管理原生内存的生命周期
            }
        }
#endif
    }
}
