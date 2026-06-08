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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace OpenVinoSharp
{
    /// <summary>
    /// 跨平台原生库加载器 / Cross-platform native library loader
    /// <para>支持 Windows、Linux 和 macOS 系统 / Supports Windows, Linux and macOS systems</para>
    /// </summary>
    internal static class NativeLibraryLoader
    {
        // 库文件名常量 / Library file name constants
        private const string WindowsLibraryName = "openvino_c.dll";
        private const string LinuxLibraryName = "libopenvino_c.so";
        private const string MacOSLibraryName = "libopenvino_c.dylib";

        // NuGet 包名称常量 / NuGet package name constants
        private static readonly string[] OpenVINOPackageNames = new[]
        {
            "jyppx.openvino.csharp.api",
            "openvino.runtime",
            "openvino.runtime.win",
            "openvino.runtime.linux-x64",
            "openvino.runtime.linux-arm64",
            "openvino.runtime.osx-x64",
            "openvino.runtime.osx-arm64",
            "openvino.runtime.ubuntu.24-x86_64",
            "openvino.runtime.ubuntu.22-x86_64",
            "openvino.runtime.ubuntu.22-arm64",
            "openvino.runtime.macos-arm64",
            "openvino.runtime.macos-x86_64",
            "openvino",
            "openvino-csharp-api"
        };

        // 已加载的库句柄 / Loaded library handle
        private static IntPtr _libraryHandle = IntPtr.Zero;
        private static readonly object _lock = new object();
        private static bool _isLoaded = false;

        /// <summary>
        /// 获取当前平台的库文件名 / Get the library file name for the current platform
        /// </summary>
        public static string GetLibraryName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return WindowsLibraryName;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return LinuxLibraryName;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return MacOSLibraryName;
            else
                throw new PlatformNotSupportedException("Unsupported operating system");
        }

        /// <summary>
        /// 获取当前平台标识 / Get the current platform identifier
        /// </summary>
        public static string GetPlatformIdentifier()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return "windows";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return "linux";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return "macos";
            else
                return "unknown";
        }

        /// <summary>
        /// 获取处理器架构标识 / Get the processor architecture identifier
        /// </summary>
        public static string GetArchitectureIdentifier()
        {
            return RuntimeInformation.ProcessArchitecture.ToString().ToLower();
        }

        /// <summary>
        /// 获取当前 .NET runtime identifier / Get the current .NET runtime identifier.
        /// </summary>
        /// <returns>运行时标识符，例如 win-x64、linux-arm64 或 osx-x64 / RID such as win-x64, linux-arm64 or osx-x64.</returns>
        public static string GetRuntimeIdentifier()
        {
            string os;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                os = "win";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                os = "linux";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                os = "osx";
            else
                os = "unknown";

            string arch;
            switch (RuntimeInformation.ProcessArchitecture)
            {
                case Architecture.X64:
                    arch = "x64";
                    break;
                case Architecture.X86:
                    arch = "x86";
                    break;
                case Architecture.Arm64:
                    arch = "arm64";
                    break;
                case Architecture.Arm:
                    arch = "arm";
                    break;
                default:
                    arch = RuntimeInformation.ProcessArchitecture.ToString().ToLower();
                    break;
            }

            return $"{os}-{arch}";
        }

        /// <summary>
        /// 加载原生库 / Load native library
        /// </summary>
        /// <param name="libraryPath">库文件路径（可选，默认为 null，使用平台默认搜索路径）/ Library file path (optional, defaults to null, uses platform default search path)</param>
        /// <returns>库句柄 / Library handle</returns>
        public static IntPtr Load(string libraryPath = null)
        {
            if (_isLoaded && _libraryHandle != IntPtr.Zero)
                return _libraryHandle;

            lock (_lock)
            {
                if (_isLoaded && _libraryHandle != IntPtr.Zero)
                    return _libraryHandle;

                string libName = libraryPath ?? GetLibraryName();
                // 尝试加载库 / Try to load library
                if (File.Exists(libraryPath))
                {
                    _libraryHandle = LoadLibraryInternal(libraryPath);
                }
                if (_libraryHandle == IntPtr.Zero)
                {
                    // 尝试从常见路径加载 / Try to load from common paths
                    _libraryHandle = TryLoadFromCommonPaths(libName);
                }

                if (_libraryHandle == IntPtr.Zero)
                {
                    // 尝试从 NuGet 包缓存加载 / Try to load from NuGet package cache
                    _libraryHandle = TryLoadFromNuGetCache(libName);
                }

                if (_libraryHandle == IntPtr.Zero)
                {
                    string searchedPaths = string.Join(Environment.NewLine + "  ", GetPossibleLibraryPaths(libName));
                    throw new DllNotFoundException(
                        $"Failed to load native library '{libName}'. " +
                        $"Platform: {GetPlatformIdentifier()}, Architecture: {GetArchitectureIdentifier()}, RID: {GetRuntimeIdentifier()}. " +
                        $"Please ensure OpenVINO runtime is installed. Searched paths:{Environment.NewLine}  {searchedPaths}");
                }

                _isLoaded = true;
                return _libraryHandle;
            }
        }

        /// <summary>
        /// 尝试从常见路径加载库 / Try to load library from common paths
        /// </summary>
        private static IntPtr TryLoadFromCommonPaths(string libName)
        {
            // 获取可能的库路径列表 / Get list of possible library paths
            string[] possiblePaths = GetPossibleLibraryPaths(libName);
            
            foreach (string path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    IntPtr handle = LoadLibraryInternal(path);
                    if (handle != IntPtr.Zero)
                        return handle;
                }
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// 尝试从 NuGet 包缓存加载库 / Try to load library from NuGet package cache
        /// </summary>
        private static IntPtr TryLoadFromNuGetCache(string libName)
        {
            string[] nugetCachePaths = GetNuGetCachePaths();
            string platform = GetPlatformIdentifier();
            string arch = GetArchitectureIdentifier();
            string rid = GetRuntimeIdentifier();
            
            foreach (string cachePath in nugetCachePaths)
            {
                if (!Directory.Exists(cachePath))
                    continue;

                // 搜索 OpenVINO 相关的包 / Search for OpenVINO related packages
                foreach (string packageName in OpenVINOPackageNames)
                {
                    string packagePath = Path.Combine(cachePath, packageName.ToLower());
                    if (!Directory.Exists(packagePath))
                        continue;

                    // 获取最新版本 / Get the latest version
                    string versionPath = GetLatestVersionPath(packagePath);
                    if (string.IsNullOrEmpty(versionPath))
                        continue;

                    // 构建可能的库路径 / Build possible library paths
                    string[] possiblePaths = new[]
                    {
                        // runtimes/{rid}/native/{libName}
                        Path.Combine(versionPath, "runtimes", rid, "native", libName),
                        // runtimes/{platform}-{arch}/native/{libName}
                        Path.Combine(versionPath, "runtimes", $"{platform}-{arch}", "native", libName),
                        // runtimes/{platform}/native/{libName}
                        Path.Combine(versionPath, "runtimes", platform, "native", libName),
                        // native/{libName}
                        Path.Combine(versionPath, "native", libName),
                        // 直接在包根目录
                        Path.Combine(versionPath, libName),
                        // lib/{arch}/{libName}
                        Path.Combine(versionPath, "lib", arch, libName),
                        // lib/native/{libName}
                        Path.Combine(versionPath, "lib", "native", libName),
                        // build/{libName}
                        Path.Combine(versionPath, "build", libName),
                        // build/native/{libName}
                        Path.Combine(versionPath, "build", "native", libName),
                        // build/{arch}/{libName}
                        Path.Combine(versionPath, "build", arch, libName)
                    };

                    foreach (string path in possiblePaths)
                    {
                        if (File.Exists(path))
                        {
                            IntPtr handle = LoadLibraryInternal(path);
                            if (handle != IntPtr.Zero)
                                return handle;
                        }
                    }
                }
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// 获取 NuGet 包缓存路径列表 / Get NuGet package cache path list
        /// </summary>
        private static string[] GetNuGetCachePaths()
        {
            var paths = new System.Collections.Generic.List<string>();
            
            // 1. 从 NUGET_PACKAGES 环境变量获取 / 1. Get from NUGET_PACKAGES environment variable
            string nugetPackages = Environment.GetEnvironmentVariable("NUGET_PACKAGES");
            if (!string.IsNullOrEmpty(nugetPackages))
            {
                paths.Add(nugetPackages);
            }

            // 2. 用户级缓存 / 2. User-level cache
            string userCache = GetUserNuGetCachePath();
            if (!string.IsNullOrEmpty(userCache))
            {
                paths.Add(userCache);
            }

            // 3. 全局缓存（适用于 .NET Core 2.1+ / .NET 5+） / 3. Global cache (for .NET Core 2.1+ / .NET 5+)
            string globalCache = GetGlobalNuGetCachePath();
            if (!string.IsNullOrEmpty(globalCache))
            {
                paths.Add(globalCache);
            }

            return paths.ToArray();
        }

        /// <summary>
        /// 获取用户级 NuGet 缓存路径 / Get user-level NuGet cache path
        /// </summary>
        private static string GetUserNuGetCachePath()
        {
            try
            {
                // Windows / Windows platform: %USERPROFILE%\.nuget\packages
                // Linux/macOS / Linux or macOS platform: ~/.nuget/packages
                string homePath = GetHomePath();
                if (!string.IsNullOrEmpty(homePath))
                {
                    return Path.Combine(homePath, ".nuget", "packages");
                }
            }
            catch { }
            return null;
        }

        /// <summary>
        /// 获取全局 NuGet 缓存路径 / Get global NuGet cache path
        /// </summary>
        private static string GetGlobalNuGetCachePath()
        {
            try
            {
                // 尝试从 dotnet nuget locals 获取，但这里简化处理 / Try to get from dotnet nuget locals, simplified here
                // Windows: %LOCALAPPDATA%\NuGet\v3-cache 或 %USERPROFILE%\.nuget\packages
                // 直接使用用户目录作为备选 / Use user directory as fallback
                return GetUserNuGetCachePath();
            }
            catch { }
            return null;
        }

        /// <summary>
        /// 获取用户主目录 / Get user home directory
        /// </summary>
        private static string GetHomePath()
        {
            // Windows
            string userProfile = Environment.GetEnvironmentVariable("USERPROFILE");
            if (!string.IsNullOrEmpty(userProfile))
                return userProfile;

            // Linux/macOS
            string home = Environment.GetEnvironmentVariable("HOME");
            if (!string.IsNullOrEmpty(home))
                return home;

            return null;
        }

        /// <summary>
        /// 获取包的最新版本路径 / Get the latest version path of the package
        /// </summary>
        private static string GetLatestVersionPath(string packagePath)
        {
            if (!Directory.Exists(packagePath))
                return null;

            try
            {
                // 获取所有版本目录 / Get all version directories
                var versionDirs = Directory.GetDirectories(packagePath)
                    .Where(d => !Path.GetFileName(d).ToLower().StartsWith(".")) // 排除隐藏目录 / Exclude hidden directories
                    .Select(d => new { Path = d, Name = Path.GetFileName(d) })
                    .Where(d => IsValidVersion(d.Name))
                    .ToList();

                if (versionDirs.Count == 0)
                    return null;

                // 按版本号排序，返回最新的 / Sort by version number, return the latest
                var latest = versionDirs
                    .OrderByDescending(d => ParseVersion(d.Name), new VersionComparer())
                    .FirstOrDefault();

                return latest?.Path;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 检查是否为有效的版本号 / Check if it is a valid version number
        /// </summary>
        private static bool IsValidVersion(string version)
        {
            if (string.IsNullOrEmpty(version))
                return false;

            // 简单的版本号检查（例如：2025.4.0, 1.0.0, 1.0.0-beta 等） / Simple version number check (e.g., 2025.4.0, 1.0.0, 1.0.0-beta, etc.)
            return System.Text.RegularExpressions.Regex.IsMatch(version, @"^\d+(\.\d+)+");
        }

        /// <summary>
        /// 解析版本号用于排序 / Parse version number for sorting
        /// </summary>
        private static System.Version ParseVersion(string version)
        {
            try
            {
                // 移除预发布标签（如 -beta, -rc1） / Remove pre-release tags (e.g., -beta, -rc1)
                string cleanVersion = version.Split('-')[0];
                return System.Version.Parse(cleanVersion);
            }
            catch
            {
                return new System.Version(0, 0, 0, 0);
            }
        }

        /// <summary>
        /// 版本号比较器 / Version number comparer
        /// </summary>
        private class VersionComparer : System.Collections.Generic.IComparer<System.Version>
        {
            public int Compare(System.Version x, System.Version y)
            {
                return x.CompareTo(y);
            }
        }

        /// <summary>
        /// 获取可能的库文件路径列表 / Get list of possible library file paths
        /// </summary>
        private static string[] GetPossibleLibraryPaths(string libName)
        {
            var paths = new System.Collections.Generic.List<string>();
            string baseFileName = Path.GetFileNameWithoutExtension(libName);
            string extension = Path.GetExtension(libName);
            string rid = GetRuntimeIdentifier();
            
            // 当前目录 / Current directory
            paths.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, libName));
            paths.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "runtimes", rid, "native", libName));
            
            // 平台特定子目录 / Platform-specific subdirectory
            string platform = GetPlatformIdentifier();
            string arch = GetArchitectureIdentifier();
            string openvinoRuntimeDir = Environment.GetEnvironmentVariable("OPENVINO_RUNTIME_DIR");
            if (!string.IsNullOrEmpty(openvinoRuntimeDir))
            {
                paths.Add(Path.Combine(openvinoRuntimeDir, libName));
                paths.Add(Path.Combine(openvinoRuntimeDir, "bin", libName));
                paths.Add(Path.Combine(openvinoRuntimeDir, "runtime", "bin", "intel64", "Release", libName));
                paths.Add(Path.Combine(openvinoRuntimeDir, "runtime", "lib", arch, libName));
            }

            // Windows 特定路径 / Windows-specific paths
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // 环境变量 PATH 中的路径会在 LoadLibrary 中自动搜索 / Paths in PATH environment variable are searched automatically by LoadLibrary
                // 检查程序集所在目录 / Check assembly directory
                string assemblyLocation = typeof(NativeLibraryLoader).Assembly.Location;
                if (!string.IsNullOrEmpty(assemblyLocation))
                {
                    // 测试项目路径存在问题 / Test project path issue exists
                    string assemblyDir = Path.GetDirectoryName(assemblyLocation);
                    paths.Add(Path.Combine(assemblyDir, libName));
                    paths.Add(Path.Combine(assemblyDir, "dll", "win-x64", libName));
                    paths.Add(Path.Combine(assemblyDir, "runtimes", rid, "native", libName));
                    paths.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dll", "win-x64", libName));
                    paths.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "runtimes", rid, "native", libName));
                }
                
                // 检查 OPENVINO_DIR 环境变量 / Check OPENVINO_DIR environment variable
                string openvinoDir = Environment.GetEnvironmentVariable("INTEL_OPENVINO_DIR");
                if (!string.IsNullOrEmpty(openvinoDir))
                {
                    paths.Add(Path.Combine(openvinoDir, "runtime", "bin", "intel64", "Release", libName));
                    paths.Add(Path.Combine(openvinoDir, libName));
                }
            }
            // Linux/macOS 特定路径 / Linux/macOS-specific paths
            else
            {
                // 标准库路径 / Standard library paths
                paths.Add($"/usr/lib/{libName}");
                paths.Add($"/usr/local/lib/{libName}");
                paths.Add($"/opt/intel/openvino/runtime/lib/{arch}/{libName}");
                
                // LD_LIBRARY_PATH 环境变量中的路径会在 dlopen 中自动搜索 / Paths in LD_LIBRARY_PATH environment variable are searched automatically by dlopen
                
                string assemblyLocation = typeof(NativeLibraryLoader).Assembly.Location;
                if (!string.IsNullOrEmpty(assemblyLocation))
                {
                    string assemblyDir = Path.GetDirectoryName(assemblyLocation);
                    paths.Add(Path.Combine(assemblyDir, libName));
                    paths.Add(Path.Combine(assemblyDir, "runtimes", rid, "native", libName));
                }
            }

            return paths.ToArray();
        }

        /// <summary>
        /// 获取已加载库的函数指针 / Get function pointer from loaded library
        /// </summary>
        /// <param name="functionName">函数名 / Function name</param>
        /// <returns>函数指针 / Function pointer</returns>
        public static IntPtr GetFunctionPointer(string functionName)
        {
            EnsureLoaded();
            return GetFunctionPointerInternal(_libraryHandle, functionName);
        }

        /// <summary>
        /// 确保库已加载（如果尚未加载则自动加载） / Ensure library is loaded (auto-load if not already loaded)
        /// </summary>
        public static void EnsureLoaded()
        {
            if (!_isLoaded || _libraryHandle == IntPtr.Zero)
            {
                Load();
            }
        }

        /// <summary>
        /// 释放已加载的库 / Free the loaded library
        /// </summary>
        public static void Free()
        {
            lock (_lock)
            {
                if (_libraryHandle != IntPtr.Zero)
                {
                    FreeLibraryInternal(_libraryHandle);
                    _libraryHandle = IntPtr.Zero;
                    _isLoaded = false;
                }
            }
        }

        #region 平台特定的实现 / Platform-specific implementations

