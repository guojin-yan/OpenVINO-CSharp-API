using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.Extensions.model
{
    public class RtdetrConfig : Config
    {
        public string model_path = null;
        public string device = "CPU";
        public bool use_gpu = false;
        public float det_thresh = 0.5f;
        public long[] input_size = { 1, 3, 640, 640 };
        public int batch_num = 1;
        public string cache_dir = "model/";
        public int categ_nums = 80;
        public bool postprcoess = true;

        public RtdetrConfig() { }

        public RtdetrConfig(string model_path, string? device = null, bool? use_gpu = null,
            float? det_thresh = null, bool? postprcoess = null, long[]? input_size = null,
            int? batch_num = null, string? cache_dir = null, int? categ_nums = null)
        {
            this.model_path = model_path;
            this.device = device ?? this.device;
            this.use_gpu = use_gpu ?? this.use_gpu;
            this.det_thresh = det_thresh ?? this.det_thresh;
            this.input_size = input_size ?? this.input_size;
            this.batch_num = batch_num ?? this.batch_num;
            this.cache_dir = cache_dir ?? this.cache_dir;
            this.categ_nums = categ_nums ?? this.categ_nums;
            this.postprcoess = postprcoess ?? this.postprcoess;
        }

        public void set_model(string model_path)
        {
            this.model_path = model_path;
        }
    }
}
