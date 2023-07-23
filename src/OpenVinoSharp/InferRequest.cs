using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    public class InferRequest
    {
        private IntPtr ptr;
        public IntPtr Ptr { get { return ptr; } set { ptr = value; } }
        public InferRequest(IntPtr ptr)
        {
            this.ptr = ptr;
        }

        public Tensor get_tensor(string tensor_name) 
        {
            IntPtr tensor_ptr = IntPtr.Zero;
            sbyte[] c_tensor_name = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(tensor_name));
            ov_status_e status = (ov_status_e)NativeMethods.ov_infer_request_get_tensor(ptr, ref c_tensor_name[0], ref tensor_ptr);
            System.Diagnostics.Debug.WriteLine("InferRequest get_tensor: " + status);
            return new Tensor(tensor_ptr);
        }

        public void infer()
        {
            ov_status_e status = (ov_status_e)NativeMethods.ov_infer_request_infer(ptr);
            System.Diagnostics.Debug.WriteLine("InferRequest infer: " + status);
        }
    }
}
