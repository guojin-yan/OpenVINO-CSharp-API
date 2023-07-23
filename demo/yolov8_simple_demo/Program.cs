using System;

namespace yolov8_simple_demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Yolov8 simple demo!");

            Yolov8Det.yolov8_det(@"E:\Git_space\OpenVinoSharp\dataset\image\demo_2.jpg", 
                @"E:\Git_space\OpenVinoSharp\model\yolov8s.xml",
                @"E:\Git_space\OpenVinoSharp\dataset\lable\COCO_lable.txt");
        }
    }
}
