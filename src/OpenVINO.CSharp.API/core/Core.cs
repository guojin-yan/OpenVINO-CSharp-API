// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenVinoSharp.native.NativeMethods;
using OpenVinoSharp.Internal;

// 日志使用示例 / Logger usage example:
// Logger.Info("消息 / Message");
// Logger.Debug("调试信息: {0}", value);
// Logger.SetCallback((level, msg) => { /* 自定义日志处理 / Custom log handling */ });

namespace OpenVinoSharp
{
    /// <summary>
    /// OpenVINO运行时核心类 / OpenVINO Runtime Core class
    /// <para>此类代表OpenVINO运行时核心实体。/ This class represents an OpenVINO runtime Core entity.</para>
    /// <para>推荐每个应用程序使用单个Core实例。/ It is recommended to have a single Core instance per application.</para>
    /// </summary>
    /// <remarks>
    /// 用户应用程序可以创建多个Core类实例，但底层插件会被创建多次且不在Core实例之间共享。
    /// User applications can create several Core class instances, but the underlying plugins
    /// are created multiple times and not shared between several Core instances.
    /// </remarks>
    public class Core : DisposableOvObject
    {
        /// <summary>
        /// 可用设备结构体 / Available devices structure
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct ov_available_devices_t
        {
            public IntPtr devices;
            public ulong size;
        }

        #region 静态构造函数 / Static Constructor

        /// <summary>
        /// 静态构造函数 - 确保原生库在使用任何 Core 功能之前加载
        /// Static constructor - ensure native library is loaded before any Core functionality
        /// </summary>
        static Core()
        {
            NativeLibraryLoader.EnsureLoaded();
        }

        #endregion

        #region 构造函数 / Constructors

        /// <summary>
        /// 默认构造函数 - 构造OpenVINO Core实例 / Default constructor - Constructs an OpenVINO Core instance
        /// </summary>
        public Core() : base()
        {
            Logger.Debug("正在创建 OpenVINO Core 实例... / Creating OpenVINO Core instance...");
            
            ExceptionHandler.ThrowOnError(ov_core_create(ref _ptr));
            Logger.Info("OpenVINO Core 实例创建成功 / OpenVINO Core instance created successfully");
        }

        /// <summary>
        /// 使用配置文件构造OpenVINO Core实例 / Constructs an OpenVINO Core instance with config file
        /// </summary>
        /// <param name="xml_config_file">
        /// 插件配置文件路径。/ Path to the XML configuration file with plugins.
        /// </param>
        public Core(string xml_config_file) : base()
        {
            Logger.Debug("正在使用配置文件创建 OpenVINO Core 实例... / Creating OpenVINO Core instance with config file...");
            Logger.Debug("配置文件路径 / Config file path: {0}", xml_config_file);
            
            // 尝试加载原生库 / Try to load native library
            try
            {
                NativeLibraryLoader.Load();
                Logger.Debug("原生库加载成功 / Native library loaded successfully");
            }
            catch (DllNotFoundException ex)
            {
                Logger.Warn("原生库加载失败，可能已由系统加载 / Failed to load native library, may already be loaded by system: {0}", ex.Message);
            }

            ExceptionHandler.ThrowOnError(ov_core_create_with_config(xml_config_file, ref _ptr));
            Logger.Info("OpenVINO Core 实例（带配置）创建成功 / OpenVINO Core instance (with config) created successfully");
        }

        #endregion

        #region 资源释放 / Resource Disposal

