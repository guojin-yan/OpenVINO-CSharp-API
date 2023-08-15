using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using ov_rank = OpenVinoSharp.Ov.ov_rank;
namespace OpenVinoSharp
{
    
    public partial class NativeMethods
    {

        /// <summary>
        /// Check this rank whether is dynamic
        /// </summary>
        /// <param name="rank">The rank pointer that will be checked.</param>
        /// <returns>The return value.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_rank_is_dynamic",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static bool ov_rank_is_dynamic(ov_rank rank);
    }
}
