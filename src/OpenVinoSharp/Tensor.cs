using OpenVinoSharp.element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    /// <summary>
    /// Tensor API holding host memory.
    /// It can throw exceptions safely for the application, where it is properly handled.
    /// </summary>
    /// <ingroup>ov_runtime_c#_api</ingroup>
    public class Tensor
    {
        private IntPtr ptr = IntPtr.Zero;
        public IntPtr Ptr { get { return ptr; } set { ptr = value; } }

        /// <summary>
        /// Constructs Tensor from the initialized std::shared_ptr
        /// </summary>
        /// <param name="ptr"></param>
        public Tensor(IntPtr ptr)
        {
            this.ptr = ptr;
        }
        /// <summary>
        /// Tensor's destructor
        /// </summary>
        ~Tensor()
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
        /// Get tensor shape
        /// </summary>
        /// <returns>A tensor shape</returns>
        public Shape get_shape() 
        {
            int l = Marshal.SizeOf(typeof(Shape.ov_shape));
            IntPtr shape_ptr = Marshal.AllocHGlobal(l);
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_tensor_get_shape(ptr, shape_ptr);
            Console.WriteLine("Tensor get_shape: " + status);
            
            return new Shape(shape_ptr);
        }
        /// <summary>
        /// Get tensor element type
        /// </summary>
        /// <returns>A tensor element type</returns>
        public element.Type get_element_type() 
        {
            uint type = 100;
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_tensor_get_element_type(ptr, out type);
            Console.WriteLine("Tensor get_element_type(): " + status);
            element.Type t = new element.Type((Type_t)type);
            return t;
        }
        //public IntPtr get_data() 
        //{
        //    IntPtr data_ptr = new IntPtr();
        //    ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_tensor_data(ptr, ref data_ptr);
        //    return data_ptr;
        //}
        public void set_data(float[] data)
        {
            int length = data.Length;
            IntPtr data_ptr = new IntPtr();
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_tensor_data(ptr, ref data_ptr);
            System.Diagnostics.Debug.WriteLine("Tensor set_data(): " + status);
            Marshal.Copy(data, 0, data_ptr, length);
        }
        public float[] get_data(int length)
        {
            IntPtr data_ptr = new IntPtr();
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_tensor_data(ptr, ref data_ptr);
            System.Diagnostics.Debug.WriteLine("Tensor set_data(): " + status);
            float[] data = new float[length];
            Marshal.Copy(data_ptr, data, 0, length);
            return data;
        }
    }
}
