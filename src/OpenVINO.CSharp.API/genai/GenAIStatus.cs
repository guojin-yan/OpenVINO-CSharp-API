// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

namespace OpenVinoSharp.GenAI
{
    /// <summary>
    /// JsonContainer 原生状态码 / Native status codes for JsonContainer.
    /// </summary>
    public enum GenAIJsonContainerStatus : int
    {
        /// <summary>成功 / Success.</summary>
        OK = 0,
        /// <summary>参数无效 / Invalid parameter.</summary>
        InvalidParam = -1,
        /// <summary>JSON 无效 / Invalid JSON.</summary>
        InvalidJson = -2,
        /// <summary>越界 / Out of bounds.</summary>
        OutOfBounds = -3,
        /// <summary>一般错误 / General error.</summary>
        Error = -4
    }

    /// <summary>
    /// ChatHistory 原生状态码 / Native status codes for ChatHistory.
    /// </summary>
    public enum GenAIChatHistoryStatus : int
    {
        /// <summary>成功 / Success.</summary>
        OK = 0,
        /// <summary>参数无效 / Invalid parameter.</summary>
        InvalidParam = -1,
        /// <summary>越界 / Out of bounds.</summary>
        OutOfBounds = -2,
        /// <summary>聊天历史为空 / Chat history is empty.</summary>
        Empty = -3,
        /// <summary>JSON 无效 / Invalid JSON.</summary>
        InvalidJson = -4,
        /// <summary>一般错误 / General error.</summary>
        Error = -5
    }

    /// <summary>
    /// 分组 beam search 的停止条件 / Stop criteria for grouped beam search.
    /// </summary>
    public enum StopCriteria : int
    {
        /// <summary>一旦得到足够候选就停止 / Stop as soon as enough candidates are complete.</summary>
        Early = 0,
        /// <summary>使用启发式停止条件 / Use heuristic stopping.</summary>
        Heuristic = 1,
        /// <summary>只有确定不会有更优候选时停止 / Stop only when no better candidate is possible.</summary>
        Never = 2
    }

    /// <summary>
    /// 流式生成回调返回状态 / Return status for streaming generation callbacks.
    /// </summary>
    public enum StreamingStatus : int
    {
        /// <summary>继续生成 / Continue generation.</summary>
        Running = 0,
        /// <summary>停止生成并保留当前历史 / Stop generation and keep current history.</summary>
        Stop = 1,
        /// <summary>取消本次生成并回滚最近输入 / Cancel generation and roll back the latest request.</summary>
        Cancel = 2
    }

    internal static class GenAIStatus
    {
        public static void ThrowOnError(GenAIJsonContainerStatus status, string operation)
        {
            if (status == GenAIJsonContainerStatus.OK)
                return;

            throw new GenAIException((int)status, operation, GetJsonMessage(status));
        }

        public static void ThrowOnError(GenAIChatHistoryStatus status, string operation)
        {
            if (status == GenAIChatHistoryStatus.OK)
                return;

            throw new GenAIException((int)status, operation, GetChatHistoryMessage(status));
        }

        private static string GetJsonMessage(GenAIJsonContainerStatus status)
        {
            switch (status)
            {
                case GenAIJsonContainerStatus.InvalidParam:
                    return "JsonContainer parameter is invalid. / JsonContainer 参数无效。";
                case GenAIJsonContainerStatus.InvalidJson:
                    return "JsonContainer JSON text is invalid. / JsonContainer JSON 文本无效。";
                case GenAIJsonContainerStatus.OutOfBounds:
                    return "JsonContainer output buffer is too small or index is out of bounds. / JsonContainer 输出缓冲区过小或索引越界。";
                case GenAIJsonContainerStatus.Error:
                    return "JsonContainer operation failed. / JsonContainer 操作失败。";
                default:
                    return "Unknown JsonContainer error. / 未知 JsonContainer 错误。";
            }
        }

        private static string GetChatHistoryMessage(GenAIChatHistoryStatus status)
        {
            switch (status)
            {
                case GenAIChatHistoryStatus.InvalidParam:
                    return "ChatHistory parameter is invalid. / ChatHistory 参数无效。";
                case GenAIChatHistoryStatus.OutOfBounds:
                    return "ChatHistory index is out of bounds. / ChatHistory 索引越界。";
                case GenAIChatHistoryStatus.Empty:
                    return "ChatHistory is empty. / ChatHistory 为空。";
                case GenAIChatHistoryStatus.InvalidJson:
                    return "ChatHistory JSON message is invalid. / ChatHistory JSON 消息无效。";
                case GenAIChatHistoryStatus.Error:
                    return "ChatHistory operation failed. / ChatHistory 操作失败。";
                default:
                    return "Unknown ChatHistory error. / 未知 ChatHistory 错误。";
            }
        }
    }
}

