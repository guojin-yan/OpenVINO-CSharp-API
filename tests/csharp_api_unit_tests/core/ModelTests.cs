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
    public class ModelTests
    {
        [TestMethod()]
        public void Model_test()
        {
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void Dispose_test()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            model.Dispose();
            Assert.IsTrue(model.Ptr == IntPtr.Zero);
            core.Dispose();
        }

        [TestMethod()]
        public void get_friendly_name_test()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            string name = model.get_friendly_name();
            Assert.IsTrue(name != "");
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_input_test()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Node node = model.get_input();
            Assert.IsTrue(node.Ptr != IntPtr.Zero);
            node.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_input_test1()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Node node = model.get_input(0);
            Assert.IsTrue(node.Ptr != IntPtr.Zero);
            node.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_input_test2()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Node node = model.get_input("images");
            Assert.IsTrue(node.Ptr != IntPtr.Zero);
            node.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_output_test()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Node node = model.get_output();
            Assert.IsTrue(node.Ptr != IntPtr.Zero);
            node.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_output_test1()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Node node = model.get_output(0);
            Assert.IsTrue(node.Ptr != IntPtr.Zero);
            node.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_output_test2()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Node node = model.get_output("output0");
            Assert.IsTrue(node.Ptr != IntPtr.Zero);
            node.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_const_input_test()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Node node = model.get_const_input();
            Assert.IsTrue(node.Ptr != IntPtr.Zero);
            node.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_const_input_test1()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Node node = model.get_const_input(0);
            Assert.IsTrue(node.Ptr != IntPtr.Zero);
            node.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_const_input_test2()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Node node = model.get_const_input("images");
            Assert.IsTrue(node.Ptr != IntPtr.Zero);
            node.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_const_output_test()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Node node = model.get_const_output();
            Assert.IsTrue(node.Ptr != IntPtr.Zero);
            node.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_const_output_test1()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            model.Dispose();
            model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml",
                "..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.bin");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Node node = model.get_const_output(0);
            Assert.IsTrue(node.Ptr != IntPtr.Zero);
            node.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_const_output_test2()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Node node = model.get_const_output("output0");
            Assert.IsTrue(node.Ptr != IntPtr.Zero);
            node.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void input_test()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Input input = model.input();
            Assert.IsTrue(input.get_node().Ptr != IntPtr.Zero);
            input.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void input_test1()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Input input = model.input(0);
            Assert.IsTrue(input.get_node().Ptr != IntPtr.Zero);
            input.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void input_test2()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Input input = model.input("images");
            Assert.IsTrue(input.get_node().Ptr != IntPtr.Zero);
            input.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void const_input_test()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Input input = model.const_input();
            Assert.IsTrue(input.get_node().Ptr != IntPtr.Zero);
            input.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void const_input_test1()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Input input = model.const_input(0);
            Assert.IsTrue(input.get_node().Ptr != IntPtr.Zero);
            input.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void const_input_test2()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Input input = model.const_input("images");
            Assert.IsTrue(input.get_node().Ptr != IntPtr.Zero);
            input.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void output_test()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Output output = model.output();
            Assert.IsTrue(output.get_node().Ptr != IntPtr.Zero);
            output.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void output_test1()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Output output = model.output(0);
            Assert.IsTrue(output.get_node().Ptr != IntPtr.Zero);
            output.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void output_test2()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Output output = model.output("output0");
            Assert.IsTrue(output.get_node().Ptr != IntPtr.Zero);
            output.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void const_output_test()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Output output = model.const_output();
            Assert.IsTrue(output.get_node().Ptr != IntPtr.Zero);
            output.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void const_output_test1()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Output output = model.const_output(0);
            Assert.IsTrue(output.get_node().Ptr != IntPtr.Zero);
            output.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void const_output_test2()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Output output = model.const_output("output0");
            Assert.IsTrue(output.get_node().Ptr != IntPtr.Zero);
            output.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_inputs_size_test()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            ulong size = model.get_inputs_size();
            Assert.IsTrue(size > 0);
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_outputs_size_test()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            ulong size = model.get_outputs_size();
            Assert.IsTrue(size > 0);
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void inputs_test()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            ulong size = model.get_inputs_size();
            Assert.IsTrue(size > 0);
            List<Input> inputs = model.inputs();
            Assert.IsTrue(inputs.Count == (int)size);
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void outputs_test()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            ulong size = model.get_outputs_size();
            Assert.IsTrue(size > 0);
            List<Output> outputs = model.outputs();
            Assert.IsTrue(outputs.Count == (int)size);
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void const_inputs_test()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            ulong size = model.get_inputs_size();
            Assert.IsTrue(size > 0);
            List<Input> inputs = model.const_inputs();
            Assert.IsTrue(inputs.Count == (int)size);
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void const_outputs_test()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
 
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            ulong size = model.get_outputs_size();
            Assert.IsTrue(size > 0);
            List<Output> outputs = model.const_outputs();
            Assert.IsTrue(outputs.Count == (int)size);
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void is_dynamic_test()
        {
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            model.Dispose();
            model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml",
                "..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.bin");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            bool flag = model.is_dynamic();
            Assert.IsTrue(!flag);
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void reshape_test()
        {
            Shape shape = new Shape(new long[4] { 1, 3, 640, 640 });

            PartialShape partial = new PartialShape(shape);

            Dictionary<string, PartialShape> pairs = new Dictionary<string, PartialShape>();

            Assert.IsTrue(partial.get_partial_shape().rank.max == 4);
            pairs.Add("images", partial);

            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            model.reshape(pairs);

            PartialShape shape1 = model.get_input().get_partial_shape();

            model.Dispose();
            core.Dispose();

        }

        [TestMethod()]
        public void reshape_test1()
        {
            Shape shape = new Shape(new long[4] { 1, 3, 640, 640 });
            PartialShape partial = new PartialShape(shape);
            Assert.IsTrue(partial.get_partial_shape().rank.max == 4);
            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            model.reshape(partial);
            PartialShape shape1 = model.get_input().get_partial_shape();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void reshape_test2()
        {
            Shape shape = new Shape(new long[4] { 1, 3, 640, 640 });

            PartialShape partial = new PartialShape(shape);

            Dictionary<ulong, PartialShape> pairs = new Dictionary<ulong, PartialShape>();

            Assert.IsTrue(partial.get_partial_shape().rank.max == 4);
            pairs.Add(0, partial);

            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            model.reshape(pairs);

            PartialShape shape1 = model.get_input().get_partial_shape();

            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void reshape_test3()
        {
            Shape shape = new Shape(new long[4] { 1, 3, 640, 640 });

            PartialShape partial = new PartialShape(shape);

            Dictionary<Node, PartialShape> pairs = new Dictionary<Node, PartialShape>();

            Assert.IsTrue(partial.get_partial_shape().rank.max == 4);
      

            var core = new Core();
            Model model = core.read_model("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Assert.IsTrue(model.Ptr != IntPtr.Zero);

            Node input = model.get_input();
            pairs.Add(input, partial);

            model.reshape(pairs);

            PartialShape shape1 = model.get_input().get_partial_shape();

            model.Dispose();
            core.Dispose();
        }
    }
}