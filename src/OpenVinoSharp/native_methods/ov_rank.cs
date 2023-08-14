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
        /// This is a structure interface equal to ov::Rank
        /// </summary>
        public struct ov_rank
        {
            /// <summary>
            /// The lower inclusive limit for the Rank.
            /// </summary>
            long min;
            /// <summary>
            /// The upper inclusive limit for the Rank.
            /// </summary>
            long max;
        };
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
