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

            _ptr = core.create_context(device_name);
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
            try
            {
                ExceptionHandler.ThrowOnError(ov_remote_context_get_device_name(_ptr, ref name_ptr));
                return StringUtils.Utf8PtrToString(name_ptr) ?? string.Empty;
            }
            finally
            {
                if (name_ptr != IntPtr.Zero)
                    ov_free(name_ptr);
            }
        }

        /// <summary>
        /// 获取设备名称 / Gets the device name.
        /// </summary>
        /// <returns>设备名称 / Device name.</returns>
        public string GetDeviceName()
        {
            return get_device_name();
        }

        /// <summary>
        /// 获取设备名称 / Gets the device name.
        /// </summary>
        public string DeviceName
        {
            get { return get_device_name(); }
        }

        /// <summary>
        /// 获取远程上下文参数字符串 / Get remote context parameter string
        /// </summary>
        /// <returns>设备相关参数字符串 / Device-specific parameter string.</returns>
        public string get_params()
        {
            ThrowIfDisposed();
            UIntPtr size = UIntPtr.Zero;
            IntPtr paramsPtr = IntPtr.Zero;
            try
            {
                ExceptionHandler.ThrowOnError(
                    ov_remote_context_get_params_native_size(_ptr, ref size, ref paramsPtr));
                return StringUtils.Utf8PtrToString(paramsPtr) ?? string.Empty;
            }
            finally
            {
                if (paramsPtr != IntPtr.Zero)
                    ov_free(paramsPtr);
            }
        }

        /// <summary>
        /// 获取远程上下文参数字符串 / Gets remote context parameters.
        /// </summary>
        /// <returns>设备相关参数字符串 / Device-specific parameter string.</returns>
        public string GetParams()
        {
            return get_params();
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
                ov_remote_context_create_tensor_native_size(_ptr, (uint)type, shape, UIntPtr.Zero, ref tensor_ptr));
            return tensor_ptr;
        }

        /// <summary>
        /// 在远程设备上创建张量 / Creates a tensor on the remote device.
        /// </summary>
        /// <param name="type">元素类型 / Element type.</param>
        /// <param name="shape">张量形状 / Tensor shape.</param>
        /// <returns>原生张量指针 / Native tensor pointer.</returns>
        public IntPtr CreateTensor(ElementType type, ov_shape_t shape)
        {
            return create_tensor(type, shape);
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

        /// <summary>
        /// 创建主机张量 / Creates a host tensor with device-friendly memory.
        /// </summary>
        /// <param name="type">元素类型 / Element type.</param>
        /// <param name="shape">张量形状 / Tensor shape.</param>
        /// <returns>原生张量指针 / Native tensor pointer.</returns>
        public IntPtr CreateHostTensor(ElementType type, ov_shape_t shape)
        {
            return create_host_tensor(type, shape);
        }

        #endregion

        /// <summary>
        /// 获取原生指针（兼容属性）/ Get native pointer (compatibility property)
        /// </summary>
        /// <value>原生指针 / Native pointer</value>
        public IntPtr Ptr => OvPtr;
    }
}
