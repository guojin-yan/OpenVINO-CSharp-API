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
using static System.Net.Mime.MediaTypeNames;

namespace OpenVinoSharp.Extensions.model.Tests
{
    [TestClass()]
    public class Yolov8PoseTests
    {
        [TestMethod()]
        public void Yolov8Pose_test()
        {
            Yolov8Pose yolo = new Yolov8Pose("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s-pose.xml");
        }

        [TestMethod()]
        public void predict_test()
        {
            Yolov8Pose yolo = new Yolov8Pose("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s-pose.xml");
            Mat image = CvInvoke.Imread("..\\..\\..\\..\\..\\dataset\\image\\demo_10.jpg");
            PoseResult result = yolo.predict(image);
            Mat im = Visualize.draw_poses(result, image, 0.2f);
            CvInvoke.Imshow("ww", im);
            CvInvoke.WaitKey(0);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void predict_test1()
        {
            Yolov8Pose yolo = new Yolov8Pose("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s-pose.xml");
            List<Mat> images = new List<Mat>();
            images.Add(CvInvoke.Imread("..\\..\\..\\..\\..\\dataset\\image\\demo_1.jpg"));
            images.Add(CvInvoke.Imread("..\\..\\..\\..\\..\\dataset\\image\\demo_9.jpg"));
            images.Add(CvInvoke.Imread("..\\..\\..\\..\\..\\dataset\\image\\demo_10.jpg"));
            List<PoseResult> results = yolo.predict(images);
            Mat im = Visualize.draw_poses(results[0], images[0], 0.2f);
            CvInvoke.Imshow("ww", im);
            CvInvoke.WaitKey(0);
             im = Visualize.draw_poses(results[1], images[1], 0.2f);
            CvInvoke.Imshow("ww", im);
            CvInvoke.WaitKey(0);
             im = Visualize.draw_poses(results[2], images[2], 0.2f);
            CvInvoke.Imshow("ww", im);
            CvInvoke.WaitKey(0);
            Assert.IsNotNull(results);
        }

        [TestMethod()]
        public void process_result_test()
        {
           
        }
    }
}