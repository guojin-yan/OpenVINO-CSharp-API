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
        /// Constructs Tensor using element type and shape. Allocate internal host storage using default allocator.
        /// </summary>
        /// <param name="type">Tensor element type.</param>
        /// <param name="shape">Tensor shape.</param>
        /// <param name="host_ptr">Pointer to pre-allocated host memory.</param>
        /// <param name="tensor">A point to ov_tensor_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_tensor_create_from_host_ptr", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_create_from_host_ptr(
            uint type,
            Ov.ov_shape shape, 
            IntPtr host_ptr, 
            ref IntPtr tensor);

        /// <summary>
        /// Constructs Tensor using element type and shape. Allocate internal host storage using default allocator.
        /// </summary>
        /// <param name="type">Tensor element type</param>
        /// <param name="shape">Tensor shape.</param>
        /// <param name="tensor">A point to ov_tensor_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_tensor_create",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_create(
            uint type, 
            Ov.ov_shape shape, 
            ref IntPtr tensor);
        /// <summary>
        /// Set new shape for tensor, deallocate/allocate if new total size is bigger than previous one.
        /// </summary>
        /// <param name="tensor">A point to ov_tensor_t..</param>
        /// <param name="shape">Tensor shape.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_tensor_set_shape",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_set_shape(
                IntPtr tensor, 
                Ov.ov_shape shape);

        /// <summary>
        /// Get shape for tensor.
        /// </summary>
        /// <param name="tensor">A point to ov_tensor_t.</param>
        /// <param name="shape">Tensor shape.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_tensor_get_shape",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_get_shape(
            IntPtr tensor, 
            IntPtr shape);

        /// <summary>
        /// Get type for tensor.
        /// </summary>
        /// <param name="tensor">A point to ov_tensor_t.</param>
        /// <param name="type">Tensor element type.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_tensor_get_element_type",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_get_element_type(
            IntPtr tensor, 
            out uint type);

        /// <summary>
        /// the total number of elements (a product of all the dims or 1 for scalar).
        /// </summary>
        /// <param name="tensor">A point to ov_tensor_t.</param>
        /// <param name="elements_size">number of elements.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_tensor_get_size",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_get_size(
            IntPtr tensor, 
            ref ulong elements_size);

        /// <summary>
        /// the size of the current Tensor in bytes.
        /// </summary>
        /// <param name="tensor">A point to ov_tensor_t</param>
        /// <param name="byte_size">the size of the current Tensor in bytes.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_tensor_get_byte_size",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_get_byte_size(
            IntPtr tensor,
            ref ulong byte_size);

        /// <summary>
        /// Provides an access to the underlaying host memory.
        /// </summary>
        /// <param name="tensor">A point to ov_tensor_t</param>
        /// <param name="data">A point to host memory.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_tensor_data", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_tensor_data(
            IntPtr tensor, 
            ref IntPtr data);

        /// <summary>
        /// Free ov_tensor_t.
        /// </summary>
        /// <param name="tensor">A point to ov_tensor_t</param>
        [DllImport(dll_extern, EntryPoint = "ov_tensor_free",
             CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_tensor_free(IntPtr tensor);
    }
}
