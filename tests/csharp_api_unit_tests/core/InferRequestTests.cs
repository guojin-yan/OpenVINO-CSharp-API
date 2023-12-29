using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenVinoSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
            InferRequest infer_request = compiled_model.create_infer_request();
            Assert.IsTrue(infer_request.Ptr != IntPtr.Zero);
            float[] data = new float[model_input_shape().data_size()];
            data[1] = 15.62f;
            Tensor input_tensor = new Tensor(model_input_shape(), data);
            float[] d = input_tensor.get_data<float>((int)model_input_shape().data_size());
            infer_request.set_tensor(model_input_name(), input_tensor);
            input_tensor.Dispose();
            infer_request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void set_tensorTest1()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            InferRequest infer_request = compiled_model.create_infer_request();
            Assert.IsTrue(infer_request.Ptr != IntPtr.Zero);
            float[] data = new float[model_input_shape().data_size()];
            Node node = model.get_input();
            Tensor input_tensor = new Tensor(model_input_shape(), data);
            infer_request.set_tensor(node, input_tensor);
            input_tensor.Dispose();
            infer_request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void set_tensorTest2()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            InferRequest infer_request = compiled_model.create_infer_request();
            Assert.IsTrue(infer_request.Ptr != IntPtr.Zero);
            float[] data = new float[model_input_shape().data_size()];
            Input node = model.input();
            Tensor input_tensor = new Tensor(model_input_shape(), data);
            infer_request.set_tensor(node, input_tensor);
            input_tensor.Dispose();
            infer_request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void set_tensorTest3()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            InferRequest infer_request = compiled_model.create_infer_request();
            Assert.IsTrue(infer_request.Ptr != IntPtr.Zero);
            float[] data = new float[model_input_shape().data_size()];
            Node node = model.get_input();
            Output node_output = new Output(node);
            Tensor input_tensor = new Tensor(model_input_shape(), data);
            infer_request.set_tensor(node_output, input_tensor);
            input_tensor.Dispose();
            infer_request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }
        [TestMethod()]
        public void set_input_tensorTest()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            InferRequest infer_request = compiled_model.create_infer_request();
            Assert.IsTrue(infer_request.Ptr != IntPtr.Zero);
            float[] data = new float[model_input_shape().data_size()];
            Tensor input_tensor = new Tensor(model_input_shape(), data);
            infer_request.set_input_tensor(0, input_tensor);
            input_tensor.Dispose();
            infer_request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void set_input_tensorTest1()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            InferRequest infer_request = compiled_model.create_infer_request();
            Assert.IsTrue(infer_request.Ptr != IntPtr.Zero);
            float[] data = new float[model_input_shape().data_size()];
            Tensor input_tensor = new Tensor(model_input_shape(), data);
            infer_request.set_input_tensor(input_tensor);
            input_tensor.Dispose();
            infer_request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void set_output_tensorTest()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            InferRequest infer_request = compiled_model.create_infer_request();
            Assert.IsTrue(infer_request.Ptr != IntPtr.Zero);
            float[] data = new float[model_output_shape().data_size()];
            Tensor output_tensor = new Tensor(model_output_shape(), data);
            infer_request.set_output_tensor(0, output_tensor);
            output_tensor.Dispose();
            infer_request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void set_output_tensorTest1()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            InferRequest infer_request = compiled_model.create_infer_request();
            Assert.IsTrue(infer_request.Ptr != IntPtr.Zero);
            float[] data = new float[model_output_shape().data_size()];
            Tensor output_tensor = new Tensor(model_output_shape(), data);
            infer_request.set_output_tensor(output_tensor);
            output_tensor.Dispose();
            infer_request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_tensorTest()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            InferRequest infer_request = compiled_model.create_infer_request();
            Assert.IsTrue(infer_request.Ptr != IntPtr.Zero);

            Tensor tensor = infer_request.get_tensor(model_input_name());
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            tensor.Dispose();
            infer_request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_tensorTest1()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            InferRequest infer_request = compiled_model.create_infer_request();
            Assert.IsTrue(infer_request.Ptr != IntPtr.Zero);
            Node node = model.get_input();
            Assert.IsTrue(node.Ptr != IntPtr.Zero);
            Tensor tensor = infer_request.get_tensor(node);
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            node.Dispose();
            tensor.Dispose();
            infer_request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_tensorTest2()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            InferRequest infer_request = compiled_model.create_infer_request();
            Assert.IsTrue(infer_request.Ptr != IntPtr.Zero);
            Input node = model.input();
            Assert.IsTrue(node.get_node().Ptr != IntPtr.Zero);
            Tensor tensor = infer_request.get_tensor(node);
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            node.Dispose();
            tensor.Dispose();
            infer_request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_tensorTest3()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            InferRequest infer_request = compiled_model.create_infer_request();
            Assert.IsTrue(infer_request.Ptr != IntPtr.Zero);
            Output node = model.output();
            Assert.IsTrue(node.get_node().Ptr != IntPtr.Zero);
            Tensor tensor = infer_request.get_tensor(node);
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            node.Dispose();
            tensor.Dispose();
            infer_request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_input_tensorTest()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            InferRequest infer_request = compiled_model.create_infer_request();
            Assert.IsTrue(infer_request.Ptr != IntPtr.Zero); 
            Tensor tensor = infer_request.get_input_tensor(0);
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            tensor.Dispose();
            infer_request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_input_tensorTest1()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            InferRequest infer_request = compiled_model.create_infer_request();
            Assert.IsTrue(infer_request.Ptr != IntPtr.Zero);
            Tensor tensor = infer_request.get_input_tensor();
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            tensor.Dispose();
            infer_request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_output_tensorTest()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            InferRequest infer_request = compiled_model.create_infer_request();
            Assert.IsTrue(infer_request.Ptr != IntPtr.Zero);
            Tensor tensor = infer_request.get_output_tensor(0);
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            tensor.Dispose();
            infer_request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_output_tensorTest1()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            InferRequest infer_request = compiled_model.create_infer_request();
            Assert.IsTrue(infer_request.Ptr != IntPtr.Zero);
            Tensor tensor = infer_request.get_output_tensor();
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            tensor.Dispose();
            infer_request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void inferTest()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            InferRequest infer_request = compiled_model.create_infer_request();
            Assert.IsTrue(infer_request.Ptr != IntPtr.Zero);
            float[] data = new float[model_input_shape().data_size()];
            Tensor input_tensor = new Tensor(model_input_shape(), data);
            infer_request.set_tensor(model_input_name(), input_tensor);
            infer_request.infer();
            input_tensor.Dispose();
            infer_request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void cancelTest()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            InferRequest infer_request = compiled_model.create_infer_request();
            Assert.IsTrue(infer_request.Ptr != IntPtr.Zero);
            float[] data = new float[model_input_shape().data_size()];
            Tensor input_tensor = new Tensor(model_input_shape(), data);
            infer_request.set_tensor(model_input_name(), input_tensor);
            infer_request.cancel();
            input_tensor.Dispose();
            infer_request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void start_asyncTest()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            InferRequest infer_request = compiled_model.create_infer_request();
            Assert.IsTrue(infer_request.Ptr != IntPtr.Zero);
            float[] data = new float[model_input_shape().data_size()];
            Tensor input_tensor = new Tensor(model_input_shape(), data);
            infer_request.set_tensor(model_input_name(), input_tensor);
            infer_request.start_async();
            infer_request.wait();

            Tensor tensor = infer_request.get_output_tensor();

            tensor.Dispose();
            input_tensor.Dispose();
            infer_request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void waitTest()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            InferRequest infer_request = compiled_model.create_infer_request();
            Assert.IsTrue(infer_request.Ptr != IntPtr.Zero);
            float[] data = new float[model_input_shape().data_size()];
            Tensor input_tensor = new Tensor(model_input_shape(), data);
            infer_request.set_tensor(model_input_name(), input_tensor);
            infer_request.start_async();
            infer_request.wait();

            Tensor tensor = infer_request.get_output_tensor();

            tensor.Dispose();
            input_tensor.Dispose();
            infer_request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void wait_forTest()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model);
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            InferRequest infer_request = compiled_model.create_infer_request();
            Assert.IsTrue(infer_request.Ptr != IntPtr.Zero);
            float[] data = new float[model_input_shape().data_size()];
            Tensor input_tensor = new Tensor(model_input_shape(), data);
            infer_request.set_tensor(model_input_name(), input_tensor);
            infer_request.start_async();
            infer_request.wait_for(1000000);

            Tensor tensor = infer_request.get_output_tensor();

            tensor.Dispose();
            input_tensor.Dispose();
            infer_request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_profiling_infoTest()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            CompiledModel compiled_model = core.compile_model(model,"CPU");
            Assert.IsTrue(compiled_model.Ptr != IntPtr.Zero);
            InferRequest infer_request = compiled_model.create_infer_request();
            Assert.IsTrue(infer_request.Ptr != IntPtr.Zero);
            float[] data = new float[model_input_shape().data_size()];
            Tensor input_tensor = new Tensor(model_input_shape(), data);
            infer_request.set_tensor(model_input_name(), input_tensor);
            infer_request.infer();
            Tensor tensor = infer_request.get_output_tensor();
            float[] data1 = tensor.get_data<float>((int)tensor.get_size());
            List<Ov.ProfilingInfo> pro = infer_request.get_profiling_info();
            Assert.IsTrue (pro.Count > 0);
            input_tensor.Dispose();
            infer_request.Dispose();
            compiled_model.Dispose();
            model.Dispose();
            core.Dispose();
        }
    }
}