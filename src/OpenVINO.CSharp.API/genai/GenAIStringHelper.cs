// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp.GenAI
{
    internal delegate ExceptionStatus GenAIStringGetter(IntPtr handle, IntPtr output, ref UIntPtr outputSize);
    internal delegate GenAIJsonContainerStatus GenAIJsonStringGetter(IntPtr handle, IntPtr output, ref UIntPtr outputSize);

    internal static class GenAIStringHelper
    {
        public static string GetString(IntPtr handle, GenAIStringGetter getter)
        {
            UIntPtr nativeSize = UIntPtr.Zero;
            ExceptionHandler.ThrowOnError(getter(handle, IntPtr.Zero, ref nativeSize));
            ulong size = StringUtils.FromNativeSize(nativeSize);
            if (size == 0)
                return string.Empty;
            if (size > int.MaxValue)
                throw new OverflowException("Native string is too large for a managed buffer. / 原生字符串过大，无法放入托管缓冲区。");

            IntPtr buffer = Marshal.AllocHGlobal((int)size);
            try
            {
                UIntPtr writableSize = StringUtils.ToNativeSize(size);
                ExceptionHandler.ThrowOnError(getter(handle, buffer, ref writableSize));
                return StringUtils.Utf8PtrToString(buffer) ?? string.Empty;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        public static string GetJsonString(IntPtr handle, GenAIJsonStringGetter getter, string operation)
        {
            UIntPtr nativeSize = UIntPtr.Zero;
            GenAIStatus.ThrowOnError(getter(handle, IntPtr.Zero, ref nativeSize), operation);
            ulong size = StringUtils.FromNativeSize(nativeSize);
            if (size == 0)
                return string.Empty;
            if (size > int.MaxValue)
                throw new OverflowException("Native string is too large for a managed buffer. / 原生字符串过大，无法放入托管缓冲区。");

            IntPtr buffer = Marshal.AllocHGlobal((int)size);
            try
            {
                UIntPtr writableSize = StringUtils.ToNativeSize(size);
                GenAIStatus.ThrowOnError(getter(handle, buffer, ref writableSize), operation);
                return StringUtils.Utf8PtrToString(buffer) ?? string.Empty;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
    }
}
