using OpenVinoSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.preprocess
{
    /// <summary>
    /// Information about user's input tensor. By default, it will be initialized to same data (type/shape/etc) as
    /// model's input parameter. User application can override particular parameters (like 'element_type') according to
    /// application's data and specify appropriate conversions in pre-processing steps
    /// </summary>
    public class InputTensorInfo : IDisposable
    {
        /// <summary>
        /// [private]InputTensorInfo class pointer.
        /// </summary>
        public IntPtr m_ptr = IntPtr.Zero;

        /// <summary>
        /// [public]InputTensorInfo class pointer.
        /// </summary>
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }

        /// <summary>
        /// Default construction through InputTensorInfo pointer.
        /// </summary>
        /// <param name="ptr">InputTensorInfo pointer.</param>
        public InputTensorInfo(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                HandleException.handler(ExceptionStatus.PTR_NULL);
                return;
            }
            this.m_ptr = ptr;
        }
        /// <summary>
        /// Default destructor
        /// </summary>
        ~InputTensorInfo() { Dispose(); }
        /// <summary>
        /// Release unmanaged resources
        /// </summary>
        public void Dispose()
        {
            if (m_ptr == IntPtr.Zero)
            {
                return;
            }
            NativeMethods.ov_preprocess_input_tensor_info_free(m_ptr);
            
            m_ptr = IntPtr.Zero;
        }
        /// <summary>
        /// Set color format for user's input tensor.
        /// </summary>
        /// <remarks>
        /// In general way, some formats support multi-plane input, e.g. NV12 image can be represented as 2 separate tensors
        /// (planes): Y plane and UV plane. set_color_format API also allows to set sub_names for such parameters for
        /// convenient usage of plane parameters. During build stage, new parameters for each plane will be inserted to the
        /// place of original parameter. This means that all parameters located after will shift their positions accordingly
        /// (e.g. {param1, param2} will become {param1/Y, param1/UV, param2})
        /// </remarks>
        /// <param name="format">Color format of input image.</param>
        /// <returns>Reference to 'this' to allow chaining with other calls in a builder-like manner.</returns>
        public InputTensorInfo set_color_format(ColorFormat format, params string[] properties)
        {
            IntPtr[] p = new IntPtr[properties.Length];
            for (int i = 0; i < properties.Length; ++i) 
            {
                p[i] = Marshal.StringToHGlobalAnsi(properties[i]);
            }
            switch (p.Length) 
            {
                case 0:
                    HandleException.handler(NativeMethods.ov_preprocess_input_tensor_info_set_color_format(m_ptr, (uint)format));
                    break;
                case 1: 
                    NativeMethods.ov_preprocess_input_tensor_info_set_color_format_with_subname(m_ptr, (uint)format, (ulong)properties.Length, p[0]);
                    break;
                case 2:
                    NativeMethods.ov_preprocess_input_tensor_info_set_color_format_with_subname(m_ptr, (uint)format, (ulong)properties.Length, p[0], p[1]);
                    break;
                case 3:
                    NativeMethods.ov_preprocess_input_tensor_info_set_color_format_with_subname(m_ptr, (uint)format, (ulong)properties.Length, p[0], p[1], p[2]);
                    break;
                case 4:
                    NativeMethods.ov_preprocess_input_tensor_info_set_color_format_with_subname(m_ptr, (uint)format, (ulong)properties.Length, p[0], p[1], p[2], p[3]);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Properties count > 4 not supported");

            }
            return this;
        }

        /// <summary>
        /// Set element type for user's input tensor
        /// </summary>
        /// <param name="type">Element type for user's input tensor.</param>
        /// <returns>Reference to 'this' to allow chaining with other calls in a builder-like manner.</returns>
        public InputTensorInfo set_element_type(OvType type)
        {
            HandleException.handler(
                NativeMethods.ov_preprocess_input_tensor_info_set_element_type(m_ptr, (uint)type.get_type()));
            return this;
        }

        /// <summary>
        ///  By default, input image shape is inherited from model input shape. Use this method to specify different
        ///  width and height of user's input image. In case if input image size is not known, use
        /// `set_spatial_dynamic_shape` method.
        /// </summary>
        /// <param name="input_height">Set fixed user's input image height.</param>
        /// <param name="input_width">Set fixed user's input image width.</param>
        /// <returns>Reference to 'this' to allow chaining with other calls in a builder-like manner.</returns>
        public InputTensorInfo set_spatial_static_shape(ulong input_height, ulong input_width)
        {
            HandleException.handler(
                NativeMethods.ov_preprocess_input_tensor_info_set_spatial_static_shape(m_ptr, input_height, input_width));
            return this;
        }

        /// <summary>
        /// Set memory type runtime information for user's input tensor
        /// </summary>
        /// <param name="memory_type">Memory type. Refer to specific plugin's documentation for exact string format</param>
        /// <returns>Reference to 'this' to allow chaining with other calls in a builder-like manner.</returns>
        public InputTensorInfo set_memory_type(string memory_type)
        {
            sbyte[] c_mem_type = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(memory_type));
            HandleException.handler(
                NativeMethods.ov_preprocess_input_tensor_info_set_memory_type(m_ptr, ref c_mem_type[0]));
            return this;
        }

        /// <summary>
        /// Set layout for user's input tensor
        /// </summary>
        /// <param name="layout">Layout for user's input tensor.</param>
        /// <returns>Reference to 'this' to allow chaining with other calls in a builder-like manner.</returns>
        public InputTensorInfo set_layout(Layout layout)
        {
            HandleException.handler(
                NativeMethods.ov_preprocess_input_tensor_info_set_layout(m_ptr, layout.Ptr));
            return this;
        }


        /// <summary>
        /// Helper function to reuse element type and shape from user's created tensor. Use this only in case if
        /// input tensor is already known and available before. Overwrites previously set element type & shape via
        /// `set_element_type` and `set_shape`. Tensor's memory type is not reused, so if `runtime_tensor` represents remote
        /// tensor with particular memory type - you should still specify appropriate memory type manually using
        /// `set_memory_type`
        /// </summary>
        /// <remarks> 
        /// As for `InputTensorInfo::set_shape`, this method shall not be used together with methods
        /// 'set_spatial_dynamic_shape' and 'set_spatial_static_shape', otherwise ov::AssertFailure exception will be thrown
        /// </remarks>
        /// <param name="runtime_tensor">User's created tensor.</param>
        /// <returns>Reference to 'this' to allow chaining with other calls in a builder-like manner.</returns>
        public InputTensorInfo set_from(Tensor runtime_tensor)
        {
            HandleException.handler(
                NativeMethods.ov_preprocess_input_tensor_info_set_from(m_ptr, runtime_tensor.Ptr));
            return this;
        }
    }
}
