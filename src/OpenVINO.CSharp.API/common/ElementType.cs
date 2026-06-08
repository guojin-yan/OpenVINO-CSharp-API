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
    /// OpenVINO元素类型枚举 / OpenVINO element type enumeration
    /// <para>此枚举包含元素类型的代码。/ This enum contains codes for element types.</para>
    /// </summary>
    public enum ElementType : uint
    {
        /// <summary>
        /// 未定义元素类型 / Undefined element type
        /// </summary>
        UNDEFINED = 0U,
        
        /// <summary>
        /// 动态元素类型 / Dynamic element type
        /// </summary>
        DYNAMIC = UNDEFINED,
        
        /// <summary>
        /// 布尔元素类型 / Boolean element type
        /// </summary>
        BOOLEAN,
        
        /// <summary>
        /// BF16（脑浮点16位）元素类型 / BF16 (brain floating-point 16-bit) element type
        /// </summary>
        BF16,
        
        /// <summary>
        /// F16（半精度浮点）元素类型 / F16 (half-precision floating-point) element type
        /// </summary>
        F16,
        
        /// <summary>
        /// F32（单精度浮点）元素类型 / F32 (single-precision floating-point) element type
        /// </summary>
        F32,
        
        /// <summary>
        /// F64（双精度浮点）元素类型 / F64 (double-precision floating-point) element type
        /// </summary>
        F64,
        
        /// <summary>
        /// I4（4位有符号整数）元素类型 / I4 (4-bit signed integer) element type
        /// </summary>
        I4,
        
        /// <summary>
        /// I8（8位有符号整数）元素类型 / I8 (8-bit signed integer) element type
        /// </summary>
        I8,
        
        /// <summary>
        /// I16（16位有符号整数）元素类型 / I16 (16-bit signed integer) element type
        /// </summary>
        I16,
        
        /// <summary>
        /// I32（32位有符号整数）元素类型 / I32 (32-bit signed integer) element type
        /// </summary>
        I32,
        
        /// <summary>
        /// I64（64位有符号整数）元素类型 / I64 (64-bit signed integer) element type
        /// </summary>
        I64,
        
        /// <summary>
        /// U1（1位无符号整数）元素类型 / U1 (1-bit unsigned integer) element type
        /// </summary>
        U1,
        
        /// <summary>
        /// U2（2位无符号整数）元素类型 / U2 (2-bit unsigned integer) element type
        /// </summary>
        U2,
        
        /// <summary>
        /// U3（3位无符号整数）元素类型 / U3 (3-bit unsigned integer) element type
        /// </summary>
        U3,
        
        /// <summary>
        /// U4（4位无符号整数）元素类型 / U4 (4-bit unsigned integer) element type
        /// </summary>
        U4,
        
        /// <summary>
        /// U6（6位无符号整数）元素类型 / U6 (6-bit unsigned integer) element type
        /// </summary>
        U6,
        
        /// <summary>
        /// U8（8位无符号整数）元素类型 / U8 (8-bit unsigned integer) element type
        /// </summary>
        U8,
        
        /// <summary>
        /// U16（16位无符号整数）元素类型 / U16 (16-bit unsigned integer) element type
        /// </summary>
        U16,
        
        /// <summary>
        /// U32（32位无符号整数）元素类型 / U32 (32-bit unsigned integer) element type
        /// </summary>
        U32,
        
        /// <summary>
        /// U64（64位无符号整数）元素类型 / U64 (64-bit unsigned integer) element type
        /// </summary>
        U64,
        
        /// <summary>
        /// NF4（4位归一化浮点）元素类型 / NF4 (4-bit normalized floating-point) element type
        /// </summary>
        NF4,
        
        /// <summary>
        /// F8E4M3（8位浮点E4M3）元素类型 / F8E4M3 (8-bit floating-point E4M3) element type
        /// </summary>
        F8E4M3,
        
        /// <summary>
        /// F8E5M3（8位浮点E5M3）元素类型 / F8E5M3 (8-bit floating-point E5M3) element type
        /// </summary>
        F8E5M3,
        
        /// <summary>
        /// 字符串元素类型 / String element type
        /// </summary>
        STRING,
        
        /// <summary>
        /// F4E2M1（4位浮点E2M1）元素类型 / F4E2M1 (4-bit floating-point E2M1) element type
        /// </summary>
        F4E2M1,
        
        /// <summary>
        /// F8E8M0（8位浮点E8M0）元素类型 / F8E8M0 (8-bit floating-point E8M0) element type
        /// </summary>
        F8E8M0,
    }
}
