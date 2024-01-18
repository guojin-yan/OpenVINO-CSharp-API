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
            KeyValuePair<string, Version> ver = core.get_versions(get_device());
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
            Model model = core.read_model(get_model_xml_file_name(), tensor);
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

            CompiledModel compiled = core.compile_model(model, get_device(), latency);
            Assert.IsTrue(compiled.Ptr != IntPtr.Zero);
            latency.Add("PERFORMANCE", "1");
            compiled = core.compile_model(get_model_xml_file_name(), get_device(), latency);
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
            compiled = core.compile_model(get_model_xml_file_name(), get_device(), latency);
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
            CompiledModel compiled = core.compile_model(get_model_xml_file_name(), get_device(), latency);
            Assert.IsTrue(compiled.Ptr != IntPtr.Zero);
            latency.Add("PERFORMANCE", "1");
            compiled = core.compile_model(get_model_xml_file_name(), get_device(), latency);
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
            core.Dispose();
        }

        [TestMethod()]
        public void set_property_enum_Test()
        {
            var core = new Core();
            KeyValuePair<string, string> key = new KeyValuePair<string, string>(PropertyKey.LOG_LEVEL.ToString(), "WARNING");
            core.set_property(get_device(), key);
            core.Dispose();
        }
        //[TestMethod()]
        //public void set_property_invalid_number_property_arguments_Test()
        //{
        //    var core = new Core();
        //    Dictionary<string, string> dict = new Dictionary<string, string>();
        //    dict.Add(PropertyKey.INFERENCE_NUM_THREADS.ToString(), "12");
        //    dict.Add(PropertyKey.NUM_STREAMS.ToString(), "7");
        //    core.set_property(get_device(), dict);
        //    string s = core.get_property(get_device(), PropertyKey.INFERENCE_NUM_THREADS);
        //    Assert.AreEqual("12", s);
        //    s = core.get_property(get_device(), PropertyKey.NUM_STREAMS);
        //    Assert.AreEqual("7", s);
        //    core.Dispose();
        //}

        [TestMethod()]
        public void set_property_enum_invalid_Test()
        {
            var core = new Core();
            KeyValuePair<string, string> key = new KeyValuePair<string, string>(PropertyKey.PERFORMANCE_HINT.ToString(), "LATENCY");
            core.set_property(get_device(), key);
            string s = core.get_property(get_device(), PropertyKey.PERFORMANCE_HINT);
            Assert.AreEqual("LATENCY", s);

            //key = new KeyValuePair<string, string>(PropertyKey.PERFORMANCE_HINT.ToString(), "LATENCY_TEST");
            //core.set_property(get_device(), key);
            //s = core.get_property(get_device(), PropertyKey.PERFORMANCE_HINT);
            //Assert.AreEqual("LATENCY_TEST", s);

            key = new KeyValuePair<string, string>(PropertyKey.ENABLE_CPU_PINNING.ToString(), "YES");
            core.set_property(get_device(), key);
            s = core.get_property(get_device(), PropertyKey.ENABLE_CPU_PINNING);
            Assert.AreEqual("YES", s);

            //key = new KeyValuePair<string, string>(PropertyKey.ENABLE_CPU_PINNING.ToString(), "INVALID_VAL");
            //core.set_property(get_device(), key);
            //s = core.get_property(get_device(), PropertyKey.ENABLE_CPU_PINNING);
            //Assert.AreEqual("INVALID_VAL", s);

            key = new KeyValuePair<string, string>(PropertyKey.SCHEDULING_CORE_TYPE.ToString(), "PCORE_ONLY");
            core.set_property(get_device(), key);
            s = core.get_property(get_device(), PropertyKey.SCHEDULING_CORE_TYPE);
            Assert.AreEqual("PCORE_ONLY", s);

            //key = new KeyValuePair<string, string>(PropertyKey.SCHEDULING_CORE_TYPE.ToString(), "INVALID_VAL");
            //core.set_property(get_device(), key);
            //s = core.get_property(get_device(), PropertyKey.SCHEDULING_CORE_TYPE);
            //Assert.AreEqual("INVALID_VAL", s);

            key = new KeyValuePair<string, string>(PropertyKey.ENABLE_HYPER_THREADING.ToString(), "YES");
            core.set_property(get_device(), key);
            s = core.get_property(get_device(), PropertyKey.ENABLE_HYPER_THREADING);
            Assert.AreEqual("YES", s);

            //key = new KeyValuePair<string, string>(PropertyKey.ENABLE_HYPER_THREADING.ToString(), "INVALID_VAL");
            //core.set_property(get_device(), key);
            //s = core.get_property(get_device(), PropertyKey.ENABLE_HYPER_THREADING);
            //Assert.AreEqual("INVALID_VAL", s);

            core.Dispose();
        }



        [TestMethod()]
        public void get_propertyTest()
        {
            var core = new Core();
            core.set_property(get_device(), Ov.cache_dir("./model"));
            string s = core.get_property(get_device(), PropertyKey.CACHE_DIR);
            Assert.IsNotNull(s);
        }

        [TestMethod()]
        public void set_propertyTest1()
        {
            var core = new Core();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add(Ov.cache_dir("./model").Key, Ov.cache_dir("./model").Value);
            core.set_property(get_device(), dict);
            string s = core.get_property(get_device(), PropertyKey.CACHE_DIR);
            Assert.IsNotNull(s);
        }

        [TestMethod()]
        public void import_modelTest()
        {
        }
    }
}
