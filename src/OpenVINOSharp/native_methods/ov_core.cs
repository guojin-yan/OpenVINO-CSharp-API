using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace OpenVinoSharp
{
    public partial class NativeMethods
    {

        /// <summary>
        /// Get version of OpenVINO.
        /// </summary>Status code of the operation: OK(0) for success.
        /// <param name="version">a pointer to the version</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_get_openvino_version", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_get_openvino_version(
            IntPtr version);

        /// <summary>
        /// Release the memory allocated by ov_version_t.
        /// </summary>
        /// <param name="version">A pointer to the ov_version_t to free memory.</param>
        [DllImport(dll_extern, EntryPoint = "ov_version_free", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_version_free(
            IntPtr version);

        /// <summary>
        /// Constructs OpenVINO Core instance by default.
        /// See RegisterPlugins for more details.
        /// </summary>
        /// <param name="core"> A pointer to the newly created ov_core_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_core_create", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_create(
            ref IntPtr core);

        /// <summary>
        /// Constructs OpenVINO Core instance using XML configuration file with devices description.
        /// See RegisterPlugins for more details.
        /// </summary>
        /// <param name="xml_config_file">A path to .xml file with devices to load from. 
        /// If XML configuration file is not specified, then default plugin.xml file will be used.</param>
        /// <param name="core">A pointer to the newly created ov_core_t.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_core_create_with_config", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_create_with_config(
            string xml_config_file, 
            ref IntPtr core);

        /// <summary>
        /// Release the memory allocated by ov_core_t.
        /// </summary>
        /// <param name="core">A pointer to the ov_core_t to free memory.</param>
        [DllImport(dll_extern, EntryPoint = "ov_core_free", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_core_free(
            IntPtr core);

        /// <summary>
        /// Reads models from IR / ONNX / PDPD / TF / TFLite formats.
        /// </summary>
        /// <param name="core">A pointer to the ie_core_t instance.</param>
        /// <param name="model_path">Path to a model.</param>
        /// <param name="bin_path">Path to a data file.</param>
        /// <param name="model">A pointer to the newly created model.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        /// <remarks>
        ///     <para>
        ///     For IR format (*.bin):
        ///     if `bin_path` is empty, will try to read a bin file with the same name as xml and
        ///     if the bin file with the same name is not found, will load IR without weights.
        ///     For the following file formats the `bin_path` parameter is not used:
        ///     </para>
        ///     <para>ONNX format (*.onnx)</para>
        ///     <para>PDPD(*.pdmodel)</para>
        ///     <para>TF(*.pb)</para>
        ///     <para>TFLite(*.tflite)</para>
        /// </remarks>
        [DllImport(dll_extern, EntryPoint = "ov_core_read_model_unicode", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_read_model_unicode(
            IntPtr core, 
            string model_path, 
            string bin_path, 
            ref IntPtr model);

        /// <summary>
        /// Reads models from IR / ONNX / PDPD / TF / TFLite formats.
        /// </summary>
        /// <param name="core">A pointer to the ie_core_t instance.</param>
        /// <param name="model_path">Path to a model.</param>
        /// <param name="bin_path">Path to a data file.</param>
        /// <param name="model">A pointer to the newly created model.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        /// <remarks>
        ///     <para>
        ///     For IR format (*.bin):
        ///     if `bin_path` is empty, will try to read a bin file with the same name as xml and
        ///     if the bin file with the same name is not found, will load IR without weights.
        ///     For the following file formats the `bin_path` parameter is not used:
        ///     </para>
        ///     <para>ONNX format (*.onnx)</para>
        ///     <para>PDPD(*.pdmodel)</para>
        ///     <para>TF(*.pb)</para>
        ///     <para>TFLite(*.tflite)</para>
        /// </remarks>
        [DllImport(dll_extern, EntryPoint = "ov_core_read_model", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_read_model(
            IntPtr core, 
            ref sbyte model_path,
            ref sbyte bin_path, 
            ref IntPtr model);

        /// <summary>
        /// Reads models from IR / ONNX / PDPD / TF / TFLite formats.
        /// </summary>
        /// <param name="core">A pointer to the ie_core_t instance.</param>
        /// <param name="model_path">Path to a model.</param>
        /// <param name="weights">Shared pointer to a constant tensor with weights.</param>
        /// <param name="model">A pointer to the newly created model.</param>
        /// <remarks>
        /// Reading ONNX / PDPD / TF / TFLite models does not support loading weights 
        /// from the @p weights tensors.</remarks>
        /// <note>
        /// Created model object shares the weights with the @p weights object.
        /// Thus, do not create @p weights on temporary data that can be freed later, 
        /// since the model constant data will point to an invalid memory.
        /// </note>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_core_read_model_from_memory", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_read_model_from_memory(IntPtr core, 
            string model_path, 
            IntPtr weights, 
            ref IntPtr model);

        /// <summary>
        /// Creates a compiled model from a source model object. Users can create 
        /// as many compiled models as they need and use them simultaneously 
        /// (up to the limitation of the hardware resources).
        /// </summary>
        /// <param name="core">A pointer to the ie_core_t instance.</param>
        /// <param name="model">Model object acquired from Core::read_model.</param>
        /// <param name="device_name">Name of a device to load a model to.</param>
        /// <param name="property_args_size">How many properties args will be passed, 
        /// each property contains 2 args: key and value.</param>
        /// <param name="compiled_model">A pointer to the newly created compiled_model.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_core_compile_model", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model(
            IntPtr core,
            IntPtr model, 
            ref sbyte device_name, 
            ulong property_args_size, 
            ref IntPtr compiled_model);

        /// <summary>
        /// Reads a model and creates a compiled model from the IR/ONNX/PDPD file. 
        /// This can be more efficient than using the ov_core_read_model_from_XXX + ov_core_compile_model flow, 
        /// especially for cases when caching is enabled and a cached model is available.
        /// </summary>
        /// <param name="core">A pointer to the ie_core_t instance.</param>
        /// <param name="model_path">Path to a model.</param>
        /// <param name="device_name">Name of a device to load a model to.</param>
        /// <param name="property_args_size">How many properties args will be passed, 
        /// each property contains 2 args: key and value.</param>
        /// <param name="compiled_model">A pointer to the newly created compiled_model.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_core_compile_model_from_file",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model_from_file(
            IntPtr core,
            ref sbyte model_path,
            ref sbyte device_name,
            ulong property_args_size,
            ref IntPtr compiled_model);

        /// <summary>
        /// Sets properties for a device, acceptable keys can be found in ov_property_key_xxx.
        /// </summary>
        /// <param name="core">A pointer to the ie_core_t instance.</param>
        /// <param name="device_name">Name of a device.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_core_set_property",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_set_property(
            IntPtr core, 
            ref sbyte device_name);

        /// <summary>
        /// Gets properties related to device behaviour.
        /// The method extracts information that can be set via the set_property method.
        /// </summary>
        /// <param name="core">A pointer to the ie_core_t instance.</param>
        /// <param name="device_name">Name of a device to get a property value.</param>
        /// <param name="property_key">Property key.</param>
        /// <param name="property_value">A pointer to property value with string format.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_core_get_property",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_get_property(
            IntPtr core,  
            ref sbyte device_name,
            ref sbyte property_key,
            ref IntPtr property_value);

        /// <summary>
        /// Returns devices available for inference.
        /// </summary>
        /// <param name="core">A pointer to the ie_core_t instance.</param>
        /// <param name="devices">A pointer to the ov_available_devices_t instance.
        /// Core objects go over all registered plugins and ask about available devices.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_core_get_available_devices",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_get_available_devices(
            IntPtr core,
            IntPtr devices);

        /// <summary>
        /// Releases memory occpuied by ov_available_devices_t
        /// </summary>
        /// <param name="devices">A pointer to the ov_available_devices_t instance.</param>
        [DllImport(dll_extern, EntryPoint = "ov_available_devices_free",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_available_devices_free(IntPtr devices);

        /// <summary>
        /// Imports a compiled model from the previously exported one.
        /// </summary>
        /// <param name="core">A pointer to the ov_core_t instance.</param>
        /// <param name="content">A pointer to content of the exported model.</param>
        /// <param name="content_size">Number of bytes in the exported network.</param>
        /// <param name="device_name">Name of a device to import a compiled model for.</param>
        /// <param name="compiled_model">A pointer to the newly created compiled_model.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_core_import_model",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_import_model(IntPtr core,
                     ref sbyte content,
                     ulong content_size,
                     ref sbyte device_name,
                     ref IntPtr compiled_model);


        /// <summary>
        /// Returns device plugins version information.
        /// Device name can be complex and identify multiple devices at once like `HETERO:CPU,GPU`;
        /// in this case, std::map contains multiple entries, each per device.
        /// </summary>
        /// <param name="core">A pointer to the ov_core_t instance.</param>
        /// <param name="device_name">Device name to identify a plugin.</param>
        /// <param name="versions">A pointer to versions corresponding to device_name.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_core_get_versions_by_device_name", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_get_versions_by_device_name(
            IntPtr core, 
            ref sbyte device_name,
            IntPtr versions);


        /// <summary>
        /// Releases memory occupied by ov_core_version_list_t.
        /// </summary>
        /// <param name="versions">A pointer to the ie_core_versions to free memory.</param>
        [DllImport(dll_extern, EntryPoint = "ov_core_versions_free", 
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_core_versions_free(
            IntPtr versions);

        /// <summary>
        /// Creates a new remote shared context object on the specified accelerator device 
        /// using specified plugin-specific low-level device API parameters (device handle, pointer, context, etc.).
        /// </summary>
        /// <param name="core">A pointer to the ov_core_t instance.</param>
        /// <param name="device_name">Device name to identify a plugin.</param>
        /// <param name="context_args_size">How many property args will be for this remote context creation.</param>
        /// <param name="context">A pointer to the newly created remote context.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_core_create_context",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_create_context(
            IntPtr core,
            ref sbyte device_name,
            ulong context_args_size,
            ref IntPtr context);


        /// <summary>
        /// Creates a compiled model from a source model within a specified remote context.
        /// </summary>
        /// <param name="core">A pointer to the ov_core_t instance.</param>
        /// <param name="model">Model object acquired from ov_core_read_model.</param>
        /// <param name="context">A pointer to the newly created remote context.</param>
        /// <param name="property_args_size">How many args will be for this compiled model.</param>
        /// <param name="compiled_model">A pointer to the newly created compiled_model.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_core_compile_model_with_context",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model_with_context(
            IntPtr core,
            IntPtr model,
            IntPtr context,
            ulong property_args_size,
            ref IntPtr compiled_model);

        /// <summary>
        /// Gets a pointer to default (plugin-supplied) shared context object for the specified accelerator device.
        /// </summary>
        /// <param name="core">A pointer to the ov_core_t instance.</param>
        /// <param name="device_name">Name of a device to get a default shared context from.</param>
        /// <param name="context">A pointer to the referenced remote context.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_core_compile_model_with_context",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_core_compile_model_with_context(
            IntPtr core, 
            ref sbyte device_name, 
            ref IntPtr context);
    }
}
