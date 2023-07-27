using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{

    /// <summary>
    /// Shape for a tensor.
    /// </summary>
    /// <ingroup>ov_runtime_c#_api</ingroup>
    public class Shape : List<long>
    {
        /// <summary>
        /// Reprents a static shape.
        /// </summary>
        public struct ov_shape
        {
            /// <summary>
            /// the rank of shape
            /// </summary>
            public long rank;
            /// <summary>
            /// the dims of shape
            /// </summary>
            public IntPtr dims_ptr;
            /// <summary>
            /// Get the dims of shape
            /// </summary>
            /// <returns>the dims of shape</returns>
            public long[] get_dims()
            {
                long[] dims = new long[rank];
                Marshal.Copy(dims_ptr, dims, 0, (int)rank);
                return dims;
            }
        }

        public ov_shape shape;
        private IntPtr ptr = IntPtr.Zero;
        public IntPtr Ptr { get { return ptr; } set { ptr = value; } }
        /// <summary>
        /// Constructs Shape from the initialized IntPtr.
        /// </summary>
        /// <param name="ptr">Initialized IntPtr</param>
        public Shape(IntPtr ptr)
        {
            this.ptr = ptr;
            var temp = Marshal.PtrToStructure(ptr, typeof(ov_shape));
            shape = (ov_shape)temp;
            long[] dims = shape.get_dims();
            for (int i = 0; i < shape.rank; ++i)
            {
                this.Add(dims[i]);
            }
        }
        /// <summary>
        /// Constructs Shape from the list.
        /// </summary>
        /// <param name="axis_lengths">Initialized list</param>
        public Shape(List<long> axis_lengths)
        {
         
            for (int i = 0; i < axis_lengths.Count; ++i)
            {
                this.Add(axis_lengths[i]);
            }
            ExceptionStatus status = 
                (ExceptionStatus)NativeMethods.ov_shape_create((long)this.Count, ref axis_lengths.ToArray()[0], ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Shape init error!");
            }
        }
        /// <summary>
        /// Constructs Shape from the initialized array.
        /// </summary>
        /// <param name="axis_lengths">Initialized array</param>
        public Shape(long[] axis_lengths)
        {

            for (int i = 0; i < axis_lengths.Length; ++i)
            {
                this.Add(axis_lengths[i]);
            }
            ExceptionStatus status =
                (ExceptionStatus)NativeMethods.ov_shape_create((long)this.Count, ref axis_lengths[0], ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Shape init error!");
            }
        }
        /// <summary>
        /// Shape's destructor
        /// </summary>
        ~Shape() 
        {
            dispose();
        }
        /// <summary>
        /// Release unmanaged resources
        /// </summary>
        public void dispose()
        {
            if (ptr == IntPtr.Zero)
            {
                return;
            }
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_core_free(ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Core free error!");
                return;
            }
            ptr = IntPtr.Zero;
        }
        /// <summary>
        /// Convert shape to string.
        /// </summary>
        /// <returns>shape string</returns>
        public string to_string() 
        {
            string s = "Shape : [";
            foreach(var i in this)
            {
                s += i.ToString() + ", ";
            }
            s = s.Substring(0, s.Length - 2);
            s += "]";
            return s;
        }
    }
}
