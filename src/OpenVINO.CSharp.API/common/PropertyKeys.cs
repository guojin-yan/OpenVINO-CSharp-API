// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

namespace OpenVinoSharp
{
    /// <summary>
    /// OpenVINO设备配置预定义属性键 / Predefined property keys for OpenVINO device configuration
    /// </summary>
    public static class PropertyKeys
    {
        #region 只读属性 / Read-Only Properties

        /// <summary>
        /// 支持的属性列表 / List of supported properties
        /// </summary>
        public const string SupportedProperties = "SUPPORTED_PROPERTIES";

        /// <summary>
        /// 可用设备列表 / List of available devices
        /// </summary>
        public const string AvailableDevices = "AVAILABLE_DEVICES";

        /// <summary>
        /// 最优推理请求数量 / Optimal number of inference requests
        /// </summary>
        public const string OptimalNumberOfInferRequests = "OPTIMAL_NUMBER_OF_INFER_REQUESTS";

        /// <summary>
        /// 异步推理请求数量范围 / Range for async inference requests
        /// </summary>
        public const string RangeForAsyncInferRequests = "RANGE_FOR_ASYNC_INFER_REQUESTS";

        /// <summary>
        /// 流数量范围 / Range for streams
        /// </summary>
        public const string RangeForStreams = "RANGE_FOR_STREAMS";

        /// <summary>
        /// 设备完整名称 / Device full name
        /// </summary>
        public const string DeviceFullName = "DEVICE_FULL_NAME";

        /// <summary>
        /// 设备能力列表 / List of device capabilities
        /// </summary>
        public const string DeviceCapabilities = "DEVICE_CAPABILITIES";

        /// <summary>
        /// 模型名称 / Model name
        /// </summary>
        public const string ModelName = "MODEL_NAME";

        /// <summary>
        /// 最优批处理大小 / Optimal batch size
        /// </summary>
        public const string OptimalBatchSize = "OPTIMAL_BATCH_SIZE";

        /// <summary>
        /// 最大批处理大小 / Maximum batch size
        /// </summary>
        public const string MaxBatchSize = "MAX_BATCH_SIZE";

        #endregion

        #region 读写属性 / Read-Write Properties

        /// <summary>
        /// 缓存目录 / Cache directory
        /// </summary>
        public const string CacheDir = "CACHE_DIR";

        /// <summary>
        /// 缓存模式 / Cache mode
        /// </summary>
        public const string CacheMode = "CACHE_MODE";

        /// <summary>
        /// 流数量 / Number of streams
        /// </summary>
        public const string NumStreams = "NUM_STREAMS";

        /// <summary>
        /// 推理线程数 / Number of inference threads
        /// </summary>
        public const string InferenceNumThreads = "INFERENCE_NUM_THREADS";

        /// <summary>
        /// 启用CPU亲和性 / Enable CPU pinning
        /// </summary>
        public const string HintEnableCpuPinning = "ENABLE_CPU_PINNING";

        /// <summary>
        /// 启用超线程 / Enable hyper threading
        /// </summary>
        public const string HintEnableHyperThreading = "ENABLE_HYPER_THREADING";

        /// <summary>
        /// 性能模式 / Performance mode
        /// </summary>
        public const string HintPerformanceMode = "PERFORMANCE_HINT";

        /// <summary>
        /// 调度核心类型 / Scheduling core type
        /// </summary>
        public const string HintSchedulingCoreType = "SCHEDULING_CORE_TYPE";

        /// <summary>
        /// 推理精度 / Inference precision
        /// </summary>
        public const string HintInferencePrecision = "INFERENCE_PRECISION_HINT";

        /// <summary>
        /// 性能提示请求数量 / Performance hint number of requests
        /// </summary>
        public const string HintNumRequests = "PERFORMANCE_HINT_NUM_REQUESTS";

        /// <summary>
        /// 日志级别 / Log level
        /// </summary>
        public const string LogLevel = "LOG_LEVEL";

        /// <summary>
        /// 模型优先级 / Model priority
        /// </summary>
        public const string HintModelPriority = "MODEL_PRIORITY";

        /// <summary>
        /// 启用性能计数 / Enable performance count
        /// </summary>
        public const string EnableProfiling = "PERF_COUNT";

        /// <summary>
        /// 设备优先级 / Device priorities
        /// </summary>
        public const string DevicePriorities = "DEVICE_PRIORITIES";

        /// <summary>
        /// 执行模式 / Execution mode
        /// </summary>
        public const string HintExecutionMode = "EXECUTION_MODE_HINT";

        /// <summary>
        /// 强制终止TBB / Force TBB terminate
        /// </summary>
        public const string ForceTbbTerminate = "FORCE_TBB_TERMINATE";

        /// <summary>
        /// 启用内存映射 / Enable mmap
        /// </summary>
        public const string EnableMmap = "ENABLE_MMAP";

        /// <summary>
        /// 自动批处理超时 / Auto batch timeout
        /// </summary>
        public const string AutoBatchTimeout = "AUTO_BATCH_TIMEOUT";

        /// <summary>
        /// GPU配置文件 / GPU config file
        /// </summary>
        public const string IntelGpuConfigFile = "CONFIG_FILE";

        #endregion
    }
}
