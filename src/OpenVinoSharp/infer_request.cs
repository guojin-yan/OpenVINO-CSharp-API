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
        private IntPtr ptr;
        public IntPtr Ptr { get { return ptr; } set { ptr = value; } }
   
        /// <summary>
        /// Constructs InferRequest from the initialized IntPtr.
        /// </summary>
        /// <param name="ptr"></param>
        public InferRequest(IntPtr ptr)
        {
            this.ptr = ptr;
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
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_infer_request_get_tensor(ptr, ref c_tensor_name[0], ref tensor_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InferRequest get_tensor() error!");
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
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_infer_request_infer(ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("InferRequest infer() error!");
            }
        }
    }
}
