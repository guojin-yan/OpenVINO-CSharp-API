using OpenVinoSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.preprocess
{
    /// <summary>
    /// Information about user's input tensor. By default, it will be initialized to same data (type/shape/etc) as
    /// model's input parameter. User application can override particular parameters (like 'element_type') according to
    /// application's data and specify appropriate conversions in pre-processing steps
    /// </summary>
    public class InputTensorInfo
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
                System.Diagnostics.Debug.WriteLine("InputTensorInfo init error : ptr is null!");
                return;
            }
            this.m_ptr = ptr;
        }
        /// <summary>
        /// Default destructor
        /// </summary>
        ~InputTensorInfo() { dispose(); }
        /// <summary>
        /// Release unmanaged resources
        /// </summary>
        public void dispose()
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
        public InputTensorInfo set_color_format(ColorFormat format)
        {
            ExceptionStatus status = NativeMethods.ov_preprocess_input_tensor_info_set_color_format(
                m_ptr, (uint)format);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InputTensorInfo set_color_format error : {0}!", status.ToString());
            }
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="sub_names_size"></param>
        /// <returns>Reference to 'this' to allow chaining with other calls in a builder-like manner.</returns>
        public InputTensorInfo set_color_format(ColorFormat format, ulong sub_names_size)
        {
            ExceptionStatus status = NativeMethods.ov_preprocess_input_tensor_info_set_color_format_with_subname(
                m_ptr, (uint)format, sub_names_size);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InputTensorInfo set_color_format error : {0}!", status.ToString());
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
            ExceptionStatus status = NativeMethods.ov_preprocess_input_tensor_info_set_element_type(
                m_ptr, (uint)type.get_type());
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InputTensorInfo set_element_type error : {0}!", status.ToString());
            }
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
            ExceptionStatus status = NativeMethods.ov_preprocess_input_tensor_info_set_spatial_static_shape(
                m_ptr, input_height, input_width);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InputTensorInfo set_shape error : {0}!", status.ToString());
            }
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
            ExceptionStatus status = NativeMethods.ov_preprocess_input_tensor_info_set_memory_type(
                m_ptr, ref c_mem_type[0]);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InputTensorInfo set_shape error : {0}!", status.ToString());
            }
            return this;
        }

        /// <summary>
        /// Set layout for user's input tensor
        /// </summary>
        /// <param name="layout">Layout for user's input tensor.</param>
        /// <returns>Reference to 'this' to allow chaining with other calls in a builder-like manner.</returns>
        public InputTensorInfo set_layout(Layout layout)
        {
            ExceptionStatus status = NativeMethods.ov_preprocess_input_tensor_info_set_layout(
                m_ptr, layout.Ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InputTensorInfo set_layout error : {0}!", status.ToString());
            }
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
            ExceptionStatus status = NativeMethods.ov_preprocess_input_tensor_info_set_from(
                m_ptr, runtime_tensor.Ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InputTensorInfo set_from error : {0}!", status.ToString());
            }
            return this;
        }
    }
}
