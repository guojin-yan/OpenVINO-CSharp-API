// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

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
