using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenVinoSharp.Extensions.model;
using OpenVinoSharp.Extensions.result;
using OpenVinoSharp.Extensions.process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;

namespace OpenVinoSharp.Extensions.model.Tests
{
    [TestClass()]
    public class PPYoloeDetTests
    {
        public string model_path = "E:\\Model\\ppyoloe\\model.pdmodel";

        public string image_path = "..\\..\\..\\..\\..\\tests\\test_data\\image\\demo_1.jpg";
        public string image_path1 = "..\\..\\..\\..\\..\\tests\\test_data\\image\\demo_2.jpg";
        public string image_path2 = "..\\..\\..\\..\\..\\tests\\test_data\\image\\demo_3.jpg";
        public string image_path3 = "..\\..\\..\\..\\..\\tests\\test_data\\image\\demo_9.jpg";
        [TestMethod()]
        public void PPYoloeDet_test()
        {
            Config config = new PPYoloeConfig(model_path);
            PPYoloeDet yoloe = new PPYoloeDet((PPYoloeConfig)config);
        }

        [TestMethod()]
        public void predict_test()
        {
            Config config = new PPYoloeConfig(model_path);
            PPYoloeDet yoloe = new PPYoloeDet((PPYoloeConfig)config);
            Mat image = CvInvoke.Imread(image_path1);

            DetResult result = yoloe.predict(image);
            Mat im = Visualize.draw_det_result(result, image);
            CvInvoke.Imshow("ww", im);
            CvInvoke.WaitKey(0);
        }

        [TestMethod()]
        public void predict_test1()
        {
            List<Mat> images = new List<Mat>()
            {
                CvInvoke.Imread(image_path),
                CvInvoke.Imread(image_path1),
                 CvInvoke.Imread(image_path2),
                  CvInvoke.Imread(image_path3)
            };
            PPYoloeConfig config = new PPYoloeConfig(model_path);
            config.batch_num = 4;
            PPYoloeDet yoloe = new PPYoloeDet((PPYoloeConfig)config);

            List<DetResult> result = yoloe.predict(images);
            Mat im = Visualize.draw_det_result(result[0], images[0]);
            CvInvoke.Imshow("ww", im);
            Mat im1 = Visualize.draw_det_result(result[1], images[1]);
            CvInvoke.Imshow("ww1", im1);
            Mat im2 = Visualize.draw_det_result(result[2], images[2]);
            CvInvoke.Imshow("ww2", im2);
            Mat im3 = Visualize.draw_det_result(result[3], images[3]);
            CvInvoke.Imshow("ww3", im3);
            CvInvoke.WaitKey(0);
        }
    }
}