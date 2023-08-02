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
        

        [DllImport(dll_extern, EntryPoint = "ov_get_openvino_version", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_get_openvino_version(IntPtr version);

        [DllImport(dll_extern, EntryPoint = "ov_version_free", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_version_free(IntPtr version);
        [DllImport(dll_extern, EntryPoint = "ov_core_create", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_create(ref IntPtr core);
        [DllImport(dll_extern, EntryPoint = "ov_core_create_with_config", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_create_with_config(string xml_config_file, ref IntPtr core);
        [DllImport(dll_extern, EntryPoint = "ov_core_free", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_core_free(IntPtr core);
        [DllImport(dll_extern, EntryPoint = "ov_core_read_model_unicode", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_read_model_unicode(IntPtr core, string model_path, string bin_path, ref IntPtr model);
        [DllImport(dll_extern, EntryPoint = "ov_core_read_model", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_read_model(IntPtr core, ref char model_path, ref char bin_path, ref IntPtr model);

        [DllImport(dll_extern, EntryPoint = "ov_core_read_model_from_memory", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_read_model_from_memory(IntPtr core, string model_path, IntPtr weights, ref IntPtr model);
        [DllImport(dll_extern, EntryPoint = "ov_core_compile_model", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model(IntPtr core, IntPtr model, ref sbyte device_name, ulong property_args_size, ref IntPtr compiled_model);
        [DllImport(dll_extern, EntryPoint = "ov_core_get_versions_by_device_name", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_get_versions_by_device_name(IntPtr core, ref sbyte device_name, IntPtr versions);
        [DllImport(dll_extern, EntryPoint = "ov_core_versions_free", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_core_versions_free(IntPtr versions);
    }
}
