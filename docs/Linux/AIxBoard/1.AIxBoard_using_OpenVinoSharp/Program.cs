using System;
using System.Runtime.InteropServices;
namespace test_openvinosharp
{
    internal class Program{
        static void Main(string[] args)
        {

            Version v = Version.get_openvino_version();
            Console.WriteLine(v.to_string());

            Core core = new Core();
            KeyValuePair<string, Version> vs = core.get_versions("CPU");
            Console.WriteLine(vs.Key);
            Console.WriteLine(vs.Value.to_string());
            vs = core.get_versions("GPU.0");
            Console.WriteLine(vs.Key);
            Console.WriteLine(vs.Value.to_string());
            core.dispose();
        }
    }
    class NativeMethods{
        
        const string dll_extern = "libopenvino_c.so";

        [DllImport(dll_extern, EntryPoint = "ov_get_openvino_version", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_get_openvino_version(IntPtr version);

        [DllImport(dll_extern, EntryPoint = "ov_version_free", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_version_free(IntPtr version);
        [DllImport(dll_extern, EntryPoint = "ov_core_create", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_core_create(ref IntPtr core);
       
        [DllImport(dll_extern, EntryPoint = "ov_core_free", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_core_free(IntPtr core);
        
        [DllImport(dll_extern, EntryPoint = "ov_core_get_versions_by_device_name", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_core_get_versions_by_device_name(IntPtr core, ref sbyte device_name, IntPtr versions);
        [DllImport(dll_extern, EntryPoint = "ov_core_versions_free", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static int ov_core_versions_free(IntPtr versions);
    }

    public struct Version
    {
        public string buildNumber;
        public string description;
        public Version(string buildNumber, string description) {
            this.buildNumber = buildNumber;
            this.description = description;
        }
        public string to_string() 
        {
            string str = "";
            str += description;
            str += "\r\n    Version : ";
            str += buildNumber.Substring(0, buildNumber.IndexOf("-"));
            str += "\r\n    Build   : ";
            str += buildNumber;
            return str;
        }
        public static Version get_openvino_version()
        {
            int l = Marshal.SizeOf(typeof(Version));
            IntPtr ptr = Marshal.AllocHGlobal(l);
            int status = NativeMethods.ov_get_openvino_version(ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("ov get_openvino_version() error!");
                return new Version();
            }
            var temp = Marshal.PtrToStructure(ptr, typeof(Version));
            Version version = (Version)temp;
            string build = String.Copy(version.buildNumber);
            string description = String.Copy(version.description);
            Version new_version = new Version(build, description);
            NativeMethods.ov_version_free(ptr);
            return new_version;
        }
    }
    public struct CoreVersion 
    {
        public string device_name;
        public Version version;
    }
    public struct CoreVersionList
    {
        public IntPtr core_version;
        public ulong size;
    }
    public class Core
    {
        private IntPtr ptr = IntPtr.Zero;
        public IntPtr Ptr { get { return ptr; } set { ptr = value; } }
        public Core(string xml_config_file = null) 
        {
            int status;
            status = NativeMethods.ov_core_create(ref ptr);
            if (status != 0) {
                ptr = IntPtr.Zero;
            }
            
        }
        ~Core() { dispose(); }
        public void dispose()
        {
            if (ptr == IntPtr.Zero)
            {
                return;
            }
            NativeMethods.ov_core_free(ptr);
    
            ptr = IntPtr.Zero;
        }
        public KeyValuePair<string, Version> get_versions(string device_name)
        {
            int status;
            int l = Marshal.SizeOf(typeof(CoreVersionList));
            IntPtr ptr_core_version_s = Marshal.AllocHGlobal(l);
            sbyte[] c_device_name = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(device_name));
            status = NativeMethods.ov_core_get_versions_by_device_name(ptr, ref c_device_name[0], ptr_core_version_s);
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
            status = NativeMethods.ov_core_versions_free(ptr_core_version_s);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Core get_versions() error : " + status.ToString());
                return new KeyValuePair<string, Version>();
            }
            return value;
        }
    }
}