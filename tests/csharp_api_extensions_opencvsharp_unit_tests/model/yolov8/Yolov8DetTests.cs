using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
using OpenVinoSharp.Extensions.result;
using OpenVinoSharp.Extensions.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.Extensions.model.Tests
{
    [TestClass()]
    public class Yolov8DetTests
    {
        public string model_xml_path = "..\\..\\..\\..\\..\\tests\\test_data\\model\\yolov8\\yolov8s.xml";
        public string image_path = "..\\..\\..\\..\\..\\tests\\test_data\\image\\demo_1.jpg";
        public string image_path1 = "..\\..\\..\\..\\..\\tests\\test_data\\image\\demo_2.jpg";
        public string image_path2 = "..\\..\\..\\..\\..\\tests\\test_data\\image\\demo_3.jpg";
        [TestMethod()]
        public void Yolov8Det_test()
        {
            Yolov8Det yolo = new Yolov8Det(model_xml_path);

        }

        [TestMethod()]
        public void predict_test()
        {
            Yolov8Det yolo = new Yolov8Det(model_xml_path);
            Mat image = Cv2.ImRead(image_path);
            DetResult result = yolo.predict(image);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void predict_test1()
        {
            Config config = new Yolov8DetConfig(model_xml_path);
            Yolov8Det yolo = new Yolov8Det((Yolov8DetConfig)config);
            List<Mat> images = new List<Mat>();
            images.Add(Cv2.ImRead(image_path));
            images.Add(Cv2.ImRead(image_path1));
            images.Add(Cv2.ImRead(image_path2));
            List<DetResult> results = yolo.predict(images);
            Assert.IsNotNull(results);
        }
        [TestMethod()]
        public void process_result_test()
        {
        }

        [TestMethod()]
        public void Yolov8Det_test1()
        {
            Config config = new Yolov8DetConfig(model_xml_path);
            Yolov8Det yolo = new Yolov8Det((Yolov8DetConfig)config);
        }
    }
}