using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    public struct ov_shape
    {
        public long rank;   //!< the rank of shape
        public IntPtr dims_ptr;  //!< the dims of shape

        public long[] get_dims()
        {
            long[] dims = new long[rank];
            Marshal.Copy(dims_ptr, dims, 0, (int)rank);
            return dims;
        }
    }
    public class Shape
    {
        public ov_shape shape;
        private IntPtr ptr;
        public IntPtr Ptr { get { return ptr; } set { ptr = value; } }
        public Shape(ov_shape shape)
        {
            this.shape = shape;
        }
        public long get_rank() 
        {
            return shape.rank;
        }
        public long[] get_dims() {
            return shape.get_dims();
        }
    }
}
