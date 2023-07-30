using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    /// <summary>
    /// This is a class of infer request that can be run in asynchronous or synchronous manners.
    /// </summary>
    /// <ingroup>ov_runtime_c#_api</ingroup>
    public class InferRequest
    {
        private IntPtr m_ptr = IntPtr.Zero;
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }
   
        /// <summary>
        /// Constructs InferRequest from the initialized IntPtr.
        /// </summary>
        /// <param name="ptr"></param>
        public InferRequest(IntPtr ptr)
        {
            this.m_ptr = ptr;
        }
        /// <summary>
        /// InferRequest's destructor
        /// </summary>
        ~InferRequest()
        {
            dispose();
        }
        /// <summary>
        /// Release unmanaged resources
        /// </summary>
        public void dispose()
        {
            if (m_ptr == IntPtr.Zero)
            {
                return;
            }
            NativeMethods.ov_core_free(m_ptr);

            m_ptr = IntPtr.Zero;
        }

        public void set_tensor(string tensor_name,Tensor tensor) 
        {
            sbyte[] c_tensor_name = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(tensor_name));
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_infer_request_set_tensor(
                m_ptr, ref c_tensor_name[0], tensor.Ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("set_tensor set_tensor error : " + status.ToString());
            }
        }

        public void set_tensor(Node node, Tensor tensor)
        {
            ExceptionStatus status;
            if (node.node_type == Node.NodeType.e_const) 
            {
                status = (ExceptionStatus)NativeMethods.ov_infer_request_set_tensor_by_const_port(
               m_ptr, node.Ptr, tensor.Ptr);
            }
            else {
                status = (ExceptionStatus)NativeMethods.ov_infer_request_set_tensor_by_port(
                m_ptr, node.Ptr, tensor.Ptr);
            }
     
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("set_tensor set_tensor error : " + status.ToString());
            }
        }
        public void set_input_tensor(ulong index, Tensor tensor)
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_infer_request_set_input_tensor_by_index(
                m_ptr, index, tensor.Ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("set_tensor set_input_tensor error : " + status.ToString());
            }
        }
        public void set_input_tensor(Tensor tensor)
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_infer_request_set_input_tensor(
                m_ptr, tensor.Ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("set_tensor set_input_tensor error : " + status.ToString());
            }
        }
        public void set_output_tensor(ulong index, Tensor tensor)
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_infer_request_set_output_tensor_by_index(
                m_ptr, index, tensor.Ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("set_tensor set_output_tensor error : " + status.ToString());
            }
        }
        public void set_output_tensor(Tensor tensor)
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_infer_request_set_output_tensor(
                m_ptr, tensor.Ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("set_tensor set_output_tensor error : " + status.ToString());
            }
        }

      

        /// <summary>
        ///  Gets an input/output tensor for inference by tensor name.
        /// </summary>
        /// <param name="tensor_name">Name of a tensor to get.</param>
        /// <returns>The tensor with name @p tensor_name. If the tensor is not found, an exception is thrown.</returns>
        public Tensor get_tensor(string tensor_name) 
        {
            IntPtr tensor_ptr = IntPtr.Zero;
            sbyte[] c_tensor_name = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(tensor_name));
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_infer_request_get_tensor(m_ptr, ref c_tensor_name[0], ref tensor_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InferRequest get_tensor() error : " + status.ToString());
                return new Tensor(IntPtr.Zero);
            }
            return new Tensor(tensor_ptr);
        }

        public Tensor get_tensor(Node node)
        {
            IntPtr tensor_ptr = IntPtr.Zero;
            ExceptionStatus status;
            if (node.node_type == Node.NodeType.e_const)
            {
                status = (ExceptionStatus)NativeMethods.ov_infer_request_get_tensor_by_const_port(
               m_ptr, node.Ptr, ref tensor_ptr);
            }
            else
            {
                status = (ExceptionStatus)NativeMethods.ov_infer_request_get_tensor_by_port(
                m_ptr, node.Ptr, ref tensor_ptr);
            }

            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("set_tensor get_tensor error : " + status.ToString());
            }
            return new Tensor(tensor_ptr);
        }
        public Tensor get_input_tensor(ulong index)
        {
            IntPtr tensor_ptr = IntPtr.Zero;
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_infer_request_get_input_tensor_by_index(
                m_ptr, index, ref tensor_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InferRequest get_input_tensor() error : " + status.ToString());
                return new Tensor(IntPtr.Zero);
            }
            return new Tensor(tensor_ptr);
        }
        public Tensor get_input_tensor()
        {
            IntPtr tensor_ptr = IntPtr.Zero;
            
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_infer_request_get_input_tensor(
                m_ptr, ref tensor_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InferRequest get_input_tensor() error : " + status.ToString());
                return new Tensor(IntPtr.Zero);
            }
            return new Tensor(tensor_ptr);
        }
        public Tensor get_output_tensor(ulong index)
        {
            IntPtr tensor_ptr = IntPtr.Zero;
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_infer_request_get_output_tensor_by_index(
                m_ptr, index, ref tensor_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InferRequest get_output_tensor() error : " + status.ToString());
                return new Tensor(IntPtr.Zero);
            }
            return new Tensor(tensor_ptr);
        }
        public Tensor get_output_tensor()
        {
            IntPtr tensor_ptr = IntPtr.Zero;

            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_infer_request_get_output_tensor(
                m_ptr, ref tensor_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InferRequest get_output_tensor() error : " + status.ToString());
                return new Tensor(IntPtr.Zero);
            }
            return new Tensor(tensor_ptr);
        }
        /// <summary>
        /// Infers specified input(s) in synchronous mode.
        /// </summary>
        /// <note>
        /// It blocks all methods of InferRequest while request is ongoing (running or waiting in a queue).
        /// Calling any method leads to throwning the ov::Busy exception.
        /// </note>
        public void infer()
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_infer_request_infer(m_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InferRequest infer() error : " + status.ToString());
            }
        }
    }
}
