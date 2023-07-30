using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.preprocess
{


    public class PrePostProcessor
    {
        private IntPtr m_ptr = IntPtr.Zero;
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }

        public PrePostProcessor(Model model)
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_prepostprocessor_create(model.Ptr, ref m_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PrePostProcessor init error : {0}!", status.ToString());
            }
        }
        ~PrePostProcessor() { dispose(); }
        public void dispose() {
            if (m_ptr == IntPtr.Zero)
            {
                return;
            }
            NativeMethods.ov_preprocess_prepostprocessor_free(m_ptr);
            m_ptr = IntPtr.Zero;
        }

        public InputInfo input()
        {
            IntPtr input_ptr = IntPtr.Zero;
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_prepostprocessor_get_input_info(m_ptr, ref input_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PrePostProcessor input error : {0}!", status.ToString());
            }
            return new InputInfo(input_ptr);
        }

        public InputInfo input(string tensor_name)
        {
            IntPtr input_ptr = IntPtr.Zero;
            sbyte[] c_tensor_name = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(tensor_name));
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_prepostprocessor_get_input_info_by_name(m_ptr, ref c_tensor_name[0], ref input_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PrePostProcessor input error : {0}!", status.ToString());
            }
            return new InputInfo(input_ptr);
        }
        public InputInfo input(ulong tensor_index)
        {
            IntPtr input_ptr = IntPtr.Zero;
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_prepostprocessor_get_input_info_by_index(m_ptr, tensor_index, ref input_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PrePostProcessor input error : {0}!", status.ToString());
            }
            return new InputInfo(input_ptr);
        }

        public OutputInfo output()
        {
            IntPtr input_ptr = IntPtr.Zero;
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_prepostprocessor_get_output_info(m_ptr, ref input_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PrePostProcessor output error : {0}!", status.ToString());
            }
            return new OutputInfo(input_ptr);
        }

        public OutputInfo output(string tensor_name)
        {
            IntPtr input_ptr = IntPtr.Zero;
            sbyte[] c_tensor_name = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(tensor_name));
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_prepostprocessor_get_output_info_by_name(m_ptr, ref c_tensor_name[0], ref input_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PrePostProcessor output error : {0}!", status.ToString());
            }
            return new OutputInfo(input_ptr);
        }
        public OutputInfo output(ulong tensor_index)
        {
            IntPtr input_ptr = IntPtr.Zero;
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_prepostprocessor_get_output_info_by_index(m_ptr, tensor_index, ref input_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PrePostProcessor output error : {0}!", status.ToString());
            }
            return new OutputInfo(input_ptr);
        }

        public Model build() 
        {
            IntPtr model_ptr = IntPtr.Zero;
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_prepostprocessor_build(
                m_ptr, ref model_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PrePostProcessor build error : " + status.ToString());
            }
            return new Model(model_ptr);
        } 
    }

   
}
