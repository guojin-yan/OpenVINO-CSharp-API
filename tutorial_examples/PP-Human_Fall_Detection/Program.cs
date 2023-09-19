using System;
namespace PP_Human_Fall_Detection
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            // ONNX格式
            //string yoloe_path = "/home/ygj/Program/PP-Human_Fall_Detection/model/ppyoloe/model.onnx"; // 目标检测模型
            //string tinypose_path = "/home/ygj/Program/PP-Human_Fall_Detection/model/tinypose/model.onnx"; // 关键点检测模型
            //string STGCN_path = "/home/ygj/Program/PP-Human_Fall_Detection/model/STGCN/model.onnx"; // 摔倒检测模型

            string yoloe_path = "/home/ygj/Program/PP-Human_Fall_Detection/model/ppyoloe/ir_fp16/model.xml"; // 目标检测模型
            string tinypose_path = "/home/ygj/Program/PP-Human_Fall_Detection/model/tinypose/ir_fp16/model.xml"; // 关键点检测模型
            string STGCN_path = "/home/ygj/Program/PP-Human_Fall_Detection/model/STGCN/ir_fp16/model.xml"; // 摔倒检测模型

            string device_name = "GPU.0";
            HumanFallDown.human_fall_down(yoloe_path, tinypose_path, STGCN_path, device_name);
        }
    }
}
