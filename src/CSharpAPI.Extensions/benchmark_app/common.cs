using OpenVinoSharp.Extensions.utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.Extensions
{
    /// <summary>
    /// Public method class.
    /// </summary>
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

        /// <summary>
        /// Fill the tensor with random data.
        /// </summary>
        /// <param name="tensor">The model tensor.</param>
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

    class LatencyMetrics
    {
        public LatencyMetrics() { }

        public LatencyMetrics(List<double> latencies,
                    string data_shape = "",
                    int percentile_boundary = 50)      
        {
            percentile_boundary = percentile_boundary;
            data_shape = data_shape;
            fill_data(latencies, percentile_boundary);
        }

        public void write_to_slog()
        {
            string percentileStr = (percentile_boundary == 50)
                                ? "   Median:           "
                                : "   " + percentile_boundary + " percentile:     ";

            Slog.INFO(percentileStr + median_or_percentile.ToString("0.00") + " ms");
            Slog.INFO("   Average:          " + avg.ToString("0.00") + " ms");
            Slog.INFO("   Min:              " + min.ToString("0.00") + " ms");
            Slog.INFO("   Max:              " + max.ToString("0.00") + " ms");
        }

        double median_or_percentile = 0;
        double avg = 0;
        double min = 0;
        double max = 0;
        string data_shape;

        private void fill_data(List<double> latencies, int percentile_boundary)
        {
            if (latencies.Count == 0)
            {
                throw new ArgumentNullException("Latency metrics class expects non-empty vector of latencies at consturction.");
            }
            latencies.Sort();
            min = latencies.Min();
            avg = latencies.Sum() / (double)latencies.Count;
            median_or_percentile = latencies[(int)(latencies.Count/ 100.0 * percentile_boundary)];
            max = latencies.Max();
        }
        private int percentile_boundary = 50;
    };

}
