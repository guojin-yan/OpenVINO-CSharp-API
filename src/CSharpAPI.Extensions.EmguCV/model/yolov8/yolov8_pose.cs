using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenVinoSharp.Extensions.result;
using System.Runtime.InteropServices;
using OpenVinoSharp.Extensions.process;
using Emgu.CV;
using Emgu.CV.Dnn;
using Emgu.CV.CvEnum;
using System.Drawing;
namespace OpenVinoSharp.Extensions.model
{
    public class Yolov8Pose : Predictor
    {
        private int m_categ_nums;
        private float m_det_thresh;
        private float m_det_nms_thresh;
        private float[] m_factors;
        private long[] m_input_size;
        private int m_batch_num;
        private int m_output_length;

        public Yolov8Pose(string model_path, string? device = null, int? categ_nums = null, bool? use_gpu = null,
            long[]? input_size = null, int? batch_num = null, string? cache_dir = null, float? det_thresh = null,
            float? det_nms_thresh = null)
            : base(model_path, device ?? Yolov8PoseOption.device, cache_dir ?? Yolov8PoseOption.cache_dir,
                  use_gpu ?? Yolov8PoseOption.use_gpu, input_size ?? Yolov8PoseOption.input_size)
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

        public PoseResult predict(Mat image)
        {
            Mat mat = new Mat();
            CvInvoke.CvtColor(image, mat, Emgu.CV.CvEnum.ColorConversion.Bgr2Rgb);
            m_factors = new float[1];
            mat = Resize.letterbox_img(mat, (int)m_input_size[2], out m_factors[0]);
            mat = Normalize.run(mat, true);
            float[] input_data = Permute.run(mat);
            float[] output_data = infer(input_data);
            return process_result(output_data, 1)[0];
        }
        public List<PoseResult> predict(List<Mat> images)
        {
            List<PoseResult> re_results = new List<PoseResult>();
            for (int beg_img_no = 0; beg_img_no < images.Count; beg_img_no += m_batch_num)
            {
                int end_img_no = Math.Min(images.Count, beg_img_no + m_batch_num);
                int batch_num = end_img_no - beg_img_no;
                List<Mat> norm_img_batch = new List<Mat>();
                m_factors = new float[batch_num];
                for (int ino = beg_img_no; ino < end_img_no; ino++)
                {
                    Mat mat = new Mat();
                    CvInvoke.CvtColor(images[ino], mat, Emgu.CV.CvEnum.ColorConversion.Bgr2Rgb);
                    mat = Resize.letterbox_img(mat, (int)m_input_size[2], out m_factors[ino - beg_img_no]);
                    mat = Normalize.run(mat, true);
                    norm_img_batch.Add(mat);
                }
                float[] input_data = PermuteBatch.run(norm_img_batch);
                float[] output_data = infer(input_data, new long[] { batch_num, 3, m_input_size[2], m_input_size[3] });
                List<PoseResult> results = process_result(output_data, batch_num);
                re_results.AddRange(results);
            }
            return re_results;

        }

        /// <summary>
        /// Result process
        /// </summary>
        /// <param name="result">Model prediction output</param>
        /// <returns>Model recognition results</returns>
        public List<PoseResult> process_result(float[] result, int batch)
        {
            List<PoseResult> re_result = new List<PoseResult>();
            for (int b = 0; b < batch; ++b)
            {
                Mat result_data = new Mat(56, m_output_length, DepthType.Cv32F,1,
                    Marshal.UnsafeAddrOfPinnedArrayElement(result, 56 * m_output_length * b * 4), 4 * m_output_length);
                result_data = result_data.T();
                List<Rectangle> position_boxes = new List<Rectangle>();
                List<float> confidences = new List<float>();
                List<PosePoint> pose_datas = new List<PosePoint>();
                for (int i = 0; i < result_data.Rows; i++)
                {
                    if (((float[,])result_data.GetData())[i, 4] > 0.25)
                    {
                        Mat mat = new Mat(result_data, new Rectangle(0, i, 4, 1));
                        float[,] data = (float[,])mat.GetData();
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
                        Mat pose_mat = new Mat(result_data, new Rectangle(5, i, 51, 1));//result_data.Row(i).ColRange(5, 56);
                   
                        float[,] pose_data = (float[,])pose_mat.GetData();
                        
                
                        PosePoint pose = new PosePoint(pose_data.Cast<float>().ToArray(), this.m_factors[b]);

                        position_boxes.Add(box);

                        confidences.Add(((float[,])result_data.GetData())[i, 4]);
                        pose_datas.Add(pose);
                    }
                }

                int[] indexes = DnnInvoke.NMSBoxes(position_boxes.ToArray(), confidences.ToArray(), this.m_det_thresh, this.m_det_nms_thresh);

                PoseResult re = new PoseResult();
                for (int i = 0; i < indexes.Length; i++)
                {
                    int index = indexes[i];
                    re.add(confidences[index], position_boxes[index], pose_datas[index]);
                    //Console.WriteLine("rect: {0}, score: {1}", position_boxes[index], confidences[index]);
                }
                re_result.Add(re);
            }

            return re_result;

        }
    }
}
