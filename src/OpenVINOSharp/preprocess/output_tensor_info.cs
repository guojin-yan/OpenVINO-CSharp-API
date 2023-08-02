using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.preprocess
{
    public class OutputTensorInfo
    {
        private IntPtr m_ptr = IntPtr.Zero;
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }
        public OutputTensorInfo(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                System.Diagnostics.Debug.WriteLine("OutputTensorInfo init error : ptr is null!");
                return;
            }
            this.m_ptr = ptr;
        }
        ~OutputTensorInfo() { dispose(); }
        public void dispose()
        {
            if (m_ptr == IntPtr.Zero)
            {
                return;
            }
            NativeMethods.ov_preprocess_output_tensor_info_free(m_ptr);
            m_ptr = IntPtr.Zero;
        }
      
        public void set_element_type(ElementType type)
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_output_set_element_type(
                m_ptr, (uint)type);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("OutputTensorInfo set_element_type error : {0}!", status.ToString());
            }
        }
    }
}
