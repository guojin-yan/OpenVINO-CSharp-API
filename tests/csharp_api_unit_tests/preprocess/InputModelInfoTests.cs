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
    public class InputModelInfoTests : OVBaseTest
    {
        [TestMethod()]
        public void InputModelInfo_test()
        {
        }

        [TestMethod()]
        public void Dispose_test()
        {
        }

        [TestMethod()]
        public void set_layout_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);
            Assert.IsTrue(processor.Ptr != IntPtr.Zero);
            InputInfo input = processor.input();
            Assert.IsTrue(input.Ptr != IntPtr.Zero);
            InputModelInfo model_info = input.model();
            Assert.IsTrue(model_info.Ptr != IntPtr.Zero);
            model_info.set_layout(new Layout("NCHW"));
            model_info.Dispose();
            input.Dispose();
            processor.Dispose();
            model.Dispose();
            core.Dispose();
        }
    }
}