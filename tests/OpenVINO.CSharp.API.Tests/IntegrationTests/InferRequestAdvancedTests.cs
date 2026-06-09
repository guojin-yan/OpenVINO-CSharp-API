// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using System.Threading;
using Xunit;
using OpenVinoSharp.Tests.TestHelpers;

namespace OpenVinoSharp.Tests.IntegrationTests
{
    /// <summary>
    /// InferRequest 高级功能测试 / InferRequest advanced features tests
    /// </summary>
    [Collection("OpenVINO Integration Tests")]
    public class InferRequestAdvancedTests
    {
        static InferRequestAdvancedTests()
        {
            // 确保 OpenVINO 原生库已加载
            // Ensure OpenVINO native library is loaded
            TestInitialization.Initialize();
        }
        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void GetProfilingInfo_WithExecutedInference_ReturnsProfilingData()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var modelObj = core.read_model("model/yolo26n.xml");
            using var model = core.compile_model(modelObj, "CPU", null!);
            using var request = model.create_infer_request();

            // 执行推理 / Execute inference
            request.infer();

            // Act
            ProfilingInfo[] profilingInfo = request.get_profiling_info();

            // Assert
            Assert.NotNull(profilingInfo);
            Assert.True(profilingInfo.Length > 0, "应该有性能分析数据 / Should have profiling data");
            
            foreach (var info in profilingInfo)
            {
                Assert.NotNull(info.node_name);
                Assert.True(info.real_time >= 0, "实际时间应该非负 / Real time should be non-negative");
                Assert.True(info.cpu_time >= 0, "CPU时间应该非负 / CPU time should be non-negative");
            }
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void SetCallback_WithAsyncInference_CallbackIsInvoked()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var modelObj = core.read_model("model/yolo26n.xml");
            using var model = core.compile_model(modelObj, "CPU", null!);
            using var request = model.create_infer_request();

            bool callbackInvoked = false;
            var callbackEvent = new ManualResetEventSlim(false);

            // Act
            request.set_callback(() =>
            {
                callbackInvoked = true;
                callbackEvent.Set();
            });

            request.start_async();
            
            // 等待回调或超时 / Wait for callback or timeout
            bool completed = callbackEvent.Wait(TimeSpan.FromSeconds(10));

            // Assert
            Assert.True(completed, "回调应该在超时前被调用 / Callback should be invoked before timeout");
            Assert.True(callbackInvoked, "回调应该被调用 / Callback should be invoked");
        }

        [OpenVINOFact]
        [Trait("Category", TestCategories.Integration)]
        [Trait("Category", TestCategories.RequiresOpenVINO)]
        public void SetCallback_WithNull_ClearsCallback()
        {
            // Arrange
            using var core = new Core();
            if (!System.IO.File.Exists("model/yolo26n.xml"))
            {
                return;
            }
            using var modelObj = core.read_model("model/yolo26n.xml");
            using var model = core.compile_model(modelObj, "CPU", null!);
            using var request = model.create_infer_request();

            bool callbackInvoked = false;
            request.set_callback(() => callbackInvoked = true);

            // Act - 清除回调 / Clear callback
            request.set_callback(null!);
            request.start_async();
            request.wait();

            // Assert - 回调不应该被调用 / Callback should not be invoked
            // 注意：清除回调后，之前的回调不应该再被调用
            Assert.False(callbackInvoked, "清除回调后，回调不应该被调用 / Callback should not be invoked after clearing");
        }
    }
}
