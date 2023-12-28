using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.Tests
{
    public class OVBaseTest
    {
        public class TestModelInfo
        {
            public string model_xml = "..\\..\\..\\..\\..\\tests\\test_data\\model\\yolov8\\yolov8s.xml";
            public string model_bin = "..\\..\\..\\..\\..\\tests\\test_data\\model\\yolov8\\yolov8s.bin";
            public string input_name = "images";
            public string output_name = "output0";

            public OvType input_type = new OvType(ElementType.F16);
            public Shape shape = new Shape(new long[] { 1, 3, 640, 640 });


        }
        TestModelInfo model_info = new TestModelInfo();

        private string device = "CPU";
        public string get_model_xml_file_name() 
        {
            if (!File.Exists(model_info.model_xml)) 
            {
                Assert.Fail();
            }
            return model_info.model_xml;
        }
        public string get_model_bin_file_name()
        {
            if (!File.Exists(model_info.model_bin))
            {
                Assert.Fail();
            }
            return model_info.model_bin;
        }
        public string get_device() 
        {
            return device;
        }

        public string model_input_name() 
        {
            return model_info.input_name;
        }
        public string model_output_name()
        {
            return model_info.output_name;
        }

        public byte[] content_from_file(string file)
        {
            FileStream fs = new FileStream(get_model_bin_file_name(), FileMode.Open, FileAccess.Read);

            long len = fs.Seek(0, SeekOrigin.End);


            fs.Seek(0, SeekOrigin.Begin);

            byte[] data = new byte[len + 1];

            fs.Read(data, 0, (int)len);
            return data;
        }
    }
}
