using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenVinoSharp.Extensions.utility;
namespace OpenVinoSharp.Extensions
{
    public static class OvExtensions
    {
        /// <summary>
        /// Print the input and output information of the model
        /// </summary>
        /// <param name="model">The openvino model.</param>
        public static void printf_model_info(Model model)
        {
            Slog.INFO("Inference Model");
            Slog.INFO("  Model name: " + model.get_friendly_name());
            Slog.INFO("  Input:");
            List<Input> inputs = model.inputs();
            foreach (var input in inputs)
            {
                Slog.INFO("     name: " + input.get_any_name());
                Slog.INFO("     type: " + input.get_element_type().c_type_string());
                Slog.INFO("     shape: " + input.get_partial_shape().to_string());
            }
            Slog.INFO("  Output:");
            List<Output> outputs = model.outputs();
            foreach (var output in outputs)
            {
                Slog.INFO("     name: " + output.get_any_name());
                Slog.INFO("     type: " + output.get_element_type().c_type_string());
                Slog.INFO("     shape: " + output.get_partial_shape().to_string());
            }
        }
    }
}
