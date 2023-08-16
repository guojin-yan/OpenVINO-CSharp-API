using OpenVinoSharp.element;
using OpenVinoSharp.preprocess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
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
        /// <summary>
        /// [private]Tensor class pointer.
        /// </summary>
        private IntPtr m_ptr = IntPtr.Zero;
        /// <summary>
        /// [public]Tensor class pointer.
        /// </summary>
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }

        /// <summary>
        /// Constructs Tensor from the initialized pointer.
        /// </summary>
        /// <param name="ptr">Tensor pointer.</param>
        public Tensor(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                System.Diagnostics.Debug.WriteLine("Tensor init error : ptr is null!");
                return;
            }
            this.m_ptr = ptr;
        }

        /// <summary>
        /// Constructs Tensor using element type ,shape and image data. 
        /// </summary>
        /// <param name="type">Tensor element type</param>
        /// <param name="shape">Tensor shape</param>
        /// <param name="mat">Image data</param>
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
        /// Constructs Tensor using element type and shape. Wraps allocated host memory.
        /// </summary>
        /// <remarks>Does not perform memory allocation internally.</remarks>
        /// <param name="type">Tensor element type</param>
        /// <param name="shape">Tensor shape</param>
        /// <param name="host_ptr">Pointer to pre-allocated host memory</param>
        public Tensor(element.Type type, Shape shape, IntPtr host_ptr)
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_tensor_create_from_host_ptr
                ((uint)type.get_type(), shape.shape, host_ptr, ref m_ptr);
            if (status != 0)
            {
                m_ptr = IntPtr.Zero;
                System.Diagnostics.Debug.WriteLine("Tensor init error : " + status.ToString());
            }
        }

        /// <summary>
        /// Constructs Tensor using element type and shape. Allocate internal host storage using default allocator
        /// </summary>
        /// <param name="type">Tensor element type</param>
        /// <param name="shape">Tensor shape</param>
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
        /// Default copy constructor
        /// </summary>
        /// <param name="tensor">other Tensor object</param>
        public Tensor(Tensor tensor) 
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_tensor_create_from_host_ptr
                ((uint)tensor.get_element_type().get_type(), tensor.get_shape().shape, tensor.data(), ref m_ptr);
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
        /// <summary>
        /// Set new shape for tensor, deallocate/allocate if new total size is bigger than previous one.
        /// </summary>
        /// <exception cref="">Memory allocation may happen</exception>
        /// <param name="shape"> A new shape</param>
        public void set_shape(Shape shape) 
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
            int l = Marshal.SizeOf(typeof(Ov.ov_shape));
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
        public OvType get_element_type() 
        {
            uint type = 100;
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_tensor_get_element_type(m_ptr, out type);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Tensor get_element_type error : " + status.ToString());
                type = 0;
            }
            element.Type t = new element.Type((Type_t)type);
            return t as OvType;
        }

        /// <summary>
        /// Returns the total number of elements (a product of all the dims or 1 for scalar).
        /// </summary>
        /// <returns>The total number of elements.</returns>
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

        /// <summary>
        /// Returns the size of the current Tensor in bytes.
        /// </summary>
        /// <returns>Tensor's size in bytes</returns>
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

        /// <summary>
        /// Copy tensor, destination tensor should have the same element type and shape
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="dst">destination tensor</param>
        public void copy_to<T>(Tensor dst) 
        {
            ulong length = this.get_size();
            T[] data = this.get_data<T>((int)length);
            dst.set_data<T>(data);
        }

        /// <summary>
        /// Provides an access to the underlaying host memory.
        /// </summary>
        /// <returns>A host pointer to tensor memory.</returns>
        public IntPtr data()
        {
            IntPtr data_ptr = new IntPtr();
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_tensor_data(m_ptr, ref data_ptr);
            return data_ptr;
        }

        /// <summary>
        /// Load the specified type of data into the underlying host memory.
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="input_data">Data to be loaded.</param>
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

        /// <summary>
        /// Read data of the specified type from the underlying host memory.
        /// </summary>
        /// <typeparam name="T">Type of data read.</typeparam>
        /// <param name="length">The length of the read data.</param>
        /// <returns>Read data.</returns>
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
