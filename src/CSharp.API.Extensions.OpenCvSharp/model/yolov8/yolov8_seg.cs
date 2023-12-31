using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenVinoSharp.Extensions.result;
using OpenVinoSharp.Extensions.process;
using OpenCvSharp.Dnn;
using System.Runtime.InteropServices;
namespace OpenVinoSharp.Extensions.model
{
    public class Yolov8Seg : Predictor
    {
        private int m_categ_nums;
        private float m_det_thresh;
        private float m_det_nms_thresh;
        private float[] m_factors;
        private long[] m_input_size;
        private int m_output_length = 8400;
        private int m_mask_length = 160;
        private int m_batch_num;
        private List<Size> m_image_sizes;

        public Yolov8Seg(string model_path, string? device = null, int? categ_nums = null, bool? use_gpu = null,
            long[]? input_size = null, int? batch_num = null, string? cache_dir = null, float? det_thresh = null,
            float? det_nms_thresh = null)
            : base(model_path, device ?? Yolov8DetOption.device, cache_dir ?? Yolov8DetOption.cache_dir,
                  use_gpu ?? Yolov8DetOption.use_gpu, input_size ?? Yolov8DetOption.input_size)
        {
            m_categ_nums = categ_nums ?? Yolov8DetOption.categ_nums;
            m_det_thresh = det_thresh ?? Yolov8DetOption.det_thresh;
            m_det_nms_thresh = det_nms_thresh ?? Yolov8DetOption.det_nms_thresh;
            m_input_size = input_size ?? Yolov8DetOption.input_size;

            m_output_length = (int)m_input_size[2] / 8 * (int)m_input_size[2] / 8 +
                 (int)m_input_size[2] / 16 * (int)m_input_size[2] / 16 +
                 (int)m_input_size[2] / 32 * (int)m_input_size[2] / 32;
            m_mask_length = (int)m_input_size[2] / 4;
            m_batch_num = batch_num ?? Yolov8DetOption.batch_num;
        }

        public Yolov8Seg(Yolov8SegConfig config)
            : base(config.model_path, config.device, config.cache_dir, config.use_gpu, config.input_size)
        {
            m_categ_nums = config.categ_nums;
            m_det_thresh = config.det_thresh;
            m_det_nms_thresh = config.det_nms_thresh;
            m_input_size = config.input_size;

            m_output_length = (int)m_input_size[2] / 8 * (int)m_input_size[2] / 8 +
                 (int)m_input_size[2] / 16 * (int)m_input_size[2] / 16 +
                 (int)m_input_size[2] / 32 * (int)m_input_size[2] / 32;

            m_batch_num = config.batch_num;
        }

        /// <summary>
        /// sigmoid
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private float sigmoid(float a)
        {
            float b = 1.0f / (1.0f + (float)Math.Exp(-a));
            return b;
        }

        public SegResult predict(Mat image)
        {
            Mat mat = new Mat();
            m_image_sizes = new List<Size>() { image.Size() };
            Cv2.CvtColor(image, mat, ColorConversionCodes.BGR2RGB);
            m_factors = new float[1];
            mat = Resize.letterbox_img(mat, (int)m_input_size[2], out m_factors[0]);
            mat = Normalize.run(mat, true);
            float[] input_data = Permute.run(mat);

            Tensor input_tensor = m_infer_request.get_input_tensor();
            input_tensor.set_data<float>(input_data);
            m_infer_request.infer();

            Tensor output_tensor_0 = m_infer_request.get_output_tensor(0);
            float[] result_detect = output_tensor_0.get_data<float>((int)output_tensor_0.get_size());

            Tensor output_tensor_1 = m_infer_request.get_output_tensor(1);
            float[] result_proto = output_tensor_1.get_data<float>((int)output_tensor_1.get_size());

            SegResult result = process_result(result_detect, result_proto, 1)[0];
            return result;

        }


