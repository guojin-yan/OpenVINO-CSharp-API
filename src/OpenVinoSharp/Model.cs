using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    public class Model
    {
        public IntPtr ptr;
        public IntPtr Ptr { get { return ptr; } set { ptr = value; } }
        public Model(IntPtr ptr)
        {
            Ptr = ptr;
        }
        public void free() 
        {
            ov_status_e status = (ov_status_e)NativeMethods.ov_model_free(ptr);
        }
    }
}
