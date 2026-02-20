// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

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
        }

        /// <summary>
        /// 获取元素数量 / Get number of elements
        /// </summary>
        public ulong size
        {
            get
            {
                ThrowIfDisposed();
                ulong sizeValue = 0;
                ExceptionHandler.ThrowOnError(ov_tensor_get_size(_ptr, ref sizeValue));
                return sizeValue;
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
                ulong sizeValue = 0;
                ExceptionHandler.ThrowOnError(ov_tensor_get_byte_size(_ptr, ref sizeValue));
                return sizeValue;
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
            return new Span<T>(ptr, (int)(byteSize / (ulong)sizeof(T)));
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
            return new ReadOnlySpan<T>(ptr, (int)(byteSize / (ulong)sizeof(T)));
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
            int length = (int)(byteSize / (ulong)sizeof(T));
            return new PointerMemoryManager<T>(ptr, length).Memory;
        }
#endif

        #endregion

        #region 类型特定数据获取 / Type-Specific Data Getters

        /// <summary>
        /// 获取浮点数组数据 / Get float array data
        /// <para>性能提示：对于大数据，考虑使用 get_span&lt;float&gt;() 避免拷贝。</para>
        /// </summary>
        /// <returns>浮点数组 / Float array</returns>
        public float[] get_float_data()
        {
            ThrowIfDisposed();
            ulong elemCount = size;
            float[] result = new float[elemCount];
            IntPtr dataPtr = data();
            Marshal.Copy(dataPtr, result, 0, (int)elemCount);
            return result;
        }

        /// <summary>
        /// 获取字节数组数据 / Get byte array data
        /// </summary>
        /// <returns>字节数组 / Byte array</returns>
        public byte[] get_byte_data()
        {
            ThrowIfDisposed();
            ulong byteCount = byte_size;
            byte[] result = new byte[byteCount];
            IntPtr dataPtr = data();
            Marshal.Copy(dataPtr, result, 0, (int)byteCount);
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
            int[] result = new int[byteCount / (ulong)sizeof(int)];
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
            long[] result = new long[byteCount / (ulong)sizeof(long)];
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
            uint[] result = new uint[byteCount / (ulong)sizeof(uint)];
            IntPtr dataPtr = data();
            int[] temp = new int[result.Length];
            Marshal.Copy(dataPtr, temp, 0, result.Length);
            Buffer.BlockCopy(temp, 0, result, 0, (int)byteCount);
            return result;
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
            T[] result = new T[length];
            void* src = data().ToPointer();
            ulong srcSize = (ulong)(length * sizeof(T));
            fixed (void* dst = result)
            {
                Buffer.MemoryCopy(src, dst, (long)srcSize, (long)srcSize);
            }
            return result;
        }

        /// <summary>
        /// 获取数据到已存在的缓冲区（零分配）/ Get data into existing buffer (zero allocation)
        /// </summary>
        /// <typeparam name="T">元素类型 / Element type</typeparam>
        /// <param name="buffer">目标缓冲区 / Destination buffer</param>
#if HAS_SPAN
        public unsafe void get_data_to<T>(Span<T> buffer) where T : unmanaged
        {
            ThrowIfDisposed();
            if (buffer.IsEmpty)
                throw new ArgumentException("缓冲区不能为空", nameof(buffer));

            void* src = data().ToPointer();
            ulong srcSize = (ulong)(buffer.Length * sizeof(T));
            fixed (void* dst = buffer)
            {
                Buffer.MemoryCopy(src, dst, (long)srcSize, (long)srcSize);
            }
        }
#else
        public unsafe void get_data_to<T>(T[] buffer, int length) where T : unmanaged
        {
            ThrowIfDisposed();
            if (buffer == null || buffer.Length == 0)
                throw new ArgumentException("缓冲区不能为空", nameof(buffer));

            void* src = data().ToPointer();
            ulong srcSize = (ulong)(length * sizeof(T));
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
            ulong srcSize = (ulong)(input_data.Length * sizeof(T));

            if (srcSize > destSize)
                throw new ArgumentException("输入数据太大。/ Input data is too large.");

            fixed (void* srcPtr = input_data)
            {
                Buffer.MemoryCopy(srcPtr, destPtr, (long)destSize, (long)srcSize);
            }
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
            ulong srcSize = (ulong)(input_data.Length * sizeof(T));

            if (srcSize > destSize)
                throw new ArgumentException("输入数据太大。/ Input data is too large.");

            fixed (void* srcPtr = input_data)
            {
                Buffer.MemoryCopy(srcPtr, destPtr, (long)destSize, (long)srcSize);
            }
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
