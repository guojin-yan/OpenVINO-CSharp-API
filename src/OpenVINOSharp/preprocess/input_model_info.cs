using OpenVinoSharp.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.preprocess
{
    public class InputModelInfo
    {
        private IntPtr m_ptr = IntPtr.Zero;
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }
        public InputModelInfo(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                System.Diagnostics.Debug.WriteLine("InputModelInfo init error : ptr is null!");
                return;
            }
            this.m_ptr = ptr;
        }
        ~InputModelInfo() { dispose(); }
        public void dispose()
        {
            if (m_ptr == IntPtr.Zero)
            {
                return;
            }
            NativeMethods.ov_preprocess_input_model_info_free(m_ptr);
            m_ptr = IntPtr.Zero;
        }
        public void set_layout(Layout layout)
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_input_model_info_set_layout(
                m_ptr, layout.Ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InputModelInfo set_layout error : {0}!", status.ToString());
            }
        }
    }
}
