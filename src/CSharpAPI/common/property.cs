using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    /// <summary>
    ///  A header for advanced hardware specific properties for OpenVINO runtime devices.
    ///  To use in set_property, compile_model, import_model, get_property methods.
    /// </summary>
    public enum PropertyKey
    {
        // Read-only property key
        /// <summary>
        /// Read-only property<string> to get a string list of supported read-only properties.
        /// </summary>
        SUPPORTED_PROPERTIES,
        /// <summary>
        /// Read-only property<string> to get a list of available device IDs.
        /// </summary>
        AVAILABLE_DEVICES,
        /// <summary>
        /// Read-only property(uint32_t string) to get an unsigned integer value of optimaln
        /// number of compiled model infer requests.
        /// </summary>
        OPTIMAL_NUMBER_OF_INFER_REQUESTS,
        /// <summary>
        /// Read-only property<string(unsigned int, unsigned int, unsigned int) to provide a
        /// hint for a range for number of async infer requests. If device supports
        /// streams, the metric provides range for number of IRs per stream.
        /// </summary>
        RANGE_FOR_ASYNC_INFER_REQUESTS,
        /// <summary>
        /// Read-only property(string(unsigned int, unsigned int)) to provide information about a range for
        /// streams on platforms where streams are supported
        /// </summary>
        RANGE_FOR_STREAMS,
        /// <summary>
        /// Read-only property<string> to get a string value representing a full device name.
        /// </summary>
        FULL_DEVICE_NAME,
        /// <summary>
        /// Read-only property<string> to get a string list of capabilities options per device.
        /// </summary>
        OPTIMIZATION_CAPABILITIES,
        /// <summary>
        /// Read-only property<string> to get a name of name of a model
        /// </summary>
        NETWORK_NAME,
        /// <summary>
        /// Read-only property（uint32_t string) to query information optimal batch size for the given device
        /// and the network
        /// </summary>
        OPTIMAL_BATCH_SIZE,
        /// <summary>
        /// Read-only property to get maximum batch size which does not cause performance degradation due
        /// to memory swap impact.
        /// </summary>
        MAX_BATCH_SIZE,

        // Read-write property key
        /// <summary>
        /// Read-write property(string) to set/get the directory which will be used to store any data cached
        /// by plugins.
        /// </summary>
        CACHE_DIR,
        /// <summary>
        /// Read-write property<string> to select the cache mode between optimize_size and optimize_speed. 
        /// If optimize_size is selected, smaller cache files will be created. 
        /// And if optimize_speed is selected, loading time will decrease but the cache file size will increase. 
        /// </summary>
        CACHE_MODE,
        /// <summary>
        /// Read-write property(uint32_t string) to set/get the number of executor logical partitions.
        /// </summary>
        NUM_STREAMS,
        /// <summary>
        /// Read-write property to set/get the name for setting CPU affinity per thread option.
        /// </summary>
        AFFINITY,
        /// <summary>
        /// Read-write property9int32_t string） to set/get the maximum number of threads that can be used
        /// for inference tasks.
        /// </summary>
        INFERENCE_NUM_THREADS,
        /// <summary>
        /// Read-write property, it is high-level OpenVINO Performance Hints
        /// </summary>
        PERFORMANCE_HINT,
        /// <summary>
        /// Read-write property, it is high-level OpenVINO hint for using CPU pinning to bind CPU threads to processors
        /// during inference
        /// </summary>
        ENABLE_CPU_PINNING,
        /// <summary>
        /// Read-write property, it is high-level OpenVINO Hints for the type of CPU core used during inference
        /// </summary>
        SCHEDULING_CORE_TYPE,
        /// <summary>
        /// Read-write property, it is high-level OpenVINO hint for using hyper threading processors during CPU inference
        /// </summary>
        ENABLE_HYPER_THREADING,
        /// <summary>
        /// Read-write property<ov_element_type_e> to set the hint for device to use specified precision for inference.
        /// </summary>
        INFERENCE_PRECISION_HINT,
        /// <summary>
        ///  (Optional) Read-write property(uint32_t string) that backs the Performance Hints by giving
        /// additional information on how many inference requests the application will be
        /// keeping in flight usually this value comes from the actual use-case  (e.g.
        /// number of video-cameras, or other sources of inputs)
        /// </summary>
        PERFORMANCE_HINT_NUM_REQUESTS,
        /// <summary>
        /// Read-write property, high-level OpenVINO model priority hint.
        /// </summary>
        MODEL_PRIORITY,
        /// <summary>
        /// Read-write property<string> for setting desirable log level.
        /// </summary>
        LOG_LEVEL,
        /// <summary>
        /// Read-write property(string) for setting performance counters option.
        /// </summary>
        PERF_COUNT,
        /// <summary>
        /// Read-write property(std::pair(std::string, Any)), device Priorities config option,
        /// with comma-separated devices listed in the desired priority
        /// </summary>
        MULTI_DEVICE_PRIORITIES,
        /// <summary>
        /// Read-write property(string) for high-level OpenVINO Execution hint
        /// unlike low-level properties that are individual (per-device), the hints are something that every device accepts
        /// and turns into device-specific settings
        /// Execution mode hint controls preferred optimization targets (performance or accuracy) for given model
        /// </summary>
        EXECUTION_MODE_HINT,
        /// <summary>
        /// Read-write property to set whether force terminate tbb when ov core destruction
        /// </summary>
        FORCE_TBB_TERMINATE,
        /// <summary>
        /// Read-write property to configure `mmap()` use for model read
        /// </summary>
        ENABLE_MMAP,
        /// <summary>
        /// Read-write property
        /// </summary>
        AUTO_BATCH_TIMEOUT,
    }

    public static partial class Ov
    {
        /// <summary>
        /// Get the read-write property(string) to set/get the directory which will be used to store any data cached by plugins.
        /// </summary>
        /// <param name="dir">
        /// The read-write property(string) to set/get the directory which will be used to store any data cached by plugins.
        /// </param>
        /// <returns>The pair data.</returns>
        public static KeyValuePair<string, string> cache_dir(string dir)
        {
            return new KeyValuePair<string, string>(PropertyKey.CACHE_DIR.ToString(), dir);
        }
    }
}
