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
        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_create_infer_request", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_compiled_model_create_infer_request(IntPtr compiled_model, ref IntPtr infer_request);
    }
}
