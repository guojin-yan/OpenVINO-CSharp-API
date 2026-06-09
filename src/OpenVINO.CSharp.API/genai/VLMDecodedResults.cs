// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using OpenVinoSharp.Internal;
using OpenVinoSharp.native;

namespace OpenVinoSharp.GenAI
{
    /// <summary>
    /// VLM 推理后的解码结果 / Decoded results returned by VLM generation.
    /// <para>
    /// 该类型拥有 <c>ov_genai_vlm_decoded_results</c> 原生对象。性能指标由 native 新建，并由
    /// <see cref="PerformanceMetrics"/> 托管包装负责释放。
    /// This type owns a native <c>ov_genai_vlm_decoded_results</c> object. Performance metrics are newly created by native
    /// code and owned by the <see cref="PerformanceMetrics"/> managed wrapper.
    /// </para>
    /// </summary>
    public class VLMDecodedResults : DisposableOvObject
    {
        /// <summary>
        /// 创建空 VLM 解码结果 / Creates an empty VLM decoded-results object.
        /// </summary>
        public VLMDecodedResults()
        {
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_vlm_decoded_results_create(ref _ptr));
        }

        internal VLMDecodedResults(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// 生成文本 / Generated text.
        /// </summary>
        public string Text => GetText();

        /// <summary>
        /// 释放原生 VLM 解码结果 / Releases the native VLM decoded results.
        /// </summary>
        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero)
            {
                GenAINativeMethods.ov_genai_vlm_decoded_results_free(_ptr);
                _ptr = IntPtr.Zero;
            }
            base.DisposeUnmanaged();
        }

        /// <summary>
        /// 获取生成文本 / Gets generated text.
        /// </summary>
        public string GetText()
        {
            ThrowIfDisposed();
            return GenAIStringHelper.GetString(_ptr, GenAINativeMethods.ov_genai_vlm_decoded_results_get_string);
        }

        /// <summary>
        /// 获取性能指标 / Gets performance metrics.
        /// </summary>
        public PerformanceMetrics GetPerformanceMetrics()
        {
            ThrowIfDisposed();
            IntPtr metricsPtr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_vlm_decoded_results_get_perf_metrics(_ptr, ref metricsPtr));
            return new PerformanceMetrics(metricsPtr, GenAINativeMethods.ov_genai_vlm_decoded_results_perf_metrics_free);
        }

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public string get_string() => GetText();

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public PerformanceMetrics get_perf_metrics() => GetPerformanceMetrics();

        /// <summary>
        /// 返回生成文本 / Returns generated text.
        /// </summary>
        public override string ToString()
        {
            return GetText();
        }
    }
}
