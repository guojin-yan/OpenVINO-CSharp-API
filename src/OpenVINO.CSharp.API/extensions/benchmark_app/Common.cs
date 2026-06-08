//  ========================================================================
//  【项目名称】OpenVINO C# API
//  【项目描述】OpenVINO™ 的 C# 语言绑定库，提供高性能深度学习推理能力
//  【版权声明】© 2026-2025 Guojin Yan. All Rights Reserved.
//  【开源协议】Apache-2.0 License（请遵守许可证条款）
//  -----------------------------------------------------------------------
//  【功能简介】
//  1. 完整的 OpenVINO™ C API 封装，提供 C# 友好的面向对象接口。
//  2. 支持模型加载、编译、推理全流程操作。
//  3. 支持 CPU、GPU、VPU 等多种推理设备。
//  4. 支持同步推理和异步推理模式。
//  5. 支持预处理和后处理流水线配置。
//  6. 支持动态形状和批量推理。
//  7. 支持模型缓存和性能分析。
//  8. 支持远程上下文（Remote Context）和零拷贝推理。
//  9. 支持 .NET Framework 4.6.1+、.NET Core 2.0+、.NET 5/6/7/8/9+。
//  10. 提供推理请求对象池，优化高并发场景性能。
//  11. 提供完善的异常处理和日志记录机制。
//  12. 提供丰富的单元测试和集成测试用例。
//  -----------------------------------------------------------------------
//  【官方资源】
//  📌 GitHub仓库：https://github.com/guojin-yan/OpenVINO-CSharp-API
//  📌 NuGet包：https://www.nuget.org/packages/OpenVINO.CSharp.API
//  📌 在线文档：https://guojin-yan.github.io/OpenVINO-CSharp-API/index.html
//  📌 示例代码：https://github.com/guojin-yan/OpenVINO-CSharp-API/tree/csharp3.3/samples
//  -----------------------------------------------------------------------
//  【社区支持】
//  💬 QQ交流群：945057948（加入获取技术支持）
//  📱 微信公众号：CSharp与边缘模型部署（教程+案例）
//  📝 CSDN博客：https://guojin.blog.csdn.net（技术文章）
//  -----------------------------------------------------------------------
//  【联系我们】
//  ✉ 项目维护：guojin_yjs@cumt.edu.cn
//  💬 微信咨询：15253793309
//  🐛 Bug反馈：https://github.com/guojin-yan/OpenVINO-CSharp-API/issues
//  💡 功能建议：https://github.com/guojin-yan/OpenVINO-CSharp-API/discussions/landing
//  -----------------------------------------------------------------------
//  【致谢】
//  本项目基于 Intel® OpenVINO™ 工具包开发，感谢 Intel 提供的优秀开源项目。
//  OpenVINO™ 是 Intel Corporation 的商标。
//  ========================================================================
//  
//  【许可声明】
//  1. 本项目采用 Apache-2.0 License 开源协议，允许自由使用、修改和分发。
//  2. 使用本项目即表示您同意 Apache-2.0 License 许可证的所有条款。
//  3. 本项目按"原样"提供，不提供任何形式的担保。
//  4. 使用本项目产生的任何风险由使用者自行承担。
//  5. 修改或分发时请保留原始版权声明和许可声明。
//  ========================================================================
//

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
            OvLogger.Debug($"get_random_array<T>: type={typeof(T).Name}, length={length}");
            
            T[] result = new T[length];
            string t = typeof(T).ToString();
            if (t == "System.Byte")
            {
                OvLogger.Debug("Generating random byte array");
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
                OvLogger.Debug("Generating random int32 array");
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
                OvLogger.Debug("Generating random int64 array");
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
                OvLogger.Debug("Generating random int16 array");
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
                OvLogger.Debug("Generating random float array");
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
                OvLogger.Debug("Generating random double array");
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
                OvLogger.Error("数据格式错误，不支持。仅支持 double、float、int、long、short 和 byte 数据格式 / Data format error, not supported. Only double, float, int, long, short and byte data formats are supported");
                OvLogger.Debug($"Unsupported data type: {t}");
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
            
            OvLogger.Debug($"fill_tensor_random: element_type={type}, size={length}");
            OvLogger.Debug($"Tensor shape: [{string.Join(",", tensor.shape)}]");
            
            switch (type)
            {
                case ElementType.F64:
                    OvLogger.Debug("Filling tensor with double (F64) random data");
                    double[] tmp1 = get_random_array<double>((int)length);
                    tensor.set_data(tmp1);
                    OvLogger.Debug("F64 tensor data filled successfully");
                    break;
                case ElementType.F32:
                    OvLogger.Debug("Filling tensor with float (F32) random data");
                    float[] tmp2 = get_random_array<float>((int)length);
                    tensor.set_data(tmp2);
                    OvLogger.Debug("F32 tensor data filled successfully");
                    break;
                case ElementType.I64:
                    OvLogger.Debug("Filling tensor with long (I64) random data");
                    long[] tmp3 = get_random_array<long>((int)length);
                    tensor.set_data(tmp3);
                    OvLogger.Debug("I64 tensor data filled successfully");
                    break;
                case ElementType.I32:
                    OvLogger.Debug("Filling tensor with int (I32) random data");
                    int[] tmp4 = get_random_array<int>((int)length);
                    tensor.set_data(tmp4);
                    OvLogger.Debug("I32 tensor data filled successfully");
                    break;
                case ElementType.I16:
                    OvLogger.Debug("Filling tensor with short (I16) random data");
                    short[] tmp5 = get_random_array<short>((int)length);
                    tensor.set_data(tmp5);
                    OvLogger.Debug("I16 tensor data filled successfully");
                    break;
                default:
                    OvLogger.Warn($"Unsupported element type for random fill: {type}");
                    OvLogger.Debug("Skipping tensor data fill for unsupported type");
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
            OvLogger.Debug($"LatencyMetrics constructor: count={latencies?.Count}, percentile={percentile_boundary}");
            this.percentile_boundary = percentile_boundary;
            this.data_shape = data_shape;
            fill_data(latencies, percentile_boundary);
        }

        /// <summary>
        /// 将指标输出到日志 / Write metrics to log
        /// </summary>
        public void write_to_slog()
        {
            OvLogger.Debug("Writing latency metrics to log");
            
            string percentileStr = (percentile_boundary == 50)
                                ? "   中位数 / Median:           "
                                : "   " + percentile_boundary + " 百分位 / percentile:     ";

            OvLogger.Info(percentileStr + median_or_percentile.ToString("0.00") + " ms");
            OvLogger.Info("   平均值 / Average:          " + avg.ToString("0.00") + " ms");
            OvLogger.Info("   最小值 / Min:              " + min.ToString("0.00") + " ms");
            OvLogger.Info("   最大值 / Max:              " + max.ToString("0.00") + " ms");
            
            OvLogger.Debug($"Latency metrics - Median/P{percentile_boundary}: {median_or_percentile:F3}ms, Avg: {avg:F3}ms, Min: {min:F3}ms, Max: {max:F3}ms");
        }

        double median_or_percentile = 0;
        double avg = 0;
        double min = 0;
        double max = 0;
        string data_shape;

        private void fill_data(List<double> latencies, int percentile_boundary)
        {
            OvLogger.Debug($"fill_data: latencies.Count={latencies?.Count}, percentile={percentile_boundary}");
            
            if (latencies == null || latencies.Count == 0)
            {
                OvLogger.Debug("ERROR: Empty latencies list provided");
                throw new ArgumentNullException("延迟指标类期望在构造时传入非空的延迟向量 / Latency metrics class expects non-empty vector of latencies at construction.");
            }
            
            OvLogger.Debug("Sorting latencies");
            latencies.Sort();
            
            min = latencies.Min();
            avg = latencies.Sum() / (double)latencies.Count;
            median_or_percentile = latencies[(int)(latencies.Count / 100.0 * percentile_boundary)];
            max = latencies.Max();
            
            OvLogger.Debug($"Calculated metrics - Min: {min:F3}, Avg: {avg:F3}, P{percentile_boundary}: {median_or_percentile:F3}, Max: {max:F3}");
        }

        private int percentile_boundary = 50;
    }
}
