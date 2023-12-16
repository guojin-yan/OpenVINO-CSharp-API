
using OpenCvSharp;
using OpenCvSharp.Dnn;
using OpenVinoSharp.Extensions.process;
using OpenVinoSharp.Extensions.result;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace OpenVinoSharp.Extensions.model
{
    public class Yolov8Det : Predictor
    {
        private int m_categ_nums;
        private float m_det_thresh;
        private float m_det_nms_thresh;
        private float[] m_factors;
        private long[] m_input_size;
        private int m_output_length = 8400;
        private int m_batch_num;

        public Yolov8Det(string model_path, string? device = null, int? categ_nums = null, bool? use_gpu = null,
            long[]? input_size = null, int? batch_num = null,string? cache_dir = null, float? det_thresh = null,
            float? det_nms_thresh = null)
            : base(model_path, device??Yolov8DetOption.device, cache_dir??Yolov8DetOption.cache_dir,
                  use_gpu?? Yolov8DetOption.use_gpu,input_size??Yolov8DetOption.input_size)
        {
            m_categ_nums = categ_nums ?? Yolov8DetOption.categ_nums;
            m_det_thresh = det_thresh ?? Yolov8DetOption.det_thresh;
            m_det_nms_thresh = det_nms_thresh ?? Yolov8DetOption.det_nms_thresh;
            m_input_size = input_size ?? Yolov8DetOption.input_size;

            m_output_length = (int)m_input_size[2] / 8 * (int)m_input_size[2] / 8 +
                 (int)m_input_size[2] / 16 * (int)m_input_size[2] / 16 +
                 (int)m_input_size[2] / 32 * (int)m_input_size[2] / 32;

            m_batch_num = batch_num ?? Yolov8DetOption.batch_num;
        }

        public DetResult predict(Mat image)
        {
            Mat mat = new Mat();
            Cv2.CvtColor(image, mat, ColorConversionCodes.BGR2RGB);
            m_factors = new float[1];
            mat = Resize.letterbox_img(mat, (int)m_input_size[2], out m_factors[0]);
            mat = Normalize.run(mat, true);
            float[] input_data = Permute.run(mat);
            float[] output_data = infer(input_data);
            return process_result(output_data, 1)[0];
        }

        public List<DetResult> predict(List<Mat> images)
        {
            List<DetResult> re_results = new List<DetResult>();
            for (int beg_img_no = 0; beg_img_no < images.Count; beg_img_no += m_batch_num) 
            {
                int end_img_no = Math.Min(images.Count, beg_img_no + m_batch_num);
                int batch_num = end_img_no - beg_img_no;
                List<Mat> norm_img_batch = new List<Mat>();
                m_factors = new float[batch_num];
                for (int ino = beg_img_no; ino < end_img_no; ino++)
                {
                    Mat mat = new Mat();
                    Cv2.CvtColor(images[ino], mat, ColorConversionCodes.BGR2RGB);
                    mat = Resize.letterbox_img(mat, (int)m_input_size[2], out m_factors[ino- beg_img_no]);
                    mat = Normalize.run(mat, true);
                    norm_img_batch.Add(mat);
                }
                float[] input_data = PermuteBatch.run(norm_img_batch);
                float[] output_data = infer(input_data, new long[] { batch_num, 3, m_input_size[2], m_input_size[3] });
                List<DetResult> results = process_result(output_data, batch_num);
                re_results.AddRange(results);
            }
            return re_results;

        }


        /// <summary>
        /// Result process
        /// </summary>
        /// <param name="result">Model prediction output</param>
        /// <returns>Model recognition results</returns>
        public List<DetResult> process_result(float[] result, int batch)
        {
            List<DetResult> re_result = new List<DetResult>();
            for (int b = 0; b < batch; ++b) 
            {
                Mat result_data = new Mat(4 + m_categ_nums, 8400, MatType.CV_32F,
                    Marshal.UnsafeAddrOfPinnedArrayElement(result, (4 + m_categ_nums) * m_output_length * b * 4), 4 * m_output_length);
                result_data = result_data.T();

                // Storage results list
                List<Rect> position_boxes = new List<Rect>();
                List<int> class_ids = new List<int>();
                List<float> confidences = new List<float>();
                // Preprocessing output results
                for (int i = 0; i < result_data.Rows; i++)
                {
                    Mat classes_scores = new Mat(result_data, new Rect(4, i, m_categ_nums, 1));
                    Point max_classId_point, min_classId_point;
                    double max_score, min_score;
                    // Obtain the maximum value and its position in a set of data
                    Cv2.MinMaxLoc(classes_scores, out min_score, out max_score,
                        out min_classId_point, out max_classId_point);
                    // Confidence level between 0 ~ 1
                    // Obtain identification box information
                    if (max_score > 0.25)
                    {
                        float cx = result_data.At<float>(i, 0);
                        float cy = result_data.At<float>(i, 1);
                        float ow = result_data.At<float>(i, 2);
                        float oh = result_data.At<float>(i, 3);
                        int x = (int)((cx - 0.5 * ow) * this.m_factors[b]);
                        int y = (int)((cy - 0.5 * oh) * this.m_factors[b]);
                        int width = (int)(ow * this.m_factors[b]);
                        int height = (int)(oh * this.m_factors[b]);
                        Rect box = new Rect();
                        box.X = x;
                        box.Y = y;
                        box.Width = width;
                        box.Height = height;

                        position_boxes.Add(box);
                        class_ids.Add(max_classId_point.X);
                        confidences.Add((float)max_score);
                    }
                }

                // NMS non maximum suppression
                int[] indexes = new int[position_boxes.Count];
                CvDnn.NMSBoxes(position_boxes, confidences, this.m_det_thresh, this.m_det_nms_thresh, out indexes);
                DetResult re = new DetResult();
                // 
                for (int i = 0; i < indexes.Length; i++)
                {
                    int index = indexes[i];
                    re.add(class_ids[index], confidences[index], position_boxes[index]);
                }
                re_result.Add(re);
            }
            return re_result;

        }
    }
}
