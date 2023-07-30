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

        [DllImport(dll_extern, EntryPoint = "ov_const_port_get_shape", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_const_port_get_shape(IntPtr port, IntPtr tensor_shape);

        [DllImport(dll_extern, EntryPoint = "ov_const_port_get_shape", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]

        public extern static int ov_port_get_shape(IntPtr port, IntPtr tensor_shape);

        [DllImport(dll_extern, EntryPoint = "ov_port_get_any_name", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]

        public extern static int ov_port_get_any_name(IntPtr port, ref IntPtr tensor_name);

        [DllImport(dll_extern, EntryPoint = "ov_port_get_partial_shape", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]

        public extern static int ov_port_get_partial_shape(IntPtr port, IntPtr partial_shape);

        [DllImport(dll_extern, EntryPoint = "ov_port_get_element_type", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]

        public extern static int ov_port_get_element_type(IntPtr port, ref uint tensor_type);

        [DllImport(dll_extern, EntryPoint = "ov_output_port_free", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]

        public extern static int ov_output_port_free(IntPtr port);

        [DllImport(dll_extern, EntryPoint = "ov_output_const_port_free", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]

        public extern static int ov_output_const_port_free(IntPtr port);
    }
}
