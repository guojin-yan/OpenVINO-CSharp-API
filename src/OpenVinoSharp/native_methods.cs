using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    internal class NativeMethods
    {
        private const string dll_extern = "./openvino/openvino_c.dll";



        #region ov_core.h

        [DllImport(dll_extern, EntryPoint= "ov_get_openvino_version", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_get_openvino_version(IntPtr version); 

        [DllImport(dll_extern, EntryPoint = "ov_version_free", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_version_free(IntPtr version);
        [DllImport(dll_extern, EntryPoint = "ov_core_create", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_core_create(ref IntPtr core);
        [DllImport(dll_extern, EntryPoint = "ov_core_create_with_config", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_core_create_with_config(string xml_config_file, ref IntPtr core);
        [DllImport(dll_extern, EntryPoint = "ov_core_free", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_core_free(IntPtr core);
        [DllImport(dll_extern, EntryPoint = "ov_core_read_model_unicode", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_core_read_model_unicode(IntPtr core, string model_path, string bin_path, ref IntPtr model);
        [DllImport(dll_extern, EntryPoint = "ov_core_read_model", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_core_read_model(IntPtr core, ref char model_path, ref char bin_path, ref IntPtr model);

        [DllImport(dll_extern, EntryPoint = "ov_core_read_model_from_memory", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_core_read_model_from_memory(IntPtr core, string model_path, IntPtr weights, ref IntPtr model);
        [DllImport(dll_extern, EntryPoint = "ov_core_compile_model", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_core_compile_model(IntPtr core, IntPtr model, ref sbyte device_name, ulong property_args_size, ref IntPtr compiled_model);
        [DllImport(dll_extern, EntryPoint = "ov_core_get_versions_by_device_name", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_core_get_versions_by_device_name(IntPtr core, ref sbyte device_name, IntPtr versions);
        [DllImport(dll_extern, EntryPoint = "ov_core_versions_free", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_core_versions_free(IntPtr versions);
        #endregion


        #region ov_model.h

        [DllImport(dll_extern, EntryPoint = "ov_model_free", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_model_free(IntPtr model);

        #endregion


        #region ov_compiled_model.h

        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_create_infer_request", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_compiled_model_create_infer_request(IntPtr compiled_model, ref IntPtr infer_request);

        #endregion



        #region ov_infer_request.h

        [DllImport(dll_extern, EntryPoint = "ov_infer_request_get_tensor", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_infer_request_get_tensor(IntPtr infer_request, ref sbyte tensor_name, ref IntPtr tensor);

        [DllImport(dll_extern, EntryPoint = "ov_infer_request_infer", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_infer_request_infer(IntPtr infer_request);



        #endregion

        #region ov_tensor.h

        [DllImport(dll_extern, EntryPoint = "ov_tensor_get_shape", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_tensor_get_shape(IntPtr tensor, IntPtr shape);
        [DllImport(dll_extern, EntryPoint = "ov_tensor_get_element_type", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_tensor_get_element_type(IntPtr tensor, out uint type);
        [DllImport(dll_extern, EntryPoint = "ov_tensor_data", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_tensor_data(IntPtr tensor, ref IntPtr data );
        #endregion

        #region ov_shape.h

        [DllImport(dll_extern, EntryPoint = "ov_shape_create", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_shape_create(long rank, ref long dims, IntPtr shape);
        [DllImport(dll_extern, EntryPoint = "ov_shape_free", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_shape_free(IntPtr shape);
      
        #endregion

    }
}
