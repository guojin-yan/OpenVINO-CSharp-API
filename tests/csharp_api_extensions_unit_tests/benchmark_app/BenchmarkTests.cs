using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenVinoSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.Extensions.Tests
{
    [TestClass()]
    public class BenchmarkTests
    {
        [TestMethod()]
        public void sync_benchmark_test()
        {
            Benchmark.sync_benchmark("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s-cls.xml");
        }
    }
}