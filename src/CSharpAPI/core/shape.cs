using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using ov_shape = OpenVinoSharp.Ov.ov_shape;

namespace OpenVinoSharp
{

    /// <summary>
    /// Shape for a tensor.
    /// </summary>
    /// <ingroup>ov_runtime_c#_api</ingroup>
    public class Shape : List<long>
    {
        /// <summary>
        /// [struct] The shape ov_shape
        /// </summary>
        public ov_shape shape;
        /// <summary>
        /// [private]Shape class pointer.
        /// </summary>
        private IntPtr m_ptr = IntPtr.Zero;
        /// <summary>
        /// [public]Shape class pointer.
        /// </summary>
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }
        /// <summary>
        /// Constructs Shape from the initialized IntPtr.
        /// </summary>
        /// <param name="ptr">Initialized IntPtr</param>
        public Shape(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero) 
            {
                System.Diagnostics.Debug.WriteLine("Shape init error : ptr is null!");
                return;
            }
            this.m_ptr = ptr;
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
            int l = Marshal.SizeOf(typeof(ov_shape));
            m_ptr = Marshal.AllocHGlobal(l);
            HandleException.handler(
                NativeMethods.ov_shape_create((long)this.Count, ref axis_lengths.ToArray()[0], m_ptr));
            var temp = Marshal.PtrToStructure(m_ptr, typeof(ov_shape));
            shape = (ov_shape)temp;
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
            int l = Marshal.SizeOf(typeof(ov_shape));
            m_ptr = Marshal.AllocHGlobal(l);
            HandleException.handler(
                NativeMethods.ov_shape_create((long)this.Count, ref axis_lengths[0], m_ptr)）;
            var temp = Marshal.PtrToStructure(m_ptr, typeof(ov_shape));
            shape = (ov_shape)temp;
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
            if (m_ptr == IntPtr.Zero)
            {
                return;
            }
           NativeMethods.ov_core_free(m_ptr);
            m_ptr = IntPtr.Zero;
        }
        /// <summary>
        /// Convert shape to string.
        /// </summary>
        /// <returns>shape string</returns>
        public string to_string() 
        {
            if (this.Count < 1) 
            {
                return "NULL";
            }
            string s = "Shape : {";
            foreach(var i in this)
            {
                s += i.ToString() + ", ";
            }
            s = s.Substring(0, s.Length - 2);
            s += "}";
            return s;
        }
    }
}
