// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.native
{
    public static partial class NativeMethods
    {
        #region Version

        /// <summary>
        /// Get version of OpenVINO.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_get_openvino_version",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_get_openvino_version(IntPtr version);

        /// <summary>
        /// Release the memory allocated by ov_version_t.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_version_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_version_free(IntPtr version);

        #endregion

        #region Log Callback

        /// <summary>
        /// Callback function type for logging messages (original C API name).
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ov_util_log_callback_func(IntPtr message);

        /// <summary>
        /// Callback function type for logging messages (C# friendly alias).
        /// </summary>
        public delegate void LogCallbackDelegate(string message);

        /// <summary>
        /// Sets user log message handling callback.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_util_set_log_callback",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ov_util_set_log_callback(ov_util_log_callback_func func);

        /// <summary>
        /// Sets user log message handling callback (C# friendly overload).
        /// </summary>
        public static void ov_util_set_log_callback(LogCallbackDelegate func)
        {
            // Create a wrapper that marshals string from IntPtr
            ov_util_log_callback_func wrapper = (IntPtr msgPtr) => {
                string message = Marshal.PtrToStringAnsi(msgPtr) ?? string.Empty;
                func(message);
            };
            ov_util_set_log_callback(wrapper);
        }

        /// <summary>
        /// Resets log message handling callback to its default (standard output).
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_util_reset_log_callback",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ov_util_reset_log_callback();

        #endregion

        #region Core Creation and Destruction

        /// <summary>
        /// Constructs OpenVINO Core instance by default.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_create",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_create(ref IntPtr core);

        /// <summary>
        /// Constructs OpenVINO Core instance using XML configuration file.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_create_with_config",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_create_with_config(
            [MarshalAs(UnmanagedType.LPStr)] string xml_config_file,
            ref IntPtr core);

        /// <summary>
        /// Release the memory allocated by ov_core_t.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_core_free(IntPtr core);

        /// <summary>
        /// Shut down the OpenVINO.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_shutdown",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_shutdown();

        #endregion

        #region Read Model

        /// <summary>
        /// Reads models from IR / ONNX / PDPD / TF / TFLite formats.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_read_model",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_read_model(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string model_path,
            [MarshalAs(UnmanagedType.LPStr)] string bin_path,
            ref IntPtr model);

        /// <summary>
        /// Reads models from memory buffer.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_read_model_from_memory_buffer",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_read_model_from_memory_buffer(
            IntPtr core,
            ref byte model_str,
            ulong str_len,
            IntPtr weights,
            ref IntPtr model);

        #endregion

        #region Compile Model

        /// <summary>
        /// Creates a compiled model from a source model object.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model(
            IntPtr core,
            IntPtr model,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            ulong property_args_size,
            ref IntPtr compiled_model);

        /// <summary>
        /// Creates a compiled model from a source model object with properties.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model(
            IntPtr core,
            IntPtr model,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            ulong property_args_size,
            ref IntPtr compiled_model,
            IntPtr key1,
            IntPtr value1);

        /// <summary>
        /// Creates a compiled model from a source model object with 2 property pairs.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model(
            IntPtr core,
            IntPtr model,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            ulong property_args_size,
            ref IntPtr compiled_model,
            IntPtr key1,
            IntPtr value1,
            IntPtr key2,
            IntPtr value2);

        /// <summary>
        /// Creates a compiled model from a source model object with 3 property pairs.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model(
            IntPtr core,
            IntPtr model,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            ulong property_args_size,
            ref IntPtr compiled_model,
            IntPtr key1,
            IntPtr value1,
            IntPtr key2,
            IntPtr value2,
            IntPtr key3,
            IntPtr value3);

        /// <summary>
        /// Reads a model and creates a compiled model from the IR/ONNX/PDPD file.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model_from_file",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model_from_file(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string model_path,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            ulong property_args_size,
            ref IntPtr compiled_model);

        /// <summary>
        /// Reads a model and creates a compiled model from file with 1 property pair.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model_from_file",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model_from_file(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string model_path,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            ulong property_args_size,
            ref IntPtr compiled_model,
            IntPtr key1,
            IntPtr value1);

        /// <summary>
        /// Reads a model and creates a compiled model from file with 2 property pairs.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model_from_file",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model_from_file(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string model_path,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            ulong property_args_size,
            ref IntPtr compiled_model,
            IntPtr key1,
            IntPtr value1,
            IntPtr key2,
            IntPtr value2);

        /// <summary>
        /// Reads a model and creates a compiled model from file with 3 property pairs.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_compile_model_from_file",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model_from_file(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string model_path,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            ulong property_args_size,
            ref IntPtr compiled_model,
            IntPtr key1,
            IntPtr value1,
            IntPtr key2,
            IntPtr value2,
            IntPtr key3,
            IntPtr value3);

        #endregion

        #region Properties

        /// <summary>
        /// Sets properties for a device.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_set_property",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern ExceptionStatus ov_core_set_property(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            IntPtr key,
            IntPtr value);

        /// <summary>
        /// Sets properties for a device with 2 property pairs.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_set_property",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern ExceptionStatus ov_core_set_property(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            IntPtr key1,
            IntPtr value1,
            IntPtr key2,
            IntPtr value2);

        /// <summary>
        /// Sets properties for a device with 3 property pairs.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_set_property",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern ExceptionStatus ov_core_set_property(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            IntPtr key1,
            IntPtr value1,
            IntPtr key2,
            IntPtr value2,
            IntPtr key3,
            IntPtr value3);

        /// <summary>
        /// Gets properties related to device behaviour.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_get_property",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_get_property(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            [MarshalAs(UnmanagedType.LPStr)] string property_key,
            ref IntPtr property_value);

        #endregion

        #region Available Devices

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

        #endregion

        #region Import/Export

        /// <summary>
        /// Imports a compiled model from the previously exported one.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_import_model",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_import_model(
            IntPtr core,
            ref byte content,
            ulong content_size,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            ref IntPtr compiled_model);

        #endregion

        #region Device Versions

        /// <summary>
        /// Returns device plugins version information.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_get_versions_by_device_name",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_get_versions_by_device_name(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            IntPtr versions);

        /// <summary>
        /// Releases memory occupied by ov_core_version_list_t.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_versions_free",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_core_versions_free(IntPtr versions);

        #endregion

        #region Remote Context

        /// <summary>
        /// Creates a new remote shared context object on the specified accelerator device.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_create_context",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_create_context(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
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
        /// Gets a pointer to default shared context object for the specified accelerator device.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_get_default_context",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_get_default_context(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string device_name,
            ref IntPtr context);

        #endregion

        #region Extensions

        /// <summary>
        /// Adds an extension to the core.
        /// </summary>
        [DllImport("openvino_c", EntryPoint = "ov_core_add_extension",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_add_extension(
            IntPtr core,
            [MarshalAs(UnmanagedType.LPStr)] string path);

        #endregion
    }

    #region Supporting Structures

    /// <summary>
    /// Structure representing available devices
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ov_available_devices_t
    {
        public IntPtr devices;
        public ulong size;
    }

    /// <summary>
    /// Structure representing OpenVINO version
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ov_version_t
    {
        public IntPtr buildNumber;
        public IntPtr description;
    }

    /// <summary>
    /// Structure representing core version
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ov_core_version_t
    {
        public IntPtr device_name;
        public ov_version_t version;
    }

    /// <summary>
    /// Structure representing core version list
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ov_core_version_list_t
    {
        public IntPtr versions;
        public ulong size;
    }

    #endregion
}
