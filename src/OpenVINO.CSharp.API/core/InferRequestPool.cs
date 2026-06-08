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
using System.Collections.Concurrent;
using System.Threading;
using OpenVinoSharp.Internal;

namespace OpenVinoSharp
{
    /// <summary>
    /// 推理请求对象池 / Inference Request Pool
    /// <para>重用 InferRequest 对象，减少频繁创建/销毁的开销。/ Reuses InferRequest objects to reduce creation/disposal overhead.</para>
    /// <para>适用于高并发推理场景，如Web服务。/ Suitable for high-concurrency inference scenarios like web services.</para>
    /// </summary>
    public class InferRequestPool : IDisposable
    {
        private readonly CompiledModel _compiledModel;
        private readonly ConcurrentBag<InferRequest> _pool;
        private readonly SemaphoreSlim _semaphore;
        private readonly int _maxSize;
        private int _currentSize;
        private bool _disposed;

        /// <summary>
        /// 创建推理请求池 / Create inference request pool
        /// </summary>
        /// <param name="compiledModel">编译后的模型 / Compiled model</param>
        /// <param name="initialSize">初始池大小 / Initial pool size</param>
        /// <param name="maxSize">最大池大小 / Maximum pool size</param>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// using var core = new Core();
        /// using var model = core.read_model("model.xml");
        /// using var compiled = core.compile_model(model, "CPU");
        /// 
        /// // 创建池 / Create pool
        /// using var pool = new InferRequestPool(compiled, initialSize: 2, maxSize: 10);
        /// 
        /// // 获取请求执行推理 / Rent request and run inference
        /// var request = pool.Rent();
        /// try {
        ///     request.set_input_tensor(input);
        ///     request.infer();
        ///     var output = request.get_output_tensor();
        /// } finally {
        ///     pool.Return(request);
        /// }
        /// </code>
        /// </example>
        public InferRequestPool(CompiledModel compiledModel, int initialSize = 2, int maxSize = 10)
        {
            _compiledModel = compiledModel ?? throw new ArgumentNullException(nameof(compiledModel));
            _pool = new ConcurrentBag<InferRequest>();
            // 信号量表示总共可以租用的对象数（池中对象 + 还可以创建的新对象）
            // Semaphore represents total rentable objects (in pool + can be created)
            _semaphore = new SemaphoreSlim(maxSize, maxSize);
            _maxSize = maxSize;
            _currentSize = 0;

            // 预热：预先创建初始数量的请求
            // Pre-warm: create initial number of requests
            for (int i = 0; i < initialSize; i++)
            {
                var request = CreateRequest();
                if (request != null)
                {
                    _pool.Add(request);
                    Interlocked.Increment(ref _currentSize);
                }
            }

            OvLogger.Debug($"InferRequestPool: 创建完成，初始大小: {initialSize}, 最大大小: {maxSize}");
            OvLogger.Debug($"InferRequestPool: Created, initial size: {initialSize}, max size: {maxSize}");
        }

        /// <summary>
        /// 当前池大小 / Current pool size
        /// </summary>
        public int Count => _currentSize;

        /// <summary>
        /// 可用请求数量 / Available request count
        /// </summary>
        public int AvailableCount => _pool.Count;

        /// <summary>
        /// 从池中获取推理请求（阻塞直到可用）/ Rent inference request from pool (blocks until available)
        /// </summary>
        /// <returns>推理请求对象 / Inference request object</returns>
        /// <exception cref="ObjectDisposedException">当对象池已被释放时抛出 / Thrown when the pool has been disposed</exception>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// var pool = new InferRequestPool(compiledModel, initialSize: 2, maxSize: 10);
        /// 
        /// // 获取请求（阻塞直到可用）/ Rent request (blocks until available)
        /// var request = pool.Rent();
        /// try {
        ///     // 设置输入 / Set input
        ///     request.set_input_tensor(input);
        ///     
        ///     // 执行推理 / Run inference
        ///     request.infer();
        ///     
        ///     // 获取输出 / Get output
        ///     var output = request.get_output_tensor();
        /// } finally {
        ///     // 必须归还请求 / Must return request
        ///     pool.Return(request);
        /// }
        /// </code>
        /// </example>
        public InferRequest Rent()
        {
            ThrowIfDisposed();
            _semaphore.Wait();
            return RentCore();
        }

