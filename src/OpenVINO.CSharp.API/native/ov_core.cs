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
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        #region Version

        /// <summary>
        /// 获取 OpenVINO 版本信息 / Get version of OpenVINO
        /// </summary>
        /// <param name="version">返回的版本信息指针 / Returned version info pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_get_openvino_version",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_get_openvino_version(IntPtr version);

        /// <summary>
        /// 释放 ov_version_t 分配的内存 / Release the memory allocated by ov_version_t
        /// </summary>
        /// <param name="version">版本信息指针 / Version info pointer</param>
        [DllImport("openvino_c", EntryPoint = "ov_version_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_version_free(IntPtr version);

        #endregion

        #region Log Callback

        /// <summary>
        /// 日志消息回调函数类型（原始 C API 名称）/ Callback function type for logging messages (original C API name)
        /// </summary>
        /// <param name="message">日志消息指针 / Log message pointer</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ov_util_log_callback_func(IntPtr message);

        /// <summary>
        /// 日志消息回调函数类型（C# 友好别名）/ Callback function type for logging messages (C# friendly alias)
        /// </summary>
        /// <param name="message">日志消息字符串 / Log message string</param>
        public delegate void LogCallbackDelegate(string message);

        /// <summary>
        /// 设置用户日志消息处理回调 / Sets user log message handling callback
        /// </summary>
        /// <param name="func">回调函数 / Callback function</param>
        [DllImport("openvino_c", EntryPoint = "ov_util_set_log_callback",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ov_util_set_log_callback(ov_util_log_callback_func func);

        /// <summary>
        /// 设置用户日志消息处理回调（C# 友好重载）/ Sets user log message handling callback (C# friendly overload)
        /// </summary>
        /// <param name="func">回调委托 / Callback delegate</param>
        public static void ov_util_set_log_callback(LogCallbackDelegate func)
        {
            // Create a wrapper that marshals string from IntPtr
            ov_util_log_callback_func wrapper = (IntPtr msgPtr) => {
                string message = OpenVinoSharp.StringUtils.Utf8PtrToString(msgPtr) ?? string.Empty;
                func(message);
            };
            ov_util_set_log_callback(wrapper);
        }

        /// <summary>
        /// 重置日志消息处理回调为默认值（标准输出）/ Resets log message handling callback to its default (standard output)
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_util_reset_log_callback",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ov_util_reset_log_callback();

        #endregion

        #region Core Creation and Destruction

        /// <summary>
        /// 默认方式构造 OpenVINO Core 实例 / Constructs OpenVINO Core instance by default
        /// </summary>
        /// <param name="core">返回的 Core 指针 / Returned Core pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        /// <remarks>
        /// 创建一个新的 OpenVINO Core 实例，用于设备管理和模型编译。
        /// Creates a new OpenVINO Core instance for device management and model compilation.
        /// </remarks>
        [DllImport("openvino_c", EntryPoint = "ov_core_create",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_create(ref IntPtr core);

        /// <summary>
        /// 使用 XML 配置文件构造 OpenVINO Core 实例 / Constructs OpenVINO Core instance using XML configuration file
        /// </summary>
        /// <param name="xml_config_file">XML 配置文件路径 / XML configuration file path</param>
        /// <param name="core">返回的 Core 指针 / Returned Core pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_create_with_config",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_create_with_config(
            [MarshalAs(UnmanagedType.LPStr)] string xml_config_file,
            ref IntPtr core);

        /// <summary>
        /// 使用 UTF-8 XML 配置文件路径构造 OpenVINO Core 实例 / Constructs OpenVINO Core with an UTF-8 XML configuration path
        /// </summary>
        /// <param name="xml_config_file">UTF-8 配置文件路径指针 / UTF-8 configuration file path pointer</param>
        /// <param name="core">返回的 Core 指针 / Returned Core pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_create_with_config",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_core_create_with_config_utf8(
            IntPtr xml_config_file,
            ref IntPtr core);

        /// <summary>
        /// 使用 Windows Unicode 配置文件路径构造 OpenVINO Core 实例 / Constructs OpenVINO Core with a Windows Unicode configuration path.
        /// </summary>
        /// <remarks>
        /// 该函数只在启用 OPENVINO_ENABLE_UNICODE_PATH_SUPPORT 的 OpenVINO C runtime 中导出。
        /// This entry point is exported only by OpenVINO C runtimes built with OPENVINO_ENABLE_UNICODE_PATH_SUPPORT.
        /// </remarks>
        [DllImport("openvino_c", EntryPoint = "ov_core_create_with_config_unicode",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_core_create_with_config_unicode(
            [MarshalAs(UnmanagedType.LPWStr)] string xml_config_file_ws,
            ref IntPtr core);

        /// <summary>
        /// 释放 ov_core_t 分配的内存 / Release the memory allocated by ov_core_t
        /// </summary>
        /// <param name="core">Core 指针 / Core pointer</param>
        [DllImport("openvino_c", EntryPoint = "ov_core_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_core_free(IntPtr core);

        /// <summary>
        /// 关闭 OpenVINO / Shut down the OpenVINO
        /// </summary>
        /// <remarks>
        /// 释放 OpenVINO 使用的所有资源。
        /// Releases all resources used by OpenVINO.
        /// </remarks>
        [DllImport("openvino_c", EntryPoint = "ov_shutdown",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_shutdown();

        #endregion

        #region Read Model

        /// <summary>
        /// 从 IR / ONNX / PDPD / TF / TFLite 格式读取模型 / Reads models from IR / ONNX / PDPD / TF / TFLite formats
        /// </summary>
        /// <param name="core">Core 指针 / Core pointer</param>
        /// <param name="model_path">模型文件路径 / Model file path</param>
        /// <param name="bin_path">权重文件路径（可为 null）/ Weights file path (can be null)</param>
        /// <param name="model">返回的模型指针 / Returned model pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_read_model",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_read_model(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string model_path,
            [MarshalAs(UnmanagedType.LPStr)] string bin_path,
            ref IntPtr model);

        /// <summary>
        /// 从 UTF-8 路径读取模型 / Reads a model from UTF-8 encoded paths
        /// </summary>
        /// <param name="core">Core 指针 / Core pointer</param>
        /// <param name="model_path">UTF-8 模型路径指针 / UTF-8 model path pointer</param>
        /// <param name="bin_path">UTF-8 权重路径指针 / UTF-8 weights path pointer</param>
        /// <param name="model">返回的模型指针 / Returned model pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_read_model",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_core_read_model_utf8(
            IntPtr core,
            IntPtr model_path,
            IntPtr bin_path,
            ref IntPtr model);

        /// <summary>
        /// 使用 Windows Unicode 路径读取模型 / Reads a model from Windows Unicode paths.
        /// </summary>
        /// <remarks>
        /// 返回的 model 为 owned pointer，调用方必须用 ov_model_free 释放。
        /// The returned model is an owned pointer and must be released with ov_model_free by the managed wrapper.
        /// </remarks>
        [DllImport("openvino_c", EntryPoint = "ov_core_read_model_unicode",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_core_read_model_unicode(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPWStr)] string model_path,
            [MarshalAs(UnmanagedType.LPWStr)] string bin_path,
            ref IntPtr model);

        /// <summary>
        /// 从内存缓冲区读取模型 / Reads models from memory buffer
        /// </summary>
        /// <param name="core">Core 指针 / Core pointer</param>
        /// <param name="model_str">模型数据缓冲区 / Model data buffer</param>
        /// <param name="str_len">模型数据长度 / Model data length</param>
        /// <param name="weights">权重数据指针（可为 IntPtr.Zero）/ Weights data pointer (can be IntPtr.Zero)</param>
        /// <param name="model">返回的模型指针 / Returned model pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_read_model_from_memory_buffer",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_read_model_from_memory_buffer(
            IntPtr core,
            ref byte model_str,
            ulong str_len,
            IntPtr weights,
            ref IntPtr model);

        /// <summary>
        /// 从内存缓冲区读取模型，长度使用本机 size_t / Reads a model from memory buffer using native size_t length
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_read_model_from_memory_buffer",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_core_read_model_from_memory_buffer_native_size(
            IntPtr core,
            ref byte model_str,
            UIntPtr str_len,
            IntPtr weights,
            ref IntPtr model);

        #endregion

        #region Compile Model

        /// <summary>
        /// 从源模型对象创建编译模型 / Creates a compiled model from a source model object
        /// </summary>
        /// <param name="core">Core 指针 / Core pointer</param>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <param name="device_name">目标设备名称（如 "CPU", "GPU"）/ Target device name (e.g., "CPU", "GPU")</param>
        /// <param name="property_args_size">属性参数数量 / Property arguments size</param>
        /// <param name="compiled_model">返回的编译模型指针 / Returned compiled model pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model(
            IntPtr core,
            IntPtr model,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            ulong property_args_size,
            ref IntPtr compiled_model);

        /// <summary>
        /// 使用 UTF-8 设备名编译模型 / Compiles a model with an UTF-8 encoded device name
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_core_compile_model_utf8(
            IntPtr core,
            IntPtr model,
            IntPtr device_name,
            UIntPtr property_args_size,
            ref IntPtr compiled_model);

        /// <summary>
        /// 从源模型对象创建编译模型（带 1 个属性对）/ Creates a compiled model from a source model object with 1 property pair
        /// </summary>
        /// <param name="core">Core 指针 / Core pointer</param>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <param name="device_name">目标设备名称 / Target device name</param>
        /// <param name="property_args_size">属性参数数量 / Property arguments size</param>
        /// <param name="compiled_model">返回的编译模型指针 / Returned compiled model pointer</param>
        /// <param name="key1">属性键 1 / Property key 1</param>
        /// <param name="value1">属性值 1 / Property value 1</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model(
            IntPtr core,
            IntPtr model,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            ulong property_args_size,
            ref IntPtr compiled_model,
            IntPtr key1,
            IntPtr value1);

        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_core_compile_model_utf8(
            IntPtr core,
            IntPtr model,
            IntPtr device_name,
            UIntPtr property_args_size,
            ref IntPtr compiled_model,
            IntPtr key1,
            IntPtr value1);

        /// <summary>
        /// 从源模型对象创建编译模型（带 2 个属性对）/ Creates a compiled model from a source model object with 2 property pairs
        /// </summary>
        /// <param name="core">Core 指针 / Core pointer</param>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <param name="device_name">目标设备名称 / Target device name</param>
        /// <param name="property_args_size">属性参数数量 / Property arguments size</param>
        /// <param name="compiled_model">返回的编译模型指针 / Returned compiled model pointer</param>
        /// <param name="key1">属性键 1 / Property key 1</param>
        /// <param name="value1">属性值 1 / Property value 1</param>
        /// <param name="key2">属性键 2 / Property key 2</param>
        /// <param name="value2">属性值 2 / Property value 2</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model(
            IntPtr core,
            IntPtr model,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            ulong property_args_size,
            ref IntPtr compiled_model,
            IntPtr key1,
            IntPtr value1,
            IntPtr key2,
            IntPtr value2);

        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_core_compile_model_utf8(
            IntPtr core,
            IntPtr model,
            IntPtr device_name,
            UIntPtr property_args_size,
            ref IntPtr compiled_model,
            IntPtr key1,
            IntPtr value1,
            IntPtr key2,
            IntPtr value2);

        /// <summary>
        /// 从源模型对象创建编译模型（带 3 个属性对）/ Creates a compiled model from a source model object with 3 property pairs
        /// </summary>
        /// <param name="core">Core 指针 / Core pointer</param>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <param name="device_name">目标设备名称 / Target device name</param>
        /// <param name="property_args_size">属性参数数量 / Property arguments size</param>
        /// <param name="compiled_model">返回的编译模型指针 / Returned compiled model pointer</param>
        /// <param name="key1">属性键 1 / Property key 1</param>
        /// <param name="value1">属性值 1 / Property value 1</param>
        /// <param name="key2">属性键 2 / Property key 2</param>
        /// <param name="value2">属性值 2 / Property value 2</param>
        /// <param name="key3">属性键 3 / Property key 3</param>
        /// <param name="value3">属性值 3 / Property value 3</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model(
            IntPtr core,
            IntPtr model,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            ulong property_args_size,
            ref IntPtr compiled_model,
            IntPtr key1,
            IntPtr value1,
            IntPtr key2,
            IntPtr value2,
            IntPtr key3,
            IntPtr value3);

        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_core_compile_model_utf8(
            IntPtr core,
            IntPtr model,
            IntPtr device_name,
            UIntPtr property_args_size,
            ref IntPtr compiled_model,
            IntPtr key1,
            IntPtr value1,
            IntPtr key2,
            IntPtr value2,
            IntPtr key3,
            IntPtr value3);

        /// <summary>
        /// 从 IR/ONNX/PDPD 文件读取并创建编译模型 / Reads a model and creates a compiled model from the IR/ONNX/PDPD file
        /// </summary>
        /// <param name="core">Core 指针 / Core pointer</param>
        /// <param name="model_path">模型文件路径 / Model file path</param>
        /// <param name="device_name">目标设备名称 / Target device name</param>
        /// <param name="property_args_size">属性参数数量 / Property arguments size</param>
        /// <param name="compiled_model">返回的编译模型指针 / Returned compiled model pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model_from_file",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model_from_file(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string model_path,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            ulong property_args_size,
            ref IntPtr compiled_model);

        /// <summary>
        /// 使用 UTF-8 模型路径和设备名从文件编译模型 / Compiles a model from UTF-8 model path and device name
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model_from_file",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_core_compile_model_from_file_utf8(
            IntPtr core,
            IntPtr model_path,
            IntPtr device_name,
            UIntPtr property_args_size,
            ref IntPtr compiled_model);

        /// <summary>
        /// 使用 Windows Unicode 模型路径从文件编译模型 / Compiles a model from a Windows Unicode model path.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model_from_file_unicode",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_core_compile_model_from_file_unicode(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPWStr)] string model_path,
            IntPtr device_name,
            UIntPtr property_args_size,
            ref IntPtr compiled_model);

        /// <summary>
        /// 从文件读取并创建编译模型（带 1 个属性对）/ Reads a model and creates a compiled model from file with 1 property pair
        /// </summary>
        /// <param name="core">Core 指针 / Core pointer</param>
        /// <param name="model_path">模型文件路径 / Model file path</param>
        /// <param name="device_name">目标设备名称 / Target device name</param>
        /// <param name="property_args_size">属性参数数量 / Property arguments size</param>
        /// <param name="compiled_model">返回的编译模型指针 / Returned compiled model pointer</param>
        /// <param name="key1">属性键 1 / Property key 1</param>
        /// <param name="value1">属性值 1 / Property value 1</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model_from_file",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model_from_file(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string model_path,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            ulong property_args_size,
            ref IntPtr compiled_model,
            IntPtr key1,
            IntPtr value1);

        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model_from_file",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_core_compile_model_from_file_utf8(
            IntPtr core,
            IntPtr model_path,
            IntPtr device_name,
            UIntPtr property_args_size,
            ref IntPtr compiled_model,
            IntPtr key1,
            IntPtr value1);

        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model_from_file_unicode",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_core_compile_model_from_file_unicode(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPWStr)] string model_path,
            IntPtr device_name,
            UIntPtr property_args_size,
            ref IntPtr compiled_model,
            IntPtr key1,
            IntPtr value1);

        /// <summary>
        /// 从文件读取并创建编译模型（带 2 个属性对）/ Reads a model and creates a compiled model from file with 2 property pairs
        /// </summary>
        /// <param name="core">Core 指针 / Core pointer</param>
        /// <param name="model_path">模型文件路径 / Model file path</param>
        /// <param name="device_name">目标设备名称 / Target device name</param>
        /// <param name="property_args_size">属性参数数量 / Property arguments size</param>
        /// <param name="compiled_model">返回的编译模型指针 / Returned compiled model pointer</param>
        /// <param name="key1">属性键 1 / Property key 1</param>
        /// <param name="value1">属性值 1 / Property value 1</param>
        /// <param name="key2">属性键 2 / Property key 2</param>
        /// <param name="value2">属性值 2 / Property value 2</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model_from_file",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model_from_file(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string model_path,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            ulong property_args_size,
            ref IntPtr compiled_model,
            IntPtr key1,
            IntPtr value1,
            IntPtr key2,
            IntPtr value2);

        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model_from_file",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_core_compile_model_from_file_utf8(
            IntPtr core,
            IntPtr model_path,
            IntPtr device_name,
            UIntPtr property_args_size,
            ref IntPtr compiled_model,
            IntPtr key1,
            IntPtr value1,
            IntPtr key2,
            IntPtr value2);

        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model_from_file_unicode",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_core_compile_model_from_file_unicode(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPWStr)] string model_path,
            IntPtr device_name,
            UIntPtr property_args_size,
            ref IntPtr compiled_model,
            IntPtr key1,
            IntPtr value1,
            IntPtr key2,
            IntPtr value2);

        /// <summary>
        /// 从文件读取并创建编译模型（带 3 个属性对）/ Reads a model and creates a compiled model from file with 3 property pairs
        /// </summary>
        /// <param name="core">Core 指针 / Core pointer</param>
        /// <param name="model_path">模型文件路径 / Model file path</param>
        /// <param name="device_name">目标设备名称 / Target device name</param>
        /// <param name="property_args_size">属性参数数量 / Property arguments size</param>
        /// <param name="compiled_model">返回的编译模型指针 / Returned compiled model pointer</param>
        /// <param name="key1">属性键 1 / Property key 1</param>
        /// <param name="value1">属性值 1 / Property value 1</param>
        /// <param name="key2">属性键 2 / Property key 2</param>
        /// <param name="value2">属性值 2 / Property value 2</param>
        /// <param name="key3">属性键 3 / Property key 3</param>
        /// <param name="value3">属性值 3 / Property value 3</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model_from_file",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model_from_file(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string model_path,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            ulong property_args_size,
            ref IntPtr compiled_model,
            IntPtr key1,
            IntPtr value1,
            IntPtr key2,
            IntPtr value2,
            IntPtr key3,
            IntPtr value3);

        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model_from_file",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_core_compile_model_from_file_utf8(
            IntPtr core,
            IntPtr model_path,
            IntPtr device_name,
            UIntPtr property_args_size,
            ref IntPtr compiled_model,
            IntPtr key1,
            IntPtr value1,
            IntPtr key2,
            IntPtr value2,
            IntPtr key3,
            IntPtr value3);

        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model_from_file_unicode",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_core_compile_model_from_file_unicode(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPWStr)] string model_path,
            IntPtr device_name,
            UIntPtr property_args_size,
            ref IntPtr compiled_model,
            IntPtr key1,
            IntPtr value1,
            IntPtr key2,
            IntPtr value2,
            IntPtr key3,
            IntPtr value3);

        #endregion

        #region Properties

        /// <summary>
        /// 为设备设置属性 / Sets properties for a device
        /// </summary>
        /// <param name="core">Core 指针 / Core pointer</param>
        /// <param name="device_name">设备名称 / Device name</param>
        /// <param name="key">属性键 / Property key</param>
        /// <param name="value">属性值 / Property value</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_set_property",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern ExceptionStatus ov_core_set_property(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            IntPtr key,
            IntPtr value);

        [DllImport("openvino_c", EntryPoint = "ov_core_set_property",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_core_set_property_utf8(
            IntPtr core,
            IntPtr device_name,
            IntPtr key,
            IntPtr value);

        /// <summary>
        /// 为设备设置属性（带 2 个属性对）/ Sets properties for a device with 2 property pairs
        /// </summary>
        /// <param name="core">Core 指针 / Core pointer</param>
        /// <param name="device_name">设备名称 / Device name</param>
        /// <param name="key1">属性键 1 / Property key 1</param>
        /// <param name="value1">属性值 1 / Property value 1</param>
        /// <param name="key2">属性键 2 / Property key 2</param>
        /// <param name="value2">属性值 2 / Property value 2</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_set_property",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern ExceptionStatus ov_core_set_property(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            IntPtr key1,
            IntPtr value1,
            IntPtr key2,
            IntPtr value2);

        [DllImport("openvino_c", EntryPoint = "ov_core_set_property",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_core_set_property_utf8(
            IntPtr core,
            IntPtr device_name,
            IntPtr key1,
            IntPtr value1,
            IntPtr key2,
            IntPtr value2);

        /// <summary>
        /// 为设备设置属性（带 3 个属性对）/ Sets properties for a device with 3 property pairs
        /// </summary>
        /// <param name="core">Core 指针 / Core pointer</param>
        /// <param name="device_name">设备名称 / Device name</param>
        /// <param name="key1">属性键 1 / Property key 1</param>
        /// <param name="value1">属性值 1 / Property value 1</param>
        /// <param name="key2">属性键 2 / Property key 2</param>
        /// <param name="value2">属性值 2 / Property value 2</param>
        /// <param name="key3">属性键 3 / Property key 3</param>
        /// <param name="value3">属性值 3 / Property value 3</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_set_property",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern ExceptionStatus ov_core_set_property(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            IntPtr key1,
            IntPtr value1,
            IntPtr key2,
            IntPtr value2,
            IntPtr key3,
            IntPtr value3);

        [DllImport("openvino_c", EntryPoint = "ov_core_set_property",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_core_set_property_utf8(
            IntPtr core,
            IntPtr device_name,
            IntPtr key1,
            IntPtr value1,
            IntPtr key2,
            IntPtr value2,
            IntPtr key3,
            IntPtr value3);

        /// <summary>
        /// 获取与设备行为相关的属性 / Gets properties related to device behaviour
        /// </summary>
        /// <param name="core">Core 指针 / Core pointer</param>
        /// <param name="device_name">设备名称 / Device name</param>
        /// <param name="property_key">属性键 / Property key</param>
        /// <param name="property_value">返回的属性值指针 / Returned property value pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_get_property",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_get_property(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            [MarshalAs(UnmanagedType.LPStr)] string property_key,
            ref IntPtr property_value);

        [DllImport("openvino_c", EntryPoint = "ov_core_get_property",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_core_get_property_utf8(
            IntPtr core,
            IntPtr device_name,
            IntPtr property_key,
            ref IntPtr property_value);

        #endregion

        #region Available Devices

        /// <summary>
        /// 返回可用于推理的设备 / Returns devices available for inference
        /// </summary>
        /// <param name="core">Core 指针 / Core pointer</param>
        /// <param name="devices">返回的可用设备列表指针 / Returned available devices list pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_get_available_devices",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_get_available_devices(
            IntPtr core,
            IntPtr devices);

        /// <summary>
        /// 释放 ov_available_devices_t 占用的内存 / Releases memory occupied by ov_available_devices_t
        /// </summary>
        /// <param name="devices">设备列表指针 / Devices list pointer</param>
        [DllImport("openvino_c", EntryPoint = "ov_available_devices_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_available_devices_free(IntPtr devices);

        #endregion

        #region Import/Export

        /// <summary>
        /// 从先前导出的编译模型导入 / Imports a compiled model from the previously exported one
        /// </summary>
        /// <param name="core">Core 指针 / Core pointer</param>
        /// <param name="content">模型内容缓冲区 / Model content buffer</param>
        /// <param name="content_size">内容大小 / Content size</param>
        /// <param name="device_name">目标设备名称 / Target device name</param>
        /// <param name="compiled_model">返回的编译模型指针 / Returned compiled model pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_import_model",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_import_model(
            IntPtr core,
            ref byte content,
            ulong content_size,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            ref IntPtr compiled_model);

        [DllImport("openvino_c", EntryPoint = "ov_core_import_model",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_core_import_model_utf8(
            IntPtr core,
            ref byte content,
            UIntPtr content_size,
            IntPtr device_name,
            ref IntPtr compiled_model);

        #endregion

        #region Device Versions

        /// <summary>
        /// 返回设备插件版本信息 / Returns device plugins version information
        /// </summary>
        /// <param name="core">Core 指针 / Core pointer</param>
        /// <param name="device_name">设备名称 / Device name</param>
        /// <param name="versions">返回的版本列表指针 / Returned versions list pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_get_versions_by_device_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_get_versions_by_device_name(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            IntPtr versions);

        [DllImport("openvino_c", EntryPoint = "ov_core_get_versions_by_device_name",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_core_get_versions_by_device_name_utf8(
            IntPtr core,
            IntPtr device_name,
            IntPtr versions);

        /// <summary>
        /// 释放 ov_core_version_list_t 占用的内存 / Releases memory occupied by ov_core_version_list_t
        /// </summary>
        /// <param name="versions">版本列表指针 / Versions list pointer</param>
        [DllImport("openvino_c", EntryPoint = "ov_core_versions_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_core_versions_free(IntPtr versions);

        #endregion

        #region Remote Context

        /// <summary>
        /// 在指定的加速器设备上创建新的远程共享上下文对象 / Creates a new remote shared context object on the specified accelerator device
        /// </summary>
        /// <param name="core">Core 指针 / Core pointer</param>
        /// <param name="device_name">设备名称 / Device name</param>
        /// <param name="context_args_size">上下文参数数量 / Context arguments size</param>
        /// <param name="context">返回的上下文指针 / Returned context pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_create_context",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_create_context(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            ulong context_args_size,
            ref IntPtr context);

        [DllImport("openvino_c", EntryPoint = "ov_core_create_context",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_core_create_context_utf8(
            IntPtr core,
            IntPtr device_name,
            UIntPtr context_args_size,
            ref IntPtr context);

        /// <summary>
        /// 在指定的远程上下文中从源模型创建编译模型 / Creates a compiled model from a source model within a specified remote context
        /// </summary>
        /// <param name="core">Core 指针 / Core pointer</param>
        /// <param name="model">模型指针 / Model pointer</param>
        /// <param name="context">远程上下文指针 / Remote context pointer</param>
        /// <param name="property_args_size">属性参数数量 / Property arguments size</param>
        /// <param name="compiled_model">返回的编译模型指针 / Returned compiled model pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model_with_context",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model_with_context(
            IntPtr core,
            IntPtr model,
            IntPtr context,
            ulong property_args_size,
            ref IntPtr compiled_model);

        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model_with_context",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_core_compile_model_with_context_native_size(
            IntPtr core,
            IntPtr model,
            IntPtr context,
            UIntPtr property_args_size,
            ref IntPtr compiled_model);

        /// <summary>
        /// 获取指定加速器设备的默认共享上下文对象指针 / Gets a pointer to default shared context object for the specified accelerator device
        /// </summary>
        /// <param name="core">Core 指针 / Core pointer</param>
        /// <param name="device_name">设备名称 / Device name</param>
        /// <param name="context">返回的上下文指针 / Returned context pointer</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_get_default_context",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_get_default_context(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            ref IntPtr context);

        [DllImport("openvino_c", EntryPoint = "ov_core_get_default_context",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_core_get_default_context_utf8(
            IntPtr core,
            IntPtr device_name,
            ref IntPtr context);

        #endregion

        #region Extensions

        /// <summary>
        /// 向 Core 添加扩展 / Adds an extension to the core
        /// </summary>
        /// <param name="core">Core 指针 / Core pointer</param>
        /// <param name="path">扩展库路径 / Extension library path</param>
        /// <returns>操作状态 / Operation status</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_add_extension",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_add_extension(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string path);

        [DllImport("openvino_c", EntryPoint = "ov_core_add_extension",
            CallingConvention = CallingConvention.Cdecl)]
        internal extern static ExceptionStatus ov_core_add_extension_utf8(
            IntPtr core,
            IntPtr path);

        #endregion
    }

    #region Supporting Structures

    /// <summary>
    /// 表示可用设备的结构体 / Structure representing available devices
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ov_available_devices_t
    {
        /// <summary>
        /// 设备名称数组指针 / Device names array pointer
        /// </summary>
        public IntPtr devices;
        /// <summary>
        /// 设备数量 / Number of devices
        /// </summary>
        public ulong size;
    }

    /// <summary>
    /// 表示 OpenVINO 版本的结构体 / Structure representing OpenVINO version
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ov_version_t
    {
        /// <summary>
        /// 构建版本号 / Build number
        /// </summary>
        public IntPtr buildNumber;
        /// <summary>
        /// 版本描述 / Version description
        /// </summary>
        public IntPtr description;
    }

    /// <summary>
    /// 表示 Core 版本的结构体 / Structure representing core version
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ov_core_version_t
    {
        /// <summary>
        /// 设备名称 / Device name
        /// </summary>
        public IntPtr device_name;
        /// <summary>
        /// 版本信息 / Version information
        /// </summary>
        public ov_version_t version;
    }

    /// <summary>
    /// 表示 Core 版本列表的结构体 / Structure representing core version list
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ov_core_version_list_t
    {
        /// <summary>
        /// 版本数组指针 / Versions array pointer
        /// </summary>
        public IntPtr versions;
        /// <summary>
        /// 版本数量 / Number of versions
        /// </summary>
        public ulong size;
    }

    #endregion
}
