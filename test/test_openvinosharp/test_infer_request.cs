using OpenVinoSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    public class test_infer_request
    {
        public static void test_get_tensor() 
        {
            Core core = new Core();

            Model model = core.read_model(@"E:\Text_Model\yolov8\yolov8s.xml");

            CompiledModel compiled_model = core.compiled_model(model, "CPU");

            InferRequest infer_request = compiled_model.create_infer_request();
            Tensor tensor = infer_request.get_tensor("images");
            Console.WriteLine("CompiledModel get_tensor() 成功！");
            core.free();
            
        }
    }
}