        /// <summary>
        /// 异步获取推理请求 / Rent inference request asynchronously
        /// </summary>
        /// <param name="cancellationToken">取消令牌 / Cancellation token</param>
        /// <returns>推理请求对象 / Inference request object</returns>
        /// <exception cref="ObjectDisposedException">当对象池已被释放时抛出 / Thrown when the pool has been disposed</exception>
        /// <exception cref="OperationCanceledException">当操作被取消时抛出 / Thrown when operation is cancelled</exception>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// var pool = new InferRequestPool(compiledModel, initialSize: 2, maxSize: 10);
        /// 
        /// // 异步获取请求 / Rent request asynchronously
        /// var request = await pool.RentAsync(cancellationToken);
        /// try {
        ///     request.set_input_tensor(input);
        ///     request.infer();
        ///     var output = request.get_output_tensor();
        /// } finally {
        ///     pool.Return(request);
        /// }
        /// </code>
        /// </example>
        public System.Threading.Tasks.Task<InferRequest> RentAsync(
            System.Threading.CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            return RentAsyncCore(cancellationToken);
        }

        /// <summary>
        /// 尝试获取推理请求（非阻塞）/ Try to rent inference request (non-blocking)
        /// </summary>
        /// <param name="request">获取到的请求 / The rented request, or null if not available</param>
        /// <returns>是否成功获取 / True if request was available, false otherwise</returns>
        /// <exception cref="ObjectDisposedException">当对象池已被释放时抛出 / Thrown when the pool has been disposed</exception>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// var pool = new InferRequestPool(compiledModel, initialSize: 2, maxSize: 10);
        /// 
        /// // 尝试获取请求（非阻塞）/ Try to rent request (non-blocking)
        /// if (pool.TryRent(out var request)) {
        ///     try {
        ///         request.set_input_tensor(input);
        ///         request.infer();
        ///         var output = request.get_output_tensor();
        ///     } finally {
        ///         pool.Return(request);
        ///     }
        /// } else {
        ///     // 池已满，处理等待或放弃 / Pool full, handle wait or fallback
        ///     Console.WriteLine("Pool is full, please try again later");
        /// }
        /// </code>
        /// </example>
        public bool TryRent(out InferRequest request)
        {
            ThrowIfDisposed();
            if (_semaphore.Wait(0))
            {
                request = RentCore();
                return true;
            }
            request = null;
            return false;
        }

        /// <summary>
        /// 归还推理请求到池中 / Return inference request to pool
        /// </summary>
        /// <param name="request">推理请求对象 / Inference request object to return</param>
        /// <remarks>
        /// 如果请求已被释放，则不会返回池中，而是减少池的计数。/ If the request has been disposed, it will not be returned to the pool; instead, the pool count is decreased.
        /// </remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// var pool = new InferRequestPool(compiledModel, initialSize: 2, maxSize: 10);
        /// var request = pool.Rent();
        /// 
        /// try {
        ///     // 执行推理 / Run inference
        ///     request.set_input_tensor(input);
        ///     request.infer();
        /// } finally {
        ///     // 确保请求被归还，即使在异常情况下 / Ensure request is returned even on exception
        ///     pool.Return(request);
        /// }
        /// </code>
        /// </example>
        public void Return(InferRequest request)
        {
            if (request == null || _disposed)
                return;

            // 检查请求是否有效
            // Check if request is valid
            if (request.IsDisposed)
            {
                // 如果请求已被释放，减少计数并创建新的
                // If request is disposed, decrease count
                Interlocked.Decrement(ref _currentSize);
                _semaphore.Release();
                return;
            }

            // 重置请求状态（取消任何待处理的推理）
            // Reset request state (cancel any pending inference)
            try
            {
                request.cancel();
            }
            catch { }

            _pool.Add(request);
            _semaphore.Release();
        }

        /// <summary>
        /// 执行推理并自动归还请求（便捷方法）/ Run inference and auto-return request (convenience method)
        /// </summary>
        /// <param name="inputSetter">设置输入的委托 / Delegate to set input tensors</param>
        /// <param name="outputGetter">获取输出的委托 / Delegate to get output tensors</param>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// var pool = new InferRequestPool(compiledModel, initialSize: 2, maxSize: 10);
        /// 
        /// // 使用便捷方法执行推理 / Use convenience method to run inference
        /// pool.RunInference(
        ///     request => {
        ///         // 设置输入 / Set input
        ///         request.set_input_tensor(input);
        ///     },
        ///     request => {
        ///         // 获取输出 / Get output
        ///         var output = request.get_output_tensor();
        ///         // 处理输出... / Process output...
        ///     }
        /// );
        /// // 请求自动归还 / Request is automatically returned
        /// </code>
        /// </example>
        public void RunInference(Action<InferRequest> inputSetter, Action<InferRequest> outputGetter)
        {
            var request = Rent();
            try
            {
                inputSetter?.Invoke(request);
                request.infer();
                outputGetter?.Invoke(request);
            }
            finally
            {
                Return(request);
            }
        }

