using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenVinoSharp.preprocess;
using OpenVinoSharp.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.preprocess.Tests
{
    [TestClass()]
    public class PrePostProcessorTests : OVBaseTest
    {
        [TestMethod()]
        public void PrePostProcessor_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);
            processor.Dispose();
            model.Dispose();
        }

        [TestMethod()]
        public void Dispose_test()
        {
        }

        [TestMethod()]
        public void input_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);
            InputInfo input = processor.input();
            input.Dispose();
            processor.Dispose();
            model.Dispose(); 
        }

        [TestMethod()]
        public void input_test1()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);
            InputInfo input = processor.input(model_input_name());
            input.Dispose();
            processor.Dispose();
            model.Dispose();
        }

        [TestMethod()]
        public void input_test2()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);
            InputInfo input = processor.input(0);
            input.Dispose();
            processor.Dispose();
            model.Dispose();
        }

        [TestMethod()]
        public void output_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);
            OutputInfo input = processor.output();
            input.Dispose();
            processor.Dispose();
            model.Dispose();
        }

        [TestMethod()]
        public void output_test1()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);
            OutputInfo input = processor.output(model_output_name());
            input.Dispose();
            processor.Dispose();
            model.Dispose();
        }

        [TestMethod()]
        public void output_test2()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);
            OutputInfo input = processor.output(0);
            input.Dispose();
            processor.Dispose();
            model.Dispose();
        }

        [TestMethod()]
        public void build_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);

            Tensor input_tensor = new Tensor(new OvType(ElementType.U8), new Shape(1, 640, 640, 3));
            InputInfo input_info = processor.input(0);
            InputTensorInfo input_tensor_info = input_info.tensor();
            input_tensor_info.set_from(input_tensor).set_layout(new Layout("NHWC")).set_color_format(ColorFormat.BGR);

            PreProcessSteps process_steps = input_info.preprocess();
            process_steps.convert_color(ColorFormat.RGB).resize(ResizeAlgorithm.RESIZE_LINEAR)
                .convert_element_type(new OvType(ElementType.F32)).scale(255.0f).convert_layout(new Layout("NCHW"));

            Model new_model = processor.build();
            new_model.Dispose();
            processor.Dispose();
            model.Dispose();
        }
    }
}