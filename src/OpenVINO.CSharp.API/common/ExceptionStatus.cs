//  ========================================================================
//  【项目名称】OpenVINO C# API
//  【项目描述】OpenVINO™ 的 C# 语言绑定库，提供高性能深度学习推理能力
//  【版权声明】© 2026-2025 Guojin Yan. All Rights Reserved.
//  【开源协议】Apache-2.0 License（请遵守许可证条款）
//  -----------------------------------------------------------------------
//  【功能简介】
//  1. 完整的 OpenVINO™ C API 封装，提供 C# 友好的面向对象接口。
//  2. 支持模型加载、编译、推理全流程操作。
//  3. 支持 CPU、GPU、VPU 等多种推理设备。
//  4. 支持同步推理和异步推理模式。
//  5. 支持预处理和后处理流水线配置。
//  6. 支持动态形状和批量推理。
//  7. 支持模型缓存和性能分析。
//  8. 支持远程上下文（Remote Context）和零拷贝推理。
//  9. 支持 .NET Framework 4.6.1+、.NET Core 2.0+、.NET 5/6/7/8/9+。
//  10. 提供推理请求对象池，优化高并发场景性能。
//  11. 提供完善的异常处理和日志记录机制。
//  12. 提供丰富的单元测试和集成测试用例。
//  -----------------------------------------------------------------------
//  【官方资源】
//  📌 GitHub仓库：https://github.com/guojin-yan/OpenVINO-CSharp-API
//  📌 NuGet包：https://www.nuget.org/packages/OpenVINO.CSharp.API
//  📌 在线文档：https://guojin-yan.github.io/OpenVINO-CSharp-API/index.html
//  📌 示例代码：https://github.com/guojin-yan/OpenVINO-CSharp-API/tree/csharp3.3/samples
//  -----------------------------------------------------------------------
//  【社区支持】
//  💬 QQ交流群：945057948（加入获取技术支持）
//  📱 微信公众号：CSharp与边缘模型部署（教程+案例）
//  📝 CSDN博客：https://guojin.blog.csdn.net（技术文章）
//  -----------------------------------------------------------------------
//  【联系我们】
//  ✉ 项目维护：guojin_yjs@cumt.edu.cn
//  💬 微信咨询：15253793309
//  🐛 Bug反馈：https://github.com/guojin-yan/OpenVINO-CSharp-API/issues
//  💡 功能建议：https://github.com/guojin-yan/OpenVINO-CSharp-API/discussions/landing
//  -----------------------------------------------------------------------
//  【致谢】
//  本项目基于 Intel® OpenVINO™ 工具包开发，感谢 Intel 提供的优秀开源项目。
//  OpenVINO™ 是 Intel Corporation 的商标。
//  ========================================================================
//  
//  【许可声明】
//  1. 本项目采用 Apache-2.0 License 开源协议，允许自由使用、修改和分发。
//  2. 使用本项目即表示您同意 Apache-2.0 License 许可证的所有条款。
//  3. 本项目按"原样"提供，不提供任何形式的担保。
//  4. 使用本项目产生的任何风险由使用者自行承担。
//  5. 修改或分发时请保留原始版权声明和许可声明。
//  ========================================================================
//

// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

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
