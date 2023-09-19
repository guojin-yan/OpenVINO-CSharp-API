using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenVinoSharp;
using OpenCvSharp.Dnn;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PP_Human
{


    public class STGCN
    {
        // Member variables
        private Core m_core;
        private Model m_model;
        private CompiledModel m_compiled_model;
        private InferRequest m_infer_request;
        private string m_input_node_name = "data_batch_0"; // 模型输入节点名称
        private string m_output_node_name = "reshape2_34.tmp_0"; // 模型预测输出节点名
        private Size2f m_coord_size = new Size2f(512, 384);
        private int m_input_length = 1700; // 模型输入节点形状
        private int m_output_length = 2; // 模型输出数据长度

        public STGCN(string model_path, string device_name)
        {
            m_core = new Core();
            m_model = m_core.read_model(model_path);
            m_compiled_model = m_core.compile_model(m_model, device_name);
            m_infer_request = m_compiled_model.create_infer_request();
        }



        public KeyValuePair<string, float> predict(List<KeyPoints> points)
        {
            // 转换数据格式
            float[] input_data = preprocess_keypoint(points);
            // 设置模型输入
            Tensor input_tensor = m_infer_request.get_input_tensor();
            input_tensor.set_data<float>(input_data);

            // 模型推理
            m_infer_request.infer();

            // 读取推理结果
            Tensor output_tensor = m_infer_request.get_output_tensor();
            float[] results = output_tensor.get_data<float>(m_output_length);

            Console.WriteLine("{0}   {1}", results[0], results[1]);
            KeyValuePair<string, float> result;
            if (results[0] > results[1])
            {
                result = new KeyValuePair<string, float>("falling", results[0]);
            }
            else 
            {
                result = new KeyValuePair<string, float>("unfalling", results[1]);
            }
            return result;
        }

        float[] preprocess_keypoint(List<KeyPoints> data) 
        { 
            float[] input_data = new float[this.m_input_length];
            // (50, 17, 2)->(2, 50, 17)
            for (int f = 0; f < 50; f++)
            {
                float[,] point = data[f].points;
                Rect rect = data[f].bbox;
                for (int g = 0; g < 17; g++)
                {
                    input_data[1 * 50 * 17 + f * 17 + g] = point[g, 0] / rect.Width * m_coord_size.Width;
                    input_data[0 * 50 * 17 + f * 17 + g] = point[g, 1] / rect.Height * m_coord_size.Height;
                }
            }
            return input_data;
        }

        public void draw_result(ref Mat image, KeyValuePair<string, float> result, Rect rect)
        {
            Cv2.PutText(image, result.Key + ": " + result.Value.ToString(), new Point(rect.X, rect.Y + rect.Height + 15),
                HersheyFonts.HersheySimplex, 0.6, new Scalar(0, 0, 255),2);
        }
        public void release()
        {
            m_infer_request.dispose();
            m_compiled_model.dispose();
            m_model.dispose();
            m_core.dispose();
        }
    }
}

