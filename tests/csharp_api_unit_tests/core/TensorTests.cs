using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenVinoSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.Tests
{
    [TestClass()]
    public class TensorTests
    {
        [TestMethod()]
        public void Tensor_test()
        {
        }

        [TestMethod()]
        public void Tensor_test1()
        {
        }

        [TestMethod()]
        public void Tensor_test2()
        {
            Shape shape = new Shape(1, 2, 3);
            float[] data = new float[6];
            Tensor tensor = new Tensor(shape, data);
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            tensor.Dispose();
        }

        [TestMethod()]
        public void Tensor_test3()
        {
            Shape shape = new Shape(1, 2, 3);
            double[] data = new double[6];
            Tensor tensor = new Tensor(shape, data);
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            tensor.Dispose();
        }

        [TestMethod()]
        public void Tensor_test4()
        {
            Shape shape = new Shape(1, 2, 3);
            int[] data = new int[6];
            Tensor tensor = new Tensor(shape, data);
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            tensor.Dispose();
        }

        [TestMethod()]
        public void Tensor_test5()
        {
            Shape shape = new Shape(1, 2, 3);
            short[] data = new short[6];
            Tensor tensor = new Tensor(shape, data);
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            tensor.Dispose();
        }

        [TestMethod()]
        public void Tensor_test6()
        {
            Shape shape = new Shape(1, 2, 3);
            long[] data = new long[6];
            Tensor tensor = new Tensor(shape, data);
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            tensor.Dispose();
        }

        [TestMethod()]
        public void Tensor_test7()
        {
            Shape shape = new Shape(1, 2, 3);
            float[] data = new float[6];
            Tensor tensor = new Tensor(new element.Type(ElementType.F32), shape, Marshal.UnsafeAddrOfPinnedArrayElement<float>(data, 0));
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            tensor.Dispose();
        }

        [TestMethod()]
        public void Tensor_test8()
        {
            Shape shape = new Shape(1, 2, 3);
            Tensor tensor = new Tensor(new element.Type(ElementType.F32), shape);
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            tensor.Dispose();
        }

        [TestMethod()]
        public void Tensor_test9()
        {
            Shape shape = new Shape(1, 2, 3);
            float[] data = new float[6];
            Tensor tensor = new Tensor(shape, data);
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            Tensor tensor1 = new Tensor(tensor);
            Assert.IsTrue(tensor1.Ptr != IntPtr.Zero);
            tensor.Dispose();
            tensor1.Dispose();
        }

        [TestMethod()]
        public void Dispose_test()
        {
        }

        [TestMethod()]
        public void set_shape_test()
        {
            Shape shape = new Shape(1, 2, 80);
            float[] data = new float[6];
            Tensor tensor = new Tensor(shape, data);
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            Shape new_shape = new Shape(1, 2, 15);
            tensor.set_shape(new_shape);
            tensor.Dispose();
        }

        [TestMethod()]
        public void get_shape_test()
        {
            Shape shape = new Shape(1, 2, 3);
            float[] data = new float[6];
            Tensor tensor = new Tensor(shape, data);
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            Shape new_shape = tensor.get_shape();
            Assert.IsTrue(shape.Count == new_shape.Count);
            tensor.Dispose();
        }

        [TestMethod()]
        public void get_element_type_test()
        {
            Shape shape = new Shape(1, 2, 3);
            float[] data = new float[6];
            Tensor tensor = new Tensor(shape, data);
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            OvType type = tensor.get_element_type();
            Assert.IsTrue((int)type.get_type()!=100);
            tensor.Dispose();
        }

        [TestMethod()]
        public void get_size_test()
        {
            Shape shape = new Shape(1, 2, 3);
            float[] data = new float[6];
            Tensor tensor = new Tensor(shape, data);
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            ulong size = tensor.get_size();
            Assert.IsTrue(size > 0);
            tensor.Dispose();
        }

        [TestMethod()]
        public void get_byte_size_test()
        {
            Shape shape = new Shape(1, 2, 3);
            float[] data = new float[6];
            Tensor tensor = new Tensor(shape, data);
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            ulong size = tensor.get_byte_size();
            Assert.IsTrue(size > 0);
            tensor.Dispose();
        }

        [TestMethod()]
        public void copy_to_test()
        {
            Shape shape = new Shape(1, 2, 3);
            float[] data = new float[6];
            data[0] = 0.6f;
            Tensor tensor = new Tensor(shape, data);
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            Tensor new_tensor = new Tensor(shape, data);
            tensor.copy_to<float>(new_tensor);
            float[] new_data = new_tensor.get_data<float>((int)new_tensor.get_size());
            Assert.IsTrue(new_data[0] == 0.6f);
            new_tensor.Dispose();
            tensor.Dispose();
        }

        [TestMethod()]
        public void data_test()
        {
            Shape shape = new Shape(1, 2, 3);
            float[] data = new float[6];
            data[0] = 0.6f;
            Tensor tensor = new Tensor(shape, data);
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            IntPtr ptr = tensor.data();
            Assert.IsTrue(ptr != IntPtr.Zero);
            tensor.Dispose();
        }

        [TestMethod()]
        public void set_data_test()
        {
            Shape shape = new Shape(1, 2, 3);
            float[] data = new float[6];
            data[0] = 0.6f;
            Tensor tensor = new Tensor(new OvType(ElementType.F32), shape);
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            tensor.set_data(data);
            float[] new_data = tensor.get_data<float>((int)tensor.get_size());
            Assert.IsTrue(new_data[0] == 0.6f);
            tensor.Dispose();
        }

        [TestMethod()]
        public void get_data_test()
        {
            Shape shape = new Shape(1, 2, 3);
            float[] data = new float[6];
            data[0] = 0.6f;
            Tensor tensor = new Tensor(shape, data);
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            float[] new_data = tensor.get_data<float>((int)tensor.get_size());
            Assert.IsTrue(new_data[0] == 0.6f);
            tensor.Dispose();
        }

        [TestMethod()]
        public void Tensor_test10()
        {
            Shape shape = new Shape(1, 2, 3);
            float[] data = new float[6];
            Tensor tensor = new Tensor(new OvType(ElementType.F32), shape, Marshal.UnsafeAddrOfPinnedArrayElement<float>(data, 0));
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            tensor.Dispose();
        }

        [TestMethod()]
        public void Tensor_test11()
        {
            Shape shape = new Shape(1, 2, 3);
            float[] data = new float[6];
            Tensor tensor = new Tensor(new OvType(ElementType.F32), shape);
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            tensor.Dispose();
        }

        [TestMethod()]
        public void Tensor_test12()
        {
            Shape shape = new Shape(1, 2, 3);
            byte[] data = new byte[6];
            Tensor tensor = new Tensor(new OvType(ElementType.F32), shape, data);
            Assert.IsTrue(tensor.Ptr != IntPtr.Zero);
            tensor.Dispose();
        }
    }
}