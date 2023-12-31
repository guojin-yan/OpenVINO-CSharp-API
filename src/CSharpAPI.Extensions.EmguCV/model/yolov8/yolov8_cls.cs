using Emgu.CV;
using OpenVinoSharp.Extensions.process;
using OpenVinoSharp.Extensions.result;
using OpenVinoSharp.Extensions.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.Extensions.model
{
    public class Yolov8Cls : Predictor
    {
        private int m_categ_nums;
        private long[] m_input_size;
        private int m_batch_num;
        private int m_result_num;
        public Yolov8Cls(string model_path, string? device = null, int? categ_nums = null, bool? use_gpu = null,
            long[]? input_size = null, int? batch_num = null, string? cache_dir = null, int? result_num = null)
            : base(model_path, device ?? Yolov8ClsOption.device, cache_dir ?? Yolov8ClsOption.cache_dir,
                  use_gpu ?? Yolov8ClsOption.use_gpu, input_size ?? Yolov8ClsOption.input_size)
        {
            m_categ_nums = categ_nums ?? Yolov8ClsOption.categ_nums;
            m_input_size = input_size ?? Yolov8ClsOption.input_size;
            m_batch_num = batch_num ?? Yolov8ClsOption.batch_num;
            m_result_num = result_num ?? Yolov8ClsOption.result_num;
        }
        public Yolov8Cls(Yolov8ClsConfig config)
            : base(config.model_path, config.device, config.cache_dir, config.use_gpu, config.input_size)
        {
            m_categ_nums = config.categ_nums;
            m_input_size = config.input_size;
            m_batch_num = config.batch_num;
            m_result_num = config.result_num;
        }
        public ClsResult predict(Mat image)
        {
            Mat mat = new Mat();
            CvInvoke.CvtColor(image, mat, Emgu.CV.CvEnum.ColorConversion.Bgr2Rgb);
            float factors = 0f;
            mat = Resize.letterbox_img(mat, (int)m_input_size[2], out factors);
            mat = Normalize.run(mat, true);
            float[] input_data = Permute.run(mat);
            float[] output_data = infer(input_data);

            List<int> sort_result = Utility.argsort(output_data);
            ClsResult result = new ClsResult();
            for (int i = 0; i < m_result_num; ++i) 
            {
                result.add(sort_result[i], output_data[sort_result[i]]);
            }
            return result;
        }
        public List<ClsResult> predict(List<Mat> images)
        {
            List<ClsResult> results = new List<ClsResult>();
            for (int beg_img_no = 0; beg_img_no < images.Count; beg_img_no += m_batch_num)
            {
                int end_img_no = Math.Min(images.Count, beg_img_no + m_batch_num);
                int batch_num = end_img_no - beg_img_no;
                List<Mat> norm_img_batch = new List<Mat>();
                float factors = 0f;
                for (int ino = beg_img_no; ino < end_img_no; ino++)
                {
                    Mat mat = new Mat();
                    CvInvoke.CvtColor(images[ino], mat, Emgu.CV.CvEnum.ColorConversion.Bgr2Rgb);
                    mat = Resize.letterbox_img(mat, (int)m_input_size[2], out factors);
                    mat = Normalize.run(mat, true);
                    norm_img_batch.Add(mat);
                }
                float[] input_data = PermuteBatch.run(norm_img_batch);
                float[] output_datas = infer(input_data, new long[] { batch_num, 3, m_input_size[2], m_input_size[3] });

                for (int i = 0; i < batch_num; ++i) 
                {
                    float[] output_data = new float[m_categ_nums];
                    Buffer.BlockCopy(output_datas, m_categ_nums * i, output_data, 0, m_categ_nums*4);
                    List<int> sort_result = Utility.argsort(output_data);
                    ClsResult result = new ClsResult();
                    for (int j = 0; j < m_result_num; ++j)
                    {
                        result.add(sort_result[j], output_data[sort_result[j]]);
                    }
                    results.Add(result);
                }
            }
            return results;
        }
    }
}
