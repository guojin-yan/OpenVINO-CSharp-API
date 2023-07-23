using System;

namespace yolov8_simple_demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Yolov8 simple demo!");

            //Yolov8Det.yolov8_det(@"E:\Git_space\OpenVinoSharp\dataset\image\demo_2.jpg", 
            //    @"E:\Git_space\OpenVinoSharp\model\yolov8s.xml",
            //    @"E:\Git_space\OpenVinoSharp\dataset\lable\COCO_lable.txt");

            string device_name = "AUTO";
            if (args.Length > 4) {
                device_name = args[4];
                Console.WriteLine("Set inference device  {0}.", args[4]);
            }
            else {
                Console.WriteLine("No inference device specified, default device set to AUTO.");
            }

            if (args[0] == "det")
            {
                Yolov8Det.run(args[1], args[2], args[3], device_name);
            }
            else if (args[0] == "seg")
            {
                Yolov8Seg.run(args[1], args[2], args[3], device_name);
            }
            else if (args[0] == "pose")
            {
                Yolov8Pose.run(args[1], args[2], args[3], device_name);
            }
            else if (args[0] == "cls")
            {
                Yolov8Cls.run(args[1], args[2], args[3], device_name);
            }
            else {
                Console.WriteLine("Please specify the model prediction type, such as 'det'、'seg'、'pose'、'cls'");
            }

        }
    }
}
