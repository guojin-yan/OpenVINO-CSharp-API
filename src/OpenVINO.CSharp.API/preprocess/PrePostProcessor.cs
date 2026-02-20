// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using System.Runtime.InteropServices;
using static OpenVinoSharp.native.NativeMethods;
using OpenVinoSharp.Internal;
using OpenVinoSharp.element;

namespace OpenVinoSharp.preprocess
{
    /// <summary>
    /// 预处理和后处理器类 / PrePostProcessor class for model preprocessing and postprocessing
    /// <para>用于配置模型输入的预处理步骤（如归一化、裁剪、颜色格式转换等）和输出后处理。/ Used to configure preprocessing steps for model inputs (normalization, crop, color format conversion, etc.) and output postprocessing.</para>
    /// </summary>
    /// <example>
    /// 使用示例 / Usage example:
    /// <code>
    /// using var core = new Core();
    /// using var model = core.read_model("model.xml");
    /// using var ppp = new PrePostProcessor(model);
    /// 
    /// // 配置输入预处理 / Configure input preprocessing
    /// var inputInfo = ppp.get_input_info();
    /// inputInfo.tensor().set_element_type(ElementType.F32);
    /// inputInfo.preprocess().scale(255.0f);
    /// 
    /// // 构建并应用 / Build and apply
    /// using var newModel = ppp.build();
    /// </code>
    /// </example>
    public class PrePostProcessor : DisposableOvObject
    {
        /// <summary>
        /// 从模型创建 PrePostProcessor 实例 / Create a PrePostProcessor from model
        /// </summary>
        /// <param name="model">输入模型 / Input model</param>
        /// <exception cref="ArgumentNullException">当 model 为 null 时抛出 / Thrown when model is null</exception>
        public PrePostProcessor(Model model) : base()
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            ExceptionHandler.ThrowOnError(ov_preprocess_prepostprocessor_create(model.OvPtr, ref _ptr));
        }

