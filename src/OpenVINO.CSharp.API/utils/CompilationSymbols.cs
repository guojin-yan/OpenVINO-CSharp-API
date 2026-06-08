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

namespace OpenVinoSharp
{
    /// <summary>
    /// 编译符号说明文档类
    /// 
    /// 本项目使用条件编译符号来支持不同版本的.NET框架。
    /// 以下是所有定义的编译符号及其含义：
    /// 
    /// ===== 框架版本符号 =====
    /// 
    /// 【.NET Framework】
    /// - NETFRAMEWORK          : 所有 .NET Framework 版本
    /// - NETFRAMEWORK_LEGACY   : .NET Framework 4.6-4.8
    /// - NET46                 : .NET Framework 4.6
    /// - NET46_OR_GREATER      : .NET Framework 4.6 或更高
    /// - NET47                 : .NET Framework 4.7
    /// - NET47_OR_GREATER      : .NET Framework 4.7 或更高
    /// - NET48                 : .NET Framework 4.8
    /// - NET48_OR_GREATER      : .NET Framework 4.8 或更高
    /// 
    /// 【.NET Core / .NET 5+】
    /// - NETCOREAPP            : 所有 .NET Core / .NET 5+ 版本
    /// - NET5_0                : .NET 5.0
    /// - NET5_0_OR_GREATER     : .NET 5.0 或更高
    /// - NET6_0                : .NET 6.0
    /// - NET6_0_OR_GREATER     : .NET 6.0 或更高
    /// - NET7_0                : .NET 7.0
    /// - NET7_0_OR_GREATER     : .NET 7.0 或更高
    /// - NET8_0                : .NET 8.0
    /// - NET8_0_OR_GREATER     : .NET 8.0 或更高
    /// - NET9_0                : .NET 9.0
    /// - NET9_0_OR_GREATER     : .NET 9.0 或更高
    /// - NET10_0               : .NET 10.0
    /// - NET10_0_OR_GREATER    : .NET 10.0 或更高
    /// 
    /// ===== 功能特性符号 =====
    /// 
    /// - HAS_SPAN              : 支持 Span&lt;T&gt; 和 ReadOnlySpan&lt;T&gt;
    ///                           (.NET Core 2.1+, .NET 5+, .NET Framework 4.7.2+)
    ///                           
    /// - HAS_MEMORY            : 支持 Memory&lt;T&gt; 和 ReadOnlyMemory&lt;T&gt;
    ///                           (.NET Core 2.1+, .NET 5+, .NET Framework 4.7.2+)
    ///                           
    /// - HAS_UNSAFE            : 支持 unsafe 代码和 Unsafe 类
    ///                           (所有目标框架)
    ///                           
    /// - HAS_NATIVELIBRARY     : 支持 NativeLibrary 类
    ///                           (.NET Core 3.0+, .NET 5+)
    ///                           用于动态加载原生库
    ///                           
    /// - HAS_INDEX_RANGE       : 支持 Index 和 Range 类型
    ///                           (.NET Core 3.0+, .NET 5+)
    ///                           
    /// - HAS_ASYNC_ENUMERABLE  : 支持 IAsyncEnumerable&lt;T&gt;
    ///                           (.NET Core 3.0+, .NET 5+)
    /// 
    /// ===== 使用示例 =====
    /// 
    /// <code>
    /// #if HAS_SPAN
    ///     // 使用 Span&lt;T&gt; 的高效实现
    ///     public void ProcessData(ReadOnlySpan&lt;byte&gt; data) { ... }
    /// #else
    ///     // 传统实现
    ///     public void ProcessData(byte[] data) { ... }
    /// #endif
    /// 
    /// #if NETFRAMEWORK
    ///     // .NET Framework 特定代码
    /// #else
    ///     // .NET Core / .NET 5+ 代码
    /// #endif
    /// 
    /// #if HAS_NATIVELIBRARY
    ///     // 使用 NativeLibrary 类加载 DLL
    /// #else
    ///     // 使用 LoadLibrary/ dlopen 加载 DLL
    /// #endif
    /// </code>
    /// </summary>
    internal static class CompilationSymbols
    {
        // 此类仅用于文档说明，不包含实际代码
    }
}
