using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    public partial class NativeMethods
    {
        /// <summary>
        /// Set an input/output tensor to infer on by the name of tensor.
        /// </summary>
        /// <param name="infer_request">A pointer to the ov_infer_request_t.</param>
        /// <param name="tensor_name">Name of the input or output tensor.</param>
        /// <param name="tensor">Reference to the tensor.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_set_tensor", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_tensor(
            IntPtr infer_request, 
            ref sbyte tensor_name,
            IntPtr tensor);

        /// <summary>
        /// Set an input/output tensor to infer request for the port.
        /// </summary>
        /// <param name="infer_request">A pointer to the ov_infer_request_t.</param>
        /// <param name="port">Port of the input or output tensor, which can be got by calling ov_model_t/ov_compiled_model_t interface.</param>
        /// <param name="tensor"></param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_set_tensor_by_port", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_tensor_by_port(
            IntPtr infer_request,
            IntPtr port,
            IntPtr tensor);
        /// <summary>
        /// Set an input/output tensor to infer request for the port.
        /// </summary>
        /// <param name="infer_request">A pointer to the ov_infer_request_t.</param>
        /// <param name="port">Const port of the input or output tensor, which can be got by call interface from ov_model_t/ov_compiled_model_t.</param>
        /// <param name="tensor">Reference to the tensor.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_set_tensor_by_const_port", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_tensor_by_const_port(
            IntPtr infer_request,
            IntPtr port,
            IntPtr tensor);
        /// <summary>
        /// Set an input tensor to infer on by the index of tensor.
        /// </summary>
        /// <param name="infer_request">A pointer to the ov_infer_request_t.</param>
        /// <param name="idx">Index of the input port. If @p idx is greater than the number of model inputs, an error will return.</param>
        /// <param name="tensor">Reference to the tensor.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_set_input_tensor_by_index",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_input_tensor_by_index(
            IntPtr infer_request,
            ulong idx,
            IntPtr tensor);
        /// <summary>
        /// Set an input tensor for the model with single input to infer on.
        /// </summary>
        /// <param name="infer_request">A pointer to the ov_infer_request_t.</param>
        /// <param name="tensor">Reference to the tensor.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_set_input_tensor", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_input_tensor(
            IntPtr infer_request,
            IntPtr tensor);
        /// <summary>
        /// Set an output tensor to infer by the index of output tensor.
        /// </summary>
        /// <param name="infer_request">A pointer to the ov_infer_request_t.</param>
        /// <param name="idx">Index of the output tensor.</param>
        /// <param name="tensor">Reference to the tensor.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_set_output_tensor_by_index", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_output_tensor_by_index(
            IntPtr infer_request,
            ulong idx,
            IntPtr tensor);
        /// <summary>
        /// Set an output tensor to infer models with single output.
        /// </summary>
        /// <param name="infer_request">A pointer to the ov_infer_request_t.</param>
        /// <param name="tensor">Reference to the tensor.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_set_output_tensor", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_output_tensor(
            IntPtr infer_request, 
            IntPtr tensor);
        /// <summary>
        /// Get an input/output tensor by the name of tensor.
        /// </summary>
        /// <param name="infer_request">A pointer to the ov_infer_request_t.</param>
        /// <param name="tensor_name">Name of the input or output tensor to get.</param>
        /// <param name="tensor">Reference to the tensor.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_get_tensor",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_tensor(
            IntPtr infer_request,
            ref sbyte tensor_name,
            ref IntPtr tensor);
        /// <summary>
        /// Get an input/output tensor by const port.
        /// </summary>
        /// <param name="infer_request">A pointer to the ov_infer_request_t.</param>
        /// <param name="port">Port of the tensor to get. @p port is not found, an error will return.</param>
        /// <param name="tensor">Reference to the tensor.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_get_tensor_by_const_port",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_tensor_by_const_port(
            IntPtr infer_request,
            IntPtr port,
            ref IntPtr tensor);
        /// <summary>
        /// Get an input/output tensor by port.
        /// </summary>
        /// <param name="infer_request">A pointer to the ov_infer_request_t.</param>
        /// <param name="port">Port of the tensor to get. @p port is not found, an error will return.</param>
        /// <param name="tensor">Reference to the tensor.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_get_tensor_by_port",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_tensor_by_port(
            IntPtr infer_request,
            IntPtr port,
            ref IntPtr tensor);
        /// <summary>
        /// Get an input tensor by the index of input tensor.
        /// </summary>
        /// <param name="infer_request">A pointer to the ov_infer_request_t.</param>
        /// <param name="idx">ndex of the tensor to get. @p idx. If the tensor with the specified @p idx is not found, an error will return.</param>
        /// <param name="tensor">Reference to the tensor.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_get_input_tensor_by_index",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_input_tensor_by_index(
            IntPtr infer_request,
            ulong idx,
            ref IntPtr tensor);
        /// <summary>
        /// Get an input tensor from the model with only one input tensor.
        /// </summary>
        /// <param name="infer_request">A pointer to the ov_infer_request_t.</param>
        /// <param name="tensor">Reference to the tensor.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_get_input_tensor",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_input_tensor(
            IntPtr infer_request, 
            ref IntPtr tensor);
        /// <summary>
        /// Get an output tensor by the index of output tensor.
        /// </summary>
        /// <param name="infer_request">A pointer to the ov_infer_request_t.</param>
        /// <param name="idx">ndex of the tensor to get. @p idx. If the tensor with the specified @p idx is not found, an error will return.</param>
        /// <param name="tensor">Reference to the tensor.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_get_output_tensor_by_index",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_output_tensor_by_index(
            IntPtr infer_request,
            ulong idx,
            ref IntPtr tensor);
        /// <summary>
        /// Get an output tensor from the model with only one output tensor.
        /// </summary>
        /// <param name="infer_request">A pointer to the ov_infer_request_t.</param>
        /// <param name="tensor">Reference to the tensor.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_get_output_tensor",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_output_tensor(
            IntPtr infer_request, 
            ref IntPtr tensor);
        /// <summary>
        /// Infer specified input(s) in synchronous mode.
        /// </summary>
        /// <param name="infer_request">A pointer to the ov_infer_request_t.</param>
        /// <returns>Status code of the operation: OK(0) for success..</returns>
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_infer", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_infer(
            IntPtr infer_request);

        /// <summary>
        /// Cancel inference request.
        /// </summary>
        /// <param name="infer_request">A pointer to the ov_infer_request_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_cancel",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_cancel(IntPtr infer_request);


        /// <summary>
        /// Start inference of specified input(s) in asynchronous mode.
        /// </summary>
        /// <param name="infer_request">A pointer to the ov_infer_request_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_start_async",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_start_async(IntPtr infer_request);

        /// <summary>
        /// Wait for the result to become available.
        /// </summary>
        /// <param name="infer_request">A pointer to the ov_infer_request_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_wait",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_wait(IntPtr infer_request);

        /// <summary>
        /// Waits for the result to become available. Blocks until the specified timeout has elapsed or the result becomes available, 
        /// whichever comes first.
        /// </summary>
        /// <param name="infer_request">A pointer to the ov_infer_request_t.</param>
        /// <param name="timeout">Maximum duration, in milliseconds, to block for.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_wait_for",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_wait_for(IntPtr infer_request, long timeout);

        /// <summary>
        /// Set callback function, which will be called when inference is done.
        /// </summary>
        /// <param name="infer_request">A pointer to the ov_infer_request_t.</param>
        /// <param name="callback">A function to be called.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_set_callback",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_set_callback(IntPtr infer_request, IntPtr callback);

        /// <summary>
        /// Release the memory allocated by ov_infer_request_t.
        /// </summary>
        /// <param name="infer_request">A pointer to the ov_infer_request_t to free memory.</param>
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_free",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_infer_request_free(IntPtr infer_request);

        /// <summary>
        /// Query performance measures per layer to identify the most time consuming operation.
        /// </summary>
        /// <param name="infer_request">A pointer to the ov_infer_request_t.</param>
        /// <param name="profiling_infos">Vector of profiling information for operations in a model.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_infer_request_get_profiling_info",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_infer_request_get_profiling_info(IntPtr infer_request, IntPtr profiling_infos);

        /// <summary>
        /// Release the memory allocated by ov_profiling_info_list_t.
        /// </summary>
        /// <param name="profiling_infos">A pointer to the ov_profiling_info_list_t to free memory.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_profiling_info_list_free",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_profiling_info_list_free(IntPtr profiling_infos);


    }
}
