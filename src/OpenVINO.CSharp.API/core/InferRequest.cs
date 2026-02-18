// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using static OpenVinoSharp.native.NativeMethods;
using OpenVinoSharp.Internal;

namespace OpenVinoSharp
{
    /// <summary>
    /// 推理请求类 / Inference request class
    /// <para>用于同步或异步推理。/ Used for synchronous or asynchronous inference.</para>
    /// <para>性能优化：支持 async/await 模式和对象池。/ Performance optimized: supports async/await pattern and object pooling.</para>
    /// </summary>
    public class InferRequest : DisposableOvObject
    {
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

        /// <summary>
        /// 获取原生指针（兼容属性）/ Get native pointer (compatibility property)
        /// </summary>
        public IntPtr Ptr => OvPtr;
    }
}
