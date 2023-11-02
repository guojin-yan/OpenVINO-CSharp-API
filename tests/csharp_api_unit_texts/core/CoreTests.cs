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
    public class CoreTests
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
            Assert.IsNotNull(ver.Key,ver.Value.buildNumber,ver.Value.description);
        }

        [TestMethod()]
        public void read_model_test()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            model.Dispose();
            model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml",
                "..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.bin");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void read_model_test1()
        {
            string xml_name = "..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml";
            string bin_name = "..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.bin";
            FileStream fs = new FileStream(bin_name, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long data_length = new FileInfo(bin_name).Length;
            byte[] bin_buff = br.ReadBytes((int)data_length);

            Shape shape = new Shape(new List<long> { 1, data_length });
            Tensor tensor = new Tensor(new element.Type(element.Type_t.u8),
                shape, bin_buff);

            Core core = new Core();
            Assert.IsTrue(core.Ptr != IntPtr.Zero);
            Model model = core.read_model(xml_name, tensor);
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
        }

        [TestMethod()]
        public void compile_model_test()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
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
            //var core = new Core();
            //Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            //Assert.IsTrue(model.Ptr != IntPtr.Zero);

            //CompiledModel compiled = core.compile_model(model, "CPU");
            //Assert.IsTrue(compiled.Ptr != IntPtr.Zero);
            //compiled.Dispose();
            //model.Dispose();
            //core.Dispose();
            Assert.Fail();
        }

        [TestMethod()]
        public void compile_model_test2()
        {
            var core = new Core();
            CompiledModel compiled = core.compile_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(compiled.Ptr != IntPtr.Zero);
            compiled.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void compile_model_test3()
        {
            var core = new Core();
            CompiledModel compiled = core.compile_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml", "CPU");
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
    }
}
