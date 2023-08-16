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
        /// Create a layout object.
        /// </summary>
        /// <param name="layout_desc">The description of layout.</param>
        /// <param name="layout">The layout input pointer.</param>
        /// <returns>a status code, return OK if successful</returns>
        [DllImport(dll_extern, EntryPoint = "ov_layout_create", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_layout_create(
            ref sbyte layout_desc, 
            ref IntPtr layout);

        /// <summary>
        /// Free layout object.
        /// </summary>
        /// <param name="layout">The pointer of layout.</param>
        [DllImport(dll_extern, EntryPoint = "ov_layout_free", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_layout_free(IntPtr layout);

        /// <summary>
        /// Convert layout object to a readable string.
        /// </summary>
        /// <param name="layout">layout will be converted.</param>
        /// <returns>string that describes the layout content.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_layout_to_string", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static string ov_layout_to_string(IntPtr layout);
    }
}
