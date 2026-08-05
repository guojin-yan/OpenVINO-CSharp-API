// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

#nullable enable

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenVinoSharp.Internal;
using OpenVinoSharp.native;

namespace OpenVinoSharp.GenAI
{
    /// <summary>
    /// OpenVINO GenAI VLM 推理管线 / OpenVINO GenAI vision-language inference pipeline.
    /// <para>
    /// 该类型拥有 <c>ov_genai_vlm_pipeline</c> 原生对象。图像输入使用调用方提供的 <see cref="Tensor"/>；
    /// 生成调用期间只短暂固定其原生指针数组，Tensor 本身仍由调用方管理。
    /// This type owns a native <c>ov_genai_vlm_pipeline</c> object. Image inputs use caller-provided <see cref="Tensor"/>
    /// instances; only the native pointer array is pinned for the duration of the native call, and tensors remain owned by the caller.
    /// </para>
    /// </summary>
    public class VLMPipeline : DisposableOvObject
    {
        /// <summary>
        /// 创建 VLM Pipeline / Creates a VLM pipeline.
        /// </summary>
        /// <param name="modelsPath">模型目录 / Model directory.</param>
        /// <param name="device">设备名称，例如 CPU、GPU 或 NPU / Device name, for example CPU, GPU, or NPU.</param>
        public VLMPipeline(string modelsPath, string device = "CPU")
            : this(modelsPath, device, null)
        {
        }

        /// <summary>
        /// 创建带属性的 VLM Pipeline / Creates a VLM pipeline with properties.
        /// </summary>
        /// <param name="modelsPath">模型目录 / Model directory.</param>
        /// <param name="device">设备名称 / Device name.</param>
        /// <param name="properties">OpenVINO 或 GenAI 属性键值对 / OpenVINO or GenAI property key-value pairs.</param>
        public VLMPipeline(string modelsPath, string device, IDictionary<string, string>? properties)
        {
            if (string.IsNullOrEmpty(modelsPath))
                throw new ArgumentException("Model path cannot be null or empty. / 模型目录不能为空。", nameof(modelsPath));
            if (string.IsNullOrEmpty(device))
                throw new ArgumentException("Device cannot be null or empty. / 设备不能为空。", nameof(device));

            CreatePipeline(modelsPath, device, properties);
        }

        internal VLMPipeline(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// 释放原生 VLM Pipeline / Releases the native VLM pipeline.
        /// </summary>
        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero)
            {
                GenAINativeMethods.ov_genai_vlm_pipeline_free(_ptr);
                _ptr = IntPtr.Zero;
            }
            base.DisposeUnmanaged();
        }

        /// <summary>
        /// 使用文本和可选图像生成 VLM 结果 / Generates VLM results from text and optional images.
        /// </summary>
        public VLMDecodedResults Generate(string prompt, Tensor[]? images = null, GenerationConfig? config = null)
        {
            return GenerateCore(prompt, images, config, null);
        }

        /// <summary>
        /// 使用文本和可选图像生成 VLM 结果，并接收流式文本 / Generates VLM results and receives streamed text.
        /// </summary>
        public VLMDecodedResults Generate(string prompt, Tensor[]? images, Func<string, StreamingStatus> streamer)
        {
            return GenerateCore(prompt, images, null, streamer);
        }

        /// <summary>
        /// 使用文本和可选图像生成 VLM 结果，并接收流式文本 / Generates VLM results and receives streamed text.
        /// </summary>
        public VLMDecodedResults Generate(string prompt, Tensor[]? images, GenerationConfig? config, Func<string, StreamingStatus> streamer)
        {
            return GenerateCore(prompt, images, config, streamer);
        }

        /// <summary>
        /// 使用文本生成 VLM 结果，并接收流式文本 / Generates VLM results from text and receives streamed text.
        /// </summary>
        public VLMDecodedResults Generate(string prompt, Func<string, StreamingStatus> streamer)
        {
            return GenerateCore(prompt, null, null, streamer);
        }

        /// <summary>
        /// 直接返回生成文本 / Generates and returns text directly.
        /// </summary>
        public string GenerateText(string prompt, Tensor[]? images = null, GenerationConfig? config = null)
        {
            using (VLMDecodedResults results = Generate(prompt, images, config))
            {
                return results.GetText();
            }
        }

        /// <summary>
        /// 根据聊天历史和可选图像生成 VLM 结果 / Generates VLM results from chat history and optional images.
        /// </summary>
        public VLMDecodedResults GenerateWithHistory(ChatHistory history, Tensor[]? images = null, GenerationConfig? config = null)
        {
            return GenerateWithHistoryCore(history, images, config, null);
        }

        /// <summary>
        /// 根据聊天历史和可选图像生成 VLM 结果，并接收流式文本 / Generates VLM results from chat history and receives streamed text.
        /// </summary>
        public VLMDecodedResults GenerateWithHistory(ChatHistory history, Tensor[]? images, GenerationConfig? config, Func<string, StreamingStatus> streamer)
        {
            return GenerateWithHistoryCore(history, images, config, streamer);
        }

