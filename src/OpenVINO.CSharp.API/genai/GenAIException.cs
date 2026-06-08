// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;

namespace OpenVinoSharp.GenAI
{
    /// <summary>
    /// OpenVINO GenAI 扩展异常 / Exception type for OpenVINO GenAI extension errors.
    /// </summary>
    public class GenAIException : Exception
    {
        /// <summary>
        /// 原生 GenAI 状态码 / Native GenAI status code.
        /// </summary>
        public int StatusCode { get; private set; }

        /// <summary>
        /// 失败的原生操作名称 / Native operation name that failed.
        /// </summary>
        public string Operation { get; private set; }

        /// <summary>
        /// 创建 GenAI 异常 / Creates a GenAI exception.
        /// </summary>
        /// <param name="statusCode">原生状态码 / Native status code.</param>
        /// <param name="operation">操作名称 / Operation name.</param>
        /// <param name="message">错误消息 / Error message.</param>
        public GenAIException(int statusCode, string operation, string message)
            : base(message)
        {
            StatusCode = statusCode;
            Operation = operation ?? string.Empty;
        }

        /// <summary>
        /// 返回可读异常文本 / Returns readable exception text.
        /// </summary>
        public override string ToString()
        {
            return $"[{Operation}:{StatusCode}] {Message}";
        }
    }
}

