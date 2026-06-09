// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

#nullable enable

using System;
using System.Collections.Generic;
using OpenVinoSharp.Internal;
using OpenVinoSharp.native;

namespace OpenVinoSharp.GenAI
{
    /// <summary>
    /// OpenVINO GenAI Whisper 推理管线 / OpenVINO GenAI Whisper inference pipeline.
    /// <para>
    /// 该类型拥有 <c>ov_genai_whisper_pipeline</c> 原生对象。构造和生成时才会加载 GenAI runtime，
    /// 不影响只使用基础 OpenVINO API 的应用。
    /// This type owns a native <c>ov_genai_whisper_pipeline</c> object. GenAI runtime is loaded only when the pipeline is
    /// constructed or used, so OpenVINO-only applications are not affected.
    /// </para>
    /// </summary>
    public class WhisperPipeline : DisposableOvObject
    {
        /// <summary>
        /// 创建 Whisper Pipeline / Creates a Whisper pipeline.
        /// </summary>
        /// <param name="modelsPath">模型目录 / Model directory.</param>
        /// <param name="device">设备名称，例如 CPU、GPU 或 NPU / Device name, for example CPU, GPU, or NPU.</param>
        public WhisperPipeline(string modelsPath, string device = "CPU")
            : this(modelsPath, device, null)
        {
        }

        /// <summary>
        /// 创建带属性的 Whisper Pipeline / Creates a Whisper pipeline with properties.
        /// </summary>
        /// <param name="modelsPath">模型目录 / Model directory.</param>
        /// <param name="device">设备名称 / Device name.</param>
        /// <param name="properties">OpenVINO 或 GenAI 属性键值对 / OpenVINO or GenAI property key-value pairs.</param>
        public WhisperPipeline(string modelsPath, string device, IDictionary<string, string>? properties)
        {
            if (string.IsNullOrEmpty(modelsPath))
                throw new ArgumentException("Model path cannot be null or empty. / 模型目录不能为空。", nameof(modelsPath));
            if (string.IsNullOrEmpty(device))
                throw new ArgumentException("Device cannot be null or empty. / 设备不能为空。", nameof(device));

            CreatePipeline(modelsPath, device, properties);
        }

        internal WhisperPipeline(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// 释放原生 Whisper Pipeline / Releases the native Whisper pipeline.
        /// </summary>
        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero)
            {
                GenAINativeMethods.ov_genai_whisper_pipeline_free(_ptr);
                _ptr = IntPtr.Zero;
            }
            base.DisposeUnmanaged();
        }

        /// <summary>
        /// 使用原始语音采样生成 Whisper 结果 / Generates Whisper results from raw speech samples.
        /// </summary>
        /// <param name="rawSpeech">原始语音 float 采样数组 / Raw speech float sample array.</param>
        /// <param name="config">可选 Whisper 生成配置 / Optional Whisper generation configuration.</param>
        /// <returns>Whisper 解码结果 / Whisper decoded results.</returns>
        public WhisperDecodedResults Generate(float[] rawSpeech, WhisperGenerationConfig? config = null)
        {
            ThrowIfDisposed();
            if (rawSpeech == null)
                throw new ArgumentNullException(nameof(rawSpeech));
            if (rawSpeech.Length == 0)
                throw new ArgumentException("Raw speech cannot be empty. / 原始语音不能为空。", nameof(rawSpeech));

            IntPtr resultsPtr = IntPtr.Zero;
            IntPtr configPtr = config == null ? IntPtr.Zero : config.OvPtr;
            unsafe
            {
                fixed (float* speechPtr = rawSpeech)
                {
                    ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_whisper_pipeline_generate(
                        _ptr,
                        (IntPtr)speechPtr,
                        StringUtils.ToNativeSize((ulong)rawSpeech.Length),
                        configPtr,
                        ref resultsPtr));
                }
            }

            return new WhisperDecodedResults(resultsPtr);
        }

#if HAS_SPAN
        /// <summary>
        /// 使用原始语音采样生成 Whisper 结果 / Generates Whisper results from raw speech samples.
        /// </summary>
        /// <param name="rawSpeech">原始语音只读 Span / Raw speech read-only span.</param>
        /// <param name="config">可选 Whisper 生成配置 / Optional Whisper generation configuration.</param>
        /// <returns>Whisper 解码结果 / Whisper decoded results.</returns>
        public WhisperDecodedResults Generate(ReadOnlySpan<float> rawSpeech, WhisperGenerationConfig? config = null)
        {
            ThrowIfDisposed();
            if (rawSpeech.IsEmpty)
                throw new ArgumentException("Raw speech cannot be empty. / 原始语音不能为空。", nameof(rawSpeech));

            IntPtr resultsPtr = IntPtr.Zero;
            IntPtr configPtr = config == null ? IntPtr.Zero : config.OvPtr;
            unsafe
            {
                fixed (float* speechPtr = rawSpeech)
                {
                    ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_whisper_pipeline_generate(
                        _ptr,
                        (IntPtr)speechPtr,
                        StringUtils.ToNativeSize((ulong)rawSpeech.Length),
                        configPtr,
                        ref resultsPtr));
                }
            }

            return new WhisperDecodedResults(resultsPtr);
        }
