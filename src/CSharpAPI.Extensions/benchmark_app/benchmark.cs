using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenVinoSharp.Extensions.utility;

namespace OpenVinoSharp.Extensions
{
    /// <summary>
    /// Model performance evaluation class
    /// </summary>
    public static class Benchmark
    {
        /// <summary>
        /// This sample demonstrates how to estimate performance of a model using Synchronous Inference Request API. 
        /// </summary>
        /// <param name="model_path">The inference model path.</param>
        /// <param name="device_name">The device name, default="CPU".</param>
        /// <returns>Running status code.</returns>
        public static int sync_benchmark(string model_path, string device_name = "CPU") 
        {
            try
            {
                Slog.INFO("OpenVINO");
                Version version = Ov.get_openvino_version();
                Slog.INFO(version.description + "   " + version.buildNumber);

                // Optimize for latency. Most of the devices are configured for latency by default,
                // but there are exceptions like GNA
                Dictionary<string, string> latency = new Dictionary<string, string>();
                latency.Add("PERFORMANCE_HINT", "1");


                // Create ov::Core and use it to compile a model.
                // Select the device by providing the name as the second parameter to CLI.
                // Using MULTI device is pointless in sync scenario
                // because only one instance of ov::InferRequest is used
                Core core = new Core();
                Model model = core.read_model(model_path);
                CompiledModel compiled_model = core.compile_model(model, device_name, latency);
                InferRequest infer_request = compiled_model.create_infer_request();
                foreach (var input in compiled_model.inputs())
                {
                    Common.fill_tensor_random(infer_request.get_tensor(input));
                }
                // Fill input data for the infer_request
                infer_request.infer();
                // Benchmark for seconds_to_run seconds and at least niter iterations
                int niter = 10;
                List<double> latencies = new List<double>();

                TimeSpan seconds_to_run = TimeSpan.FromSeconds(10);
                DateTime start = DateTime.Now;
                var time_point = start;
                var time_point_to_finish = start + seconds_to_run;

                while (time_point < time_point_to_finish || latencies.Count() < niter)
                {
                    infer_request.infer();
                    var iter_end = DateTime.Now;
                    latencies.Add((iter_end - time_point).TotalMilliseconds);
                    time_point = iter_end;
                }

                var end = time_point;
                double duration = (end - start).TotalMilliseconds;
                // Report results
                Slog.INFO("Count:      " + latencies.Count.ToString() + " iterations");
                Slog.INFO("Duration:   " + duration + " ms");
                Slog.INFO("Latency:");
                int percent = 50;
                new LatencyMetrics(latencies, "", percent).write_to_slog();
                Slog.INFO("Throughput: " + (latencies.Count * 1000 / duration).ToString("0.00") + "FPS");
            }
            catch (Exception ex) {
                Slog.INFO(ex.Message);
                return 1;
            }
            return 0;
        }
    }
}
