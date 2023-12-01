using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenVinoSharp.Extensions.RuntimeOption;
using OpenVinoSharp.Extensions.result;
using OpenVinoSharp.Extensions.process;
namespace OpenVinoSharp.Extensions.model
{
    public class Yolov8eg : Predictor
    {
        private int m_categ_nums;
        private float m_det_thresh;
        private float m_det_nms_thresh;
        private float[] m_factors;
        private long[] m_input_size;
        private int m_output_length = 8400;
        private int m_batch_num;

        public Yolov8eg(string model_path, string? device = null, int? categ_nums = null, bool? use_gpu = null,
            long[]? input_size = null, int? batch_num = null, string? cache_dir = null, float? det_thresh = null,
            float? det_nms_thresh = null)
            : base(model_path, device ?? Yolov8DetOption.device, cache_dir ?? Yolov8DetOption.cache_dir,
                  use_gpu ?? Yolov8DetOption.use_gpu, input_size ?? Yolov8DetOption.input_size)
        {
            m_categ_nums = categ_nums ?? Yolov8DetOption.categ_nums;
            m_det_thresh = det_thresh ?? Yolov8DetOption.det_thresh;
            m_det_nms_thresh = det_nms_thresh ?? Yolov8DetOption.det_nms_thresh;
            m_input_size = input_size ?? Yolov8DetOption.input_size;

            m_output_length = (int)m_input_size[2] / 8 * (int)m_input_size[2] / 8 +
                 (int)m_input_size[2] / 16 * (int)m_input_size[2] / 16 +
                 (int)m_input_size[2] / 32 * (int)m_input_size[2] / 32;

            m_batch_num = batch_num ?? Yolov8DetOption.batch_num;
        }
    }
}
