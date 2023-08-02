using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;


namespace OpenVinoSharp
{

    /// <summary>
    /// <para>This class represents an OpenVINO runtime Core entity.</para>
    /// <ingroupe>ov_runtime_c#_api</ingroupe>
    /// </summary>
    /// <remark>User applications can create several Core class instances, but in this case the underlying plugins
    /// are created multiple times and not shared between several Core instances.The recommended way is to have
    /// a single Core instance per application.
    /// </remark>
    public class Core
    {
        private IntPtr ptr = IntPtr.Zero;
        public IntPtr Ptr { get { return ptr; } set { ptr = value; } }

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
            ExceptionStatus status;
            if (!String.IsNullOrEmpty(xml_config_file)) {
                status = (ExceptionStatus)NativeMethods.ov_core_create_with_config(xml_config_file, ref ptr);
            }
            status = (ExceptionStatus)NativeMethods.ov_core_create(ref ptr);
            if (status != 0) {
                ptr = IntPtr.Zero;

                System.Diagnostics.Debug.WriteLine("Core init error : " + status.ToString());
            }
            
        }
        /// <summary>
        /// Core's destructor
        /// </summary>
        ~Core() { dispose(); }
        /// <summary>
        /// Release unmanaged resources
        /// </summary>
        public void dispose()
        {
            if (ptr == IntPtr.Zero)
            {
                return;
            }
            NativeMethods.ov_core_free(ptr);
    
            ptr = IntPtr.Zero;
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
            ExceptionStatus status;
            int l = Marshal.SizeOf(typeof(CoreVersionList));
            IntPtr ptr_core_version_s = Marshal.AllocHGlobal(l);
            sbyte[] c_device_name = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(device_name));
            status = (ExceptionStatus)NativeMethods.ov_core_get_versions_by_device_name(ptr, ref c_device_name[0], ptr_core_version_s);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Core get_versions() error : " + status.ToString());
                return new KeyValuePair<string, Version>();
            }
            var temp1 = Marshal.PtrToStructure(ptr_core_version_s, typeof(CoreVersionList));

            CoreVersionList core_version_s = (CoreVersionList)temp1;
            var temp2 = Marshal.PtrToStructure(core_version_s.core_version, typeof(CoreVersion));
            CoreVersion core_version = (CoreVersion)temp2;
            KeyValuePair<string, Version> value = new KeyValuePair<string, Version>(core_version.device_name, core_version.version);
            NativeMethods.ov_core_versions_free(ptr_core_version_s);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Core get_versions() error : " + status.ToString());
                return new KeyValuePair<string, Version>();
            }
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
            IntPtr model_ptr = new IntPtr();
            string extension = System.IO.Path.GetExtension(model_path);//扩展名
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_core_read_model_unicode(ptr, model_path, bin_path, ref model_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Core read_model() error : " + status.ToString());
                return new Model(IntPtr.Zero);
            }

            return new Model(model_ptr);
        }

        /// <summary>
        ///     Creates a compiled model from a source model object.
        /// </summary>
        /// <param name="model">Model object acquired from Core::read_model.</param>
        /// <param name="device_name">Name of a device to load a model to.</param>
        /// <param name="property_args_size">Optional map of pairs: (property name, property value) relevant only for t
        /// his load operation.</param>
        /// <returns>A compiled model.</returns>
        /// <remarks>
        /// Users can create as many compiled models as they need and use
        /// them simultaneously (up to the limitation of the hardware resources).
        /// </remarks>
        public CompiledModel compiled_model(Model model, string device_name, ulong property_args_size = 0) 
        {
            
            IntPtr compiled_model_ptr = new IntPtr();
            sbyte[] c_device = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(device_name));
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_core_compile_model(ptr, model.m_ptr, ref c_device[0], property_args_size, ref compiled_model_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Core compiled_model() error : " + status.ToString());
                return new CompiledModel(IntPtr.Zero);
            }
            return new CompiledModel(compiled_model_ptr);
        }

    }
}
