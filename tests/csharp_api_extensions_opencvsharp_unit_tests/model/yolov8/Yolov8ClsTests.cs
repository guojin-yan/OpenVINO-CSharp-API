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
    public class Yolov8ClsTests
    {
        public string model_xml_path = "..\\..\\..\\..\\..\\tests\\test_data\\model\\yolov8\\yolov8s-cls.xml";
        public string image_path = "..\\..\\..\\..\\..\\tests\\test_data\\image\\demo_4.jpg";
        public string image_path1 = "..\\..\\..\\..\\..\\tests\\test_data\\image\\demo_6.jpg";
        public string image_path2 = "..\\..\\..\\..\\..\\tests\\test_data\\image\\demo_7.jpg";
        [TestMethod()]
        public void Yolov8Cls_test()
        {
            Yolov8Cls yolo = new Yolov8Cls(model_xml_path);
        }

        [TestMethod()]
        public void predict_test()
        {
            Yolov8Cls yolo = new Yolov8Cls(model_xml_path);
            Mat image = Cv2.ImRead(image_path);
            ClsResult result = yolo.predict(image);
            result.update_lable(ImageNetOption.lables);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void predict_test1()
        {
            Yolov8Cls yolo = new Yolov8Cls(model_xml_path);
            List<Mat> images = new List<Mat>();
            images.Add(Cv2.ImRead(image_path));
            images.Add(Cv2.ImRead(image_path1));
            images.Add(Cv2.ImRead(image_path2));
            List<ClsResult> results = yolo.predict(images);
            results[0].update_lable(ImageNetOption.lables);
            results[1].update_lable(ImageNetOption.lables);
            results[2].update_lable(ImageNetOption.lables);
            Assert.IsNotNull(results);
        }
    }
}