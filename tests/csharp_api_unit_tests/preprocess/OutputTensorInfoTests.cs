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
    public class OutputTensorInfoTests : OVBaseTest
    {
        [TestMethod()]
        public void OutputTensorInfo_test()
        {
        }

        [TestMethod()]
        public void Dispose_test()
        {
        }

        [TestMethod()]
        public void set_element_type_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);
            OutputInfo output = processor.output();
            Assert.IsNotNull(output);
            OutputTensorInfo output_tensor = output.tensor();
            Assert.IsNotNull(output_tensor);
            output_tensor.set_element_type(new OvType(ElementType.F32));
            output.Dispose();
            processor.Dispose();
            model.Dispose();
        }
    }
}