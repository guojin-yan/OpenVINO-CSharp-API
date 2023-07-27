using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    /// <summary>
    /// This class represents a compiled model.
    /// </summary>
    /// <ingroup>ov_runtime_c#_api</ingroup>
    /// <remarks>
    /// A model is compiled by a specific device by applying multiple optimization 
    /// transformations, then mapping to compute kernels.
    /// </remarks>
    public class CompiledModel
    {
        private IntPtr ptr;
        public IntPtr Ptr { get { return ptr; } set { ptr = value; } }

        /// <summary>
        /// Constructs CompiledModel from the initialized ptr.
        /// </summary>
        /// <param name="ptr"></param>
        public CompiledModel(IntPtr ptr) 
        {
            this.ptr = ptr;
        }
        /// <summary>
        /// CompiledModel()'s destructor
        /// </summary>
        ~CompiledModel()
        {
            dispose();
        }
        /// <summary>
        /// Release unmanaged resources
        /// </summary>
        public void dispose()
        {
            if (ptr == IntPtr.Zero)
            {
                return;
            }
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_core_free(ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Core free error!");
                return;
            }
            ptr = IntPtr.Zero;
        }
        /// <summary>
        /// Creates an inference request object used to infer the compiled model.
        /// The created request has allocated input and output tensors (which can be changed later).
        /// </summary>
        /// <returns>InferRequest object</returns>
        public InferRequest create_infer_request()
        {
            IntPtr infer_request_ptr = IntPtr.Zero;
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_compiled_model_create_infer_request(ptr, ref infer_request_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("CompiledModel create_infer_request() error!");
                return new InferRequest(IntPtr.Zero);
            }
            return new InferRequest(infer_request_ptr);
        }
        
    }
}
