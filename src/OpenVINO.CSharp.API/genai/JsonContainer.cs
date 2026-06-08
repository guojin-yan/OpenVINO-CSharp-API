// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using OpenVinoSharp.Internal;
using OpenVinoSharp.native;

namespace OpenVinoSharp.GenAI
{
    /// <summary>
    /// OpenVINO GenAI JSON 容器 / JSON container used by OpenVINO GenAI.
    /// <para>
    /// 该类型主要用于 ChatHistory、工具定义和额外上下文的 JSON 数据传递。
    /// This type is mainly used to pass JSON data for ChatHistory, tool definitions, and extra context.
    /// </para>
    /// </summary>
    public class JsonContainer : DisposableOvObject
    {
        /// <summary>
        /// 创建空 JSON 对象容器 / Creates an empty JSON object container.
        /// </summary>
        public JsonContainer()
        {
            GenAIStatus.ThrowOnError(GenAINativeMethods.ov_genai_json_container_create(ref _ptr), nameof(JsonContainer));
        }

        internal JsonContainer(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// 从 JSON 字符串创建容器 / Creates a container from a JSON string.
        /// </summary>
        public static JsonContainer FromJsonString(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw new ArgumentException("JSON string cannot be null or empty. / JSON 字符串不能为空。", nameof(json));

            IntPtr ptr = IntPtr.Zero;
            GenAIJsonContainerStatus status = StringUtils.WithUtf8Ptr(
                json,
                jsonPtr => GenAINativeMethods.ov_genai_json_container_create_from_json_string(ref ptr, jsonPtr));
            GenAIStatus.ThrowOnError(status, nameof(FromJsonString));
            return new JsonContainer(ptr);
        }

        /// <summary>
        /// 创建空 JSON 对象 / Creates an empty JSON object.
        /// </summary>
        public static JsonContainer CreateObject()
        {
            IntPtr ptr = IntPtr.Zero;
            GenAIStatus.ThrowOnError(GenAINativeMethods.ov_genai_json_container_create_object(ref ptr), nameof(CreateObject));
            return new JsonContainer(ptr);
        }

        /// <summary>
        /// 创建空 JSON 数组 / Creates an empty JSON array.
        /// </summary>
        public static JsonContainer CreateArray()
        {
            IntPtr ptr = IntPtr.Zero;
            GenAIStatus.ThrowOnError(GenAINativeMethods.ov_genai_json_container_create_array(ref ptr), nameof(CreateArray));
            return new JsonContainer(ptr);
        }

        /// <summary>
        /// 释放原生 JSON 容器 / Releases the native JSON container.
        /// </summary>
        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero)
            {
                GenAINativeMethods.ov_genai_json_container_free(_ptr);
                _ptr = IntPtr.Zero;
            }
            base.DisposeUnmanaged();
        }

        /// <summary>
        /// 转换为 JSON 字符串 / Converts the container to a JSON string.
        /// </summary>
        public string ToJsonString()
        {
            ThrowIfDisposed();
            return GenAIStringHelper.GetJsonString(_ptr, GenAINativeMethods.ov_genai_json_container_to_json_string, nameof(ToJsonString));
        }

        /// <summary>
        /// 创建深拷贝 / Creates a deep copy.
        /// </summary>
        public JsonContainer Copy()
        {
            ThrowIfDisposed();
            IntPtr ptr = IntPtr.Zero;
            GenAIStatus.ThrowOnError(GenAINativeMethods.ov_genai_json_container_copy(_ptr, ref ptr), nameof(Copy));
            return new JsonContainer(ptr);
        }

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public string to_json_string() => ToJsonString();

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public JsonContainer copy() => Copy();

        /// <summary>
        /// 返回 JSON 字符串 / Returns the JSON string.
        /// </summary>
        public override string ToString()
        {
            return ToJsonString();
        }
    }
}

