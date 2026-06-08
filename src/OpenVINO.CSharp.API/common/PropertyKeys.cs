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
