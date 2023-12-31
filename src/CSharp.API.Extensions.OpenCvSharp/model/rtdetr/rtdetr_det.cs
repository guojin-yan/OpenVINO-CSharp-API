using OpenCvSharp;
using OpenCvSharp.Dnn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenVinoSharp.Extensions.result;
using OpenCvSharp.Flann;
using System.Runtime.InteropServices;
using OpenVinoSharp.Extensions.process;

namespace OpenVinoSharp.Extensions.model
{
    public class RtdetrDet : Predictor
    {
        private int m_categ_nums;
        private float m_det_thresh;
        private long[] m_input_size;
        private int m_batch_num;
        public bool m_post_flag;
        private List<float[]> m_factors = new List<float[]>();
        public List<float[]> m_im_shape = new List<float[]>();
        public RtdetrDet(RtdetrConfig config)
            : base(config.model_path, config.device, config.cache_dir, config.use_gpu, config.input_size)
        {
            m_categ_nums = config.categ_nums;
            m_det_thresh = config.det_thresh;
            m_input_size = config.input_size;
            m_batch_num = config.batch_num;
            this.m_post_flag = config.postprcoess;
        }


        /// <summary>
        /// The function takes an input image, preprocesses it, performs inference using a pre-trained
        /// model, and returns the image with bounding boxes drawn around detected objects.
        /// </summary>
        /// <param name="Mat">The `Mat` class is a data structure in OpenCV that represents an image
        /// matrix. It is used to store and manipulate image data.</param>
        /// <returns>
        /// The method is returning a `Mat` object, which is an image with bounding boxes drawn on it.
        /// </returns>
        public DetResult predict(Mat image)
        {
            Mat mat = new Mat();
            Cv2.CvtColor(image, mat, ColorConversionCodes.BGR2RGB);
            Cv2.Resize(mat, mat, new Size(m_input_size[2], m_input_size[3]));
            m_factors.Clear(); m_factors = new List<float[]>();
            m_im_shape.Clear(); m_im_shape = new List<float[]>();
            m_im_shape.Add( new float[] { (float)mat.Rows, (float)mat.Cols });
            m_factors.Add( new float[] { 640.0f / (float)image.Rows, 640.0f / (float)image.Cols });
            mat = Normalize.run(mat, true);
            float[] input_data = Permute.run(mat);
            if (m_post_flag)
            {
                Tensor image_tensor = m_infer_request.get_tensor("image");
                Tensor shape_tensor = m_infer_request.get_tensor("im_shape");
                Tensor scale_tensor = m_infer_request.get_tensor("scale_factor");
                image_tensor.set_shape(new Shape(new List<long> { 1, 3, 640, 640 }));
                shape_tensor.set_shape(new Shape(new List<long> { 1, 2 }));
                scale_tensor.set_shape(new Shape(new List<long> { 1, 2 }));
                image_tensor.set_data(input_data);
                shape_tensor.set_data(m_im_shape[0]);
                scale_tensor.set_data(m_factors[0]);
            }
            else
            {
                Tensor image_tensor = m_infer_request.get_input_tensor();
                image_tensor.set_shape(new Shape(new List<long> { 1, 3, 640, 640 }));
                image_tensor.set_data(input_data);
            }
            m_infer_request.infer();
            DetResult results;
            if (m_post_flag)
            {
                Tensor output_tensor = m_infer_request.get_output_tensor(0);
                float[] result = output_tensor.get_data<float>(300 * 6);

                results = postprocess(result, null, 1)[0];
            }
            else
            {
                Tensor score_tensor = m_infer_request.get_tensor(m_model.outputs()[1].get_any_name());
                Tensor bbox_tensor = m_infer_request.get_tensor(m_model.outputs()[0].get_any_name());
                float[] score = score_tensor.get_data<float>(300 * 80);
                float[] bbox = bbox_tensor.get_data<float>(300 * 4);
                results = postprocess(score, bbox, 1)[0];
            }
            return results;
        }

