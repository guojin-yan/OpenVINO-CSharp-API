using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCvSharp.Dnn;
using OpenVinoSharp;

namespace PP_Human
{
    public class TinyPose
    {
        // Member variables
        private Core m_core;
        private Model m_model;
        private CompiledModel m_compiled_model;
        private InferRequest m_infer_request;
        private string m_input_node_name = "image";
        private string m_output_node_name_1 = "conv2d_585.tmp_1";
        private string m_output_node_name_2 = "argmax_0.tmp_0";
        private Size m_input_size = new Size(256, 192); 
        private Size m_output_size = new Size(64, 48);
        private Size m_image_size = new Size(0, 0); 

        /// <summary>
        /// 初始化预测器
        /// </summary>
        /// <param name="mode_path">模型地址</param>
        /// <param name="device_name">设备名称</param>
        public TinyPose(string model_path, string device_name)
        {
            m_core = new Core();
            m_model = m_core.read_model(model_path);
            m_compiled_model = m_core.compile_model(m_model, device_name);
            m_infer_request = m_compiled_model.create_infer_request();

            long[] input_sizes = new long[] { 1, 3, (long)(this.m_input_size.Width), (long)(this.m_input_size.Height) };
            Shape input_shape = new Shape(input_sizes);
            Tensor tensor = m_infer_request.get_input_tensor();
            tensor.set_shape(input_shape);
        }


        /// <summary>
        /// 预测
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public float[,] predict(Mat image)
        {
            this.m_image_size.Width = image.Cols;
            this.m_image_size.Height = image.Rows;

            Mat input_mat = new Mat();
            //Cv2.CvtColor(image, input_mat, ColorConversionCodes.BGR2RGB);

            Point center = new Point(image.Cols / 2, image.Rows / 2); // 变换中心点
            Size input_size = new Size(m_image_size.Width,m_image_size.Height); // 输入尺寸
            int rot = 0; // 旋转角度
            Size output_size = new Size(m_input_size.Height, m_input_size.Width) ; // 输出尺寸
            Mat trans = get_affine_transform(center, input_size, rot, output_size, false); // 变换矩阵

            Tensor input_tensor = m_infer_request.get_input_tensor();

            Cv2.WarpAffine(image, input_mat, trans, output_size, InterpolationFlags.Linear);

            input_mat = CvDnn.BlobFromImage(input_mat, 1.0, output_size, 0, false, false);

            float[] input_data = new float[input_tensor.get_size()];
            Marshal.Copy(input_mat.Ptr(0), input_data, 0, input_data.Length);
            input_tensor.set_data<float>(input_data);

            m_infer_request.infer();
            // 读取模型输出
            // 读取预测结果
            Tensor output_tensor = m_infer_request.get_tensor(m_output_node_name_1);
            int point_size = m_output_size.Width * m_output_size.Height;
            float[] result = output_tensor.get_data<float>(17 * point_size);
            // 处理模型输出结果
            float[,] points = process_result(result);
            return points;

        }

