// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using OpenVinoSharp.Internal;
using OpenVinoSharp.native;

namespace OpenVinoSharp.GenAI
{
    /// <summary>
    /// OpenVINO GenAI 文本生成配置 / Text generation configuration for OpenVINO GenAI.
    /// <para>
    /// 该类包装 <c>ov_genai_generation_config</c>，用于控制采样、beam search、停止词和输出长度等参数。
    /// This class wraps <c>ov_genai_generation_config</c> and controls sampling, beam search, stop strings, and output length.
    /// </para>
    /// </summary>
    public class GenerationConfig : DisposableOvObject
    {
        /// <summary>
        /// 创建默认生成配置 / Creates a default generation configuration.
        /// </summary>
        public GenerationConfig()
        {
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_generation_config_create(ref _ptr));
        }

        /// <summary>
        /// 从 JSON 文件创建生成配置 / Creates a generation configuration from a JSON file.
        /// </summary>
        /// <param name="jsonPath">JSON 配置文件路径 / Path to the JSON configuration file.</param>
        public GenerationConfig(string jsonPath)
        {
            if (string.IsNullOrEmpty(jsonPath))
                throw new ArgumentException("JSON path cannot be null or empty. / JSON 路径不能为空。", nameof(jsonPath));

            ExceptionHandler.ThrowOnError(StringUtils.WithUtf8Ptr(
                jsonPath,
                pathPtr => GenAINativeMethods.ov_genai_generation_config_create_from_json(pathPtr, ref _ptr)));
        }

