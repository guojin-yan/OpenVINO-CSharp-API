using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.preprocess
{

    /// <summary>
    /// Main class for adding pre- and post- processing steps to existing ov::Model
    /// </summary>
    /// <remarks>
    /// This is a helper class for writing easy pre- and post- processing operations on ov::Model object assuming that
    /// any preprocess operation takes one input and produces one output.
    ///
    /// For advanced preprocessing scenarios, like combining several functions with multiple inputs/outputs into one,
    /// client's code can use transformation passes over ov::Model
    /// </remarks>
    public class PrePostProcessor
    {
        /// <summary>
        /// [private]PrePostProcessor class pointer.
        /// </summary>
        public IntPtr m_ptr = IntPtr.Zero;

        /// <summary>
        /// [public]PrePostProcessor class pointer.
        /// </summary>
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }

        /// <summary>
        /// Default construction through Model.
        /// </summary>
        /// <param name="model">model.</param>
        public PrePostProcessor(Model model)
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_prepostprocessor_create(model.Ptr, ref m_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PrePostProcessor init error : {0}!", status.ToString());
            }
        }
        /// <summary>
        /// Default destructor
        /// </summary>
        ~PrePostProcessor() { dispose(); }
        /// <summary>
        /// Release unmanaged resources
        /// </summary>
        public void dispose() {
            if (m_ptr == IntPtr.Zero)
            {
                return;
            }
            NativeMethods.ov_preprocess_prepostprocessor_free(m_ptr);
            m_ptr = IntPtr.Zero;
        }

        /// <summary>
        /// Gets input pre-processing data structure. Should be used only if model/function has only one input
        /// Using returned structure application's code is able to set user's tensor data (e.g layout), preprocess steps,
        /// target model's data
        /// </summary>
        /// <returns>Reference to model's input information structure</returns>
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

        /// <summary>
        /// Gets input pre-processing data structure for input identified by it's tensor name
        /// </summary>
        /// <param name="tensor_name">Tensor name of specific input. Throws if tensor name is not associated with any input in a model</param>
        /// <returns>Reference to model's input information structure</returns>
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
        /// <summary>
        /// Gets input pre-processing data structure for input identified by it's order in a model
        /// </summary>
        /// <param name="tensor_index">Input index of specific input. Throws if input index is out of range for associated function.</param>
        /// <returns>Reference to model's input information structure</returns>
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

        /// <summary>
        /// Gets output post-processing data structure. Should be used only if model/function has only one output
        /// Using returned structure application's code is able to set model's output data, post-process steps, user's
        /// tensor data (e.g layout)
        /// </summary>
        /// <returns>Reference to model's output information structure</returns>
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

        /// <summary>
        /// Gets output post-processing data structure for output identified by it's tensor name
        /// </summary>
        /// <param name="tensor_name">Tensor name of specific output. Throws if tensor name is not associated with any input in a model</param>
        /// <returns>Reference to model's output information structure</returns>
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

        /// <summary>
        /// Gets output post-processing data structure for output identified by it's order in a model
        /// </summary>
        /// <param name="tensor_index">utput index of specific output. Throws if output index is out of range for associated function</param>
        /// <returns>Reference to model's output information structure</returns>
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

        /// <summary>
        /// Adds pre/post-processing operations to function passed in constructor
        /// </summary>
        /// <returns>Function with added pre/post-processing operations</returns>
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
