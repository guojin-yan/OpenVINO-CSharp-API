using OpenCvSharp;
using OpenVinoSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace yolov8
{
    public partial class FormTestTime : Form
    {
        public FormTestTime()
        {
            InitializeComponent();
        }

        private void btn_choose_model_path_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            //若要改变对话框标题
            dlg.Title = "选择推理模型文件";
            //指定当前目录
            //dlg.InitialDirectory = System.Environment.CurrentDirectory;
            //dlg.InitialDirectory = System.IO.Path.GetFullPath(@"..//..//..//..");
            //设置文件过滤效果
            dlg.Filter = "模型文件(*.pt,*.onnx,*.engine,*.xml)|*.pt;*.onnx;*.engine;*.xml";
            DirectoryInfo path = new DirectoryInfo(Application.StartupPath);
            dlg.InitialDirectory = path.Parent.Parent.Parent.Parent.Parent.FullName + "\\model";//上 2层目录
            //判断文件对话框是否打开
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                tb_model_path.Text = dlg.FileName;
            }
        }

        private void btn_choose_claspath_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            //若要改变对话框标题
            dlg.Title = "选择分类文件";
            DirectoryInfo path = new DirectoryInfo(Application.StartupPath);
            dlg.InitialDirectory = path.Parent.Parent.Parent.Parent.Parent.FullName + "\\dataset\\lable";//上 2层目录
            //判断文件对话框是否打开
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                tb_clas_path.Text = dlg.FileName;
            }
        }

        private void btn_choose_testimage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            //若要改变对话框标题
            dlg.Title = "选择测试图片文件";
            //指定当前目录
            //dlg.InitialDirectory = System.Environment.CurrentDirectory;
            //dlg.InitialDirectory = System.IO.Path.GetFullPath(@"..//..//..//..");
            //设置文件过滤效果
            dlg.Filter = "图片文件(*.png,*.jpg,*.jepg)|*.png;*.jpg;*.jepg";
            DirectoryInfo path = new DirectoryInfo(Application.StartupPath);
            dlg.InitialDirectory = path.Parent.Parent.Parent.Parent.Parent.FullName + "\\dataset\\image";//上 2层目录
            //判断文件对话框是否打开
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                tb_test_image.Text = dlg.FileName;
            }
        }

        private void btn_model_deploy_Click(object sender, EventArgs e)
        {

            // 清屏专用
            textBox1.Clear();
            string model_path = tb_model_path.Text;
            //model_path = @"E:\Git_space\基于Csharp部署Yolov8\model\yolov8s.engine";
            //model_path = @"E:\Git_space\Csharp_deploy_Yolov8\model\yolov8s.onnx";
            //model_path = @"E:\Git_space\Csharp_deploy_Yolov8\model\yolov8s-seg.onnx";
            //model_path = @"E:\Git_space\Csharp_deploy_Yolov8\model\yolov8s-pose.onnx";

            string classer_path = tb_clas_path.Text;
            //classer_path = @"E:\Git_space\Csharp_deploy_Yolov8\demo\det_lable.txt";
            //classer_path = @"E:\Git_space\Csharp_deploy_Yolov8\demo\cls_lable.txt";
            string image_path = tb_test_image.Text;
            //image_path = @"E:\Git_space\Csharp_deploy_Yolov8\demo\demo_9.jpg";


            DateTime begin = DateTime.Now;
            DateTime end = DateTime.Now;
            TimeSpan model_load = new TimeSpan(0, 0, 0);
            TimeSpan data_load = new TimeSpan(0, 0, 0);
            TimeSpan model_infer = new TimeSpan(0, 0, 0);
            TimeSpan result_process = new TimeSpan(0, 0, 0);

            int n = Convert.ToInt32(tb_count.Text);
            for (int i = 0; i < n; i++)
            {
                begin = DateTime.Now;
                // 配置图片数据
                Mat image = new Mat(image_path);
                int max_image_length = image.Cols > image.Rows ? image.Cols : image.Rows;
                Mat max_image = Mat.Zeros(new OpenCvSharp.Size(max_image_length, max_image_length), MatType.CV_8UC3);
                Rect roi = new Rect(0, 0, image.Cols, image.Rows);
                image.CopyTo(new Mat(max_image, roi));
                end = DateTime.Now;
                data_load += end.Subtract(begin);


                Mat result_image = new Mat();

                #region 

                if (rb_yolov8_det.Checked) // yolov8-det模型
                {
                    float[] result_array = new float[8400 * 84];
                    float[] factors = new float[2];
                    factors = new float[2];
                    factors[0] = factors[1] = (float)(max_image_length / 640.0);
                    textBox1.AppendText("------Yolov8 detection model deploy OpnenVINO-------\r\n");
                    begin = DateTime.Now;
                    Core core = new Core(model_path, "CPU");
                    end = DateTime.Now;
                    model_load += end.Subtract(begin);
                    begin = DateTime.Now;
                    byte[] image_data = max_image.ImEncode(".bmp");
                    //存储byte的长度
                    ulong image_size = Convert.ToUInt64(image_data.Length);
                    // 加载推理图片数据
                    core.load_input_data("images", image_data, image_size, 1);
                    end = DateTime.Now;
                    data_load += end.Subtract(begin);
                    begin = DateTime.Now;
                    // 模型推理
                    core.infer();
                    end = DateTime.Now;
                    model_infer += end.Subtract(begin);
                    begin = DateTime.Now;
                    // 读取推理结果
                    result_array = core.read_infer_result<float>("output0", 8400 * 84);

                    core.delet();

                    DetectionResult result_pro = new DetectionResult(classer_path, factors);
                    result_image = result_pro.draw_result(result_pro.process_result(result_array), image.Clone());
                    end = DateTime.Now;
                    result_process += end.Subtract(begin);
                }
                #endregion

                #region
                else if (rb_yolov8_seg.Checked) // yolov8-seg 模型
                {
                    float[] det_result_array = new float[8400 * 116];
                    float[] proto_result_array = new float[32 * 160 * 160];
                    float[] factors = new float[4];
                    factors[0] = factors[1] = (float)(max_image_length / 640.0);
                    factors[2] = image.Rows;
                    factors[3] = image.Cols;

                    textBox1.AppendText("------Yolov8 segmentation model deploy OpnenVINO-------\r\n");
                    begin = DateTime.Now;
                    Core core = new Core(model_path, "CPU");
                    end = DateTime.Now;
                    model_load += end.Subtract(begin);
                    begin = DateTime.Now;

                    byte[] image_data = max_image.ImEncode(".bmp");
                    //存储byte的长度
                    ulong image_size = Convert.ToUInt64(image_data.Length);
                    // 加载推理图片数据
                    core.load_input_data("images", image_data, image_size, 1);
                    end = DateTime.Now;
                    data_load += end.Subtract(begin);
                    begin = DateTime.Now;
                    // 模型推理
                    core.infer();
                    end = DateTime.Now;
                    model_infer += end.Subtract(begin);
                    begin = DateTime.Now;
                    // 读取推理结果
                    det_result_array = core.read_infer_result<float>("output0", 8400 * 116);
                    proto_result_array = core.read_infer_result<float>("output1", 32 * 160 * 160);
                    core.delet();



                    SegmentationResult result_pro = new SegmentationResult(classer_path, factors);
                    result_image = result_pro.draw_result(result_pro.process_result(det_result_array, proto_result_array), image.Clone());
                    end = DateTime.Now;
                    result_process += end.Subtract(begin);
                }

                #endregion

                #region
                else if (rb_yolov8_cls.Checked)
                {
                    float[] result_array = new float[1000];
                    textBox1.AppendText("------Yolov8 Classification model deploy OpnenVINO-------\r\n");
                    begin = DateTime.Now;
                    Core core = new Core(model_path, "CPU");
                    end = DateTime.Now;
                    model_load += end.Subtract(begin);
                    begin = DateTime.Now;
                    byte[] image_data = max_image.ImEncode(".bmp");
                    //存储byte的长度
                    ulong image_size = Convert.ToUInt64(image_data.Length);
                    // 加载推理图片数据
                    core.load_input_data("images", image_data, image_size, 1);
                    end = DateTime.Now;
                    data_load += end.Subtract(begin);
                    begin = DateTime.Now;
                    // 模型推理
                    core.infer();
                    end = DateTime.Now;
                    model_infer += end.Subtract(begin);
                    begin = DateTime.Now;
                    // 读取推理结果
                    result_array = core.read_infer_result<float>("output0", 1000);

                    core.delet();


                    ClasResult result_pro = new ClasResult(classer_path);
                    KeyValuePair<string, float> result_cls = result_pro.process_result(result_array);
                    result_image = result_pro.draw_result(result_cls, image.Clone());
                    end = DateTime.Now;
                    result_process += end.Subtract(begin);
                    Console.WriteLine(result_cls.ToString());
                }
                #endregion
                #region
                else if (rb_yolov8_pose.Checked) // yolov8-det模型
                {
                    float[] result_array = new float[8400 * 56];
                    float[] factors = new float[2];
                    factors[0] = factors[1] = (float)(max_image_length / 640.0);


                    textBox1.AppendText("------Yolov8 Pose model deploy OpnenVINO-------\r\n");
                    begin = DateTime.Now;
                    Core core = new Core(model_path, "CPU");
                    end = DateTime.Now;
                    model_load += end.Subtract(begin);
                    begin = DateTime.Now;
                    byte[] image_data = max_image.ImEncode(".bmp");
                    //存储byte的长度
                    ulong image_size = Convert.ToUInt64(image_data.Length);
                    // 加载推理图片数据
                    core.load_input_data("images", image_data, image_size, 1);
                    end = DateTime.Now;
                    data_load += end.Subtract(begin);
                    begin = DateTime.Now;
                    // 模型推理
                    core.infer();
                    end = DateTime.Now;
                    model_infer += end.Subtract(begin);
                    begin = DateTime.Now;
                    // 读取推理结果
                    result_array = core.read_infer_result<float>("output0", 8400 * 56);

                    core.delet();
                    PoseResult result_pro = new PoseResult(factors);
                    result_image = result_pro.draw_result(result_pro.process_result(result_array), image.Clone());
                    end = DateTime.Now;
                    result_process += end.Subtract(begin);
                }
                #endregion
            }

            textBox1.AppendText(String.Format("模型加载时间：{0}; ", model_load.TotalMilliseconds / n));
            textBox1.AppendText(String.Format("数据加载时间：{0};\r\n", data_load.TotalMilliseconds / n));
            textBox1.AppendText(String.Format("模型推理时间：{0}; ", model_infer.TotalMilliseconds / n));
            textBox1.AppendText(String.Format("结果处理时间：{0};\r\n", result_process.TotalMilliseconds / n));
            GC.Collect();
        }
    }
}
