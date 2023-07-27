using OpenVinoSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace test
{
    public class test_tensor
    {
        public static void test_get_shape() {
            Core core = new Core();

            Model model = core.read_model(@"E:\GitSpace\OpenVinoSharp\model\yolov8s.xml");

            CompiledModel compiled_model = core.compiled_model(model, "CPU");

            InferRequest infer_request = compiled_model.create_infer_request();
            Tensor tensor = infer_request.get_tensor("images");
            Shape shape = tensor.get_shape();
            Console.WriteLine("shape "+ shape.shape.rank);
            Console.WriteLine(shape.to_string());
            Console.WriteLine("Tensor get_shape() 成功！");
            core.free();
        }
        public static void test_get_element_type()
        {
            Core core = new Core();

            Model model = core.read_model(@"E:\GitSpace\OpenVinoSharp\model\yolov8s.xml");

            CompiledModel compiled_model = core.compiled_model(model, "CPU");

            InferRequest infer_request = compiled_model.create_infer_request();
            Tensor tensor = infer_request.get_tensor("images");
            OpenVinoSharp.element.Type type = tensor.get_element_type();
            Console.WriteLine("tensor type = {0} ;",type.to_string());
            Console.WriteLine("Tensor get_shape() 成功！");
            core.free();
        }
    }
}
