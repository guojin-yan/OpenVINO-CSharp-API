using OpenVinoSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    public class test_tensor
    {
        public static void test_get_shape() {
            Core core = new Core();

            Model model = core.read_model(@"E:\Text_Model\yolov8\yolov8s.xml");

            CompiledModel compiled_model = core.compiled_model(model, "CPU");

            InferRequest infer_request = compiled_model.create_infer_request();
            Tensor tensor = infer_request.get_tensor("images");
            Shape shape = tensor.get_shape();
            Console.WriteLine("shape "+ shape.shape.rank);
            long[] dims = shape.get_dims();
            Console.WriteLine("dims({0}, {1}, {2}, {3})", dims[0], dims[1], dims[2], dims[3]);
            Console.WriteLine("Tensor get_shape() 成功！");
            core.free();
        }
        public static void test_get_element_type()
        {
            Core core = new Core();

            Model model = core.read_model(@"E:\Text_Model\yolov8\yolov8s.xml");

            CompiledModel compiled_model = core.compiled_model(model, "CPU");

            InferRequest infer_request = compiled_model.create_infer_request();
            Tensor tensor = infer_request.get_tensor("images");
            uint type = tensor.get_element_type();
            Console.WriteLine("tensor type = {0} ;",type);
            Console.WriteLine("Tensor get_shape() 成功！");
            core.free();
        }
    }
}
