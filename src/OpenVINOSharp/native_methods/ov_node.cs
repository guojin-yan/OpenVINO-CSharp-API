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
        /// Get the shape of port object.
        /// </summary>
        /// <param name="port">A pointer to ov_output_const_port_t.</param>
        /// <param name="tensor_shape">tensor shape.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_const_port_get_shape", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_const_port_get_shape(
            IntPtr port, 
            IntPtr tensor_shape);

        /// <summary>
        /// Get the shape of port object.
        /// </summary>
        /// <param name="port">A pointer to ov_output_port_t.</param>
        /// <param name="tensor_shape">A pointer to the tensor name.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_const_port_get_shape",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_port_get_shape(
            IntPtr port, 
            IntPtr tensor_shape);
        /// <summary>
        /// Get the tensor name of port.
        /// </summary>
        /// <param name="port">A pointer to the ov_output_const_port_t.</param>
        /// <param name="tensor_name">A pointer to the tensor name.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_port_get_any_name", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_port_get_any_name(
            IntPtr port, 
            ref IntPtr tensor_name);

        /// <summary>
        /// Get the partial shape of port.
        /// </summary>
        /// <param name="port">A pointer to the ov_output_const_port_t.</param>
        /// <param name="partial_shape">Partial shape.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_port_get_partial_shape",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_port_get_partial_shape(
            IntPtr port, 
            IntPtr partial_shape);

        /// <summary>
        /// Get the tensor type of port.
        /// </summary>
        /// <param name="port">A pointer to the ov_output_const_port_t.</param>
        /// <param name="tensor_type">tensor type.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_port_get_element_type", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_port_get_element_type(
            IntPtr port, 
            ref uint tensor_type);

        /// <summary>
        /// free port object
        /// </summary>
        /// <param name="port">The pointer to the instance of the ov_output_port_t to free.</param>
        [DllImport(dll_extern, EntryPoint = "ov_output_port_free", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_output_port_free(
            IntPtr port);

        /// <summary>
        /// free const port
        /// </summary>
        /// <param name="port">The pointer to the instance of the ov_output_const_port_t to free.</param>
        [DllImport(dll_extern, EntryPoint = "ov_output_const_port_free", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_output_const_port_free(
            IntPtr port);
    }
}
