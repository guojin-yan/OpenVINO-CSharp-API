using Emgu.CV;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenVinoSharp.Extensions.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenVinoSharp.Extensions.result;
using OpenVinoSharp.Extensions.process;

namespace OpenVinoSharp.Extensions.model.Tests
{
    [TestClass()]
    public class Yolov8SegTests
    {
        public string model_xml_path = "..\\..\\..\\..\\..\\tests\\test_data\\model\\yolov8\\yolov8s-seg.xml";
        public string image_path = "..\\..\\..\\..\\..\\tests\\test_data\\image\\demo_1.jpg";
        public string image_path1 = "..\\..\\..\\..\\..\\tests\\test_data\\image\\demo_2.jpg";
        public string image_path2 = "..\\..\\..\\..\\..\\tests\\test_data\\image\\demo_3.jpg";
        [TestMethod()]
        public void Yolov8Seg_test()
        {
            Yolov8Seg yolo = new Yolov8Seg(model_xml_path);
        }

        [TestMethod()]
        public void predict_test()
        {
            Yolov8Seg yolo = new Yolov8Seg(model_xml_path);
            Mat image = CvInvoke.Imread(image_path1);
            SegResult result = yolo.predict(image);
            //CvInvoke.Imshow("aa", result.datas[0].mask);
            //CvInvoke.Imshow("bb", result.datas[1].mask);
            //CvInvoke.Imshow("cc", result.datas[2].mask);
            //Mat new_mat = Visualize.draw_seg_result(result, image);
            //CvInvoke.Imshow("dd", new_mat);

            //CvInvoke.WaitKey(0);
            //CvInvoke.WaitKey(0);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void predict_test1()
        {
            Yolov8Seg yolo = new Yolov8Seg(model_xml_path);
            Mat image = CvInvoke.Imread(image_path1);
            List<Mat> images = new List<Mat>();
            images.Add(image);
            List<SegResult> result = yolo.predict(images);
            //CvInvoke.Imshow("aa", result.datas[0].mask);
            //CvInvoke.Imshow("bb", result.datas[1].mask);
            //CvInvoke.Imshow("cc", result.datas[2].mask);
            //Mat new_mat = Visualize.draw_seg_result(result, image);
            //CvInvoke.Imshow("dd", new_mat);

            //CvInvoke.WaitKey(0);
            //CvInvoke.WaitKey(0);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void process_result_test()
        {

        }

        [TestMethod()]
        public void Yolov8Seg_test1()
        {
            Config config = new Yolov8SegConfig(model_xml_path);
            Yolov8Seg yolo = new Yolov8Seg((Yolov8SegConfig)config);
        }
    }
}