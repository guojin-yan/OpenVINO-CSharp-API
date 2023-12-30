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
    public class PreProcessStepsTests:OVBaseTest
    {
        [TestMethod()]
        public void PreProcessSteps_test()
        {
        }

        [TestMethod()]
        public void Dispose_test()
        {
        }

        [TestMethod()]
        public void resize_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);
            Assert.IsTrue(processor.Ptr != IntPtr.Zero);
            InputInfo input = processor.input();
            Assert.IsTrue(input.Ptr != IntPtr.Zero);
            PreProcessSteps process_steps = input.preprocess();
            Assert.IsTrue(process_steps.Ptr != IntPtr.Zero);

            process_steps.resize(ResizeAlgorithm.RESIZE_LINEAR);

            process_steps.Dispose();
            input.Dispose();
            processor.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void scale_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);
            Assert.IsTrue(processor.Ptr != IntPtr.Zero);
            InputInfo input = processor.input();
            Assert.IsTrue(input.Ptr != IntPtr.Zero);
            PreProcessSteps process_steps = input.preprocess();
            Assert.IsTrue(process_steps.Ptr != IntPtr.Zero);

            process_steps.scale(0.5f);

            process_steps.Dispose();
            input.Dispose();
            processor.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void mean_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);
            Assert.IsTrue(processor.Ptr != IntPtr.Zero);
            InputInfo input = processor.input();
            Assert.IsTrue(input.Ptr != IntPtr.Zero);
            PreProcessSteps process_steps = input.preprocess();
            Assert.IsTrue(process_steps.Ptr != IntPtr.Zero);

            process_steps.mean(0.5f);

            process_steps.Dispose();
            input.Dispose();
            processor.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void crop_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);
            Assert.IsTrue(processor.Ptr != IntPtr.Zero);
            InputInfo input = processor.input();
            Assert.IsTrue(input.Ptr != IntPtr.Zero);
            PreProcessSteps process_steps = input.preprocess();
            Assert.IsTrue(process_steps.Ptr != IntPtr.Zero);


            int[] begin = { 0, 0, 5, 10 };
            int[] end = { 1, 3, 15, 20 };

            process_steps.crop(begin, end);

            process_steps.Dispose();
            input.Dispose();
            processor.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void crop_test1()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);
            Assert.IsTrue(processor.Ptr != IntPtr.Zero);
            InputInfo input = processor.input();
            Assert.IsTrue(input.Ptr != IntPtr.Zero);
            PreProcessSteps process_steps = input.preprocess();
            Assert.IsTrue(process_steps.Ptr != IntPtr.Zero);


            List<int> begin = new List<int> { 0, 0, 5, 10 };
           List<int> end = new List<int>{ 1, 3, 15, 20 };

            process_steps.crop(begin, end);

            process_steps.Dispose();
            input.Dispose();
            processor.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void convert_layout_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);
            Assert.IsTrue(processor.Ptr != IntPtr.Zero);
            InputInfo input = processor.input();
            Assert.IsTrue(input.Ptr != IntPtr.Zero);
            PreProcessSteps process_steps = input.preprocess();
            Assert.IsTrue(process_steps.Ptr != IntPtr.Zero);

            process_steps.convert_layout(new Layout("NCHW"));

            process_steps.Dispose();
            input.Dispose();
            processor.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void reverse_channels_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);
            Assert.IsTrue(processor.Ptr != IntPtr.Zero);
            InputInfo input = processor.input();
            Assert.IsTrue(input.Ptr != IntPtr.Zero);
            PreProcessSteps process_steps = input.preprocess();
            Assert.IsTrue(process_steps.Ptr != IntPtr.Zero);

            process_steps.reverse_channels();

            process_steps.Dispose();
            input.Dispose();
            processor.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void convert_color_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);
            Assert.IsTrue(processor.Ptr != IntPtr.Zero);
            InputInfo input = processor.input();
            Assert.IsTrue(input.Ptr != IntPtr.Zero);
            PreProcessSteps process_steps = input.preprocess();
            Assert.IsTrue(process_steps.Ptr != IntPtr.Zero);

            process_steps.convert_color(ColorFormat.RGB);

            process_steps.Dispose();
            input.Dispose();
            processor.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void convert_element_type_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);
            Assert.IsTrue(processor.Ptr != IntPtr.Zero);
            InputInfo input = processor.input();
            Assert.IsTrue(input.Ptr != IntPtr.Zero);
            PreProcessSteps process_steps = input.preprocess();
            Assert.IsTrue(process_steps.Ptr != IntPtr.Zero);

            process_steps.convert_element_type(new OvType(ElementType.F32));

            process_steps.Dispose();
            input.Dispose();
            processor.Dispose();
            model.Dispose();
            core.Dispose();
        }
    }
}