        /// <summary>
        /// 开始聊天模式并保留 KV cache / Starts chat mode and keeps KV cache.
        /// </summary>
        [Obsolete("OpenVINO GenAI deprecated stateful chat mode. Use GenerateWithHistory(ChatHistory, ...) instead.")]
        public void StartChat()
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_vlm_pipeline_start_chat(_ptr));
        }

        /// <summary>
        /// 结束聊天模式并清理 KV cache / Finishes chat mode and clears KV cache.
        /// </summary>
        [Obsolete("OpenVINO GenAI deprecated stateful chat mode. Use GenerateWithHistory(ChatHistory, ...) instead.")]
        public void FinishChat()
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_vlm_pipeline_finish_chat(_ptr));
        }

        /// <summary>
        /// 获取当前文本生成配置副本 / Gets a copy of the current generation configuration.
        /// </summary>
        public GenerationConfig GetGenerationConfig()
        {
            ThrowIfDisposed();
            IntPtr configPtr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_vlm_pipeline_get_generation_config(_ptr, ref configPtr));
            return new GenerationConfig(configPtr);
        }

        /// <summary>
        /// 设置默认文本生成配置 / Sets the default generation configuration.
        /// </summary>
        public void SetGenerationConfig(GenerationConfig config)
        {
            ThrowIfDisposed();
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_vlm_pipeline_set_generation_config(_ptr, config.OvPtr));
        }

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public VLMDecodedResults generate(string prompt, Tensor[]? images = null, GenerationConfig? config = null) => Generate(prompt, images, config);

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public string generate_text(string prompt, Tensor[]? images = null, GenerationConfig? config = null) => GenerateText(prompt, images, config);

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public VLMDecodedResults generate_with_history(ChatHistory history, Tensor[]? images = null, GenerationConfig? config = null) => GenerateWithHistory(history, images, config);

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        [Obsolete("OpenVINO GenAI deprecated stateful chat mode. Use generate_with_history(...) instead.")]
        public void start_chat() => StartChat();

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        [Obsolete("OpenVINO GenAI deprecated stateful chat mode. Use generate_with_history(...) instead.")]
        public void finish_chat() => FinishChat();

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig get_generation_config() => GetGenerationConfig();

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public void set_generation_config(GenerationConfig config) => SetGenerationConfig(config);

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
                            return GenAINativeMethods.ov_genai_vlm_pipeline_create(modelPtr, devicePtr, argCount, ref _ptr);
                        case 2:
                            return GenAINativeMethods.ov_genai_vlm_pipeline_create(modelPtr, devicePtr, argCount, ref _ptr, propertyPtrs[0], propertyPtrs[1]);
                        case 4:
                            return GenAINativeMethods.ov_genai_vlm_pipeline_create(modelPtr, devicePtr, argCount, ref _ptr, propertyPtrs[0], propertyPtrs[1], propertyPtrs[2], propertyPtrs[3]);
                        case 6:
                            return GenAINativeMethods.ov_genai_vlm_pipeline_create(modelPtr, devicePtr, argCount, ref _ptr, propertyPtrs[0], propertyPtrs[1], propertyPtrs[2], propertyPtrs[3], propertyPtrs[4], propertyPtrs[5]);
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

        private VLMDecodedResults GenerateCore(string prompt, Tensor[]? images, GenerationConfig? config, Func<string, StreamingStatus>? streamer)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(prompt))
                throw new ArgumentException("Prompt cannot be null or empty. / prompt 不能为空。", nameof(prompt));

            IntPtr resultsPtr = IntPtr.Zero;
            IntPtr configPtr = config == null ? IntPtr.Zero : config.OvPtr;

            WithImageTensorPointers(images, (imageArrayPtr, imageCount) =>
            {
                if (streamer == null)
                {
                    ExceptionStatus status = StringUtils.WithUtf8Ptr(
                        prompt,
                        promptPtr => GenAINativeMethods.ov_genai_vlm_pipeline_generate(_ptr, promptPtr, imageArrayPtr, imageCount, configPtr, IntPtr.Zero, ref resultsPtr));
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
                        promptPtr => GenAINativeMethods.ov_genai_vlm_pipeline_generate(_ptr, promptPtr, imageArrayPtr, imageCount, configPtr, ref callback, ref resultsPtr));
                    GC.KeepAlive(nativeCallback);
                    ExceptionHandler.ThrowOnError(status);
                }
            });

            return new VLMDecodedResults(resultsPtr);
        }

        private VLMDecodedResults GenerateWithHistoryCore(ChatHistory history, Tensor[]? images, GenerationConfig? config, Func<string, StreamingStatus>? streamer)
        {
            ThrowIfDisposed();
            if (history == null)
                throw new ArgumentNullException(nameof(history));

            IntPtr resultsPtr = IntPtr.Zero;
            IntPtr configPtr = config == null ? IntPtr.Zero : config.OvPtr;

            WithImageTensorPointers(images, (imageArrayPtr, imageCount) =>
            {
                if (streamer == null)
                {
                    ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_vlm_pipeline_generate_with_history(
                        _ptr,
                        history.OvPtr,
                        imageArrayPtr,
                        imageCount,
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

                    ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_vlm_pipeline_generate_with_history(
                        _ptr,
                        history.OvPtr,
                        imageArrayPtr,
                        imageCount,
                        configPtr,
                        ref callback,
                        ref resultsPtr));
                    GC.KeepAlive(nativeCallback);
                }
            });

            return new VLMDecodedResults(resultsPtr);
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

        private static void WithImageTensorPointers(Tensor[]? images, Action<IntPtr, UIntPtr> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (images == null || images.Length == 0)
            {
                action(IntPtr.Zero, UIntPtr.Zero);
                return;
            }

            IntPtr[] tensorPtrs = new IntPtr[images.Length];
            for (int i = 0; i < images.Length; i++)
            {
                if (images[i] == null)
                    throw new ArgumentException("Image tensor entries cannot be null. / 图像 Tensor 元素不能为空。", nameof(images));
                tensorPtrs[i] = images[i].OvPtr;
            }

            GCHandle handle = default(GCHandle);
            try
            {
                handle = GCHandle.Alloc(tensorPtrs, GCHandleType.Pinned);
                action(handle.AddrOfPinnedObject(), StringUtils.ToNativeSize((ulong)tensorPtrs.Length));
            }
            finally
            {
                if (handle.IsAllocated)
                    handle.Free();
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
