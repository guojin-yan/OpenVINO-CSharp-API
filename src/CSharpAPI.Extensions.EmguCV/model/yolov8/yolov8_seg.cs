using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenVinoSharp.Extensions.RuntimeOption;
using OpenVinoSharp.Extensions.result;
using OpenVinoSharp.Extensions.process;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
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
            m_image_sizes = new List<Size>() { image.Size };
            CvInvoke.CvtColor(image, mat, Emgu.CV.CvEnum.ColorConversion.Bgr2Rgb);
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
                    m_image_sizes.Add(images[ino].Size);
                    CvInvoke.CvtColor(images[ino], mat, Emgu.CV.CvEnum.ColorConversion.Bgr2Rgb);
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
                //Mat m = Visualize.draw_seg_result(results[0], images[beg_img_no]);
                //Cv2.ImShow("www", m);
                //Cv2.WaitKey(0);
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
                Mat detect_data = new Mat(36 + m_categ_nums, m_output_length, DepthType.Cv32F, 1, 
                    Marshal.UnsafeAddrOfPinnedArrayElement(detect, (4 + m_categ_nums) * m_output_length * b * 4), 4 * m_output_length);
                Mat proto_data = new Mat(32, 25600, DepthType.Cv32F, 1,
                    Marshal.UnsafeAddrOfPinnedArrayElement(proto, 32 * 25600 * b * 4), 4 * 25600);
                detect_data = detect_data.T();
                List<Rectangle> position_boxes = new List<Rectangle>();
                List<int> class_ids = new List<int>();
                List<float> confidences = new List<float>();
                List<Mat> masks = new List<Mat>();
                for (int i = 0; i < detect_data.Rows; i++)
                {

                    Mat classes_scores = new Mat(detect_data, new Rectangle(4, i, m_categ_nums, 1));// GetArray(i, 5, classes_scores);
                    Point max_classId_point = new Point(), min_classId_point = new Point();
                    double max_score = 0, min_score = 0;
                    CvInvoke.MinMaxLoc(classes_scores, ref min_score, ref max_score,
                        ref min_classId_point, ref max_classId_point);

                    if (max_score > 0.25)
                    {
                        //Console.WriteLine(max_score);

                        Mat mask = new Mat(detect_data, new Rectangle(4 + m_categ_nums, i, 32, 1));//detect_data.Row(i).ColRange(4 + categ_nums, categ_nums + 36);

                        Mat mat = new Mat(detect_data, new Rectangle(0, i, 4, 1));
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

                        position_boxes.Add(box);
                        class_ids.Add(max_classId_point.X);
                        confidences.Add((float)max_score);
                        masks.Add(mask);
                    }
                }


                int[] indexes = DnnInvoke.NMSBoxes(position_boxes.ToArray(), confidences.ToArray(), this.m_det_thresh, this.m_det_nms_thresh);

                SegResult re = new SegResult(); // Output Result Class
                                                       // RGB images with colors
                Mat rgb_mask = Mat.Zeros((int)m_image_sizes[b].Width, (int)m_image_sizes[b].Height, DepthType.Cv8U, 3);
                Random rd = new Random(); // Generate Random Numbers
                for (int i = 0; i < indexes.Length; i++)
                {
                    int index = indexes[i];
                    // Division scope
                    Rectangle box = position_boxes[index];
                    int box_x1 = Math.Max(0, box.X);
                    int box_y1 = Math.Max(0, box.Y);
                    int box_x2 = Math.Max(0, box.Location.X + box.Width);
                    int box_y2 = Math.Max(0, box.Location.Y + box.Height);

                    // Segmentation results
                    Mat original_mask = new Mat();
                    CvInvoke.Multiply(masks[index], proto_data, original_mask);

                    float[] data = (float[])original_mask.GetData();
                    for (int col = 0; col < original_mask.Cols; col++)
                    {
                        data[col] = sigmoid(data[col]);
                    }
                    original_mask = new Mat(original_mask.Size, DepthType.Cv32F, 1,
                        Marshal.UnsafeAddrOfPinnedArrayElement(data, 0), original_mask.Cols);
                    // 1x25600 -> 160x160 Convert to original size
                    Mat reshape_mask = original_mask.Reshape(1, 160);

                    //Console.WriteLine("m1.size = {0}", m1.Size());

                    // Split size after scaling
                    int mx1 = Math.Max(0, (int)((box_x1 / m_factors[b]) * 0.25));
                    int mx2 = Math.Min(160, (int)((box_x2 / m_factors[b]) * 0.25));
                    int my1 = Math.Max(0, (int)((box_y1 / m_factors[b]) * 0.25));
                    int my2 = Math.Min(160, (int)((box_y2 / m_factors[b]) * 0.25));
                    // Crop Split Region
                    Mat mask_roi = new Mat(reshape_mask, new Rectangle(my1, my2, mx1 - my1, mx2 - my2));
                    // Convert the segmented area to the actual size of the image
                    Mat actual_maskm = new Mat();
                    CvInvoke.Resize(mask_roi, actual_maskm, new Size(box_x2 - box_x1, box_y2 - box_y1));
                    // Binary segmentation region
                    float[] data1 = (float[])actual_maskm.GetData();
                    for (int r = 0; r < actual_maskm.Rows; r++)
                    {
                        for (int c = 0; c < actual_maskm.Cols; c++)
                        {
                            float pv = data1[r * actual_maskm.Cols + c];
                            if (pv > 0.5)
                            {
                                data1[r * actual_maskm.Cols + c] = 1.0f;
                            }
                            else
                            {
                                data1[r * actual_maskm.Cols + c] = 0.0f;
                            }
                        }
                    }
                    actual_maskm = new Mat(actual_maskm.Size, DepthType.Cv32F, 1,
                        Marshal.UnsafeAddrOfPinnedArrayElement(data1, 0), actual_maskm.Cols);
                    // 预测
                    Mat bin_mask = new Mat();
                    actual_maskm = actual_maskm * 200;
                    actual_maskm.ConvertTo(bin_mask, DepthType.Cv8U);
                    if ((box_y1 + bin_mask.Rows) >= (int)m_image_sizes[b].Height)
                    {
                        box_y2 = (int)m_image_sizes[b].Height - 1;
                    }
                    if ((box_x1 + bin_mask.Cols) >= (int)m_image_sizes[b].Width)
                    {
                        box_x2 = (int)m_image_sizes[b].Width - 1;
                    }
                    // Obtain segmentation area
                    Mat mask = Mat.Zeros((int)m_image_sizes[b].Width, (int)m_image_sizes[b].Height, DepthType.Cv8U, 1);
                    bin_mask = new Mat(bin_mask, new Rectangle(0, 0, box_y2 - box_y1, box_x2 - box_x1));
                    Rectangle roi = new Rectangle(box_x1, box_y1, box_x2 - box_x1, box_y2 - box_y1);
                    bin_mask.CopyTo(new Mat(mask, roi));
                    // Color segmentation area
                    CvInvoke.Add(rgb_mask, new ScalarArray(new MCvScalar( rd.Next(0, 255), rd.Next(0, 255), rd.Next(0, 255))), rgb_mask, mask);

                    re.add(class_ids[index], confidences[index], position_boxes[index], rgb_mask.Clone());

                }
                re_result.Add(re);
            }

            return re_result;
        }


    }
}
