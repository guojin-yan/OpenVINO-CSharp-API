// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using OpenVinoSharp.Internal;
using OpenVinoSharp.native;

namespace OpenVinoSharp.GenAI
{
    /// <summary>
    /// Whisper 解码结果分段 / A decoded Whisper result chunk.
    /// <para>
    /// 该类型拥有 <c>ov_genai_whisper_decoded_result_chunk</c> 原生对象，并在释放时调用匹配的 free 函数。
    /// This type owns a native <c>ov_genai_whisper_decoded_result_chunk</c> object and releases it with the matching free function.
    /// </para>
    /// </summary>
    public class WhisperDecodedResultChunk : DisposableOvObject
    {
        /// <summary>
        /// 创建空 Whisper 分段结果 / Creates an empty Whisper decoded result chunk.
        /// </summary>
        public WhisperDecodedResultChunk()
        {
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_whisper_decoded_result_chunk_create(ref _ptr));
        }

        internal WhisperDecodedResultChunk(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// 分段起始时间戳，单位由 OpenVINO GenAI runtime 定义 / Chunk start timestamp, in units defined by OpenVINO GenAI runtime.
        /// </summary>
        public float StartTimestamp => GetStartTimestamp();

        /// <summary>
        /// 分段结束时间戳，单位由 OpenVINO GenAI runtime 定义 / Chunk end timestamp, in units defined by OpenVINO GenAI runtime.
        /// </summary>
        public float EndTimestamp => GetEndTimestamp();

        /// <summary>
        /// 分段文本 / Chunk text.
        /// </summary>
        public string Text => GetText();

        /// <summary>
        /// 释放原生 Whisper 分段结果 / Releases the native Whisper decoded result chunk.
        /// </summary>
        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero)
            {
                GenAINativeMethods.ov_genai_whisper_decoded_result_chunk_free(_ptr);
                _ptr = IntPtr.Zero;
            }
            base.DisposeUnmanaged();
        }

        /// <summary>
        /// 获取分段起始时间戳 / Gets the chunk start timestamp.
        /// </summary>
        public float GetStartTimestamp()
        {
            ThrowIfDisposed();
            float value = 0;
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_whisper_decoded_result_chunk_get_start_ts(_ptr, ref value));
            return value;
        }

        /// <summary>
        /// 获取分段结束时间戳 / Gets the chunk end timestamp.
        /// </summary>
        public float GetEndTimestamp()
        {
            ThrowIfDisposed();
            float value = 0;
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_whisper_decoded_result_chunk_get_end_ts(_ptr, ref value));
            return value;
        }

        /// <summary>
        /// 获取分段文本 / Gets the chunk text.
        /// </summary>
        public string GetText()
        {
            ThrowIfDisposed();
            return GenAIStringHelper.GetString(_ptr, GenAINativeMethods.ov_genai_whisper_decoded_result_chunk_get_text);
        }

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public float get_start_ts() => GetStartTimestamp();

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public float get_end_ts() => GetEndTimestamp();

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public string get_text() => GetText();

        /// <summary>
        /// 返回分段文本 / Returns the chunk text.
        /// </summary>
        public override string ToString()
        {
            return GetText();
        }
    }
}
