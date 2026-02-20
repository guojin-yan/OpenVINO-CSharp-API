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
//  📌 示例代码：https://github.com/guojin-yan/OpenVINO-CSharp-API/tree/csharp3.2/samples
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
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using static OpenVinoSharp.native.NativeMethods;
using OpenVinoSharp.Internal;
using OpenVinoSharp.native;

namespace OpenVinoSharp
{
    /// <summary>
    /// 推理请求类 / Inference request class
    /// <para>用于同步或异步推理。/ Used for synchronous or asynchronous inference.</para>
    /// <para>性能优化：支持 async/await 模式和对象池。/ Performance optimized: supports async/await pattern and object pooling.</para>
    /// </summary>
    public class InferRequest : DisposableOvObject
    {
        #region 字段 / Fields

        // 静态回调字典，防止委托被垃圾回收 / Static callback dictionary to prevent delegates from being GC'd
        private static readonly Dictionary<IntPtr, Action> _callbackRegistry = new Dictionary<IntPtr, Action>();
        private static readonly object _registryLock = new object();
        
        // 空操作回调委托（用于清除回调）/ No-op callback delegate (used for clearing callback)
        private static readonly ov_infer_request_callback_func _noOpCallback = (IntPtr args) => { };
        private static readonly IntPtr _noOpCallbackPtr = Marshal.GetFunctionPointerForDelegate(_noOpCallback);
        
        // 当前请求注册的回调 / Currently registered callback for this request
        private Action? _currentCallback;
        
        // 原生回调委托实例（保持引用防止GC）/ Native callback delegate instance (keep reference to prevent GC)
        private ov_infer_request_callback_func? _nativeCallbackDelegate;
        
        // 原生回调结构体（必须保持生命周期，C++保存了指针）/ Native callback struct (must keep alive, C++ stores pointer)
        private ov_callback_t _callbackStruct;

        #endregion

        #region 构造函数 / Constructors

        /// <summary>
        /// 从原生指针构造 / Construct from native pointer
        /// </summary>
        /// <param name="ptr">原生推理请求指针 / Native inference request pointer</param>
        public InferRequest(IntPtr ptr) : base(ptr)
        {
            if (ptr == IntPtr.Zero)
                throw new ArgumentNullException(nameof(ptr), "原生对象地址为空 / Native object address is NULL");
        }

        #endregion

        #region 资源释放 / Resource Disposal

        /// <inheritdoc/>
        protected override void DisposeUnmanaged()
        {
            // 从注册表中移除回调 / Remove callback from registry
            if (_currentCallback != null)
            {
                lock (_registryLock)
                {
                    _callbackRegistry.Remove(_ptr);
                }
                _currentCallback = null;
                _nativeCallbackDelegate = null;
            }

            if (_ptr != IntPtr.Zero && IsEnabledDispose)
            {
                ov_infer_request_free(_ptr);
            }
            base.DisposeUnmanaged();
        }

        #endregion

        #region 输入张量设置 / Input Tensor Setting

        /// <summary>
        /// 设置单个输入张量 / Set single input tensor
        /// <para>用于单输入模型。/ Used for single-input models.</para>
        /// </summary>
        /// <param name="tensor">输入张量 / Input tensor</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void set_input_tensor(Tensor tensor)
        {
            ThrowIfDisposed();
            if (tensor == null)
                throw new ArgumentNullException(nameof(tensor));
            ExceptionHandler.ThrowOnError(
                ov_infer_request_set_input_tensor(_ptr, tensor.OvPtr));
        }

        /// <summary>
        /// 通过索引设置输入张量 / Set input tensor by index
        /// </summary>
        /// <param name="idx">输入索引 / Input index</param>
        /// <param name="tensor">输入张量 / Input tensor</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void set_input_tensor(ulong idx, Tensor tensor)
        {
            ThrowIfDisposed();
            if (tensor == null)
                throw new ArgumentNullException(nameof(tensor));
            ExceptionHandler.ThrowOnError(
                ov_infer_request_set_input_tensor_by_index(_ptr, idx, tensor.OvPtr));
        }

