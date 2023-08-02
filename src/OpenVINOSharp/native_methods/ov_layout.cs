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


        [DllImport(dll_extern, EntryPoint = "ov_layout_create", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_layout_create(ref sbyte layout_desc, ref IntPtr layout);

        [DllImport(dll_extern, EntryPoint = "ov_layout_free", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_layout_free(IntPtr layout);

        [DllImport(dll_extern, EntryPoint = "ov_layout_to_string", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static string ov_layout_to_string(IntPtr layout);
    }
}
