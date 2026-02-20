// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using Xunit;

namespace OpenVinoSharp.Tests.TestHelpers
{
    /// <summary>
    /// 测试初始化 / Test initialization
    /// <para>确保在运行任何 OpenVINO 测试之前加载原生库。</para>
    /// </summary>
    public static class TestInitialization
    {
        private static bool _initialized = false;
        private static readonly object _lock = new object();

        /// <summary>
        /// 初始化 OpenVINO 运行时环境
        /// Initialize OpenVINO runtime environment
        /// </summary>
        public static void Initialize()
        {
            if (_initialized)
                return;

            lock (_lock)
            {
                if (_initialized)
                    return;

                try
                {
                    // 显式加载 OpenVINO 原生库
                    // Explicitly load OpenVINO native library
                    Ov.Initialize();
                    System.Diagnostics.Debug.WriteLine("OpenVINO native library loaded successfully in tests.");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to load OpenVINO native library: {ex.Message}");
                    // 不要抛出异常，让测试自行处理 / Don't throw, let tests handle it
                }

                _initialized = true;
            }
        }
    }

    /// <summary>
    /// 测试集合夹具 / Test collection fixture
    /// 用于确保在所有集成测试运行之前初始化 OpenVINO
    /// </summary>
    public class OpenVINOTestFixture : IDisposable
    {
        public OpenVINOTestFixture()
        {
            // 在测试集合开始之前初始化 / Initialize before test collection starts
            TestInitialization.Initialize();
        }

        public void Dispose()
        {
            // 清理资源（如果需要）/ Cleanup resources if needed
        }
    }

    /// <summary>
    /// 应用于所有 OpenVINO 集成测试的集合定义
    /// Collection definition applied to all OpenVINO integration tests
    /// </summary>
    [CollectionDefinition("OpenVINO Integration Tests", DisableParallelization = true)]
    public class OpenVINOIntegrationTestCollection : ICollectionFixture<OpenVINOTestFixture>
    {
        // 这个类没有代码，只是作为集合定义的标记
        // This class has no code, just serves as a marker for collection definition
    }
}
