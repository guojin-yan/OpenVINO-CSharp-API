using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    public partial class Core
    {
        private IntPtr IEPtr = new IntPtr();

        public Core() { }
        public Core(string model_file, string device_name)
        {

            IEPtr = NativeMethods.core_init(model_file, device_name);
        }

        public void set_input_sharp(string input_node_name, ulong[] input_size)
        {
            
            int length = input_size.Length;
            if (length == 4)
            {
                IEPtr = NativeMethods.set_input_image_sharp(IEPtr, input_node_name, ref input_size[0]);
            }
            else 
            {
                IEPtr = NativeMethods.set_input_data_sharp(IEPtr, input_node_name, ref input_size[0]);
            }
                
        }

        public void load_input_data(string input_node_name, float[] input_data)
        {
            IEPtr = NativeMethods.load_input_data(IEPtr, input_node_name, ref input_data[0]);
        }
        public void load_input_data(string input_node_name, byte[] image_data, ulong image_size)
        {
            IEPtr = NativeMethods.load_image_input_data(IEPtr, input_node_name, ref image_data[0], image_size);
        }

        public void inference_engine_infer()
        {
            IEPtr = NativeMethods.core_infer(IEPtr);
        }
        public T[] read_inference_result<T>(string output_node_name, int data_size)
        {
            Type t = typeof(T);
            T[] result = new T[data_size];
            if (t is System.Int32)
            {
                int[] inference_result = new int[data_size];
                NativeMethods.read_infer_result_I32(IEPtr, output_node_name, data_size, ref inference_result[0]);
                result = (T[])Convert.ChangeType(inference_result, typeof(T[]));
                return result;
            }
            else
            {
                float[] inference_result = new float[data_size];
                NativeMethods.read_infer_result_F32(IEPtr, output_node_name, data_size, ref inference_result[0]);
                result = (T[])Convert.ChangeType(inference_result, typeof(T[]));
                return result;
            }
        }

        public void inference_engine_delet()
        {
            NativeMethods.core_delet(IEPtr);
        }
    }
}
