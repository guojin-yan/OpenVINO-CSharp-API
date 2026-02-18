// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using OpenVinoSharp.Internal;

namespace OpenVinoSharp.extensions.benchmark_app
{
    /// <summary>
    /// 基准测试公共方法类 / Benchmark common methods class
    /// </summary>
    public static class Common
    {
        static Random rd = new Random((int)DateTime.Now.Ticks);

        static T[] get_random_array<T>(int length)
        {
            Logger.Debug($"get_random_array<T>: type={typeof(T).Name}, length={length}");
            
            T[] result = new T[length];
            string t = typeof(T).ToString();
            if (t == "System.Byte")
            {
                Logger.Debug("Generating random byte array");
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
                Logger.Debug("Generating random int32 array");
                int[] tmp = new int[length];
                int min = int.MinValue;
                int max = int.MaxValue;
                for (int i = 0; i < length; ++i)
                {
                    tmp[i] = rd.Next(min, max);
                }
                result = (T[])Convert.ChangeType(tmp, typeof(T[]));
            }
            else if (t == "System.Int64")
            {
                Logger.Debug("Generating random int64 array");
                long[] tmp = new long[length];
                long min = long.MinValue;
                long max = long.MaxValue;
                for (int i = 0; i < length; ++i)
                {
                    tmp[i] = rd.Next((int)min, (int)max);
                }
                result = (T[])Convert.ChangeType(tmp, typeof(T[]));
            }
            else if (t == "System.Int16")
            {
                Logger.Debug("Generating random int16 array");
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
                Logger.Debug("Generating random float array");
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
                Logger.Debug("Generating random double array");
                double[] tmp = new double[length];
                double min = double.MinValue;
                double max = double.MaxValue;
                for (int i = 0; i < length; ++i)
                {
                    tmp[i] = rd.NextDouble() * (max - min) + min;
                }
                result = (T[])Convert.ChangeType(tmp, typeof(T[]));
            }
            else
            {
                Logger.Error("数据格式错误，不支持。仅支持 double、float、int、long、short 和 byte 数据格式 / Data format error, not supported. Only double, float, int, long, short and byte data formats are supported");
                Logger.Debug($"Unsupported data type: {t}");
            }
            return result;
        }

        /// <summary>
        /// 用随机数据填充张量 / Fill the tensor with random data
        /// </summary>
        /// <param name="tensor">模型张量 / The model tensor</param>
        public static void fill_tensor_random(Tensor tensor)
        {
            ElementType type = tensor.element_type;
            ulong length = tensor.size;
            
            Logger.Debug($"fill_tensor_random: element_type={type}, size={length}");
            Logger.Debug($"Tensor shape: [{string.Join(",", tensor.shape)}]");
            
            switch (type)
            {
                case ElementType.F64:
                    Logger.Debug("Filling tensor with double (F64) random data");
                    double[] tmp1 = get_random_array<double>((int)length);
                    tensor.set_data(tmp1);
                    Logger.Debug("F64 tensor data filled successfully");
                    break;
                case ElementType.F32:
                    Logger.Debug("Filling tensor with float (F32) random data");
                    float[] tmp2 = get_random_array<float>((int)length);
                    tensor.set_data(tmp2);
                    Logger.Debug("F32 tensor data filled successfully");
                    break;
                case ElementType.I64:
                    Logger.Debug("Filling tensor with long (I64) random data");
                    long[] tmp3 = get_random_array<long>((int)length);
                    tensor.set_data(tmp3);
                    Logger.Debug("I64 tensor data filled successfully");
                    break;
                case ElementType.I32:
                    Logger.Debug("Filling tensor with int (I32) random data");
                    int[] tmp4 = get_random_array<int>((int)length);
                    tensor.set_data(tmp4);
                    Logger.Debug("I32 tensor data filled successfully");
                    break;
                case ElementType.I16:
                    Logger.Debug("Filling tensor with short (I16) random data");
                    short[] tmp5 = get_random_array<short>((int)length);
                    tensor.set_data(tmp5);
                    Logger.Debug("I16 tensor data filled successfully");
                    break;
                default:
                    Logger.Warn($"Unsupported element type for random fill: {type}");
                    Logger.Debug("Skipping tensor data fill for unsupported type");
                    break;
            }
        }
    }

    /// <summary>
    /// 延迟指标类 / Latency metrics class
    /// </summary>
    class LatencyMetrics
    {
        public LatencyMetrics() { }

        public LatencyMetrics(List<double> latencies, string data_shape = "", int percentile_boundary = 50)
        {
            Logger.Debug($"LatencyMetrics constructor: count={latencies?.Count}, percentile={percentile_boundary}");
            this.percentile_boundary = percentile_boundary;
            this.data_shape = data_shape;
            fill_data(latencies, percentile_boundary);
        }

        /// <summary>
        /// 将指标输出到日志 / Write metrics to log
        /// </summary>
        public void write_to_slog()
        {
            Logger.Debug("Writing latency metrics to log");
            
            string percentileStr = (percentile_boundary == 50)
                                ? "   中位数 / Median:           "
                                : "   " + percentile_boundary + " 百分位 / percentile:     ";

            Logger.Info(percentileStr + median_or_percentile.ToString("0.00") + " ms");
            Logger.Info("   平均值 / Average:          " + avg.ToString("0.00") + " ms");
            Logger.Info("   最小值 / Min:              " + min.ToString("0.00") + " ms");
            Logger.Info("   最大值 / Max:              " + max.ToString("0.00") + " ms");
            
            Logger.Debug($"Latency metrics - Median/P{percentile_boundary}: {median_or_percentile:F3}ms, Avg: {avg:F3}ms, Min: {min:F3}ms, Max: {max:F3}ms");
        }

        double median_or_percentile = 0;
        double avg = 0;
        double min = 0;
        double max = 0;
        string data_shape;

        private void fill_data(List<double> latencies, int percentile_boundary)
        {
            Logger.Debug($"fill_data: latencies.Count={latencies?.Count}, percentile={percentile_boundary}");
            
            if (latencies == null || latencies.Count == 0)
            {
                Logger.Debug("ERROR: Empty latencies list provided");
                throw new ArgumentNullException("延迟指标类期望在构造时传入非空的延迟向量 / Latency metrics class expects non-empty vector of latencies at construction.");
            }
            
            Logger.Debug("Sorting latencies");
            latencies.Sort();
            
            min = latencies.Min();
            avg = latencies.Sum() / (double)latencies.Count;
            median_or_percentile = latencies[(int)(latencies.Count / 100.0 * percentile_boundary)];
            max = latencies.Max();
            
            Logger.Debug($"Calculated metrics - Min: {min:F3}, Avg: {avg:F3}, P{percentile_boundary}: {median_or_percentile:F3}, Max: {max:F3}");
        }

        private int percentile_boundary = 50;
    }
}