        /// <summary>
        /// 裁剪关键点识别区域
        /// </summary>
        /// <param name="source_image">原图片</param>
        /// <param name="rects">矩形区域数组</param>
        /// <returns>文字区域mat</returns>
        public List<Mat> get_point_roi(Mat source_image, List<Rect> person_rects, out List<Rect> point_rects)
        {
            Mat image = source_image.Clone();
            List<Mat> rois = new List<Mat>();
            point_rects = new List<Rect>();

            for (int r = 0; r < person_rects.Count; r++)
            {
                Point locate = person_rects[r].Location;
                int width = person_rects[r].Width;
                int height = person_rects[r].Height;
                double centre_X = locate.X + width * 0.5;
                double centre_Y = locate.Y + height * 0.5;
                double nwidth = width * 1.3;
                double nheight = height * 1.3;
                if (nheight > (nwidth * 4.0 / 3.0))
                {
                    nwidth = nheight * 0.75;
                }
                Rect rect_new = new Rect((int)(centre_X - 0.5 * nwidth),
                    (int)(centre_Y - 0.5 * nheight), (int)nwidth, (int)nheight);

                int x_min = Math.Max(0, rect_new.X);
                int x_max = Math.Min(source_image.Cols - 1, rect_new.X + rect_new.Width);
                int y_min = Math.Max(0, rect_new.Y);
                int y_max = Math.Min(source_image.Rows - 1, rect_new.Y + rect_new.Height);

                Rect rect = new Rect(x_min, y_min, x_max - x_min, y_max - y_min);

                //Rect rect = get_IOU(sourse_rect, rect_new);
                Mat roi = new Mat(image, rect);
                rois.Add(roi);
                point_rects.Add(rect);
            }
            return rois;

        }
        /// <summary>
        /// 处理关键点预测结果
        /// </summary>
        /// <param name="het_map"></param>
        /// <param name="image"></param>
        /// <returns>预测点(x, y, confindence)</returns>
        private float[,] process_result(float[] het_map)
        {
            float[,] point_meses = new float[17, 3];

            for (int p = 0; p < 17; p++)
            {
                // 提取一个点结果图像
                float[,] map = new float[this.m_output_size.Width, this.m_output_size.Height];
                for (int h = 0; h < this.m_output_size.Width; h++)
                {
                    for (int w = 0; w < this.m_output_size.Height; w++)
                    {
                        map[h, w] = het_map[p * this.m_output_size.Width * this.m_output_size.Height 
                            + h * this.m_output_size.Height + w];
                    }
                }
                // 通过获取最大值获得点的粗略位置
                float maxval = 0;
                int[] index_int = get_max_point(map, ref maxval);
                // 保存关键点的信息
                point_meses[p, 0] = index_int[0];
                point_meses[p, 1] = index_int[1];
                point_meses[p, 2] = maxval;
                

                // 高斯滤波细化点位置
                Mat gaussianblur = Mat.Zeros(this.m_output_size.Width + 2, this.m_output_size.Height + 2, MatType.CV_32FC1); // 高斯图像背景
                Mat roi = new Mat(new List<int>() { this.m_output_size.Width, this.m_output_size.Height }, MatType.CV_32FC1, map); // 将点结果转为Mat数据
                Rect rect = new Rect(1, 1, this.m_output_size.Height, this.m_output_size.Width);
                roi.CopyTo(new Mat(gaussianblur, rect)); // 将点结果放在背景上
                Cv2.GaussianBlur(gaussianblur, gaussianblur, new Size(3, 3), 0); // 高斯滤波
                gaussianblur = new Mat(gaussianblur, rect); // 提取高斯滤波结果
                double max_temp = 0;
                double min_temp = 0;
                Cv2.MinMaxIdx(gaussianblur, out min_temp, out max_temp); // 获取高斯滤波后的最大值
                Mat mat = new Mat(this.m_output_size.Width, this.m_output_size.Height, MatType.CV_32FC1, maxval / max_temp);
                gaussianblur = gaussianblur.Mul(mat); // 滤波结果乘滤波前后最大值的比值
                // 将数据小于1e-10去掉，并取对数结果
                float[,] process_map = new float[this.m_output_size.Width, this.m_output_size.Height];
                for (int h = 0; h < this.m_output_size.Width; h++)
                {
                    for (int w = 0; w < this.m_output_size.Height; w++)
                    {
                        float temp = gaussianblur.At<float>(h, w);
                        if (temp < 1e-10)
                        {
                            temp = (float)1e-10;
                        }
                        temp = (float)Math.Log(temp);
                        process_map[h, w] = temp;

                    }
                }

                // 基于泰勒展开的坐标解码
                int py = index_int[1];
                int px = index_int[0];
                if ((2 < py) && (py < this.m_output_size.Width - 2) && (2 < px) && (px < this.m_output_size.Height - 2))
                {
                    // 求导数和偏导数
                    float dx = 0.5f * (process_map[py, px + 1] - process_map[py, px - 1]);
                    float dy = 0.5f * (process_map[py + 1, px] - process_map[py - 1, px]);
                    float dxx = 0.25f * (process_map[py, px + 2] - 2 * process_map[py, px] + process_map[py, px - 2]);
                    float dxy = 0.25f * (process_map[py + 1, px + 1] - process_map[py - 1, px + 1]
                        - process_map[py + 1, px - 1] + process_map[py - 1, px - 1]);
                    float dyy = 0.25f * (process_map[py + 2 * 1, px] - 2 * process_map[py, px] + process_map[py - 2 * 1, px]);
                    // 构建相应的倒数矩阵
                    Mat derivative = new Mat(2, 2, MatType.CV_32FC1, new float[] { dx, 0, dy, 0 });
                    Mat hessian = new Mat(2, 2, MatType.CV_32FC1, new float[] { dxx, dxy, dxy, dyy });
                    if (dxx * dyy - dxy * dxy != 0)
                    {
                        Mat hessianinv = new Mat();
                        Cv2.Invert(hessian, hessianinv); // 矩阵求逆
                        mat = new Mat(2, 2, MatType.CV_32FC1, -1);
                        hessianinv = hessianinv.Mul(mat); // 矩阵取－
                        Mat offset = new Mat();
                        Cv2.Multiply(hessianinv, derivative, offset); // 矩阵相乘
                        offset = offset.T(); // 矩阵转置
                        // 获取定位偏差
                        double error_x = offset.At<Vec2d>(0)[0];
                        double error_y = offset.At<Vec2d>(0)[1];
                        if (Math.Abs(error_x) > 10) 
                        {
                            error_x = 0;
                        }
                        if (Math.Abs(error_y) > 10) 
                        {
                            error_y = 0;
                        }
                        // 修正横纵坐标
                        point_meses[p, 0] = px + (float)error_x;
                        point_meses[p, 1] = py + (float)error_y;

                    }
                }
            }

            // 获取反向变换矩阵
            Point center = new Point(this.m_image_size.Width / 2, this.m_image_size.Height / 2); // 变换中心点
            Size input_size = new Size(this.m_image_size.Width, this.m_image_size.Height); // 输入尺寸
            int rot = 0; // 旋转角度
            Size output_size = new Size(this.m_output_size.Height, this.m_output_size.Width); // 输出尺寸
            Mat trans = get_affine_transform(center, input_size, rot, output_size, true); // 变换矩阵
            // 获取变换结果
            double scale_x_1 = trans.At<Vec3d>(0)[0];
            double scale_x_2 = trans.At<Vec3d>(0)[1];
            double scale_x_3 = trans.At<Vec3d>(0)[2];
            double scale_y_1 = trans.At<Vec3d>(1)[0];
            double scale_y_2 = trans.At<Vec3d>(1)[1];
            double scale_y_3 = trans.At<Vec3d>(1)[2];
            // 变换预测点的位置
            for (int p = 0; p < 17; p++)
            {
                point_meses[p, 0] = point_meses[p, 0] * (float)scale_x_1 + point_meses[p, 1] * (float)scale_x_2 + 1.0f * (float)scale_x_3;
                point_meses[p, 1] = point_meses[p, 0] * (float)scale_y_1 + point_meses[p, 1] * (float)scale_y_2 + 1.0f * (float)scale_y_3;

            }

            //float scale_x = (float)m_image_size.Width / (float)input_size.Width;
            //float scale_y = (float)m_image_size.Height / (float)input_size.Height;
            //for (int p = 0; p < 17; p++)
            //{
            //    point_meses[p, 0] = point_meses[p, 0] * scale_x * 4;
            //    point_meses[p, 1] = point_meses[p, 1] * scale_y * 4;

            //}

            return point_meses;
        }



