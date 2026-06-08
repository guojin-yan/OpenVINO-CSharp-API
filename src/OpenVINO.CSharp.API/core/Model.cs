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
            UIntPtr size = UIntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_model_inputs_size_native_size(_ptr, ref size));
            return StringUtils.FromNativeSize(size);
        }

        /// <summary>
        /// 获取输入 / Get input
        /// </summary>
        /// <returns>节点输入端口 / Node input port</returns>
        public Input input()
        {
            ThrowIfDisposed();
            return CreateInputPort(
                (ref IntPtr port) => ov_model_input(_ptr, ref port),
                (ref IntPtr constPort) => ov_model_const_input(_ptr, ref constPort));
        }

        /// <summary>
        /// 获取指定索引的输入 / Get input at specified index
        /// </summary>
        /// <param name="idx">输入索引 / Input index</param>
        /// <returns>节点输入端口 / Node input port</returns>
        public Input input(ulong idx)
        {
            return get_input(idx);
        }

        /// <summary>
        /// 获取指定名称的输入 / Get input by name
        /// </summary>
        /// <param name="name">输入名称 / Input name</param>
        /// <returns>节点输入端口 / Node input port</returns>
        public Input input(string name)
        {
            return get_input_by_name(name);
        }

        /// <summary>
        /// 获取指定索引的输入 / Get input at specified index
        /// </summary>
        /// <param name="idx">输入索引 / Input index</param>
        /// <returns>节点输入端口 / Node input port</returns>
        public Input get_input(ulong idx)
        {
            ThrowIfDisposed();
            UIntPtr nativeIndex = StringUtils.ToNativeSize(idx);
            return CreateInputPort(
                (ref IntPtr port) => ov_model_input_by_index_native_size(_ptr, nativeIndex, ref port),
                (ref IntPtr constPort) => ov_model_const_input_by_index_native_size(_ptr, nativeIndex, ref constPort));
        }

        /// <summary>
        /// 获取指定名称的输入 / Get input by name
        /// </summary>
        /// <param name="name">输入名称 / Input name</param>
        /// <returns>节点输入端口 / Node input port</returns>
        public Input get_input_by_name(string name)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Parameter cannot be null or empty", nameof(name));

            return StringUtils.WithUtf8Ptr(name, namePtr => CreateInputPort(
                (ref IntPtr port) => ov_model_input_by_name_utf8(_ptr, namePtr, ref port),
                (ref IntPtr constPort) => ov_model_const_input_by_name_utf8(_ptr, namePtr, ref constPort)));
        }
        /// <summary>
        /// Get all input of model.
        /// </summary>
        /// <returns>All input of model.</returns>
        public List<Input> inputs()
        {
            ulong input_size = get_inputs_size();
            List<Input> inputs = new List<Input>();
            for (ulong index = 0; index < input_size; ++index)
            {
                inputs.Add(get_input(index));
            }
            return inputs;
        }

        /// <summary>
        /// 获取模型输入数量 / Gets the number of model inputs.
        /// </summary>
        public ulong InputCount
        {
            get { return get_inputs_size(); }
        }

        /// <summary>
        /// 获取默认输入端口 / Gets the default input port.
        /// </summary>
        /// <returns>输入端口 / Input port.</returns>
        public Input GetInput()
        {
            return input();
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
        /// <param name="name">输入名称 / Input name.</param>
        /// <returns>输入端口 / Input port.</returns>
        public Input GetInput(string name)
        {
            return get_input_by_name(name);
        }

        /// <summary>
        /// 获取全部输入端口 / Gets all input ports.
        /// </summary>
        public List<Input> Inputs
        {
            get { return inputs(); }
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
            UIntPtr size = UIntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_model_outputs_size_native_size(_ptr, ref size));
            return StringUtils.FromNativeSize(size);
        }
        /// <summary>
        /// 获取输入 / Get input
        /// </summary>
        /// <returns>节点输入端口 / Node input port</returns>
        public Output output()
        {
            ThrowIfDisposed();
            return CreateOutputPort(
                (ref IntPtr port) => ov_model_output(_ptr, ref port),
                (ref IntPtr constPort) => ov_model_const_output(_ptr, ref constPort));
        }
        /// <summary>
        /// 获取指定索引的输出 / Get output at specified index
        /// </summary>
        /// <param name="idx">输出索引 / Output index</param>
        /// <returns>输出节点描述 / Output node description</returns>
        public Output output(ulong idx)
        {
            return get_output(idx);
        }

        /// <summary>
        /// 获取指定名称的输出 / Get output by name
        /// </summary>
        /// <param name="name">输出名称 / Output name</param>
        /// <returns>输出节点描述 / Output node description</returns>
        public Output output(string name)
        {
            return get_output_by_name(name);
        }
        /// <summary>
        /// 获取指定索引的输出 / Get output at specified index
        /// </summary>
        /// <param name="idx">输出索引 / Output index</param>
        /// <returns>输出节点描述 / Output node description</returns>
        public Output get_output(ulong idx)
        {
            ThrowIfDisposed();
            UIntPtr nativeIndex = StringUtils.ToNativeSize(idx);
            return CreateOutputPort(
                (ref IntPtr port) => ov_model_output_by_index_native_size(_ptr, nativeIndex, ref port),
                (ref IntPtr constPort) => ov_model_const_output_by_index_native_size(_ptr, nativeIndex, ref constPort));
        }

        /// <summary>
        /// 获取指定名称的输出 / Get output by name
        /// </summary>
        /// <param name="name">输出名称 / Output name</param>
        /// <returns>输出节点描述 / Output node description</returns>
        public Output get_output_by_name(string name)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Parameter cannot be null or empty", nameof(name));

            return StringUtils.WithUtf8Ptr(name, namePtr => CreateOutputPort(
                (ref IntPtr port) => ov_model_output_by_name_utf8(_ptr, namePtr, ref port),
                (ref IntPtr constPort) => ov_model_const_output_by_name_utf8(_ptr, namePtr, ref constPort)));
        }

        /// <summary>
        /// Get all output of model
        /// </summary>
        /// <returns>All output of model</returns>
        public List<Output> outputs()
        {
            ulong output_size = get_outputs_size();
            List<Output> outputs = new List<Output>();
            for (ulong index = 0; index < output_size; ++index)
            {
                outputs.Add(get_output(index));
            }
            return outputs;
        }

        /// <summary>
        /// 获取模型输出数量 / Gets the number of model outputs.
        /// </summary>
        public ulong OutputCount
        {
            get { return get_outputs_size(); }
        }

        /// <summary>
        /// 获取默认输出端口 / Gets the default output port.
        /// </summary>
        /// <returns>输出端口 / Output port.</returns>
        public Output GetOutput()
        {
            return output();
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
        /// <param name="name">输出名称 / Output name.</param>
        /// <returns>输出端口 / Output port.</returns>
        public Output GetOutput(string name)
        {
            return get_output_by_name(name);
        }

        /// <summary>
        /// 获取全部输出端口 / Gets all output ports.
        /// </summary>
        public List<Output> Outputs
        {
            get { return outputs(); }
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
            try
            {
                return StringUtils.Utf8PtrToString(name_ptr);
            }
            finally
            {
                if (name_ptr != IntPtr.Zero)
                    ov_free(name_ptr);
            }
        }

        /// <summary>
        /// 获取模型友好名称 / Gets the model friendly name.
        /// </summary>
        public string FriendlyName
        {
            get { return get_friendly_name(); }
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
                Input input = get_input(i);
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

        /// <summary>
        /// 检查模型是否包含动态形状 / Checks whether the model contains dynamic shapes.
        /// </summary>
        /// <returns>是否为动态模型 / True when the model is dynamic.</returns>
        public bool IsDynamic()
        {
            return is_dynamic();
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
                PartialShape.FreeNativeStruct(ref nativeShape);
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
                Input input = get_input(i);
                try
                {
                    ov_partial_shape_t partialShape = shape.to_partial_shape_struct();
                    try
                    {
                        string inputName = input.get_any_name();
                        ExceptionHandler.ThrowOnError(StringUtils.WithUtf8Ptr(
                            inputName,
                            namePtr => ov_model_reshape_input_by_name_utf8(_ptr, namePtr, partialShape)));
                    }
                    finally
                    {
                        PartialShape.FreeNativeStruct(ref partialShape);
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
            
            ov_partial_shape_t partialShape = shape.to_partial_shape_struct();
            try
            {
                ExceptionHandler.ThrowOnError(StringUtils.WithUtf8Ptr(
                    input_name,
                    namePtr => ov_model_reshape_input_by_name_utf8(_ptr, namePtr, partialShape)));
            }
            finally
            {
                PartialShape.FreeNativeStruct(ref partialShape);
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
                ExceptionHandler.ThrowOnError(StringUtils.WithUtf8Ptr(
                    input_name,
                    namePtr => ov_model_reshape_input_by_name_utf8(_ptr, namePtr, nativeShape)));
            }
            finally
            {
                PartialShape.FreeNativeStruct(ref nativeShape);
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

        /// <summary>
        /// 重塑模型输入 / Reshapes model inputs.
        /// </summary>
        /// <param name="partialShape">新的部分形状 / New partial shape.</param>
        public void Reshape(PartialShape partialShape)
        {
            reshape(partialShape);
        }

        /// <summary>
        /// 重塑模型输入 / Reshapes model inputs.
        /// </summary>
        /// <param name="shape">新的形状 / New shape.</param>
        public void Reshape(Shape shape)
        {
            reshape(shape);
        }

        /// <summary>
        /// 按输入名称重塑模型 / Reshapes a model input by name.
        /// </summary>
        /// <param name="inputName">输入名称 / Input name.</param>
        /// <param name="shape">新的形状 / New shape.</param>
        public void Reshape(string inputName, Shape shape)
        {
            reshape(inputName, shape);
        }

        /// <summary>
        /// 按输入名称重塑模型 / Reshapes a model input by name.
        /// </summary>
        /// <param name="inputName">输入名称 / Input name.</param>
        /// <param name="partialShape">新的部分形状 / New partial shape.</param>
        public void Reshape(string inputName, PartialShape partialShape)
        {
            reshape(inputName, partialShape);
        }

        /// <summary>
        /// 按输入名称重塑模型 / Reshapes a model input by name.
        /// </summary>
        /// <param name="inputName">输入名称 / Input name.</param>
        /// <param name="dims">维度数组 / Dimension array.</param>
        public void Reshape(string inputName, long[] dims)
        {
            reshape(inputName, dims);
        }

        /// <summary>
        /// 批量重塑多个输入 / Reshapes multiple inputs.
        /// </summary>
        /// <param name="shapes">形状字典 / Shape dictionary.</param>
        public void Reshape(Dictionary<string, Shape> shapes)
        {
            reshape(shapes);
        }

        #endregion

        /// <summary>
        /// 获取原生指针（兼容属性）/ Get native pointer (compatibility property)
        /// </summary>
        public IntPtr Ptr => OvPtr;

        private delegate ExceptionStatus PortCreator(ref IntPtr port);

        private Input CreateInputPort(PortCreator createPort, PortCreator createConstPort)
        {
            IntPtr portPtr = IntPtr.Zero;
            IntPtr constPortPtr = IntPtr.Zero;
            try
            {
                ExceptionHandler.ThrowOnError(createPort(ref portPtr));
                ExceptionHandler.ThrowOnError(createConstPort(ref constPortPtr));
                return new Input(portPtr, false, constPortPtr);
            }
            catch
            {
                if (constPortPtr != IntPtr.Zero)
                    ov_output_const_port_free(constPortPtr);
                if (portPtr != IntPtr.Zero)
                    ov_output_port_free(portPtr);
                throw;
            }
        }

        private Output CreateOutputPort(PortCreator createPort, PortCreator createConstPort)
        {
            IntPtr portPtr = IntPtr.Zero;
            IntPtr constPortPtr = IntPtr.Zero;
            try
            {
                ExceptionHandler.ThrowOnError(createPort(ref portPtr));
                ExceptionHandler.ThrowOnError(createConstPort(ref constPortPtr));
                return new Output(portPtr, false, constPortPtr);
            }
            catch
            {
                if (constPortPtr != IntPtr.Zero)
                    ov_output_const_port_free(constPortPtr);
                if (portPtr != IntPtr.Zero)
                    ov_output_port_free(portPtr);
                throw;
            }
        }
    }
}
