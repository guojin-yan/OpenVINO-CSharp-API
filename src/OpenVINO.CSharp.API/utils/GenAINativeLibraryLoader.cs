// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace OpenVinoSharp
{
    /// <summary>
    /// OpenVINO GenAI 原生库加载器 / Native library loader for OpenVINO GenAI.
    /// <para>
    /// 该加载器只负责 <c>openvino_genai_c</c>，避免影响既有 OpenVINO Core runtime 的加载行为。
    /// This loader only manages <c>openvino_genai_c</c> so the existing OpenVINO Core runtime loader remains stable.
    /// </para>
    /// </summary>
    internal static class GenAINativeLibraryLoader
    {
        private const string WindowsLibraryName = "openvino_genai_c.dll";
        private const string LinuxLibraryName = "libopenvino_genai_c.so";
        private const string MacOSLibraryName = "libopenvino_genai_c.dylib";

        private static readonly object LockObject = new object();
        private static readonly HashSet<string> WindowsSearchDirectories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private static IntPtr _libraryHandle = IntPtr.Zero;
        private static bool _isLoaded;

        /// <summary>
        /// 获取当前平台的 GenAI C API 库文件名 / Gets the GenAI C API library file name for the current platform.
        /// </summary>
        public static string GetLibraryName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return WindowsLibraryName;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return LinuxLibraryName;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return MacOSLibraryName;

            throw new PlatformNotSupportedException("Unsupported operating system for OpenVINO GenAI.");
        }

        /// <summary>
        /// 加载 GenAI 原生库 / Loads the GenAI native library.
        /// </summary>
        /// <param name="libraryPath">可选的库文件完整路径 / Optional full native library path.</param>
        /// <returns>已加载库的句柄 / Loaded library handle.</returns>
        public static IntPtr Load(string libraryPath = null)
        {
            if (_isLoaded && _libraryHandle != IntPtr.Zero)
                return _libraryHandle;

            lock (LockObject)
            {
                if (_isLoaded && _libraryHandle != IntPtr.Zero)
                    return _libraryHandle;

                string requestedPath = libraryPath;
                if (string.IsNullOrWhiteSpace(requestedPath))
                    requestedPath = Environment.GetEnvironmentVariable("OPENVINO_GENAI_C_LIBRARY");

                var loadFailures = new List<string>();
                if (!string.IsNullOrWhiteSpace(requestedPath))
                {
                    _libraryHandle = TryLoadExactPath(requestedPath, loadFailures);
                }

                if (_libraryHandle == IntPtr.Zero)
                {
                    foreach (string path in GetPossibleLibraryPaths(GetLibraryName()))
                    {
                        _libraryHandle = TryLoadExactPath(path, loadFailures);
                        if (_libraryHandle != IntPtr.Zero)
                            break;
                    }
                }

                if (_libraryHandle == IntPtr.Zero)
                {
                    string searchedPaths = string.Join(Environment.NewLine + "  ", GetPossibleLibraryPaths(GetLibraryName()));
                    throw new DllNotFoundException(
                        "Failed to load OpenVINO GenAI native library 'openvino_genai_c'. " +
                        $"RID: {NativeLibraryLoader.GetRuntimeIdentifier()}. " +
                        "Set OPENVINO_GENAI_RUNTIME_DIR or call GenAI.Initialize(fullPath). " +
                        $"Searched paths:{Environment.NewLine}  {searchedPaths}" +
                        FormatLoadFailures(loadFailures));
                }

                _isLoaded = true;
                return _libraryHandle;
            }
        }

        /// <summary>
        /// 尝试加载 GenAI 原生库，失败时返回错误文本 / Tries to load GenAI and returns the error text on failure.
        /// </summary>
        public static bool TryEnsureLoaded(out string errorMessage)
        {
            try
            {
                EnsureLoaded();
                errorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 确保 GenAI 原生库已加载 / Ensures the GenAI native library is loaded.
        /// </summary>
        public static void EnsureLoaded()
        {
            if (!_isLoaded || _libraryHandle == IntPtr.Zero)
            {
                Load();
            }
        }

        /// <summary>
        /// 释放 GenAI 原生库句柄 / Frees the GenAI native library handle.
        /// </summary>
        public static void Free()
        {
            lock (LockObject)
            {
                if (_libraryHandle != IntPtr.Zero)
                {
                    FreeLibraryInternal(_libraryHandle);
                    _libraryHandle = IntPtr.Zero;
                    _isLoaded = false;
                }
            }
        }

        /// <summary>
        /// 获取 GenAI 原生库可能存在的位置 / Gets possible search paths for the GenAI native library.
        /// </summary>
        internal static string[] GetPossibleLibraryPaths(string libraryName = null)
        {
            string libName = string.IsNullOrEmpty(libraryName) ? GetLibraryName() : libraryName;
            string rid = NativeLibraryLoader.GetRuntimeIdentifier();
            string arch = NativeLibraryLoader.GetArchitectureIdentifier();
            var paths = new List<string>();

            AddIfNotEmpty(paths, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, libName));
            AddIfNotEmpty(paths, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "runtimes", rid, "native", libName));
            AddIfNotEmpty(paths, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dll", "win-x64", libName));

            AddRuntimeRoot(paths, Environment.GetEnvironmentVariable("OPENVINO_GENAI_RUNTIME_DIR"), libName, arch);
            AddRuntimeRoot(paths, Environment.GetEnvironmentVariable("OPENVINO_RUNTIME_DIR"), libName, arch);
            AddRuntimeRoot(paths, Environment.GetEnvironmentVariable("INTEL_OPENVINO_DIR"), libName, arch);

            string assemblyLocation = typeof(GenAINativeLibraryLoader).Assembly.Location;
            if (!string.IsNullOrEmpty(assemblyLocation))
            {
                string assemblyDir = Path.GetDirectoryName(assemblyLocation);
                AddIfNotEmpty(paths, Path.Combine(assemblyDir, libName));
                AddIfNotEmpty(paths, Path.Combine(assemblyDir, "runtimes", rid, "native", libName));
                AddIfNotEmpty(paths, Path.Combine(assemblyDir, "dll", "win-x64", libName));
            }

            string pathVariable = Environment.GetEnvironmentVariable("PATH");
            if (!string.IsNullOrEmpty(pathVariable))
            {
                foreach (string item in pathVariable.Split(Path.PathSeparator))
                {
                    if (!string.IsNullOrWhiteSpace(item))
                        AddIfNotEmpty(paths, Path.Combine(item.Trim(), libName));
                }
            }

            return paths.Where(File.Exists).Concat(paths.Where(p => !File.Exists(p))).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
        }

        private static void AddRuntimeRoot(List<string> paths, string root, string libName, string arch)
        {
            if (string.IsNullOrWhiteSpace(root))
                return;

            AddIfNotEmpty(paths, Path.Combine(root, libName));
            AddIfNotEmpty(paths, Path.Combine(root, "bin", libName));
            AddIfNotEmpty(paths, Path.Combine(root, "runtime", "bin", "intel64", "Release", libName));
            AddIfNotEmpty(paths, Path.Combine(root, "runtime", "lib", arch, libName));
            AddIfNotEmpty(paths, Path.Combine(root, "runtimes", NativeLibraryLoader.GetRuntimeIdentifier(), "native", libName));
        }

        private static void AddIfNotEmpty(List<string> paths, string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
                paths.Add(path);
        }

        private static IntPtr TryLoadExactPath(string path, List<string> loadFailures)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                return IntPtr.Zero;

            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    int win32Error;
                    IntPtr handle = LoadLibraryWindows(path, out win32Error);
                    if (handle == IntPtr.Zero && win32Error != 0)
                    {
                        loadFailures?.Add($"{path}: Win32 error {win32Error} - {new Win32Exception(win32Error).Message}");
                    }

                    return handle;
                }

                return LoadLibraryInternal(path);
            }
            catch (Exception ex)
            {
                loadFailures?.Add($"{path}: {ex.Message}");
                return IntPtr.Zero;
            }
        }

        private static string FormatLoadFailures(IReadOnlyCollection<string> loadFailures)
        {
            if (loadFailures == null || loadFailures.Count == 0)
                return string.Empty;

            return Environment.NewLine + "Load failures:" + Environment.NewLine + "  " +
                string.Join(Environment.NewLine + "  ", loadFailures);
        }

