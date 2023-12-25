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
        /// Get the input size of ov_compiled_model_t.
        /// </summary>
        /// <param name="compiled_model">A pointer to the ov_compiled_model_t.</param>
        /// <param name="size">the compiled_model's input size.</param>
        /// <returns></returns>
        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_inputs_size", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_inputs_size(
            IntPtr compiled_model, ref ulong size);

        /// <summary>
        /// Get the single const input port of ov_compiled_model_t, which only support single input model.
        /// </summary>
        /// <param name="compiled_model">A pointer to the ov_compiled_model_t.</param>
        /// <param name="input_port">A pointer to the ov_output_const_port_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_input", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_input(
            IntPtr compiled_model, ref IntPtr input_port);

        /// <summary>
        /// Get a const input port of ov_compiled_model_t by port index.
        /// </summary>
        /// <param name="compiled_model">A pointer to the ov_compiled_model_t.</param>
        /// <param name="index">input index.</param>
        /// <param name="input_port">A pointer to the ov_output_const_port_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_input_by_index", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_input_by_index(
            IntPtr compiled_model,
            ulong index,
            ref IntPtr input_port);

        /// <summary>
        /// Get a const input port of ov_compiled_model_t by name.
        /// </summary>
        /// <param name="compiled_model">A pointer to the ov_compiled_model_t.</param>
        /// <param name="name">nput tensor name (char *).</param>
        /// <param name="input_port">A pointer to the ov_output_const_port_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_input_by_name",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_input_by_name(
            IntPtr compiled_model,
            ref sbyte name,
            ref IntPtr input_port);

        /// <summary>
        /// Get the output size of ov_compiled_model_t.
        /// </summary>
        /// <param name="compiled_model">A pointer to the ov_compiled_model_t.</param>
        /// <param name="size">the compiled_model's output size.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_outputs_size", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_outputs_size(
            IntPtr compiled_model,
            ref ulong size);

        /// <summary>
        /// Get the single const output port of ov_compiled_model_t, which only support single output model.
        /// </summary>
        /// <param name="compiled_model">A pointer to the ov_compiled_model_t.</param>
        /// <param name="output_port">A pointer to the ov_output_const_port_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_output", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_output(
            IntPtr compiled_model, ref IntPtr output_port);


        /// <summary>
        /// Get a const output port of ov_compiled_model_t by port index.
        /// </summary>
        /// <param name="compiled_model">A pointer to the ov_compiled_model_t.</param>
        /// <param name="index">input index.</param>
        /// <param name="output_port">A pointer to the ov_output_const_port_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_output_by_index",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_output_by_index(
            IntPtr compiled_model,
            ulong index,
            ref IntPtr output_port);


        /// <summary>
        /// Get a const output port of ov_compiled_model_t by name.
        /// </summary>
        /// <param name="compiled_model">A pointer to the ov_compiled_model_t.</param>
        /// <param name="name">tensor name (char *).</param>
        /// <param name="output_port">A pointer to the ov_output_const_port_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_output_by_name",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_output_by_name(
            IntPtr compiled_model,
            ref sbyte name,
            ref IntPtr output_port);

        /// <summary>
        /// Gets runtime model information from a device.
        /// </summary>
        /// <param name="compiled_model">A pointer to the ov_compiled_model_t.</param>
        /// <param name="model">A pointer to the ov_model_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_get_runtime_model", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_get_runtime_model(
            IntPtr compiled_model, 
            ref IntPtr model);

        /// <summary>
        /// Creates an inference request object used to infer the compiled model. 
        /// </summary>
        /// <param name="compiled_model">A pointer to the ov_compiled_model_t.</param>
        /// <param name="infer_request">A pointer to the ov_infer_request_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_create_infer_request",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_create_infer_request(
            IntPtr compiled_model, 
            ref IntPtr infer_request);

        /// <summary>
        /// Sets properties for a device, acceptable keys can be found in ov_property_key_xxx.
        /// </summary>
        /// <param name="compiled_model">A pointer to the ov_compiled_model_t.</param>
        /// <param name="property_key">The property key string.</param>
        /// <param name="property_value">The property value string.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_set_property", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_set_property(
            IntPtr compiled_model,
            IntPtr property_key,
            IntPtr property_value);

        /// <summary>
        /// Gets properties for current compiled model.
        /// </summary>
        /// <param name="compiled_model">A pointer to the ov_compiled_model_t.</param>
        /// <param name="property_key">Property key.</param>
        /// <param name="property_value">A pointer to property value.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_get_property", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_get_property(
            IntPtr compiled_model,
            ref sbyte property_key,
            ref IntPtr property_value);

        /// <summary>
        /// Exports the current compiled model to an output stream `std::ostream`.
        /// The exported model can also be imported via the ov::Core::import_model method.
        /// </summary>
        /// <param name="compiled_model">A pointer to the ov_compiled_model_t.</param>
        /// <param name="export_model_path">Path to the file.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_export_model", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_export_model(
            IntPtr compiled_model, 
            ref sbyte export_model_path);

        /// <summary>
        /// Release the memory allocated by ov_compiled_model_t.
        /// </summary>
        /// <param name="compiled_model">A pointer to the ov_compiled_model_t.</param>
        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_free", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_compiled_model_free(IntPtr compiled_model);

        /// <summary>
        /// Returns pointer to device-specific shared context on a remote accelerator 
        /// device that was used to create this CompiledModel.
        /// </summary>
        /// <param name="compiled_model">A pointer to the ov_compiled_model_t.</param>
        /// <param name="context">Return context.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_compiled_model_get_context", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_compiled_model_get_context(
            IntPtr compiled_model,
            ref IntPtr context);

    }
}
