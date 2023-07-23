using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    /// @struct ov_version_t
    /// @ingroup ov_version_t_c#_api
    /// @brief reconstruct the ov_version_t struct in the c api // 重构C API中的ov_version_t结构体
    public struct ov_version_t 
    {
        public string buildNumber;  //!< A string representing OpenVINO version
        public string description;  //!< A string representing OpenVINO description
    }
    /// @class ov_version
    /// @ingroup ov_version_c#_api
    /// @brief Represents OpenVINO version information
    public class ov_version 
    {
        public ov_version_t version;
        private IntPtr ptr;
        public IntPtr Ptr { get { return ptr; } set { ptr = value; } }
        public ov_version() 
        {
            int l = Marshal.SizeOf(typeof(ov_version_t));
            ptr = Marshal.AllocHGlobal(l);
            ov_status_e status = (ov_status_e)NativeMethods.ov_get_openvino_version(ptr);
            var temp = Marshal.PtrToStructure(ptr, typeof(ov_version_t));
            this.version = (ov_version_t)temp;
        }
        public void free() 
        {
            ov_status_e status = (ov_status_e)NativeMethods.ov_version_free(ptr);
        }
    };
    public class Core
    {
        private IntPtr ptr;
        public IntPtr Ptr { get { return ptr; } set { ptr = value; } }
        public Core() 
        { 
            ov_status_e status = (ov_status_e)NativeMethods.ov_core_create(ref ptr);
            System.Diagnostics.Debug.WriteLine(status);
        }
        public Core(string xml_config_file)
        {
            ov_status_e status = (ov_status_e)NativeMethods.ov_core_create_with_config(xml_config_file, ref ptr);
            System.Diagnostics.Debug.WriteLine("Core: " + status);
        }

        public void free() 
        {
            int status = NativeMethods.ov_core_free(ptr);
        }

        public Model read_model(string model_path, string bin_path = "") 
        {
            IntPtr model_ptr = new IntPtr();
            string extension = System.IO.Path.GetExtension(model_path);//扩展名
            ov_status_e status = (ov_status_e)NativeMethods.ov_core_read_model_unicode(ptr, model_path, bin_path, ref model_ptr);
            System.Diagnostics.Debug.WriteLine("read_model: " + status);


            return new Model(model_ptr);
        }

        public CompiledModel compiled_model(Model model, string device_name = "AUTO", ulong property_args_size = 0) 
        {
            
            IntPtr compiled_model_ptr = new IntPtr();
            sbyte[] c_device = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(device_name));
            ov_status_e status = (ov_status_e)NativeMethods.ov_core_compile_model(ptr, model.ptr, ref c_device[0], property_args_size, ref compiled_model_ptr);
            System.Diagnostics.Debug.WriteLine("compiled_model: " + status);
            return new CompiledModel(compiled_model_ptr);
        }

    }
}
