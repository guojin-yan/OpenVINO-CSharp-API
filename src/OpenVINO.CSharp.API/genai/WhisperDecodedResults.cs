// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

#nullable enable

using System;
using OpenVinoSharp.Internal;
using OpenVinoSharp.native;

namespace OpenVinoSharp.GenAI
{
    /// <summary>
    /// Whisper 推理后的解码结果 / Decoded results returned by Whisper generation.
    /// <para>
    /// 该类型拥有 <c>ov_genai_whisper_decoded_results</c> 原生对象。由结果对象返回的 chunk 和 metrics 是新建对象，
    /// 调用方通过托管包装类型负责释放。
    /// This type owns a native <c>ov_genai_whisper_decoded_results</c> object. Chunks and metrics returned from it are newly
    /// created native objects and are owned by their managed wrappers.
    /// </para>
    /// </summary>
    public class WhisperDecodedResults : DisposableOvObject
    {
        /// <summary>
        /// 创建空 Whisper 解码结果 / Creates an empty Whisper decoded-results object.
        /// </summary>
        public WhisperDecodedResults()
        {
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_whisper_decoded_results_create(ref _ptr));
        }

        internal WhisperDecodedResults(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// 文本结果数量 / Number of text results.
        /// </summary>
        public ulong TextCount => GetTextCount();

        /// <summary>
        /// 是否包含时间戳分段 / Whether timestamp chunks are available.
        /// </summary>
        public bool HasChunks => GetHasChunks();

        /// <summary>
        /// 分段数量 / Number of chunks.
        /// </summary>
        public ulong ChunkCount => GetChunkCount();

        /// <summary>
        /// 释放原生 Whisper 解码结果 / Releases the native Whisper decoded results.
        /// </summary>
        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero)
            {
                GenAINativeMethods.ov_genai_whisper_decoded_results_free(_ptr);
                _ptr = IntPtr.Zero;
            }
            base.DisposeUnmanaged();
        }

        /// <summary>
        /// 获取性能指标 / Gets performance metrics.
        /// </summary>
        /// <remarks>
        /// 返回的 <see cref="PerformanceMetrics"/> 是新建原生对象的拥有包装。OpenVINO GenAI 2026.3 未导出通用 metrics free，
        /// 因此使用已导出的 <c>ov_genai_decoded_results_perf_metrics_free</c> 释放同类型指针。
        /// The returned <see cref="PerformanceMetrics"/> owns a newly created native object and releases it with the exported
        /// <c>ov_genai_decoded_results_perf_metrics_free</c> because OpenVINO GenAI 2026.3 does not export a generic metrics free function.
        /// </remarks>
        public PerformanceMetrics GetPerformanceMetrics()
        {
            ThrowIfDisposed();
            IntPtr metricsPtr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_whisper_decoded_results_get_perf_metrics(_ptr, ref metricsPtr));
            return new PerformanceMetrics(metricsPtr, GenAINativeMethods.ov_genai_decoded_results_perf_metrics_free);
        }

        /// <summary>
        /// 获取文本结果数量 / Gets the number of text results.
        /// </summary>
        public ulong GetTextCount()
        {
            ThrowIfDisposed();
            UIntPtr count = UIntPtr.Zero;
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_whisper_decoded_results_get_texts_count(_ptr, ref count));
            return StringUtils.FromNativeSize(count);
        }

        /// <summary>
        /// 获取指定索引的文本 / Gets the text result at the specified index.
        /// </summary>
        public string GetTextAt(ulong index)
        {
            ThrowIfDisposed();
            UIntPtr nativeIndex = StringUtils.ToNativeSize(index);
            return GenAIStringHelper.GetString(
                _ptr,
                (IntPtr handle, IntPtr output, ref UIntPtr outputSize) =>
                    GenAINativeMethods.ov_genai_whisper_decoded_results_get_text_at(handle, nativeIndex, output, ref outputSize));
        }

        /// <summary>
        /// 获取指定索引的分数 / Gets the score at the specified index.
        /// </summary>
        public float GetScoreAt(ulong index)
        {
            ThrowIfDisposed();
            float score = 0;
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_whisper_decoded_results_get_score_at(
                _ptr,
                StringUtils.ToNativeSize(index),
                ref score));
            return score;
        }

        /// <summary>
        /// 获取是否包含时间戳分段 / Gets whether timestamp chunks are available.
        /// </summary>
        public bool GetHasChunks()
        {
            ThrowIfDisposed();
            byte value = 0;
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_whisper_decoded_results_has_chunks(_ptr, ref value));
            return value != 0;
        }

        /// <summary>
        /// 获取分段数量 / Gets the chunk count.
        /// </summary>
        public ulong GetChunkCount()
        {
            ThrowIfDisposed();
            UIntPtr count = UIntPtr.Zero;
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_whisper_decoded_results_get_chunks_count(_ptr, ref count));
            return StringUtils.FromNativeSize(count);
        }

        /// <summary>
        /// 获取指定索引的分段；如果原生结果没有分段会返回 null / Gets a chunk by index; returns null when native results contain no chunks.
        /// </summary>
        public WhisperDecodedResultChunk? GetChunkAt(ulong index)
        {
            ThrowIfDisposed();
            IntPtr chunkPtr = IntPtr.Zero;
            ExceptionStatus status = GenAINativeMethods.ov_genai_whisper_decoded_results_get_chunk_at(
                _ptr,
                StringUtils.ToNativeSize(index),
                ref chunkPtr);
            if (status == ExceptionStatus.NOT_FOUND)
                return null;

            ExceptionHandler.ThrowOnError(status);
            return new WhisperDecodedResultChunk(chunkPtr);
        }

        /// <summary>
        /// 获取原生结果的字符串表示 / Gets the native string representation.
        /// </summary>
        public string GetString()
        {
            ThrowIfDisposed();
            return GenAIStringHelper.GetString(_ptr, GenAINativeMethods.ov_genai_whisper_decoded_results_get_string);
        }

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public PerformanceMetrics get_perf_metrics() => GetPerformanceMetrics();

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public ulong get_texts_count() => GetTextCount();

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public string get_text_at(ulong index) => GetTextAt(index);

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public float get_score_at(ulong index) => GetScoreAt(index);

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public bool has_chunks() => GetHasChunks();

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public ulong get_chunks_count() => GetChunkCount();

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public WhisperDecodedResultChunk? get_chunk_at(ulong index) => GetChunkAt(index);

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public string get_string() => GetString();

        /// <summary>
        /// 返回原生结果的字符串表示 / Returns the native string representation.
        /// </summary>
        public override string ToString()
        {
            return GetString();
        }
    }
}
