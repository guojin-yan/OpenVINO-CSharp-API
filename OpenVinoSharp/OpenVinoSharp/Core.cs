using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    public partial class Core
    {
        private IntPtr ptr = new IntPtr();

        public Core() { }
        public Core(string model_file, string device_name){

            ptr = NativeMethods.core_init(model_file, device_name);
        }

        public void set_input_sharp(string input_node_name, ulong[] input_size) {
            
            int length = input_size.Length;
            if (length == 4) {
                ptr = NativeMethods.set_input_image_sharp(ptr, input_node_name, ref input_size[0]);
            }
            else {
                ptr = NativeMethods.set_input_data_sharp(ptr, input_node_name, ref input_size[0]);
            }      
        }

        public void load_input_data(string input_node_name, float[] input_data) {
            ptr = NativeMethods.load_input_data(ptr, input_node_name, ref input_data[0]);
        }
        public void load_input_data(string input_node_name, byte[] image_data, ulong image_size) {
            ptr = NativeMethods.load_image_input_data(ptr, input_node_name, ref image_data[0], image_size);
        }

        public void infer() {
            ptr = NativeMethods.core_infer(ptr);
        }
        public T[] read_infer_result<T>(string output_node_name, int data_size) {
            Type t = typeof(T);
            T[] result = new T[data_size];
            if (t is System.Int32) {
                int[] inference_result = new int[data_size];
                NativeMethods.read_infer_result_I32(ptr, output_node_name, data_size, ref inference_result[0]);
                result = (T[])Convert.ChangeType(inference_result, typeof(T[]));
                return result;
            }
            else {
                float[] inference_result = new float[data_size];
                NativeMethods.read_infer_result_F32(ptr, output_node_name, data_size, ref inference_result[0]);
                result = (T[])Convert.ChangeType(inference_result, typeof(T[]));
                return result;
            }
        }

        public void delet() {
            NativeMethods.core_delet(ptr);
        }
    }
}
