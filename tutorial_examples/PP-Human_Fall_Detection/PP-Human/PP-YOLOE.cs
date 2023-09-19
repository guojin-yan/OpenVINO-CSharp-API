using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenVinoSharp;
using OpenCvSharp.Dnn;
using System.Runtime.InteropServices;

namespace PP_Human
{
    public class YOLOE
    {
        // Member variables
        private Core m_core; 
        private Model m_model;
        private CompiledModel m_compiled_model;
        private InferRequest m_infer_request;
        private string m_input_node_name = "image"; 
        private string m_output_node_name_1 = "tmp_20";
        private string m_output_node_name_2 = "concat_14.tmp_0"; 
        private Size m_input_size = new Size(640, 640); 
        private int m_output_length = 8400; 

        public YOLOE(string model_path, string device_name)
        {
            m_core = new Core();
            m_model = m_core.read_model(model_path);
            m_compiled_model = m_core.compile_model(m_model, device_name);
            m_infer_request = m_compiled_model.create_infer_request();
        }



        public ResBboxs predict(Mat image)
        {
            // 设置图片输入
            Mat input_mat = CvDnn.BlobFromImage(image, 1.0, m_input_size, 0, true, false);

            Tensor input_tensor = m_infer_request.get_input_tensor();
            Shape input_shape = input_tensor.get_shape();
            float[] input_data = new float[input_shape[1] * input_shape[2] * input_shape[3]];
            Marshal.Copy(input_mat.Ptr(0), input_data, 0, input_data.Length);
            input_tensor.set_data<float>(input_data);

            // 求取缩放大小
            double scale_x = (double)image.Width / (double)this.m_input_size.Width;
            double scale_y = (double)image.Height / (double)this.m_input_size.Height;
            Point2d scale_factor = new Point2d(scale_x, scale_y);
            // 模型推理
            m_infer_request.infer();

            Tensor output_con_tensor = m_infer_request.get_tensor(m_output_node_name_2);
            Tensor output_box_tensor = m_infer_request.get_tensor(m_output_node_name_1);
            // 读取置信值结果
            float[] results_con = output_con_tensor.get_data<float>(m_output_length);
            // 读取预测框
            float[] result_box = output_box_tensor.get_data<float>(4 * m_output_length);
            // 处理模型推理数据
            ResBboxs result = process_result(results_con, result_box, scale_factor);
            return result;
        }

        private ResBboxs process_result(float[] results_con, float[] result_box, Point2d scale_factor)
        {
            // 处理预测结果
            List<float> confidences = new List<float>();
            List<Rect> boxes = new List<Rect>();
            for (int c = 0; c < m_output_length; c++)
            {   // 重新构建
                Rect rect = new Rect((int)(result_box[4 * c] * scale_factor.X), (int)(result_box[4 * c + 1] * scale_factor.Y),
                    (int)((result_box[4 * c + 2] - result_box[4 * c]) * scale_factor.X),
                    (int)((result_box[4 * c + 3] - result_box[4 * c + 1]) * scale_factor.Y));
                boxes.Add(rect);
                confidences.Add(results_con[c]);
            }
            // 非极大值抑制获取结果候选框
            int[] indexes = new int[boxes.Count];
            CvDnn.NMSBoxes(boxes, confidences, 0.5f, 0.5f, out indexes);
            // 提取合格的结果
            List<Rect> boxes_result = new List<Rect>();
            List<float> con_result = new List<float>();
            List<int> clas_result = new List<int>();
            for (int i = 0; i < indexes.Length; i++)
            {
                boxes_result.Add(boxes[indexes[i]]);
                con_result.Add(confidences[indexes[i]]);
            }
            return new ResBboxs(boxes_result, con_result.ToArray(),clas_result.ToArray());
        }

        public void draw_boxes(ResBboxs result, ref Mat image) 
        {
            for (int i = 0; i < result.bboxs.Count; i++) 
            {
                Cv2.Rectangle(image, result.bboxs[i], new Scalar(255, 0, 0), 3);
                Cv2.PutText(image, "score: " + result.scores[i].ToString(), new Point(result.bboxs[i].X, result.bboxs[i].Y - 10),
                    HersheyFonts.HersheySimplex, 0.6, new Scalar(0, 0, 255), 2);
            }
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