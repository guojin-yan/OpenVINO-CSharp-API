using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
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
    public class Yolov8PoseTests
    {
        public string model_xml_path = "..\\..\\..\\..\\..\\tests\\test_data\\model\\yolov8\\yolov8s-pose.xml";
        public string image_path = "..\\..\\..\\..\\..\\tests\\test_data\\image\\demo_1.jpg";
        public string image_path1 = "..\\..\\..\\..\\..\\tests\\test_data\\image\\demo_9.jpg";
        public string image_path2 = "..\\..\\..\\..\\..\\tests\\test_data\\image\\demo_10.jpg";
        [TestMethod()]
        public void Yolov8Pose_test()
        {
            Yolov8Pose yolo = new Yolov8Pose(model_xml_path);

        }

        [TestMethod()]
        public void predict_test()
        {
            Yolov8Pose yolo = new Yolov8Pose(model_xml_path);
            Mat image = Cv2.ImRead(image_path2);
            PoseResult result = yolo.predict(image);
            Mat im = Visualize.draw_poses(result, image, 0.2f);
            //Cv2.ImShow("ww", im);
            //Cv2.WaitKey(0);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void predict_test1()
        {
            Yolov8Pose yolo = new Yolov8Pose(model_xml_path);
            List<Mat> images = new List<Mat>();
            images.Add(Cv2.ImRead(image_path));
            images.Add(Cv2.ImRead(image_path1));
            images.Add(Cv2.ImRead(image_path2));
            List<PoseResult> results = yolo.predict(images);
            Assert.IsNotNull(results);
        }

        [TestMethod()]
        public void process_result_test()
        {
        }

        [TestMethod()]
        public void Yolov8Pose_test1()
        {
            Config config = new Yolov8PoseConfig(model_xml_path);
            Yolov8Pose yolo = new Yolov8Pose((Yolov8PoseConfig)config);
        }
    }
}