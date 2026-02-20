// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using static OpenVinoSharp.native.NativeMethods;
using OpenVinoSharp.Internal;
using OpenVinoSharp.native;

namespace OpenVinoSharp
{
    /// <summary>
    /// 远程上下文类 / Remote context class
    /// <para>用于管理远程设备（如GPU）的内存和计算资源。/ Used to manage memory and compute resources on remote devices (e.g., GPU).</para>
    /// </summary>
    /// <example>
    /// 使用示例 / Usage example:
    /// <code>
    /// using (Core core = new Core())
    /// {
    ///     // 为GPU创建设备上下文 / Create device context for GPU
    ///     using (RemoteContext context = new RemoteContext(core, "GPU"))
    ///     {
    ///         string deviceName = context.get_device_name();
    ///         Console.WriteLine($"设备: {deviceName}"); // 输出 / Output: GPU
    ///         
    ///         // 在远程设备上创建张量 / Create tensor on remote device
    ///         ov_shape_t shape = new ov_shape_t { rank = 4, dims = new long[] { 1, 3, 224, 224 } };
    ///         IntPtr tensorPtr = context.create_tensor(ElementType.F32, shape);
    ///     }
    /// }
    /// </code>
    /// </example>
    public class RemoteContext : DisposableOvObject
    {
        #region 构造函数 / Constructors

        /// <summary>
        /// 从原生指针构造 / Construct from native pointer
        /// </summary>
        /// <param name="ptr">原生远程上下文指针 / Native remote context pointer</param>
        public RemoteContext(IntPtr ptr) : base(ptr) { }

        /// <summary>
        /// 创建远程上下文 / Create remote context
        /// </summary>
        /// <param name="core">Core实例 / Core instance</param>
        /// <param name="device_name">设备名称 / Device name</param>
        /// <exception cref="ArgumentNullException">当core为null时抛出 / Thrown when core is null</exception>
        /// <exception cref="ArgumentException">当设备名称为空时抛出 / Thrown when device name is empty</exception>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// using (Core core = new Core())
        /// {
        ///     using (RemoteContext context = new RemoteContext(core, "GPU"))
        ///     {
        ///         Console.WriteLine(context.get_device_name());
        ///     }
        /// }
        /// </code>
        /// </example>
        public RemoteContext(Core core, string device_name) : base()
        {
            if (core == null)
                throw new ArgumentNullException(nameof(core));
            if (string.IsNullOrEmpty(device_name))
                throw new ArgumentException("参数不能为空 / Parameter cannot be empty", nameof(device_name));

            ExceptionHandler.ThrowOnError(ov_core_create_context(core.OvPtr, device_name, 0, ref _ptr));
        }

        #endregion

        #region 资源释放 / Resource Disposal

        /// <inheritdoc/>
        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero && IsEnabledDispose)
            {
                ov_remote_context_free(_ptr);
            }
            base.DisposeUnmanaged();
        }

        #endregion

        #region 设备信息 / Device Information

        /// <summary>
        /// 获取设备名称 / Get device name
        /// </summary>
        /// <returns>设备名称 / Device name</returns>
        /// <exception cref="ObjectDisposedException">当对象已释放时抛出 / Thrown when object is disposed</exception>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// using (RemoteContext context = new RemoteContext(core, "GPU.0"))
        /// {
        ///     string name = context.get_device_name();
        ///     Console.WriteLine(name); // "GPU.0"
        /// }
        /// </code>
        /// </example>
        public string get_device_name()
        {
            ThrowIfDisposed();
            IntPtr name_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_remote_context_get_device_name(_ptr, ref name_ptr));
            string name = Marshal.PtrToStringAnsi(name_ptr) ?? string.Empty;
            ov_free(name_ptr);
            return name;
        }

        #endregion

        #region 张量创建 / Tensor Creation

        /// <summary>
        /// 在远程设备上创建张量 / Create tensor on remote device
        /// </summary>
        /// <param name="type">元素类型 / Element type</param>
        /// <param name="shape">张量形状 / Tensor shape</param>
        /// <returns>张量指针 / Tensor pointer</returns>
        /// <exception cref="ObjectDisposedException">当对象已释放时抛出 / Thrown when object is disposed</exception>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// ov_shape_t shape = new ov_shape_t { rank = 4, dims = new long[] { 1, 3, 224, 224 } };
        /// IntPtr tensorPtr = context.create_tensor(ElementType.F32, shape);
        /// </code>
        /// </example>
        public IntPtr create_tensor(ElementType type, ov_shape_t shape)
        {
            ThrowIfDisposed();
            IntPtr tensor_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(
                ov_remote_context_create_tensor(_ptr, (uint)type, shape, 0, ref tensor_ptr));
            return tensor_ptr;
        }

        /// <summary>
        /// 创建主机张量（对设备友好的内存）/ Create host tensor (device-friendly memory)
        /// </summary>
        /// <param name="type">元素类型 / Element type</param>
        /// <param name="shape">张量形状 / Tensor shape</param>
        /// <returns>张量指针 / Tensor pointer</returns>
        /// <exception cref="ObjectDisposedException">当对象已释放时抛出 / Thrown when object is disposed</exception>
        /// <remarks>
        /// 创建的张量内存布局对远程设备最优，可减少数据传输 / 
        /// The created tensor has optimal memory layout for the remote device, reducing data transfer
        /// </remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// ov_shape_t shape = new ov_shape_t { rank = 4, dims = new long[] { 1, 3, 224, 224 } };
        /// // 创建GPU友好的主机张量 / Create GPU-friendly host tensor
        /// IntPtr tensorPtr = context.create_host_tensor(ElementType.F32, shape);
        /// </code>
        /// </example>
        public IntPtr create_host_tensor(ElementType type, ov_shape_t shape)
        {
            ThrowIfDisposed();
            IntPtr tensor_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(
                ov_remote_context_create_host_tensor(_ptr, (uint)type, shape, ref tensor_ptr));
            return tensor_ptr;
        }

        #endregion

        /// <summary>
        /// 获取原生指针（兼容属性）/ Get native pointer (compatibility property)
        /// </summary>
        /// <value>原生指针 / Native pointer</value>
        public IntPtr Ptr => OvPtr;
    }
}
