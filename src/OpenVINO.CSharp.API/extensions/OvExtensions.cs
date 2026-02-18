// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using OpenVinoSharp.Internal;

namespace OpenVinoSharp.extensions
{
    /// <summary>
    /// OpenVINO 扩展方法 / OpenVINO Extension Methods
    /// </summary>
    public static class OvExtensions
    {
        /// <summary>
        /// 打印模型的输入输出信息 / Print the input and output information of the model
        /// </summary>
        /// <param name="model">OpenVINO 模型 / The OpenVINO model</param>
        public static void printf_model_info(Model model)
        {
            Logger.Info("推理模型 / Inference Model");
            Logger.Info("  模型名称 / Model name: " + model.get_friendly_name());
            Logger.Info("  输入 / Input:");
            ulong input_size = model.get_inputs_size();
            for (ulong i = 0; i < input_size; i++)
            {
                using (NodeInput input = model.get_input(i))
                {
                    Logger.Info("     名称 / name: " + input.get_any_name());
                    Logger.Info("     类型 / type: " + input.get_element_type().get_type().ToString());
                    Logger.Info("     形状 / shape: " + input.get_shape().ToString());
                }
            }
            Logger.Info("  输出 / Output:");
            ulong output_size = model.get_outputs_size();
            for (ulong i = 0; i < output_size; i++)
            {
                using (NodeOutput output = model.get_output(i))
                {
                    Logger.Info("     名称 / name: " + output.get_any_name());
                    Logger.Info("     类型 / type: " + output.get_element_type().get_type().ToString());
                    Logger.Info("     形状 / shape: " + output.get_shape().ToString());
                }
            }
        }
    }
}
