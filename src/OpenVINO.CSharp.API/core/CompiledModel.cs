// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static OpenVinoSharp.native.NativeMethods;
using OpenVinoSharp.Internal;

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
            ulong size = 0;
            ExceptionHandler.ThrowOnError(ov_compiled_model_inputs_size(_ptr, ref size));
            return size;
        }

        /// <summary>
        /// 获取指定索引的输入端口 / Get input port at specified index
        /// </summary>
        /// <param name="idx">输入索引 / Input index</param>
        /// <returns>节点输入端口 / Node input port</returns>
        public NodeInput get_input(ulong idx)
        {
            ThrowIfDisposed();
            IntPtr node_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_compiled_model_input_by_index(_ptr, idx, ref node_ptr));
            return new NodeInput(node_ptr);
        }

        /// <summary>
        /// 通过张量名称获取输入端口 / Get input port by tensor name
        /// </summary>
        /// <param name="tensor_name">张量名称 / Tensor name</param>
        /// <returns>节点输入端口 / Node input port</returns>
        public NodeInput get_input_by_name(string tensor_name)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(tensor_name))
                throw new ArgumentException("Parameter cannot be null or empty", nameof(tensor_name));
            
            IntPtr node_ptr = IntPtr.Zero;
            sbyte[] nameBytes = StringUtils.StringToSByteArray(tensor_name);
            ExceptionHandler.ThrowOnError(
                ov_compiled_model_input_by_name(_ptr, ref nameBytes[0], ref node_ptr));
            return new NodeInput(node_ptr);
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
            ulong size = 0;
            ExceptionHandler.ThrowOnError(ov_compiled_model_outputs_size(_ptr, ref size));
            return size;
        }

        /// <summary>
        /// 获取指定索引的输出端口 / Get output port at specified index
        /// </summary>
        /// <param name="idx">输出索引 / Output index</param>
        /// <returns>常量输出节点描述 / Const output node description</returns>
        public NodeOutput get_output(ulong idx)
        {
            ThrowIfDisposed();
            IntPtr node_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_compiled_model_output_by_index(_ptr, idx, ref node_ptr));
            return new NodeOutput(node_ptr);
        }

        /// <summary>
        /// 通过张量名称获取输出端口 / Get output port by tensor name
        /// </summary>
        /// <param name="tensor_name">张量名称 / Tensor name</param>
        /// <returns>常量输出节点描述 / Const output node description</returns>
        public NodeOutput get_output_by_name(string tensor_name)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(tensor_name))
                throw new ArgumentException("Parameter cannot be null or empty", nameof(tensor_name));
            
            IntPtr node_ptr = IntPtr.Zero;
            sbyte[] nameBytes = StringUtils.StringToSByteArray(tensor_name);
            ExceptionHandler.ThrowOnError(
                ov_compiled_model_output_by_name(_ptr, ref nameBytes[0], ref node_ptr));
            return new NodeOutput(node_ptr);
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
        /// 导出模型 / Export model
        /// </summary>
        /// <param name="model_path">导出路径 / Export path</param>
        public void export_model(string model_path)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(model_path))
                throw new ArgumentException("Parameter cannot be null or empty", nameof(model_path));

            ExceptionHandler.ThrowOnError(ov_compiled_model_export_model(_ptr, model_path));
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
        /// 设置编译模型属性 / Set property for compiled model
        /// </summary>
        /// <param name="key">属性键 / Property key</param>
        /// <param name="value">属性值 / Property value</param>
        public void set_property(string key, string value)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Parameter cannot be null or empty", nameof(key));

            IntPtr keyPtr = Marshal.StringToHGlobalAnsi(key);
            IntPtr valuePtr = Marshal.StringToHGlobalAnsi(value ?? string.Empty);
            try
            {
                ExceptionHandler.ThrowOnError(
                    ov_compiled_model_set_property(_ptr, 2, keyPtr, valuePtr));
            }
            finally
            {
                Marshal.FreeHGlobal(keyPtr);
                Marshal.FreeHGlobal(valuePtr);
            }
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
            sbyte[] keyBytes = StringUtils.StringToSByteArray(key);
            ExceptionHandler.ThrowOnError(
                ov_compiled_model_get_property(_ptr, ref keyBytes[0], ref value));
            return Marshal.PtrToStringAnsi(value) ?? string.Empty;
        }

        #endregion

        /// <summary>
        /// 获取原生指针（兼容属性）/ Get native pointer (compatibility property)
        /// </summary>
        public IntPtr Ptr => OvPtr;
    }
}
