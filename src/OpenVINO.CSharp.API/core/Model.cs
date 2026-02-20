// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static OpenVinoSharp.native.NativeMethods;
using OpenVinoSharp.Internal;
using OpenVinoSharp.native;

namespace OpenVinoSharp
{
    /// <summary>
    /// 模型类 / Model class
    /// <para>表示OpenVINO模型，包含计算图定义。/ Represents an OpenVINO model containing a computation graph definition.</para>
    /// </summary>
    public class Model : DisposableOvObject
    {
        #region 构造函数 / Constructors

        /// <summary>
        /// 从原生指针构造 / Construct from native pointer
        /// </summary>
        /// <param name="ptr">原生模型指针 / Native model pointer</param>
        public Model(IntPtr ptr) : base(ptr) { }

        #endregion

        #region 资源释放 / Resource Disposal

        /// <inheritdoc/>
        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero && IsEnabledDispose)
            {
                ov_model_free(_ptr);
            }
            base.DisposeUnmanaged();
        }

        #endregion

        #region 输入信息 / Input Information

        /// <summary>
        /// 获取输入数量 / Get number of inputs
        /// </summary>
        /// <returns>输入数量 / Number of inputs</returns>
        public ulong get_inputs_size()
        {
            ThrowIfDisposed();
            ulong size = 0;
            ExceptionHandler.ThrowOnError(ov_model_inputs_size(_ptr, ref size));
            return size;
        }

        /// <summary>
        /// 获取指定索引的输入 / Get input at specified index
        /// </summary>
        /// <param name="idx">输入索引 / Input index</param>
        /// <returns>节点输入端口 / Node input port</returns>
        public NodeInput get_input(ulong idx)
        {
            ThrowIfDisposed();
            IntPtr node_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_model_input_by_index(_ptr, idx, ref node_ptr));
            return new NodeInput(node_ptr);
        }

        /// <summary>
        /// 获取指定名称的输入 / Get input by name
        /// </summary>
        /// <param name="name">输入名称 / Input name</param>
        /// <returns>节点输入端口 / Node input port</returns>
        public NodeInput get_input_by_name(string name)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Parameter cannot be null or empty", nameof(name));
            
            IntPtr node_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_model_input_by_name(_ptr, name, ref node_ptr));
            return new NodeInput(node_ptr);
        }

        /// <summary>
        /// 获取指定节点输出的输入 / Get input by node output
        /// </summary>
        /// <param name="input_port">输入端口 / Input port</param>
        /// <returns>节点输入端口 / Node input port</returns>
        public NodeInput get_input_by_port(NodeInput input_port)
        {
            ThrowIfDisposed();
            if (input_port == null)
                throw new ArgumentNullException(nameof(input_port));
            
            // ov_model_input_by_port is not available in the C API
            // Using get_input_by_name as fallback
            return get_input_by_name(input_port.get_any_name());
        }

        #endregion

        #region 输出信息 / Output Information

        /// <summary>
        /// 获取输出数量 / Get number of outputs
        /// </summary>
        /// <returns>输出数量 / Number of outputs</returns>
        public ulong get_outputs_size()
        {
            ThrowIfDisposed();
            ulong size = 0;
            ExceptionHandler.ThrowOnError(ov_model_outputs_size(_ptr, ref size));
            return size;
        }

        /// <summary>
        /// 获取指定索引的输出 / Get output at specified index
        /// </summary>
        /// <param name="idx">输出索引 / Output index</param>
        /// <returns>输出节点描述 / Output node description</returns>
        public NodeOutput get_output(ulong idx)
        {
            ThrowIfDisposed();
            IntPtr node_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_model_output_by_index(_ptr, idx, ref node_ptr));
            return new NodeOutput(node_ptr);
        }

        /// <summary>
        /// 获取指定名称的输出 / Get output by name
        /// </summary>
        /// <param name="name">输出名称 / Output name</param>
        /// <returns>输出节点描述 / Output node description</returns>
        public NodeOutput get_output_by_name(string name)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Parameter cannot be null or empty", nameof(name));
            
            IntPtr node_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_model_output_by_name(_ptr, name, ref node_ptr));
            return new NodeOutput(node_ptr);
        }

        /// <summary>
        /// 获取指定节点输出的输出 / Get output by node output
        /// </summary>
        /// <param name="output_port">输出端口 / Output port</param>
        /// <returns>输出节点描述 / Output node description</returns>
        public NodeOutput get_output_by_port(NodeOutput output_port)
        {
            ThrowIfDisposed();
            if (output_port == null)
                throw new ArgumentNullException(nameof(output_port));
            
            // ov_model_output_by_port is not available in the C API
            // Using get_output_by_name as fallback
            return get_output_by_name(output_port.get_any_name());
        }

        #endregion

        #region 模型属性 / Model Properties

        /// <summary>
        /// 获取模型的友好名称 / Get the friendly name of the model
        /// </summary>
        /// <returns>友好名称 / Friendly name</returns>
        public string get_friendly_name()
        {
            ThrowIfDisposed();
            IntPtr name_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_model_get_friendly_name(_ptr, ref name_ptr));
            string name = Marshal.PtrToStringAnsi(name_ptr) ?? string.Empty;
            ov_free(name_ptr);
            return name;
        }

        /// <summary>
        /// 检查模型是否为动态形状 / Check if model has dynamic shapes
        /// <para>如果任一输入具有动态维度，则返回true。/ Returns true if any input has dynamic dimensions.</para>
        /// </summary>
        /// <returns>是否为动态 / Whether dynamic</returns>
        public bool is_dynamic()
        {
            ThrowIfDisposed();
            ulong inputCount = get_inputs_size();
            for (ulong i = 0; i < inputCount; i++)
            {
                NodeInput input = get_input(i);
                try
                {
                    PartialShape partialShape = input.get_partial_shape();
                    if (partialShape.is_dynamic())
                        return true;
                }
                finally
                {
                    input.Dispose();
                }
            }
            return false;
        }

        #endregion

        #region 重塑功能 / Reshape Methods

        /// <summary>
        /// 重塑模型所有输入 / Reshape all inputs of the model
        /// </summary>
        /// <param name="partial_shape">新部分形状 / New partial shape</param>
        public void reshape(PartialShape partial_shape)
        {
            ThrowIfDisposed();
            if (partial_shape == null)
                throw new ArgumentNullException(nameof(partial_shape));
            
            ov_partial_shape_t nativeShape = partial_shape.ToNativeStruct();
            try
            {
                ExceptionHandler.ThrowOnError(ov_model_reshape_single_input(_ptr, nativeShape));
            }
            finally
            {
                // 释放维度数组内存 / Free dimension array memory
                if (nativeShape.dims != IntPtr.Zero)
                    Marshal.FreeHGlobal(nativeShape.dims);
            }
        }

        /// <summary>
        /// 重塑所有输入为指定形状 / Reshape all inputs to specified shape
        /// </summary>
        /// <param name="shape">新形状 / New shape</param>
        public void reshape(Shape shape)
        {
            ThrowIfDisposed();
            if (shape == null)
                throw new ArgumentNullException(nameof(shape));
            
            ulong inputCount = get_inputs_size();
            for (ulong i = 0; i < inputCount; i++)
            {
                NodeInput input = get_input(i);
                try
                {
                    ov_partial_shape_t partialShape = shape.ToPartialShapeStruct();
                    try
                    {
                        ExceptionHandler.ThrowOnError(
                            ov_model_reshape_input_by_name(_ptr, input.get_any_name(), partialShape));
                    }
                    finally
                    {
                        // 释放维度数组内存 / Free dimension array memory
                        if (partialShape.dims != IntPtr.Zero)
                            Marshal.FreeHGlobal(partialShape.dims);
                    }
                }
                finally
                {
                    input.Dispose();
                }
            }
        }

        /// <summary>
        /// 重塑单个输入 / Reshape single input
        /// </summary>
        /// <param name="input_name">输入名称 / Input name</param>
        /// <param name="shape">新形状 / New shape</param>
        public void reshape(string input_name, Shape shape)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(input_name))
                throw new ArgumentException("Parameter cannot be null or empty", nameof(input_name));
            if (shape == null)
                throw new ArgumentNullException(nameof(shape));
            
            ov_partial_shape_t partialShape = shape.ToPartialShapeStruct();
            try
            {
                ExceptionHandler.ThrowOnError(
                    ov_model_reshape_input_by_name(_ptr, input_name, partialShape));
            }
            finally
            {
                // 释放维度数组内存 / Free dimension array memory
                if (partialShape.dims != IntPtr.Zero)
                    Marshal.FreeHGlobal(partialShape.dims);
            }
        }

        /// <summary>
        /// 重塑单个输入（使用PartialShape）/ Reshape single input (using PartialShape)
        /// </summary>
        /// <param name="input_name">输入名称 / Input name</param>
        /// <param name="partial_shape">新部分形状 / New partial shape</param>
        public void reshape(string input_name, PartialShape partial_shape)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(input_name))
                throw new ArgumentException("Parameter cannot be null or empty", nameof(input_name));
            if (partial_shape == null)
                throw new ArgumentNullException(nameof(partial_shape));
            
            ov_partial_shape_t nativeShape = partial_shape.ToNativeStruct();
            try
            {
                ExceptionHandler.ThrowOnError(
                    ov_model_reshape_input_by_name(_ptr, input_name, nativeShape));
            }
            finally
            {
                // 释放维度数组内存 / Free dimension array memory
                if (nativeShape.dims != IntPtr.Zero)
                    Marshal.FreeHGlobal(nativeShape.dims);
            }
        }

        /// <summary>
        /// 重塑单个输入（使用维度数组）/ Reshape single input (using dimension array)
        /// </summary>
        /// <param name="input_name">输入名称 / Input name</param>
        /// <param name="dims">维度数组 / Dimension array</param>
        public void reshape(string input_name, long[] dims)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(input_name))
                throw new ArgumentException("Parameter cannot be null or empty", nameof(input_name));
            if (dims == null)
                throw new ArgumentNullException(nameof(dims));
            
            using (Shape shape = new Shape(dims))
            {
                reshape(input_name, shape);
            }
        }

        /// <summary>
        /// 批量重塑多个输入 / Batch reshape multiple inputs
        /// </summary>
        /// <param name="shapes">形状字典 / Shapes dictionary (input name -> shape)</param>
        public void reshape(Dictionary<string, Shape> shapes)
        {
            ThrowIfDisposed();
            if (shapes == null)
                throw new ArgumentNullException(nameof(shapes));
            
            foreach (var pair in shapes)
            {
                reshape(pair.Key, pair.Value);
            }
        }

        #endregion

        /// <summary>
        /// 获取原生指针（兼容属性）/ Get native pointer (compatibility property)
        /// </summary>
        public IntPtr Ptr => OvPtr;
    }
}