        public List<SegResult> predict(List<Mat> images)
        {
            List<SegResult> re_results = new List<SegResult>();
            for (int beg_img_no = 0; beg_img_no < images.Count; beg_img_no += m_batch_num)
            {
                
                int end_img_no = Math.Min(images.Count, beg_img_no + m_batch_num);
                int batch_num = end_img_no - beg_img_no;
                List<Mat> norm_img_batch = new List<Mat>();
                m_factors = new float[batch_num];
                m_image_sizes = new List<Size>(batch_num);
                for (int ino = beg_img_no; ino < end_img_no; ino++)
                {
                    Mat mat = new Mat();
                    m_image_sizes.Add(images[ino].Size());
                    Cv2.CvtColor(images[ino], mat, ColorConversionCodes.BGR2RGB);
                    mat = Resize.letterbox_img(mat, (int)m_input_size[2], out m_factors[ino - beg_img_no]);
                    mat = Normalize.run(mat, true);
                    norm_img_batch.Add(mat);
                }
                float[] input_data = PermuteBatch.run(norm_img_batch);
                Tensor input_tensor = m_infer_request.get_input_tensor();
                input_tensor.set_shape(new Shape(new long[] { batch_num, 3, m_input_size[2], m_input_size[3] }));
                input_tensor.set_data<float>(input_data);
                m_infer_request.infer();
                Tensor output_tensor_0 = m_infer_request.get_output_tensor(0);
                float[] result_detect = output_tensor_0.get_data<float>((int)output_tensor_0.get_size());

                Tensor output_tensor_1 = m_infer_request.get_output_tensor(1);
                float[] result_proto = output_tensor_1.get_data<float>((int)output_tensor_1.get_size());
                List<SegResult> results = process_result(result_detect, result_proto, batch_num);
                re_results.AddRange(results);
            }
            return re_results;

        }

