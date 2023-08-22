using OpenVinoSharp;
namespace test_openvinosharp_api
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            string model_path = "E:\\Model\\classification\\public\\resnet-50-pytorch\\FP32\\resnet-50-pytorch.xml";
            //string model_path = "E:\\Model\\model.onnx";
            Core core = new Core();

            Model model = core.read_model(model_path);
            CompiledModel compiled_model = core.compile_model(model, "AUTO");


            Console.WriteLine("Model name: {0}", model.get_friendly_name());
            Input input = compiled_model.input();
            Console.WriteLine("/------- [In] -------/");
            Console.WriteLine("Input name: {0}", input.get_any_name());
            Console.WriteLine("Input type: {0}", input.get_element_type().to_string());

            //Console.WriteLine("Input shape: {0}", input.get_partial_shape().to_string());
            Output output = compiled_model.output();
            Console.WriteLine("/------- [Out] -------/");
            Console.WriteLine("Output name: {0}", output.get_any_name());
            Console.WriteLine("Output type: {0}", output.get_element_type().to_string());
            Console.WriteLine("Output shape: {0}", output.get_shape().to_string());

            InferRequest infer_request = compiled_model.create_infer_request();
        }
    }
}