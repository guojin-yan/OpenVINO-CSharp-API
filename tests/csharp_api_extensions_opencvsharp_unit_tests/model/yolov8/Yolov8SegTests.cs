using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
using OpenVinoSharp.Extensions.model;
using OpenVinoSharp.Extensions.result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Mat image = Cv2.ImRead(image_path1);
            SegResult result = yolo.predict(image);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void predict_test1()
        {
            Yolov8Seg yolo = new Yolov8Seg(model_xml_path);
            List<Mat> images = new List<Mat>();
            images.Add(Cv2.ImRead(image_path));
            images.Add(Cv2.ImRead(image_path1));
            images.Add(Cv2.ImRead(image_path2));
            List<SegResult> results = yolo.predict(images);
            Assert.IsNotNull(results);
        }

        [TestMethod()]
        public void process_result_test()
        {
        }
    }
}