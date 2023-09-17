using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    public partial class NativeMethods
    {
        /// <summary>
        /// Initialize a fully shape object, allocate space for its dimensions 
        /// and set its content id dims is not null.
        /// </summary>
        /// <param name="rank">The rank value for this object, it should be more than 0(>0)</param>
        /// <param name="dims">The dimensions data for this shape object, it's size should be equal to rank.</param>
        /// <param name="shape">The input/output shape object pointer.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_shape_create", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_shape_create(
            long rank,
            ref long dims, 
            IntPtr shape);

        /// <summary>
        /// Free a shape object's internal memory.
        /// </summary>
        /// <param name="shape">The input shape object pointer.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_shape_free", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_shape_free(
            IntPtr shape);
    }
}
