using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.preprocess
{
    public class InputInfo
    {
        private IntPtr m_ptr = IntPtr.Zero;
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }
        public InputInfo(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                System.Diagnostics.Debug.WriteLine("InputInfo init error : ptr is null!");
                return;
            }
            this.m_ptr = ptr;
        }
        ~InputInfo() { dispose(); }
        public void dispose()
        {
            if (m_ptr == IntPtr.Zero)
            {
                return;
            }
            NativeMethods.ov_preprocess_input_info_free(m_ptr);
            m_ptr = IntPtr.Zero;
        }

        public InputTensorInfo tensor() 
        {
            IntPtr input_tensor_ptr = IntPtr.Zero;
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_input_info_get_tensor_info(
                m_ptr, ref input_tensor_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InputInfo tensor error : {0}!", status.ToString());
            }
            return new InputTensorInfo(input_tensor_ptr);
        }

        public PreProcessSteps preprocess() 
        {
            IntPtr preprocess_ptr = IntPtr.Zero;
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_input_info_get_preprocess_steps(
                m_ptr, ref preprocess_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InputInfo preprocess error : {0}!", status.ToString());
            }
            return new PreProcessSteps(preprocess_ptr);
        }

        public InputModelInfo model()
        {
            IntPtr model_ptr = IntPtr.Zero;
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_input_info_get_model_info(
                m_ptr, ref model_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InputInfo preprocess error : {0}!", status.ToString());
            }
            return new InputModelInfo(model_ptr);
        }
    };
}
