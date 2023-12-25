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
    public class CoreTests : OVBaseTest
    {


        [TestMethod()]
        public void Core_test()
        {
            Core core = new Core();
            Assert.IsTrue(core.Ptr != IntPtr.Zero);
        }

        [TestMethod()]
        public void Dispose_test()
        {
            Core core = new Core();
            core.Dispose();
            Assert.IsTrue(core.Ptr == IntPtr.Zero);
        }

        [TestMethod()]
        public void get_versions_test()
        {
            var core = new Core();
            KeyValuePair<string, Version> ver = core.get_versions("CPU");
            Assert.IsNotNull(ver.Key, ver.Value.buildNumber, ver.Value.description);
        }

        [TestMethod()]
        public void read_model_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            model.Dispose();
            model = core.read_model(get_model_xml_file_name(), get_model_bin_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void read_model_test1()
        {
            byte[] data = content_from_file(get_model_bin_file_name());

            Shape shape = new Shape(new List<long> { 1, data.Length });
            Tensor tensor = new Tensor(new element.Type(element.Type_t.u8), shape, data);

            Core core = new Core();
            Assert.IsTrue(core.Ptr != IntPtr.Zero);
            Model model = core.read_model(Path.GetFullPath(get_model_xml_file_name()), tensor);
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
        }

        [TestMethod()]
        public void compile_model_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled = core.compile_model(model);
            Assert.IsTrue(compiled.Ptr != IntPtr.Zero);
            compiled.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void compile_model_test1()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);

            Dictionary<string, string> latency = new Dictionary<string, string>();
            latency.Add("PERFORMANCE_HINT", "1");

            CompiledModel compiled = core.compile_model(model, "CPU", latency);
            Assert.IsTrue(compiled.Ptr != IntPtr.Zero);
            latency.Add("PERFORMANCE", "1");
            compiled = core.compile_model(get_model_xml_file_name(), "CPU", latency);
            Assert.IsTrue(compiled.Ptr != IntPtr.Zero);
            compiled.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void compile_model_test2()
        {
            var core = new Core();
            Dictionary<string, string> latency = new Dictionary<string, string>();
            latency.Add("PERFORMANCE_HINT", "1");
            CompiledModel compiled = core.compile_model(get_model_xml_file_name(), latency);
            Assert.IsTrue(compiled.Ptr != IntPtr.Zero);
            latency.Add("PERFORMANCE", "1");
            compiled = core.compile_model(get_model_xml_file_name(), "CPU", latency);
            Assert.IsTrue(compiled.Ptr != IntPtr.Zero);
            compiled.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void compile_model_test3()
        {
            var core = new Core();
            Dictionary<string, string> latency = new Dictionary<string, string>();
            latency.Add("PERFORMANCE_HINT", "1");
            CompiledModel compiled = core.compile_model(get_model_xml_file_name(), "CPU", latency);
            Assert.IsTrue(compiled.Ptr != IntPtr.Zero);
            latency.Add("PERFORMANCE", "1");
            compiled = core.compile_model(get_model_xml_file_name(), "CPU", latency);
            Assert.IsTrue(compiled.Ptr != IntPtr.Zero);
            compiled.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_available_devices_test()
        {
            var core = new Core();
            List<string> devicces = core.get_available_devices();
            Assert.IsNotNull(devicces);
        }

        [TestMethod()]
        public void set_propertyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void get_propertyTest()
        {
            Assert.Fail();
        }
    }
}
