﻿using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
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

namespace OpenVinoSharp.Extensions.model
{
    public class Yolov5Det : Predictor
    {
        private int m_categ_nums;
        private float m_det_thresh;
        private float m_det_nms_thresh;
        private float[] m_factors;
        private long[] m_input_size;
        private int m_output_length = 25200;
        private int m_batch_num;
        public Yolov5Det(Yolov5DetConfig config)
            : base(config.model_path, config.device, config.cache_dir, config.use_gpu, config.input_size)
        {
            m_categ_nums = config.categ_nums;
            m_det_thresh = config.det_thresh;
            m_det_nms_thresh = config.det_nms_thresh;
            m_input_size = config.input_size;

            m_batch_num = config.batch_num;
        }
        public DetResult predict(Mat image)
        {
            Mat mat = new Mat();
            CvInvoke.CvtColor(image, mat, ColorConversion.Bgr2Rgb);
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
                    CvInvoke.CvtColor(images[ino], mat, ColorConversion.Bgr2Rgb);
                    mat = Resize.letterbox_img(mat, (int)m_input_size[2], out m_factors[ino - beg_img_no]);
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
                Mat result_data = new Mat(m_output_length, 5 + m_categ_nums, DepthType.Cv32F, 1,
                Marshal.UnsafeAddrOfPinnedArrayElement(result, (4 + m_categ_nums) * m_output_length * b * 4), 4 * (5 + m_categ_nums));
                //result_data = result_data.T();

                // Storage results list
                List<Rectangle> position_boxes = new List<Rectangle>();
                List<int> class_ids = new List<int>();
                List<float> confidences = new List<float>();
                // Preprocessing output results
                for (int i = 0; i < result_data.Rows; i++)
                {
                    Mat mat = new Mat(result_data, new Rectangle(0, i, 5, 1));
                    float[,] data = (float[,])mat.GetData();

                    float confidence = data[0, 4];
                    if (confidence < this.m_det_thresh)
                    {
                        continue;
                    }
                    Mat classes_scores = new Mat(result_data, new Rectangle(5, i, m_categ_nums, 1));//GetArray(i, 5, classes_scores);
                    Point max_classId_point = new Point(), min_classId_point = new Point();
                    double max_score = 0, min_score = 0;
                    // Obtain the maximum value and its position in a set of data
                    CvInvoke.MinMaxLoc(classes_scores, ref min_score, ref max_score,
                        ref min_classId_point, ref max_classId_point);
                    // Confidence level between 0 ~ 1
                    // Obtain identification box information
                    if (max_score > 0.25)
                    {
                        float cx = data[0, 0];
                        float cy = data[0, 1];
                        float ow = data[0, 2];
                        float oh = data[0, 3];
                        int x = (int)((cx - 0.5 * ow) * this.m_factors[b]);
                        int y = (int)((cy - 0.5 * oh) * this.m_factors[b]);
                        int width = (int)(ow * this.m_factors[b]);
                        int height = (int)(oh * this.m_factors[b]);
                        Rectangle box = new Rectangle();
                        box.X = x;
                        box.Y = y;
                        box.Width = width;
                        box.Height = height;

                        position_boxes.Add(box);
                        class_ids.Add(max_classId_point.X);
                        confidences.Add((float)confidence);
                    }
                }

                // NMS non maximum suppression
                int[] indexes = DnnInvoke.NMSBoxes(position_boxes.ToArray(), confidences.ToArray(), this.m_det_thresh, this.m_det_nms_thresh);

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
