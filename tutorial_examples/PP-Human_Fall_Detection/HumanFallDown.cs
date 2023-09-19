using OpenCvSharp;
using PP_Human;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP_Human_Fall_Detection
{
    /// <summary>
    /// 测试人体摔倒检测
    /// </summary>
    public class HumanFallDown
    {

        /// <summary>
        /// 人体摔倒检测
        /// </summary>
        static public void human_fall_down(string yoloe_path, string tinypose_path, string STGCN_path, string device_name)
        {

            // 定义模型预测器
            // yoloe 模型预测
            YOLOE yoloe_predictor = new YOLOE(yoloe_path, device_name);
            Console.WriteLine("目标检测模型加载成功！！");
            // tinypose 预测器
            TinyPose tinyPose_predictor = new TinyPose(tinypose_path, device_name);
            Console.WriteLine("关键点识别模型加载成功！！");
            // STGCN 模型
            STGCN stgcn_predictor = new STGCN(STGCN_path, device_name);
            Console.WriteLine("摔倒识别模型加载成功！！");

            // 测试视频信息
            // 视频路径
            string test_video = "/home/ygj/Program/PP-Human_Fall_Detection/demo/摔倒.mp4";
            //string test_video = @"E:\Git_space\基于Csharp和OpenVINO部署PP-Human\demo\摔倒2.mp4";
            // 视频读取器
            VideoCapture video_capture = new VideoCapture(test_video);
            // 视频帧率
            double fps = video_capture.Fps;
            // 视频帧数
            int frame_count = video_capture.FrameCount;
            Console.WriteLine("video fps: {0}, frame_count: {1}", Math.Round(fps), frame_count);



            // 定义相关变量
            // 视频帧
            int frame_id = 0;
            // 视频帧图像
            Mat frame = new Mat();
            // 可视化
            Mat visualize_frame = new Mat();

            // 关键点处理类
            MotPoint mot_point = new MotPoint();
            // 关键点快开始预测标志位
            bool flag_stgcn = false;

            // 可视化窗口
            Window window = new Window("image", WindowFlags.AutoSize);
            //window.Resize(100, 100);
            // 摔倒预测结果
            KeyValuePair<string, float> fall_down_result = new KeyValuePair<string, float>("unfalling", 1.0f);
            // 设置起始帧
            //video_capture.Set(VideoCaptureProperties.PosFrames, 100);

            // 创建视频保存器
            VideoWriter video_writer = new VideoWriter(@"output.avi",
                FourCC.MP42, 30, new Size(video_capture.FrameWidth, video_capture.FrameHeight));

            while (true)
            {
                // 判断视频是否打开
                if (!video_capture.IsOpened())
                {
                    Console.WriteLine("视频打开失败！！");
                    break;
                }
                // 每10帧打印一次过程
                if (frame_id % 10 == 0)
                {
                    Console.WriteLine("检测进程 frame id: {0} - {1}", frame_id, frame_id + 10);
                }

                // 读取视频帧
                if (!video_capture.Read(frame))
                {
                    Console.WriteLine("视频读取完毕！！{0}", frame_id);
                    break;
                }

                // 复制可视化图片
                visualize_frame = frame.Clone();

                //****************************1. 行人区域识别  ******************************//
                ResBboxs person_result = yoloe_predictor.predict(frame);
                // 判断是否识别到人
                if (person_result.bboxs.Count < 1)
                {
                    continue;
                }
                // 绘制行人区域
                yoloe_predictor.draw_boxes(person_result, ref visualize_frame);

                //****************************2. 行人关键点识别  ******************************//
                //裁剪行人区域
                List<Rect> point_rects;
                List<Mat> person_rois = tinyPose_predictor.get_point_roi(frame, person_result.bboxs, out point_rects);


                for (int p = 0; p < person_rois.Count; p++)
                {
                    Cv2.ImShow("www", person_rois[p]);
                    // 关键点识别
                    float[,] person_point = tinyPose_predictor.predict(person_rois[p]);
                    KeyPoints key_point = new KeyPoints(frame_id, person_point, point_rects[p]);
                    //for (int i = 0; i < 17; ++i) {
                    //    Console.WriteLine(key_point.points[i,0] + " " + key_point.points[i, 1]);
                    //}
                    
                    flag_stgcn = mot_point.add_point(key_point);
                    tinyPose_predictor.draw_poses(key_point, ref visualize_frame);
                }


                //****************************3. 行人摔倒识别  ******************************//

                if (flag_stgcn)
                {
                    List<List<KeyPoints>> predict_points = mot_point.get_points();
                    for (int p = 0; p < predict_points.Count; p++)
                    {
                        Console.WriteLine(predict_points[p].Count);
                        fall_down_result = stgcn_predictor.predict(predict_points[p]);
                    }
                }
                stgcn_predictor.draw_result(ref visualize_frame, fall_down_result, person_result.bboxs[0]);

                window.ShowImage(visualize_frame);
                video_writer.Write(visualize_frame);
                Cv2.WaitKey(1);

                frame_id++; // 帧号累加

            }

            yoloe_predictor.release();
            tinyPose_predictor.release();
            stgcn_predictor.release();

            visualize_frame.Release();
            video_capture.Release();
        }


    }
}