        /// <summary>
        /// 释放非托管资源 / Releases unmanaged resources
        /// </summary>
        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero && IsEnabledDispose)
            {
                ov_preprocess_prepostprocessor_free(_ptr);
            }
            base.DisposeUnmanaged();
        }

        /// <summary>
        /// 构建并应用预处理步骤到模型，返回应用了预处理的新模型 / Build and apply preprocessing steps to the model, returns a new model with preprocessing applied
        /// </summary>
        /// <returns>应用了预处理的新模型 / New model with preprocessing applied</returns>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// using var ppp = new PrePostProcessor(model);
        /// // ... 配置预处理步骤 / Configure preprocessing steps ...
        /// using var newModel = ppp.build();
        /// </code>
        /// </example>
        public Model build()
        {
            ThrowIfDisposed();
            IntPtr model_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_preprocess_prepostprocessor_build(_ptr, ref model_ptr));
            return new Model(model_ptr);
        }

        /// <summary>
        /// 获取输入信息 / Get input info
        /// </summary>
        /// <returns>输入信息对象 / Input info object</returns>
        /// <remarks>适用于单输入模型 / Suitable for single-input models</remarks>
        public InputInfo get_input_info()
        {
            ThrowIfDisposed();
            IntPtr info_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_preprocess_prepostprocessor_get_input_info(_ptr, ref info_ptr));
            return new InputInfo(info_ptr);
        }

        /// <summary>
        /// 根据名称获取输入信息 / Get input info by name
        /// </summary>
        /// <param name="tensor_name">输入张量名称 / Input tensor name</param>
        /// <returns>输入信息对象 / Input info object</returns>
        /// <exception cref="ArgumentNullException">当 tensor_name 为空或 null 时抛出 / Thrown when tensor_name is empty or null</exception>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// var inputInfo = ppp.get_input_info_by_name("input_image");
        /// </code>
        /// </example>
        public InputInfo get_input_info_by_name(string tensor_name)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(tensor_name))
                throw new ArgumentNullException(nameof(tensor_name));

            IntPtr info_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_preprocess_prepostprocessor_get_input_info_by_name(_ptr, tensor_name, ref info_ptr));
            return new InputInfo(info_ptr);
        }

        /// <summary>
        /// 根据索引获取输入信息 / Get input info by index
        /// </summary>
        /// <param name="tensor_index">输入张量索引 / Input tensor index</param>
        /// <returns>输入信息对象 / Input info object</returns>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// var inputInfo = ppp.get_input_info_by_index(0);
        /// </code>
        /// </example>
        public InputInfo get_input_info_by_index(ulong tensor_index)
        {
            ThrowIfDisposed();
            IntPtr info_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_preprocess_prepostprocessor_get_input_info_by_index(_ptr, tensor_index, ref info_ptr));
            return new InputInfo(info_ptr);
        }

        /// <summary>
        /// 获取输出信息 / Get output info
        /// </summary>
        /// <returns>输出信息对象 / Output info object</returns>
        /// <remarks>适用于单输出模型 / Suitable for single-output models</remarks>
        public OutputInfo get_output_info()
        {
            ThrowIfDisposed();
            IntPtr info_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_preprocess_prepostprocessor_get_output_info(_ptr, ref info_ptr));
            return new OutputInfo(info_ptr);
        }

        /// <summary>
        /// 根据索引获取输出信息 / Get output info by index
        /// </summary>
        /// <param name="tensor_index">输出张量索引 / Output tensor index</param>
        /// <returns>输出信息对象 / Output info object</returns>
        public OutputInfo get_output_info_by_index(ulong tensor_index)
        {
            ThrowIfDisposed();
            IntPtr info_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_preprocess_prepostprocessor_get_output_info_by_index(_ptr, tensor_index, ref info_ptr));
            return new OutputInfo(info_ptr);
        }

        /// <summary>
        /// 根据名称获取输出信息 / Get output info by name
        /// </summary>
        /// <param name="tensor_name">输出张量名称 / Output tensor name</param>
        /// <returns>输出信息对象 / Output info object</returns>
        /// <exception cref="ArgumentNullException">当 tensor_name 为空或 null 时抛出 / Thrown when tensor_name is empty or null</exception>
        public OutputInfo get_output_info_by_name(string tensor_name)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(tensor_name))
                throw new ArgumentNullException(nameof(tensor_name));

            IntPtr info_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_preprocess_prepostprocessor_get_output_info_by_name(_ptr, tensor_name, ref info_ptr));
            return new OutputInfo(info_ptr);
        }
    }

    /// <summary>
    /// 输入信息类，用于配置输入预处理 / Input info class for preprocessing configuration
    /// <para>提供对输入张量信息、预处理步骤和模型信息的访问。/ Provides access to input tensor info, preprocessing steps, and model info.</para>
    /// </summary>
    /// <example>
    /// 使用示例 / Usage example:
    /// <code>
    /// var inputInfo = ppp.get_input_info();
    /// inputInfo.tensor().set_layout("NCHW");
    /// inputInfo.preprocess().resize(ResizeAlgorithm.RESIZE_LINEAR);
    /// </code>
    /// </example>
    public class InputInfo : DisposableOvObject
    {
        /// <summary>
        /// 创建 InputInfo 实例 / Create InputInfo instance
        /// </summary>
        /// <param name="ptr">非托管对象指针 / Unmanaged object pointer</param>
        public InputInfo(IntPtr ptr) : base(ptr) { }

        /// <summary>
        /// 释放非托管资源 / Releases unmanaged resources
        /// </summary>
        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero && IsEnabledDispose)
            {
                ov_preprocess_input_info_free(_ptr);
            }
            base.DisposeUnmanaged();
        }

        /// <summary>
        /// 获取张量信息 / Get tensor info
        /// </summary>
        /// <returns>输入张量信息对象 / Input tensor info object</returns>
        /// <remarks>用于配置输入张量的属性，如元素类型、颜色格式、布局等 / Used to configure input tensor properties like element type, color format, layout, etc.</remarks>
        public InputTensorInfo get_tensor_info()
        {
            ThrowIfDisposed();
            IntPtr info_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_preprocess_input_info_get_tensor_info(_ptr, ref info_ptr));
            return new InputTensorInfo(info_ptr);
        }

        /// <summary>
        /// 获取预处理步骤 / Get preprocess steps
        /// </summary>
        /// <returns>预处理步骤对象 / Preprocessing steps object</returns>
        /// <remarks>用于添加预处理操作，如缩放、裁剪、颜色转换等 / Used to add preprocessing operations like scale, crop, color conversion, etc.</remarks>
        public PreprocessSteps get_preprocess_steps()
        {
            ThrowIfDisposed();
            IntPtr steps_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_preprocess_input_info_get_preprocess_steps(_ptr, ref steps_ptr));
            return new PreprocessSteps(steps_ptr);
        }

        /// <summary>
        /// 获取模型信息 / Get model info
        /// </summary>
        /// <returns>输入模型信息对象 / Input model info object</returns>
        /// <remarks>用于配置模型的输入布局 / Used to configure the model's input layout</remarks>
        public InputModelInfo get_model_info()
        {
            ThrowIfDisposed();
            IntPtr info_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_preprocess_input_info_get_model_info(_ptr, ref info_ptr));
            return new InputModelInfo(info_ptr);
        }
    }

    /// <summary>
    /// 输入张量信息类 / Input tensor info class
    /// <para>用于配置输入张量的属性，包括元素类型、颜色格式、布局、内存类型等。/ Used to configure input tensor properties including element type, color format, layout, memory type, etc.</para>
    /// </summary>
    /// <example>
    /// 使用示例 / Usage example:
    /// <code>
    /// var tensorInfo = inputInfo.get_tensor_info();
    /// tensorInfo.set_element_type(ElementType.F32);
    /// tensorInfo.set_color_format(ColorFormat.RGB);
    /// tensorInfo.set_layout(new Layout("NHWC"));
    /// </code>
    /// </example>
    public class InputTensorInfo : DisposableOvObject
    {
        /// <summary>
        /// 创建 InputTensorInfo 实例 / Create InputTensorInfo instance
        /// </summary>
        /// <param name="ptr">非托管对象指针 / Unmanaged object pointer</param>
        public InputTensorInfo(IntPtr ptr) : base(ptr) { }

        /// <summary>
        /// 释放非托管资源 / Releases unmanaged resources
        /// </summary>
        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero && IsEnabledDispose)
            {
                ov_preprocess_input_tensor_info_free(_ptr);
            }
            base.DisposeUnmanaged();
        }

        /// <summary>
        /// 设置元素类型 / Set element type
        /// </summary>
        /// <param name="element_type">元素类型 / Element type (e.g., ElementType.F32, ElementType.U8)</param>
        /// <remarks>定义输入张量的数据类型 / Defines the data type of the input tensor</remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// tensorInfo.set_element_type(ElementType.F32);
        /// </code>
        /// </example>
        public void set_element_type(ElementType element_type)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(ov_preprocess_input_tensor_info_set_element_type(_ptr, (uint)element_type));
        }

        /// <summary>
        /// 设置颜色格式 / Set color format
        /// </summary>
        /// <param name="color_format">颜色格式 / Color format (e.g., ColorFormat.RGB, ColorFormat.BGR)</param>
        /// <remarks>定义输入图像的颜色格式 / Defines the color format of the input image</remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// tensorInfo.set_color_format(ColorFormat.RGB);
        /// </code>
        /// </example>
        public void set_color_format(ColorFormat color_format)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(ov_preprocess_input_tensor_info_set_color_format(_ptr, (uint)color_format));
        }

        /// <summary>
        /// 设置布局 / Set layout
        /// </summary>
        /// <param name="layout">张量布局 / Tensor layout (e.g., "NCHW", "NHWC")</param>
        /// <exception cref="ArgumentNullException">当 layout 为 null 时抛出 / Thrown when layout is null</exception>
        /// <remarks>定义输入张量的维度顺序 / Defines the dimension order of the input tensor</remarks>
        public void set_layout(Layout layout)
        {
            ThrowIfDisposed();
            if (layout == null) throw new ArgumentNullException(nameof(layout));
            ExceptionHandler.ThrowOnError(ov_preprocess_input_tensor_info_set_layout(_ptr, layout.OvPtr));
        }

        /// <summary>
        /// 设置静态空间形状（高度和宽度）/ Set spatial static shape (height and width)
        /// </summary>
        /// <param name="height">图像高度 / Image height</param>
        /// <param name="width">图像宽度 / Image width</param>
        /// <remarks>定义输入图像的固定尺寸 / Defines the fixed dimensions of the input image</remarks>
        public void set_spatial_static_shape(ulong height, ulong width)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(ov_preprocess_input_tensor_info_set_spatial_static_shape(_ptr, height, width));
        }

        /// <summary>
        /// 设置颜色格式，带有子名称 / Set color format with sub names
        /// </summary>
        /// <param name="color_format">颜色格式 / Color format</param>
        /// <param name="sub_names">每个平面的子名称数组 / Sub names array for each plane</param>
        /// <remarks>用于多平面颜色格式，如 NV12、YUV 等 / Used for multi-plane color formats like NV12, YUV, etc.</remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// tensorInfo.set_color_format(ColorFormat.NV12, new[] { "y", "uv" });
        /// </code>
        /// </example>
        public void set_color_format(ColorFormat color_format, string[] sub_names)
        {
            ThrowIfDisposed();
            if (sub_names == null || sub_names.Length == 0)
            {
                ExceptionHandler.ThrowOnError(ov_preprocess_input_tensor_info_set_color_format(_ptr, (uint)color_format));
                return;
            }

            // Convert string array to unmanaged memory
            IntPtr[] ptrArray = new IntPtr[sub_names.Length];
            GCHandle[] handles = new GCHandle[sub_names.Length];
            try
            {
                for (int i = 0; i < sub_names.Length; i++)
                {
                    handles[i] = GCHandle.Alloc(System.Text.Encoding.ASCII.GetBytes(sub_names[i] + '\0'), GCHandleType.Pinned);
                    ptrArray[i] = handles[i].AddrOfPinnedObject();
                }

                GCHandle arrayHandle = GCHandle.Alloc(ptrArray, GCHandleType.Pinned);
                try
                {
                    ExceptionHandler.ThrowOnError(
                        ov_preprocess_input_tensor_info_set_color_format_with_subname(
                            _ptr, (uint)color_format, (ulong)sub_names.Length, arrayHandle.AddrOfPinnedObject()));
                }
                finally
                {
                    arrayHandle.Free();
                }
            }
            finally
            {
                for (int i = 0; i < handles.Length; i++)
                {
                    if (handles[i].IsAllocated)
                        handles[i].Free();
                }
            }
        }

        /// <summary>
        /// 设置内存类型（用于 GPU/DirectX 等）/ Set memory type (for GPU/DirectX etc.)
        /// </summary>
        /// <param name="mem_type">内存类型字符串 / Memory type string (e.g., "GPU", "DX11")</param>
        /// <exception cref="ArgumentNullException">当 mem_type 为空或 null 时抛出 / Thrown when mem_type is empty or null</exception>
        /// <remarks>指定输入数据的内存位置 / Specifies the memory location of the input data</remarks>
        public void set_memory_type(string mem_type)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(mem_type))
                throw new ArgumentNullException(nameof(mem_type));

            ExceptionHandler.ThrowOnError(ov_preprocess_input_tensor_info_set_memory_type(_ptr, mem_type));
        }

        /// <summary>
        /// 从现有张量设置 / Set from existing tensor
        /// </summary>
        /// <param name="tensor">源张量 / Source tensor</param>
        /// <exception cref="ArgumentNullException">当 tensor 为 null 时抛出 / Thrown when tensor is null</exception>
        /// <remarks>使用现有张量的属性配置输入 / Configures input using properties from an existing tensor</remarks>
        public void set_from(Tensor tensor)
        {
            ThrowIfDisposed();
            if (tensor == null) throw new ArgumentNullException(nameof(tensor));

            ExceptionHandler.ThrowOnError(ov_preprocess_input_tensor_info_set_from(_ptr, tensor.OvPtr));
        }
    }

    /// <summary>
    /// 预处理步骤类 / Preprocess steps class
    /// <para>用于配置输入数据的预处理操作链，包括调整大小、归一化、裁剪、颜色转换等。/ Used to configure the preprocessing operation chain for input data, including resize, normalization, crop, color conversion, etc.</para>
    /// </summary>
    /// <example>
    /// 使用示例 / Usage example:
    /// <code>
    /// var steps = inputInfo.get_preprocess_steps();
    /// steps.resize(ResizeAlgorithm.RESIZE_LINEAR);
    /// steps.scale(255.0f);
    /// steps.mean(0.5f);
    /// </code>
    /// </example>
    public class PreprocessSteps : DisposableOvObject
    {
        /// <summary>
        /// 创建 PreprocessSteps 实例 / Create PreprocessSteps instance
        /// </summary>
        /// <param name="ptr">非托管对象指针 / Unmanaged object pointer</param>
        public PreprocessSteps(IntPtr ptr) : base(ptr) { }

        /// <summary>
        /// 释放非托管资源 / Releases unmanaged resources
        /// </summary>
        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero && IsEnabledDispose)
            {
                ov_preprocess_preprocess_steps_free(_ptr);
            }
            base.DisposeUnmanaged();
        }

        /// <summary>
        /// 添加调整大小操作 / Add resize operation
        /// </summary>
        /// <param name="algorithm">调整大小算法 / Resize algorithm (e.g., RESIZE_LINEAR, RESIZE_CUBIC, RESIZE_NEAREST)</param>
        /// <remarks>将输入图像调整为模型期望的尺寸 / Resizes the input image to the model's expected dimensions</remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// steps.resize(ResizeAlgorithm.RESIZE_LINEAR);
        /// </code>
        /// </example>
        public void resize(ResizeAlgorithm algorithm)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(ov_preprocess_preprocess_steps_resize(_ptr, (uint)algorithm));
        }

        /// <summary>
        /// 添加缩放操作（单通道）/ Add scale operation (single channel)
        /// </summary>
        /// <param name="value">缩放值 / Scale value</param>
        /// <remarks>对所有通道应用相同的缩放：output = input / value / Applies the same scale to all channels: output = input / value</remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// steps.scale(255.0f); // 将 [0,255] 归一化到 [0,1] / Normalize [0,255] to [0,1]
        /// </code>
        /// </example>
        public void scale(float value)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(ov_preprocess_preprocess_steps_scale(_ptr, value));
        }

        /// <summary>
        /// 添加均值操作（单通道）/ Add mean operation (single channel)
        /// </summary>
        /// <param name="value">均值值 / Mean value</param>
        /// <remarks>对所有通道应用相同的均值减法：output = input - value / Applies the same mean subtraction to all channels: output = input - value</remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// steps.mean(0.5f);
        /// </code>
        /// </example>
        public void mean(float value)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(ov_preprocess_preprocess_steps_mean(_ptr, value));
        }

        /// <summary>
        /// 添加裁剪操作 / Add crop operation
        /// </summary>
        /// <param name="begin">裁剪起始坐标 / Crop begin coordinates [y_begin, x_begin]</param>
        /// <param name="end">裁剪结束坐标 / Crop end coordinates [y_end, x_end]</param>
        /// <exception cref="ArgumentNullException">当 begin 或 end 为 null 时抛出 / Thrown when begin or end is null</exception>
        /// <remarks>从输入图像中裁剪指定区域 / Crops a specified region from the input image</remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// steps.crop(new[] { 10, 10 }, new[] { 110, 110 }); // 裁剪 100x100 区域 / Crop 100x100 region
        /// </code>
        /// </example>
        public void crop(int[] begin, int[] end)
        {
            ThrowIfDisposed();
            if (begin == null) throw new ArgumentNullException(nameof(begin));
            if (end == null) throw new ArgumentNullException(nameof(end));

            ExceptionHandler.ThrowOnError(ov_preprocess_preprocess_steps_crop(_ptr, begin, begin.Length, end, end.Length));
        }

        /// <summary>
        /// 添加布局转换操作 / Add convert layout operation
        /// </summary>
        /// <param name="layout">目标布局 / Target layout</param>
        /// <exception cref="ArgumentNullException">当 layout 为 null 时抛出 / Thrown when layout is null</exception>
        /// <remarks>转换张量的维度顺序，如 NHWC 到 NCHW / Converts tensor dimension order, e.g., NHWC to NCHW</remarks>
        public void convert_layout(Layout layout)
        {
            ThrowIfDisposed();
            if (layout == null) throw new ArgumentNullException(nameof(layout));
            ExceptionHandler.ThrowOnError(ov_preprocess_preprocess_steps_convert_layout(_ptr, layout.OvPtr));
        }

        /// <summary>
        /// 添加元素类型转换操作 / Add convert element type operation
        /// </summary>
        /// <param name="element_type">目标元素类型 / Target element type</param>
        /// <remarks>转换张量的数据类型，如 U8 到 F32 / Converts tensor data type, e.g., U8 to F32</remarks>
        public void convert_element_type(ElementType element_type)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(ov_preprocess_preprocess_steps_convert_element_type(_ptr, (uint)element_type));
        }

        /// <summary>
        /// 添加颜色转换操作 / Add convert color operation
        /// </summary>
        /// <param name="color_format">目标颜色格式 / Target color format</param>
        /// <remarks>转换图像颜色格式，如 RGB 到 BGR / Converts image color format, e.g., RGB to BGR</remarks>
        public void convert_color(ColorFormat color_format)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(ov_preprocess_preprocess_steps_convert_color(_ptr, (uint)color_format));
        }

        /// <summary>
        /// 反转通道顺序 / Reverse channels
        /// </summary>
        /// <remarks>反转颜色通道顺序，如 RGB 变为 BGR / Reverses color channel order, e.g., RGB becomes BGR</remarks>
        public void reverse_channels()
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(ov_preprocess_preprocess_steps_reverse_channels(_ptr));
        }

        /// <summary>
        /// 添加多通道缩放操作 / Add scale operation for multiple channels
        /// </summary>
        /// <param name="values">每个通道的缩放值数组 / Scaling values array for each channel</param>
        /// <exception cref="ArgumentNullException">当 values 为 null 时抛出 / Thrown when values is null</exception>
        /// <remarks>为每个颜色通道应用不同的缩放值 / Applies different scale values for each color channel</remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// steps.scale_multi_channels(new[] { 255.0f, 255.0f, 255.0f }); // 对 RGB 分别缩放 / Scale RGB separately
        /// </code>
        /// </example>
        public void scale_multi_channels(float[] values)
        {
            ThrowIfDisposed();
            if (values == null) throw new ArgumentNullException(nameof(values));

            ExceptionHandler.ThrowOnError(
                ov_preprocess_preprocess_steps_scale_multi_channels(_ptr, values, values.Length));
        }

        /// <summary>
        /// 添加多通道均值操作 / Add mean operation for multiple channels
        /// </summary>
        /// <param name="values">每个通道的均值数组 / Mean values array for each channel</param>
        /// <exception cref="ArgumentNullException">当 values 为 null 时抛出 / Thrown when values is null</exception>
        /// <remarks>为每个颜色通道应用不同的均值 / Applies different mean values for each color channel</remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// steps.mean_multi_channels(new[] { 0.485f, 0.456f, 0.406f }); // ImageNet 均值 / ImageNet means
        /// </code>
        /// </example>
        public void mean_multi_channels(float[] values)
        {
            ThrowIfDisposed();
            if (values == null) throw new ArgumentNullException(nameof(values));

            ExceptionHandler.ThrowOnError(
                ov_preprocess_preprocess_steps_mean_multi_channels(_ptr, values, values.Length));
        }

        /// <summary>
        /// 添加填充操作 / Add pad operation
        /// </summary>
        /// <param name="pads_begin">每个轴起始位置填充元素数量 / Number of padding elements to add at the beginning of each axis</param>
        /// <param name="pads_end">每个轴结束位置填充元素数量 / Number of padding elements to add at the end of each axis</param>
        /// <param name="value">填充区域的值（CONSTANT 模式）/ Value to be populated in the padded area (for CONSTANT mode)</param>
        /// <param name="mode">填充模式 / Padding mode</param>
        /// <exception cref="ArgumentNullException">当 pads_begin 或 pads_end 为 null 时抛出 / Thrown when pads_begin or pads_end is null</exception>
        /// <remarks>在图像边缘添加填充像素 / Adds padding pixels around the image edges</remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// steps.pad(new[] { 10, 10 }, new[] { 10, 10 }, 0.0f, PaddingMode.CONSTANT);
        /// </code>
        /// </example>
        public void pad(int[] pads_begin, int[] pads_end, float value = 0.0f, PaddingMode mode = PaddingMode.CONSTANT)
        {
            ThrowIfDisposed();
            if (pads_begin == null) throw new ArgumentNullException(nameof(pads_begin));
            if (pads_end == null) throw new ArgumentNullException(nameof(pads_end));

            ExceptionHandler.ThrowOnError(
                ov_preprocess_preprocess_steps_pad(_ptr, pads_begin, (ulong)pads_begin.Length, 
                    pads_end, (ulong)pads_end.Length, value, (uint)mode));
        }
    }

    /// <summary>
    /// 输入模型信息类 / Input model info class
    /// <para>用于配置模型输入的布局。/ Used to configure the layout of model inputs.</para>
    /// </summary>
    public class InputModelInfo : DisposableOvObject
    {
        /// <summary>
        /// 创建 InputModelInfo 实例 / Create InputModelInfo instance
        /// </summary>
        /// <param name="ptr">非托管对象指针 / Unmanaged object pointer</param>
        public InputModelInfo(IntPtr ptr) : base(ptr) { }

        /// <summary>
        /// 释放非托管资源 / Releases unmanaged resources
        /// </summary>
        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero && IsEnabledDispose)
            {
                ov_preprocess_input_model_info_free(_ptr);
            }
            base.DisposeUnmanaged();
        }

        /// <summary>
        /// 设置模型输入布局 / Set layout
        /// </summary>
        /// <param name="layout">张量布局 / Tensor layout</param>
        /// <exception cref="ArgumentNullException">当 layout 为 null 时抛出 / Thrown when layout is null</exception>
        /// <remarks>定义模型期望的输入维度顺序 / Defines the expected input dimension order of the model</remarks>
        public void set_layout(Layout layout)
        {
            ThrowIfDisposed();
            if (layout == null) throw new ArgumentNullException(nameof(layout));
            ExceptionHandler.ThrowOnError(ov_preprocess_input_model_info_set_layout(_ptr, layout.OvPtr));
        }
    }

    /// <summary>
    /// 输出信息类，用于配置输出后处理 / Output info class for postprocessing configuration
    /// <para>提供对输出张量信息的访问。/ Provides access to output tensor information.</para>
    /// </summary>
    public class OutputInfo : DisposableOvObject
    {
        /// <summary>
        /// 创建 OutputInfo 实例 / Create OutputInfo instance
        /// </summary>
        /// <param name="ptr">非托管对象指针 / Unmanaged object pointer</param>
        public OutputInfo(IntPtr ptr) : base(ptr) { }

        /// <summary>
        /// 释放非托管资源 / Releases unmanaged resources
        /// </summary>
        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero && IsEnabledDispose)
            {
                ov_preprocess_output_info_free(_ptr);
            }
            base.DisposeUnmanaged();
        }

        /// <summary>
        /// 获取输出张量信息 / Get tensor info
        /// </summary>
        /// <returns>输出张量信息对象 / Output tensor info object</returns>
        /// <remarks>用于配置输出张量的属性，如元素类型 / Used to configure output tensor properties like element type</remarks>
        public OutputTensorInfo get_tensor_info()
        {
            ThrowIfDisposed();
            IntPtr info_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_preprocess_output_info_get_tensor_info(_ptr, ref info_ptr));
            return new OutputTensorInfo(info_ptr);
        }
    }

    /// <summary>
    /// 输出张量信息类 / Output tensor info class
    /// <para>用于配置输出张量的属性。/ Used to configure output tensor properties.</para>
    /// </summary>
    public class OutputTensorInfo : DisposableOvObject
    {
        /// <summary>
        /// 创建 OutputTensorInfo 实例 / Create OutputTensorInfo instance
        /// </summary>
        /// <param name="ptr">非托管对象指针 / Unmanaged object pointer</param>
        public OutputTensorInfo(IntPtr ptr) : base(ptr) { }

        /// <summary>
        /// 释放非托管资源 / Releases unmanaged resources
        /// </summary>
        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero && IsEnabledDispose)
            {
                ov_preprocess_output_tensor_info_free(_ptr);
            }
            base.DisposeUnmanaged();
        }

        /// <summary>
        /// 设置输出元素类型 / Set element type
        /// </summary>
        /// <param name="element_type">元素类型 / Element type</param>
        /// <remarks>定义输出张量的数据类型 / Defines the data type of the output tensor</remarks>
        public void set_element_type(ElementType element_type)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(ov_preprocess_output_set_element_type(_ptr, (uint)element_type));
        }
    }
}
