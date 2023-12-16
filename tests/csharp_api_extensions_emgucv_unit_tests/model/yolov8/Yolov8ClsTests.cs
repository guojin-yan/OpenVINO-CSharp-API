﻿using Emgu.CV;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenVinoSharp.Extensions.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenVinoSharp.Extensions.result;
using OpenVinoSharp.Extensions.model;

namespace OpenVinoSharp.Extensions.model.Tests
{
    [TestClass()]
    public class Yolov8ClsTests
    {
        [TestMethod()]
        public void predict_test()
        {
            Yolov8Cls yolo = new Yolov8Cls("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s-cls.xml");
            Mat image = CvInvoke.Imread("..\\..\\..\\..\\..\\dataset\\image\\demo_4.jpg");
            ClsResult result = yolo.predict(image);
            result.update_lable(ImageNetOption.lables);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void predict_test1()
        {
            Yolov8Cls yolo = new Yolov8Cls("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s-cls.xml");
            List<Mat> images = new List<Mat>();
            images.Add(CvInvoke.Imread("..\\..\\..\\..\\..\\dataset\\image\\demo_4.jpg"));
            images.Add(CvInvoke.Imread("..\\..\\..\\..\\..\\dataset\\image\\demo_6.jpg"));
            images.Add(CvInvoke.Imread("..\\..\\..\\..\\..\\dataset\\image\\demo_7.jpg"));
            List<ClsResult> results = yolo.predict(images);
            results[0].update_lable(ImageNetOption.lables);
            results[1].update_lable(ImageNetOption.lables);
            results[2].update_lable(ImageNetOption.lables);
            Assert.IsNotNull(results);
        }

        [TestMethod()]
        public void Yolov8Cls_test()
        {
            Yolov8Cls yolo = new Yolov8Cls("..\\..\\..\\..\\..\\model\\yolov8\\yolov8s-cls.xml");
        }
    }
}