        /// <summary>
        /// 通过张量名称设置输入张量 / Set input tensor by tensor name
        /// </summary>
        /// <param name="tensor_name">张量名称 / Tensor name</param>
        /// <param name="tensor">输入张量 / Input tensor</param>
        public void set_input_tensor(string tensor_name, Tensor tensor)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(tensor_name))
                throw new ArgumentException("Parameter cannot be null or empty", nameof(tensor_name));
            if (tensor == null)
                throw new ArgumentNullException(nameof(tensor));
            
            ExceptionHandler.ThrowOnError(
                ov_infer_request_set_tensor(_ptr, tensor_name, tensor.OvPtr));
        }

        /// <summary>
        /// 设置多个输入张量 / Set multiple input tensors
        /// </summary>
        /// <param name="tensors">输入张量字典 / Dictionary of input tensors</param>
        public void set_input_tensors(Dictionary<string, Tensor> tensors)
        {
            ThrowIfDisposed();
            if (tensors == null)
                throw new ArgumentNullException(nameof(tensors));
            
            foreach (var pair in tensors)
            {
                set_input_tensor(pair.Key, pair.Value);
            }
        }

        #endregion

        #region 输出张量设置 / Output Tensor Setting

        /// <summary>
        /// 设置单个输出张量 / Set single output tensor
        /// <para>用于单输出模型。/ Used for single-output models.</para>
        /// </summary>
        /// <param name="tensor">输出张量 / Output tensor</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void set_output_tensor(Tensor tensor)
        {
            ThrowIfDisposed();
            if (tensor == null)
                throw new ArgumentNullException(nameof(tensor));
            ExceptionHandler.ThrowOnError(
                ov_infer_request_set_output_tensor(_ptr, tensor.OvPtr));
        }

        /// <summary>
        /// 通过索引设置输出张量 / Set output tensor by index
        /// </summary>
        /// <param name="idx">输出索引 / Output index</param>
        /// <param name="tensor">输出张量 / Output tensor</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void set_output_tensor(ulong idx, Tensor tensor)
        {
            ThrowIfDisposed();
            if (tensor == null)
                throw new ArgumentNullException(nameof(tensor));
            ExceptionHandler.ThrowOnError(
                ov_infer_request_set_output_tensor_by_index(_ptr, idx, tensor.OvPtr));
        }

        /// <summary>
        /// 通过张量名称设置输出张量 / Set output tensor by tensor name
        /// </summary>
        /// <param name="tensor_name">张量名称 / Tensor name</param>
        /// <param name="tensor">输出张量 / Output tensor</param>
        public void set_output_tensor(string tensor_name, Tensor tensor)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(tensor_name))
                throw new ArgumentException("Parameter cannot be null or empty", nameof(tensor_name));
            if (tensor == null)
                throw new ArgumentNullException(nameof(tensor));
            
            ExceptionHandler.ThrowOnError(
                ov_infer_request_set_tensor(_ptr, tensor_name, tensor.OvPtr));
        }

        /// <summary>
        /// 通过端口设置张量 / Set tensor by port
        /// </summary>
        /// <param name="port">节点端口 / Node port</param>
        /// <param name="tensor">张量 / Tensor</param>
        public void set_tensor_by_port(NodeOutput port, Tensor tensor)
        {
            ThrowIfDisposed();
            if (port == null)
                throw new ArgumentNullException(nameof(port));
            if (tensor == null)
                throw new ArgumentNullException(nameof(tensor));

            ExceptionHandler.ThrowOnError(
                ov_infer_request_set_tensor_by_port(_ptr, port.OvPtr, tensor.OvPtr));
        }

        /// <summary>
        /// 通过常量端口设置张量 / Set tensor by const port
        /// </summary>
        /// <param name="port">常量节点端口 / Const node port</param>
        /// <param name="tensor">张量 / Tensor</param>
        public void set_tensor_by_const_port(NodeInput port, Tensor tensor)
        {
            ThrowIfDisposed();
            if (port == null)
                throw new ArgumentNullException(nameof(port));
            if (tensor == null)
                throw new ArgumentNullException(nameof(tensor));

            ExceptionHandler.ThrowOnError(
                ov_infer_request_set_tensor_by_const_port(_ptr, port.OvPtr, tensor.OvPtr));
        }

        #endregion

        #region 张量获取 / Tensor Getting

        /// <summary>
        /// 获取单个输入张量 / Get single input tensor
        /// </summary>
        /// <returns>输入张量 / Input tensor</returns>
        public Tensor get_input_tensor()
        {
            ThrowIfDisposed();
            IntPtr tensor_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(
                ov_infer_request_get_input_tensor(_ptr, ref tensor_ptr));
            return new Tensor(tensor_ptr);
        }

        /// <summary>
        /// 通过索引获取输入张量 / Get input tensor by index
        /// </summary>
        /// <param name="idx">输入索引 / Input index</param>
        /// <returns>输入张量 / Input tensor</returns>
        public Tensor get_input_tensor(ulong idx)
        {
            ThrowIfDisposed();
            IntPtr tensor_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(
                ov_infer_request_get_input_tensor_by_index(_ptr, idx, ref tensor_ptr));
            return new Tensor(tensor_ptr);
        }

        /// <summary>
        /// 通过张量名称获取张量 / Get tensor by name
        /// </summary>
        /// <param name="tensor_name">张量名称 / Tensor name</param>
        /// <returns>张量 / Tensor</returns>
        public Tensor get_tensor(string tensor_name)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(tensor_name))
                throw new ArgumentException("Parameter cannot be null or empty", nameof(tensor_name));
            
            IntPtr tensor_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(
                ov_infer_request_get_tensor(_ptr, tensor_name, ref tensor_ptr));
            return new Tensor(tensor_ptr);
        }

        /// <summary>
        /// 获取单个输出张量 / Get single output tensor
        /// </summary>
        /// <returns>输出张量 / Output tensor</returns>
        public Tensor get_output_tensor()
        {
            ThrowIfDisposed();
            IntPtr tensor_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(
                ov_infer_request_get_output_tensor(_ptr, ref tensor_ptr));
            return new Tensor(tensor_ptr);
        }

        /// <summary>
        /// 通过索引获取输出张量 / Get output tensor by index
        /// </summary>
        /// <param name="idx">输出索引 / Output index</param>
        /// <returns>输出张量 / Output tensor</returns>
        public Tensor get_output_tensor(ulong idx)
        {
            ThrowIfDisposed();
            IntPtr tensor_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(
                ov_infer_request_get_output_tensor_by_index(_ptr, idx, ref tensor_ptr));
            return new Tensor(tensor_ptr);
        }

        /// <summary>
        /// 通过张量名称获取输出张量 / Get output tensor by tensor name
        /// </summary>
        /// <param name="tensor_name">张量名称 / Tensor name</param>
        /// <returns>输出张量 / Output tensor</returns>
        public Tensor get_output_tensor(string tensor_name)
        {
            return get_tensor(tensor_name);
        }

        /// <summary>
        /// 通过端口获取张量 / Get tensor by port
        /// </summary>
        /// <param name="port">节点端口 / Node port</param>
        /// <returns>张量 / Tensor</returns>
        public Tensor get_tensor_by_port(NodeOutput port)
        {
            ThrowIfDisposed();
            if (port == null)
                throw new ArgumentNullException(nameof(port));

            IntPtr tensor_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(
                ov_infer_request_get_tensor_by_port(_ptr, port.OvPtr, ref tensor_ptr));
            return new Tensor(tensor_ptr);
        }

        /// <summary>
        /// 通过常量端口获取张量 / Get tensor by const port
        /// </summary>
        /// <param name="port">常量节点端口 / Const node port</param>
        /// <returns>张量 / Tensor</returns>
        public Tensor get_tensor_by_const_port(NodeInput port)
        {
            ThrowIfDisposed();
            if (port == null)
                throw new ArgumentNullException(nameof(port));

            IntPtr tensor_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(
                ov_infer_request_get_tensor_by_const_port(_ptr, port.OvPtr, ref tensor_ptr));
            return new Tensor(tensor_ptr);
        }

        #endregion

        #region 推理执行 / Inference Execution

        /// <summary>
        /// 执行同步推理 / Perform synchronous inference
        /// <para>阻塞调用，直到推理完成。/ Blocking call until inference completes.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void infer()
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(ov_infer_request_infer(_ptr));
        }

        /// <summary>
        /// 执行同步推理并获取结果 / Perform synchronous inference and get results
        /// </summary>
        /// <returns>输出张量数组 / Array of output tensors</returns>
        public Tensor[] infer_and_get_results()
        {
            infer();
            // ov_infer_request_get_compiled_model is not available, 
            // so we assume single output for this helper method
            // Users should call get_output_tensor() directly for multiple outputs
            return new Tensor[] { get_output_tensor() };
        }

        #endregion

        #region 异步推理 / Asynchronous Inference

        /// <summary>
        /// 启动异步推理 / Start asynchronous inference
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void start_async()
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(ov_infer_request_start_async(_ptr));
        }

        /// <summary>
        /// 等待推理完成（阻塞）/ Wait for inference to complete (blocking)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void wait()
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(ov_infer_request_wait(_ptr));
        }

        /// <summary>
        /// 等待推理完成（带超时）/ Wait for inference to complete (with timeout)
        /// </summary>
        /// <param name="timeout">超时时间（毫秒）/ Timeout in milliseconds</param>
        /// <returns>是否在超时前完成 / Whether completed before timeout</returns>
        public bool wait_for(long timeout)
        {
            ThrowIfDisposed();
            ExceptionStatus status = ov_infer_request_wait_for(_ptr, timeout);
            return status == ExceptionStatus.OK;
        }

        /// <summary>
        /// 取消推理 / Cancel inference
        /// </summary>
        public void cancel()
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(ov_infer_request_cancel(_ptr));
        }

        /// <summary>
        /// 静态原生回调函数 / Static native callback function
        /// <para>通过 args 参数获取用户回调并执行。</para>
        /// </summary>
        private static void NativeCallbackHandler(IntPtr args)
        {
            if (args == IntPtr.Zero) return;
            
            Action? callback = null;
            lock (_registryLock)
            {
                _callbackRegistry.TryGetValue(args, out callback);
            }
            callback?.Invoke();
        }

        /// <summary>
        /// 设置异步推理完成回调 / Set callback for async inference completion
        /// <para>当异步推理完成时，将调用此回调函数。</para>
        /// </summary>
        /// <param name="callback">回调函数 / Callback function</param>
        public void set_callback(Action callback)
        {
            ThrowIfDisposed();
            
            // 清除之前的回调 / Clear previous callback
            if (_currentCallback != null)
            {
                lock (_registryLock)
                {
                    _callbackRegistry.Remove(_ptr);
                }
                _currentCallback = null;
                _nativeCallbackDelegate = null;
            }

            if (callback == null)
            {
                // 清除回调 - 使用空操作回调而不是空指针 / Clear callback - use no-op callback instead of null pointer
                // C++ 代码总会调用 callback_func，所以不能传空指针
                _callbackStruct = new ov_callback_t
                {
                    callback_func = _noOpCallbackPtr,
                    args = IntPtr.Zero
                };
                ExceptionHandler.ThrowOnError(ov_infer_request_set_callback(_ptr, ref _callbackStruct));
                return;
            }

            // 保存当前回调 / Save current callback
            _currentCallback = callback;
            
            // 创建并保存原生回调委托 / Create and save native callback delegate
            _nativeCallbackDelegate = NativeCallbackHandler;

            lock (_registryLock)
            {
                _callbackRegistry[_ptr] = callback;
            }

            // 保存到实例字段，确保生命周期与对象相同 / Save to instance field to ensure same lifetime as object
            _callbackStruct = new ov_callback_t
            {
                callback_func = Marshal.GetFunctionPointerForDelegate(_nativeCallbackDelegate),
                args = _ptr  // 使用请求指针作为 key / Use request pointer as key
            };

            ExceptionHandler.ThrowOnError(ov_infer_request_set_callback(_ptr, ref _callbackStruct));
        }

