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

        public Tensor(element.Type type, Shape shape)
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_tensor_create
                ((uint)type.get_type(), shape.shape, ref m_ptr);
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
            NativeMethods.ov_tensor_free(m_ptr);
            m_ptr = IntPtr.Zero;
        }

        void set_shape(Shape shape) 
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_tensor_set_shape(m_ptr, shape.shape);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Tensor set_shape error : " + status.ToString());
            }
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
                System.Diagnostics.Debug.WriteLine("Tensor get_shape error : " + status.ToString());
                shape_ptr = IntPtr.Zero;
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
                System.Diagnostics.Debug.WriteLine("Tensor get_element_type error : " + status.ToString());
                type = 0;
            }
            element.Type t = new element.Type((Type_t)type);
            return t;
        }

        public ulong get_size() 
        {
            ulong size = 0;
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_tensor_get_size(m_ptr, ref size);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Tensor get_size error : " + status.ToString());
            }
            return size;
        }
        public ulong get_byte_size()
        {
            ulong size = 0;
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_tensor_get_byte_size(m_ptr, ref size);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Tensor get_byte_size error : " + status.ToString());
            }
            return size;
        }
        public IntPtr data()
        {
            IntPtr data_ptr = new IntPtr();
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_tensor_data(m_ptr, ref data_ptr);
            return data_ptr;
        }


        public void set_data<T>(T[] input_data)
        {
            IntPtr data_ptr = new IntPtr();
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_tensor_data(m_ptr, ref data_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Tensor dispose error : " + status.ToString());
                return;
            }
            int length = input_data.Length;

            string t = typeof(T).ToString();
            if (t == "System.Byte")
            {
                float[] data = (float[])Convert.ChangeType(input_data, typeof(float[]));
                Marshal.Copy(data, 0, data_ptr, length);
            }
            else if (t == "System.Int32")
            {
                int[] data = (int[])Convert.ChangeType(input_data, typeof(int[]));
                Marshal.Copy(data, 0, data_ptr, length);
            }
            else if (t == "System.Int64")
            {
                long[] data = (long[])Convert.ChangeType(input_data, typeof(long[]));
                Marshal.Copy(data, 0, data_ptr, length);
            }
            else if (t == "System.Int16")
            {
                short[] data = (short[])Convert.ChangeType(input_data, typeof(short[]));
                Marshal.Copy(data, 0, data_ptr, length);
            }
            else if (t == "System.Single")
            {
                float[] data = (float[])Convert.ChangeType(input_data, typeof(float[]));
                Marshal.Copy(data, 0, data_ptr, length);
            }
            else if (t == "System.Double")
            {
                double[] data = (double[])Convert.ChangeType(input_data, typeof(double[]));
                Marshal.Copy(data, 0, data_ptr, length);
            }
            else 
            {
                Console.WriteLine("Data format error, not supported. Only double, flaot, int, long, shaort and byte data formats are supported");
            }
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
            else if (t == "System.Int32")
            {
                int[] data = new int[length];
                Marshal.Copy(data_ptr, data, 0, length);
                result = (T[])Convert.ChangeType(data, typeof(T[]));
                return result;
            }
            else if (t == "System.Int64")
            {
                long[] data = new long[length];
                Marshal.Copy(data_ptr, data, 0, length);
                result = (T[])Convert.ChangeType(data, typeof(T[]));
                return result;
            }
            else if (t == "System.Int16")
            {
                short[] data = new short[length];
                Marshal.Copy(data_ptr, data, 0, length);
                result = (T[])Convert.ChangeType(data, typeof(T[]));
                return result;
            }
            else if (t == "System.Single")
            {
                float[] data = new float[length];
                Marshal.Copy(data_ptr, data, 0, length);
                result = (T[])Convert.ChangeType(data, typeof(T[]));
                return result;
            }
            else if (t == "System.Double")
            {
                double[] data = new double[length];
                Marshal.Copy(data_ptr, data, 0, length);
                result = (T[])Convert.ChangeType(data, typeof(T[]));
                return result;
            }
            else
            {
                Console.WriteLine("Data format error, not supported. Only double, flaot, int, long, shaort and byte data formats are supported");
                return result;
            }

        }

    }
}
