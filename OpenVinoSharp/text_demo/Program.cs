using System;
using OpenCvSharp;
using OpenVinoSharp;

namespace text_demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DateTime dt1 = System.DateTime.Now;
            // 测试花卉分类模型
             text_flower_clas();
            // 测试车辆识别模型
            //text_vehicle_yolov3();
            DateTime dt2 = System.DateTime.Now;
            TimeSpan ts1 = dt2.Subtract(dt1);
            Console.WriteLine(ts1);
        }

        /// <summary>
        /// 花卉分类模型测试
        /// </summary>
        static void text_flower_clas()
        {
            string device_name = "CPU";

            // 本地模型地址
            // 路径中不能包含中文，调试时请将模型放置在没有中文的路径下
            string model_file_paddle = @"E:\Text_Model\flowerclas\inference.pdmodel"; // paddlepaddle格式
            string model_file_onnx = @"E:\Text_Model\flowerclas\flower_clas_auto.onnx"; // onnx格式
            string model_file_ir = @"E:\Text_Model\flowerclas\flower_clas.xml"; // ir格式

            // 测试图片地址
            string image_file = @"E:\Git_space\C#调用OpenVINO部署Al模型项目开发\database\flower_clas\text_image_08.jpg";
            // 模型输入节点名
            string input_node_name = "x";
            // 模型输出节点名
            string output_node_name = "softmax_1.tmp_0";

            // 初始化推理
            Core ie = new Core(model_file_paddle, device_name);

            // 设置图片输入大小
            ulong[] image_sharp = new ulong[] { 1, 3, 224, 224 };
            ie.set_input_sharp(input_node_name, image_sharp);

            Mat image = new Mat(image_file);
            byte[] image_data = new byte[2048 * 2048 * 3];
            //存储byte的长度
            ulong image_size = new ulong();
            image_data = image.ImEncode(".bmp");
            image_size = Convert.ToUInt64(image_data.Length);
            ie.load_input_data(input_node_name, image_data, image_size);

            ie.inference_engine_infer();

            float[] result = new float[102];
            result = ie.read_inference_result<float>(output_node_name, 102);

            int[] index = find_array_max(result, 5);
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("the index is {0} , the score is {1} ", index[i], result[index[i]]);
            }
            ie.inference_engine_delet();


        }


        /// <summary>
        /// 获取数组中前N个最大的数据的位置
        /// </summary>
        /// <param name="result">输入数组</param>
        /// <param name="max_num">n</param>
        /// <returns>中前N个最大的数据的索引值</returns>
        static int[] find_array_max(float[] result, int max_num)
        {
            int size = result.Length;
            float[] temp_result = new float[size];
            for (int i = 0; i < size; i++)
            {
                temp_result[i] = result[i];
            }
            for (int i = 0; i < size; i++)
            {
                float max = temp_result[i];
                for (int j = i + 1; j < size; j++)
                {
                    if (max < temp_result[j])
                    {
                        float temp = temp_result[j];
                        temp_result[j] = max;
                        max = temp;
                    }
                }
                temp_result[i] = max;
            }
            int[] index = new int[max_num];
            for (int i = 0; i < max_num; i++)
            {
                int s;
                for (s = 0; s < size; s++)
                {
                    if (result[s] == temp_result[i])
                        break;
                }
                index[i] = s;
            }
            return index;
        }


        /// <summary>
        /// 测试车辆类型识别
        /// </summary>
        static void text_vehicle_yolov3()
        {
            string device_name = "CPU";
            string model_file = @"E:/Text_Model/vehicle_yolov3_darknet/model.pdmodel";

            string image_file = "E:/Text_dataset/vehicle_yolov3_darknet/006.jpg";
            string[] input_node_name = new string[] { "image", "scale_factor", "im_shape" };
            string[] output_node_name = new string[] { "multiclass_nms3_0.tmp_2", "multiclass_nms3_0.tmp_0" };

            string[] lable = new string[] { "car ", "truck", "bus", "motorbike", "tricycle", "carplate" };
            Core ie = new Core(model_file, device_name);
            ulong[] image_sharp = new ulong[] { 1, 3, 608, 608 };
            ie.set_input_sharp(input_node_name[0], image_sharp);

            Mat image = new Mat(image_file);
            byte[] image_data = new byte[2048 * 2048 * 3];
            //存储byte的长度
            ulong image_size = new ulong();
            image_data = image.ImEncode(".bmp");
            image_size = Convert.ToUInt64(image_data.Length);
            ie.load_input_data(input_node_name[0], image_data, image_size);

            ulong[] data_sharp = new ulong[] { 1, 2 };

            ie.set_input_sharp(input_node_name[1], data_sharp);
            float scale_h = 608.00f / image.Height;
            float scale_w = 608.00f / image.Width;
            float[] scale_factor = new float[] { scale_h, scale_w };
            ie.load_input_data(input_node_name[1], scale_factor);

            ie.set_input_sharp(input_node_name[2], data_sharp);
            float[] im_shape = new float[] { 608, 608 };
            ie.load_input_data(input_node_name[2], im_shape);

            ie.inference_engine_infer();

            int[] resule_num = new int[1];
            resule_num = ie.read_inference_result<int>(output_node_name[0], 1);

            int result_size = 6 * resule_num[0];
            float[] result = new float[1000];
            result = ie.read_inference_result<float>(output_node_name[1], result_size);

            image = draw_image_resule(image, resule_num[0], result, lable, 0.2f);

            Cv2.ImShow("Detection results", image);

            Cv2.WaitKey(0);

        }
        /// <summary>
        /// 根据输入结果，在指定图片上绘制识别框
        /// </summary>
        /// <param name="sourse_image">源图片</param>
        /// <param name="num_result">结果数量</param>
        /// <param name="result">结果数组</param>
        /// <param name="lable">标志数组</param>
        /// <param name="score_min">分数下限</param>
        /// <returns>修改过的图片</returns>
        static Mat draw_image_resule(Mat sourse_image, int num_result, float[] result, string[] lable, float score_min)
        {
            Mat image = sourse_image.Clone();
            for (int i = 0; i < num_result; i++)
            {
                int class_id = Convert.ToInt16(result[i * 6 + 0]);
                float score = result[i * 6 + 1];
                if (score > score_min)
                {
                    float x1 = result[i * 6 + 2];
                    float y1 = result[i * 6 + 3];
                    float x2 = result[i * 6 + 4];
                    float y2 = result[i * 6 + 5];
                    Cv2.PutText(image, (lable[class_id] + score.ToString("0.000")),
                        new Point(x1, y1 - 5), HersheyFonts.HersheySimplex, 0.3,
                        new Scalar(0, 0, 255), 1);
                    Cv2.Rectangle(image, new Point(x1, y1), new Point(x2, y2), new Scalar(0, 0, 255));
                }
            }
            return image;
        }



    }
}
