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
        
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_get_tensor", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_infer_request_get_tensor(IntPtr infer_request, ref sbyte tensor_name, ref IntPtr tensor);

        [DllImport(dll_extern, EntryPoint = "ov_infer_request_infer", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_infer_request_infer(IntPtr infer_request);
    }
}