#if HAS_NATIVELIBRARY
        private static IntPtr LoadLibraryInternal(string libraryPath)
        {
            try
            {
                return NativeLibrary.Load(libraryPath);
            }
            catch
            {
                return IntPtr.Zero;
            }
        }

        private static void FreeLibraryInternal(IntPtr handle)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                FreeLibrary_Windows(handle);
                return;
            }

            NativeLibrary.Free(handle);
        }
#else
        private static IntPtr LoadLibraryInternal(string libraryPath)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return LoadLibraryWindows(libraryPath);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return LoadLibrary_Linux(libraryPath, RTLD_LAZY | RTLD_LOCAL);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return LoadLibrary_MacOS(libraryPath, RTLD_LAZY | RTLD_LOCAL);

            return IntPtr.Zero;
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

        private const int RTLD_LAZY = 0x00001;
        private const int RTLD_LOCAL = 0x00000;

        [DllImport("libdl", EntryPoint = "dlopen", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr LoadLibrary_Linux(string filename, int flags);

        [DllImport("libSystem.dylib", EntryPoint = "dlopen", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr LoadLibrary_MacOS(string filename, int flags);

        [DllImport("libdl", EntryPoint = "dlclose", SetLastError = true)]
        private static extern int dlclose(IntPtr handle);
#endif

        private const int LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008;

        private static IntPtr LoadLibraryWindows(string libraryPath, out int win32Error)
        {
            win32Error = 0;
            string fullPath = Path.GetFullPath(libraryPath);
            ConfigureWindowsDependencySearchPath(fullPath);

            string dir = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrEmpty(dir))
                SetDllDirectory_Windows(dir);

            IntPtr handle = LoadLibraryEx_Windows(fullPath, IntPtr.Zero, LOAD_WITH_ALTERED_SEARCH_PATH);
            if (handle != IntPtr.Zero)
                return handle;

            win32Error = Marshal.GetLastWin32Error();
            handle = LoadLibrary_Windows(fullPath);
            if (handle != IntPtr.Zero)
            {
                win32Error = 0;
                return handle;
            }

            int fallbackError = Marshal.GetLastWin32Error();
            if (fallbackError != 0)
                win32Error = fallbackError;

            return IntPtr.Zero;
        }

        private static IntPtr LoadLibraryWindows(string libraryPath)
        {
            int ignoredError;
            return LoadLibraryWindows(libraryPath, out ignoredError);
        }

        private static void ConfigureWindowsDependencySearchPath(string libraryPath)
        {
            string[] directories = GetWindowsDependencyDirectories(libraryPath)
                .Where(Directory.Exists)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();
            if (directories.Length == 0)
                return;

            lock (WindowsSearchDirectories)
            {
                string pathVariable = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
                var existingPathItems = new HashSet<string>(
                    pathVariable.Split(Path.PathSeparator)
                        .Where(p => !string.IsNullOrWhiteSpace(p))
                        .Select(p => NormalizeDirectoryPath(p.Trim())),
                    StringComparer.OrdinalIgnoreCase);
                var newDirectories = new List<string>();

                foreach (string directory in directories)
                {
                    string normalized = NormalizeDirectoryPath(directory);
                    if (WindowsSearchDirectories.Add(normalized) && !existingPathItems.Contains(normalized))
                        newDirectories.Add(normalized);
                }

                if (newDirectories.Count > 0)
                {
                    string newPath = string.Join(Path.PathSeparator.ToString(), newDirectories);
                    if (!string.IsNullOrWhiteSpace(pathVariable))
                        newPath += Path.PathSeparator + pathVariable;

                    Environment.SetEnvironmentVariable("PATH", newPath);
                }
            }
        }

        private static IEnumerable<string> GetWindowsDependencyDirectories(string libraryPath)
        {
            string fullPath = Path.GetFullPath(libraryPath);
            string libraryDirectory = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrWhiteSpace(libraryDirectory))
                yield return libraryDirectory;

            string runtimeRoot = FindRuntimeRoot(libraryDirectory);
            if (string.IsNullOrWhiteSpace(runtimeRoot))
                yield break;

            yield return Path.Combine(runtimeRoot, "runtime", "bin", "intel64", "Release");
            yield return Path.Combine(runtimeRoot, "runtime", "3rdparty", "tbb", "bin");
            yield return Path.Combine(runtimeRoot, "bin");
            yield return Path.Combine(runtimeRoot, "runtime", "lib", NativeLibraryLoader.GetArchitectureIdentifier());
        }

        private static string FindRuntimeRoot(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
                return null;

            var current = new DirectoryInfo(directory);
            while (current != null)
            {
                string releaseBin = Path.Combine(current.FullName, "runtime", "bin", "intel64", "Release");
                if (Directory.Exists(releaseBin))
                    return current.FullName;

                current = current.Parent;
            }

            return null;
        }

        private static string NormalizeDirectoryPath(string directory)
        {
            return Path.GetFullPath(directory).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        [DllImport("kernel32", EntryPoint = "LoadLibrary", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadLibrary_Windows(string lpFileName);

        [DllImport("kernel32", EntryPoint = "LoadLibraryEx", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadLibraryEx_Windows(string lpFileName, IntPtr hFile, int dwFlags);

        [DllImport("kernel32", EntryPoint = "FreeLibrary", SetLastError = true)]
        private static extern bool FreeLibrary_Windows(IntPtr hModule);

        [DllImport("kernel32.dll", EntryPoint = "SetDllDirectory", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetDllDirectory_Windows(string lpPathName);
    }
}
