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
    public class ShapeTests
    {
        [TestMethod()]
        public void Shape_test()
        {
        }

        [TestMethod()]
        public void Shape_test1()
        {
            List<long> data = new List<long>() { 1, 2, 3 };
            Shape shape = new Shape(data);
            shape.Dispose();
        }

        [TestMethod()]
        public void Shape_test2()
        {
            long[] data = new long[] { 1, 2, 3 };
            Shape shape = new Shape(data);
            shape.Dispose();
        }

        [TestMethod()]
        public void Shape_test3()
        {
            Shape shape = new Shape(1,2,9);
            shape.Dispose();
        }

        [TestMethod()]
        public void Dispose_test()
        {
            Shape shape = new Shape(1, 2, 9);
            shape.Dispose();
        }

        [TestMethod()]
        public void to_string_test()
        {
            Shape shape = new Shape(1, 2, 9);
            string msg = shape.to_string();
            Assert.IsNotNull(msg);
            shape.Dispose();
        }

        [TestMethod()]
        public void data_size_test()
        {
            Shape shape = new Shape(1, 2, 9);
            long size = shape.data_size();
            Assert.IsTrue(size == 18);
            shape.Dispose();
        }
    }
}