using OpenVinoSharp.element;
using OpenVinoSharp.preprocess;
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
        private IntPtr m_ptr = IntPtr.Zero;
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }

        /// <summary>
        /// Constructs Tensor from the initialized std::shared_ptr
        /// </summary>
        /// <param name="ptr"></param>
        public Tensor(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                System.Diagnostics.Debug.WriteLine("Tensor init error : ptr is null!");
                return;
            }
            this.m_ptr = ptr;
        }

        public Tensor(element.Type type, Shape shape, OvMat mat) 
        {
            int l =mat.mat_data.Length;
            IntPtr data = Marshal.AllocHGlobal(l);
            Marshal.Copy(mat.mat_data, 0, data, (int)mat.mat_data_size);
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_tensor_create_from_host_ptr
                ((uint)type.get_type(), shape.shape, data, ref m_ptr);
            if (status != 0)
            {
                m_ptr = IntPtr.Zero;
                System.Diagnostics.Debug.WriteLine("Tensor init error : " + status.ToString());
            }
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
            if (m_ptr == IntPtr.Zero)
            {
                return;
            }
            NativeMethods.ov_core_free(m_ptr);
            m_ptr = IntPtr.Zero;
        }
        /// <summary>
        /// Get tensor shape
        /// </summary>
        /// <returns>A tensor shape</returns>
        public Shape get_shape() 
        {
            int l = Marshal.SizeOf(typeof(Shape.ov_shape));
            IntPtr shape_ptr = Marshal.AllocHGlobal(l);
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_tensor_get_shape(m_ptr, shape_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Tensor get_shape error : {0}!", status.ToString());
                shape_ptr= IntPtr.Zero;
            }

            return new Shape(shape_ptr);
        }
        /// <summary>
        /// Get tensor element type
        /// </summary>
        /// <returns>A tensor element type</returns>
        public element.Type get_element_type() 
        {
            uint type = 100;
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_tensor_get_element_type(m_ptr, out type);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Tensor get_element_type error : {0}!", status.ToString());
                type = 0;
            }
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
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_tensor_data(m_ptr, ref data_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Tensor dispose error : " + status.ToString());
            }
            Marshal.Copy(data, 0, data_ptr, length);
        }
        public T[] get_data<T>(int length)
        {
            IntPtr data_ptr = new IntPtr();
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_tensor_data(m_ptr, ref data_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Tensor dispose error : " + status.ToString());
            }
            string t = typeof(T).ToString();
            T[] result = new T[length];
            if (t == "System.Byte")
            {
                byte[] data = new byte[length];
                Marshal.Copy(data_ptr, data, 0, length);
                result = (T[])Convert.ChangeType(data, typeof(T[]));
                return result;
            }
            else
            {
                float[] data = new float[length];
                Marshal.Copy(data_ptr, data, 0, length);
                result = (T[])Convert.ChangeType(data, typeof(T[]));
                return result;
            }
        }

    }
}
