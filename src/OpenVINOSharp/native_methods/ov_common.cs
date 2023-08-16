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
        /// <summary>
        /// Print the error info.
        /// </summary>
        /// <param name="status">a status code.</param>
        /// <returns>error info.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_get_error_info", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static string ov_get_error_info(int status);

        /// <summary>
        /// free char
        /// </summary>
        /// <param name="content">The pointer to the char to free.</param>
        [DllImport(dll_extern, EntryPoint = "ov_free", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_free(ref char content);

    }
}
