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
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_set_tensor", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_infer_request_set_tensor(
            IntPtr infer_request, 
            ref sbyte tensor_name,
            IntPtr tensor);
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_set_tensor_by_port", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_infer_request_set_tensor_by_port(
            IntPtr infer_request,
            IntPtr port,
            IntPtr tensor);

        [DllImport(dll_extern, EntryPoint = "ov_infer_request_set_tensor_by_const_port", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_infer_request_set_tensor_by_const_port(
            IntPtr infer_request,
            IntPtr port,
            IntPtr tensor);

        [DllImport(dll_extern, EntryPoint = "ov_infer_request_set_input_tensor_by_index",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_infer_request_set_input_tensor_by_index(
            IntPtr infer_request,
            ulong idx,
            IntPtr tensor);

        [DllImport(dll_extern, EntryPoint = "ov_infer_request_set_input_tensor", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_infer_request_set_input_tensor(
            IntPtr infer_request,
            IntPtr tensor);

        [DllImport(dll_extern, EntryPoint = "ov_infer_request_set_output_tensor_by_index", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_infer_request_set_output_tensor_by_index(
            IntPtr infer_request,
            ulong idx,
            IntPtr tensor);

        [DllImport(dll_extern, EntryPoint = "ov_infer_request_set_output_tensor", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_infer_request_set_output_tensor(
            IntPtr infer_request, 
            IntPtr tensor);

        [DllImport(dll_extern, EntryPoint = "ov_infer_request_get_tensor",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_infer_request_get_tensor(
            IntPtr infer_request,
            ref sbyte tensor_name,
            ref IntPtr tensor);

        [DllImport(dll_extern, EntryPoint = "ov_infer_request_get_tensor_by_const_port",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_infer_request_get_tensor_by_const_port(
            IntPtr infer_request,
            IntPtr port,
            ref IntPtr tensor);

        [DllImport(dll_extern, EntryPoint = "ov_infer_request_get_tensor_by_port",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_infer_request_get_tensor_by_port(
            IntPtr infer_request,
            IntPtr port,
            ref IntPtr tensor);

        [DllImport(dll_extern, EntryPoint = "ov_infer_request_get_input_tensor_by_index",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_infer_request_get_input_tensor_by_index(
            IntPtr infer_request,
            ulong idx,
            ref IntPtr tensor);

        [DllImport(dll_extern, EntryPoint = "ov_infer_request_get_input_tensor",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_infer_request_get_input_tensor(
            IntPtr infer_request, 
            ref IntPtr tensor);

        [DllImport(dll_extern, EntryPoint = "ov_infer_request_get_output_tensor_by_index",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_infer_request_get_output_tensor_by_index(
            IntPtr infer_request,
            ulong idx,
            ref IntPtr tensor);

        [DllImport(dll_extern, EntryPoint = "ov_infer_request_get_output_tensor",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_infer_request_get_output_tensor(
            IntPtr infer_request, 
            ref IntPtr tensor);
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_infer", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_infer_request_infer(
            IntPtr infer_request);
    }
}
