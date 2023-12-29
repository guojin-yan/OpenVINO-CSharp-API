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
    public class PartialShapeTests
    {
        [TestMethod()]
        public void PartialShape_test()
        {
        }

        [TestMethod()]
        public void PartialShape_test1()
        {
            Dimension[] dimensions = new Dimension[] { new Dimension(10), new Dimension(10), new Dimension(10) };
            PartialShape shape = new PartialShape(dimensions);
            Assert.IsNotNull(shape);
        }

        [TestMethod()]
        public void PartialShape_test2()
        {
            List<Dimension> dimensions = new List<Dimension> { new Dimension(10), new Dimension(10), new Dimension(10) };
            PartialShape shape = new PartialShape(dimensions);
            Assert.IsNotNull(shape);
        }

        [TestMethod()]
        public void PartialShape_test3()
        {
            Dimension rank = new Dimension(3);
            Dimension[] dimensions = new Dimension[] { new Dimension(10), new Dimension(10), new Dimension(10) };
            PartialShape shape = new PartialShape(rank, dimensions);
            Assert.IsNotNull(shape);
        }

        [TestMethod()]
        public void PartialShape_test4()
        {
            Dimension rank = new Dimension(3);
            List<Dimension> dimensions = new List<Dimension> { new Dimension(10), new Dimension(10), new Dimension(10) };
            PartialShape shape = new PartialShape(rank, dimensions);
            Assert.IsNotNull(shape);
        }

        [TestMethod()]
        public void PartialShape_test5()
        {
            long rank = 3;
            long[] dimensions = new long[] { 10, 10, 10 };
            PartialShape shape = new PartialShape(rank, dimensions);
            Assert.IsNotNull(shape);
        }

        [TestMethod()]
        public void PartialShape_test6()
        {
            long rank = 3;
            List<long> dimensions = new List<long> { 10, 10, 10 };
            PartialShape shape = new PartialShape(rank, dimensions);
            Assert.IsNotNull(shape);
        }

        [TestMethod()]
        public void PartialShape_test7()
        {
            Shape shape = new Shape(1,3,9);
            PartialShape shape1 = new PartialShape(shape);
            Assert.IsNotNull(shape1);
        }

        [TestMethod()]
        public void get_partial_shape_test()
        {
            Shape shape = new Shape(1, 3, 9);
            PartialShape shape1 = new PartialShape(shape);
            Assert.IsNotNull(shape1);
            Ov.ov_partial_shape ov_partial = shape1.get_partial_shape();
            Assert.IsNotNull(ov_partial);
        }

        [TestMethod()]
        public void get_rank_test()
        {
            long rank = 3;
            long[] dimensions = new long[] { 10, 10, 10 };
            PartialShape shape = new PartialShape(rank, dimensions);
            Assert.IsNotNull(shape);
            Dimension dimension =  shape.get_rank();
            Assert.IsNotNull(dimension);
        }

        [TestMethod()]
        public void get_dimensions_test()
        {
            long rank = 3;
            long[] dimensions = new long[] { 10, 10, 10 };
            PartialShape shape = new PartialShape(rank, dimensions);
            Assert.IsNotNull(shape);
            Dimension[] dimension = shape.get_dimensions();
            Assert.IsNotNull(dimension);
        }

        [TestMethod()]
        public void to_shape_test()
        {
            long rank = 3;
            long[] dimensions = new long[] { 10, 10, 10 };
            PartialShape shape = new PartialShape(rank, dimensions);
            Assert.IsNotNull(shape);
            Shape shape1 = shape.to_shape();
            Assert.IsNotNull(shape1);
        }

        [TestMethod()]
        public void is_static_test()
        {
            long rank = 3;
            long[] dimensions = new long[] { 10, 10, 10 };
            PartialShape shape = new PartialShape(rank, dimensions);
            Assert.IsNotNull(shape);
            shape.is_static();
        }

        [TestMethod()]
        public void is_dynamic_test()
        {
            long rank = 3;
            long[] dimensions = new long[] { 10, 10, 10 };
            PartialShape shape = new PartialShape(rank, dimensions);
            Assert.IsNotNull(shape);
            shape.is_dynamic();
        }

        [TestMethod()]
        public void to_string_test()
        {
            long rank = 3;
            long[] dimensions = new long[] { 10, 10, 10 };
            PartialShape shape = new PartialShape(rank, dimensions);
            Assert.IsNotNull(shape);
            string msg = shape.to_string();
            Assert.IsNotNull(msg);
        }
    }
}