        /// <summary>
        /// Result process
        /// </summary>
        /// <param name="detect">detection output</param>
        /// <param name="proto">segmentation output</param>
        /// <returns></returns>
        public List<SegResult> process_result(float[] detect, float[] proto, int batch)
        {
            List<SegResult> re_result = new List<SegResult>();
            for (int b = 0; b < batch; ++b) 
            {
                Mat detect_data = new Mat(36 + m_categ_nums, m_output_length, MatType.CV_32FC1,
                     Marshal.UnsafeAddrOfPinnedArrayElement(detect, (4 + m_categ_nums) * m_output_length * b * 4));
                Mat proto_data = new Mat(32, 25600, MatType.CV_32F, proto);
                detect_data = detect_data.T();
                List<Rect> position_boxes = new List<Rect>();
                List<int> class_ids = new List<int>();
                List<float> confidences = new List<float>();
                List<Mat> masks = new List<Mat>();
                for (int i = 0; i < detect_data.Rows; i++)
                {

                    Mat classes_scores = new Mat(detect_data, new Rect(4, i, m_categ_nums, 1));//GetArray(i, 5, classes_scores);
                    Point max_classId_point, min_classId_point;
                    double max_score, min_score;
                    Cv2.MinMaxLoc(classes_scores, out min_score, out max_score,
                        out min_classId_point, out max_classId_point);

                    if (max_score > 0.25)
                    {
                        //Console.WriteLine(max_score);

                        Mat mask = new Mat(detect_data, new Rect(4 + m_categ_nums, i, 32, 1));//detect_data.Row(i).ColRange(4 + categ_nums, categ_nums + 36);

                        float cx = detect_data.At<float>(i, 0);
                        float cy = detect_data.At<float>(i, 1);
                        float ow = detect_data.At<float>(i, 2);
                        float oh = detect_data.At<float>(i, 3);
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
                        masks.Add(mask);
                    }
                }


                int[] indexes = new int[position_boxes.Count];
                CvDnn.NMSBoxes(position_boxes, confidences, this.m_det_thresh, this.m_det_nms_thresh, out indexes);

                SegResult re = new SegResult(); // Output Result Class
                                                       // RGB images with colors
                Mat rgb_mask = Mat.Zeros(new Size((int)m_image_sizes[b].Width, (int)m_image_sizes[b].Height), MatType.CV_8UC3);
                Random rd = new Random(); // Generate Random Numbers
                for (int i = 0; i < indexes.Length; i++)
                {
                    int index = indexes[i];
                    // Division scope
                    Rect box = position_boxes[index];
                    int box_x1 = Math.Max(0, box.X);
                    int box_y1 = Math.Max(0, box.Y);
                    int box_x2 = Math.Max(0, box.BottomRight.X);
                    int box_y2 = Math.Max(0, box.BottomRight.Y);

                    // Segmentation results
                    Mat original_mask = masks[index] * proto_data;
                    for (int col = 0; col < original_mask.Cols; col++)
                    {
                        original_mask.Set<float>(0, col, sigmoid(original_mask.At<float>(0, col)));
                    }
                    // 1x25600 -> 160x160 Convert to original size
                    Mat reshape_mask = original_mask.Reshape(1, 160);

                    //Console.WriteLine("m1.size = {0}", m1.Size());

                    // Split size after scaling
                    int mx1 = Math.Max(0, (int)((box_x1 / m_factors[b]) * 0.25));
                    int mx2 = Math.Min(160, (int)((box_x2 / m_factors[b]) * 0.25));
                    int my1 = Math.Max(0, (int)((box_y1 / m_factors[b]) * 0.25));
                    int my2 = Math.Min(160, (int)((box_y2 / m_factors[b]) * 0.25));
                    // Crop Split Region
                    Mat mask_roi = new Mat(reshape_mask, new OpenCvSharp.Range(my1, my2), new OpenCvSharp.Range(mx1, mx2));
                    // Convert the segmented area to the actual size of the image
                    Mat actual_maskm = new Mat();
                    Cv2.Resize(mask_roi, actual_maskm, new Size(box_x2 - box_x1, box_y2 - box_y1));
                    // Binary segmentation region
                    for (int r = 0; r < actual_maskm.Rows; r++)
                    {
                        for (int c = 0; c < actual_maskm.Cols; c++)
                        {
                            float pv = actual_maskm.At<float>(r, c);
                            if (pv > 0.5)
                            {
                                actual_maskm.Set<float>(r, c, 1.0f);
                            }
                            else
                            {
                                actual_maskm.Set<float>(r, c, 0.0f);
                            }
                        }
                    }

                    // 预测
                    Mat bin_mask = new Mat();
                    actual_maskm = actual_maskm * 200;
                    actual_maskm.ConvertTo(bin_mask, MatType.CV_8UC1);
                    if ((box_y1 + bin_mask.Rows) >= (int)m_image_sizes[b].Height)
                    {
                        box_y2 = (int)m_image_sizes[b].Height - 1;
                    }
                    if ((box_x1 + bin_mask.Cols) >= (int)m_image_sizes[b].Width)
                    {
                        box_x2 = (int)m_image_sizes[b].Width - 1;
                    }
                    // Obtain segmentation area
                    Mat mask = Mat.Zeros(new Size((int)m_image_sizes[b].Width, (int)m_image_sizes[b].Height), MatType.CV_8UC1);
                    bin_mask = new Mat(bin_mask, new OpenCvSharp.Range(0, box_y2 - box_y1), new OpenCvSharp.Range(0, box_x2 - box_x1));
                    Rect roi = new Rect(box_x1, box_y1, box_x2 - box_x1, box_y2 - box_y1);
                    bin_mask.CopyTo(new Mat(mask, roi));
                    // Color segmentation area
                    Cv2.Add(rgb_mask, new Scalar(rd.Next(0, 255), rd.Next(0, 255), rd.Next(0, 255)), rgb_mask, mask);

                    re.add(class_ids[index], confidences[index], position_boxes[index], rgb_mask.Clone());

                }
                re_result.Add(re);
            }

            return re_result;
        }


    }
}
