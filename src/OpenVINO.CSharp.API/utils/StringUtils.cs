// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenVinoSharp
{
    /// <summary>
    /// 优化的字符串转换工具类
    /// 提供高效的 C 字符串和 C# 字符串之间的转换
    /// </summary>
    internal static class StringUtils
    {
        // UTF-8 编码实例（缓存以提高性能）
        internal static readonly Encoding Utf8Encoding = new UTF8Encoding(false, false);
        
        // ANSI 编码实例
        internal static readonly Encoding AnsiEncoding = Encoding.Default;

        /// <summary>
        /// 将 C# 字符串转换为 C 字符串指针 (UTF-8 编码)
        /// 使用 Marshal.StringToHGlobalAnsi 在 .NET Framework 上更稳定
        /// </summary>
        /// <param name="str">C# 字符串</param>
        /// <returns>指向 UTF-8 编码的内存指针，需要用 Marshal.FreeHGlobal 释放</returns>
        public static IntPtr StringToUtf8Ptr(string str)
        {
            if (str == null)
                return IntPtr.Zero;
            
            // 计算需要的字节数
            int byteCount = Utf8Encoding.GetByteCount(str);
            // 分配内存 (+1 为 null 终止符)
            IntPtr ptr = Marshal.AllocHGlobal(byteCount + 1);
            // 将字符串转换为字节
            byte[] bytes = Utf8Encoding.GetBytes(str);
            // 复制到分配的内存
            Marshal.Copy(bytes, 0, ptr, byteCount);
            // 写入 null 终止符
            Marshal.WriteByte(ptr, byteCount, 0);
            
            return ptr;
        }

        /// <summary>
        /// 将 C 字符串指针转换为 C# 字符串 (UTF-8 编码)
        /// </summary>
        /// <param name="ptr">C 字符串指针</param>
        /// <returns>C# 字符串</returns>
        public static string Utf8PtrToString(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return null;

#if HAS_SPAN
            // 使用 Span 进行高效转换（无额外分配）
            unsafe
            {
                byte* bytePtr = (byte*)ptr;
                int length = 0;
                while (bytePtr[length] != 0)
                    length++;
                
                return Utf8Encoding.GetString(bytePtr, length);
            }
#else
            // 传统方法
            int length = 0;
            while (Marshal.ReadByte(ptr, length) != 0)
                length++;
            
            byte[] bytes = new byte[length];
            Marshal.Copy(ptr, bytes, 0, length);
            return Utf8Encoding.GetString(bytes);
#endif
        }

        /// <summary>
        /// 将 C# 字符串转换为 sbyte 数组（用于 C API 的 char*）
        /// </summary>
        /// <param name="str">C# 字符串</param>
        /// <returns>sbyte 数组</returns>
        public static sbyte[] StringToSByteArray(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new sbyte[] { 0 };

            byte[] bytes = Utf8Encoding.GetBytes(str);
            sbyte[] result = new sbyte[bytes.Length + 1]; // +1 for null terminator
            
            for (int i = 0; i < bytes.Length; i++)
            {
                result[i] = (sbyte)bytes[i];
            }
            // null terminator 已经是 0
            
            return result;
        }

        /// <summary>
        /// 将 C 字符串指针 (sbyte*) 转换为 C# 字符串
        /// </summary>
        /// <param name="ptr">sbyte 指针</param>
        /// <returns>C# 字符串</returns>
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
            // 计算长度
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
        /// 释放由 StringToUtf8Ptr 分配的内存
        /// </summary>
        /// <param name="ptr">要释放的指针</param>
        public static void FreeUtf8Ptr(IntPtr ptr)
        {
            if (ptr != IntPtr.Zero)
                Marshal.FreeHGlobal(ptr);
        }

        /// <summary>
        /// 将 C# 字符串数组转换为 C 字符串指针数组
        /// </summary>
        /// <param name="strings">C# 字符串数组</param>
        /// <returns>指针数组，每个元素需要用 FreeUtf8Ptr 释放</returns>
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
        /// 释放字符串指针数组
        /// </summary>
        /// <param name="ptrs">指针数组</param>
        public static void FreeUtf8PtrArray(IntPtr[] ptrs)
        {
            if (ptrs == null)
                return;

            foreach (var ptr in ptrs)
            {
                FreeUtf8Ptr(ptr);
            }
        }

#if HAS_SPAN
        /// <summary>
        /// 将字符串转换为 UTF-8 编码的 Span（.NET Core 2.1+ / .NET 5+）
        /// </summary>
        /// <param name="str">C# 字符串</param>
        /// <param name="span">目标 Span</param>
        /// <returns>实际写入的字节数</returns>
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
        /// 将 UTF-8 编码的 Span 转换为字符串（.NET Core 2.1+ / .NET 5+）
        /// </summary>
        /// <param name="span">UTF-8 编码的 Span</param>
        /// <returns>C# 字符串</returns>
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
