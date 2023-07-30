using OpenVinoSharp.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.preprocess
{
    public class PreProcessSteps
    {
        private IntPtr m_ptr = IntPtr.Zero;
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }
        public PreProcessSteps(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                System.Diagnostics.Debug.WriteLine("PreProcessSteps init error : ptr is null!");
                return;
            }
            this.m_ptr = ptr;
        }
        ~PreProcessSteps() { dispose(); }
        public void dispose()
        {
            if (m_ptr == IntPtr.Zero)
            {
                return;
            }
            NativeMethods.ov_preprocess_preprocess_steps_free(m_ptr);
           
            m_ptr = IntPtr.Zero;
        }
        public void resize(ResizeAlgorithm resize) 
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_preprocess_steps_resize(
                m_ptr, (int)resize);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PreProcessSteps resize error : {0}!", status.ToString());
            }
        }
        public void scale(float value)
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_preprocess_steps_scale(
                m_ptr, value);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PreProcessSteps resize error : {0}!", status.ToString());
            }
        }
        public void mean(float value)
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_preprocess_steps_mean(
                m_ptr, value);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PreProcessSteps mean error : {0}!", status.ToString());
            }
        }
        public void crop(int[] begin, int[] end)
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_preprocess_steps_crop(
                m_ptr, ref begin[0], begin.Length, ref end[0], end.Length);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PreProcessSteps crop error : {0}!", status.ToString());
            }
        }
        public void convert_layout(Layout layout)
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_preprocess_steps_convert_layout(
                m_ptr, layout.Ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PreProcessSteps convert_layout error : {0}!", status.ToString());
            }
        }
        
        public void reverse_channels()
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_preprocess_steps_reverse_channels(
                m_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PreProcessSteps reverse_channels error : {0}!", status.ToString());
            }
        }
        public void convert_color(ColorFormat format) 
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_preprocess_steps_convert_color(
                m_ptr, (uint)format);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PreProcessSteps convert_element_type error : {0}!", status.ToString());
            }
        }
        public void convert_element_type(OvType type)
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_preprocess_steps_convert_element_type(
                m_ptr, (uint)type.get_type());
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PreProcessSteps convert_element_type error : {0}!", status.ToString());
            }
        }
    }
}
