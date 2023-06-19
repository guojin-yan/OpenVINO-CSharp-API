using System;
using OpenVinoSharp;

namespace yolov5 // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int b = NativeMethods.test(12, 35);
            Console.WriteLine(b);
            Yolov5.yolov5_demo();
        }
    }
}

