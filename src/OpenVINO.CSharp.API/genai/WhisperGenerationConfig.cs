// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using OpenVinoSharp.Internal;
using OpenVinoSharp.native;

namespace OpenVinoSharp.GenAI
{
    /// <summary>
    /// OpenVINO GenAI Whisper 生成配置 / Whisper generation configuration for OpenVINO GenAI.
    /// <para>
    /// 该类型拥有并释放 <c>ov_genai_whisper_generation_config</c> 原生句柄；仅在创建或调用 GenAI API 时加载
    /// <c>openvino_genai_c</c>，不会影响只使用基础 OpenVINO API 的应用。
    /// This type owns and releases the native <c>ov_genai_whisper_generation_config</c> handle; it loads
    /// <c>openvino_genai_c</c> only when GenAI APIs are used, so core OpenVINO-only applications remain compatible.
    /// </para>
    /// </summary>
    public class WhisperGenerationConfig : DisposableOvObject
    {
        /// <summary>
        /// 创建默认 Whisper 生成配置 / Creates a default Whisper generation configuration.
        /// </summary>
        public WhisperGenerationConfig()
        {
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_whisper_generation_config_create(ref _ptr));
        }

        /// <summary>
        /// 从 JSON 文件创建 Whisper 生成配置 / Creates a Whisper generation configuration from a JSON file.
        /// </summary>
        /// <param name="jsonPath">JSON 配置文件路径 / Path to the JSON configuration file.</param>
        public WhisperGenerationConfig(string jsonPath)
        {
            if (string.IsNullOrEmpty(jsonPath))
                throw new ArgumentException("JSON path cannot be null or empty. / JSON 路径不能为空。", nameof(jsonPath));

            ExceptionHandler.ThrowOnError(StringUtils.WithUtf8Ptr(
                jsonPath,
                pathPtr => GenAINativeMethods.ov_genai_whisper_generation_config_create_from_json(pathPtr, ref _ptr)));
        }

