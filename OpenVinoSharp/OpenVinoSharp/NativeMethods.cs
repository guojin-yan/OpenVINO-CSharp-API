using System;
using System.Runtime.InteropServices;


namespace OpenVinoSharp
{
    internal class NativeMethods
    {
        private const string openvino_dll_path = @"E:\Git_space\C#调用OpenVINO部署Al模型项目开发\OpenVinoSharp\CppOpenVinoAPI\dll\OpenVinoSharp.dll";

        [DllImport(openvino_dll_path, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr inference_engine_init(string model_file, string device_name);

        [DllImport(openvino_dll_path, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr set_input_image_sharp(IntPtr inference_engine, string input_node_name, ref ulong input_size);

        [DllImport(openvino_dll_path, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr set_input_data_sharp(IntPtr inference_engine, string input_node_name, ref ulong input_size);

        [DllImport(openvino_dll_path, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr load_image_input_data(IntPtr inference_engine, string input_node_name, ref byte image_data, ulong image_size);

        [DllImport(openvino_dll_path, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr load_input_data(IntPtr inference_engine, string input_node_name, ref float input_data);

        [DllImport(openvino_dll_path, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr inference_engine_infer(IntPtr inference_engine);

        [DllImport(openvino_dll_path, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void read_inference_result_F32(IntPtr inference_engine, string output_node_name, int data_size, ref float inference_result);

        [DllImport(openvino_dll_path, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void read_inference_result_I32(IntPtr inference_engine, string output_node_name, int data_size, ref int inference_result);

        [DllImport(openvino_dll_path, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void inference_engine_delet(IntPtr inference_engine);
    }
}
