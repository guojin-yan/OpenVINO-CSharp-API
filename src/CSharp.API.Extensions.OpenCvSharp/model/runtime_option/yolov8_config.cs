using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.Extensions.model
{
    public class Yolov8DetConfig : Config
    {
        public string device = "CPU";
        public bool use_gpu = false;
        public float det_thresh = 0.3f;
        public float det_nms_thresh = 0.5f;
        public long[] input_size = { 1, 3, 640, 640 };
        public int batch_num = 1;
        public string cache_dir = "model/";
        public int categ_nums = 80;

        public string model_path = null;

        public Yolov8DetConfig() { }

        public Yolov8DetConfig(string model_path, string? device = null, bool? use_gpu = null, 
            float? det_thresh = null, float? det_nms_thresh = null, long[]? input_size = null, 
            int? batch_num = null, string? cache_dir = null, int? categ_nums = null)
        {
            this.model_path = model_path;
            this.device = device?? this.device;
            this.use_gpu = use_gpu?? this.use_gpu;
            this.det_thresh = det_thresh ?? this.det_thresh;
            this.det_nms_thresh = det_nms_thresh ?? this.det_nms_thresh;
            this.input_size = input_size ?? this.input_size;
            this.batch_num = batch_num ?? this.batch_num;
            this.cache_dir = cache_dir ?? this.cache_dir;
            this.categ_nums = categ_nums ?? this.categ_nums;
        }

        public void set_model(string model_path)
        {
            this.model_path = model_path;
        }
    }

    public class Yolov8SegConfig : Config
    {
        public string device = "CPU";
        public bool use_gpu = false;
        public float det_thresh = 0.3f;
        public float det_nms_thresh = 0.5f;
        public long[] input_size = { 1, 3, 640, 640 };
        public int batch_num = 1;
        public string cache_dir = "model/";
        public int categ_nums = 80;

        public string model_path = null;

        public Yolov8SegConfig() { }

        public Yolov8SegConfig(string model_path, string? device = null, bool? use_gpu = null, 
            float? det_thresh = null, float? det_nms_thresh = null, long[]? input_size = null, 
            int? batch_num = null, string? cache_dir = null, int? categ_nums = null)
        {
            this.model_path = model_path;
            this.device = device ?? this.device;
            this.use_gpu = use_gpu ?? this.use_gpu;
            this.det_thresh = det_thresh ?? this.det_thresh;
            this.det_nms_thresh = det_nms_thresh ?? this.det_nms_thresh;
            this.input_size = input_size ?? this.input_size;
            this.batch_num = batch_num ?? this.batch_num;
            this.cache_dir = cache_dir ?? this.cache_dir;
            this.categ_nums = categ_nums ?? this.categ_nums;
        }

        public void set_model(string model_path)
        {
            this.model_path = model_path;
        }
    }


    public class Yolov8PoseConfig : Config
    {
        public string device = "CPU";
        public bool use_gpu = false;
        public float det_thresh = 0.3f;
        public float det_nms_thresh = 0.5f;
        public long[] input_size = { 1, 3, 640, 640 };
        public int batch_num = 1;
        public string cache_dir = "model/";

        public string model_path = null;

        public Yolov8PoseConfig() { }
        public Yolov8PoseConfig(string model_path, string? device = null, bool? use_gpu = null, 
            float? det_thresh = null, float? det_nms_thresh = null, long[]? input_size = null, 
            int? batch_num = null, string? cache_dir = null)
        {
            this.model_path = model_path;
            this.device = device ?? this.device;
            this.use_gpu = use_gpu ?? this.use_gpu;
            this.det_thresh = det_thresh ?? this.det_thresh;
            this.det_nms_thresh = det_nms_thresh ?? this.det_nms_thresh;
            this.input_size = input_size ?? this.input_size;
            this.batch_num = batch_num ?? this.batch_num;
            this.cache_dir = cache_dir ?? this.cache_dir;
        }

        public void set_model(string model_path)
        {
            this.model_path = model_path;
        }
    }

    public class Yolov8ClsConfig : Config
    {
        public string model_path = null;
        public string device = "CPU";
        public bool use_gpu = false;
        public float thresh = 0.3f;
        public long[] input_size = { 1, 3, 224, 224 };
        public int batch_num = 1;
        public string cache_dir = "model/";
        public int categ_nums = 1000;
        public int result_num = 10;

        public Yolov8ClsConfig() { }
        public Yolov8ClsConfig(string model_path, string? device = null, bool? use_gpu = null, 
            float? thresh = null, long[]? input_size = null, int? batch_num = null, 
            string? cache_dir = null, int? categ_nums = null, int? result_num = null)
        {
            this.model_path = model_path;
            this.device = device ?? this.device;
            this.use_gpu = use_gpu ?? this.use_gpu;
            this.thresh = thresh ?? this.thresh;
            this.input_size = input_size ?? this.input_size;
            this.batch_num = batch_num ?? this.batch_num;
            this.cache_dir = cache_dir ?? this.cache_dir;
            this.categ_nums = categ_nums ?? this.categ_nums;
            this.result_num = result_num ?? this.result_num;
        }

        public void set_model(string model_path)
        {
            this.model_path = model_path;
        }
    }
}
