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
using System.Threading;

namespace OpenVinoSharp.Internal
{
    /// <summary>
    /// 可释放对象基类，用于管理原生资源 / Base class for disposable objects with native resources
    /// <para>实现 IDisposable 模式，确保原生资源被正确释放。/ Implements IDisposable pattern to ensure native resources are properly released.</para>
    /// </summary>
    /// <example>
    /// 使用示例 / Usage example:
    /// <code>
    /// public class MyClass : DisposableObject
    /// {
    ///     private IntPtr _nativePtr;
    ///     
    ///     protected override void DisposeUnmanaged()
    ///     {
    ///         if (_nativePtr != IntPtr.Zero)
    ///             FreeNative(_nativePtr);
    ///         base.DisposeUnmanaged();
    ///     }
    /// }
    /// </code>
    /// </example>
    public abstract class DisposableObject : IDisposable
    {
        private volatile int _disposeSignaled = 0;

        /// <summary>
        /// 获取一个值，该值指示此实例是否已被释放。/ Gets a value indicating whether this instance has been disposed.
        /// </summary>
        /// <value>如果对象已被释放则为 true，否则为 false。/ true if the object has been disposed; otherwise, false.</value>
        public bool IsDisposed { get; protected set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否允许释放此实例。/ Gets or sets a value indicating whether you permit disposing this instance.
        /// </summary>
        /// <value>如果允许释放则为 true，否则为 false。/ true if dispose is enabled; otherwise, false.</value>
        /// <remarks>
        /// 当设置为 false 时，对象将不会被垃圾回收器释放。/ When set to false, the object will not be disposed by the garbage collector.
        /// </remarks>
        public bool IsEnabledDispose { get; set; }

        /// <summary>
        /// 获取或设置使用 GCHandle 分配的句柄。/ Gets or sets a handle which allocates using GCHandle.
        /// </summary>
        /// <value>GCHandle 实例，用于固定托管对象。/ GCHandle instance used to pin managed objects.</value>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// byte[] data = new byte[1024];
        /// GCHandle handle = AllocGCHandle(data);
        /// IntPtr ptr = handle.AddrOfPinnedObject();
        /// </code>
        /// </example>
        protected GCHandle DataHandle { get; private set; }

        /// <summary>
        /// 获取或设置通过 AllocMemory 分配的内存地址。/ Gets or sets a memory address allocated by AllocMemory.
        /// </summary>
        /// <value>分配的内存指针，如果未分配则为 IntPtr.Zero。/ Allocated memory pointer, or IntPtr.Zero if not allocated.</value>
        protected IntPtr AllocatedMemory { get; set; }

        /// <summary>
        /// 获取或设置分配内存的字节长度。/ Gets or sets the byte length of the allocated memory.
        /// </summary>
        /// <value>分配内存的大小（字节）。/ Size of allocated memory in bytes.</value>
        protected long AllocatedMemorySize { get; set; }

        /// <summary>
        /// 默认构造函数 / Default constructor
        /// <para>默认启用自动释放功能。/ Automatically enables dispose by default.</para>
        /// </summary>
        protected DisposableObject()
            : this(true)
        {
        }

        /// <summary>
        /// 构造函数 / Constructor
        /// </summary>
        /// <param name="isEnabledDispose">
        /// 如果允许GC释放此类则为 true。/ true if you permit disposing this class by GC.
        /// <para>设置为 false 可防止垃圾回收器自动释放对象。/ Set to false to prevent automatic disposal by garbage collector.</para>
        /// </param>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// // 创建不由GC管理的对象 / Create object not managed by GC
        /// var obj = new MyDisposable(false);
        /// </code>
        /// </example>
        protected DisposableObject(bool isEnabledDispose)
        {
            IsDisposed = false;
            IsEnabledDispose = isEnabledDispose;
            AllocatedMemory = IntPtr.Zero;
            AllocatedMemorySize = 0;
        }

        /// <summary>
        /// 释放资源 / Releases the resources
        /// <para>调用此方法后，对象将释放所有托管和非托管资源。/ After calling this method, the object releases all managed and unmanaged resources.</para>
        /// </summary>
        /// <remarks>
        /// 此方法实现了 IDisposable 接口，建议使用 using 语句或 try-finally 块调用。/ This method implements IDisposable interface, recommended to use with using statement or try-finally block.
        /// </remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// using (var obj = new MyDisposable())
        /// {
        ///     // 使用对象 / Use object
        /// } // 自动调用 Dispose / Dispose called automatically
        /// </code>
        /// </example>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源 / Releases the resources
        /// </summary>
        /// <param name="disposing">
        /// 如果为 true，则方法由用户代码直接调用，可释放托管和非托管资源；/ If true, the method has been called directly by user code, both managed and unmanaged resources can be disposed.
        /// <para>如果为 false，则方法由运行时在终结器内部调用，只能释放非托管资源。/ If false, the method has been called by runtime from inside the finalizer, only unmanaged resources can be disposed.</para>
        /// </param>
        /// <remarks>
        /// 此方法遵循标准 Dispose 模式，子类应重写 DisposeManaged 和 DisposeUnmanaged 方法。/ This method follows standard Dispose pattern, subclasses should override DisposeManaged and DisposeUnmanaged methods.
        /// </remarks>
        protected virtual void Dispose(bool disposing)
        {
#pragma warning disable 420
            // http://stackoverflow.com/questions/425132/a-reference-to-a-volatile-field-will-not-be-treated-as-volatile-implications
            if (Interlocked.Exchange(ref _disposeSignaled, 1) != 0)
            {
                return;
            }

            IsDisposed = true;

            if (IsEnabledDispose)
            {
                if (disposing)
                {
                    DisposeManaged();
                }
                DisposeUnmanaged();
            }
        }

        /// <summary>
        /// 析构函数 / Destructor
        /// <para>当对象被垃圾回收时调用，确保非托管资源被释放。/ Called when object is garbage collected to ensure unmanaged resources are released.</para>
        /// </summary>
        /// <remarks>
        /// 终结器仅作为安全网，建议显式调用 Dispose 方法。/ Finalizer acts as safety net, explicit Dispose call is recommended.
        /// </remarks>
        ~DisposableObject()
        {
            Dispose(false);
        }

        /// <summary>
        /// 释放托管资源 / Releases managed resources
        /// <para>子类应重写此方法以释放托管资源。/ Subclasses should override this method to release managed resources.</para>
        /// </summary>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// protected override void DisposeManaged()
        /// {
        ///     _managedResource?.Dispose();
        ///     _managedResource = null;
        ///     base.DisposeManaged();
        /// }
        /// </code>
        /// </example>
        protected virtual void DisposeManaged()
        {
        }

        /// <summary>
        /// 释放非托管资源 / Releases unmanaged resources
        /// <para>子类应重写此方法以释放非托管资源。/ Subclasses should override this method to release unmanaged resources.</para>
        /// <para>此方法会自动释放 GCHandle 和通过 AllocMemory 分配的内存。/ This method automatically releases GCHandle and memory allocated by AllocMemory.</para>
        /// </summary>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// protected override void DisposeUnmanaged()
        /// {
        ///     if (_nativePtr != IntPtr.Zero)
        ///     {
        ///         NativeFree(_nativePtr);
        ///         _nativePtr = IntPtr.Zero;
        ///     }
        ///     base.DisposeUnmanaged(); // 释放基类资源 / Release base class resources
        /// }
        /// </code>
        /// </example>
        protected virtual void DisposeUnmanaged()
        {
            if (DataHandle.IsAllocated)
            {
                DataHandle.Free();
            }
            if (AllocatedMemorySize > 0)
            {
                GC.RemoveMemoryPressure(AllocatedMemorySize);
                AllocatedMemorySize = 0;
            }
            if (AllocatedMemory != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(AllocatedMemory);
                AllocatedMemory = IntPtr.Zero;
            }
        }

        /// <summary>
        /// 使用 GCHandle 固定对象。/ Pins the object to be allocated by GCHandle.
        /// </summary>
        /// <param name="obj">要固定的对象，不能为 null。/ The object to pin, must not be null.</param>
        /// <returns>分配的 GCHandle 实例。/ The allocated GCHandle instance.</returns>
        /// <exception cref="ArgumentNullException">当 obj 为 null 时抛出。/ Thrown when obj is null.</exception>
        /// <remarks>
        /// 此方法会将对象固定在内存中，防止垃圾回收器移动它。/ This method pins the object in memory to prevent garbage collector from moving it.
        /// </remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// byte[] buffer = new byte[1024];
        /// GCHandle handle = AllocGCHandle(buffer);
        /// try
        /// {
        ///     IntPtr ptr = handle.AddrOfPinnedObject();
        ///     // 使用指针 / Use pointer
        /// }
        /// finally
        /// {
        ///     handle.Free();
        /// }
        /// </code>
        /// </example>
        protected internal GCHandle AllocGCHandle(object obj)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (DataHandle.IsAllocated)
                DataHandle.Free();
            DataHandle = GCHandle.Alloc(obj, GCHandleType.Pinned);
            return DataHandle;
        }