        internal WhisperGenerationConfig(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// 从 JSON 文件创建 Whisper 生成配置 / Creates a Whisper generation configuration from a JSON file.
        /// </summary>
        /// <param name="jsonPath">JSON 配置文件路径 / Path to the JSON configuration file.</param>
        public static WhisperGenerationConfig FromJson(string jsonPath)
        {
            return new WhisperGenerationConfig(jsonPath);
        }

        /// <summary>
        /// 释放原生 Whisper 生成配置 / Releases the native Whisper generation configuration.
        /// </summary>
        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero)
            {
                GenAINativeMethods.ov_genai_whisper_generation_config_free(_ptr);
                _ptr = IntPtr.Zero;
            }
            base.DisposeUnmanaged();
        }

        /// <summary>
        /// 获取底层文本生成配置的拥有副本 / Gets an owned copy of the underlying text generation configuration.
        /// </summary>
        /// <remarks>
        /// 返回的 <see cref="GenerationConfig"/> 是新创建的原生对象，由调用方负责释放。
        /// The returned <see cref="GenerationConfig"/> wraps a newly created native object owned by the caller.
        /// </remarks>
        public GenerationConfig GetGenerationConfig()
        {
            ThrowIfDisposed();
            IntPtr configPtr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_whisper_generation_config_get_generation_config(_ptr, ref configPtr));
            return new GenerationConfig(configPtr);
        }

        /// <summary>
        /// 设置 decoder start token id / Sets the decoder start token id.
        /// </summary>
        public WhisperGenerationConfig SetDecoderStartTokenId(long tokenId) => SetTokenId(tokenId, GenAINativeMethods.ov_genai_whisper_generation_config_set_decoder_start_token_id);

        /// <summary>
        /// 获取 decoder start token id / Gets the decoder start token id.
        /// </summary>
        public long GetDecoderStartTokenId() => GetTokenId(GenAINativeMethods.ov_genai_whisper_generation_config_get_decoder_start_token_id);

        /// <summary>
        /// Decoder start token id / Decoder start token id.
        /// </summary>
        public long DecoderStartTokenId
        {
            get { return GetDecoderStartTokenId(); }
            set { SetDecoderStartTokenId(value); }
        }

        /// <summary>
        /// 设置 padding token id / Sets the padding token id.
        /// </summary>
        public WhisperGenerationConfig SetPadTokenId(long tokenId) => SetTokenId(tokenId, GenAINativeMethods.ov_genai_whisper_generation_config_set_pad_token_id);

        /// <summary>
        /// 获取 padding token id / Gets the padding token id.
        /// </summary>
        public long GetPadTokenId() => GetTokenId(GenAINativeMethods.ov_genai_whisper_generation_config_get_pad_token_id);

        /// <summary>
        /// Padding token id / Padding token id.
        /// </summary>
        public long PadTokenId
        {
            get { return GetPadTokenId(); }
            set { SetPadTokenId(value); }
        }

        /// <summary>
        /// 设置 translate token id / Sets the translate token id.
        /// </summary>
        public WhisperGenerationConfig SetTranslateTokenId(long tokenId) => SetTokenId(tokenId, GenAINativeMethods.ov_genai_whisper_generation_config_set_translate_token_id);

        /// <summary>
        /// 获取 translate token id / Gets the translate token id.
        /// </summary>
        public long GetTranslateTokenId() => GetTokenId(GenAINativeMethods.ov_genai_whisper_generation_config_get_translate_token_id);

        /// <summary>
        /// Translate token id / Translate token id.
        /// </summary>
        public long TranslateTokenId
        {
            get { return GetTranslateTokenId(); }
            set { SetTranslateTokenId(value); }
        }

        /// <summary>
        /// 设置 transcribe token id / Sets the transcribe token id.
        /// </summary>
        public WhisperGenerationConfig SetTranscribeTokenId(long tokenId) => SetTokenId(tokenId, GenAINativeMethods.ov_genai_whisper_generation_config_set_transcribe_token_id);

        /// <summary>
        /// 获取 transcribe token id / Gets the transcribe token id.
        /// </summary>
        public long GetTranscribeTokenId() => GetTokenId(GenAINativeMethods.ov_genai_whisper_generation_config_get_transcribe_token_id);

        /// <summary>
        /// Transcribe token id / Transcribe token id.
        /// </summary>
        public long TranscribeTokenId
        {
            get { return GetTranscribeTokenId(); }
            set { SetTranscribeTokenId(value); }
        }

        /// <summary>
        /// 设置 previous start-of-transcript token id / Sets the previous start-of-transcript token id.
        /// </summary>
        public WhisperGenerationConfig SetPrevSotTokenId(long tokenId) => SetTokenId(tokenId, GenAINativeMethods.ov_genai_whisper_generation_config_set_prev_sot_token_id);

        /// <summary>
        /// 获取 previous start-of-transcript token id / Gets the previous start-of-transcript token id.
        /// </summary>
        public long GetPrevSotTokenId() => GetTokenId(GenAINativeMethods.ov_genai_whisper_generation_config_get_prev_sot_token_id);

        /// <summary>
        /// Previous start-of-transcript token id / Previous start-of-transcript token id.
        /// </summary>
        public long PrevSotTokenId
        {
            get { return GetPrevSotTokenId(); }
            set { SetPrevSotTokenId(value); }
        }

        /// <summary>
        /// 设置 no-timestamps token id / Sets the no-timestamps token id.
        /// </summary>
        public WhisperGenerationConfig SetNoTimestampsTokenId(long tokenId) => SetTokenId(tokenId, GenAINativeMethods.ov_genai_whisper_generation_config_set_no_timestamps_token_id);

        /// <summary>
        /// 获取 no-timestamps token id / Gets the no-timestamps token id.
        /// </summary>
        public long GetNoTimestampsTokenId() => GetTokenId(GenAINativeMethods.ov_genai_whisper_generation_config_get_no_timestamps_token_id);

        /// <summary>
        /// No-timestamps token id / No-timestamps token id.
        /// </summary>
        public long NoTimestampsTokenId
        {
            get { return GetNoTimestampsTokenId(); }
            set { SetNoTimestampsTokenId(value); }
        }

        /// <summary>
        /// 设置最大初始时间戳索引 / Sets the maximum initial timestamp index.
        /// </summary>
        public WhisperGenerationConfig SetMaxInitialTimestampIndex(ulong index) => SetSize(index, GenAINativeMethods.ov_genai_whisper_generation_config_set_max_initial_timestamp_index);

        /// <summary>
        /// 获取最大初始时间戳索引 / Gets the maximum initial timestamp index.
        /// </summary>
        public ulong GetMaxInitialTimestampIndex() => GetSize(GenAINativeMethods.ov_genai_whisper_generation_config_get_max_initial_timestamp_index);

        /// <summary>
        /// 最大初始时间戳索引 / Maximum initial timestamp index.
        /// </summary>
        public ulong MaxInitialTimestampIndex
        {
            get { return GetMaxInitialTimestampIndex(); }
            set { SetMaxInitialTimestampIndex(value); }
        }

        /// <summary>
        /// 设置模型是否为多语言模型 / Sets whether the model is multilingual.
        /// </summary>
        public WhisperGenerationConfig SetIsMultilingual(bool value) => SetBool(value, GenAINativeMethods.ov_genai_whisper_generation_config_set_is_multilingual);

        /// <summary>
        /// 获取模型是否为多语言模型 / Gets whether the model is multilingual.
        /// </summary>
        public bool GetIsMultilingual() => GetBool(GenAINativeMethods.ov_genai_whisper_generation_config_get_is_multilingual);

        /// <summary>
        /// 模型是否为多语言模型 / Whether the model is multilingual.
        /// </summary>
        public bool IsMultilingual
        {
            get { return GetIsMultilingual(); }
            set { SetIsMultilingual(value); }
        }

        /// <summary>
        /// 设置语言 token，传入 null 表示取消设置 / Sets the language token; pass null to unset it.
        /// </summary>
        public WhisperGenerationConfig SetLanguage(string? language) => SetOptionalString(language, GenAINativeMethods.ov_genai_whisper_generation_config_set_language);

        /// <summary>
        /// 获取语言 token；未设置时返回 null / Gets the language token; returns null when it is not set.
        /// </summary>
        public string? GetLanguage() => GetOptionalString(GenAINativeMethods.ov_genai_whisper_generation_config_get_language);

        /// <summary>
        /// 语言 token；设置为 null 可取消设置 / Language token; set to null to unset.
        /// </summary>
        public string? Language
        {
            get { return GetLanguage(); }
            set { SetLanguage(value); }
        }

        /// <summary>
        /// 设置任务，通常为 translate 或 transcribe；传入 null 表示取消设置 / Sets the task, usually translate or transcribe; pass null to unset it.
        /// </summary>
        public WhisperGenerationConfig SetTask(string? task) => SetOptionalString(task, GenAINativeMethods.ov_genai_whisper_generation_config_set_task);

        /// <summary>
        /// 获取任务；未设置时返回 null / Gets the task; returns null when it is not set.
        /// </summary>
        public string? GetTask() => GetOptionalString(GenAINativeMethods.ov_genai_whisper_generation_config_get_task);

        /// <summary>
        /// Whisper 任务；设置为 null 可取消设置 / Whisper task; set to null to unset.
        /// </summary>
        public string? Task
        {
            get { return GetTask(); }
            set { SetTask(value); }
        }

        /// <summary>
        /// 设置是否返回时间戳 / Sets whether timestamps should be returned.
        /// </summary>
        public WhisperGenerationConfig SetReturnTimestamps(bool value) => SetBool(value, GenAINativeMethods.ov_genai_whisper_generation_config_set_return_timestamps);

        /// <summary>
        /// 获取是否返回时间戳 / Gets whether timestamps should be returned.
        /// </summary>
        public bool GetReturnTimestamps() => GetBool(GenAINativeMethods.ov_genai_whisper_generation_config_get_return_timestamps);

        /// <summary>
        /// 是否返回时间戳 / Whether timestamps should be returned.
        /// </summary>
        public bool ReturnTimestamps
        {
            get { return GetReturnTimestamps(); }
            set { SetReturnTimestamps(value); }
        }

        /// <summary>
        /// 设置初始提示词，传入 null 表示取消设置 / Sets the initial prompt; pass null to unset it.
        /// </summary>
        public WhisperGenerationConfig SetInitialPrompt(string? initialPrompt) => SetOptionalString(initialPrompt, GenAINativeMethods.ov_genai_whisper_generation_config_set_initial_prompt);

        /// <summary>
        /// 获取初始提示词；未设置时返回 null / Gets the initial prompt; returns null when it is not set.
        /// </summary>
        public string? GetInitialPrompt() => GetOptionalString(GenAINativeMethods.ov_genai_whisper_generation_config_get_initial_prompt);

        /// <summary>
        /// 初始提示词；设置为 null 可取消设置 / Initial prompt; set to null to unset.
        /// </summary>
        public string? InitialPrompt
        {
            get { return GetInitialPrompt(); }
            set { SetInitialPrompt(value); }
        }

        /// <summary>
        /// 设置热词文本，传入 null 表示取消设置 / Sets hotwords text; pass null to unset it.
        /// </summary>
        public WhisperGenerationConfig SetHotwords(string? hotwords) => SetOptionalString(hotwords, GenAINativeMethods.ov_genai_whisper_generation_config_set_hotwords);

        /// <summary>
        /// 获取热词文本；未设置时返回 null / Gets hotwords text; returns null when it is not set.
        /// </summary>
        public string? GetHotwords() => GetOptionalString(GenAINativeMethods.ov_genai_whisper_generation_config_get_hotwords);

        /// <summary>
        /// 热词文本；设置为 null 可取消设置 / Hotwords text; set to null to unset.
        /// </summary>
        public string? Hotwords
        {
            get { return GetHotwords(); }
            set { SetHotwords(value); }
        }

        /// <summary>
        /// 设置开头阶段要抑制的 token id 列表 / Sets token ids suppressed at the beginning.
        /// </summary>
        public WhisperGenerationConfig SetBeginSuppressTokens(IEnumerable<long> tokens) => SetTokenArray(tokens, GenAINativeMethods.ov_genai_whisper_generation_config_set_begin_suppress_tokens);

        /// <summary>
        /// 设置开头阶段要抑制的 token id 列表 / Sets token ids suppressed at the beginning.
        /// </summary>
        public WhisperGenerationConfig SetBeginSuppressTokens(params long[] tokens) => SetBeginSuppressTokens((IEnumerable<long>)tokens);

        /// <summary>
        /// 获取开头阶段要抑制的 token id 数量 / Gets the count of token ids suppressed at the beginning.
        /// </summary>
        public ulong GetBeginSuppressTokensCount() => GetTokenArrayCount(GenAINativeMethods.ov_genai_whisper_generation_config_get_begin_suppress_tokens_count);

        /// <summary>
        /// 获取开头阶段要抑制的 token id 列表 / Gets token ids suppressed at the beginning.
        /// </summary>
        public long[] GetBeginSuppressTokens() => GetTokenArray(
            GenAINativeMethods.ov_genai_whisper_generation_config_get_begin_suppress_tokens_count,
            GenAINativeMethods.ov_genai_whisper_generation_config_get_begin_suppress_tokens);

        /// <summary>
        /// 开头阶段要抑制的 token id 列表 / Token ids suppressed at the beginning.
        /// </summary>
        public long[] BeginSuppressTokens
        {
            get { return GetBeginSuppressTokens(); }
            set { SetBeginSuppressTokens(value); }
        }

        /// <summary>
        /// 设置生成期间要抑制的 token id 列表 / Sets token ids suppressed during generation.
        /// </summary>
        public WhisperGenerationConfig SetSuppressTokens(IEnumerable<long> tokens) => SetTokenArray(tokens, GenAINativeMethods.ov_genai_whisper_generation_config_set_suppress_tokens);

        /// <summary>
        /// 设置生成期间要抑制的 token id 列表 / Sets token ids suppressed during generation.
        /// </summary>
        public WhisperGenerationConfig SetSuppressTokens(params long[] tokens) => SetSuppressTokens((IEnumerable<long>)tokens);

        /// <summary>
        /// 获取生成期间要抑制的 token id 数量 / Gets the count of token ids suppressed during generation.
        /// </summary>
        public ulong GetSuppressTokensCount() => GetTokenArrayCount(GenAINativeMethods.ov_genai_whisper_generation_config_get_suppress_tokens_count);

        /// <summary>
        /// 获取生成期间要抑制的 token id 列表 / Gets token ids suppressed during generation.
        /// </summary>
        public long[] GetSuppressTokens() => GetTokenArray(
            GenAINativeMethods.ov_genai_whisper_generation_config_get_suppress_tokens_count,
            GenAINativeMethods.ov_genai_whisper_generation_config_get_suppress_tokens);

        /// <summary>
        /// 生成期间要抑制的 token id 列表 / Token ids suppressed during generation.
        /// </summary>
        public long[] SuppressTokens
        {
            get { return GetSuppressTokens(); }
            set { SetSuppressTokens(value); }
        }

        /// <summary>
        /// 验证 Whisper 生成配置是否存在冲突 / Validates the Whisper generation configuration for conflicts.
        /// </summary>
        public void Validate()
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_whisper_generation_config_validate(_ptr));
        }

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public static WhisperGenerationConfig from_json(string jsonPath) => FromJson(jsonPath);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig get_generation_config() => GetGenerationConfig();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public WhisperGenerationConfig set_decoder_start_token_id(long tokenId) => SetDecoderStartTokenId(tokenId);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public long get_decoder_start_token_id() => GetDecoderStartTokenId();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public WhisperGenerationConfig set_pad_token_id(long tokenId) => SetPadTokenId(tokenId);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public long get_pad_token_id() => GetPadTokenId();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public WhisperGenerationConfig set_translate_token_id(long tokenId) => SetTranslateTokenId(tokenId);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public long get_translate_token_id() => GetTranslateTokenId();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public WhisperGenerationConfig set_transcribe_token_id(long tokenId) => SetTranscribeTokenId(tokenId);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public long get_transcribe_token_id() => GetTranscribeTokenId();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public WhisperGenerationConfig set_prev_sot_token_id(long tokenId) => SetPrevSotTokenId(tokenId);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public long get_prev_sot_token_id() => GetPrevSotTokenId();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public WhisperGenerationConfig set_no_timestamps_token_id(long tokenId) => SetNoTimestampsTokenId(tokenId);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public long get_no_timestamps_token_id() => GetNoTimestampsTokenId();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public WhisperGenerationConfig set_max_initial_timestamp_index(ulong index) => SetMaxInitialTimestampIndex(index);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public ulong get_max_initial_timestamp_index() => GetMaxInitialTimestampIndex();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public WhisperGenerationConfig set_is_multilingual(bool value) => SetIsMultilingual(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public bool get_is_multilingual() => GetIsMultilingual();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public WhisperGenerationConfig set_language(string? language) => SetLanguage(language);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public string? get_language() => GetLanguage();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public WhisperGenerationConfig set_task(string? task) => SetTask(task);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public string? get_task() => GetTask();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public WhisperGenerationConfig set_return_timestamps(bool value) => SetReturnTimestamps(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public bool get_return_timestamps() => GetReturnTimestamps();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public WhisperGenerationConfig set_initial_prompt(string? initialPrompt) => SetInitialPrompt(initialPrompt);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public string? get_initial_prompt() => GetInitialPrompt();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public WhisperGenerationConfig set_hotwords(string? hotwords) => SetHotwords(hotwords);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public string? get_hotwords() => GetHotwords();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public WhisperGenerationConfig set_begin_suppress_tokens(params long[] tokens) => SetBeginSuppressTokens(tokens);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public ulong get_begin_suppress_tokens_count() => GetBeginSuppressTokensCount();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public long[] get_begin_suppress_tokens() => GetBeginSuppressTokens();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public WhisperGenerationConfig set_suppress_tokens(params long[] tokens) => SetSuppressTokens(tokens);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public ulong get_suppress_tokens_count() => GetSuppressTokensCount();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public long[] get_suppress_tokens() => GetSuppressTokens();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public void validate() => Validate();

        private delegate ExceptionStatus TokenIdSetter(IntPtr config, long tokenId);
        private delegate ExceptionStatus TokenIdGetter(IntPtr config, ref long tokenId);
        private delegate ExceptionStatus SizeSetter(IntPtr config, UIntPtr value);
        private delegate ExceptionStatus SizeGetter(IntPtr config, ref UIntPtr value);
        private delegate ExceptionStatus BoolSetter(IntPtr config, bool value);
        private delegate ExceptionStatus BoolGetter(IntPtr config, ref byte value);
        private delegate ExceptionStatus StringSetter(IntPtr config, IntPtr value);
        private delegate ExceptionStatus OptionalStringGetter(IntPtr config, IntPtr output, ref UIntPtr outputSize);
        private delegate ExceptionStatus TokenArraySetter(IntPtr config, IntPtr tokens, UIntPtr count);
        private delegate ExceptionStatus TokenArrayCountGetter(IntPtr config, ref UIntPtr count);
        private delegate ExceptionStatus TokenArrayGetter(IntPtr config, IntPtr tokens, UIntPtr count);

        private WhisperGenerationConfig SetTokenId(long tokenId, TokenIdSetter setter)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(setter(_ptr, tokenId));
            return this;
        }

        private long GetTokenId(TokenIdGetter getter)
        {
            ThrowIfDisposed();
            long tokenId = 0;
            ExceptionHandler.ThrowOnError(getter(_ptr, ref tokenId));
            return tokenId;
        }

        private WhisperGenerationConfig SetSize(ulong value, SizeSetter setter)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(setter(_ptr, StringUtils.ToNativeSize(value)));
            return this;
        }

        private ulong GetSize(SizeGetter getter)
        {
            ThrowIfDisposed();
            UIntPtr value = UIntPtr.Zero;
            ExceptionHandler.ThrowOnError(getter(_ptr, ref value));
            return StringUtils.FromNativeSize(value);
        }

        private WhisperGenerationConfig SetBool(bool value, BoolSetter setter)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(setter(_ptr, value));
            return this;
        }

        private bool GetBool(BoolGetter getter)
        {
            ThrowIfDisposed();
            byte value = 0;
            ExceptionHandler.ThrowOnError(getter(_ptr, ref value));
            return value != 0;
        }

        private WhisperGenerationConfig SetOptionalString(string? value, StringSetter setter)
        {
            ThrowIfDisposed();
            if (value == null)
            {
                ExceptionHandler.ThrowOnError(setter(_ptr, IntPtr.Zero));
                return this;
            }

            ExceptionHandler.ThrowOnError(StringUtils.WithUtf8Ptr(value, valuePtr => setter(_ptr, valuePtr)));
            return this;
        }

        private string? GetOptionalString(OptionalStringGetter getter)
        {
            ThrowIfDisposed();
            UIntPtr nativeSize = UIntPtr.Zero;
            ExceptionStatus status = getter(_ptr, IntPtr.Zero, ref nativeSize);
            if (status == ExceptionStatus.NOT_FOUND)
                return null;

            ExceptionHandler.ThrowOnError(status);
            ulong size = StringUtils.FromNativeSize(nativeSize);
            if (size == 0)
                return string.Empty;
            if (size > int.MaxValue)
                throw new OverflowException("Native string is too large for a managed buffer. / 原生字符串过大，无法放入托管缓冲区。");

            IntPtr buffer = Marshal.AllocHGlobal((int)size);
            try
            {
                UIntPtr writableSize = StringUtils.ToNativeSize(size);
                status = getter(_ptr, buffer, ref writableSize);
                if (status == ExceptionStatus.NOT_FOUND)
                    return null;

                ExceptionHandler.ThrowOnError(status);
                return StringUtils.Utf8PtrToString(buffer) ?? string.Empty;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        private WhisperGenerationConfig SetTokenArray(IEnumerable<long> tokens, TokenArraySetter setter)
        {
            ThrowIfDisposed();
            if (tokens == null)
                throw new ArgumentNullException(nameof(tokens));

            long[] values = tokens.ToArray();
            GCHandle handle = default(GCHandle);
            try
            {
                IntPtr tokenPtr = IntPtr.Zero;
                if (values.Length > 0)
                {
                    handle = GCHandle.Alloc(values, GCHandleType.Pinned);
                    tokenPtr = handle.AddrOfPinnedObject();
                }

                ExceptionHandler.ThrowOnError(setter(_ptr, tokenPtr, StringUtils.ToNativeSize((ulong)values.Length)));
            }
            finally
            {
                if (handle.IsAllocated)
                    handle.Free();
            }

            return this;
        }

        private ulong GetTokenArrayCount(TokenArrayCountGetter getter)
        {
            ThrowIfDisposed();
            UIntPtr count = UIntPtr.Zero;
            ExceptionHandler.ThrowOnError(getter(_ptr, ref count));
            return StringUtils.FromNativeSize(count);
        }

        private long[] GetTokenArray(TokenArrayCountGetter countGetter, TokenArrayGetter getter)
        {
            ThrowIfDisposed();
            UIntPtr nativeCount = UIntPtr.Zero;
            ExceptionHandler.ThrowOnError(countGetter(_ptr, ref nativeCount));
            ulong count = StringUtils.FromNativeSize(nativeCount);
            if (count == 0)
                return new long[0];
            if (count > int.MaxValue)
                throw new OverflowException("Native token array is too large for a managed array. / 原生 token 数组过大，无法放入托管数组。");

            long[] values = new long[(int)count];
            GCHandle handle = GCHandle.Alloc(values, GCHandleType.Pinned);
            try
            {
                ExceptionHandler.ThrowOnError(getter(_ptr, handle.AddrOfPinnedObject(), nativeCount));
                return values;
            }
            finally
            {
                handle.Free();
            }
        }
    }
}
