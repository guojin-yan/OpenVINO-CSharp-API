// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;

namespace OpenVinoSharp
{
    /// <summary>
    /// OpenVINO异常类 / OpenVINO Exception class
    /// <para>用于封装OpenVINO运行时错误。/ Used to encapsulate OpenVINO runtime errors.</para>
    /// </summary>
    public class OVException : Exception
    {
        /// <summary>
        /// 异常状态码 / Exception status code
        /// </summary>
        public ExceptionStatus Status { get; private set; }

        /// <summary>
        /// 使用状态和消息构造OVException / Constructs OVException with status and message
        /// </summary>
        /// <param name="status">异常状态码 / Exception status</param>
        /// <param name="message">错误消息 / Error message</param>
        public OVException(ExceptionStatus status, string message)
            : base(message)
        {
            Status = status;
        }

        /// <summary>
        /// 使用状态、消息和内部异常构造OVException / Constructs OVException with status, message and inner exception
        /// </summary>
        /// <param name="status">异常状态码 / Exception status</param>
        /// <param name="message">错误消息 / Error message</param>
        /// <param name="innerException">内部异常 / Inner exception</param>
        public OVException(ExceptionStatus status, string message, Exception innerException)
            : base(message, innerException)
        {
            Status = status;
        }

        /// <summary>
        /// 获取异常描述 / Get exception description
        /// </summary>
        /// <returns>异常字符串表示 / Exception string representation</returns>
        public override string ToString()
        {
            return $"[{Status}] {Message}";
        }
    }
}
