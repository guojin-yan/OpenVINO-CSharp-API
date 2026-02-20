// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

namespace OpenVinoSharp
{
    /// <summary>
    /// OpenVINO 异常状态码枚举 / OpenVINO exception status code enumeration
    /// <para>包含所有接口函数可能的返回值代码 / Contains all possible return value codes for interface functions</para>
    /// </summary>
    public enum ExceptionStatus : int
    {
        /// <summary>
        /// 操作成功完成 / Operation completed successfully
        /// </summary>
        OK = 0,

        // C++ 接口异常映射 / C++ interface exception mapping
        /// <summary>
        /// 一般错误 / General error
        /// </summary>
        GENERAL_ERROR = -1,
        
        /// <summary>
        /// 功能未实现 / Not implemented
        /// </summary>
        NOT_IMPLEMENTED = -2,
        
        /// <summary>
        /// 网络未加载 / Network not loaded
        /// </summary>
        NETWORK_NOT_LOADED = -3,
        
        /// <summary>
        /// 参数不匹配 / Parameter mismatch
        /// </summary>
        PARAMETER_MISMATCH = -4,
        
        /// <summary>
        /// 未找到 / Not found
        /// </summary>
        NOT_FOUND = -5,
        
        /// <summary>
        /// 越界 / Out of bounds
        /// </summary>
        OUT_OF_BOUNDS = -6,
        
        /// <summary>
        /// 意外错误 / Unexpected error
        /// </summary>
        UNEXPECTED = -7,
        
        /// <summary>
        /// 请求繁忙 / Request busy
        /// </summary>
        REQUEST_BUSY = -8,
        
        /// <summary>
        /// 结果未就绪 / Result not ready
        /// </summary>
        RESULT_NOT_READY = -9,
        
        /// <summary>
        /// 未分配 / Not allocated
        /// </summary>
        NOT_ALLOCATED = -10,
        
        /// <summary>
        /// 推理未开始 / Inference not started
        /// </summary>
        INFER_NOT_STARTED = -11,
        
        /// <summary>
        /// 网络未读取 / Network not read
        /// </summary>
        NETWORK_NOT_READ = -12,
        
        /// <summary>
        /// 推理已取消 / Inference cancelled
        /// </summary>
        INFER_CANCELLED = -13,

        // C 包装器异常 / C wrapper exceptions
        /// <summary>
        /// 无效的C参数 / Invalid C parameter
        /// </summary>
        INVALID_C_PARAM = -14,
        
        /// <summary>
        /// 未知的C错误 / Unknown C error
        /// </summary>
        UNKNOWN_C_ERROR = -15,
        
        /// <summary>
        /// C方法未实现 / C method not implemented
        /// </summary>
        NOT_IMPLEMENT_C_METHOD = -16,
        
        /// <summary>
        /// 未知异常 / Unknown exception
        /// </summary>
        UNKNOW_EXCEPTION = -17,
        
        /// <summary>
        /// 指针为空 / Pointer is null
        /// </summary>
        PTR_NULL = -100,
    }
}
