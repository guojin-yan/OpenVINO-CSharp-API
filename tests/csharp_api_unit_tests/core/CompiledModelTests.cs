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
    public class CompiledModelTests : OVBaseTest
    {
        [TestMethod()]
        public void CompiledModel_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void Dispose_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            compiled_model.Dispose();
            Assert.IsTrue(compiled_model.Ptr == IntPtr.Zero);
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void create_infer_request_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            InferRequest infer_request = compiled_model.create_infer_request();
            Assert.IsTrue(infer_request.Ptr != IntPtr.Zero);
            infer_request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_input_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            Node input = compiled_model.get_input();
            Assert.IsTrue(input.Ptr != IntPtr.Zero);
            input.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_input_test1()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            Node input = compiled_model.get_input(model_input_name());
            Assert.IsTrue(input.Ptr != IntPtr.Zero);
            input.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_input_test2()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            Node input = compiled_model.get_input(0);
            Assert.IsTrue(input.Ptr != IntPtr.Zero);
            input.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_output_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            Node output = compiled_model.get_output();
            Assert.IsTrue(output.Ptr != IntPtr.Zero);
            output.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_output_test1()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            Node output = compiled_model.get_output(model_output_name());
            Assert.IsTrue(output.Ptr != IntPtr.Zero);
            output.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_output_test2()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            Node output = compiled_model.get_output(0);
            Assert.IsTrue(output.Ptr != IntPtr.Zero);
            output.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_inputs_size_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            ulong size = compiled_model.get_inputs_size();
            Assert.IsTrue(size > 0);
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_outputs_size_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            ulong size = compiled_model.get_outputs_size();
            Assert.IsTrue(size > 0);
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void input_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            Input input = compiled_model.input();
            Assert.IsTrue(input.get_node().Ptr != IntPtr.Zero);
            input.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void input_test1()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            Input input = compiled_model.input(model_input_name());
            Assert.IsTrue(input.get_node().Ptr != IntPtr.Zero);
            input.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void input_test2()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            Input input = compiled_model.input(0);
            Assert.IsTrue(input.get_node().Ptr != IntPtr.Zero);
            input.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void output_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            Output output = compiled_model.output();
            Assert.IsTrue(output.get_node().Ptr != IntPtr.Zero);
            output.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void output_test1()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            Output output = compiled_model.output(model_output_name());
            Assert.IsTrue(output.get_node().Ptr != IntPtr.Zero);
            output.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void output_test2()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            Output output = compiled_model.output(0);
            Assert.IsTrue(output.get_node().Ptr != IntPtr.Zero);
            output.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void inputs_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            List<Input> inputs = compiled_model.inputs();
            Assert.IsTrue(inputs.Count > 0);
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void outputs_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            List<Output> outputs = compiled_model.outputs();
            Assert.IsTrue(outputs.Count > 0);
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_runtime_model_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            Model runtime = compiled_model.get_runtime_model();
            Assert.IsTrue(runtime.Ptr != IntPtr.Zero);
            runtime.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void export_model_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model,get_device());
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            compiled_model.export_model("test_exported_model.blob");
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void set_property_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model, "BATCH:" + get_device() + "(4)");
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            KeyValuePair<string, string> key = new KeyValuePair<string, string>(PropertyKey.AUTO_BATCH_TIMEOUT.ToString(), "5000");
            compiled_model.set_property(key);
            string result = compiled_model.get_property("AUTO_BATCH_TIMEOUT");
            Assert.AreEqual("5000", result);
        }

        [TestMethod()]
        public void get_property_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model, get_device());
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            string result = compiled_model.get_property("SUPPORTED_PROPERTIES");
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_context_test()
        {
            Assert.Fail();
        }
    }
}