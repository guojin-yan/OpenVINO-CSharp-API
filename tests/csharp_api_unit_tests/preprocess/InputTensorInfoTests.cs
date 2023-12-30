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
    public class InputTensorInfoTests : OVBaseTest
    {
        [TestMethod()]
        public void InputTensorInfo_test()
        {
        }

        [TestMethod()]
        public void Dispose_test()
        {
        }

        [TestMethod()]
        public void set_color_format_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);
            Assert.IsTrue(processor.Ptr != IntPtr.Zero);
            InputInfo input = processor.input();
            Assert.IsTrue(input.Ptr != IntPtr.Zero);
            InputTensorInfo input_tensor = input.tensor();
            Assert.IsTrue(input_tensor.Ptr != IntPtr.Zero);
            input_tensor.set_color_format(ColorFormat.NV12_SINGLE_PLANE);
            input_tensor.set_color_format(ColorFormat.NV12_TWO_PLANES, "y", "uv");
            input_tensor.Dispose();
            input.Dispose();
            processor.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void set_element_type_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);
            Assert.IsTrue(processor.Ptr != IntPtr.Zero);
            InputInfo input = processor.input();
            Assert.IsTrue(input.Ptr != IntPtr.Zero);
            InputTensorInfo input_tensor = input.tensor();
            Assert.IsTrue(input_tensor.Ptr != IntPtr.Zero);
            input_tensor.set_element_type(new OvType(ElementType.F32));
            input_tensor.Dispose();
            input.Dispose();
            processor.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void set_spatial_static_shape_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);
            Assert.IsTrue(processor.Ptr != IntPtr.Zero);
            InputInfo input = processor.input();
            Assert.IsTrue(input.Ptr != IntPtr.Zero);
            InputTensorInfo input_tensor = input.tensor();
            Assert.IsTrue(input_tensor.Ptr != IntPtr.Zero);
            input_tensor.set_spatial_static_shape(100, 100);
            input_tensor.Dispose();
            input.Dispose();
            processor.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void set_memory_type_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);
            Assert.IsTrue(processor.Ptr != IntPtr.Zero);
            InputInfo input = processor.input();
            Assert.IsTrue(input.Ptr != IntPtr.Zero);
            InputTensorInfo input_tensor = input.tensor();
            Assert.IsTrue(input_tensor.Ptr != IntPtr.Zero);
            input_tensor.set_memory_type("GPU_SURFACE");
            input_tensor.Dispose();
            input.Dispose();
            processor.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void set_layout_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);
            Assert.IsTrue(processor.Ptr != IntPtr.Zero);
            InputInfo input = processor.input();
            Assert.IsTrue(input.Ptr != IntPtr.Zero);
            InputTensorInfo input_tensor = input.tensor();
            Assert.IsTrue(input_tensor.Ptr != IntPtr.Zero);
            input_tensor.set_layout(new Layout("NCHW"));
            input_tensor.Dispose();
            input.Dispose();
            processor.Dispose();
            model.Dispose();
            core.Dispose();
        }

        [TestMethod()]
        public void set_from_test()
        {
            var core = new Core();
            Model model = core.read_model(get_model_xml_file_name());
            Assert.IsTrue(model.Ptr != IntPtr.Zero);
            PrePostProcessor processor = new PrePostProcessor(model);
            Assert.IsTrue(processor.Ptr != IntPtr.Zero);
            InputInfo input = processor.input();
            Assert.IsTrue(input.Ptr != IntPtr.Zero);
            InputTensorInfo input_tensor = input.tensor();
            Assert.IsTrue(input_tensor.Ptr != IntPtr.Zero);

            Shape shape = new Shape(1, 2, 3);
            Tensor tensor = new Tensor(new element.Type(ElementType.F32), shape);

            input_tensor.set_from(tensor);
            input_tensor.Dispose();
            input.Dispose();
            processor.Dispose();
            model.Dispose();
            core.Dispose();
        }
    }
}