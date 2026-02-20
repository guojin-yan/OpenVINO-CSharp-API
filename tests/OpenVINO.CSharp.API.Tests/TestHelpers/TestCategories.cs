// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

namespace OpenVinoSharp.Tests
{
    /// <summary>
    /// 测试类别常量 / Test category constants
    /// </summary>
    public static class TestCategories
    {
        /// <summary>
        /// 单元测试 / Unit tests
        /// </summary>
        public const string Unit = "Unit";

        /// <summary>
        /// 集成测试 / Integration tests
        /// </summary>
        public const string Integration = "Integration";

        /// <summary>
        /// 性能测试 / Performance tests
        /// </summary>
        public const string Performance = "Performance";

        /// <summary>
        /// 需要 OpenVINO 运行时的测试 / Tests requiring OpenVINO runtime
        /// </summary>
        public const string RequiresOpenVINO = "RequiresOpenVINO";

        /// <summary>
        /// 内存测试 / Memory tests
        /// </summary>
        public const string Memory = "Memory";
    }
}
