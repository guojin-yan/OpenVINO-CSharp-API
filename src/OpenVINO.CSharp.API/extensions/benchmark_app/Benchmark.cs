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
using System.Collections.Generic;
using System.Linq;
using OpenVinoSharp.Internal;

namespace OpenVinoSharp.extensions.benchmark_app
{
    /// <summary>
    /// 模型性能评估类 / Model performance evaluation class
    /// </summary>
    public static class Benchmark
    {
        /// <summary>
        /// 使用同步推理请求 API 估计模型性能 / Estimate model performance using Synchronous Inference Request API
        /// </summary>
        /// <param name="model_path">推理模型路径 / The inference model path</param>
        /// <param name="device_name">设备名称，默认="CPU" / The device name, default="CPU"</param>
        /// <returns>运行状态码 / Running status code</returns>
        public static int sync_benchmark(string model_path, string device_name = "CPU")
        {
            OvLogger.Debug("Entering sync_benchmark");
            OvLogger.Debug($"Parameters: model_path={model_path}, device_name={device_name}");

            try
            {
                OvLogger.Info("OpenVINO");
                Version version = Ov.get_openvino_version();
                OvLogger.Info(version.description + "   " + version.buildNumber);
                OvLogger.Debug($"OpenVINO Version: {version.description}, Build: {version.buildNumber}");

                // 优化延迟 / Optimize for latency
                OvLogger.Debug("Configuring performance hint for latency optimization");
                Dictionary<string, string> latency = new Dictionary<string, string>();
                latency.Add("PERFORMANCE_HINT", "1");

                // 创建 ov::Core 并编译模型 / Create ov::Core and compile model
                OvLogger.Debug("Creating OpenVINO Core");
                Core core = new Core();
                
                OvLogger.Debug($"Reading model from: {model_path}");
                Model model = core.read_model(model_path);
                
                OvLogger.Debug($"Compiling model for device: {device_name}");
                OvLogger.Debug($"Compile options: PERFORMANCE_HINT=LATENCY");
                CompiledModel compiled_model = core.compile_model(model, device_name, latency);
                
                OvLogger.Debug("Creating inference request");
                InferRequest infer_request = compiled_model.create_infer_request();
                
                // 用随机数据填充输入张量 / Fill input tensors with random data
                ulong input_size = compiled_model.get_inputs_size();
                OvLogger.Debug($"Model has {input_size} input(s)");
                
                for (ulong i = 0; i < input_size; i++)
                {
                    using (Input input = compiled_model.get_input(i))
                    {
                        string input_name = input.get_any_name();
                        OvLogger.Debug($"Processing input[{i}]: name={input_name}");
                        
                        Tensor tensor = infer_request.get_tensor(input_name);
                        OvLogger.Debug($"Input[{i}] tensor shape: [{string.Join(",", tensor.shape)}], element_type: {tensor.element_type}");
                        
                        Common.fill_tensor_random(tensor);
                        OvLogger.Debug($"Input[{i}] filled with random data");
                    }
                }
                
                // 预热推理 / Warm-up inference
                OvLogger.Debug("Starting warm-up inference");
                infer_request.infer();
                OvLogger.Debug("Warm-up inference completed");
                
                // 基准测试 / Benchmark
                OvLogger.Debug("Starting benchmark loop");
                int niter = 10;
                List<double> latencies = new List<double>();

                TimeSpan seconds_to_run = TimeSpan.FromSeconds(10);
                DateTime start = DateTime.Now;
                var time_point = start;
                var time_point_to_finish = start + seconds_to_run;

                OvLogger.Debug($"Benchmark start time: {start:HH:mm:ss.fff}");
                OvLogger.Debug($"Target duration: {seconds_to_run.TotalSeconds}s or {niter} iterations minimum");

                int iteration = 0;
                while (time_point < time_point_to_finish || latencies.Count < niter)
                {
                    iteration++;
                    OvLogger.Debug($"Iteration {iteration}: starting inference");
                    
                    infer_request.infer();
                    var iter_end = DateTime.Now;
                    double iter_latency = (iter_end - time_point).TotalMilliseconds;
                    latencies.Add(iter_latency);
                    
                    OvLogger.Debug($"Iteration {iteration}: completed in {iter_latency:F3} ms");
                    time_point = iter_end;
                }

                var end = time_point;
                double duration = (end - start).TotalMilliseconds;
                
                OvLogger.Debug($"Benchmark completed: {iteration} iterations in {duration:F2} ms");
                
                // 报告结果 / Report results
                OvLogger.Info("计数 / Count:      " + latencies.Count.ToString() + " iterations");
                OvLogger.Info("持续时间 / Duration:   " + duration + " ms");
                OvLogger.Info("延迟 / Latency:");
                int percent = 50;
                new LatencyMetrics(latencies, "", percent).write_to_slog();
                OvLogger.Info("吞吐量 / Throughput: " + (latencies.Count * 1000 / duration).ToString("0.00") + " FPS");
                
                OvLogger.Debug("Exiting sync_benchmark successfully");
            }
            catch (Exception ex)
            {
                OvLogger.Error($"Benchmark failed: {ex.Message}");
                OvLogger.Debug($"Exception details: {ex}");
                return 1;
            }
            return 0;
        }
    }
}
