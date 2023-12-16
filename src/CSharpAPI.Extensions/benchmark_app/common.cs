using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OpenVinoSharp.Extensions.benchmark_app
{
    public static class Common
    {
        static Random rd = new Random((int)DateTime.Now.Ticks);
        static T[] get_random_array<T>(int length) 
        {
            T[] result = new T[length];
            string t = typeof(T).ToString();
            if (t == "System.Byte")
            {
                byte[] tmp = new byte[length];
                byte min = byte.MinValue;
                byte max = byte.MaxValue;
                for (int i = 0; i < length; ++i) 
                {
                    tmp[i] = (byte)rd.Next(min, max);
                }
                result = (T[])Convert.ChangeType(tmp, typeof(T[]));
            }
            else if (t == "System.Int32")
            {
                int[] tmp = new int[length];
                int min = int.MinValue;
                int max = int.MaxValue;
                for (int i = 0; i < length; ++i)
                {
                    tmp[i] = (int)rd.Next(min, max);
                }
                result = (T[])Convert.ChangeType(tmp, typeof(T[]));
            }
            else if (t == "System.Int64")
            {
                long[] tmp = new long[length];
                long min = long.MinValue;
                long max = long.MaxValue;
                for (int i = 0; i < length; ++i)
                {
                    tmp[i] = (int)rd.Next((int)min, (int)max);
                }
                result = (T[])Convert.ChangeType(tmp, typeof(T[]));
            }
            else if (t == "System.Int16")
            {
                short[] tmp = new short[length];
                short min = short.MinValue;
                short max = short.MaxValue;
                for (int i = 0; i < length; ++i)
                {
                    tmp[i] = (short)rd.Next((int)min, (int)max);
                }
                result = (T[])Convert.ChangeType(tmp, typeof(T[]));
            }
            else if (t == "System.Single")
            {
                float[] tmp = new float[length];
                float min = float.MinValue;
                float max = float.MaxValue;
                for (int i = 0; i < length; ++i)
                {
                    tmp[i] = (float)rd.NextDouble() * (max - min) + min;
                }
                result = (T[])Convert.ChangeType(tmp, typeof(T[]));
            }
            else if (t == "System.Double")
            {
                double[] tmp = new double[length];
                double min = double.MinValue;
                double max = double.MaxValue;
                for (int i = 0; i < length; ++i)
                {
                    tmp[i] = (double)rd.NextDouble() * (max - min) + min;
                }
                result = (T[])Convert.ChangeType(tmp, typeof(T[]));
            }
            else
            {
                Console.WriteLine("Data format error, not supported. Only double, flaot, int, long, shaort and byte data formats are supported");
            }
            return result;
        }


        public static void fill_tensor_random(Tensor tensor) 
        {
            OvType type = tensor.get_element_type();
            ulong length = tensor.get_size();
            switch (type.get_type()) 
            {
                case ElementType.F64:
                    double[] tmp1 = get_random_array<double>((int)length);
                    tensor.set_data(tmp1);
                    break;
                case ElementType.F32:
                    float[] tmp2 = get_random_array<float>((int)length);
                    tensor.set_data(tmp2);
                    break;
                case ElementType.I64:
                    long[] tmp3 = get_random_array<long>((int)length);
                    tensor.set_data(tmp3);
                    break;
                case ElementType.I32:
                    int[] tmp4 = get_random_array<int>((int)length);
                    tensor.set_data(tmp4);
                    break;
                case ElementType.I16:
                    short[] tmp5 = get_random_array<short>((int)length);
                    tensor.set_data(tmp5);
                    break;
            }
        }

    }
}
