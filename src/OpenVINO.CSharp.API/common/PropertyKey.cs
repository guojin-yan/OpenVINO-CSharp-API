// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

namespace OpenVinoSharp
{
    /// <summary>
    /// Property keys for device configuration
    /// </summary>
    public enum PropertyKey
    {
        /// <summary>
        /// Read-write property to set/get the number of threads used by CPU plugin
        /// </summary>
        CPU_THREADS_NUM,
        /// <summary>
        /// Read-write property to set/get the bind thread mode
        /// </summary>
        CPU_BIND_THREAD,
        /// <summary>
        /// Read-write property to set/get the number of inference requests
        /// </summary>
        CPU_THROUGHPUT_STREAMS,
        /// <summary>
        /// Read-only property to get the device name
        /// </summary>
        DEVICE_ID,
        /// <summary>
        /// Read-only property to get the supported properties
        /// </summary>
        SUPPORTED_PROPERTIES,
        /// <summary>
        /// Read-only property to get the available devices
        /// </summary>
        AVAILABLE_DEVICES,
        /// <summary>
        /// Read-only property to get the device full name
        /// </summary>
        DEVICE_FULL_NAME,
        /// <summary>
        /// Read-write property to set/get the cache directory
        /// </summary>
        CACHE_DIR,
        /// <summary>
        /// Read-write property to enable/disable model caching
        /// </summary>
        CACHE_ENABLE,
        /// <summary>
        /// Read-only property to get the optimal number of inference requests
        /// </summary>
        OPTIMAL_NUMBER_OF_INFER_REQUESTS,
        /// <summary>
        /// Read-only property to get the maximum number of batches
        /// </summary>
        MAX_BATCH_SIZE,
        /// <summary>
        /// Read-only property to get the range of batches
        /// </summary>
        BATCH_PROPERTY,
        /// <summary>
        /// Read-only property to get the metric keys
        /// </summary>
        METRIC_KEYS,
        /// <summary>
        /// Read-only property to get the configuration keys
        /// </summary>
        CONFIGURATION_KEYS,
    }
}