#endif

        /// <summary>
        /// 获取当前 Whisper 生成配置副本 / Gets a copy of the current Whisper generation configuration.
        /// </summary>
        /// <remarks>
        /// 返回对象是新建原生配置的拥有包装，由调用方释放。
        /// The returned object owns a newly created native configuration and must be disposed by the caller.
        /// </remarks>
        public WhisperGenerationConfig GetGenerationConfig()
        {
            ThrowIfDisposed();
            IntPtr configPtr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_whisper_pipeline_get_generation_config(_ptr, ref configPtr));
            return new WhisperGenerationConfig(configPtr);
        }

        /// <summary>
        /// 设置默认 Whisper 生成配置 / Sets the default Whisper generation configuration.
        /// </summary>
        public void SetGenerationConfig(WhisperGenerationConfig config)
        {
            ThrowIfDisposed();
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_whisper_pipeline_set_generation_config(_ptr, config.OvPtr));
        }

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public WhisperDecodedResults generate(float[] rawSpeech, WhisperGenerationConfig? config = null) => Generate(rawSpeech, config);

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public WhisperGenerationConfig get_generation_config() => GetGenerationConfig();

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public void set_generation_config(WhisperGenerationConfig config) => SetGenerationConfig(config);

        private void CreatePipeline(string modelsPath, string device, IDictionary<string, string>? properties)
        {
            IntPtr[] propertyPtrs = BuildPropertyPointers(properties);
            try
            {
                ExceptionStatus status = StringUtils.WithUtf8Ptrs(modelsPath, device, (modelPtr, devicePtr) =>
                {
                    UIntPtr argCount = StringUtils.ToNativeSize((ulong)propertyPtrs.Length);
                    switch (propertyPtrs.Length)
                    {
                        case 0:
                            return GenAINativeMethods.ov_genai_whisper_pipeline_create(modelPtr, devicePtr, argCount, ref _ptr);
                        case 2:
                            return GenAINativeMethods.ov_genai_whisper_pipeline_create(modelPtr, devicePtr, argCount, ref _ptr, propertyPtrs[0], propertyPtrs[1]);
                        case 4:
                            return GenAINativeMethods.ov_genai_whisper_pipeline_create(modelPtr, devicePtr, argCount, ref _ptr, propertyPtrs[0], propertyPtrs[1], propertyPtrs[2], propertyPtrs[3]);
                        case 6:
                            return GenAINativeMethods.ov_genai_whisper_pipeline_create(modelPtr, devicePtr, argCount, ref _ptr, propertyPtrs[0], propertyPtrs[1], propertyPtrs[2], propertyPtrs[3], propertyPtrs[4], propertyPtrs[5]);
                        default:
                            throw new ArgumentException("Only 0, 1, 2, or 3 property pairs are supported. / 当前仅支持 0、1、2、3 组属性。", nameof(properties));
                    }
                });

                ExceptionHandler.ThrowOnError(status);
            }
            finally
            {
                foreach (IntPtr ptr in propertyPtrs)
                    StringUtils.FreeUtf8Ptr(ptr);
            }
        }

        private static IntPtr[] BuildPropertyPointers(IDictionary<string, string>? properties)
        {
            if (properties == null || properties.Count == 0)
                return new IntPtr[0];
            if (properties.Count > 3)
                throw new ArgumentException("Only 0, 1, 2, or 3 property pairs are supported. / 当前仅支持 0、1、2、3 组属性。", nameof(properties));

            IntPtr[] pointers = new IntPtr[properties.Count * 2];
            int index = 0;
            try
            {
                foreach (KeyValuePair<string, string> item in properties)
                {
                    if (string.IsNullOrEmpty(item.Key))
                        throw new ArgumentException("Property key cannot be null or empty. / 属性名不能为空。", nameof(properties));

                    pointers[index++] = StringUtils.StringToUtf8Ptr(item.Key);
                    pointers[index++] = StringUtils.StringToUtf8Ptr(item.Value ?? string.Empty);
                }
                return pointers;
            }
            catch
            {
                foreach (IntPtr ptr in pointers)
                    StringUtils.FreeUtf8Ptr(ptr);
                throw;
            }
        }
    }
}
