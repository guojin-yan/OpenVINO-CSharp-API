using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.preprocess
{
    public class OutputInfo
    {
        private IntPtr m_ptr = IntPtr.Zero;
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }
        public OutputInfo(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                System.Diagnostics.Debug.WriteLine("OutputInfo init error : ptr is null!");
                return;
            }
            this.m_ptr = ptr;
        }
        ~OutputInfo() { dispose(); }
        public void dispose()
        {
            if (m_ptr == IntPtr.Zero)
            {
                return;
            }
            NativeMethods.ov_preprocess_output_info_free(m_ptr);
            m_ptr = IntPtr.Zero;
        }

        public OutputTensorInfo tensor()
        {
            IntPtr output_tensor_ptr = IntPtr.Zero;
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_output_info_get_tensor_info(
                m_ptr, ref output_tensor_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InputInfo tensor error : {0}!", status.ToString());
            }
            return new OutputTensorInfo(output_tensor_ptr);
        }
    }
}
