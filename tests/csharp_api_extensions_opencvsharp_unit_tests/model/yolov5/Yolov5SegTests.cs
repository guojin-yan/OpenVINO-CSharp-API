using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
using OpenVinoSharp.Extensions.result;
using OpenVinoSharp.Extensions.process;
using OpenVinoSharp.Extensions.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.Extensions.model.Tests
{
    [TestClass()]
    public class Yolov5SegTests
    {
        public string model_xml_path = "..\\..\\..\\..\\..\\tests\\test_data\\model\\yolov5\\yolov5s-seg.onnx";
        public string image_path = "..\\..\\..\\..\\..\\tests\\test_data\\image\\demo_1.jpg";
        public string image_path1 = "..\\..\\..\\..\\..\\tests\\test_data\\image\\demo_2.jpg";
        public string image_path2 = "..\\..\\..\\..\\..\\tests\\test_data\\image\\demo_3.jpg";

        [TestMethod()]
        public void Yolov5Seg_test()
        {
            Yolov5SegConfig config = new Yolov5SegConfig(model_xml_path);
            Yolov5Seg yolo = new Yolov5Seg(config);
        }

        [TestMethod()]
        public void predict_test()
        {
            Yolov5SegConfig config = new Yolov5SegConfig(model_xml_path);
            Yolov5Seg yolo = new Yolov5Seg(config);
            Mat image = Cv2.ImRead(image_path1);
            SegResult result = yolo.predict(image);
            Mat im = Visualize.draw_seg_result(result, image);
            Cv2.ImShow("ww", im);
            Cv2.WaitKey(0);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void predict_test1()
        {
            Yolov5SegConfig config = new Yolov5SegConfig(model_xml_path);
            Yolov5Seg yolo = new Yolov5Seg(config);
            List<Mat> images = new List<Mat>();
            images.Add(Cv2.ImRead(image_path));
            images.Add(Cv2.ImRead(image_path1));
            images.Add(Cv2.ImRead(image_path2));
            List<SegResult> results = yolo.predict(images);
            Mat im = Visualize.draw_seg_result(results[0], images[0]);
            Cv2.ImShow("ww", im);
            Cv2.WaitKey(0);
            im = Visualize.draw_seg_result(results[1], images[1]);
            Cv2.ImShow("ww", im);
            Cv2.WaitKey(0);
            im = Visualize.draw_seg_result(results[2], images[2]);
            Cv2.ImShow("ww", im);
            Cv2.WaitKey(0);
            Assert.IsNotNull(results);
        }

        [TestMethod()]
        public void process_result_test()
        {
           
        }
    }
}