#if HAS_ASYNC_ENUMERABLE
        /// <summary>
        /// 执行异步推理（async/await 模式）/ Perform asynchronous inference (async/await pattern)
        /// <para>.NET Core 3.0+ / .NET 5+ 支持 / Supported on .NET Core 3.0+ / .NET 5+</para>
        /// </summary>
        /// <param name="cancellationToken">取消令牌 / Cancellation token</param>
        /// <returns>异步任务 / Async task</returns>
        public async Task infer_async(CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            
            start_async();
            
            // 使用轮询等待，支持取消令牌
            while (!cancellationToken.IsCancellationRequested)
            {
                if (wait_for(10)) // 10ms 轮询间隔
                {
                    return;
                }
                await Task.Yield(); // 让出线程
            }
            
            // 取消推理
            cancel();
            throw new OperationCanceledException(cancellationToken);
        }

        /// <summary>
        /// 执行异步推理并获取结果 / Perform asynchronous inference and get results
        /// </summary>
        /// <param name="cancellationToken">取消令牌 / Cancellation token</param>
        /// <returns>输出张量数组 / Array of output tensors</returns>
        public async Task<Tensor[]> infer_async_and_get_results(CancellationToken cancellationToken = default)
        {
            await infer_async(cancellationToken);
            return new Tensor[] { get_output_tensor() };
        }
