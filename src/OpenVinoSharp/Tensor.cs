using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    public class Tensor
    {
        private IntPtr ptr;
        public IntPtr Ptr { get { return ptr; } set { ptr = value; } }
        public Tensor(IntPtr ptr)
        {
            this.ptr = ptr;
        }
        public Shape get_shape() 
        {
            int l = Marshal.SizeOf(typeof(ov_shape));
            IntPtr shape_ptr = Marshal.AllocHGlobal(l);
            ov_status_e status = (ov_status_e)NativeMethods.ov_tensor_get_shape(ptr, shape_ptr);
            Console.WriteLine("Tensor get_shape: " + status);
            var temp = Marshal.PtrToStructure(shape_ptr, typeof(ov_shape));
            return new Shape((ov_shape)temp);
        }
        public uint get_element_type() 
        {
            uint type = 100;
            ov_status_e status = (ov_status_e)NativeMethods.ov_tensor_get_element_type(ptr, out type);
            Console.WriteLine("Tensor get_element_type(): " + status);
            return type;
        }
        //public IntPtr get_data() 
        //{
        //    IntPtr data_ptr = new IntPtr();
        //    ov_status_e status = (ov_status_e)NativeMethods.ov_tensor_data(ptr, ref data_ptr);
        //    return data_ptr;
        //}
        public void set_data(float[] data)
        {
            int length = data.Length;
            IntPtr data_ptr = new IntPtr();
            ov_status_e status = (ov_status_e)NativeMethods.ov_tensor_data(ptr, ref data_ptr);
            System.Diagnostics.Debug.WriteLine("Tensor set_data(): " + status);
            Marshal.Copy(data, 0, data_ptr, length);
        }
        public float[] get_data(int length)
        {
            IntPtr data_ptr = new IntPtr();
            ov_status_e status = (ov_status_e)NativeMethods.ov_tensor_data(ptr, ref data_ptr);
            System.Diagnostics.Debug.WriteLine("Tensor set_data(): " + status);
            float[] data = new float[length];
            Marshal.Copy(data_ptr, data, 0, length);
            return data;
        }
    }
}
