// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenVinoSharp.Internal;
using OpenVinoSharp.native;

namespace OpenVinoSharp.GenAI
{
    /// <summary>
    /// OpenVINO GenAI LLM 推理管线 / OpenVINO GenAI LLM inference pipeline.
    /// <para>
    /// 该类型包装 <c>ov_genai_llm_pipeline</c>，用于从模型目录加载大语言模型并执行文本生成。
    /// This type wraps <c>ov_genai_llm_pipeline</c> to load an LLM model directory and run text generation.
    /// </para>
    /// </summary>
    public class LLMPipeline : DisposableOvObject
    {
        /// <summary>
        /// 创建 LLM Pipeline / Creates an LLM pipeline.
        /// </summary>
        /// <param name="modelsPath">模型目录 / Model directory.</param>
        /// <param name="device">设备名称，例如 CPU、GPU、NPU / Device name, for example CPU, GPU, or NPU.</param>
        public LLMPipeline(string modelsPath, string device = "CPU")
            : this(modelsPath, device, null)
        {
        }

        /// <summary>
        /// 创建带属性的 LLM Pipeline / Creates an LLM pipeline with properties.
        /// </summary>
        /// <param name="modelsPath">模型目录 / Model directory.</param>
        /// <param name="device">设备名称 / Device name.</param>
        /// <param name="properties">OpenVINO 或 GenAI 属性键值对 / OpenVINO or GenAI property key-value pairs.</param>
        public LLMPipeline(string modelsPath, string device, IDictionary<string, string> properties)
        {
            if (string.IsNullOrEmpty(modelsPath))
                throw new ArgumentException("Model path cannot be null or empty. / 模型目录不能为空。", nameof(modelsPath));
            if (string.IsNullOrEmpty(device))
                throw new ArgumentException("Device cannot be null or empty. / 设备不能为空。", nameof(device));

            CreatePipeline(modelsPath, device, properties);
        }

        internal LLMPipeline(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// 释放原生 LLM Pipeline / Releases the native LLM pipeline.
        /// </summary>
        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero)
            {
                GenAINativeMethods.ov_genai_llm_pipeline_free(_ptr);
                _ptr = IntPtr.Zero;
            }
            base.DisposeUnmanaged();
        }

        /// <summary>
        /// 根据文本 prompt 生成结果 / Generates a result from a text prompt.
        /// </summary>
        public DecodedResults Generate(string prompt, GenerationConfig config = null)
        {
            return GenerateCore(prompt, config, null);
        }

        /// <summary>
        /// 根据文本 prompt 生成结果，并在流式回调中接收增量文本 / Generates from a prompt and receives streamed text chunks.
        /// </summary>
        public DecodedResults Generate(string prompt, Func<string, StreamingStatus> streamer)
        {
            return GenerateCore(prompt, null, streamer);
        }

        /// <summary>
        /// 根据文本 prompt 生成结果，并在流式回调中接收增量文本 / Generates from a prompt and receives streamed text chunks.
        /// </summary>
        public DecodedResults Generate(string prompt, GenerationConfig config, Func<string, StreamingStatus> streamer)
        {
            return GenerateCore(prompt, config, streamer);
        }

        /// <summary>
        /// 根据文本 prompt 生成结果，并使用 Action 接收流式文本 / Generates from a prompt and streams text to an Action.
        /// </summary>
        public DecodedResults Generate(string prompt, Action<string> streamer)
        {
            if (streamer == null)
                throw new ArgumentNullException(nameof(streamer));

            return Generate(prompt, text =>
            {
                streamer(text);
                return StreamingStatus.Running;
            });
        }

        /// <summary>
        /// 直接返回生成文本 / Generates and returns text directly.
        /// </summary>
        public string GenerateText(string prompt, GenerationConfig config = null)
        {
            using (DecodedResults results = Generate(prompt, config))
            {
                return results.GetText();
            }
        }

        /// <summary>
        /// 根据聊天历史生成结果 / Generates from chat history.
        /// </summary>
        public DecodedResults GenerateWithHistory(ChatHistory history, GenerationConfig config = null)
        {
            return GenerateWithHistoryCore(history, config, null);
        }

        /// <summary>
        /// 根据聊天历史生成结果，并在流式回调中接收增量文本 / Generates from chat history with streamed text chunks.
        /// </summary>
        public DecodedResults GenerateWithHistory(ChatHistory history, GenerationConfig config, Func<string, StreamingStatus> streamer)
        {
            return GenerateWithHistoryCore(history, config, streamer);
        }

