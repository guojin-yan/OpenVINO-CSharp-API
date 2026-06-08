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

using OpenVinoSharp.Internal;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static OpenVinoSharp.native.NativeMethods;

namespace OpenVinoSharp
{
    /// <summary>
    /// 编译模型类 / Compiled model class
    /// <para>代表已编译的可执行模型。/ Represents a compiled executable model.</para>
    /// </summary>
    public class CompiledModel : DisposableOvObject
    {
        #region 构造函数 / Constructors

        /// <summary>
        /// 从原生指针构造 / Construct from native pointer
        /// </summary>
        /// <param name="ptr">原生编译模型指针 / Native compiled model pointer</param>
        public CompiledModel(IntPtr ptr) : base(ptr) { }

        #endregion

        #region 资源释放 / Resource Disposal

        /// <inheritdoc/>
        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero && IsEnabledDispose)
            {
                ov_compiled_model_free(_ptr);
            }
            base.DisposeUnmanaged();
        }

        #endregion

        #region 输入信息 / Input Information

        /// <summary>
        /// 获取输入端口数量 / Get number of input ports
        /// </summary>
        /// <returns>输入数量 / Number of inputs</returns>
        public ulong get_inputs_size()
        {
            ThrowIfDisposed();
            UIntPtr size = UIntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_compiled_model_inputs_size_native_size(_ptr, ref size));
            return StringUtils.FromNativeSize(size);
        }

        /// <summary>
        /// 获取指定索引的输入端口 / Get input port at specified index
        /// </summary>
        /// <param name="idx">输入索引 / Input index</param>
        /// <returns>节点输入端口 / Node input port</returns>
        public Input get_input(ulong idx)
        {
            ThrowIfDisposed();
            IntPtr node_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(
                ov_compiled_model_input_by_index_native_size(_ptr, StringUtils.ToNativeSize(idx), ref node_ptr));
            return new Input(node_ptr, true);
        }

        /// <summary>
        /// 通过张量名称获取输入端口 / Get input port by tensor name
        /// </summary>
        /// <param name="tensor_name">张量名称 / Tensor name</param>
        /// <returns>节点输入端口 / Node input port</returns>
        public Input get_input_by_name(string tensor_name)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(tensor_name))
                throw new ArgumentException("Parameter cannot be null or empty", nameof(tensor_name));
            
            IntPtr node_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(StringUtils.WithUtf8Ptr(
                tensor_name,
                namePtr => ov_compiled_model_input_by_name_utf8(_ptr, namePtr, ref node_ptr)));
            return new Input(node_ptr, true);
        }

        /// <summary>
        /// 获取输入端口数量 / Gets the number of input ports.
        /// </summary>
        public ulong InputCount
        {
            get { return get_inputs_size(); }
        }

        /// <summary>
        /// 按索引获取输入端口 / Gets an input port by index.
        /// </summary>
        /// <param name="idx">输入索引 / Input index.</param>
        /// <returns>输入端口 / Input port.</returns>
        public Input GetInput(ulong idx)
        {
            return get_input(idx);
        }

        /// <summary>
        /// 按名称获取输入端口 / Gets an input port by name.
        /// </summary>
        /// <param name="tensorName">张量名称 / Tensor name.</param>
        /// <returns>输入端口 / Input port.</returns>
        public Input GetInput(string tensorName)
        {
            return get_input_by_name(tensorName);
        }

        #endregion

        #region 输出信息 / Output Information

        /// <summary>
        /// 获取输出端口数量 / Get number of output ports
        /// </summary>
        /// <returns>输出数量 / Number of outputs</returns>
        public ulong get_outputs_size()
        {
            ThrowIfDisposed();
            UIntPtr size = UIntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_compiled_model_outputs_size_native_size(_ptr, ref size));
            return StringUtils.FromNativeSize(size);
        }

        /// <summary>
        /// 获取指定索引的输出端口 / Get output port at specified index
        /// </summary>
        /// <param name="idx">输出索引 / Output index</param>
        /// <returns>常量输出节点描述 / Const output node description</returns>
        public Output get_output(ulong idx)
        {
            ThrowIfDisposed();
            IntPtr node_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(
                ov_compiled_model_output_by_index_native_size(_ptr, StringUtils.ToNativeSize(idx), ref node_ptr));
            return new Output(node_ptr, true);
        }

        /// <summary>
        /// 通过张量名称获取输出端口 / Get output port by tensor name
        /// </summary>
        /// <param name="tensor_name">张量名称 / Tensor name</param>
        /// <returns>常量输出节点描述 / Const output node description</returns>
        public Output get_output_by_name(string tensor_name)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(tensor_name))
                throw new ArgumentException("Parameter cannot be null or empty", nameof(tensor_name));
            
            IntPtr node_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(StringUtils.WithUtf8Ptr(
                tensor_name,
                namePtr => ov_compiled_model_output_by_name_utf8(_ptr, namePtr, ref node_ptr)));
            return new Output(node_ptr, true);
        }

        /// <summary>
        /// 获取输出端口数量 / Gets the number of output ports.
        /// </summary>
        public ulong OutputCount
        {
            get { return get_outputs_size(); }
        }

        /// <summary>
        /// 按索引获取输出端口 / Gets an output port by index.
        /// </summary>
        /// <param name="idx">输出索引 / Output index.</param>
        /// <returns>输出端口 / Output port.</returns>
        public Output GetOutput(ulong idx)
        {
            return get_output(idx);
        }

        /// <summary>
        /// 按名称获取输出端口 / Gets an output port by name.
        /// </summary>
        /// <param name="tensorName">张量名称 / Tensor name.</param>
        /// <returns>输出端口 / Output port.</returns>
        public Output GetOutput(string tensorName)
        {
            return get_output_by_name(tensorName);
        }

        #endregion

        #region 运行时操作 / Runtime Operations

        /// <summary>
        /// 创建推理请求 / Create inference request
        /// </summary>
        /// <returns>推理请求对象 / Inference request object</returns>
        public InferRequest create_infer_request()
        {
            ThrowIfDisposed();
            IntPtr infer_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_compiled_model_create_infer_request(_ptr, ref infer_ptr));
            return new InferRequest(infer_ptr);
        }

        /// <summary>
        /// 创建推理请求 / Creates an inference request.
        /// </summary>
        /// <returns>推理请求 / Inference request.</returns>
        public InferRequest CreateInferRequest()
        {
            return create_infer_request();
        }

        /// <summary>
        /// 导出模型 / Export model
        /// </summary>
        /// <param name="model_path">导出路径 / Export path</param>
        public void export_model(string model_path)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(model_path))
                throw new ArgumentException("Parameter cannot be null or empty", nameof(model_path));

            ExceptionHandler.ThrowOnError(StringUtils.WithUtf8Ptr(
                model_path,
                pathPtr => ov_compiled_model_export_model_utf8(_ptr, pathPtr)));
        }

        /// <summary>
        /// 导出编译模型 / Exports the compiled model.
        /// </summary>
        /// <param name="modelPath">导出路径 / Export path.</param>
        public void ExportModel(string modelPath)
        {
            export_model(modelPath);
        }

        /// <summary>
        /// 获取运行时模型 / Get runtime model
        /// <para>返回设备特定的运行时模型表示。/ Returns device-specific runtime model representation.</para>
        /// </summary>
        /// <returns>运行时模型 / Runtime model</returns>
        public Model get_runtime_model()
        {
            ThrowIfDisposed();
            IntPtr model_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_compiled_model_get_runtime_model(_ptr, ref model_ptr));
            return new Model(model_ptr);
        }

        /// <summary>
        /// 获取运行时模型 / Gets the runtime model.
        /// </summary>
        /// <returns>运行时模型 / Runtime model.</returns>
        public Model GetRuntimeModel()
        {
            return get_runtime_model();
        }

        /// <summary>
        /// 设置编译模型属性 / Set property for compiled model
        /// </summary>
        /// <param name="key">属性键 / Property key</param>
        /// <param name="value">属性值 / Property value</param>
        public void set_property(string key, string value)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Parameter cannot be null or empty", nameof(key));

            IntPtr keyPtr = StringUtils.StringToUtf8Ptr(key);
            IntPtr valuePtr = StringUtils.StringToUtf8Ptr(value ?? string.Empty);
            try
            {
                ExceptionHandler.ThrowOnError(
                    ov_compiled_model_set_property_native_size(_ptr, StringUtils.ToNativeSize(2), keyPtr, valuePtr));
            }
            finally
            {
                Marshal.FreeHGlobal(keyPtr);
                Marshal.FreeHGlobal(valuePtr);
            }
        }

        /// <summary>
        /// 设置编译模型属性 / Sets a compiled model property.
        /// </summary>
        /// <param name="key">属性键 / Property key.</param>
        /// <param name="value">属性值 / Property value.</param>
        public void SetProperty(string key, string value)
        {
            set_property(key, value);
        }

        /// <summary>
        /// 获取编译模型属性 / Get property of compiled model
        /// </summary>
        /// <param name="key">属性键 / Property key</param>
        /// <returns>属性值 / Property value</returns>
        public string get_property(string key)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Parameter cannot be null or empty", nameof(key));

            IntPtr value = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(StringUtils.WithUtf8Ptr(
                key,
                keyPtr => ov_compiled_model_get_property_utf8(_ptr, keyPtr, ref value)));
            try
            {
                return StringUtils.Utf8PtrToString(value);
            }
            finally
            {
                if (value != IntPtr.Zero)
                    ov_free(value);
            }
        }

        /// <summary>
        /// 获取编译模型属性 / Gets a compiled model property.
        /// </summary>
        /// <param name="key">属性键 / Property key.</param>
        /// <returns>属性值 / Property value.</returns>
        public string GetProperty(string key)
        {
            return get_property(key);
        }

        #endregion

        #region 远程上下文 / Remote Context

        /// <summary>
        /// 获取远程上下文 / Get remote context
        /// <para>返回用于创建此编译模型的远程上下文。/ Returns the remote context used to create this compiled model.</para>
        /// </summary>
        /// <returns>远程上下文对象 / Remote context object</returns>
        public RemoteContext get_context()
        {
            ThrowIfDisposed();
            IntPtr context_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_compiled_model_get_context(_ptr, ref context_ptr));
            return new RemoteContext(context_ptr);
        }

        /// <summary>
        /// 获取远程上下文 / Gets the remote context.
        /// </summary>
        /// <returns>远程上下文 / Remote context.</returns>
        public RemoteContext GetContext()
        {
            return get_context();
        }

        #endregion

        /// <summary>
        /// 获取原生指针（兼容属性）/ Get native pointer (compatibility property)
        /// </summary>
        public IntPtr Ptr => OvPtr;
    }
}