        /// <summary>
        /// 获取模型输出最大点位置
        /// </summary>
        /// <param name="map"></param>
        /// <param name="maxval"></param>
        /// <returns></returns>
        private int[] get_max_point(float[,] map, ref float maxval)
        {
            int height = map.GetLength(0);
            int width = map.GetLength(1);
            int[] index = new int[2];
            int[] index_h = new int[height];
            float[] maxval_h = new float[height];
            for (int h = 0; h < height; h++)
            {
                float val = map[h, 0];
                for (int w = 0; w < width; w++)
                {
                    if (val < map[h, w])
                    {
                        val = map[h, w];
                        maxval_h[h] = val;
                        index_h[h] = w;
                    }
                }
            }
            float maxval_temp = maxval_h[0];
            for (int h = 0; h < height; h++)
            {
                if (maxval_temp < maxval_h[h])
                {
                    maxval_temp = maxval_h[h];
                    index[1] = h;
                    index[0] = index_h[h];
                    maxval = maxval_temp;
                }
            }
            return index;
        }


        /// <summary>
        /// 获取仿射变换矩阵
        /// </summary>
        /// <param name="center">变换中心</param>
        /// <param name="input_size">输入尺寸</param>
        /// <param name="rot">旋转角度</param>
        /// <param name="output_size">输出尺寸</param>
        /// <param name="inv">是否反向</param>
        /// <returns>变换矩阵</returns>
        Mat get_affine_transform(Point center, Size input_size, int rot, Size output_size, bool inv = false)
        {
            Point2f shift = new Point2f(0.0f, 0.0f);
            // 输入尺寸宽度
            int src_w = input_size.Width;

            // 输出尺寸
            int dst_w = output_size.Width;
            int dst_h = output_size.Height;

            // 旋转角度
            double rot_rad = 3.1715926 * rot / 180.0;
            int pt = (int)(src_w * -0.5);
            double sn = Math.Sin(rot_rad);
            double cs = Math.Cos(rot_rad);

            Point2f src_dir = new Point2f((float)(-1.0 * pt * sn), (float)(pt * cs));
            Point2f dst_dir = new Point2f(0.0f, (float)(dst_w * -0.5));
            Point2f[] src = new Point2f[3];
            src[0] = new Point2f((float)(center.X + input_size.Width * shift.X), (float)(center.Y + input_size.Height * shift.Y));
            src[1] = new Point2f(center.X + src_dir.X + input_size.Width * shift.X, center.Y + src_dir.Y + input_size.Height * shift.Y);
            Point2f direction = src[0] - src[1];
            src[2] = new Point2f(src[1].X - direction.Y, src[1].Y - direction.X);

            Point2f[] dst = new Point2f[3];
            dst[0] = new Point2f((float)(dst_w * 0.5), (float)(dst_h * 0.5));
            dst[1] = new Point2f((float)(dst_w * 0.5 + dst_dir.X), (float)(dst_h * 0.5 + dst_dir.Y));
            direction = dst[0] - dst[1];
            dst[2] = new Point2f(dst[1].X - direction.Y, dst[1].Y - direction.X);

            // 是否为反向
            if (inv) { return Cv2.GetAffineTransform(dst, src); }
            else { return Cv2.GetAffineTransform(src, dst); }
        }
        /// <summary>
        /// 绘制预测结果
        /// </summary>
        /// <param name="points"></param>
        /// <param name="image"></param>
        public void draw_poses(KeyPoints key_points,ref Mat image)
        {
            float[,] points = (float[,])key_points.points.Clone();
            Rect rect = key_points.bbox;
            // 连接点关系
            int[,] edgs = new int[17, 2] { { 0, 1 }, { 0, 2}, {1, 3}, {2, 4}, {3, 5}, {4, 6}, {5, 7}, {6, 8},
                 {7, 9}, {8, 10}, {5, 11}, {6, 12}, {11, 13}, {12, 14},{13, 15 }, {14, 16 }, {11, 12 } };
            // 颜色库
            Scalar[] colors = new Scalar[18] { new Scalar(255, 0, 0), new Scalar(255, 85, 0), new Scalar(255, 170, 0),
                new Scalar(255, 255, 0), new Scalar(170, 255, 0), new Scalar(85, 255, 0), new Scalar(0, 255, 0),
                new Scalar(0, 255, 85), new Scalar(0, 255, 170), new Scalar(0, 255, 255), new Scalar(0, 170, 255),
                new Scalar(0, 85, 255), new Scalar(0, 0, 255), new Scalar(85, 0, 255), new Scalar(170, 0, 255),
                new Scalar(255, 0, 255), new Scalar(255, 0, 170), new Scalar(255, 0, 85) };
            // 绘制阈值
            double visual_thresh = 0.0000;
            // 绘制关键点
            for (int p = 0; p < 17; p++)
            {

                points[p, 0] = points[p, 0] + rect.X;
                points[p, 1] = points[p, 1] + rect.Y;
                if (points[p, 2] < visual_thresh)
                {
                    continue;
                }
                Point point = new Point((int)points[p, 0], (int)points[p, 1]);
                Cv2.Circle(image, point, 2, colors[p], -1);
            }
            // 绘制
            for (int p = 0; p < 17; p++)
            {
                if (points[edgs[p, 0], 2] < visual_thresh || points[edgs[p, 1], 2] < visual_thresh)
                {
                    continue;
                }

                float[] point_x = new float[] { points[edgs[p, 0], 0], points[edgs[p, 1], 0] };
                float[] point_y = new float[] { points[edgs[p, 0], 1], points[edgs[p, 1], 1] };

                Point center_point = new Point((int)((point_x[0] + point_x[1]) / 2), (int)((point_y[0] + point_y[1]) / 2));
                double length = Math.Sqrt(Math.Pow((double)(point_x[0] - point_x[1]), 2.0) + Math.Pow((double)(point_y[0] - point_y[1]), 2.0));
                int stick_width = 2;
                Size axis = new Size(length / 2, stick_width);
                double angle = (Math.Atan2((double)(point_y[0] - point_y[1]), (double)(point_x[0] - point_x[1]))) * 180 / Math.PI;
                Point[] polygon = Cv2.Ellipse2Poly(center_point, axis, (int)angle, 0, 360, 1);
                Cv2.FillConvexPoly(image, polygon, colors[p]);

            }
        }
        /// <summary>
        /// 释放推理器内存
        /// </summary>
        public void release()
        {
            m_infer_request.dispose();
            m_compiled_model.dispose();
            m_model.dispose();
            m_core.dispose();
        }
    }
}
