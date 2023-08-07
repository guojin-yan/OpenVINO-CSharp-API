namespace OpenVinoSharp
{
    public class Core
    {
        private IntPtr core_ptr = new IntPtr();

        // @brief core class default initialization method.
        public Core() { }
        // @brief The core class parameter input initialization method.
        // @param model_file The inference model path.
        // @param device_name The device name.
        public Core(string model, string device, string cache_dir = "")
        {
            core_ptr = NativeMethods.core_init(model, device, cache_dir);
        }
        // @brief Set the size of the input nodes for the inference model.
        // @param node_name The input node name.
        // @param input_size The input shape array.
        public void set_input_sharp(string node_name, ulong[] input_size)
        {
            int length = input_size.Length;
            core_ptr = NativeMethods.set_input_sharp(core_ptr, node_name, ref input_size[0], length);
        }

        // @brief Load infer data.
        // @param node_name The input node name.
        // @param input_data The input data array.
        public void load_input_data(string node_name, float[] input_data)
        {
            core_ptr = NativeMethods.load_input_data(core_ptr, node_name, ref input_data[0]);
        }
        // @brief Load input image data.
        // @param node_name The input node name.
        // @param image_data The image data.
        // @param image_size The length of image data.
        public void load_input_data(string node_name, byte[] image_data, ulong image_size, int type)
        {
            core_ptr = NativeMethods.load_image_input_data(core_ptr, node_name, ref image_data[0], image_size, type);
        }
        // @brief Model infer.
        public void infer()
        {
            core_ptr = NativeMethods.core_infer(core_ptr);
        }
        // @brief Read model infer data.
        // @param node_name The output node name.
        // @param data_size The length of output data.
        // @return The result of ata array.
        public T[] read_infer_result<T>(string node_name, int data_size)
        {
            // Get Setting Type
            string t = typeof(T).ToString();
            // Create a new return value array
            T[] result = new T[data_size];
            if (t == "System.Int32")
            { // Read data type as int data
                int[] inference_result = new int[data_size];
                NativeMethods.read_infer_result_I32(core_ptr, node_name, ref inference_result[0]);
                result = (T[])Convert.ChangeType(inference_result, typeof(T[]));
                return result;
            }
            else if (t == "System.Int64")
            {
                // Read data type as long data
                long[] inference_result = new long[data_size];
                NativeMethods.read_infer_result_I64(core_ptr, node_name, ref inference_result[0]);
                result = (T[])Convert.ChangeType(inference_result, typeof(T[]));
                return result;
            }
            else
            {
                // Read data type as float data
                float[] inference_result = new float[data_size];
                NativeMethods.read_infer_result_F32(core_ptr, node_name, ref inference_result[0]);
                result = (T[])Convert.ChangeType(inference_result, typeof(T[]));
                return result;
            }
        }
        // @brief Delete core adress.
        public void delet()
        {
            NativeMethods.core_delet(core_ptr);
        }

    }
}