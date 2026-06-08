// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using System.Text;
using OpenVinoSharp.Internal;
using OpenVinoSharp.native;

namespace OpenVinoSharp.GenAI
{
    /// <summary>
    /// OpenVINO GenAI 聊天历史 / Chat history for OpenVINO GenAI.
    /// <para>
    /// 每条消息通常是类似 <c>{"role":"user","content":"..."}</c> 的 JSON 对象。
    /// Each message is usually a JSON object like <c>{"role":"user","content":"..."}</c>.
    /// </para>
    /// </summary>
    public class ChatHistory : DisposableOvObject
    {
        /// <summary>
        /// 创建空聊天历史 / Creates an empty chat history.
        /// </summary>
        public ChatHistory()
        {
            GenAIStatus.ThrowOnError(GenAINativeMethods.ov_genai_chat_history_create(ref _ptr), nameof(ChatHistory));
        }

        /// <summary>
        /// 从 JSON 数组容器创建聊天历史 / Creates chat history from a JSON array container.
        /// </summary>
        public ChatHistory(JsonContainer messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            GenAIStatus.ThrowOnError(
                GenAINativeMethods.ov_genai_chat_history_create_from_json_container(ref _ptr, messages.OvPtr),
                nameof(ChatHistory));
        }

        internal ChatHistory(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// 消息数量 / Number of messages.
        /// </summary>
        public ulong Count => GetSize();

        /// <summary>
        /// 是否为空 / Whether the history is empty.
        /// </summary>
        public bool IsEmpty => Empty();

        /// <summary>
        /// 释放原生聊天历史 / Releases the native chat history.
        /// </summary>
        protected override void DisposeUnmanaged()
        {
            if (_ptr != IntPtr.Zero)
            {
                GenAINativeMethods.ov_genai_chat_history_free(_ptr);
                _ptr = IntPtr.Zero;
            }
            base.DisposeUnmanaged();
        }

        /// <summary>
        /// 追加一条 JSON 消息 / Appends a JSON message.
        /// </summary>
        public ChatHistory PushBack(JsonContainer message)
        {
            ThrowIfDisposed();
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            GenAIStatus.ThrowOnError(GenAINativeMethods.ov_genai_chat_history_push_back(_ptr, message.OvPtr), nameof(PushBack));
            return this;
        }

        /// <summary>
        /// 追加一条 JSON 字符串消息 / Appends a JSON string message.
        /// </summary>
        public ChatHistory PushBackJson(string messageJson)
        {
            using (JsonContainer message = JsonContainer.FromJsonString(messageJson))
            {
                return PushBack(message);
            }
        }

        /// <summary>
        /// 添加一条角色消息 / Adds a role message.
        /// </summary>
        public ChatHistory AddMessage(string role, string content)
        {
            if (string.IsNullOrEmpty(role))
                throw new ArgumentException("Role cannot be null or empty. / role 不能为空。", nameof(role));
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            string json = "{\"role\":\"" + EscapeJson(role) + "\",\"content\":\"" + EscapeJson(content) + "\"}";
            return PushBackJson(json);
        }

        /// <summary>
        /// 添加用户消息 / Adds a user message.
        /// </summary>
        public ChatHistory AddUserMessage(string content) => AddMessage("user", content);

        /// <summary>
        /// 添加 assistant 消息 / Adds an assistant message.
        /// </summary>
        public ChatHistory AddAssistantMessage(string content) => AddMessage("assistant", content);

        /// <summary>
        /// 移除最后一条消息 / Removes the last message.
        /// </summary>
        public ChatHistory PopBack()
        {
            ThrowIfDisposed();
            GenAIStatus.ThrowOnError(GenAINativeMethods.ov_genai_chat_history_pop_back(_ptr), nameof(PopBack));
            return this;
        }

        /// <summary>
        /// 获取所有消息 / Gets all messages.
        /// </summary>
        public JsonContainer GetMessages()
        {
            ThrowIfDisposed();
            IntPtr ptr = IntPtr.Zero;
            GenAIStatus.ThrowOnError(GenAINativeMethods.ov_genai_chat_history_get_messages(_ptr, ref ptr), nameof(GetMessages));
            return new JsonContainer(ptr);
        }

        /// <summary>
        /// 根据索引获取消息 / Gets a message by index.
        /// </summary>
        public JsonContainer GetMessage(ulong index)
        {
            ThrowIfDisposed();
            IntPtr ptr = IntPtr.Zero;
            GenAIStatus.ThrowOnError(
                GenAINativeMethods.ov_genai_chat_history_get_message(_ptr, StringUtils.ToNativeSize(index), ref ptr),
                nameof(GetMessage));
            return new JsonContainer(ptr);
        }

        /// <summary>
        /// 获取第一条消息 / Gets the first message.
        /// </summary>
        public JsonContainer GetFirst()
        {
            ThrowIfDisposed();
            IntPtr ptr = IntPtr.Zero;
            GenAIStatus.ThrowOnError(GenAINativeMethods.ov_genai_chat_history_get_first(_ptr, ref ptr), nameof(GetFirst));
            return new JsonContainer(ptr);
        }

        /// <summary>
        /// 获取最后一条消息 / Gets the last message.
        /// </summary>
        public JsonContainer GetLast()
        {
            ThrowIfDisposed();
            IntPtr ptr = IntPtr.Zero;
            GenAIStatus.ThrowOnError(GenAINativeMethods.ov_genai_chat_history_get_last(_ptr, ref ptr), nameof(GetLast));
            return new JsonContainer(ptr);
        }

        /// <summary>
        /// 清空聊天历史 / Clears the chat history.
        /// </summary>
        public ChatHistory Clear()
        {
            ThrowIfDisposed();
            GenAIStatus.ThrowOnError(GenAINativeMethods.ov_genai_chat_history_clear(_ptr), nameof(Clear));
            return this;
        }

        /// <summary>
        /// 获取消息数量 / Gets number of messages.
        /// </summary>
        public ulong GetSize()
        {
            ThrowIfDisposed();
            UIntPtr size = UIntPtr.Zero;
            GenAIStatus.ThrowOnError(GenAINativeMethods.ov_genai_chat_history_size(_ptr, ref size), nameof(GetSize));
            return StringUtils.FromNativeSize(size);
        }

        /// <summary>
        /// 检查聊天历史是否为空 / Checks whether the chat history is empty.
        /// </summary>
        public bool Empty()
        {
            ThrowIfDisposed();
            int empty = 0;
            GenAIStatus.ThrowOnError(GenAINativeMethods.ov_genai_chat_history_empty(_ptr, ref empty), nameof(Empty));
            return empty != 0;
        }

        /// <summary>
        /// 设置工具定义 / Sets tool definitions.
        /// </summary>
        public ChatHistory SetTools(JsonContainer tools)
        {
            ThrowIfDisposed();
            if (tools == null)
                throw new ArgumentNullException(nameof(tools));

            GenAIStatus.ThrowOnError(GenAINativeMethods.ov_genai_chat_history_set_tools(_ptr, tools.OvPtr), nameof(SetTools));
            return this;
        }

        /// <summary>
        /// 获取工具定义 / Gets tool definitions.
        /// </summary>
        public JsonContainer GetTools()
        {
            ThrowIfDisposed();
            IntPtr ptr = IntPtr.Zero;
            GenAIStatus.ThrowOnError(GenAINativeMethods.ov_genai_chat_history_get_tools(_ptr, ref ptr), nameof(GetTools));
            return new JsonContainer(ptr);
        }

        /// <summary>
        /// 设置额外上下文 / Sets extra context.
        /// </summary>
        public ChatHistory SetExtraContext(JsonContainer extraContext)
        {
            ThrowIfDisposed();
            if (extraContext == null)
                throw new ArgumentNullException(nameof(extraContext));

            GenAIStatus.ThrowOnError(
                GenAINativeMethods.ov_genai_chat_history_set_extra_context(_ptr, extraContext.OvPtr),
                nameof(SetExtraContext));
            return this;
        }

        /// <summary>
        /// 获取额外上下文 / Gets extra context.
        /// </summary>
        public JsonContainer GetExtraContext()
        {
            ThrowIfDisposed();
            IntPtr ptr = IntPtr.Zero;
            GenAIStatus.ThrowOnError(GenAINativeMethods.ov_genai_chat_history_get_extra_context(_ptr, ref ptr), nameof(GetExtraContext));
            return new JsonContainer(ptr);
        }

        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public ChatHistory push_back(JsonContainer message) => PushBack(message);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public ChatHistory pop_back() => PopBack();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public JsonContainer get_messages() => GetMessages();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public JsonContainer get_message(ulong index) => GetMessage(index);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public JsonContainer get_first() => GetFirst();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public JsonContainer get_last() => GetLast();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public ChatHistory clear() => Clear();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public ulong size() => GetSize();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public bool empty() => Empty();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public ChatHistory set_tools(JsonContainer tools) => SetTools(tools);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public JsonContainer get_tools() => GetTools();
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public ChatHistory set_extra_context(JsonContainer extraContext) => SetExtraContext(extraContext);
        /// <summary>兼容 C 风格别名 / C-style alias.</summary>
        public JsonContainer get_extra_context() => GetExtraContext();

        private static string EscapeJson(string value)
        {
            StringBuilder builder = new StringBuilder(value.Length + 8);
            foreach (char ch in value)
            {
                switch (ch)
                {
                    case '\\':
                        builder.Append("\\\\");
                        break;
                    case '"':
                        builder.Append("\\\"");
                        break;
                    case '\b':
                        builder.Append("\\b");
                        break;
                    case '\f':
                        builder.Append("\\f");
                        break;
                    case '\n':
                        builder.Append("\\n");
                        break;
                    case '\r':
                        builder.Append("\\r");
                        break;
                    case '\t':
                        builder.Append("\\t");
                        break;
                    default:
                        if (ch < 32)
                            builder.Append("\\u").Append(((int)ch).ToString("x4"));
                        else
                            builder.Append(ch);
                        break;
                }
            }
            return builder.ToString();
        }
    }
}

