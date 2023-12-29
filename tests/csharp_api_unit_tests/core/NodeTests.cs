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
    public class NodeTests : OVBaseTest
    {
        [TestMethod()]
        public void Node_test()
        {
        }

        [TestMethod()]
        public void Dispose_test()
        {
        }

        [TestMethod()]
        public void get_shape_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Node node = model.get_const_input(model_input_name());
            Assert.IsTrue(node.Ptr != IntPtr.Zero);
            Shape shape = node.get_shape();
            Assert.IsNotNull(shape);
            node.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_partial_shape_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Node node = model.get_const_input(model_input_name());
            Assert.IsTrue(node.Ptr != IntPtr.Zero);
            PartialShape shape = node.get_partial_shape();
            Assert.IsNotNull(shape);
            node.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_name_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Node node = model.get_const_input(model_input_name());
            Assert.IsTrue(node.Ptr != IntPtr.Zero);
            string name = node.get_name();
            Assert.IsNotNull(name);
            node.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void get_element_type_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            Node node = model.get_const_input(model_input_name());
            Assert.IsTrue(node.Ptr != IntPtr.Zero);
            OvType type = node.get_element_type();
            Assert.IsNotNull(type);
            node.Dispose();
            model.Dispose();
            core.Dispose();
        }
    }
}