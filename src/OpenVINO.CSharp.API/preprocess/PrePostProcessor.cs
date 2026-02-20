// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using static OpenVinoSharp.native.NativeMethods;
using OpenVinoSharp.Internal;
using OpenVinoSharp.element;

namespace OpenVinoSharp.preprocess
{
    /// <summary>
    /// PrePostProcessor class for model preprocessing
    /// </summary>
    public class PrePostProcessor : DisposableOvObject
    {
        /// <summary>
        /// Create a PrePostProcessor from model
        /// </summary>
        public PrePostProcessor(Model model) : base()
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            ExceptionHandler.ThrowOnError(ov_preprocess_prepostprocessor_create(model.OvPtr, ref _ptr));
        }

        /// <summary>
        /// releases unmanaged resources
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
        /// Build and apply preprocessing steps to the model.
        /// Returns a new model with preprocessing applied.
        /// </summary>
        public Model build()
        {
            ThrowIfDisposed();
            IntPtr model_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_preprocess_prepostprocessor_build(_ptr, ref model_ptr));
            return new Model(model_ptr);
        }

        /// <summary>
        /// Get input info
        /// </summary>
        public InputInfo get_input_info()
        {
            ThrowIfDisposed();
            IntPtr info_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_preprocess_prepostprocessor_get_input_info(_ptr, ref info_ptr));
            return new InputInfo(info_ptr);
        }

        /// <summary>
        /// Get input info by name
        /// </summary>
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
        /// Get input info by index
        /// </summary>
        public InputInfo get_input_info_by_index(ulong tensor_index)
        {
            ThrowIfDisposed();
            IntPtr info_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_preprocess_prepostprocessor_get_input_info_by_index(_ptr, tensor_index, ref info_ptr));
            return new InputInfo(info_ptr);
        }

        /// <summary>
        /// Get output info
        /// </summary>
        public OutputInfo get_output_info()
        {
            ThrowIfDisposed();
            IntPtr info_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_preprocess_prepostprocessor_get_output_info(_ptr, ref info_ptr));
            return new OutputInfo(info_ptr);
        }

        /// <summary>
        /// Get output info by index
        /// </summary>
        public OutputInfo get_output_info_by_index(ulong tensor_index)
        {
            ThrowIfDisposed();
            IntPtr info_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_preprocess_prepostprocessor_get_output_info_by_index(_ptr, tensor_index, ref info_ptr));
            return new OutputInfo(info_ptr);
        }

        /// <summary>
        /// Get output info by name
        /// </summary>
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
    /// Input info for preprocessing
    /// </summary>
    public class InputInfo : DisposableOvObject
    {
        public InputInfo(IntPtr ptr) : base(ptr) { }

        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero && IsEnabledDispose)
            {
                ov_preprocess_input_info_free(_ptr);
            }
            base.DisposeUnmanaged();
        }

        /// <summary>
        /// Get tensor info
        /// </summary>
        public InputTensorInfo get_tensor_info()
        {
            ThrowIfDisposed();
            IntPtr info_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_preprocess_input_info_get_tensor_info(_ptr, ref info_ptr));
            return new InputTensorInfo(info_ptr);
        }

        /// <summary>
        /// Get preprocess steps
        /// </summary>
        public PreprocessSteps get_preprocess_steps()
        {
            ThrowIfDisposed();
            IntPtr steps_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_preprocess_input_info_get_preprocess_steps(_ptr, ref steps_ptr));
            return new PreprocessSteps(steps_ptr);
        }

        /// <summary>
        /// Get model info
        /// </summary>
        public InputModelInfo get_model_info()
        {
            ThrowIfDisposed();
            IntPtr info_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_preprocess_input_info_get_model_info(_ptr, ref info_ptr));
            return new InputModelInfo(info_ptr);
        }
    }

    /// <summary>
    /// Input tensor info
    /// </summary>
    public class InputTensorInfo : DisposableOvObject
    {
        public InputTensorInfo(IntPtr ptr) : base(ptr) { }

        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero && IsEnabledDispose)
            {
                ov_preprocess_input_tensor_info_free(_ptr);
            }
            base.DisposeUnmanaged();
        }

        /// <summary>
        /// Set element type
        /// </summary>
        public void set_element_type(ElementType element_type)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(ov_preprocess_input_tensor_info_set_element_type(_ptr, (uint)element_type));
        }

        /// <summary>
        /// Set color format
        /// </summary>
        public void set_color_format(ColorFormat color_format)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(ov_preprocess_input_tensor_info_set_color_format(_ptr, (uint)color_format));
        }

        /// <summary>
        /// Set layout
        /// </summary>
        public void set_layout(Layout layout)
        {
            ThrowIfDisposed();
            if (layout == null) throw new ArgumentNullException(nameof(layout));
            ExceptionHandler.ThrowOnError(ov_preprocess_input_tensor_info_set_layout(_ptr, layout.OvPtr));
        }

        /// <summary>
        /// Set spatial static shape
        /// </summary>
        public void set_spatial_static_shape(ulong height, ulong width)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(ov_preprocess_input_tensor_info_set_spatial_static_shape(_ptr, height, width));
        }

        /// <summary>
        /// Set color format with sub names
        /// </summary>
        /// <param name="color_format">Color format</param>
        /// <param name="sub_names">Sub names for each plane</param>
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
        /// Set memory type (for GPU/DirectX etc.)
        /// </summary>
        /// <param name="mem_type">Memory type string</param>
        public void set_memory_type(string mem_type)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(mem_type))
                throw new ArgumentNullException(nameof(mem_type));

            ExceptionHandler.ThrowOnError(ov_preprocess_input_tensor_info_set_memory_type(_ptr, mem_type));
        }

        /// <summary>
        /// Set from existing tensor
        /// </summary>
        /// <param name="tensor">Source tensor</param>
        public void set_from(Tensor tensor)
        {
            ThrowIfDisposed();
            if (tensor == null) throw new ArgumentNullException(nameof(tensor));

            ExceptionHandler.ThrowOnError(ov_preprocess_input_tensor_info_set_from(_ptr, tensor.OvPtr));
        }
    }

    /// <summary>
    /// Preprocess steps
    /// </summary>
    public class PreprocessSteps : DisposableOvObject
    {
        public PreprocessSteps(IntPtr ptr) : base(ptr) { }

        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero && IsEnabledDispose)
            {
                ov_preprocess_preprocess_steps_free(_ptr);
            }
            base.DisposeUnmanaged();
        }

        /// <summary>
        /// Add resize operation
        /// </summary>
        public void resize(ResizeAlgorithm algorithm)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(ov_preprocess_preprocess_steps_resize(_ptr, (uint)algorithm));
        }

        /// <summary>
        /// Add scale operation
        /// </summary>
        public void scale(float value)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(ov_preprocess_preprocess_steps_scale(_ptr, value));
        }

        /// <summary>
        /// Add mean operation
        /// </summary>
        public void mean(float value)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(ov_preprocess_preprocess_steps_mean(_ptr, value));
        }

        /// <summary>
        /// Add crop operation
        /// </summary>
        public void crop(int[] begin, int[] end)
        {
            ThrowIfDisposed();
            if (begin == null) throw new ArgumentNullException(nameof(begin));
            if (end == null) throw new ArgumentNullException(nameof(end));

            ExceptionHandler.ThrowOnError(ov_preprocess_preprocess_steps_crop(_ptr, begin, begin.Length, end, end.Length));
        }

        /// <summary>
        /// Add convert layout operation
        /// </summary>
        public void convert_layout(Layout layout)
        {
            ThrowIfDisposed();
            if (layout == null) throw new ArgumentNullException(nameof(layout));
            ExceptionHandler.ThrowOnError(ov_preprocess_preprocess_steps_convert_layout(_ptr, layout.OvPtr));
        }

        /// <summary>
        /// Add convert element type operation
        /// </summary>
        public void convert_element_type(ElementType element_type)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(ov_preprocess_preprocess_steps_convert_element_type(_ptr, (uint)element_type));
        }

        /// <summary>
        /// Add convert color operation
        /// </summary>
        public void convert_color(ColorFormat color_format)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(ov_preprocess_preprocess_steps_convert_color(_ptr, (uint)color_format));
        }

        /// <summary>
        /// Reverse channels
        /// </summary>
        public void reverse_channels()
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(ov_preprocess_preprocess_steps_reverse_channels(_ptr));
        }

        /// <summary>
        /// Add scale operation for multiple channels
        /// </summary>
        /// <param name="values">Scaling values for each channel</param>
        public void scale_multi_channels(float[] values)
        {
            ThrowIfDisposed();
            if (values == null) throw new ArgumentNullException(nameof(values));

            ExceptionHandler.ThrowOnError(
                ov_preprocess_preprocess_steps_scale_multi_channels(_ptr, values, values.Length));
        }

        /// <summary>
        /// Add mean operation for multiple channels
        /// </summary>
        /// <param name="values">Mean values for each channel</param>
        public void mean_multi_channels(float[] values)
        {
            ThrowIfDisposed();
            if (values == null) throw new ArgumentNullException(nameof(values));

            ExceptionHandler.ThrowOnError(
                ov_preprocess_preprocess_steps_mean_multi_channels(_ptr, values, values.Length));
        }

        /// <summary>
        /// Add pad operation
        /// </summary>
        /// <param name="pads_begin">Number of padding elements to add at the beginning of each axis</param>
        /// <param name="pads_end">Number of padding elements to add at the end of each axis</param>
        /// <param name="value">Value to be populated in the padded area (for CONSTANT mode)</param>
        /// <param name="mode">Padding mode</param>
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
    /// Input model info
    /// </summary>
    public class InputModelInfo : DisposableOvObject
    {
        public InputModelInfo(IntPtr ptr) : base(ptr) { }

        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero && IsEnabledDispose)
            {
                ov_preprocess_input_model_info_free(_ptr);
            }
            base.DisposeUnmanaged();
        }

        /// <summary>
        /// Set layout
        /// </summary>
        public void set_layout(Layout layout)
        {
            ThrowIfDisposed();
            if (layout == null) throw new ArgumentNullException(nameof(layout));
            ExceptionHandler.ThrowOnError(ov_preprocess_input_model_info_set_layout(_ptr, layout.OvPtr));
        }
    }

    /// <summary>
    /// Output info for postprocessing
    /// </summary>
    public class OutputInfo : DisposableOvObject
    {
        public OutputInfo(IntPtr ptr) : base(ptr) { }

        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero && IsEnabledDispose)
            {
                ov_preprocess_output_info_free(_ptr);
            }
            base.DisposeUnmanaged();
        }

        /// <summary>
        /// Get tensor info
        /// </summary>
        public OutputTensorInfo get_tensor_info()
        {
            ThrowIfDisposed();
            IntPtr info_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_preprocess_output_info_get_tensor_info(_ptr, ref info_ptr));
            return new OutputTensorInfo(info_ptr);
        }
    }

    /// <summary>
    /// Output tensor info
    /// </summary>
    public class OutputTensorInfo : DisposableOvObject
    {
        public OutputTensorInfo(IntPtr ptr) : base(ptr) { }

        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero && IsEnabledDispose)
            {
                ov_preprocess_output_tensor_info_free(_ptr);
            }
            base.DisposeUnmanaged();
        }

        /// <summary>
        /// Set element type
        /// </summary>
        public void set_element_type(ElementType element_type)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(ov_preprocess_output_set_element_type(_ptr, (uint)element_type));
        }
    }
}
