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
        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_inputs_size", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_compiled_model_inputs_size(
            IntPtr compiled_model, ref ulong size);

        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_input", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_compiled_model_input(
            IntPtr compiled_model, ref IntPtr input_port);

        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_input_by_index", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_compiled_model_input_by_index(
            IntPtr compiled_model,
            ulong index,
            ref IntPtr input_port);

        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_input_by_name",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_compiled_model_input_by_name(
            IntPtr compiled_model,
            ref sbyte name,
            ref IntPtr input_port);

        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_outputs_size", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_compiled_model_outputs_size(
            IntPtr compiled_model,
            ref ulong size);

        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_output", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_compiled_model_output(
            IntPtr compiled_model, ref IntPtr output_port);

        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_output_by_index",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_compiled_model_output_by_index(
            IntPtr compiled_model,
            ulong index,
            ref IntPtr output_port);

        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_output_by_name",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_compiled_model_output_by_name(
            IntPtr compiled_model,
            ref sbyte name,
            ref IntPtr output_port);

        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_get_runtime_model", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_compiled_model_get_runtime_model(
            IntPtr compiled_model, 
            ref IntPtr model);

        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_create_infer_request",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_compiled_model_create_infer_request(
            IntPtr compiled_model, 
            ref IntPtr infer_request);

        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_set_property", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_compiled_model_set_property(
            IntPtr compiled_model);

        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_get_property", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_compiled_model_get_property(
            IntPtr compiled_model,
                               ref sbyte property_key,
                               ref IntPtr property_value);

        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_export_model", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_compiled_model_export_model(
            IntPtr compiled_model, 
            ref sbyte export_model_path);

        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_free", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_compiled_model_free(IntPtr compiled_model);

        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_get_context", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_compiled_model_get_context(
            IntPtr compiled_model,
            ref IntPtr context);

    }
}
