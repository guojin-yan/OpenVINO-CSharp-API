using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.preprocess
{
    /// <summary>
    /// Class holding postprocessing information for one output
    /// From postprocessing pipeline perspective, each output can be represented as:
    ///    - Model's output info,  (OutputInfo::model)
    ///    - Postprocessing steps applied to user's input (OutputInfo::postprocess)
    ///    - User's desired output parameter information, which is a final one after preprocessing (OutputInfo::tensor)
    /// </summary>
    public class OutputInfo
    {
        /// <summary>
        /// [private]OutputInfo class pointer.
        /// </summary>
        public IntPtr m_ptr = IntPtr.Zero;

        /// <summary>
        /// [public]OutputInfo class pointer.
        /// </summary>
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }

        /// <summary>
        /// Default construction through OutputInfo pointer.
        /// </summary>
        /// <param name="ptr">OutputInfo pointer.</param>
        public OutputInfo(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                System.Diagnostics.Debug.WriteLine("OutputInfo init error : ptr is null!");
                return;
            }
            this.m_ptr = ptr;
        }
        /// <summary>
        /// Default destructor
        /// </summary>
        ~OutputInfo() { dispose(); }
        /// <summary>
        /// Release unmanaged resources
        /// </summary>
        public void dispose()
        {
            if (m_ptr == IntPtr.Zero)
            {
                return;
            }
            NativeMethods.ov_preprocess_output_info_free(m_ptr);
            m_ptr = IntPtr.Zero;
        }

        /// <summary>
        /// Get current output tensor information with ability to change specific data
        /// </summary>
        /// <returns>Reference to current output tensor structure</returns>
        public OutputTensorInfo tensor()
        {
            IntPtr output_tensor_ptr = IntPtr.Zero;
            ExceptionStatus status = NativeMethods.ov_preprocess_output_info_get_tensor_info(
                m_ptr, ref output_tensor_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InputInfo tensor error : {0}!", status.ToString());
            }
            return new OutputTensorInfo(output_tensor_ptr);
        }
    }
}
