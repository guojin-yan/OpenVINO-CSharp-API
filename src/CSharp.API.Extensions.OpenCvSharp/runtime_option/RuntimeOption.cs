using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.Extensions.RuntimeOption
{
    public struct YoloOption
    {
        public static List<string> lable = new List<string>{ "person", "bicycle", "car", "motorbike", "aeroplane", "bus", "train", "truck",
            "boat", "traffic light","fire hydrant","stop sign", "parking meter", "bench", "bird", "cat", "dog",
            "horse", "sheep", "cow", "elephant", "bear", "zebra", "giraffe","backpack", "umbrella","handbag",
            "tie", "suitcase", "frisbee", "skis", "snowboard", "sports ball", "kite", "baseball bat",
            "baseball glove","skateboard", "surfboard","tennis racket", "bottle", "wine glass", "cup", "fork",
            "knife", "spoon", "bowl", "banana", "apple", "sandwich", "orange","broccoli", "carrot","hot dog",
            "pizza", "donut", "cake", "chair", "sofa", "pottedplant", "bed", "diningtable", "toilet",
            "tvmonitor", "laptop", "mouse","remote","keyboard", "cell phone", "microwave", "oven", "toaster",
            "sink", "refrigerator", "book", "clock", "vase", "scissors", "teddy bear", "hair drier", "toothbrush"};
    }
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
}