        /// <summary>
        /// 分配指定大小的内存。/ Allocates the specified size of memory.
        /// </summary>
        /// <param name="size">要分配的字节数，必须大于 0。/ The number of bytes to allocate, must be greater than 0.</param>
        /// <returns>分配的内存指针。/ The allocated memory pointer.</returns>
        /// <exception cref="ArgumentOutOfRangeException">当 size 小于等于 0 时抛出。/ Thrown when size is less than or equal to 0.</exception>
        /// <remarks>
        /// 分配的内存会在对象释放时自动释放。/ Allocated memory is automatically released when the object is disposed.
        /// <para>使用此方法分配的内存会通知 GC 以优化内存压力管理。/ Memory allocated with this method notifies GC for optimized memory pressure management.</para>
        /// </remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// IntPtr buffer = AllocMemory(1024);
        /// try
        /// {
        ///     Marshal.Copy(data, 0, buffer, data.Length);
        /// }
        /// finally
        /// {
        ///     // 内存会在 Dispose 时自动释放 / Memory released automatically in Dispose
        /// }
        /// </code>
        /// </example>
        protected IntPtr AllocMemory(int size)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size));

            if (AllocatedMemory != IntPtr.Zero)
                Marshal.FreeHGlobal(AllocatedMemory);
            AllocatedMemory = Marshal.AllocHGlobal(size);
            NotifyMemoryPressure(size);
            return AllocatedMemory;
        }

        /// <summary>
        /// 通知 GC 已分配的内存大小。/ Notifies the allocated size of memory.
        /// </summary>
        /// <param name="size">内存大小（字节），必须大于 0。/ Memory size in bytes, must be greater than 0.</param>
        /// <exception cref="ArgumentOutOfRangeException">当 size 小于等于 0 时抛出。/ Thrown when size is less than or equal to 0.</exception>
        /// <remarks>
        /// 此方法帮助 GC 了解对象持有的非托管内存量，以优化垃圾回收。/ This method helps GC understand the amount of unmanaged memory held by the object for optimized garbage collection.
        /// </remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// // 通知 GC 对象持有 10MB 非托管内存 / Notify GC that object holds 10MB unmanaged memory
        /// NotifyMemoryPressure(10 * 1024 * 1024);
        /// </code>
        /// </example>
        protected void NotifyMemoryPressure(long size)
        {
            if (!IsEnabledDispose)
                return;
            if (size == 0)
                return;
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size));

            if (AllocatedMemorySize > 0)
                GC.RemoveMemoryPressure(AllocatedMemorySize);

            AllocatedMemorySize = size;
            GC.AddMemoryPressure(size);
        }

        /// <summary>
        /// 如果此对象已被释放，则抛出 ObjectDisposedException。/ If this object is disposed, then ObjectDisposedException is thrown.
        /// </summary>
        /// <exception cref="ObjectDisposedException">当对象已被释放时抛出。/ Thrown when object has been disposed.</exception>
        /// <remarks>
        /// 在访问对象资源前调用此方法进行验证。/ Call this method before accessing object resources for validation.
        /// </remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// public void DoSomething()
        /// {
        ///     ThrowIfDisposed();
        ///     // 安全地访问资源 / Safely access resources
        /// }
        /// </code>
        /// </example>
        public void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);
        }
    }
}
