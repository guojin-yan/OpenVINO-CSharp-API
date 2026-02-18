// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

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
            Logger.Debug("Entering sync_benchmark");
            Logger.Debug($"Parameters: model_path={model_path}, device_name={device_name}");

            try
            {
                Logger.Info("OpenVINO");
                Version version = Ov.get_openvino_version();
                Logger.Info(version.description + "   " + version.buildNumber);
                Logger.Debug($"OpenVINO Version: {version.description}, Build: {version.buildNumber}");

                // 优化延迟 / Optimize for latency
                Logger.Debug("Configuring performance hint for latency optimization");
                Dictionary<string, string> latency = new Dictionary<string, string>();
                latency.Add("PERFORMANCE_HINT", "1");

                // 创建 ov::Core 并编译模型 / Create ov::Core and compile model
                Logger.Debug("Creating OpenVINO Core");
                Core core = new Core();
                
                Logger.Debug($"Reading model from: {model_path}");
                Model model = core.read_model(model_path);
                
                Logger.Debug($"Compiling model for device: {device_name}");
                Logger.Debug($"Compile options: PERFORMANCE_HINT=LATENCY");
                CompiledModel compiled_model = core.compile_model(model, device_name, latency);
                
                Logger.Debug("Creating inference request");
                InferRequest infer_request = compiled_model.create_infer_request();
                
                // 用随机数据填充输入张量 / Fill input tensors with random data
                ulong input_size = compiled_model.get_inputs_size();
                Logger.Debug($"Model has {input_size} input(s)");
                
                for (ulong i = 0; i < input_size; i++)
                {
                    using (NodeInput input = compiled_model.get_input(i))
                    {
                        string input_name = input.get_any_name();
                        Logger.Debug($"Processing input[{i}]: name={input_name}");
                        
                        Tensor tensor = infer_request.get_tensor(input_name);
                        Logger.Debug($"Input[{i}] tensor shape: [{string.Join(",", tensor.shape)}], element_type: {tensor.element_type}");
                        
                        Common.fill_tensor_random(tensor);
                        Logger.Debug($"Input[{i}] filled with random data");
                    }
                }
                
                // 预热推理 / Warm-up inference
                Logger.Debug("Starting warm-up inference");
                infer_request.infer();
                Logger.Debug("Warm-up inference completed");
                
                // 基准测试 / Benchmark
                Logger.Debug("Starting benchmark loop");
                int niter = 10;
                List<double> latencies = new List<double>();

                TimeSpan seconds_to_run = TimeSpan.FromSeconds(10);
                DateTime start = DateTime.Now;
                var time_point = start;
                var time_point_to_finish = start + seconds_to_run;

                Logger.Debug($"Benchmark start time: {start:HH:mm:ss.fff}");
                Logger.Debug($"Target duration: {seconds_to_run.TotalSeconds}s or {niter} iterations minimum");

                int iteration = 0;
                while (time_point < time_point_to_finish || latencies.Count < niter)
                {
                    iteration++;
                    Logger.Debug($"Iteration {iteration}: starting inference");
                    
                    infer_request.infer();
                    var iter_end = DateTime.Now;
                    double iter_latency = (iter_end - time_point).TotalMilliseconds;
                    latencies.Add(iter_latency);
                    
                    Logger.Debug($"Iteration {iteration}: completed in {iter_latency:F3} ms");
                    time_point = iter_end;
                }

                var end = time_point;
                double duration = (end - start).TotalMilliseconds;
                
                Logger.Debug($"Benchmark completed: {iteration} iterations in {duration:F2} ms");
                
                // 报告结果 / Report results
                Logger.Info("计数 / Count:      " + latencies.Count.ToString() + " iterations");
                Logger.Info("持续时间 / Duration:   " + duration + " ms");
                Logger.Info("延迟 / Latency:");
                int percent = 50;
                new LatencyMetrics(latencies, "", percent).write_to_slog();
                Logger.Info("吞吐量 / Throughput: " + (latencies.Count * 1000 / duration).ToString("0.00") + " FPS");
                
                Logger.Debug("Exiting sync_benchmark successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Benchmark failed: {ex.Message}");
                Logger.Debug($"Exception details: {ex}");
                return 1;
            }
            return 0;
        }
    }
}
