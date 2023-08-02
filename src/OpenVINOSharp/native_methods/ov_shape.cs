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

        [DllImport(dll_extern, EntryPoint = "ov_shape_create", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_shape_create(long rank, ref long dims, IntPtr shape);
        [DllImport(dll_extern, EntryPoint = "ov_shape_free", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_shape_free(IntPtr shape);
    }
}
