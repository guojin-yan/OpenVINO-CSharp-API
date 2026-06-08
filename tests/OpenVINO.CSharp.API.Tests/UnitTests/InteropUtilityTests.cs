using System;
using System.Runtime.InteropServices;
using OpenVinoSharp;
using Xunit;

namespace OpenVinoSharp.Tests.UnitTests
{
    /// <summary>
    /// 互操作工具测试 / Interop utility tests.
    /// </summary>
    public class InteropUtilityTests
    {
        /// <summary>
        /// UTF-8 字符串往返转换应保留中文路径 / UTF-8 string round-trip should preserve Chinese paths.
        /// </summary>
        [Fact]
        public void Utf8PtrRoundTrip_PreservesChinesePath()
        {
            const string value = @"E:\模型\中文路径\推理.xml";
            IntPtr ptr = StringUtils.StringToUtf8Ptr(value);

            try
            {
                Assert.Equal(value, StringUtils.Utf8PtrToString(ptr));
            }
            finally
            {
                StringUtils.FreeUtf8Ptr(ptr);
            }
        }

        /// <summary>
        /// 空字符串应以 UTF-8 null 终止符形式往返 / Empty strings should round-trip as UTF-8 null-terminated strings.
        /// </summary>
        [Fact]
        public void Utf8PtrRoundTrip_PreservesEmptyString()
        {
            IntPtr ptr = StringUtils.StringToUtf8Ptr(string.Empty);

            try
            {
                Assert.Equal(string.Empty, StringUtils.Utf8PtrToString(ptr));
            }
            finally
            {
                StringUtils.FreeUtf8Ptr(ptr);
            }
        }

        /// <summary>
        /// size_t 辅助函数应按当前进程指针宽度转换 / size_t helpers should convert with the current pointer width.
        /// </summary>
        [Fact]
        public void NativeSizeHelpers_RoundTrip()
        {
            UIntPtr nativeSize = StringUtils.ToNativeSize(123UL);

            Assert.Equal(123UL, StringUtils.FromNativeSize(nativeSize));
        }

        /// <summary>
        /// runtime identifier 应匹配当前 OS 和 CPU 架构 / Runtime identifier should match the current OS and CPU architecture.
        /// </summary>
        [Fact]
        public void RuntimeIdentifier_MatchesCurrentPlatform()
        {
            string rid = NativeLibraryLoader.GetRuntimeIdentifier();

            Assert.Contains("-", rid);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Assert.StartsWith("win-", rid);
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                Assert.StartsWith("linux-", rid);
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                Assert.StartsWith("osx-", rid);

            switch (RuntimeInformation.ProcessArchitecture)
            {
                case Architecture.X64:
                    Assert.EndsWith("-x64", rid);
                    break;
                case Architecture.X86:
                    Assert.EndsWith("-x86", rid);
                    break;
                case Architecture.Arm64:
                    Assert.EndsWith("-arm64", rid);
                    break;
                case Architecture.Arm:
                    Assert.EndsWith("-arm", rid);
                    break;
            }
        }
    }
}
