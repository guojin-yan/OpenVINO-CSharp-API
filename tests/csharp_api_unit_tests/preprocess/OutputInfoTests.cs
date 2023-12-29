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
    public class OutputInfoTests : OVBaseTest
    {
        [TestMethod()]
        public void OutputInfo_test()
        {
        }

        [TestMethod()]
        public void Dispose_test()
        {
        }

        [TestMethod()]
        public void tensor_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);
            Assert.IsTrue(processor.Ptr != IntPtr.Zero);
            OutputInfo output = processor.output();
            Assert.IsTrue(output.Ptr != IntPtr.Zero);
            OutputTensorInfo tensor_info = output.tensor();
            Assert.IsTrue(tensor_info.Ptr != IntPtr.Zero);
            tensor_info.Dispose();
            output.Dispose();
            processor.Dispose();
            model.Dispose();
            core.Dispose();
        }
    }
}