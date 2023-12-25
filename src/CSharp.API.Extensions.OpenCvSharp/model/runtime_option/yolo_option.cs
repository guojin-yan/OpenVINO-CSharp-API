using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.Extensions.model
{
    public struct Yolov8DetOption
    {
        public static string device = "CPU"; 
        public static bool use_gpu = false;
        public static float det_thresh = 0.3f;
        public static float det_nms_thresh = 0.5f;
        public static long[] input_size = { 1, 3, 640, 640 };
        public static int batch_num = 1;
        public static string cache_dir = "model/";
        public static int categ_nums = 80;

    }

    public struct Yolov8PoseOption
    {
        public static string device = "CPU";
        public static bool use_gpu = false;
        public static float det_thresh = 0.3f;
        public static float det_nms_thresh = 0.5f;
        public static long[] input_size = { 1, 3, 640, 640 };
        public static int batch_num = 1;
        public static string cache_dir = "model/";
    }

    public struct Yolov8ClsOption
    {
        public static string device = "CPU";
        public static bool use_gpu = false;
        public static float thresh = 0.3f;
        public static long[] input_size = { 1, 3, 224, 224 };
        public static int batch_num = 1;
        public static string cache_dir = "model/";
        public static int categ_nums = 1000;
        public static int result_num = 10;
    }
}
