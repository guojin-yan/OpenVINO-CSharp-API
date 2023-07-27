﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    public partial class NativeMethods
    {

        [DllImport(dll_extern, EntryPoint = "ov_tensor_get_shape", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_tensor_get_shape(IntPtr tensor, IntPtr shape);
        [DllImport(dll_extern, EntryPoint = "ov_tensor_get_element_type", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_tensor_get_element_type(IntPtr tensor, out uint type);
        [DllImport(dll_extern, EntryPoint = "ov_tensor_data", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_tensor_data(IntPtr tensor, ref IntPtr data);
    }
}