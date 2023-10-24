using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OpenVinoSharp
{
    /// <summary>
    /// <para>This class represents an OpenVINO runtime Core entity.</para>
    /// <ingroupe>ov_runtime_c#_api</ingroupe>
    /// </summary>
    /// <remarks>User applications can create several Core class instances, but in this case the underlying plugins
    /// are created multiple times and not shared between several Core instances.The recommended way is to have
    /// a single Core instance per application.
    /// </remarks>
    public class Core : IDisposable
    {
        /// <summary>
        /// [private]Core class pointer.
        /// </summary>
        private IntPtr m_ptr = IntPtr.Zero;
        /// <summary>
        /// [public]Core class pointer.
        /// </summary>
        public IntPtr Ptr 
        { 
            get { return m_ptr; }
            set { m_ptr = value; } 
        }

        /// <summary>
        /// Represent all available devices.
        /// </summary>
        struct ov_available_devices_t
        {
            /// <summary>
            ///  devices' name
            /// </summary>
            public IntPtr devices;
            /// <summary>
            /// devices' number
            /// </summary>
            public ulong size;
        }

        /// <summary>
        ///  Constructs an OpenVINO Core instance with devices and their plugins description.
        ///     <para>There are two ways how to configure device plugins:</para>
        ///     <para>1. (default) Use XML configuration file in case of dynamic libraries build;</para>
        ///     <para>2. Use strictly defined configuration in case of static libraries build.</para>
        /// </summary>
        /// <param name="xml_config_file">
        ///  Path to the .xml file with plugins to load from. If the XML configuration file is not
        ///  specified, default OpenVINO Runtime plugins are loaded from:
        ///     <para>1. (dynamic build) default `plugins.xml` file located in the same folder as OpenVINO runtime shared library;</para>
        ///     <para>2. (static build) statically defined configuration.In this case path to the.xml file is ignored.</para>
        ///  </param>
        public Core(string xml_config_file = null)
        {
            if (!String.IsNullOrEmpty(xml_config_file))
            {
                HandleException.handler(
                    NativeMethods.ov_core_create_with_config(xml_config_file, ref m_ptr));
            }
            else
            {
                HandleException.handler(
                NativeMethods.ov_core_create(ref m_ptr));
            }


        }
        /// <summary>
        /// Core's destructor
        /// </summary>
        ~Core() { Dispose(); }
        /// <summary>
        /// Release unmanaged resources
        /// </summary>
        public void Dispose()
        {
            if (m_ptr == IntPtr.Zero)
            {
                return;
            }
            NativeMethods.ov_core_free(m_ptr);

            m_ptr = IntPtr.Zero;
        }
        /// <summary>
        /// Returns device plugins version information.
        /// </summary>
        /// <param name="device_name">Device name to identify a plugin.</param>
        /// <returns>A vector of versions.</returns>
        /// <remarks>
        /// Device name can be complex and identify multiple devices at once like `HETERO:CPU,GPU`;
        /// in this case, std::map contains multiple entries, each per device.
        /// </remarks>
        public KeyValuePair<string, Version> get_versions(string device_name)
        {
            if (string.IsNullOrEmpty(device_name))
            {
                throw new ArgumentNullException(nameof(device_name));
            }
            int l = Marshal.SizeOf(typeof(CoreVersionList));
            IntPtr ptr_core_version_s = Marshal.AllocHGlobal(l);
            sbyte[] c_device_name = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(device_name));
            HandleException.handler(
                NativeMethods.ov_core_get_versions_by_device_name(m_ptr, ref c_device_name[0], ptr_core_version_s));
            var temp1 = Marshal.PtrToStructure(ptr_core_version_s, typeof(CoreVersionList));
            CoreVersionList core_version_s = (CoreVersionList)temp1;
            var temp2 = Marshal.PtrToStructure(core_version_s.core_version, typeof(CoreVersion));
            CoreVersion core_version = (CoreVersion)temp2;
            KeyValuePair<string, Version> value = new KeyValuePair<string, Version>(core_version.device_name, core_version.version);
            NativeMethods.ov_core_versions_free(ptr_core_version_s);
            return value;
        }


        /// <summary>
        /// Reads models from IR / ONNX / PDPD / TF / TFLite file formats.
        /// </summary>
        /// <param name="model_path">Path to a model.</param>
        /// <param name="bin_path">Path to a data file.</param>
        /// <returns>A model.</returns>
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
        public Model read_model(string model_path, string bin_path = "")
        {
            if (string.IsNullOrEmpty(model_path))
            {
                throw new ArgumentNullException(nameof(model_path));
            }
            IntPtr model_ptr = new IntPtr();
            sbyte[] c_model_path = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(model_path));

            if (bin_path == "")
            {
                sbyte c_bin_path = new sbyte();
                HandleException.handler(
                    NativeMethods.ov_core_read_model(m_ptr, ref c_model_path[0], ref c_bin_path, ref model_ptr));
            }
            else
            {
                sbyte[] c_bin_path = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(bin_path));
                HandleException.handler(
                    NativeMethods.ov_core_read_model(m_ptr, ref c_model_path[0], ref c_bin_path[0], ref model_ptr));
            }

            return new Model(model_ptr);
        }

        /// <summary>
        /// Reads models from IR / ONNX / PDPD / TF / TFLite formats.
        /// </summary>
        /// <param name="model_path">String with a model in IR / ONNX / PDPD / TF / TFLite format.</param>
        /// <param name="weights">Shared pointer to a constant tensor with weights.</param>
        /// <remarks>
        /// Created model object shares the weights with the @p weights object.
        /// Thus, do not create @p weights on temporary data that can be freed later, since the model constant data will point to an invalid memory.
        /// </remarks>
        /// <returns>A model.</returns>
        public Model read_model(string model_path, Tensor weights)
        {
            if (string.IsNullOrEmpty(model_path))
            {
                throw new ArgumentNullException(nameof(model_path));
            }
            if (weights == null)
            {
                throw new ArgumentNullException(nameof(weights));
            }
            IntPtr model_ptr = new IntPtr();
            sbyte[] c_model_path = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(model_path));
            HandleException.handler(
                NativeMethods.ov_core_read_model_from_memory(m_ptr, ref c_model_path[0], weights.Ptr, ref model_ptr));
            return new Model(model_ptr);
        }

        /// <summary>
        /// Creates a compiled model from a source model object.
        /// </summary>
        /// <param name="model">Model object acquired from Core::read_model.</param>
        /// <returns>A compiled model.</returns>
        /// <remarks>
        /// Users can create as many compiled models as they need and use
        /// them simultaneously (up to the limitation of the hardware resources).
        /// </remarks>
        public CompiledModel compile_model(Model model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            IntPtr compiled_model_ptr = new IntPtr();
            string device_name = "AUTO";
            sbyte[] c_device = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(device_name));
            HandleException.handler(
                NativeMethods.ov_core_compile_model(m_ptr, model.m_ptr, ref c_device[0], 0, ref compiled_model_ptr));
            return new CompiledModel(compiled_model_ptr);
        }

        /// <summary>
        /// Creates and loads a compiled model from a source model to the default OpenVINO device selected by the AUTO
        /// </summary>
        /// <param name="model">Model object acquired from Core::read_model.</param>
        /// <param name="device_name">Name of a device to load a model to.</param>
        /// <returns>A compiled model.</returns>
        /// <remarks>
        /// Users can create as many compiled models as they need and use
        /// them simultaneously (up to the limitation of the hardware resources).
        /// </remarks>
        public CompiledModel compile_model(Model model, string device_name)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (string.IsNullOrEmpty(device_name))
            {
                throw new ArgumentNullException(nameof(device_name));
            }
            IntPtr compiled_model_ptr = new IntPtr();
            sbyte[] c_device = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(device_name));
            HandleException.handler(
                NativeMethods.ov_core_compile_model(m_ptr, model.m_ptr, ref c_device[0], 0, ref compiled_model_ptr));
            return new CompiledModel(compiled_model_ptr);
        }

        /// <summary>
        /// Reads and loads a compiled model from the IR/ONNX/PDPD file to the default OpenVINO device selected by the AUTO plugin.
        /// </summary>
        /// <param name="model_path">Path to a model.</param>
        /// <remarks>
        /// This can be more efficient than using the Core::read_model + Core::compile_model(model_in_memory_object) flow, 
        /// especially for cases when caching is enabled and a cached model is availab
        /// </remarks>
        /// <returns> A compiled model.</returns>
        public CompiledModel compile_model(string model_path)
        {
            if (string.IsNullOrEmpty(model_path))
            {
                throw new ArgumentNullException(nameof(model_path));
            }
            IntPtr compiled_model_ptr = new IntPtr();
            string device_name = "AUTO";
            sbyte[] c_model = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(model_path));
            sbyte[] c_device = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(device_name));
            HandleException.handler(
                NativeMethods.ov_core_compile_model_from_file(m_ptr, ref c_model[0], ref c_device[0], 0, ref compiled_model_ptr));
            return new CompiledModel(compiled_model_ptr);
        }
        /// <summary>
        /// Reads a model and creates a compiled model from the IR/ONNX/PDPD file.
        /// </summary>
        /// <param name="model_path">Path to a model.</param>
        /// <param name="device_name">Name of a device to load a model to.</param>
        /// <remarks>
        /// This can be more efficient than using the Core::read_model + Core::compile_model(model_in_memory_object) flow, 
        /// especially for cases when caching is enabled and a cached model is availab
        /// </remarks>
        /// <returns>A compiled model.</returns>
        public CompiledModel compile_model(string model_path, string device_name)
        {
            if (string.IsNullOrEmpty(model_path))
            {
                throw new ArgumentNullException(nameof(model_path));
            }
            if (string.IsNullOrEmpty(device_name))
            {
                throw new ArgumentNullException(nameof(device_name));
            }
            IntPtr compiled_model_ptr = new IntPtr();
            sbyte[] c_model = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(model_path));
            sbyte[] c_device = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(device_name));
            HandleException.handler(
                NativeMethods.ov_core_compile_model_from_file(m_ptr, ref c_model[0], ref c_device[0], 0, ref compiled_model_ptr));
            return new CompiledModel(compiled_model_ptr);
        }


        /// <summary>
        /// Returns devices available for inference.
        /// Core objects go over all registered plugins and ask about available devices.
        /// </summary>
        /// <returns>A vector of devices. The devices are returned as { CPU, GPU.0, GPU.1, GNA }.</returns>
        /// <remarks>
        /// If there is more than one device of a specific type, they are enumerated with the .# suffix.
        /// Such enumerated device can later be used as a device name in all Core methods like Core::compile_model,
        /// Core::query_model, Core::set_property and so on.
        /// </remarks>
        public List<string> get_available_devices()
        {
            int l = Marshal.SizeOf(typeof(ov_available_devices_t));
            IntPtr devices_ptr = Marshal.AllocHGlobal(l);
            HandleException.handler(
                NativeMethods.ov_core_get_available_devices(m_ptr, devices_ptr));

            var temp1 = Marshal.PtrToStructure(devices_ptr, typeof(ov_available_devices_t));

            ov_available_devices_t devices_s = (ov_available_devices_t)temp1;
            IntPtr[] devices_ptrs = new IntPtr[devices_s.size];
            Marshal.Copy(devices_s.devices, devices_ptrs, 0, (int)devices_s.size);
            List<string> devices = new List<string>();
            for (int i = 0; i < (int)devices_s.size; ++i)
            {
                devices.Add(Marshal.PtrToStringAnsi(devices_ptrs[i]));
            }
            NativeMethods.ov_available_devices_free(devices_ptr);
            return devices;
        }
    }
}

