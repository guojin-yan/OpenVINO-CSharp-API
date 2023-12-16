using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenVinoSharp.Extensions.utility;

namespace OpenVinoSharp.Extensions
{
    public static class Benchmark
    {
        public static void sync_benchmark(string model_path, string device_name = "CPU") 
        {
            Slog.INFO("OpenVINO");
            Version version = Ov.get_openvino_version();
            Slog.INFO(version.description);
            Slog.INFO(version.buildNumber);

            Dictionary<string, string> latency = new Dictionary<string, string>();
            latency.Add("PERFORMANCE_HINT", "1");

            Core core = new Core();
            Model model = core.read_model(model_path);
            CompiledModel compiled_model = core.compile_model(model, device_name, latency);
            InferRequest infer_request = compiled_model.create_infer_request();
            foreach (var input in compiled_model.inputs()) 
            { }
        }
    }
}
