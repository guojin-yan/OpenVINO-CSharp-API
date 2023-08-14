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
        /// This is a structure interface equal to ov::Dimension
        /// </summary>
        public struct ov_dimension
        {
            /// <summary>
            /// The lower inclusive limit for the dimension.
            /// </summary>
            long min;
            /// <summary>
            /// The upper inclusive limit for the dimension.
            /// </summary>
            long max;
        };

        /// <summary>
        /// Check this dimension whether is dynamic
        /// </summary>
        /// <param name="dim">The dimension pointer that will be checked.</param>
        /// <returns>Boolean, true is dynamic and false is static.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_dimension_is_dynamic",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static bool ov_dimension_is_dynamic(ov_dimension dim);

    }
}
