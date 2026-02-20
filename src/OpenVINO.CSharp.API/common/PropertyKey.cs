// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

namespace OpenVinoSharp
{
    /// <summary>
    /// 设备配置属性键 / Property keys for device configuration
    /// </summary>
    public enum PropertyKey
    {
        /// <summary>
        /// 读写属性，用于设置/获取CPU插件使用的线程数 / Read-write property to set/get the number of threads used by CPU plugin
        /// </summary>
        CPU_THREADS_NUM,
        /// <summary>
        /// 读写属性，用于设置/获取线程绑定模式 / Read-write property to set/get the bind thread mode
        /// </summary>
        CPU_BIND_THREAD,
        /// <summary>
        /// 读写属性，用于设置/获取推理请求流数量 / Read-write property to set/get the number of inference requests
        /// </summary>
        CPU_THROUGHPUT_STREAMS,
        /// <summary>
        /// 只读属性，用于获取设备名称 / Read-only property to get the device name
        /// </summary>
        DEVICE_ID,
        /// <summary>
        /// 只读属性，用于获取支持的属性列表 / Read-only property to get the supported properties
        /// </summary>
        SUPPORTED_PROPERTIES,
        /// <summary>
        /// 只读属性，用于获取可用设备列表 / Read-only property to get the available devices
        /// </summary>
        AVAILABLE_DEVICES,
        /// <summary>
        /// 只读属性，用于获取设备全名 / Read-only property to get the device full name
        /// </summary>
        DEVICE_FULL_NAME,
        /// <summary>
        /// 读写属性，用于设置/获取缓存目录 / Read-write property to set/get the cache directory
        /// </summary>
        CACHE_DIR,
        /// <summary>
        /// 读写属性，用于启用/禁用模型缓存 / Read-write property to enable/disable model caching
        /// </summary>
        CACHE_ENABLE,
        /// <summary>
        /// 只读属性，用于获取最优推理请求数量 / Read-only property to get the optimal number of inference requests
        /// </summary>
        OPTIMAL_NUMBER_OF_INFER_REQUESTS,
        /// <summary>
        /// 只读属性，用于获取最大批处理大小 / Read-only property to get the maximum number of batches
        /// </summary>
        MAX_BATCH_SIZE,
        /// <summary>
        /// 只读属性，用于获取批处理范围 / Read-only property to get the range of batches
        /// </summary>
        BATCH_PROPERTY,
        /// <summary>
        /// 只读属性，用于获取指标键列表 / Read-only property to get the metric keys
        /// </summary>
        METRIC_KEYS,
        /// <summary>
        /// 只读属性，用于获取配置键列表 / Read-only property to get the configuration keys
        /// </summary>
        CONFIGURATION_KEYS,
    }
}
