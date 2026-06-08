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

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenVinoSharp
{
    /// <summary>
    /// 优化的字符串转换工具类 / Optimized string conversion utility class
    /// <para>提供高效的 C 字符串和 C# 字符串之间的转换 / Provides efficient conversion between C strings and C# strings</para>
    /// </summary>
    internal static class StringUtils
    {
        // UTF-8 编码实例（缓存以提高性能） / UTF-8 encoding instance (cached for performance)
        internal static readonly Encoding Utf8Encoding = new UTF8Encoding(false, false);
        
        /// <summary>
        /// 将 C# 字符串转换为 C 字符串指针 (UTF-8 编码) / Convert C# string to C string pointer (UTF-8 encoding)
        /// <para>OpenVINO C API 使用 UTF-8 char*，这里显式分配 UTF-8 + null 终止符内存。/ OpenVINO C API uses UTF-8 char*, so this allocates UTF-8 + null-terminated memory explicitly.</para>
        /// </summary>
        /// <param name="str">C# 字符串 / C# string</param>
        /// <returns>指向 UTF-8 编码的内存指针，需要用 Marshal.FreeHGlobal 释放 / Pointer to UTF-8 encoded memory, needs to be freed with Marshal.FreeHGlobal</returns>
        public static IntPtr StringToUtf8Ptr(string str)
        {
            if (str == null)
                return IntPtr.Zero;
            
            // 计算需要的字节数 / Calculate required byte count
            int byteCount = Utf8Encoding.GetByteCount(str);
            // 分配内存 (+1 为 null 终止符) / Allocate memory (+1 for null terminator)
            IntPtr ptr = Marshal.AllocHGlobal(byteCount + 1);
            // 将字符串转换为字节 / Convert string to bytes
            byte[] bytes = Utf8Encoding.GetBytes(str);
            // 复制到分配的内存 / Copy to allocated memory
            Marshal.Copy(bytes, 0, ptr, byteCount);
            // 写入 null 终止符 / Write null terminator
            Marshal.WriteByte(ptr, byteCount, 0);
            
            return ptr;
        }

        /// <summary>
        /// 将 C 字符串指针转换为 C# 字符串 (UTF-8 编码) / Convert C string pointer to C# string (UTF-8 encoding)
        /// </summary>
        /// <param name="ptr">C 字符串指针 / C string pointer</param>
        /// <returns>C# 字符串 / C# string</returns>
        public static string Utf8PtrToString(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return null;

#if HAS_SPAN
            // 使用 Span 进行高效转换（无额外分配） / Use Span for efficient conversion (no additional allocation)
            unsafe
            {
                byte* bytePtr = (byte*)ptr;
                int length = 0;
                while (bytePtr[length] != 0)
                    length++;
                
                return Utf8Encoding.GetString(bytePtr, length);
            }
#else
            // 传统方法 / Traditional method
            int length = 0;
            while (Marshal.ReadByte(ptr, length) != 0)
                length++;
            
            byte[] bytes = new byte[length];
            Marshal.Copy(ptr, bytes, 0, length);
            return Utf8Encoding.GetString(bytes);
#endif
        }

        /// <summary>
        /// 将 C# 字符串转换为 sbyte 数组（用于 C API 的 char*） / Convert C# string to sbyte array (for C API char*)
        /// </summary>
        /// <param name="str">C# 字符串 / C# string</param>
        /// <returns>sbyte 数组 / sbyte array</returns>
        public static sbyte[] StringToSByteArray(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new sbyte[] { 0 };

            byte[] bytes = Utf8Encoding.GetBytes(str);
            sbyte[] result = new sbyte[bytes.Length + 1]; // +1 for null terminator / +1 for null terminator
            
            for (int i = 0; i < bytes.Length; i++)
            {
                result[i] = (sbyte)bytes[i];
            }
            // null terminator 已经是 0 / Null terminator is already 0
            
            return result;
        }

        /// <summary>
        /// 将 C 字符串指针 (sbyte*) 转换为 C# 字符串 / Convert C string pointer (sbyte*) to C# string
        /// </summary>
        /// <param name="ptr">sbyte 指针 / sbyte pointer</param>
        /// <returns>C# 字符串 / C# string</returns>
        public static unsafe string SBytePtrToString(sbyte* ptr)
        {
            if (ptr == null)
                return null;

#if HAS_SPAN
            // 使用 Span 进行高效转换
            int length = 0;
            while (ptr[length] != 0)
                length++;
            
            ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(ptr, length);
            return Utf8SpanToString(span);
#else
            // 计算长度 / Calculate length
            int length = 0;
            while (ptr[length] != 0)
                length++;
            
            byte[] bytes = new byte[length];
            fixed (byte* dest = bytes)
            {
                Buffer.MemoryCopy(ptr, dest, length, length);
            }
            return Utf8Encoding.GetString(bytes);
#endif
        }

        /// <summary>
        /// 释放由 StringToUtf8Ptr 分配的内存 / Free memory allocated by StringToUtf8Ptr
        /// </summary>
        /// <param name="ptr">要释放的指针 / Pointer to free</param>
        public static void FreeUtf8Ptr(IntPtr ptr)
        {
            if (ptr != IntPtr.Zero)
                Marshal.FreeHGlobal(ptr);
        }

        /// <summary>
        /// 将 C# 字符串数组转换为 C 字符串指针数组 / Convert C# string array to C string pointer array
        /// </summary>
        /// <param name="strings">C# 字符串数组 / C# string array</param>
        /// <returns>指针数组，每个元素需要用 FreeUtf8Ptr 释放 / Pointer array, each element needs to be freed with FreeUtf8Ptr</returns>
        public static IntPtr[] StringArrayToUtf8PtrArray(string[] strings)
        {
            if (strings == null)
                return null;

            IntPtr[] ptrs = new IntPtr[strings.Length];
            for (int i = 0; i < strings.Length; i++)
            {
                ptrs[i] = StringToUtf8Ptr(strings[i]);
            }
            return ptrs;
        }

        /// <summary>
        /// 释放字符串指针数组 / Free string pointer array
        /// </summary>
        /// <param name="ptrs">指针数组 / Pointer array</param>
        public static void FreeUtf8PtrArray(IntPtr[] ptrs)
        {
            if (ptrs == null)
                return;

            foreach (var ptr in ptrs)
            {
                FreeUtf8Ptr(ptr);
            }
        }

        /// <summary>
        /// 使用 UTF-8 原生字符串执行回调并自动释放内存 / Execute an action with a native UTF-8 string and free it automatically
        /// </summary>
        /// <typeparam name="T">回调返回类型 / Callback return type</typeparam>
        /// <param name="value">托管字符串 / Managed string</param>
        /// <param name="action">接收 UTF-8 指针的回调 / Callback receiving the UTF-8 pointer</param>
        /// <returns>回调返回值 / Callback result</returns>
        public static T WithUtf8Ptr<T>(string value, Func<IntPtr, T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            IntPtr ptr = StringToUtf8Ptr(value);
            try
            {
                return action(ptr);
            }
            finally
            {
                FreeUtf8Ptr(ptr);
            }
        }

        /// <summary>
        /// 使用两个 UTF-8 原生字符串执行回调并自动释放内存 / Execute an action with two native UTF-8 strings and free them automatically
        /// </summary>
        /// <typeparam name="T">回调返回类型 / Callback return type</typeparam>
        /// <param name="first">第一个托管字符串 / First managed string</param>
        /// <param name="second">第二个托管字符串 / Second managed string</param>
        /// <param name="action">接收两个 UTF-8 指针的回调 / Callback receiving the two UTF-8 pointers</param>
        /// <returns>回调返回值 / Callback result</returns>
        public static T WithUtf8Ptrs<T>(string first, string second, Func<IntPtr, IntPtr, T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            IntPtr firstPtr = StringToUtf8Ptr(first);
            IntPtr secondPtr = StringToUtf8Ptr(second);
            try
            {
                return action(firstPtr, secondPtr);
            }
            finally
            {
                FreeUtf8Ptr(secondPtr);
                FreeUtf8Ptr(firstPtr);
            }
        }

        /// <summary>
        /// 将元素数量转换为本机 size_t 宽度 / Convert an element count to the native size_t width
        /// </summary>
        /// <param name="value">元素数量 / Element count</param>
        /// <returns>本机 size_t 值 / Native size_t value</returns>
        public static UIntPtr ToNativeSize(ulong value)
        {
            if (UIntPtr.Size == 4 && value > uint.MaxValue)
                throw new OverflowException("size_t value exceeds 32-bit native pointer width. / size_t 数值超过 32 位本机指针宽度。");

            return new UIntPtr(value);
        }

        /// <summary>
        /// 将本机 size_t 转换为托管 ulong / Convert native size_t to managed ulong
        /// </summary>
        /// <param name="value">本机 size_t 值 / Native size_t value</param>
        /// <returns>托管 ulong 值 / Managed ulong value</returns>
        public static ulong FromNativeSize(UIntPtr value)
        {
            return value.ToUInt64();
        }

#if HAS_SPAN
        /// <summary>
        /// 将字符串转换为 UTF-8 编码的 Span（.NET Core 2.1+ / .NET 5+） / Convert string to UTF-8 encoded Span (.NET Core 2.1+ / .NET 5+)
        /// </summary>
        /// <param name="str">C# 字符串 / C# string</param>
        /// <param name="span">目标 Span / Target Span</param>
        /// <returns>实际写入的字节数 / Actual number of bytes written</returns>
        public static int StringToUtf8Span(string str, Span<byte> span)
        {
            if (str == null)
                return 0;

            // Some target frameworks may not expose Encoding.GetBytes(ReadOnlySpan<char>, Span<byte>)
            // Fallback to allocating a temporary byte[] which is broadly supported.
            byte[] bytes = Utf8Encoding.GetBytes(str);
            int write = Math.Min(bytes.Length, span.Length);
            new ReadOnlySpan<byte>(bytes, 0, write).CopyTo(span);
            return write;
        }

        /// <summary>
        /// 将 UTF-8 编码的 Span 转换为字符串（.NET Core 2.1+ / .NET 5+） / Convert UTF-8 encoded Span to string (.NET Core 2.1+ / .NET 5+)
        /// </summary>
        /// <param name="span">UTF-8 编码的 Span / UTF-8 encoded Span</param>
        /// <returns>C# 字符串 / C# string</returns>
        public static string Utf8SpanToString(ReadOnlySpan<byte> span)
        {
            // Some target frameworks may not expose Encoding.GetString(ReadOnlySpan<byte>)
            // Use the array-backed overload to ensure compatibility.
            if (span.IsEmpty)
                return string.Empty;

            byte[] bytes = span.ToArray();
            return Utf8Encoding.GetString(bytes);
        }
#endif
    }
}
