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
            Model new_model = processor.build();
            new_model.Dispose();
            processor.Dispose();
            model.Dispose();
        }
    }
}