#if HAS_NATIVELIBRARY
        // .NET Core 3.0+ / .NET 5+ 使用 NativeLibrary API / .NET Core 3.0+ / .NET 5+ uses NativeLibrary API
        
        private static IntPtr LoadLibraryInternal(string libraryPath)
        {
            try
            {
                return NativeLibrary.Load(libraryPath);
            }
            catch (DllNotFoundException)
            {
                return IntPtr.Zero;
            }
        }

        private static IntPtr GetFunctionPointerInternal(IntPtr handle, string functionName)
        {
            return NativeLibrary.GetExport(handle, functionName);
        }

        private static void FreeLibraryInternal(IntPtr handle)
        {
            NativeLibrary.Free(handle);
        }

#else
        // .NET Framework 使用 P/Invoke / .NET Framework uses P/Invoke

        private static IntPtr LoadLibraryInternal(string libraryPath)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                //return LoadLibrary_Windows(libraryPath);
                SetDllDirectory_Windows(Path.GetDirectoryName(libraryPath));
                IntPtr handle = LoadLibrary_Windows(libraryPath);
        
                if (handle == IntPtr.Zero)
                {
                    // 获取系统错误码 / Get system error code
                    int errorCode = Marshal.GetLastWin32Error();
                    //throw new Exception($"无法加载 DLL: {libraryPath}。系统错误码: {errorCode}");
                }
                return handle;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return LoadLibrary_Linux(libraryPath, RTLD_LAZY | RTLD_LOCAL);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return LoadLibrary_MacOS(libraryPath, RTLD_LAZY | RTLD_LOCAL);
            }
            else
            {
                return IntPtr.Zero;
            }
        }

        private static IntPtr GetFunctionPointerInternal(IntPtr handle, string functionName)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return GetProcAddress_Windows(handle, functionName);
            }
            else
            {
                return dlsym(handle, functionName);
            }
        }

        private static void FreeLibraryInternal(IntPtr handle)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                FreeLibrary_Windows(handle);
            }
            else
            {
                dlclose(handle);
            }
        }

        // Windows API
        [DllImport("kernel32", EntryPoint = "LoadLibrary", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr LoadLibrary_Windows(string lpFileName);

        [DllImport("kernel32", EntryPoint = "GetProcAddress", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr GetProcAddress_Windows(IntPtr hModule, string lpProcName);

        [DllImport("kernel32", EntryPoint = "FreeLibrary", SetLastError = true)]
        private static extern bool FreeLibrary_Windows(IntPtr hModule);

        // 导入 API / Import API
        [DllImport("kernel32.dll", EntryPoint = "SetDllDirectory", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool SetDllDirectory_Windows(string lpPathName);



        // Linux API / Linux API
        private const int RTLD_LAZY = 0x00001;
        private const int RTLD_NOW = 0x00002;
        private const int RTLD_LOCAL = 0x00000;
        private const int RTLD_GLOBAL = 0x00100;

        [DllImport("libdl", EntryPoint = "dlopen", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr LoadLibrary_Linux(string filename, int flags);

        [DllImport("libdl", EntryPoint = "dlopen", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr dlopen(string filename, int flags);

        [DllImport("libdl", EntryPoint = "dlsym", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr dlsym(IntPtr handle, string symbol);

        [DllImport("libdl", EntryPoint = "dlclose", SetLastError = true)]
        private static extern int dlclose(IntPtr handle);

        [DllImport("libdl", EntryPoint = "dlerror", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr dlerror();

        // macOS API (使用相同的 libdl) / macOS API (uses libSystem.dylib)
        [DllImport("libSystem.dylib", EntryPoint = "dlopen", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr LoadLibrary_MacOS(string filename, int flags);

        [DllImport("libSystem.dylib", EntryPoint = "dlopen", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr dlopen_mac(string filename, int flags);

        [DllImport("libSystem.dylib", EntryPoint = "dlsym", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr dlsym_mac(IntPtr handle, string symbol);

        [DllImport("libSystem.dylib", EntryPoint = "dlclose", SetLastError = true)]
        private static extern int dlclose_mac(IntPtr handle);

#endif

        #endregion
    }
}