#endif

        #endregion

        #region 性能分析 / Profiling

        /// <summary>
        /// 获取性能分析信息 / Get profiling information
        /// <para>返回每个层的性能测量数据，用于识别最耗时的操作。</para>
        /// </summary>
        /// <returns>性能分析信息列表 / List of profiling information</returns>
        public ProfilingInfo[] get_profiling_info()
        {
            ThrowIfDisposed();
            
            ov_profiling_info_list_t info_list = new ov_profiling_info_list_t();
            try
            {
                ExceptionHandler.ThrowOnError(ov_infer_request_get_profiling_info(_ptr, ref info_list));
                
                ProfilingInfo[] result = new ProfilingInfo[info_list.size];
                int structSize = Marshal.SizeOf(typeof(ov_profiling_info_t));
                
                for (ulong i = 0; i < info_list.size; i++)
                {
                    IntPtr ptr = new IntPtr(info_list.profiling_infos.ToInt64() + (long)(i * (ulong)structSize));
                    ov_profiling_info_t native_info = Marshal.PtrToStructure<ov_profiling_info_t>(ptr);
                    
                    result[i] = new ProfilingInfo
                    {
                        status = (ProfilingInfo.Status)(int)native_info.status,
                        real_time = native_info.real_time,
                        cpu_time = native_info.cpu_time,
                        node_name = Marshal.PtrToStringAnsi(native_info.node_name) ?? string.Empty,
                        exec_type = Marshal.PtrToStringAnsi(native_info.exec_type) ?? string.Empty,
                        node_type = Marshal.PtrToStringAnsi(native_info.node_type) ?? string.Empty
                    };
                }
                
                return result;
            }
            finally
            {
                if (info_list.profiling_infos != IntPtr.Zero)
                {
                    ov_profiling_info_list_free(ref info_list);
                }
            }
        }

        #endregion

        /// <summary>
        /// 获取原生指针（兼容属性）/ Get native pointer (compatibility property)
        /// </summary>
        public IntPtr Ptr => OvPtr;
    }
}
