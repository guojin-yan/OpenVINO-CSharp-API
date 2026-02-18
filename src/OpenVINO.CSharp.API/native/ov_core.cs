// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Get version of OpenVINO.
        /// </summary>
        /// <param name="version">a pointer to the version</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_get_openvino_version",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_get_openvino_version(IntPtr version);

        /// <summary>
        /// Release the memory allocated by ov_version_t.
        /// </summary>
        /// <param name="version">A pointer to the ov_version_t to free memory.</param>
        [DllImport("openvino_c", EntryPoint = "ov_version_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_version_free(IntPtr version);

        /// <summary>
        /// Callback function type for logging messages.
        /// </summary>
        /// <param name="message">The log message as a null-terminated C string.</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void LogCallbackDelegate(string message);

        /// <summary>
        /// Sets user log message handling callback.
        /// </summary>
        /// <param name="func"> The function pointer to user-defined message logging callback. Null pointer is accepted(no logging).</param>
        [DllImport("openvino_c", EntryPoint = "ov_util_set_log_callback",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ov_util_set_log_callback(LogCallbackDelegate func);

        /// <summary>
        /// Resets log message handling callback to its default (standard output).
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_util_reset_log_callback",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ov_util_reset_log_callback();

        /// <summary>
        /// Constructs OpenVINO Core instance by default.
        /// </summary>
        /// <param name="core"> A pointer to the newly created ov_core_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_create",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_create(ref IntPtr core);

        /// <summary>
        /// Constructs OpenVINO Core instance using XML configuration file with devices description.
        /// </summary>
        /// <param name="xml_config_file">A path to .xml file with devices to load from.</param>
        /// <param name="core">A pointer to the newly created ov_core_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_create_with_config",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_create_with_config(string xml_config_file, ref IntPtr core);

        /// <summary>
        /// Release the memory allocated by ov_core_t.
        /// </summary>
        /// <param name="core">A pointer to the ov_core_t to free memory.</param>
        [DllImport("openvino_c", EntryPoint = "ov_core_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_core_free(IntPtr core);

        /// <summary>
        /// Reads models from IR / ONNX / PDPD / TF / TFLite formats.
        /// </summary>
        /// <param name="core">A pointer to the ov_core_t instance.</param>
        /// <param name="model_path">Path to a model.</param>
        /// <param name="bin_path">Path to a data file.</param>
        /// <param name="model">A pointer to the newly created model.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_read_model",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_read_model(
            IntPtr core,
            ref sbyte model_path,
            ref sbyte bin_path,
            ref IntPtr model);

        /// <summary>
        /// Reads models from memory buffer.
        /// </summary>
        /// <param name="core">A pointer to the ov_core_t instance.</param>
        /// <param name="xml_model_file_byte">Model data buffer.</param>
        /// <param name="str_size">The length of model string.</param>
        /// <param name="weights">Shared pointer to a constant tensor with weights.</param>
        /// <param name="model">A pointer to the newly created model.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_read_model_from_memory_buffer",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_read_model_from_memory_buffer(
            IntPtr core,
            ref byte xml_model_file_byte,
            ulong str_size,
            IntPtr weights,
            ref IntPtr model);

        /// <summary>
        /// Creates a compiled model from a source model object.
        /// </summary>
        /// <param name="core">A pointer to the ov_core_t instance.</param>
        /// <param name="model">Model object acquired from Core::read_model.</param>
        /// <param name="device_name">Name of a device to load a model to.</param>
        /// <param name="property_args_size">How many properties args will be passed.</param>
        /// <param name="compiled_model">A pointer to the newly created compiled_model.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model(
            IntPtr core,
            IntPtr model,
            ref sbyte device_name,
            ulong property_args_size,
            ref IntPtr compiled_model);

        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model(
            IntPtr core,
            IntPtr model,
            ref sbyte device_name,
            ulong property_args_size,
            ref IntPtr compiled_model,
            IntPtr varg1, IntPtr varg2);

        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model(
            IntPtr core,
            IntPtr model,
            ref sbyte device_name,
            ulong property_args_size,
            ref IntPtr compiled_model,
            IntPtr varg1, IntPtr varg2,
            IntPtr varg3, IntPtr varg4);

        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model(
            IntPtr core,
            IntPtr model,
            ref sbyte device_name,
            ulong property_args_size,
            ref IntPtr compiled_model,
            IntPtr varg1, IntPtr varg2,
            IntPtr varg3, IntPtr varg4,
            IntPtr varg5, IntPtr varg6);

        /// <summary>
        /// Reads a model and creates a compiled model from the IR/ONNX/PDPD file.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model_from_file",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model_from_file(
            IntPtr core,
            ref sbyte model_path,
            ref sbyte device_name,
            ulong property_args_size,
            ref IntPtr compiled_model);

        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model_from_file",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model_from_file(
            IntPtr core,
            ref sbyte model_path,
            ref sbyte device_name,
            ulong property_args_size,
            ref IntPtr compiled_model,
            IntPtr varg1, IntPtr varg2);

        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model_from_file",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model_from_file(
            IntPtr core,
            ref sbyte model_path,
            ref sbyte device_name,
            ulong property_args_size,
            ref IntPtr compiled_model,
            IntPtr varg1, IntPtr varg2,
            IntPtr varg3, IntPtr varg4);

        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model_from_file",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model_from_file(
            IntPtr core,
            ref sbyte model_path,
            ref sbyte device_name,
            ulong property_args_size,
            ref IntPtr compiled_model,
            IntPtr varg1, IntPtr varg2,
            IntPtr varg3, IntPtr varg4,
            IntPtr varg5, IntPtr varg6);

        /// <summary>
        /// Sets properties for a device.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_set_property",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern ExceptionStatus ov_core_set_property(IntPtr core,
            ref sbyte device_name, IntPtr varg1, IntPtr varg2);

        [DllImport("openvino_c", EntryPoint = "ov_core_set_property",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern ExceptionStatus ov_core_set_property(IntPtr core,
            ref sbyte device_name, IntPtr varg1, IntPtr varg2, IntPtr varg3, IntPtr varg4);

        [DllImport("openvino_c", EntryPoint = "ov_core_set_property",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern ExceptionStatus ov_core_set_property(IntPtr core,
            ref sbyte device_name, IntPtr varg1, IntPtr varg2, IntPtr varg3, IntPtr varg4, IntPtr varg5, IntPtr varg6);

        /// <summary>
        /// Gets properties related to device behaviour.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_get_property",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_get_property(
            IntPtr core,
            ref sbyte device_name,
            ref sbyte property_key,
            ref IntPtr property_value);

        /// <summary>
        /// Returns devices available for inference.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_get_available_devices",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_get_available_devices(
            IntPtr core,
            IntPtr devices);

        /// <summary>
        /// Releases memory occupied by ov_available_devices_t
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_available_devices_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_available_devices_free(IntPtr devices);

        /// <summary>
        /// Imports a compiled model from the previously exported one.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_import_model",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_import_model(
            IntPtr core,
            ref byte content,
            ulong content_size,
            ref sbyte device_name,
            ref IntPtr compiled_model);

        /// <summary>
        /// Returns device plugins version information.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_get_versions_by_device_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_get_versions_by_device_name(
            IntPtr core,
            ref sbyte device_name,
            IntPtr versions);

        /// <summary>
        /// Releases memory occupied by ov_core_version_list_t.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_versions_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_core_versions_free(IntPtr versions);

        /// <summary>
        /// Creates a new remote shared context object.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_create_context",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_create_context(
            IntPtr core,
            ref sbyte device_name,
            ulong context_args_size,
            ref IntPtr context);

        /// <summary>
        /// Creates a compiled model from a source model within a specified remote context.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model_with_context",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model_with_context(
            IntPtr core,
            IntPtr model,
            IntPtr context,
            ulong property_args_size,
            ref IntPtr compiled_model);

        /// <summary>
        /// Gets a pointer to default shared context object.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_get_default_context",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_get_default_context(IntPtr core, ref sbyte device_name, ref IntPtr context);
    }
}
