using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    public class CompiledModel
    {
        private IntPtr ptr;
        public IntPtr Ptr { get { return ptr; } set { ptr = value; } }
        public CompiledModel(IntPtr ptr) 
        {
            this.ptr = ptr;
        }
        public InferRequest create_infer_request()
        {
            IntPtr infer_request_ptr = IntPtr.Zero;
            ov_status_e status = (ov_status_e)NativeMethods.ov_compiled_model_create_infer_request(ptr, ref infer_request_ptr);
            System.Diagnostics.Debug.WriteLine("create_infer_request: " + status);
            return new InferRequest(infer_request_ptr);
        }
        
    }
}
