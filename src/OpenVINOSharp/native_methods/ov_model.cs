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

        [DllImport(dll_extern, EntryPoint = "ov_model_free", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_model_free(IntPtr model);


        [DllImport(dll_extern, EntryPoint = "ov_model_const_input", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_model_const_input(IntPtr model, ref IntPtr input_port);
        [DllImport(dll_extern, EntryPoint = "ov_model_const_input_by_name", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_model_const_input_by_name(IntPtr model, ref sbyte tensor_name, ref IntPtr input_port);

        [DllImport(dll_extern, EntryPoint = "ov_model_const_input_by_index", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_model_const_input_by_index(IntPtr model, ulong index, ref IntPtr input_port);


        [DllImport(dll_extern, EntryPoint = "ov_model_const_output", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_model_const_output(IntPtr model, ref IntPtr output_port);

        [DllImport(dll_extern, EntryPoint = "ov_model_const_output_by_index", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_model_const_output_by_index(IntPtr model, ulong index, ref IntPtr output_port);

        [DllImport(dll_extern, EntryPoint = "ov_model_const_output_by_name", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_model_const_output_by_name(IntPtr model, ref sbyte tensor_name, ref IntPtr output_port);



        [DllImport(dll_extern, EntryPoint = "ov_model_get_friendly_name", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_model_get_friendly_name(IntPtr model, ref IntPtr friendly_name);

    }
}
