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
    public class OvTests : OVBaseTest
    {
        [TestMethod()]
        public void get_openvino_version_test()
        {
            Version version = Ov.get_openvino_version();
            Assert.IsNotNull(version);
        }

        [TestMethod()]
        public void content_from_file_test()
        {
            byte[] data = Ov.content_from_file(get_model_bin_file_name());
            Assert.IsTrue(data.Length>0);
        }
    }
}