        /// <inheritdoc/>
        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero && IsEnabledDispose)
            {
                ov_core_free(_ptr);
            }
            base.DisposeUnmanaged();
        }

        #endregion

        #region 静态方法 / Static Methods

        /// <summary>
        /// 关闭OpenVINO并释放所有静态资源 / Shut down OpenVINO and release all static resources
        /// <para>高级用户可以使用此函数在动态加载库卸载时清理资源。/ Advanced users can use this to clean up resources when dynamically loaded library is unloaded.</para>
        /// </summary>
        public static void shutdown() => ov_shutdown();

        #endregion

        #region 模型读取 / Model Reading

        /// <summary>
        /// 从文件读取模型 / Read model from file
        /// <para>支持IR、ONNX、PDPD、TF、TFLite格式。/ Supports IR, ONNX, PDPD, TF, TFLite formats.</para>
        /// </summary>
        /// <param name="model_path">模型文件路径 / Path to model file</param>
        /// <param name="bin_path">权重文件路径（IR格式时使用，可选）/ Path to weights file (for IR format, optional)</param>
        /// <returns>模型对象 / Model object</returns>
        public Model read_model(string model_path, string bin_path = null)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(model_path))
                throw new ArgumentException("参数不能为空", nameof(model_path));

            IntPtr model_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(
                ov_core_read_model(_ptr, model_path, bin_path ?? string.Empty, ref model_ptr));

            return new Model(model_ptr);
        }

        /// <summary>
        /// 从文件读取模型并指定权重张量 / Read model from file with weights tensor
        /// </summary>
        /// <param name="model_path">模型文件路径 / Path to model file</param>
        /// <param name="weights">权重张量 / Weights tensor</param>
        /// <returns>模型对象 / Model object</returns>
        public Model read_model(string model_path, Tensor weights)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(model_path))
                throw new ArgumentException("参数不能为空", nameof(model_path));
            if (weights == null)
                throw new ArgumentNullException(nameof(weights));

            byte[] data = Ov.content_from_file(model_path);
            IntPtr model_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(
                ov_core_read_model_from_memory_buffer(_ptr, ref data[0], (ulong)data.Length, weights.OvPtr, ref model_ptr));
            return new Model(model_ptr);
        }

        /// <summary>
        /// 从内存缓冲区读取模型 / Read model from memory buffer
        /// </summary>
        /// <param name="xml_model_data">模型XML数据 / Model XML data</param>
        /// <param name="weights">权重张量 / Weights tensor</param>
        /// <returns>模型对象 / Model object</returns>
        public Model read_model(byte[] xml_model_data, Tensor weights)
        {
            ThrowIfDisposed();
            if (xml_model_data == null)
                throw new ArgumentNullException(nameof(xml_model_data));
            if (weights == null)
                throw new ArgumentNullException(nameof(weights));

            IntPtr model_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(
                ov_core_read_model_from_memory_buffer(_ptr, ref xml_model_data[0], (ulong)xml_model_data.Length, weights.OvPtr, ref model_ptr));
            return new Model(model_ptr);
        }

#if HAS_SPAN
        /// <summary>
        /// 从Span内存缓冲区读取模型（高性能，零拷贝）/ Read model from Span memory buffer (high performance, zero-copy)
        /// <para>.NET Core 2.1+ / .NET 5+ 支持 / Supported on .NET Core 2.1+ / .NET 5+</para>
        /// </summary>
        /// <param name="xml_model_data">模型XML数据 / Model XML data</param>
        /// <param name="weights">权重张量 / Weights tensor</param>
        /// <returns>模型对象 / Model object</returns>
        public unsafe Model read_model(ReadOnlySpan<byte> xml_model_data, Tensor weights)
        {
            ThrowIfDisposed();
            if (weights == null)
                throw new ArgumentNullException(nameof(weights));

            IntPtr model_ptr = IntPtr.Zero;
            fixed (byte* dataPtr = xml_model_data)
            {
                ExceptionHandler.ThrowOnError(
                    ov_core_read_model_from_memory_buffer(_ptr, ref *dataPtr, (ulong)xml_model_data.Length, weights.OvPtr, ref model_ptr));
            }
            return new Model(model_ptr);
        }
