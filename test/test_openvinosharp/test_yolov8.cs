using OpenCvSharp;
using OpenCvSharp.Dnn;
using OpenVinoSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace test
{
    public class test_yolov8
    {
        public static void test_yolov8_det() 
        {
            string image_path = @"E:\GitSpace\OpenVinoSharp\dataset\image\demo_2.jpg";
            string classer_path = @"E:\GitSpace\OpenVinoSharp\dataset\lable\COCO_lable.txt";
            // 配置图片数据
            Mat image = new Mat(image_path);
            int max_image_length = image.Cols > image.Rows ? image.Cols : image.Rows;
            Mat max_image = Mat.Zeros(new OpenCvSharp.Size(max_image_length, max_image_length), MatType.CV_8UC3);
            Rect roi = new Rect(0, 0, image.Cols, image.Rows);
            image.CopyTo(new Mat(max_image, roi));
            //Mat BN_image = CvDnn.BlobFromImage(max_image, 1 / 255.0, new OpenCvSharp.Size(640, 640), new Scalar(0, 0, 0), true, false);
            //int length = BN_image.Cols * BN_image.Rows * BN_image.ElemSize();
            //float[] data = new float[length];
            //Marshal.Copy(BN_image.Data, data, 0, length);

            Mat res_mat = new Mat();
            Cv2.Resize(max_image, res_mat, new Size(640, 640));
            Mat input_mat = new Mat();
            res_mat.ConvertTo(input_mat, MatType.CV_32FC3, 1.0 / 255);
            float[] data = new float[3 * 640 * 640];

            for (int c = 0; c < 3; c++)
            {
                for (int h = 0; h < 640; h++)
                {
                    for (int w = 0; w < 640; w++)
                    {
                        data[c * 640 * 640 + h * 640 + w] = (float)(input_mat.At<Vec3f> (h, w)[c]);// .at < cv::Vec < float, 3 >> (h, w)[c];
                    }
                }
            }

            float[] factors = new float[2];
            factors[0] = factors[1] = (float)(max_image_length / 640.0);

            Core core = new Core();

            Model model = core.read_model(@"E:\GitSpace\OpenVinoSharp\model\yolov8s.xml");

            CompiledModel compiled_model = core.compiled_model(model, "CPU");

            InferRequest infer_request = compiled_model.create_infer_request();
            Tensor input_tensor = infer_request.get_tensor("images");
            Shape shape = input_tensor.get_shape();
            Console.WriteLine("shape " + shape.shape.rank);
            long[] dims = shape.get_dims();
            Console.WriteLine("dims({0}, {1}, {2}, {3})", dims[0], dims[1], dims[2], dims[3]);
            Console.WriteLine("Tensor get_shape() 成功！");

            input_tensor.set_data(data);
            Console.WriteLine("Tensor set_data() 成功！");
            DateTime star = DateTime.Now;
            infer_request.infer();
            DateTime end = DateTime.Now;
            Console.WriteLine("InferRequest infer() 成功！" + end.Subtract(star).TotalMilliseconds);

            Tensor output_tensor = infer_request.get_tensor("output0");

            float[] result_array = output_tensor.get_data(8400 * 84);

            DetectionResult result_pro = new DetectionResult(classer_path, factors);
            Mat result_image = result_pro.draw_result(result_pro.process_result(result_array), image.Clone());

            Cv2.ImShow("result", result_image);
            Cv2.WaitKey(0);
            core.free();


        }
    }

    public class DetectionResult
    {

        // 识别结果类型
        public string[] class_names;
        // 图片信息  缩放比例h, 缩放比例h,,height, width
        public float[] scales;
        // 置信度阈值
        public float score_threshold;
        // 非极大值抑制阈值
        public float nms_threshold;
        /// <summary>
        /// 读取本地识别结果类型文件到内存
        /// </summary>
        /// <param name="path">文件路径</param>
        public void read_class_names(string path)
        {

            List<string> str = new List<string>();
            StreamReader sr = new StreamReader(path);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                str.Add(line);
            }

            class_names = str.ToArray();
        }
        /// <summary>
        /// 结果处理类构造
        /// </summary>
        /// <param name="path">识别类别文件地址</param>
        /// <param name="scales">缩放比例</param>
        /// <param name="score_threshold">分数阈值</param>
        /// <param name="nms_threshold">非极大值抑制阈值</param>
        public DetectionResult(string path, float[] scales, float score_threshold = 0.25f, float nms_threshold = 0.5f)
        {
            read_class_names(path);
            this.scales = scales;
            this.score_threshold = score_threshold;
            this.nms_threshold = nms_threshold;
        }
        /// <summary>
        /// 结果处理
        /// </summary>
        /// <param name="result">模型预测输出</param>
        /// <returns>模型识别结果</returns>
        public Result process_result(float[] result)
        {
            Mat result_data = new Mat(84, 8400, MatType.CV_32F, result);
            result_data = result_data.T();

            // 存放结果list
            List<Rect> position_boxes = new List<Rect>();
            List<int> class_ids = new List<int>();
            List<float> confidences = new List<float>();
            // 预处理输出结果
            for (int i = 0; i < result_data.Rows; i++)
            {
                Mat classes_scores = result_data.Row(i).ColRange(4, 84);//GetArray(i, 5, classes_scores);
                Point max_classId_point, min_classId_point;
                double max_score, min_score;
                // 获取一组数据中最大值及其位置
                Cv2.MinMaxLoc(classes_scores, out min_score, out max_score,
                    out min_classId_point, out max_classId_point);
                // 置信度 0～1之间
                // 获取识别框信息
                if (max_score > 0.25)
                {
                    float cx = result_data.At<float>(i, 0);
                    float cy = result_data.At<float>(i, 1);
                    float ow = result_data.At<float>(i, 2);
                    float oh = result_data.At<float>(i, 3);
                    int x = (int)((cx - 0.5 * ow) * this.scales[0]);
                    int y = (int)((cy - 0.5 * oh) * this.scales[1]);
                    int width = (int)(ow * this.scales[0]);
                    int height = (int)(oh * this.scales[1]);
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

            // NMS非极大值抑制
            int[] indexes = new int[position_boxes.Count];
            CvDnn.NMSBoxes(position_boxes, confidences, this.score_threshold, this.nms_threshold, out indexes);

            Result re_result = new Result();
            // 将识别结果绘制到图片上
            for (int i = 0; i < indexes.Length; i++)
            {
                int index = indexes[i];
                int idx = class_ids[index];
                re_result.add(confidences[index], position_boxes[index], this.class_names[class_ids[index]]);
            }
            return re_result;
        }
        /// <summary>
        /// 结果绘制
        /// </summary>
        /// <param name="result">识别结果</param>
        /// <param name="image">绘制图片</param>
        /// <returns></returns>
        public Mat draw_result(Result result, Mat image)
        {

            // 将识别结果绘制到图片上
            for (int i = 0; i < result.length; i++)
            {
                //Console.WriteLine(result.rects[i]);
                Cv2.Rectangle(image, result.rects[i], new Scalar(0, 0, 255), 2, LineTypes.Link8);
                Cv2.Rectangle(image, new Point(result.rects[i].TopLeft.X, result.rects[i].TopLeft.Y - 20),
                    new Point(result.rects[i].BottomRight.X, result.rects[i].TopLeft.Y), new Scalar(0, 255, 255), -1);
                Cv2.PutText(image, result.classes[i] + "-" + result.scores[i].ToString("0.00"),
                    new Point(result.rects[i].X, result.rects[i].Y - 10),
                    HersheyFonts.HersheySimplex, 0.6, new Scalar(0, 0, 0), 1);
            }
            return image;
        }

    }
    public class Result
    {
        // 获取结果长度
        public int length
        {
            get
            {
                return scores.Count;
            }
        }

        // 识别结果类
        public List<string> classes = new List<string>();
        // 置信值
        public List<float> scores = new List<float>();
        // 预测框
        public List<Rect> rects = new List<Rect>();

        /// <summary>
        /// 物体检测
        /// </summary>
        /// <param name="score">预测分数</param>
        /// <param name="rect">识别框</param>
        /// <param name="cla">识别类</param>
        public void add(float score, Rect rect, string cla)
        {
            scores.Add(score);
            rects.Add(rect);
            classes.Add(cla);
        }

    }
    
}
