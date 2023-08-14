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
        /// It represents a shape that may be partially or totally dynamic.
        /// </summary>
        /// <remarks>
        /// <para>Dynamic rank. (Informal notation: `?`)</para>
        /// <para>Static rank, but dynamic dimensions on some or all axes.
        /// (Informal notation examples: `{1,2,?,4}`, `{?,?,?}`)</para>
        /// <para> Static rank, and static dimensions on all axes.
        /// (Informal notation examples: `{1,2,3,4}`, `{6}`, `{}`)</para>
        /// </remarks>
        public struct ov_partial_shape
        {
            /// <summary>
            /// The rank
            /// </summary>
            public ov_rank rank;
            /// <summary>
            /// The dimension
            /// </summary>
            public IntPtr dims = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ov_dimension)));
            /// <summary>
            /// Default Constructor
            /// </summary>
            public ov_partial_shape() 
            {
                rank = new ov_rank();
                dims = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ov_dimension)));
            }
        }
    }
}
