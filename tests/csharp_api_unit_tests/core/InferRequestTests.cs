using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenVinoSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.Tests
{
    [TestClass()]
    public class InferRequestTests : OVBaseTest
    {
        [TestMethod()]
        public void InferRequestTest()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            InferRequest request = compiled_model.create_infer_request();
            Assert.IsTrue(request.Ptr != IntPtr.Zero);
            request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void DisposeTest()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            InferRequest request = compiled_model.create_infer_request();
            Assert.IsTrue(request.Ptr != IntPtr.Zero);
            request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void set_tensorTest()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            InferRequest request = compiled_model.create_infer_request();
            Assert.IsTrue(request.Ptr != IntPtr.Zero);

            Tensor input_tensor = new Tensor()

            request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void set_tensorTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void set_tensorTest2()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void set_input_tensorTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void set_input_tensorTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void set_output_tensorTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void set_output_tensorTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void get_tensorTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void get_tensorTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void get_tensorTest2()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void get_tensorTest3()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void get_input_tensorTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void get_input_tensorTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void get_output_tensorTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void get_output_tensorTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void inferTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void cancelTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void start_asyncTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void waitTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void wait_forTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void get_profiling_infoTest()
        {
            Assert.Fail();
        }
    }
}