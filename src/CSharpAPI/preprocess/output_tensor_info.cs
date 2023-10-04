using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.preprocess
{
    /// <summary>
    /// Information about user's desired output tensor. By default, it will be initialized to same data
    /// (type/shape/etc) as model's output parameter. User application can override particular parameters (like
    /// 'element_type') according to application's data and specify appropriate conversions in post-processing steps
    /// </summary>
    public class OutputTensorInfo : IDisposable
    {
        /// <summary>
        /// [private]OutputTensorInfo class pointer.
        /// </summary>
        public IntPtr m_ptr = IntPtr.Zero;

        /// <summary>
        /// [public]OutputTensorInfo class pointer.
        /// </summary>
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }

        /// <summary>
        /// Default construction through OutputTensorInfo pointer.
        /// </summary>
        /// <param name="ptr">OutputTensorInfo pointer.</param>
        public OutputTensorInfo(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                System.Diagnostics.Debug.WriteLine("OutputTensorInfo init error : ptr is null!");
                return;
            }
            this.m_ptr = ptr;
        }
        /// <summary>
        /// Default destructor
        /// </summary>
        ~OutputTensorInfo() { Dispose(); }
        /// <summary>
        /// Release unmanaged resources
        /// </summary>
        public void Dispose()
        {
            if (m_ptr == IntPtr.Zero)
            {
                return;
            }
            NativeMethods.ov_preprocess_output_tensor_info_free(m_ptr);
            m_ptr = IntPtr.Zero;
        }

        /// <summary>
        /// Set element type for user's desired output tensor.
        /// </summary>
        /// <param name="type">Element type for user's output tensor.</param>
        /// <returns>Reference to 'this' to allow chaining with other calls in a builder-like manner.</returns>
        public OutputTensorInfo set_element_type(ElementType type)
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_output_set_element_type(
                m_ptr, (uint)type);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("OutputTensorInfo set_element_type error : {0}!", status.ToString());
            }
            return this;
        }
    }
}
