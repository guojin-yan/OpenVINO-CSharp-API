using OpenVinoSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.preprocess
{
    /// <summary>
    /// Information about model's input tensor. If all information is already included to loaded model, this info
    /// may not be needed. However it can be set to specify additional information about model, like 'layout'.
    /// </summary>
    /// <example>
    /// Example of usage of model 'layout':
    /// Support model has input parameter with shape {1, 3, 224, 224} and user needs to resize input image to model's
    /// dimensions. It can be done like this
    /// </example>
    public class InputModelInfo : IDisposable
    {
        /// <summary>
        /// [private]InputModelInfo class pointer.
        /// </summary>
        public IntPtr m_ptr = IntPtr.Zero;

        /// <summary>
        /// [public]InputModelInfo class pointer.
        /// </summary>
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }

        /// <summary>
        /// Default construction through InputModelInfo pointer.
        /// </summary>
        /// <param name="ptr">InputModelInfo pointer.</param>
        public InputModelInfo(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                System.Diagnostics.Debug.WriteLine("InputModelInfo init error : ptr is null!");
                return;
            }
            this.m_ptr = ptr;
        }
        /// <summary>
        /// Default destructor
        /// </summary>
        ~InputModelInfo() { Dispose(); }
        /// <summary>
        /// Release unmanaged resources
        /// </summary>
        public void Dispose()
        {
            if (m_ptr == IntPtr.Zero)
            {
                return;
            }
            NativeMethods.ov_preprocess_input_model_info_free(m_ptr);
            m_ptr = IntPtr.Zero;
        }

        /// <summary>
        /// Set layout for model's input tensor. This version allows chaining for Lvalue objects
        /// </summary>
        /// <param name="layout">Layout for model's input tensor.</param>
        /// <returns>Reference to 'this' to allow chaining with other calls in a builder-like manner</returns>
        public InputModelInfo set_layout(Layout layout)
        {
            ExceptionStatus status = NativeMethods.ov_preprocess_input_model_info_set_layout(
                m_ptr, layout.Ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InputModelInfo set_layout error : {0}!", status.ToString());
            }
            return this;
        }
    }
}