        public List<DetResult> predict(List<Mat> images)
        {
            List<DetResult> re_results = new List<DetResult>();
            for (int beg_img_no = 0; beg_img_no < images.Count; beg_img_no += m_batch_num)
            {
                int end_img_no = Math.Min(images.Count, beg_img_no + m_batch_num);
                int batch_num = end_img_no - beg_img_no;
                List<Mat> norm_img_batch = new List<Mat>();
                m_factors.Clear(); m_factors = new List<float[]>();
                m_im_shape.Clear(); m_im_shape = new List<float[]>();
                for (int ino = beg_img_no; ino < end_img_no; ino++)
                {
                    Mat mat = new Mat();
                    Cv2.CvtColor(images[ino], mat, ColorConversionCodes.BGR2RGB);
                   
                    m_factors.Add(new float[] { 640.0f / (float)mat.Rows, 640.0f / (float)mat.Cols });
                    Cv2.Resize(mat, mat, new Size(m_input_size[2], m_input_size[3]));
                    m_im_shape.Add(new float[] { (float)mat.Rows, (float)mat.Cols });
                    mat = Normalize.run(mat, true);
                    norm_img_batch.Add(mat);
                }
                float[] input_data = PermuteBatch.run(norm_img_batch);

                if (m_post_flag)
                {
                    Tensor image_tensor = m_infer_request.get_tensor("image");
                    Tensor shape_tensor = m_infer_request.get_tensor("im_shape");
                    Tensor scale_tensor = m_infer_request.get_tensor("scale_factor");
                    image_tensor.set_shape(new Shape(new List<long> { batch_num, 3, 640, 640 }));
                    shape_tensor.set_shape(new Shape(new List<long> { batch_num, 2 }));
                    scale_tensor.set_shape(new Shape(new List<long> { batch_num, 2 }));
                    image_tensor.set_data(input_data);
                    shape_tensor.set_data(list_to_array(m_im_shape));
                    scale_tensor.set_data(list_to_array(m_factors));
                }
                else
                {
                    Tensor image_tensor = m_infer_request.get_input_tensor();
                    image_tensor.set_shape(new Shape(new List<long> {batch_num, 3, 640, 640 }));
                    image_tensor.set_data(input_data);
                }
                m_infer_request.infer();
                List<DetResult> results = new List<DetResult> ();
                if (m_post_flag)
                {
                    Tensor output_tensor = m_infer_request.get_output_tensor(0);
                    float[] result = output_tensor.get_data<float>((int)output_tensor.get_size());

                    results = postprocess(result, null, batch_num);
                }
                else
                {
                    Tensor score_tensor = m_infer_request.get_tensor(m_model.outputs()[1].get_any_name());
                    Tensor bbox_tensor = m_infer_request.get_tensor(m_model.outputs()[0].get_any_name());
                    float[] score = score_tensor.get_data<float>((int)score_tensor.get_size());
                    float[] bbox = bbox_tensor.get_data<float>((int)bbox_tensor.get_size());
                    results = postprocess(score, bbox, batch_num);
                }
                re_results.AddRange(results);
            }
            return re_results;
        }

        /// <summary>
        /// The function takes in an array of scores and bounding box coordinates, and based on a flag,
        /// it either applies a threshold to filter the results or performs additional calculations to
        /// determine the maximum score and bounding box.
        /// </summary>
        /// <param name="score">An array of floating-point values representing the scores for each
        /// bounding box. The length of the array is 300.</param>
        /// <param name="bbox">The `bbox` parameter is an array of floats that represents the bounding
        /// box coordinates for each detected object. Each object is represented by 4 values in the
        /// array, which correspond to the x-coordinate, y-coordinate, width, and height of the bounding
        /// <returns>
        /// The method is returning an object of type ResultData.
        /// </returns>
        public List<DetResult> postprocess(float[] score, float[] bbox, int batch)
        {
            List<DetResult> re_result = new List<DetResult>();
            for (int b = 0; b < batch; ++b)
            {
                DetResult result = new DetResult();
                if (m_post_flag)
                {
                    for (int i = 0; i < 300; ++i)
                    {
                        if (score[b * 1800 + 6 * i + 1] > m_det_thresh)
                        {

                            result.add((int)score[b * 1800 + 6 * i], score[b * 1800 + 6 * i + 1],
                                new Rect((int)score[b * 1800 + 6 * i + 2], (int)score[b * 1800 + 6 * i + 3],
                                (int)(score[b * 1800 + 6 * i + 4] - score[b * 1800 + 6 * i + 2]),
                                (int)(score[b * 1800 + 6 * i + 5] - score[b * 1800 + 6 * i + 3])));
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 300; ++i)
                    {
                        float[] s = new float[m_categ_nums];
                        for (int j = 0; j < m_categ_nums; ++j)
                        {
                            s[j] = score[b * 24000 + m_categ_nums * i + j];
                        }
                        int clsid = argmax(s, m_categ_nums);
                        float max_score = sigmoid(s[clsid]);
                        if (max_score > m_det_thresh)
                        {
                            float cx = (float)(bbox[b * 1200 + 4 * i] * 640.0 / m_factors[b][1]);
                            float cy = (float)(bbox[b * 1200 + 4 * i + 1] * 640.0 / m_factors[b][0]);
                            float w = (float)(bbox[b * 1200 + 4 * i + 2] * 640.0 / m_factors[b][1]);
                            float h = (float)(bbox[b * 1200 + 4 * i + 3] * 640.0 / m_factors[b][0]);

                            result.add(clsid, max_score, new Rect((int)(cx - w / 2), (int)(cy - h / 2), (int)w, (int)h));
                        }
                    }
                }
                re_result.Add(result);
            }
        
            return re_result;
        }


        /// <summary>
        /// The sigmoid function takes in a float value and returns the result of applying the sigmoid function
        /// to that value.
        /// </summary>
        /// <param name="data">The parameter "data" is a float value that represents the input to the sigmoid
        /// function.</param>
        /// <returns>
        /// the result of the sigmoid function applied to the input data.
        /// </returns>
        private float sigmoid(float data)
        {
            return (float)(1.0f / (1 + Math.Exp(-data)));
        }

        /// <summary>
        /// The function `argmax` takes an array of floats and returns the index of the maximum value in the
        /// array.
        /// </summary>
        /// <param name="data">An array of float values.</param>
        /// <param name="length">The `length` parameter is the number of elements in the `data` array that
        /// should be considered for finding the maximum value.</param>
        /// <returns>
        /// The method is returning the index of the maximum value in the given array.
        /// </returns>
        private int argmax(float[] data, int length)
        {
            List<float> arr = new List<float>(data);
            float max = arr.Max();
            return arr.FindIndex(val => val == max);
        }

        private float[] list_to_array(List<float[]> data) 
        {
            return data.SelectMany(arr => arr).ToArray();
        }

    }
}
