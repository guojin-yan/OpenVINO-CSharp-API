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
        /// Release the memory allocated by ov_model_t.
        /// </summary>
        /// <param name="model">A pointer to the ov_model_t to free memory.</param>
        [DllImport(dll_extern, EntryPoint = "ov_model_free",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_model_free(
            IntPtr model);

        /// <summary>
        /// Get a const single input port of ov_model_t, which only support single input model.
        /// </summary>
        /// <param name="model">A pointer to the ov_model_t.</param>
        /// <param name="input_port">A pointer to the ov_output_port_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_model_const_input", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_const_input(
            IntPtr model, 
            ref IntPtr input_port);

        /// <summary>
        /// Get a const input port of ov_model_t by name.
        /// </summary>
        /// <param name="model">A pointer to the ov_model_t.</param>
        /// <param name="tensor_name">input tensor name (char *).</param>
        /// <param name="input_port">A pointer to the ov_output_port_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_model_const_input_by_name", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_const_input_by_name(
            IntPtr model, 
            ref sbyte tensor_name, 
            ref IntPtr input_port);

        /// <summary>
        /// Get a const input port of ov_model_t by port index.
        /// </summary>
        /// <param name="model">A pointer to the ov_model_t.</param>
        /// <param name="index">input tensor index.</param>
        /// <param name="input_port">A pointer to the ov_output_port_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_model_const_input_by_index", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_const_input_by_index(
            IntPtr model, 
            ulong index, 
            ref IntPtr input_port);

        /// <summary>
        /// Get single input port of ov_model_t, which only support single input model.
        /// </summary>
        /// <param name="model">A pointer to the ov_model_t.</param>
        /// <param name="input_port">A pointer to the ov_output_port_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_model_input",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_input(
            IntPtr model,
            ref IntPtr input_port);

        /// <summary>
        /// Get an input port of ov_model_t by name.
        /// </summary>
        /// <param name="model">A pointer to the ov_model_t.</param>
        /// <param name="tensor_name">input tensor name (char *).</param>
        /// <param name="input_port">A pointer to the ov_output_port_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_model_input_by_name",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_input_by_name(
            IntPtr model,
            ref sbyte tensor_name,
            ref IntPtr input_port);

        /// <summary>
        /// Get an input port of ov_model_t by port index.
        /// </summary>
        /// <param name="model">A pointer to the ov_model_t.</param>
        /// <param name="index">input tensor index.</param>
        /// <param name="input_port">A pointer to the ov_output_port_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_model_input_by_index",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_input_by_index(
            IntPtr model,
            ulong index,
            ref IntPtr input_port);


        /// <summary>
        /// Get a single const output port of ov_model_t, which only support single output model..
        /// </summary>
        /// <param name="model">A pointer to the ov_model_t.</param>
        /// <param name="output_port">A pointer to the ov_output_const_port_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_model_const_output", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_const_output(
            IntPtr model, 
            ref IntPtr output_port);

        /// <summary>
        /// Get a const output port of ov_model_t by port index.
        /// </summary>
        /// <param name="model">A pointer to the ov_model_t.</param>
        /// <param name="index">input tensor index.</param>
        /// <param name="output_port">A pointer to the ov_output_const_port_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_model_const_output_by_index", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_const_output_by_index(
            IntPtr model, 
            ulong index, 
            ref IntPtr output_port);

        /// <summary>
        /// Get a const output port of ov_model_t by name.
        /// </summary>
        /// <param name="model">A pointer to the ov_model_t.</param>
        /// <param name="tensor_name">input tensor name (char *).</param>
        /// <param name="output_port">A pointer to the ov_output_const_port_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_model_const_output_by_name",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_const_output_by_name(
            IntPtr model, 
            ref sbyte tensor_name, 
            ref IntPtr output_port);


        /// <summary>
        /// Get an single output port of ov_model_t, which only support single output model.
        /// </summary>
        /// <param name="model">A pointer to the ov_model_t.</param>
        /// <param name="output_port">A pointer to the ov_output_const_port_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_model_output",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_output(
            IntPtr model,
            ref IntPtr output_port);

        /// <summary>
        /// Get an output port of ov_model_t by port index.
        /// </summary>
        /// <param name="model">A pointer to the ov_model_t.</param>
        /// <param name="index">input tensor index.</param>
        /// <param name="output_port">A pointer to the ov_output_port_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_model_output_by_index",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_output_by_index(
            IntPtr model,
            ulong index,
            ref IntPtr output_port);

        /// <summary>
        /// Get an output port of ov_model_t by name.
        /// </summary>
        /// <param name="model">A pointer to the ov_model_t.</param>
        /// <param name="tensor_name">output tensor name (char *).</param>
        /// <param name="output_port">A pointer to the ov_output_port_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_model_output_by_name",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_output_by_name(
            IntPtr model, 
            ref sbyte tensor_name, 
            ref IntPtr output_port);

        /// <summary>
        /// Get the input size of ov_model_t.
        /// </summary>
        /// <param name="model">A pointer to the ov_model_t.</param>
        /// <param name="input_size">the model's input size.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_model_inputs_size",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_inputs_size(IntPtr model, ref ulong input_size);

        /// <summary>
        /// Get the output size of ov_model_t.
        /// </summary>
        /// <param name="model">A pointer to the ov_model_t.</param>
        /// <param name="output_size">the model's output size.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_model_outputs_size",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_outputs_size(
            IntPtr model, 
            ref ulong output_size);

        /// <summary>
        /// Returns true if any of the ops defined in the model is dynamic shape..
        /// </summary>
        /// <param name="model">A pointer to the ov_model_t.</param>
        /// <returns>true if model contains dynamic shapes</returns>
        [DllImport(dll_extern, EntryPoint = "ov_model_is_dynamic",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static bool ov_model_is_dynamic(
            IntPtr model);

        /// <summary>
        /// Do reshape in model with a list of (name, partial shape).
        /// </summary>
        /// <param name="model">A pointer to the ov_model_t.</param>
        /// <param name="tensor_names">The list of input tensor names.</param>
        /// <param name="partial_shapes">A PartialShape list.</param>
        /// <param name="size">The item count in the list.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_model_reshape",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_reshape(
            IntPtr model,
            IntPtr[] tensor_names,
            IntPtr partial_shapes,
            ulong size);


        /// <summary>
        /// Do reshape in model with partial shape for a specified name.
        /// </summary>
        /// <param name="model">A pointer to the ov_model_t.</param>
        /// <param name="tensor_name">The tensor name of input tensor.</param>
        /// <param name="partial_shape">A PartialShape.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_model_reshape_input_by_name",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_reshape_input_by_name(
            IntPtr model,
            ref sbyte tensor_name,
            PartialShape.ov_partial_shape partial_shape);

        /// <summary>
        /// Do reshape in model for one node(port 0).
        /// </summary>
        /// <param name="model">A pointer to the ov_model_t.</param>
        /// <param name="partial_shape">A PartialShape.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_model_reshape_single_input",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_reshape_single_input(
            IntPtr model,
            PartialShape.ov_partial_shape partial_shape);

        /// <summary>
        /// Do reshape in model with a list of (port id, partial shape).
        /// </summary>
        /// <param name="model">A pointer to the ov_model_t.</param>
        /// <param name="port_indexes">The array of port indexes.</param>
        /// <param name="partial_shape">A PartialShape list.</param>
        /// <param name="size">The item count in the list.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_model_reshape_by_port_indexes",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_reshape_by_port_indexes(
            IntPtr model,
            ref ulong port_indexes,
            IntPtr partial_shape,
            ulong size);

        /// <summary>
        /// Do reshape in model with a list of (ov_output_port_t, partial shape).
        /// </summary>
        /// <param name="model">A pointer to the ov_model_t.</param>
        /// <param name="output_ports">The ov_output_port_t list.</param>
        /// <param name="partial_shapes">A PartialShape list.</param>
        /// <param name="size">The item count in the list.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_model_reshape_by_ports",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_reshape_by_ports(
            IntPtr model,
            ref IntPtr output_ports,
            IntPtr partial_shapes,
            ulong size);

        /// <summary>
        /// Gets the friendly name for a model.
        /// </summary>
        /// <param name="model">A pointer to the ov_model_t.</param>
        /// <param name="friendly_name">the model's friendly name.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_model_get_friendly_name", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_model_get_friendly_name(
            IntPtr model,
            ref IntPtr friendly_name);

    }
}
