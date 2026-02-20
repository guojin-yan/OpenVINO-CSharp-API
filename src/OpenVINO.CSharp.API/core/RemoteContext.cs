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
        public RemoteContext(Core core, string device_name) : base()
        {
            if (core == null)
                throw new ArgumentNullException(nameof(core));
            if (string.IsNullOrEmpty(device_name))
                throw new ArgumentException("参数不能为空", nameof(device_name));

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
        public IntPtr Ptr => OvPtr;
    }
}
