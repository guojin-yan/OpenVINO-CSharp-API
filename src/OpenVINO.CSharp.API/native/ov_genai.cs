// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using System.Runtime.InteropServices;
using OpenVinoSharp.GenAI;

namespace OpenVinoSharp.native
{
    /// <summary>
    /// OpenVINO GenAI C API 的底层 P/Invoke 声明 / Low-level P/Invoke declarations for OpenVINO GenAI C API.
    /// <para>
    /// 所有字符串均以 UTF-8 指针传入，避免不同 .NET 目标框架上的默认 ANSI 编码差异。
    /// All strings are passed as UTF-8 pointers to avoid default ANSI differences across .NET target frameworks.
    /// </para>
    /// </summary>
    internal static class GenAINativeMethods
    {
        private const string GenAILibrary = "openvino_genai_c";

        static GenAINativeMethods()
        {
            GenAINativeLibraryLoader.EnsureLoaded();
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate StreamingStatus ov_genai_streamer_callback_func(IntPtr str, IntPtr args);

        [StructLayout(LayoutKind.Sequential)]
        internal struct streamer_callback
        {
            public IntPtr callback_func;
            public IntPtr args;
        }

        #region Generation config / 生成配置

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_create", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_create(ref IntPtr config);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_create_from_json", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_create_from_json(IntPtr json_path, ref IntPtr config);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_free", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ov_genai_generation_config_free(IntPtr config);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_max_new_tokens", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_max_new_tokens(IntPtr config, UIntPtr value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_max_length", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_max_length(IntPtr config, UIntPtr value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_ignore_eos", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_ignore_eos(IntPtr config, [MarshalAs(UnmanagedType.I1)] bool value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_min_new_tokens", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_min_new_tokens(IntPtr config, UIntPtr value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_echo", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_echo(IntPtr config, [MarshalAs(UnmanagedType.I1)] bool value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_logprobs", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_logprobs(IntPtr config, UIntPtr value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_stop_strings", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_stop_strings(IntPtr config, IntPtr strings, UIntPtr count);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_include_stop_str_in_output", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_include_stop_str_in_output(IntPtr config, [MarshalAs(UnmanagedType.I1)] bool value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_stop_token_ids", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_stop_token_ids(IntPtr config, IntPtr token_ids, UIntPtr token_ids_num);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_num_beam_groups", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_num_beam_groups(IntPtr config, UIntPtr value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_num_beams", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_num_beams(IntPtr config, UIntPtr value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_diversity_penalty", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_diversity_penalty(IntPtr config, float value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_length_penalty", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_length_penalty(IntPtr config, float value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_num_return_sequences", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_num_return_sequences(IntPtr config, UIntPtr value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_no_repeat_ngram_size", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_no_repeat_ngram_size(IntPtr config, UIntPtr value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_stop_criteria", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_stop_criteria(IntPtr config, StopCriteria value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_temperature", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_temperature(IntPtr config, float value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_top_p", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_top_p(IntPtr config, float value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_top_k", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_top_k(IntPtr config, UIntPtr value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_min_p", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_min_p(IntPtr config, float value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_do_sample", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_do_sample(IntPtr config, [MarshalAs(UnmanagedType.I1)] bool value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_repetition_penalty", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_repetition_penalty(IntPtr config, float value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_presence_penalty", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_presence_penalty(IntPtr config, float value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_frequency_penalty", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_frequency_penalty(IntPtr config, float value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_rng_seed", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_rng_seed(IntPtr config, UIntPtr value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_assistant_confidence_threshold", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_assistant_confidence_threshold(IntPtr config, float value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_num_assistant_tokens", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_num_assistant_tokens(IntPtr config, UIntPtr value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_max_ngram_size", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_max_ngram_size(IntPtr config, UIntPtr value);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_set_eos_token_id", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_set_eos_token_id(IntPtr config, long id);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_get_max_new_tokens", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_get_max_new_tokens(IntPtr config, ref UIntPtr max_new_tokens);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_generation_config_validate", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_generation_config_validate(IntPtr config);

        #endregion

        #region Whisper generation config / Whisper 生成配置

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_create", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_create(ref IntPtr config);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_create_from_json", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_create_from_json(IntPtr json_path, ref IntPtr config);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_free", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ov_genai_whisper_generation_config_free(IntPtr config);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_get_generation_config", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_get_generation_config(IntPtr config, ref IntPtr generation_config);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_set_decoder_start_token_id", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_set_decoder_start_token_id(IntPtr config, long token_id);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_get_decoder_start_token_id", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_get_decoder_start_token_id(IntPtr config, ref long token_id);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_set_pad_token_id", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_set_pad_token_id(IntPtr config, long token_id);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_get_pad_token_id", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_get_pad_token_id(IntPtr config, ref long token_id);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_set_translate_token_id", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_set_translate_token_id(IntPtr config, long token_id);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_get_translate_token_id", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_get_translate_token_id(IntPtr config, ref long token_id);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_set_transcribe_token_id", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_set_transcribe_token_id(IntPtr config, long token_id);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_get_transcribe_token_id", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_get_transcribe_token_id(IntPtr config, ref long token_id);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_set_prev_sot_token_id", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_set_prev_sot_token_id(IntPtr config, long token_id);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_get_prev_sot_token_id", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_get_prev_sot_token_id(IntPtr config, ref long token_id);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_set_no_timestamps_token_id", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_set_no_timestamps_token_id(IntPtr config, long token_id);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_get_no_timestamps_token_id", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_get_no_timestamps_token_id(IntPtr config, ref long token_id);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_set_max_initial_timestamp_index", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_set_max_initial_timestamp_index(IntPtr config, UIntPtr index);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_get_max_initial_timestamp_index", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_get_max_initial_timestamp_index(IntPtr config, ref UIntPtr index);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_set_is_multilingual", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_set_is_multilingual(IntPtr config, [MarshalAs(UnmanagedType.I1)] bool is_multilingual);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_get_is_multilingual", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_get_is_multilingual(IntPtr config, ref byte is_multilingual);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_set_language", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_set_language(IntPtr config, IntPtr language);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_get_language", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_get_language(IntPtr config, IntPtr language, ref UIntPtr language_size);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_set_task", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_set_task(IntPtr config, IntPtr task);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_get_task", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_get_task(IntPtr config, IntPtr task, ref UIntPtr task_size);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_set_return_timestamps", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_set_return_timestamps(IntPtr config, [MarshalAs(UnmanagedType.I1)] bool return_timestamps);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_get_return_timestamps", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_get_return_timestamps(IntPtr config, ref byte return_timestamps);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_set_initial_prompt", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_set_initial_prompt(IntPtr config, IntPtr initial_prompt);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_get_initial_prompt", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_get_initial_prompt(IntPtr config, IntPtr initial_prompt, ref UIntPtr prompt_size);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_set_hotwords", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_set_hotwords(IntPtr config, IntPtr hotwords);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_get_hotwords", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_get_hotwords(IntPtr config, IntPtr hotwords, ref UIntPtr hotwords_size);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_set_begin_suppress_tokens", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_set_begin_suppress_tokens(IntPtr config, IntPtr tokens, UIntPtr tokens_count);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_get_begin_suppress_tokens_count", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_get_begin_suppress_tokens_count(IntPtr config, ref UIntPtr tokens_count);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_get_begin_suppress_tokens", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_get_begin_suppress_tokens(IntPtr config, IntPtr tokens, UIntPtr tokens_count);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_set_suppress_tokens", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_set_suppress_tokens(IntPtr config, IntPtr tokens, UIntPtr tokens_count);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_get_suppress_tokens_count", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_get_suppress_tokens_count(IntPtr config, ref UIntPtr tokens_count);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_get_suppress_tokens", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_get_suppress_tokens(IntPtr config, IntPtr tokens, UIntPtr tokens_count);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_whisper_generation_config_validate", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_whisper_generation_config_validate(IntPtr config);

        #endregion

        #region Decoded results and LLM pipeline / 解码结果与 LLM Pipeline

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_decoded_results_create", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_decoded_results_create(ref IntPtr results);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_decoded_results_free", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ov_genai_decoded_results_free(IntPtr results);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_decoded_results_get_perf_metrics", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_decoded_results_get_perf_metrics(IntPtr results, ref IntPtr metrics);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_decoded_results_perf_metrics_free", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ov_genai_decoded_results_perf_metrics_free(IntPtr metrics);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_decoded_results_get_string", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_decoded_results_get_string(IntPtr results, IntPtr output, ref UIntPtr output_size);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_llm_pipeline_create", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_llm_pipeline_create(IntPtr models_path, IntPtr device, UIntPtr property_args_size, ref IntPtr pipe);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_llm_pipeline_create", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_llm_pipeline_create(IntPtr models_path, IntPtr device, UIntPtr property_args_size, ref IntPtr pipe, IntPtr key0, IntPtr value0);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_llm_pipeline_create", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_llm_pipeline_create(IntPtr models_path, IntPtr device, UIntPtr property_args_size, ref IntPtr pipe, IntPtr key0, IntPtr value0, IntPtr key1, IntPtr value1);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_llm_pipeline_create", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_llm_pipeline_create(IntPtr models_path, IntPtr device, UIntPtr property_args_size, ref IntPtr pipe, IntPtr key0, IntPtr value0, IntPtr key1, IntPtr value1, IntPtr key2, IntPtr value2);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_llm_pipeline_free", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ov_genai_llm_pipeline_free(IntPtr pipe);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_llm_pipeline_generate", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_llm_pipeline_generate(IntPtr pipe, IntPtr inputs, IntPtr config, IntPtr streamer, ref IntPtr results);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_llm_pipeline_generate", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_llm_pipeline_generate(IntPtr pipe, IntPtr inputs, IntPtr config, ref streamer_callback streamer, ref IntPtr results);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_llm_pipeline_generate_with_history", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_llm_pipeline_generate_with_history(IntPtr pipe, IntPtr history, IntPtr config, IntPtr streamer, ref IntPtr results);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_llm_pipeline_generate_with_history", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_llm_pipeline_generate_with_history(IntPtr pipe, IntPtr history, IntPtr config, ref streamer_callback streamer, ref IntPtr results);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_llm_pipeline_start_chat", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_llm_pipeline_start_chat(IntPtr pipe);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_llm_pipeline_finish_chat", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_llm_pipeline_finish_chat(IntPtr pipe);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_llm_pipeline_get_generation_config", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_llm_pipeline_get_generation_config(IntPtr pipe, ref IntPtr config);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_llm_pipeline_set_generation_config", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_llm_pipeline_set_generation_config(IntPtr pipe, IntPtr config);

        #endregion

        #region Performance metrics / 性能指标

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_perf_metrics_get_load_time", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_perf_metrics_get_load_time(IntPtr metrics, ref float load_time);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_perf_metrics_get_num_generation_tokens", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_perf_metrics_get_num_generation_tokens(IntPtr metrics, ref UIntPtr count);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_perf_metrics_get_num_input_tokens", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_perf_metrics_get_num_input_tokens(IntPtr metrics, ref UIntPtr count);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_perf_metrics_get_ttft", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_perf_metrics_get_ttft(IntPtr metrics, ref float mean, ref float std);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_perf_metrics_get_tpot", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_perf_metrics_get_tpot(IntPtr metrics, ref float mean, ref float std);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_perf_metrics_get_ipot", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_perf_metrics_get_ipot(IntPtr metrics, ref float mean, ref float std);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_perf_metrics_get_throughput", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_perf_metrics_get_throughput(IntPtr metrics, ref float mean, ref float std);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_perf_metrics_get_inference_duration", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_perf_metrics_get_inference_duration(IntPtr metrics, ref float mean, ref float std);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_perf_metrics_get_generate_duration", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_perf_metrics_get_generate_duration(IntPtr metrics, ref float mean, ref float std);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_perf_metrics_get_tokenization_duration", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_perf_metrics_get_tokenization_duration(IntPtr metrics, ref float mean, ref float std);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_perf_metrics_get_detokenization_duration", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_perf_metrics_get_detokenization_duration(IntPtr metrics, ref float mean, ref float std);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_perf_metrics_get_chat_template_duration", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_perf_metrics_get_chat_template_duration(IntPtr metrics, ref float mean, ref float std);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_perf_metrics_add_in_place", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ExceptionStatus ov_genai_perf_metrics_add_in_place(IntPtr left, IntPtr right);

        #endregion

        #region JsonContainer / JSON 容器

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_json_container_create", CallingConvention = CallingConvention.Cdecl)]
        internal static extern GenAIJsonContainerStatus ov_genai_json_container_create(ref IntPtr container);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_json_container_create_from_json_string", CallingConvention = CallingConvention.Cdecl)]
        internal static extern GenAIJsonContainerStatus ov_genai_json_container_create_from_json_string(ref IntPtr container, IntPtr json_str);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_json_container_create_object", CallingConvention = CallingConvention.Cdecl)]
        internal static extern GenAIJsonContainerStatus ov_genai_json_container_create_object(ref IntPtr container);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_json_container_create_array", CallingConvention = CallingConvention.Cdecl)]
        internal static extern GenAIJsonContainerStatus ov_genai_json_container_create_array(ref IntPtr container);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_json_container_free", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ov_genai_json_container_free(IntPtr container);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_json_container_to_json_string", CallingConvention = CallingConvention.Cdecl)]
        internal static extern GenAIJsonContainerStatus ov_genai_json_container_to_json_string(IntPtr container, IntPtr output, ref UIntPtr output_size);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_json_container_copy", CallingConvention = CallingConvention.Cdecl)]
        internal static extern GenAIJsonContainerStatus ov_genai_json_container_copy(IntPtr source, ref IntPtr target);

        #endregion

        #region ChatHistory / 聊天历史

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_chat_history_create", CallingConvention = CallingConvention.Cdecl)]
        internal static extern GenAIChatHistoryStatus ov_genai_chat_history_create(ref IntPtr history);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_chat_history_create_from_json_container", CallingConvention = CallingConvention.Cdecl)]
        internal static extern GenAIChatHistoryStatus ov_genai_chat_history_create_from_json_container(ref IntPtr history, IntPtr messages);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_chat_history_free", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ov_genai_chat_history_free(IntPtr history);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_chat_history_push_back", CallingConvention = CallingConvention.Cdecl)]
        internal static extern GenAIChatHistoryStatus ov_genai_chat_history_push_back(IntPtr history, IntPtr message);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_chat_history_pop_back", CallingConvention = CallingConvention.Cdecl)]
        internal static extern GenAIChatHistoryStatus ov_genai_chat_history_pop_back(IntPtr history);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_chat_history_get_messages", CallingConvention = CallingConvention.Cdecl)]
        internal static extern GenAIChatHistoryStatus ov_genai_chat_history_get_messages(IntPtr history, ref IntPtr messages);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_chat_history_get_message", CallingConvention = CallingConvention.Cdecl)]
        internal static extern GenAIChatHistoryStatus ov_genai_chat_history_get_message(IntPtr history, UIntPtr index, ref IntPtr message);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_chat_history_get_first", CallingConvention = CallingConvention.Cdecl)]
        internal static extern GenAIChatHistoryStatus ov_genai_chat_history_get_first(IntPtr history, ref IntPtr message);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_chat_history_get_last", CallingConvention = CallingConvention.Cdecl)]
        internal static extern GenAIChatHistoryStatus ov_genai_chat_history_get_last(IntPtr history, ref IntPtr message);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_chat_history_clear", CallingConvention = CallingConvention.Cdecl)]
        internal static extern GenAIChatHistoryStatus ov_genai_chat_history_clear(IntPtr history);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_chat_history_size", CallingConvention = CallingConvention.Cdecl)]
        internal static extern GenAIChatHistoryStatus ov_genai_chat_history_size(IntPtr history, ref UIntPtr size);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_chat_history_empty", CallingConvention = CallingConvention.Cdecl)]
        internal static extern GenAIChatHistoryStatus ov_genai_chat_history_empty(IntPtr history, ref int empty);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_chat_history_set_tools", CallingConvention = CallingConvention.Cdecl)]
        internal static extern GenAIChatHistoryStatus ov_genai_chat_history_set_tools(IntPtr history, IntPtr tools);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_chat_history_get_tools", CallingConvention = CallingConvention.Cdecl)]
        internal static extern GenAIChatHistoryStatus ov_genai_chat_history_get_tools(IntPtr history, ref IntPtr tools);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_chat_history_set_extra_context", CallingConvention = CallingConvention.Cdecl)]
        internal static extern GenAIChatHistoryStatus ov_genai_chat_history_set_extra_context(IntPtr history, IntPtr extra_context);

        [DllImport(GenAILibrary, EntryPoint = "ov_genai_chat_history_get_extra_context", CallingConvention = CallingConvention.Cdecl)]
        internal static extern GenAIChatHistoryStatus ov_genai_chat_history_get_extra_context(IntPtr history, ref IntPtr extra_context);

        #endregion
    }
}
