using OpenVinoSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    public class test_compiled_model
    {
        public static void test_create_infer_request() 
        {
            Core core = new Core();

            Model model = core.read_model(@"E:\Text_Model\yolov8\yolov8s.xml");

            CompiledModel compiled_model = core.compiled_model(model, "CPU");
            
            InferRequest infer_request = compiled_model.create_infer_request();
            Console.WriteLine("CompiledModel create_infer_request() 成功！");
            core.free();
        }
    }
}
