using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.preprocess
{
    /// <summary>
    ///  Class holding preprocessing information for one input
    ///  From preprocessing pipeline perspective, each input can be represented as:
    ///    - User's input parameter info (InputInfo::tensor)
    ///    - Preprocessing steps applied to user's input (InputInfo::preprocess)
    ///    - Model's input info, which is a final input's info after preprocessing (InputInfo::model)
    /// </summary>
    public class InputInfo
    {
        /// <summary>
        /// [private]InputInfo class pointer.
        /// </summary>
        public IntPtr m_ptr = IntPtr.Zero;

        /// <summary>
        /// [public]InputInfo class pointer.
        /// </summary>
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }

        /// <summary>
        /// Default construction through InputInfo pointer.
        /// </summary>
        /// <param name="ptr">InputInfo pointer.</param>
        public InputInfo(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                System.Diagnostics.Debug.WriteLine("InputInfo init error : ptr is null!");
                return;
            }
            this.m_ptr = ptr;
        }
        /// <summary>
        /// Default destructor
        /// </summary>
        ~InputInfo() { dispose(); }
        /// <summary>
        /// Release unmanaged resources.
        /// </summary>
        public void dispose()
        {
            if (m_ptr == IntPtr.Zero)
            {
                return;
            }
            NativeMethods.ov_preprocess_input_info_free(m_ptr);
            m_ptr = IntPtr.Zero;
        }

        /// <summary>
        ///  Get current input tensor information with ability to change specific data
        /// </summary>
        /// <returns>Reference to current input tensor structure</returns>
        public InputTensorInfo tensor() 
        {
            IntPtr input_tensor_ptr = IntPtr.Zero;
            ExceptionStatus status = NativeMethods.ov_preprocess_input_info_get_tensor_info(
                m_ptr, ref input_tensor_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InputInfo tensor error : {0}!", status.ToString());
            }
            return new InputTensorInfo(input_tensor_ptr);
        }

        /// <summary>
        /// Get current input preprocess information with ability to add more preprocessing steps
        /// </summary>
        /// <returns>Reference to current preprocess steps structure.</returns>
        public PreProcessSteps preprocess() 
        {
            IntPtr preprocess_ptr = IntPtr.Zero;
            ExceptionStatus status = NativeMethods.ov_preprocess_input_info_get_preprocess_steps(
                m_ptr, ref preprocess_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InputInfo preprocess error : {0}!", status.ToString());
            }
            return new PreProcessSteps(preprocess_ptr);
        }

        /// <summary>
        /// Get current input model information with ability to change original model's input data
        /// </summary>
        /// <returns>Reference to current model's input information structure.</returns>
        public InputModelInfo model()
        {
            IntPtr model_ptr = IntPtr.Zero;
            ExceptionStatus status = NativeMethods.ov_preprocess_input_info_get_model_info(
                m_ptr, ref model_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InputInfo preprocess error : {0}!", status.ToString());
            }
            return new InputModelInfo(model_ptr);
        }
    };
}
