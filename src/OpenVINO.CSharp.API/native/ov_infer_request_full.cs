// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        // Profiling Info
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_profiling_info",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_profiling_info(IntPtr infer_request, IntPtr profiling_infos);

        [DllImport("openvino_c", EntryPoint = "ov_profiling_info_list_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_profiling_info_list_free(IntPtr profiling_infos);

        // Callback
        //[DllImport("openvino_c", EntryPoint = "ov_infer_request_set_callback",
        //    CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        //public extern static ExceptionStatus ov_infer_request_set_callback(IntPtr infer_request, IntPtr callback);

        // Additional tensor methods
        [DllImport("openvino_c", EntryPoint = "ov_infer_request_set_tensor",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_tensor(IntPtr infer_request, string tensor_name, IntPtr tensor);

        [DllImport("openvino_c", EntryPoint = "ov_infer_request_get_tensor",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_tensor(IntPtr infer_request, string tensor_name, ref IntPtr tensor);
    }

    /// <summary>
    /// Callback structure for async inference
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ov_callback_t
    {
        public IntPtr callback_func;
        public IntPtr args;
    }
}