        /// <summary>
        /// 执行异步推理并自动归还请求 / Run inference asynchronously and auto-return request
        /// </summary>
        /// <param name="inputSetter">设置输入的委托 / Delegate to set input tensors</param>
        /// <param name="outputGetter">获取输出的委托 / Delegate to get output tensors</param>
        /// <returns>异步任务 / Asynchronous task</returns>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// var pool = new InferRequestPool(compiledModel, initialSize: 2, maxSize: 10);
        /// 
        /// // 使用异步便捷方法执行推理 / Use async convenience method to run inference
        /// await pool.RunInferenceAsync(
        ///     request => {
        ///         // 设置输入 / Set input
        ///         request.set_input_tensor(input);
        ///     },
        ///     request => {
        ///         // 获取输出 / Get output
        ///         var output = request.get_output_tensor();
        ///         // 处理输出... / Process output...
        ///     }
        /// );
        /// // 请求自动归还 / Request is automatically returned
        /// </code>
        /// </example>
        public async System.Threading.Tasks.Task RunInferenceAsync(
            Action<InferRequest> inputSetter,
            Action<InferRequest> outputGetter)
        {
            var request = await RentAsync();
            try
            {
                inputSetter?.Invoke(request);
                request.start_async();
                await System.Threading.Tasks.Task.Run(() => request.wait());
                outputGetter?.Invoke(request);
            }
            finally
            {
                Return(request);
            }
        }

        /// <summary>
        /// 清空池并释放所有请求 / Clear pool and dispose all requests
        /// </summary>
        /// <remarks>
        /// 此方法会释放池中所有请求，但不会释放池本身。/ This method disposes all requests in the pool but does not dispose the pool itself.
        /// 调用后当前大小会归零，后续 Rent 调用会创建新的请求。/ After calling, current size becomes zero, subsequent Rent calls will create new requests.
        /// </remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// var pool = new InferRequestPool(compiledModel, initialSize: 2, maxSize: 10);
        /// 
        /// // 使用一段时间后清空池 / Clear pool after some usage
        /// pool.Clear();
        /// 
        /// // 池已清空，计数归零 / Pool cleared, count reset
        /// Console.WriteLine($"Pool count: {pool.Count}"); // 输出 0 / Output: 0
        /// </code>
        /// </example>
        public void Clear()
        {
            while (_pool.TryTake(out var request))
            {
                try
                {
                    request?.Dispose();
                }
                catch { }
                Interlocked.Decrement(ref _currentSize);
            }
        }

        /// <summary>
        /// 释放资源 / Dispose resources
        /// </summary>
        /// <remarks>
        /// 释放池中所有请求和信号量资源。/ Disposes all requests in the pool and the semaphore resource.
        /// 调用后对象池不可再使用。/ After calling, the pool cannot be used anymore.
        /// </remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// // 使用 using 语句自动释放 / Use using statement for automatic disposal
        /// using (var pool = new InferRequestPool(compiledModel, initialSize: 2, maxSize: 10)) {
        ///     // 使用池 / Use pool
        ///     var request = pool.Rent();
        ///     // ... 执行推理 / Run inference ...
        ///     pool.Return(request);
        /// } // 自动调用 Dispose / Dispose called automatically
        /// 
        /// // 或者手动释放 / Or manual disposal
        /// var pool2 = new InferRequestPool(compiledModel);
        /// try {
        ///     // 使用池 / Use pool
        /// } finally {
        ///     pool2.Dispose(); // 手动释放 / Manual disposal
        /// }
        /// </code>
        /// </example>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            Clear();
            _semaphore?.Dispose();

            OvLogger.Debug("InferRequestPool: 已释放");
            OvLogger.Debug("InferRequestPool: Disposed");
        }

        private InferRequest RentCore()
        {
            if (_pool.TryTake(out var request))
            {
                OvLogger.Debug("InferRequestPool: 从池中获取请求");
                OvLogger.Debug("InferRequestPool: Rent from pool");
                return request;
            }

            // 池为空但信号量已获取，创建新请求
            // Pool empty but semaphore acquired, create new request
            OvLogger.Debug("InferRequestPool: 创建新请求");
            OvLogger.Debug("InferRequestPool: Create new request");
            var newRequest = CreateRequest();
            if (newRequest != null)
            {
                Interlocked.Increment(ref _currentSize);
            }
            return newRequest;
        }

        private async System.Threading.Tasks.Task<InferRequest> RentAsyncCore(
            System.Threading.CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);
            return RentCore();
        }

        private InferRequest CreateRequest()
        {
            try
            {
                return _compiledModel.create_infer_request();
            }
            catch (Exception ex)
            {
                OvLogger.Error($"InferRequestPool: 创建请求失败 - {ex.Message}");
                OvLogger.Error($"InferRequestPool: Failed to create request - {ex.Message}");
                throw;
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(InferRequestPool));
        }
    }
}
