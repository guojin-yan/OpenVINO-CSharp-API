using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.Tests
{
    public class OVBaseTest
    {
        private string model_xml = "..\\..\\..\\..\\..\\tests\\test_data\\model\\yolov8\\yolov8s.xml";
        private string model_bin = "..\\..\\..\\..\\..\\tests\\test_data\\model\\yolov8\\yolov8s.bin";

        private string device = "CPU";
        public string get_model_xml_file_name() 
        {
            if (!File.Exists(model_xml)) 
            {
                Assert.Fail();
            }
            return model_xml;
        }
        public string get_model_bin_file_name()
        {
            if (!File.Exists(model_bin))
            {
                Assert.Fail();
            }
            return model_bin;
        }
        public string get_device() 
        {
            return device;
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
