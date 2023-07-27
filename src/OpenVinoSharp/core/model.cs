using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    public class Model
    {
        public IntPtr ptr = IntPtr.Zero;
        public IntPtr Ptr { get { return ptr; } set { ptr = value; } }
        public Model(IntPtr ptr)
        {
            Ptr = ptr;
        }
        /// <summary>
        /// Model's destructor
        /// </summary>
        ~Model() { dispose(); }
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
    }
}