#endif

        #endregion

        #region 模型编译 / Model Compilation

        /// <summary>
        /// 编译模型 / Compile model
        /// </summary>
        /// <param name="model">模型对象 / Model object</param>
        /// <returns>编译后的模型 / Compiled model</returns>
        public CompiledModel compile_model(Model model)
            => compile_model(model, "AUTO", null);

        /// <summary>
        /// 编译模型 / Compile model
        /// </summary>
        /// <param name="model">模型对象 / Model object</param>
        /// <param name="properties">编译属性 / Compilation properties</param>
        /// <returns>编译后的模型 / Compiled model</returns>
        public CompiledModel compile_model(Model model, Dictionary<string, string> properties)
            => compile_model(model, "AUTO", properties);

        /// <summary>
        /// 编译模型到指定设备 / Compile model for specified device
        /// </summary>
        /// <param name="model">模型对象 / Model object</param>
        /// <param name="device_name">设备名称（如"CPU"、"GPU"）/ Device name (e.g., "CPU", "GPU")</param>
        /// <param name="properties">编译属性（可选）/ Compilation properties (optional)</param>
        /// <returns>编译后的模型 / Compiled model</returns>
        public CompiledModel compile_model(Model model, string device_name, Dictionary<string, string> properties)
        {
            ThrowIfDisposed();
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (string.IsNullOrEmpty(device_name))
                throw new ArgumentException("参数不能为空", nameof(device_name));

            IntPtr compiled_model_ptr = IntPtr.Zero;

            if (properties == null || properties.Count == 0)
            {
                ExceptionHandler.ThrowOnError(
                    ov_core_compile_model(_ptr, model.OvPtr, device_name, 0, ref compiled_model_ptr));
            }
            else
            {
                CompileModelWithProperties(model.OvPtr, device_name, properties, ref compiled_model_ptr);
            }
            return new CompiledModel(compiled_model_ptr);
        }

        /// <summary>
        /// 从文件编译模型 / Compile model from file
        /// </summary>
        /// <param name="model_path">模型文件路径 / Path to model file</param>
        /// <returns>编译后的模型 / Compiled model</returns>
        public CompiledModel compile_model(string model_path)
            => compile_model(model_path, "AUTO", null);

        /// <summary>
        /// 从文件编译模型 / Compile model from file
        /// </summary>
        /// <param name="model_path">模型文件路径 / Path to model file</param>
        /// <param name="properties">编译属性 / Compilation properties</param>
        /// <returns>编译后的模型 / Compiled model</returns>
        public CompiledModel compile_model(string model_path, Dictionary<string, string> properties)
            => compile_model(model_path, "AUTO", properties);

        /// <summary>
        /// 从文件编译模型到指定设备 / Compile model from file for specified device
        /// </summary>
        /// <param name="model_path">模型文件路径 / Path to model file</param>
        /// <param name="device_name">设备名称 / Device name</param>
        /// <param name="properties">编译属性（可选）/ Compilation properties (optional)</param>
        /// <param name="use_cache">是否使用缓存 / Whether to use cache</param>
        /// <returns>编译后的模型 / Compiled model</returns>
        public CompiledModel compile_model(string model_path, string device_name, Dictionary<string, string> properties, bool use_cache = true)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(model_path))
                throw new ArgumentException("参数不能为空", nameof(model_path));
            if (string.IsNullOrEmpty(device_name))
                throw new ArgumentException("参数不能为空", nameof(device_name));

            // 尝试从缓存获取
            if (use_cache && ModelCache.Enabled)
            {
                var cached = ModelCache.TryGet(model_path, device_name, properties);
                if (cached != null)
                {
                    Logger.Debug($"Core: 使用缓存的编译模型 - {model_path}");
                    return cached;
                }
            }

            IntPtr compiled_model_ptr = IntPtr.Zero;

            Logger.Debug($"Core: 编译模型 - {model_path} [{device_name}]");
            
            if (properties == null || properties.Count == 0)
            {
                ExceptionHandler.ThrowOnError(
                    ov_core_compile_model_from_file(_ptr, model_path, device_name, 0, ref compiled_model_ptr));
            }
            else
            {
                CompileModelFromFileWithProperties(model_path, device_name, properties, ref compiled_model_ptr);
            }
            
            var compiledModel = new CompiledModel(compiled_model_ptr);
            
            // 添加到缓存
            if (use_cache && ModelCache.Enabled)
            {
                ModelCache.Add(model_path, device_name, properties, compiledModel);
            }
            
            return compiledModel;
        }

        /// <summary>
        /// 带属性的模型编译（内部方法）/ Compile model with properties (internal method)
        /// </summary>
        private void CompileModelWithProperties(IntPtr modelPtr, string device_name, Dictionary<string, string> properties, ref IntPtr compiled_model_ptr)
        {
            // 使用ArrayPool减少内存分配 / Use ArrayPool to reduce memory allocation
            IntPtr[] inputs = new IntPtr[properties.Count * 2];
            int idx = 0;
            foreach (var item in properties)
            {
                inputs[idx++] = Marshal.StringToHGlobalAnsi(item.Key);
                inputs[idx++] = Marshal.StringToHGlobalAnsi(item.Value);
            }

            try
            {
                ExceptionStatus status;
                switch (properties.Count)
                {
                    case 1:
                        status = ov_core_compile_model(_ptr, modelPtr, device_name, 2, ref compiled_model_ptr, inputs[0], inputs[1]);
                        break;
                    case 2:
                        status = ov_core_compile_model(_ptr, modelPtr, device_name, 4, ref compiled_model_ptr, inputs[0], inputs[1], inputs[2], inputs[3]);
                        break;
                    case 3:
                        status = ov_core_compile_model(_ptr, modelPtr, device_name, 6, ref compiled_model_ptr, inputs[0], inputs[1], inputs[2], inputs[3], inputs[4], inputs[5]);
                        break;
                    default:
                        throw new ArgumentException("仅支持0、1、2、3个属性参数。/ Only supports 0, 1, 2, or 3 property parameters.");
                }
                ExceptionHandler.ThrowOnError(status);
            }
            finally
            {
                foreach (var ptr in inputs)
                {
                    if (ptr != IntPtr.Zero)
                        Marshal.FreeHGlobal(ptr);
                }
            }
        }

        private void CompileModelFromFileWithProperties(string model_path, string device_name, Dictionary<string, string> properties, ref IntPtr compiled_model_ptr)
        {
            IntPtr[] inputs = new IntPtr[properties.Count * 2];
            int idx = 0;
            foreach (var item in properties)
            {
                inputs[idx++] = Marshal.StringToHGlobalAnsi(item.Key);
                inputs[idx++] = Marshal.StringToHGlobalAnsi(item.Value);
            }

            try
            {
                ExceptionStatus status;
                switch (properties.Count)
                {
                    case 1:
                        status = ov_core_compile_model_from_file(_ptr, model_path, device_name, 2, ref compiled_model_ptr, inputs[0], inputs[1]);
                        break;
                    case 2:
                        status = ov_core_compile_model_from_file(_ptr, model_path, device_name, 4, ref compiled_model_ptr, inputs[0], inputs[1], inputs[2], inputs[3]);
                        break;
                    case 3:
                        status = ov_core_compile_model_from_file(_ptr, model_path, device_name, 6, ref compiled_model_ptr, inputs[0], inputs[1], inputs[2], inputs[3], inputs[4], inputs[5]);
                        break;
                    default:
                        throw new ArgumentException("仅支持0、1、2、3个属性参数。/ Only supports 0, 1, 2, or 3 property parameters.");
                }
                ExceptionHandler.ThrowOnError(status);
            }
            finally
            {
                foreach (var ptr in inputs)
                {
                    if (ptr != IntPtr.Zero)
                        Marshal.FreeHGlobal(ptr);
                }
            }
        }

        #endregion

        #region 扩展和导入 / Extensions and Import

        /// <summary>
        /// 添加扩展到Core / Add extension to Core
        /// </summary>
        /// <param name="path">扩展库路径 / Path to extension library</param>
        public void add_extension(string path)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("参数不能为空", nameof(path));
            ExceptionHandler.ThrowOnError(ov_core_add_extension(_ptr, path));
        }

        /// <summary>
        /// 导入已编译的模型 / Import compiled model
        /// </summary>
        /// <param name="model_path">模型文件路径 / Path to model file</param>
        /// <param name="device_name">设备名称 / Device name</param>
        /// <returns>编译后的模型 / Compiled model</returns>
        public CompiledModel import_model(string model_path, string device_name = "AUTO")
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(model_path))
                throw new ArgumentException("参数不能为空", nameof(model_path));

            IntPtr value = IntPtr.Zero;
            byte[] data = Ov.content_from_file(model_path);
            ExceptionHandler.ThrowOnError(
                ov_core_import_model(_ptr, ref data[0], (ulong)data.Length, device_name, ref value));
            return new CompiledModel(value);
        }

        #endregion

        #region 设备属性和查询 / Device Properties and Query

        /// <summary>
        /// 获取设备版本信息 / Get device version information
        /// </summary>
        /// <param name="device_name">设备名称 / Device name</param>
        /// <returns>设备名称和版本 / Device name and version</returns>
        public KeyValuePair<string, Version> get_versions(string device_name)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(device_name))
                throw new ArgumentException("参数不能为空", nameof(device_name));

            int size = Marshal.SizeOf(typeof(CoreVersionList));
            IntPtr ptr_core_version_s = Marshal.AllocHGlobal(size);
            try
            {
                ExceptionHandler.ThrowOnError(
                    ov_core_get_versions_by_device_name(_ptr, device_name, ptr_core_version_s));

                CoreVersionList core_version_s = Marshal.PtrToStructure<CoreVersionList>(ptr_core_version_s);
                CoreVersion core_version = Marshal.PtrToStructure<CoreVersion>(core_version_s.core_version);
                var value = new KeyValuePair<string, Version>(
                    core_version.device_name, core_version.version);
                ov_core_versions_free(ptr_core_version_s);
                return value;
            }
            catch
            {
                Marshal.FreeHGlobal(ptr_core_version_s);
                throw;
            }
        }

        /// <summary>
        /// 获取可用设备列表 / Get list of available devices
        /// </summary>
        /// <returns>设备名称列表 / List of device names</returns>
        public List<string> get_available_devices()
        {
            ThrowIfDisposed();

            int size = Marshal.SizeOf(typeof(ov_available_devices_t));
            IntPtr devices_ptr = Marshal.AllocHGlobal(size);
            try
            {
                ExceptionHandler.ThrowOnError(ov_core_get_available_devices(_ptr, devices_ptr));

                ov_available_devices_t devices_s = Marshal.PtrToStructure<ov_available_devices_t>(devices_ptr);
                IntPtr[] devices_ptrs = new IntPtr[devices_s.size];
                Marshal.Copy(devices_s.devices, devices_ptrs, 0, (int)devices_s.size);
                
                List<string> devices = new List<string>((int)devices_s.size);
                for (int i = 0; i < (int)devices_s.size; ++i)
                {
                    string deviceName = Marshal.PtrToStringAnsi(devices_ptrs[i]);
                    if (!string.IsNullOrEmpty(deviceName))
                        devices.Add(deviceName);
                }
                ov_available_devices_free(devices_ptr);
                return devices;
            }
            catch
            {
                Marshal.FreeHGlobal(devices_ptr);
                throw;
            }
        }

        /// <summary>
        /// 设置设备属性 / Set device property
        /// </summary>
        /// <param name="device_name">设备名称 / Device name</param>
        /// <param name="key">属性键 / Property key</param>
        /// <param name="value">属性值 / Property value</param>
        public void set_property(string device_name, string key, string value)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(device_name))
                throw new ArgumentException("参数不能为空", nameof(device_name));
            
            IntPtr keyPtr = Marshal.StringToHGlobalAnsi(key);
            IntPtr valuePtr = Marshal.StringToHGlobalAnsi(value);
            try
            {
                ExceptionHandler.ThrowOnError(
                    ov_core_set_property(_ptr, device_name, keyPtr, valuePtr));
            }
            finally
            {
                Marshal.FreeHGlobal(keyPtr);
                Marshal.FreeHGlobal(valuePtr);
            }
        }

        /// <summary>
        /// 批量设置设备属性 / Set multiple device properties
        /// </summary>
        /// <param name="device_name">设备名称 / Device name</param>
        /// <param name="properties">属性字典 / Properties dictionary</param>
        public void set_property(string device_name, Dictionary<string, string> properties)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(device_name))
                throw new ArgumentException("参数不能为空", nameof(device_name));
            if (properties == null || properties.Count == 0) return;

            IntPtr[] inputs = new IntPtr[properties.Count * 2];
            int idx = 0;
            foreach (var item in properties)
            {
                inputs[idx++] = Marshal.StringToHGlobalAnsi(item.Key);
                inputs[idx++] = Marshal.StringToHGlobalAnsi(item.Value);
            }

            try
            {
                ExceptionStatus status;
                switch (properties.Count)
                {
                    case 1:
                        status = ov_core_set_property(_ptr, device_name, inputs[0], inputs[1]);
                        break;
                    case 2:
                        status = ov_core_set_property(_ptr, device_name, inputs[0], inputs[1], inputs[2], inputs[3]);
                        break;
                    case 3:
                        status = ov_core_set_property(_ptr, device_name, inputs[0], inputs[1], inputs[2], inputs[3], inputs[4], inputs[5]);
                        break;
                    default:
                        throw new ArgumentException("仅支持1、2、3个属性参数。/ Only supports 1, 2, or 3 property parameters.");
                }
                ExceptionHandler.ThrowOnError(status);
            }
            finally
            {
                foreach (var ptr in inputs)
                {
                    if (ptr != IntPtr.Zero)
                        Marshal.FreeHGlobal(ptr);
                }
            }
        }

        /// <summary>
        /// 获取设备属性 / Get device property
        /// </summary>
        /// <param name="device_name">设备名称 / Device name</param>
        /// <param name="key">属性键 / Property key</param>
        /// <returns>属性值 / Property value</returns>
        public string get_property(string device_name, string key)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(device_name))
                throw new ArgumentException("参数不能为空", nameof(device_name));
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("参数不能为空", nameof(key));

            IntPtr value = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(
                ov_core_get_property(_ptr, device_name, key, ref value));
            return Marshal.PtrToStringAnsi(value) ?? string.Empty;
        }

        #endregion

        #region 远程上下文 / Remote Context

        /// <summary>
        /// 创建远程上下文 / Create remote context
        /// <para>用于GPU等设备的远程内存管理。/ Used for remote memory management on GPU and other devices.</para>
        /// </summary>
        /// <param name="device_name">设备名称（如"GPU"）/ Device name (e.g., "GPU")</param>
        /// <returns>远程上下文指针 / Remote context pointer</returns>
        public IntPtr create_context(string device_name)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(device_name))
                throw new ArgumentException("参数不能为空", nameof(device_name));

            IntPtr context_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_core_create_context(_ptr, device_name, 0, ref context_ptr));
            return context_ptr;
        }

        /// <summary>
        /// 获取默认远程上下文 / Get default remote context
        /// </summary>
        /// <param name="device_name">设备名称 / Device name</param>
        /// <returns>远程上下文指针 / Remote context pointer</returns>
        public IntPtr get_default_context(string device_name)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(device_name))
                throw new ArgumentException("参数不能为空", nameof(device_name));

            IntPtr context_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_core_get_default_context(_ptr, device_name, ref context_ptr));
            return context_ptr;
        }

        /// <summary>
        /// 在远程上下文中编译模型 / Compile model with remote context
        /// </summary>
        /// <param name="model">模型对象 / Model object</param>
        /// <param name="context">远程上下文指针 / Remote context pointer</param>
        /// <returns>编译后的模型 / Compiled model</returns>
        public CompiledModel compile_model_with_context(Model model, IntPtr context)
        {
            ThrowIfDisposed();
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (context == IntPtr.Zero)
                throw new ArgumentException("上下文指针不能为空", nameof(context));

            IntPtr compiled_model_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(
                ov_core_compile_model_with_context(_ptr, model.OvPtr, context, 0, ref compiled_model_ptr));
            return new CompiledModel(compiled_model_ptr);
        }

        #endregion

        /// <summary>
        /// 获取原生指针（兼容属性）/ Get native pointer (compatibility property)
        /// </summary>
        public IntPtr Ptr => OvPtr;
    }
}
