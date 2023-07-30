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
        [DllImport(dll_extern, EntryPoint = "ov_tensor_create_from_host_ptr", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_tensor_create_from_host_ptr(
            uint type, 
            Shape.ov_shape shape, 
            IntPtr host_ptr, 
            ref IntPtr tensor);

        [DllImport(dll_extern, EntryPoint = "ov_tensor_create",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_tensor_create(uint type, Shape.ov_shape shape, ref IntPtr tensor);

            [DllImport(dll_extern, EntryPoint = "ov_tensor_set_shape",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_tensor_set_shape(IntPtr tensor, Shape.ov_shape shape);

        [DllImport(dll_extern, EntryPoint = "ov_tensor_get_shape",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_tensor_get_shape(IntPtr tensor, IntPtr shape);
        [DllImport(dll_extern, EntryPoint = "ov_tensor_get_element_type",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_tensor_get_element_type(IntPtr tensor, out uint type);

        [DllImport(dll_extern, EntryPoint = "ov_tensor_get_size",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_tensor_get_size(IntPtr tensor, ref ulong elements_size);
            [DllImport(dll_extern, EntryPoint = "ov_tensor_get_byte_size",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_tensor_get_byte_size(IntPtr tensor, ref ulong byte_size);
        [DllImport(dll_extern, EntryPoint = "ov_tensor_data", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_tensor_data(IntPtr tensor, ref IntPtr data);

        [DllImport(dll_extern, EntryPoint = "ov_tensor_free",
             CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_tensor_free(IntPtr tensor);
    }
}
