using OpenCvSharp;
using OpenCvSharp.Dnn;
using OpenVinoSharp;
using System.Data;
using System.Runtime.InteropServices;

namespace test_openvinosharp_api
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string model_path = "./../../../../../model\\yolov8\\yolov8s-cls.xml";
            Core core = new Core(); // 初始化推理核心
            Model model = core.read_model(model_path); // 读取本地模型
            CompiledModel compiled_model = core.compile_model(model, "AUTO"); // 便哟模型到指定设备

            // 获取模型的输入输出信息
            Console.WriteLine("Model name: {0}", model.get_friendly_name());
            Input input = compiled_model.input();
            Console.WriteLine("/------- [In] -------/");
            Console.WriteLine("Input name: {0}", input.get_any_name());
            Console.WriteLine("Input type: {0}", input.get_element_type().to_string());
            Console.WriteLine("Input shape: {0}", input.get_shape().to_string());
            Output output = compiled_model.output();
            Console.WriteLine("/------- [Out] -------/");
            Console.WriteLine("Output name: {0}", output.get_any_name());
            Console.WriteLine("Output type: {0}", output.get_element_type().to_string());
            Console.WriteLine("Output shape: {0}", output.get_shape().to_string());
            // 创建推理请求
            InferRequest infer_request = compiled_model.create_infer_request();
            // 获取输入张量
            Tensor input_tensor = infer_request.get_input_tensor();
            Console.WriteLine("/------- [Input tensor] -------/");
            Console.WriteLine("Input tensor type: {0}", input_tensor.get_element_type().to_string());
            Console.WriteLine("Input tensor shape: {0}", input_tensor.get_shape().to_string());
            Console.WriteLine("Input tensor size: {0}", input_tensor.get_size());
            // 读取并处理输入数据
            Mat image = Cv2.ImRead(@"./../../../../../dataset\image\demo_7.jpg");
            Mat input_mat = new Mat();
            input_mat = CvDnn.BlobFromImage(image, 1.0 / 255.0, new Size(224, 224), 0, true, false);
            // 加载推理数据
            Shape input_shape = input_tensor.get_shape();
            long channels = input_shape[1];
            long height = input_shape[2];
            long width = input_shape[3];
            float[] input_data = new float[channels * height * width];
            Marshal.Copy(input_mat.Ptr(0), input_data, 0, input_data.Length);
            input_tensor.set_data(input_data);
            // 模型推理
            infer_request.infer(); 
            // 获取输出张量
            Tensor output_tensor = infer_request.get_output_tensor();
            Console.WriteLine("/------- [Output tensor] -------/");
            Console.WriteLine("Output tensor type: {0}", output_tensor.get_element_type().to_string());
            Console.WriteLine("Output tensor shape: {0}", output_tensor.get_shape().to_string());
            Console.WriteLine("Output tensor size: {0}", output_tensor.get_size());
            // 获取输出数据
            float[] result = output_tensor.get_data<float>(1000);
            List<float[]> new_list = new List<float[]> { };
            for (int i = 0; i < result.Length; i++)
            {
                new_list.Add(new float[] { (float)result[i], i });
            }
            new_list.Sort((a, b) => b[0].CompareTo(a[0]));

            KeyValuePair<int, float>[] cls = new KeyValuePair<int, float>[10];
            for (int i = 0; i < 10; ++i)
            {
                cls[i] = new KeyValuePair<int, float>((int)new_list[i][1], new_list[i][0]);
            }
            Console.WriteLine("\n Classification Top 10 result : \n");
            Console.WriteLine("classid probability");
            Console.WriteLine("------- -----------");

            string[] labelNames=core.GetLabelNames();
            for (int i = 0; i < 10; ++i)
            {
                Console.WriteLine("{0}  {2}   {1}", cls[i].Key.ToString("0"), cls[i].Value.ToString("0.000000"), labelNames[cls[i].Key]);                
            }
            // 销毁非托管内存
            output_tensor.dispose();
            input_shape.dispose();
            infer_request.dispose();
            compiled_model.dispose();
            input.dispose();
            output.dispose();
            model.dispose();
            core.dispose();

        }
    }
}