        internal GenerationConfig(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// 从 JSON 文件创建生成配置 / Creates a generation configuration from a JSON file.
        /// </summary>
        public static GenerationConfig FromJson(string jsonPath)
        {
            return new GenerationConfig(jsonPath);
        }

        /// <summary>
        /// 释放原生生成配置 / Releases the native generation configuration.
        /// </summary>
        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero)
            {
                GenAINativeMethods.ov_genai_generation_config_free(_ptr);
                _ptr = IntPtr.Zero;
            }
            base.DisposeUnmanaged();
        }

        /// <summary>
        /// 设置最大新增 token 数 / Sets the maximum number of new tokens.
        /// </summary>
        public GenerationConfig SetMaxNewTokens(ulong value)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_generation_config_set_max_new_tokens(_ptr, StringUtils.ToNativeSize(value)));
            return this;
        }

        /// <summary>
        /// 获取最大新增 token 数 / Gets the maximum number of new tokens.
        /// </summary>
        public ulong GetMaxNewTokens()
        {
            ThrowIfDisposed();
            UIntPtr value = UIntPtr.Zero;
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_generation_config_get_max_new_tokens(_ptr, ref value));
            return StringUtils.FromNativeSize(value);
        }

        /// <summary>
        /// 最大新增 token 数 / Maximum number of new tokens.
        /// </summary>
        public ulong MaxNewTokens
        {
            get { return GetMaxNewTokens(); }
            set { SetMaxNewTokens(value); }
        }

        /// <summary>
        /// 设置总生成长度上限 / Sets the maximum total token length.
        /// </summary>
        public GenerationConfig SetMaxLength(ulong value) => SetSizeValue(value, GenAINativeMethods.ov_genai_generation_config_set_max_length);

        /// <summary>
        /// 设置是否忽略 EOS token / Sets whether EOS token should be ignored.
        /// </summary>
        public GenerationConfig SetIgnoreEos(bool value) => SetBoolValue(value, GenAINativeMethods.ov_genai_generation_config_set_ignore_eos);

        /// <summary>
        /// 设置最小新增 token 数 / Sets the minimum number of new tokens.
        /// </summary>
        public GenerationConfig SetMinNewTokens(ulong value) => SetSizeValue(value, GenAINativeMethods.ov_genai_generation_config_set_min_new_tokens);

        /// <summary>
        /// 设置输出是否包含输入 prompt / Sets whether the output should include the input prompt.
        /// </summary>
        public GenerationConfig SetEcho(bool value) => SetBoolValue(value, GenAINativeMethods.ov_genai_generation_config_set_echo);

        /// <summary>
        /// 设置 logprobs 个数 / Sets the number of logprobs.
        /// </summary>
        public GenerationConfig SetLogProbs(ulong value) => SetSizeValue(value, GenAINativeMethods.ov_genai_generation_config_set_logprobs);

        /// <summary>
        /// 设置停止字符串 / Sets stop strings.
        /// </summary>
        /// <param name="strings">停止字符串集合 / Stop string collection.</param>
        public GenerationConfig SetStopStrings(IEnumerable<string> strings)
        {
            ThrowIfDisposed();
            if (strings == null)
                throw new ArgumentNullException(nameof(strings));

            string[] values = strings.ToArray();
            IntPtr[] stringPtrs = StringUtils.StringArrayToUtf8PtrArray(values);
            GCHandle handle = default(GCHandle);
            try
            {
                IntPtr arrayPtr = IntPtr.Zero;
                if (stringPtrs.Length > 0)
                {
                    handle = GCHandle.Alloc(stringPtrs, GCHandleType.Pinned);
                    arrayPtr = handle.AddrOfPinnedObject();
                }

                ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_generation_config_set_stop_strings(
                    _ptr,
                    arrayPtr,
                    StringUtils.ToNativeSize((ulong)stringPtrs.Length)));
            }
            finally
            {
                if (handle.IsAllocated)
                    handle.Free();
                StringUtils.FreeUtf8PtrArray(stringPtrs);
            }

            return this;
        }

        /// <summary>
        /// 设置停止字符串 / Sets stop strings.
        /// </summary>
        public GenerationConfig SetStopStrings(params string[] strings)
        {
            return SetStopStrings((IEnumerable<string>)strings);
        }

        /// <summary>
        /// 设置是否把匹配到的停止字符串包含在输出中 / Sets whether matched stop strings are included in output.
        /// </summary>
        public GenerationConfig SetIncludeStopStringInOutput(bool value) => SetBoolValue(value, GenAINativeMethods.ov_genai_generation_config_set_include_stop_str_in_output);

        /// <summary>
        /// 设置停止 token id 集合 / Sets stop token ids.
        /// </summary>
        public GenerationConfig SetStopTokenIds(IEnumerable<long> tokenIds)
        {
            ThrowIfDisposed();
            if (tokenIds == null)
                throw new ArgumentNullException(nameof(tokenIds));

            long[] values = tokenIds.ToArray();
            GCHandle handle = default(GCHandle);
            try
            {
                IntPtr tokenPtr = IntPtr.Zero;
                if (values.Length > 0)
                {
                    handle = GCHandle.Alloc(values, GCHandleType.Pinned);
                    tokenPtr = handle.AddrOfPinnedObject();
                }

                ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_generation_config_set_stop_token_ids(
                    _ptr,
                    tokenPtr,
                    StringUtils.ToNativeSize((ulong)values.Length)));
            }
            finally
            {
                if (handle.IsAllocated)
                    handle.Free();
            }

            return this;
        }

        /// <summary>
        /// 设置停止 token id 集合 / Sets stop token ids.
        /// </summary>
        public GenerationConfig SetStopTokenIds(params long[] tokenIds)
        {
            return SetStopTokenIds((IEnumerable<long>)tokenIds);
        }

        /// <summary>
        /// 设置 beam 分组数 / Sets the number of beam groups.
        /// </summary>
        public GenerationConfig SetNumBeamGroups(ulong value) => SetSizeValue(value, GenAINativeMethods.ov_genai_generation_config_set_num_beam_groups);

        /// <summary>
        /// 设置 beam 数 / Sets the number of beams.
        /// </summary>
        public GenerationConfig SetNumBeams(ulong value) => SetSizeValue(value, GenAINativeMethods.ov_genai_generation_config_set_num_beams);

        /// <summary>
        /// 设置多样性惩罚 / Sets diversity penalty.
        /// </summary>
        public GenerationConfig SetDiversityPenalty(float value) => SetFloatValue(value, GenAINativeMethods.ov_genai_generation_config_set_diversity_penalty);

        /// <summary>
        /// 设置长度惩罚 / Sets length penalty.
        /// </summary>
        public GenerationConfig SetLengthPenalty(float value) => SetFloatValue(value, GenAINativeMethods.ov_genai_generation_config_set_length_penalty);

        /// <summary>
        /// 设置返回序列数量 / Sets number of returned sequences.
        /// </summary>
        public GenerationConfig SetNumReturnSequences(ulong value) => SetSizeValue(value, GenAINativeMethods.ov_genai_generation_config_set_num_return_sequences);

        /// <summary>
        /// 设置 no-repeat ngram 大小 / Sets no-repeat ngram size.
        /// </summary>
        public GenerationConfig SetNoRepeatNgramSize(ulong value) => SetSizeValue(value, GenAINativeMethods.ov_genai_generation_config_set_no_repeat_ngram_size);

        /// <summary>
        /// 设置停止条件 / Sets stop criteria.
        /// </summary>
        public GenerationConfig SetStopCriteria(StopCriteria value)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_generation_config_set_stop_criteria(_ptr, value));
            return this;
        }

        /// <summary>
        /// 设置温度 / Sets sampling temperature.
        /// </summary>
        public GenerationConfig SetTemperature(float value) => SetFloatValue(value, GenAINativeMethods.ov_genai_generation_config_set_temperature);

        /// <summary>
        /// 设置 top-p / Sets top-p.
        /// </summary>
        public GenerationConfig SetTopP(float value) => SetFloatValue(value, GenAINativeMethods.ov_genai_generation_config_set_top_p);

        /// <summary>
        /// 设置 top-k / Sets top-k.
        /// </summary>
        public GenerationConfig SetTopK(ulong value) => SetSizeValue(value, GenAINativeMethods.ov_genai_generation_config_set_top_k);

        /// <summary>
        /// 设置 min-p / Sets min-p.
        /// </summary>
        public GenerationConfig SetMinP(float value) => SetFloatValue(value, GenAINativeMethods.ov_genai_generation_config_set_min_p);

        /// <summary>
        /// 设置是否启用采样 / Sets whether sampling is enabled.
        /// </summary>
        public GenerationConfig SetDoSample(bool value) => SetBoolValue(value, GenAINativeMethods.ov_genai_generation_config_set_do_sample);

        /// <summary>
        /// 设置重复惩罚 / Sets repetition penalty.
        /// </summary>
        public GenerationConfig SetRepetitionPenalty(float value) => SetFloatValue(value, GenAINativeMethods.ov_genai_generation_config_set_repetition_penalty);

        /// <summary>
        /// 设置 presence penalty / Sets presence penalty.
        /// </summary>
        public GenerationConfig SetPresencePenalty(float value) => SetFloatValue(value, GenAINativeMethods.ov_genai_generation_config_set_presence_penalty);

        /// <summary>
        /// 设置 frequency penalty / Sets frequency penalty.
        /// </summary>
        public GenerationConfig SetFrequencyPenalty(float value) => SetFloatValue(value, GenAINativeMethods.ov_genai_generation_config_set_frequency_penalty);

        /// <summary>
        /// 设置随机种子 / Sets random seed.
        /// </summary>
        public GenerationConfig SetRngSeed(ulong value) => SetSizeValue(value, GenAINativeMethods.ov_genai_generation_config_set_rng_seed);

        /// <summary>
        /// 设置 assistant confidence threshold / Sets assistant confidence threshold.
        /// </summary>
        public GenerationConfig SetAssistantConfidenceThreshold(float value) => SetFloatValue(value, GenAINativeMethods.ov_genai_generation_config_set_assistant_confidence_threshold);

        /// <summary>
        /// 设置 assistant token 数量 / Sets number of assistant tokens.
        /// </summary>
        public GenerationConfig SetNumAssistantTokens(ulong value) => SetSizeValue(value, GenAINativeMethods.ov_genai_generation_config_set_num_assistant_tokens);

        /// <summary>
        /// 设置最大 ngram 大小 / Sets maximum ngram size.
        /// </summary>
        public GenerationConfig SetMaxNgramSize(ulong value) => SetSizeValue(value, GenAINativeMethods.ov_genai_generation_config_set_max_ngram_size);

        /// <summary>
        /// 设置 EOS token id / Sets EOS token id.
        /// </summary>
        public GenerationConfig SetEosTokenId(long id)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_generation_config_set_eos_token_id(_ptr, id));
            return this;
        }

        /// <summary>
        /// 验证配置是否存在冲突 / Validates the configuration for conflicting parameters.
        /// </summary>
        public void Validate()
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(GenAINativeMethods.ov_genai_generation_config_validate(_ptr));
        }

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_max_new_tokens(ulong value) => SetMaxNewTokens(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public ulong get_max_new_tokens() => GetMaxNewTokens();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_max_length(ulong value) => SetMaxLength(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_ignore_eos(bool value) => SetIgnoreEos(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_min_new_tokens(ulong value) => SetMinNewTokens(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_echo(bool value) => SetEcho(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_logprobs(ulong value) => SetLogProbs(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_stop_strings(params string[] strings) => SetStopStrings(strings);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_include_stop_str_in_output(bool value) => SetIncludeStopStringInOutput(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_stop_token_ids(params long[] tokenIds) => SetStopTokenIds(tokenIds);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_num_beam_groups(ulong value) => SetNumBeamGroups(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_num_beams(ulong value) => SetNumBeams(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_diversity_penalty(float value) => SetDiversityPenalty(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_length_penalty(float value) => SetLengthPenalty(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_num_return_sequences(ulong value) => SetNumReturnSequences(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_no_repeat_ngram_size(ulong value) => SetNoRepeatNgramSize(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_stop_criteria(StopCriteria value) => SetStopCriteria(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_temperature(float value) => SetTemperature(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_top_p(float value) => SetTopP(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_top_k(ulong value) => SetTopK(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_min_p(float value) => SetMinP(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_do_sample(bool value) => SetDoSample(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_repetition_penalty(float value) => SetRepetitionPenalty(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_presence_penalty(float value) => SetPresencePenalty(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_frequency_penalty(float value) => SetFrequencyPenalty(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_rng_seed(ulong value) => SetRngSeed(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_assistant_confidence_threshold(float value) => SetAssistantConfidenceThreshold(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_num_assistant_tokens(ulong value) => SetNumAssistantTokens(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_max_ngram_size(ulong value) => SetMaxNgramSize(value);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public GenerationConfig set_eos_token_id(long id) => SetEosTokenId(id);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public void validate() => Validate();

        private delegate ExceptionStatus SizeSetter(IntPtr config, UIntPtr value);
        private delegate ExceptionStatus FloatSetter(IntPtr config, float value);
        private delegate ExceptionStatus BoolSetter(IntPtr config, bool value);

        private GenerationConfig SetSizeValue(ulong value, SizeSetter setter)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(setter(_ptr, StringUtils.ToNativeSize(value)));
            return this;
        }

        private GenerationConfig SetFloatValue(float value, FloatSetter setter)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(setter(_ptr, value));
            return this;
        }

        private GenerationConfig SetBoolValue(bool value, BoolSetter setter)
        {
            ThrowIfDisposed();
            ExceptionHandler.ThrowOnError(setter(_ptr, value));
            return this;
        }
    }
}

