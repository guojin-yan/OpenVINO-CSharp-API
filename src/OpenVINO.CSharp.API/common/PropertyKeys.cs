// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

namespace OpenVinoSharp
{
    /// <summary>
    /// OpenVINO设备配置预定义属性键 / Predefined property keys for OpenVINO device configuration
    /// </summary>
    public static class PropertyKeys
    {
        #region 只读属性 / Read-Only Properties

        /// <summary>
        /// 获取支持的只读属性字符串列表 / Read-only property to get a string list of supported read-only properties
        /// </summary>
        public const string SupportedProperties = "SUPPORTED_PROPERTIES";

        /// <summary>
        /// 获取可用设备ID列表 / Read-only property to get a list of available device IDs
        /// </summary>
        public const string AvailableDevices = "AVAILABLE_DEVICES";

        /// <summary>
        /// 获取编译模型推理请求的最佳数量 / Read-only property to get optimal number of compiled model infer requests
        /// </summary>
        public const string OptimalNumberOfInferRequests = "OPTIMAL_NUMBER_OF_INFER_REQUESTS";

        /// <summary>
        /// 提供异步推理请求数量的范围提示 / Read-only property to provide a hint for a range for number of async infer requests
        /// </summary>
        public const string RangeForAsyncInferRequests = "RANGE_FOR_ASYNC_INFER_REQUESTS";

        /// <summary>
        /// 提供流数量的范围信息 / Read-only property to provide information about a range for streams
        /// </summary>
        public const string RangeForStreams = "RANGE_FOR_STREAMS";

        /// <summary>
        /// 获取完整设备名称的字符串值 / Read-only property to get a string value representing a full device name
        /// </summary>
        public const string DeviceFullName = "DEVICE_FULL_NAME";

        /// <summary>
        /// 获取每设备能力选项字符串列表 / Read-only property to get a string list of capabilities options per device
        /// </summary>
        public const string DeviceCapabilities = "DEVICE_CAPABILITIES";

        /// <summary>
        /// 获取模型名称 / Read-only property to get a name of model
        /// </summary>
        public const string ModelName = "MODEL_NAME";

        /// <summary>
        /// 查询最佳批处理大小信息 / Read-only property to query information optimal batch size
        /// </summary>
        public const string OptimalBatchSize = "OPTIMAL_BATCH_SIZE";

        /// <summary>
        /// 获取最大批处理大小 / Read-only property to get maximum batch size
        /// </summary>
        public const string MaxBatchSize = "MAX_BATCH_SIZE";

        #endregion

        #region 读写属性 / Read-Write Properties

        /// <summary>
        /// 设置/获取缓存目录 / Read-write property to set/get the directory for cache
        /// </summary>
        public const string CacheDir = "CACHE_DIR";

        /// <summary>
        /// 选择缓存模式 / Read-write property to select the cache mode
        /// </summary>
        public const string CacheMode = "CACHE_MODE";

        /// <summary>
        /// 设置执行器逻辑分区数量 / Read-write property to set the number of executor logical partitions
        /// </summary>
        public const string NumStreams = "NUM_STREAMS";

        /// <summary>
        /// 设置/获取最大线程数 / Read-write property to set/get the maximum number of threads
        /// </summary>
        public const string InferenceNumThreads = "INFERENCE_NUM_THREADS";

        /// <summary>
        /// 使用CPU固定（亲和性）/ Read-write property for using CPU pinning
        /// </summary>
        public const string HintEnableCpuPinning = "ENABLE_CPU_PINNING";

        /// <summary>
        /// 使用超线程处理器 / Read-write property for using hyper threading processors
        /// </summary>
        public const string HintEnableHyperThreading = "ENABLE_HYPER_THREADING";

        /// <summary>
        /// 性能提示 / Read-write property for Performance Hints
        /// </summary>
        public const string HintPerformanceMode = "PERFORMANCE_HINT";

        /// <summary>
        /// 调度核心类型 / Read-write property for scheduling core type
        /// </summary>
        public const string HintSchedulingCoreType = "SCHEDULING_CORE_TYPE";

        /// <summary>
        /// 设置推理精度提示 / Read-write property to set hint for inference precision
        /// </summary>
        public const string HintInferencePrecision = "INFERENCE_PRECISION_HINT";

        /// <summary>
        /// 请求数量提示 / Read-write property for number of requests hint
        /// </summary>
        public const string HintNumRequests = "PERFORMANCE_HINT_NUM_REQUESTS";

        /// <summary>
        /// 设置日志级别 / Read-write property for setting log level
        /// </summary>
        public const string LogLevel = "LOG_LEVEL";

        /// <summary>
        /// 模型优先级提示 / Read-write property for model priority hint
        /// </summary>
        public const string HintModelPriority = "MODEL_PRIORITY";

        /// <summary>
        /// 启用性能分析 / Read-write property for enabling profiling
        /// </summary>
        public const string EnableProfiling = "PERF_COUNT";

        /// <summary>
        /// 设备优先级 / Read-write property for device priorities
        /// </summary>
        public const string DevicePriorities = "DEVICE_PRIORITIES";

        /// <summary>
        /// 执行模式提示 / Read-write property for execution mode hint
        /// </summary>
        public const string HintExecutionMode = "EXECUTION_MODE_HINT";

        /// <summary>
        /// 强制终止TBB / Read-write property to force terminate tbb
        /// </summary>
        public const string ForceTbbTerminate = "FORCE_TBB_TERMINATE";

        /// <summary>
        /// 启用内存映射 / Read-write property to enable mmap
        /// </summary>
        public const string EnableMmap = "ENABLE_MMAP";

        /// <summary>
        /// 自动批处理超时 / Read-write property for auto batch timeout
        /// </summary>
        public const string AutoBatchTimeout = "AUTO_BATCH_TIMEOUT";

        /// <summary>
        /// GPU配置文件 / Read-write property for GPU config file
        /// </summary>
        public const string IntelGpuConfigFile = "CONFIG_FILE";

        #endregion
    }
}
