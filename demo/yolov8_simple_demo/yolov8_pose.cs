using OpenCvSharp;
using OpenVinoSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yolov8;

namespace yolov8_simple_demo
{
    public class Yolov8Pose
    {
        public static void run(string model_path, string image_path, string classer_path, string device = "AUTO")
        {

            DateTime begin = DateTime.Now;
            DateTime end = DateTime.Now;
            TimeSpan ts = new TimeSpan(0, 0, 0);

            Console.WriteLine("----Yolov8 Pose model deploy OpnenVinoSharp-----\r\n");
            begin = DateTime.Now;
            // Initialize Core, loading and building model.
            Core core = new Core(model_path, device);
            end = DateTime.Now;
            ts = end.Subtract(begin);
            Console.WriteLine("[ INFO ] Loading model file: {0}.", model_path);
            Console.WriteLine("[ INFO ] Loading and building model time: {0} ms", ts.TotalMilliseconds);
            begin = DateTime.Now;
            // Process input image data
            Mat image = new Mat(image_path); // Read image by opencvsharp
            int max_image_length = image.Cols > image.Rows ? image.Cols : image.Rows;
            Mat max_image = Mat.Zeros(new OpenCvSharp.Size(max_image_length, max_image_length), MatType.CV_8UC3);
            Rect roi = new Rect(0, 0, image.Cols, image.Rows);
            image.CopyTo(new Mat(max_image, roi));
            // Convert image data into array data
            byte[] image_data = max_image.ImEncode(".bmp");
            // Get image data length
            ulong image_size = Convert.ToUInt64(image_data.Length);
            // Load inference image data
            core.load_input_data("images", image_data, image_size, 1);
            end = DateTime.Now;
            ts = end.Subtract(begin);
            Console.WriteLine("[ INFO ] Reading image file: {0}.", image_path);
            Console.WriteLine("[ INFO ] Reading and loading image time: {0} ms", ts.TotalMilliseconds);


            float[] factors = new float[2];
            factors[0] = factors[1] = (float)(max_image_length / 640.0);
            begin = DateTime.Now;
            // Model data infer
            core.infer();
            end = DateTime.Now;
            ts = end.Subtract(begin);
            Console.WriteLine("[ INFO ] Infering model time: {0} ms", ts.TotalMilliseconds);
            begin = DateTime.Now;
            // Read inference results
            float[] result_array = core.read_infer_result<float>("output0", 8400 * 56);
            // Initialize data processing class
            PoseResult result_pro = new PoseResult(factors);
            // Get result images
            Mat result_image = result_pro.draw_result(result_pro.process_result(result_array), image.Clone());
            end = DateTime.Now;
            ts = end.Subtract(begin);
            Console.WriteLine("[ INFO ] Result processing time: {0} ms", ts.TotalMilliseconds);

            begin = DateTime.Now;
            // Clear model memory
            core.delet();
            end = DateTime.Now;
            ts = end.Subtract(begin);
            Console.WriteLine("[ INFO ] Clear model memory time: {0} ms", ts.TotalMilliseconds);
            Cv2.ImShow("result", result_image);
            Cv2.WaitKey(0);

        }
    }
}
