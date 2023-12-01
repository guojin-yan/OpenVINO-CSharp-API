using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
using OpenVinoSharp.Extensions.Result;
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
        [TestMethod()]
        public void Yolov8Det_test()
        {
            Yolov8Det yolo = new Yolov8Det("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");

        }

        [TestMethod()]
        public void predict_test()
        {
            Yolov8Det yolo = new Yolov8Det("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            Mat image = Cv2.ImRead("..\\..\\..\\..\\..\\dataset\\image\\demo_2.jpg");
            DetResult result = yolo.predict(image);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void predict_test1()
        {
            Yolov8Det yolo = new Yolov8Det("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s.xml");
            List<Mat> images = new List<Mat>();
            images.Add(Cv2.ImRead("..\\..\\..\\..\\..\\dataset\\image\\demo_1.jpg"));
            images.Add(Cv2.ImRead("..\\..\\..\\..\\..\\dataset\\image\\demo_2.jpg"));
            images.Add(Cv2.ImRead("..\\..\\..\\..\\..\\dataset\\image\\demo_3.jpg"));
            List<DetResult> results = yolo.predict(images);
            Assert.IsNotNull(results);
        }
        [TestMethod()]
        public void process_result_test()
        {
        }
    }
}