        /// <summary>
        /// 开始聊天模式并保留 KV cache / Starts chat mode and keeps KV cache.
        /// </summary>
        public void StartChat()
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_llm_pipeline_start_chat(_ptr));
        }

        /// <summary>
        /// 结束聊天模式并清理 KV cache / Finishes chat mode and clears KV cache.
        /// </summary>
        public void FinishChat()
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_llm_pipeline_finish_chat(_ptr));
        }

        /// <summary>
        /// 获取当前生成配置副本 / Gets a copy of the current generation configuration.
        /// </summary>
        public GenerationConfig GetGenerationConfig()
        {
            ThrowIfDisposed();
            IntPtr configPtr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_llm_pipeline_get_generation_config(_ptr, ref configPtr));
            return new GenerationConfig(configPtr);
        }

        /// <summary>
        /// 设置默认生成配置 / Sets the default generation configuration.
        /// </summary>
        public void SetGenerationConfig(GenerationConfig config)
        {
            ThrowIfDisposed();
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_llm_pipeline_set_generation_config(_ptr, config.OvPtr));
        }

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public DecodedResults generate(string prompt, GenerationConfig config = null) => Generate(prompt, config);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public string generate_text(string prompt, GenerationConfig config = null) => GenerateText(prompt, config);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public DecodedResults generate_with_history(ChatHistory history, GenerationConfig config = null) => GenerateWithHistory(history, config);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public void start_chat() => StartChat();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public void finish_chat() => FinishChat();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig get_generation_config() => GetGenerationConfig();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public void set_generation_config(GenerationConfig config) => SetGenerationConfig(config);

        private void CreatePipeline(string modelsPath, string device, IDictionary<string, string> properties)
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
                            return GenAINativeMethods.ov_genai_llm_pipeline_create(modelPtr, devicePtr, argCount, ref _ptr);
                        case 2:
                            return GenAINativeMethods.ov_genai_llm_pipeline_create(modelPtr, devicePtr, argCount, ref _ptr, propertyPtrs[0], propertyPtrs[1]);
                        case 4:
                            return GenAINativeMethods.ov_genai_llm_pipeline_create(modelPtr, devicePtr, argCount, ref _ptr, propertyPtrs[0], propertyPtrs[1], propertyPtrs[2], propertyPtrs[3]);
                        case 6:
                            return GenAINativeMethods.ov_genai_llm_pipeline_create(modelPtr, devicePtr, argCount, ref _ptr, propertyPtrs[0], propertyPtrs[1], propertyPtrs[2], propertyPtrs[3], propertyPtrs[4], propertyPtrs[5]);
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

        private DecodedResults GenerateCore(string prompt, GenerationConfig config, Func<string, StreamingStatus> streamer)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(prompt))
                throw new ArgumentException("Prompt cannot be null or empty. / prompt 不能为空。", nameof(prompt));

            IntPtr resultsPtr = IntPtr.Zero;
            IntPtr configPtr = config == null ? IntPtr.Zero : config.OvPtr;

            if (streamer == null)
            {
                ExceptionStatus status = StringUtils.WithUtf8Ptr(
                    prompt,
                    promptPtr => GenAINativeMethods.ov_genai_llm_pipeline_generate(_ptr, promptPtr, configPtr, IntPtr.Zero, ref resultsPtr));
                ExceptionHandler.ThrowOnError(status);
            }
            else
            {
                GenAINativeMethods.ov_genai_streamer_callback_func nativeCallback = CreateStreamerCallback(streamer);
                GenAINativeMethods.streamer_callback callback = new GenAINativeMethods.streamer_callback
                {
                    callback_func = Marshal.GetFunctionPointerForDelegate(nativeCallback),
                    args = IntPtr.Zero
                };

                ExceptionStatus status = StringUtils.WithUtf8Ptr(
                    prompt,
                    promptPtr => GenAINativeMethods.ov_genai_llm_pipeline_generate(_ptr, promptPtr, configPtr, ref callback, ref resultsPtr));
                GC.KeepAlive(nativeCallback);
                ExceptionHandler.ThrowOnError(status);
            }

            return new DecodedResults(resultsPtr);
        }

        private DecodedResults GenerateWithHistoryCore(ChatHistory history, GenerationConfig config, Func<string, StreamingStatus> streamer)
        {
            ThrowIfDisposed();
            if (history == null)
                throw new ArgumentNullException(nameof(history));

            IntPtr resultsPtr = IntPtr.Zero;
            IntPtr configPtr = config == null ? IntPtr.Zero : config.OvPtr;

            if (streamer == null)
            {
                ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_llm_pipeline_generate_with_history(
                    _ptr,
                    history.OvPtr,
                    configPtr,
                    IntPtr.Zero,
                    ref resultsPtr));
            }
            else
            {
                GenAINativeMethods.ov_genai_streamer_callback_func nativeCallback = CreateStreamerCallback(streamer);
                GenAINativeMethods.streamer_callback callback = new GenAINativeMethods.streamer_callback
                {
                    callback_func = Marshal.GetFunctionPointerForDelegate(nativeCallback),
                    args = IntPtr.Zero
                };

                ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_llm_pipeline_generate_with_history(
                    _ptr,
                    history.OvPtr,
                    configPtr,
                    ref callback,
                    ref resultsPtr));
                GC.KeepAlive(nativeCallback);
            }

            return new DecodedResults(resultsPtr);
        }

        private static GenAINativeMethods.ov_genai_streamer_callback_func CreateStreamerCallback(Func<string, StreamingStatus> streamer)
        {
            return (strPtr, args) =>
            {
                try
                {
                    string text = StringUtils.Utf8PtrToString(strPtr) ?? string.Empty;
                    return streamer(text);
                }
                catch
                {
                    return StreamingStatus.Cancel;
                }
            };
        }

        private static IntPtr[] BuildPropertyPointers(IDictionary<string, string> properties)
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
