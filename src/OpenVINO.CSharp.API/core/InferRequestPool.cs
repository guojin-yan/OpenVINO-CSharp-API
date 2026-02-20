// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

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
        /// 创建推理请求池
        /// </summary>
        /// <param name="compiledModel">编译后的模型</param>
        /// <param name="initialSize">初始池大小</param>
        /// <param name="maxSize">最大池大小</param>
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
            for (int i = 0; i < initialSize; i++)
            {
                var request = CreateRequest();
                if (request != null)
                {
                    _pool.Add(request);
                    Interlocked.Increment(ref _currentSize);
                }
            }

            Logger.Debug($"InferRequestPool: 创建完成，初始大小: {initialSize}, 最大大小: {maxSize}");
        }

        /// <summary>
        /// 当前池大小
        /// </summary>
        public int Count => _currentSize;

        /// <summary>
        /// 可用请求数量
        /// </summary>
        public int AvailableCount => _pool.Count;

        /// <summary>
        /// 从池中获取推理请求（阻塞直到可用）
        /// </summary>
        /// <returns>推理请求对象</returns>
        public InferRequest Rent()
        {
            ThrowIfDisposed();
            _semaphore.Wait();
            return RentCore();
        }

        /// <summary>
        /// 异步获取推理请求
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>推理请求对象</returns>
        public System.Threading.Tasks.Task<InferRequest> RentAsync(
            System.Threading.CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            return RentAsyncCore(cancellationToken);
        }

        /// <summary>
        /// 尝试获取推理请求（非阻塞）
        /// </summary>
        /// <param name="request">获取到的请求</param>
        /// <returns>是否成功获取</returns>
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
        /// 归还推理请求到池中
        /// </summary>
        /// <param name="request">推理请求对象</param>
        public void Return(InferRequest request)
        {
            if (request == null || _disposed)
                return;

            // 检查请求是否有效
            if (request.IsDisposed)
            {
                // 如果请求已被释放，减少计数并创建新的
                Interlocked.Decrement(ref _currentSize);
                _semaphore.Release();
                return;
            }

            // 重置请求状态（取消任何待处理的推理）
            try
            {
                request.cancel();
            }
            catch { }

            _pool.Add(request);
            _semaphore.Release();
        }

        /// <summary>
        /// 执行推理并自动归还请求（便捷方法）
        /// </summary>
        /// <param name="inputSetter">设置输入的委托</param>
        /// <param name="outputGetter">获取输出的委托</param>
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
        /// 执行异步推理并自动归还请求
        /// </summary>
        /// <param name="inputSetter">设置输入的委托</param>
        /// <param name="outputGetter">获取输出的委托</param>
        /// <returns>异步任务</returns>
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
        /// 清空池并释放所有请求
        /// </summary>
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
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            Clear();
            _semaphore?.Dispose();

            Logger.Debug("InferRequestPool: 已释放");
        }

        private InferRequest RentCore()
        {
            if (_pool.TryTake(out var request))
            {
                Logger.Debug("InferRequestPool: 从池中获取请求");
                return request;
            }

            // 池为空但信号量已获取，创建新请求
            Logger.Debug("InferRequestPool: 创建新请求");
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
                Logger.Error($"InferRequestPool: 创建请求失败 - {ex.Message}");
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
