// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using OpenVinoSharp.Internal;
using OpenVinoSharp.native;

namespace OpenVinoSharp.GenAI
{
    /// <summary>
    /// 平均值与标准差 / Mean and standard deviation pair.
    /// </summary>
    public struct MetricStatistics
    {
        /// <summary>平均值 / Mean value.</summary>
        public float Mean { get; }

        /// <summary>标准差 / Standard deviation.</summary>
        public float StandardDeviation { get; }

        /// <summary>
        /// 创建统计值 / Creates metric statistics.
        /// </summary>
        public MetricStatistics(float mean, float standardDeviation)
        {
            Mean = mean;
            StandardDeviation = standardDeviation;
        }

        /// <summary>
        /// 返回可读文本 / Returns readable text.
        /// </summary>
        public override string ToString()
        {
            return $"mean={Mean}, std={StandardDeviation}";
        }
    }

    /// <summary>
    /// OpenVINO GenAI 性能指标 / Performance metrics for OpenVINO GenAI generation.
    /// </summary>
    public class PerformanceMetrics : DisposableOvObject
    {
        private readonly Action<IntPtr> _freeAction;

        internal PerformanceMetrics(IntPtr ptr, Action<IntPtr> freeAction)
            : base(ptr)
        {
            _freeAction = freeAction ?? throw new ArgumentNullException(nameof(freeAction));
        }

        /// <summary>
        /// 释放原生性能指标对象 / Releases the native performance metrics object.
        /// </summary>
        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero)
            {
                _freeAction(_ptr);
                _ptr = IntPtr.Zero;
            }
            base.DisposeUnmanaged();
        }

        /// <summary>模型加载耗时，单位毫秒 / Model load time in milliseconds.</summary>
        public float LoadTime => GetFloat(GenAINativeMethods.ov_genai_perf_metrics_get_load_time);

        /// <summary>生成 token 数 / Number of generated tokens.</summary>
        public ulong NumGenerationTokens => GetSize(GenAINativeMethods.ov_genai_perf_metrics_get_num_generation_tokens);

        /// <summary>输入 token 数 / Number of input tokens.</summary>
        public ulong NumInputTokens => GetSize(GenAINativeMethods.ov_genai_perf_metrics_get_num_input_tokens);

        /// <summary>首 token 延迟 / Time to first token.</summary>
        public MetricStatistics TimeToFirstToken => GetStatistics(GenAINativeMethods.ov_genai_perf_metrics_get_ttft);

        /// <summary>每输出 token 耗时 / Time per output token.</summary>
        public MetricStatistics TimePerOutputToken => GetStatistics(GenAINativeMethods.ov_genai_perf_metrics_get_tpot);

        /// <summary>每输出 token 推理耗时 / Inference time per output token.</summary>
        public MetricStatistics InferenceTimePerOutputToken => GetStatistics(GenAINativeMethods.ov_genai_perf_metrics_get_ipot);

        /// <summary>吞吐量 token/s / Throughput in tokens per second.</summary>
        public MetricStatistics Throughput => GetStatistics(GenAINativeMethods.ov_genai_perf_metrics_get_throughput);

        /// <summary>推理耗时 / Inference duration.</summary>
        public MetricStatistics InferenceDuration => GetStatistics(GenAINativeMethods.ov_genai_perf_metrics_get_inference_duration);

        /// <summary>生成耗时 / Generate duration.</summary>
        public MetricStatistics GenerateDuration => GetStatistics(GenAINativeMethods.ov_genai_perf_metrics_get_generate_duration);

        /// <summary>分词耗时 / Tokenization duration.</summary>
        public MetricStatistics TokenizationDuration => GetStatistics(GenAINativeMethods.ov_genai_perf_metrics_get_tokenization_duration);

        /// <summary>反分词耗时 / Detokenization duration.</summary>
        public MetricStatistics DetokenizationDuration => GetStatistics(GenAINativeMethods.ov_genai_perf_metrics_get_detokenization_duration);

        /// <summary>Chat template 应用耗时 / Chat template application duration.</summary>
        public MetricStatistics ChatTemplateDuration => GetStatistics(GenAINativeMethods.ov_genai_perf_metrics_get_chat_template_duration);

        /// <summary>
        /// 将另一个指标累加到当前指标 / Adds another metrics object into this one.
        /// </summary>
        public void AddInPlace(PerformanceMetrics other)
        {
            ThrowIfDisposed();
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_perf_metrics_add_in_place(_ptr, other.OvPtr));
        }

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public void add_in_place(PerformanceMetrics other) => AddInPlace(other);

        private delegate ExceptionStatus FloatGetter(IntPtr metrics, ref float value);
        private delegate ExceptionStatus SizeGetter(IntPtr metrics, ref UIntPtr value);
        private delegate ExceptionStatus StatisticsGetter(IntPtr metrics, ref float mean, ref float std);

        private float GetFloat(FloatGetter getter)
        {
            ThrowIfDisposed();
            float value = 0;
            ExceptionHandler.ThrowOnError(getter(_ptr, ref value));
            return value;
        }

        private ulong GetSize(SizeGetter getter)
        {
            ThrowIfDisposed();
            UIntPtr value = UIntPtr.Zero;
            ExceptionHandler.ThrowOnError(getter(_ptr, ref value));
            return StringUtils.FromNativeSize(value);
        }

        private MetricStatistics GetStatistics(StatisticsGetter getter)
        {
            ThrowIfDisposed();
            float mean = 0;
            float std = 0;
            ExceptionHandler.ThrowOnError(getter(_ptr, ref mean, ref std));
            return new MetricStatistics(mean, std);
        }
    }
}

