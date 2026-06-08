// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using OpenVinoSharp.Internal;
using Xunit;

namespace OpenVinoSharp.Tests.Benchmarks
{
    /// <summary>
    /// Tensor 操作基准测试 / Tensor operation benchmarks
    /// </summary>
    [SimpleJob(RuntimeMoniker.Net80)]
    [MemoryDiagnoser]
    [Trait("Category", TestCategories.Performance)]
    public class TensorBenchmarks
    {
        private float[]? _testData;
        private Shape? _shape;
        private Tensor? _tensor;

        [Params(224 * 224 * 3, 512 * 512 * 3, 1024 * 1024)]
        public int DataSize { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _testData = TestDataGenerator.GenerateRandomFloatArray(DataSize);
            _shape = new Shape(new long[] { 1, 3, (long)Math.Sqrt(DataSize / 3), (long)Math.Sqrt(DataSize / 3) });
            _tensor = new Tensor(_shape, ElementType.F32);
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _tensor?.Dispose();
            _shape?.Dispose();
        }

        [Benchmark(Description = "set_data (float[])")]
        public void SetData_Array()
        {
            _tensor!.set_data(_testData!);
        }

        [Benchmark(Description = "get_float_data")]
        public float[] GetFloatData()
        {
            return _tensor!.get_float_data();
        }

        [Benchmark(Description = "get_span (zero-copy)")]
        public unsafe void GetSpan()
        {
            var span = _tensor!.get_span<float>();
            // 读取一点数据防止被优化掉
            var _ = span[0];
        }

        [Benchmark(Description = "get_byte_size")]
        public ulong GetByteSize()
        {
            return _tensor!.byte_size;
        }

        [Benchmark(Description = "get_size")]
        public ulong GetSize()
        {
            return _tensor!.size;
        }
    }

    /// <summary>
    /// Shape 操作基准测试 / Shape operation benchmarks
    /// </summary>
    [SimpleJob(RuntimeMoniker.Net80)]
    [MemoryDiagnoser]
    [Trait("Category", TestCategories.Performance)]
    public class ShapeBenchmarks
    {
        private Shape? _shape;

        [GlobalSetup]
        public void Setup()
        {
            _shape = new Shape(new long[] { 1, 3, 224, 224 });
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _shape?.Dispose();
        }

        [Benchmark(Description = "get_dims")]
        public long[] GetDims()
        {
            return _shape!.get_dims();
        }

        [Benchmark(Description = "get_rank")]
        public long GetRank()
        {
            return _shape!.get_rank();
        }

        [Benchmark(Description = "get_total_elements")]
        public long GetTotalElements()
        {
            return _shape!.get_total_elements();
        }

        [Benchmark(Description = "get_dim(index)")]
        [Arguments(0)]
        [Arguments(2)]
        public long GetDim(int index)
        {
            return _shape!.get_dim(index);
        }
    }

    /// <summary>
    /// 日志基准测试 / OvLogger benchmarks
    /// </summary>
    [SimpleJob(RuntimeMoniker.Net80)]
    [MemoryDiagnoser]
    [Trait("Category", TestCategories.Performance)]
    public class OvLoggerBenchmarks
    {
        [GlobalSetup]
        public void Setup()
        {
            // 设置高级别，禁用日志输出
            OvLogger.MinLevel = LogLevel.NONE;
        }

        [Benchmark(Description = "Log (disabled)")]
        public void LogDisabled()
        {
            OvLogger.Debug("This is a debug message: {0}", 42);
        }

        [Benchmark(Description = "IsEnabled check")]
        public bool IsEnabledCheck()
        {
            return OvLogger.IsDebugEnabled;
        }
    }
}
