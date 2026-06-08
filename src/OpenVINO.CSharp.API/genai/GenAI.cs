// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

namespace OpenVinoSharp.GenAI
{
    /// <summary>
    /// OpenVINO GenAI 全局入口 / Global entry point for OpenVINO GenAI.
    /// </summary>
    public static class GenAI
    {
        /// <summary>
        /// 初始化 GenAI 原生运行时 / Initializes the GenAI native runtime.
        /// </summary>
        /// <param name="libraryPath">
        /// 可选的 <c>openvino_genai_c</c> 完整路径；为空时自动搜索。
        /// Optional full path to <c>openvino_genai_c</c>; auto-searches when null.
        /// </param>
        public static void Initialize(string libraryPath = null)
        {
            GenAINativeLibraryLoader.Load(libraryPath);
        }

        /// <summary>
        /// 尝试初始化 GenAI 原生运行时 / Tries to initialize the GenAI native runtime.
        /// </summary>
        /// <param name="errorMessage">失败原因 / Failure reason.</param>
        /// <returns>如果 GenAI 可用则为 true / True when GenAI is available.</returns>
        public static bool TryInitialize(out string errorMessage)
        {
            return GenAINativeLibraryLoader.TryEnsureLoaded(out errorMessage);
        }

        /// <summary>
        /// 检查当前进程是否可以加载 GenAI runtime / Checks whether GenAI runtime can be loaded in this process.
        /// </summary>
        public static bool IsAvailable
        {
            get
            {
                string _;
                return TryInitialize(out _);
            }
        }
    }
}

