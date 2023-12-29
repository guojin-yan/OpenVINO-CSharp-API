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
    public class OutputTests : OVBaseTest
    {
        [TestMethod()]
        public void Output_test()
        {
        }

        [TestMethod()]
        public void Dispose_test()
        {
        }

        [TestMethod()]
        public void get_node_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Output input = model.output();
            Assert.IsTrue(input.get_node().Ptr != IntPtr.Zero);
            Node node = input.get_node();
            Assert.IsTrue(node.Ptr != IntPtr.Zero);
            input.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_index_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Output input = model.output();
            Assert.IsTrue(input.get_node().Ptr != IntPtr.Zero);
            ulong index = input.get_index();
            Assert.IsNotNull(index);
            input.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_element_type_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Output input = model.output();
            Assert.IsTrue(input.get_node().Ptr != IntPtr.Zero);
            OvType type = input.get_element_type();
            Assert.IsNotNull(type);
            input.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_shape_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Output input = model.output();
            Assert.IsTrue(input.get_node().Ptr != IntPtr.Zero);
            Shape shape = input.get_shape();
            Assert.IsNotNull(shape);
            input.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_any_name_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Output input = model.output();
            Assert.IsTrue(input.get_node().Ptr != IntPtr.Zero);
            string name = input.get_any_name();
            Assert.IsNotNull(name);
            input.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_partial_shape_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Output input = model.output();
            Assert.IsTrue(input.get_node().Ptr != IntPtr.Zero);
            PartialShape shape = input.get_partial_shape();
            Assert.IsNotNull(shape);
            input.Dispose();
            model.Dispose();
            core.Dispose();
        }
    }
}