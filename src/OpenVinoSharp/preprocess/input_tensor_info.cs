using OpenVinoSharp.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.preprocess
{
    public class InputTensorInfo
    {
        private IntPtr m_ptr = IntPtr.Zero;
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }
        public InputTensorInfo(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                System.Diagnostics.Debug.WriteLine("InputTensorInfo init error : ptr is null!");
                return;
            }
            this.m_ptr = ptr;
        }
        ~InputTensorInfo() { dispose(); }
        public void dispose()
        {
            if (m_ptr == IntPtr.Zero)
            {
                return;
            }
            NativeMethods.ov_preprocess_input_tensor_info_free(m_ptr);
            
            m_ptr = IntPtr.Zero;
        }
        public void set_color_format(ColorFormat format)
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_input_tensor_info_set_color_format(
                m_ptr, (uint)format);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InputTensorInfo set_color_format error : {0}!", status.ToString());
            }
        }
        public void set_color_format(ColorFormat format, ulong sub_names_size)
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_input_tensor_info_set_color_format_with_subname(
                m_ptr, (uint)format, sub_names_size);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InputTensorInfo set_color_format error : {0}!", status.ToString());
            }
        }
        public void set_element_type(OvType type)
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_input_tensor_info_set_element_type(
                m_ptr, (uint)type.get_type());
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InputTensorInfo set_element_type error : {0}!", status.ToString());
            }
        }
        public void set_shape(ulong input_height, ulong input_width)
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_input_tensor_info_set_spatial_static_shape(
                m_ptr, input_height, input_width);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InputTensorInfo set_shape error : {0}!", status.ToString());
            }
        }

        public void set_memory_type(string mem_type)
        {
            sbyte[] c_mem_type = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(mem_type));
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_input_tensor_info_set_memory_type(
                m_ptr, ref c_mem_type[0]);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InputTensorInfo set_shape error : {0}!", status.ToString());
            }
        }

        public void set_layout(Layout layout)
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_input_tensor_info_set_layout(
                m_ptr, layout.Ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InputTensorInfo set_layout error : {0}!", status.ToString());
            }
        }

        public void set_from(Tensor tensor)
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_input_tensor_info_set_from(
                m_ptr, tensor.Ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InputTensorInfo set_from error : {0}!", status.ToString());
            }
        }
    }
}
