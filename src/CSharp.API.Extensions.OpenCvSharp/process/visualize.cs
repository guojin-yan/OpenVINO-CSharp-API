using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenVinoSharp.Extensions.result;
using OpenVinoSharp.Extensions.model;

namespace OpenVinoSharp.Extensions.process
{
    public static class Visualize
    {

        /// <summary>
        /// Result drawing
        /// </summary>
        /// <param name="result">recognition result</param>
        /// <param name="image">image</param>
        /// <returns></returns>
        public static Mat draw_det_result(DetResult result, Mat image)
        {
            // Draw recognition results on the image
            for (int i = 0; i < result.count; i++)
            {
                //Console.WriteLine(result.rects[i]);
                Cv2.Rectangle(image, result.datas[i].box, new Scalar(0, 0, 255), 2, LineTypes.Link8);
                Cv2.Rectangle(image, new Point(result.datas[i].box.TopLeft.X, result.datas[i].box.TopLeft.Y + 30),
                    new Point(result.datas[i].box.BottomRight.X, result.datas[i].box.TopLeft.Y), new Scalar(0, 255, 255), -1);
                Cv2.PutText(image, CocoOption.lables[result.datas[i].index] + "-" + result.datas[i].score.ToString("0.00"),
                    new Point(result.datas[i].box.X, result.datas[i].box.Y + 25),
                    HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 0, 0), 2);
            }
            return image;
        }
        public static Mat draw_seg_result(SegResult result, Mat image)
        {
            Mat masked_img = new Mat();
            // Draw recognition results on the image
            for (int i = 0; i < result.count; i++)
            {
                Cv2.Rectangle(image, result.datas[i].box, new Scalar(0, 0, 255), 2, LineTypes.Link8);
                Cv2.Rectangle(image, new Point(result.datas[i].box.TopLeft.X, result.datas[i].box.TopLeft.Y + 30),
                    new Point(result.datas[i].box.BottomRight.X, result.datas[i].box.TopLeft.Y), new Scalar(0, 255, 255), -1);
                Cv2.PutText(image, CocoOption.lables[result.datas[i].index] + "-" + result.datas[i].score.ToString("0.00"),
                    new Point(result.datas[i].box.X, result.datas[i].box.Y + 25),
                    HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 0, 0), 2);
                Cv2.AddWeighted(image, 0.5, result.datas[i].mask, 0.5, 0, masked_img);
            }
            return masked_img;
        }

        /// <summary>
        /// Key point result drawing
        /// </summary>
        /// <param name="pose">Key point data</param>
        /// <param name="image">image</param>
        public static Mat draw_poses(PoseResult pose, Mat img, float visual_thresh = 0.2f, bool with_box = true)
        {
            Mat image = img.Clone();
            // Connection point relationship
            int[,] edgs = new int[17, 2] { { 0, 1 }, { 0, 2}, {1, 3}, {2, 4}, {3, 5}, {4, 6}, {5, 7}, {6, 8},
                 {7, 9}, {8, 10}, {5, 11}, {6, 12}, {11, 13}, {12, 14},{13, 15 }, {14, 16 }, {11, 12 } };
            // Color Library
            Scalar[] colors = new Scalar[18] { new Scalar(255, 0, 0), new Scalar(255, 85, 0), new Scalar(255, 170, 0),
                new Scalar(255, 255, 0), new Scalar(170, 255, 0), new Scalar(85, 255, 0), new Scalar(0, 255, 0),
                new Scalar(0, 255, 85), new Scalar(0, 255, 170), new Scalar(0, 255, 255), new Scalar(0, 170, 255),
                new Scalar(0, 85, 255), new Scalar(0, 0, 255), new Scalar(85, 0, 255), new Scalar(170, 0, 255),
                new Scalar(255, 0, 255), new Scalar(255, 0, 170), new Scalar(255, 0, 85) };
            for (int i = 0; i < pose.count; ++i)
            {
                // Draw Keys
                for (int p = 0; p < 17; p++)
                {
                    if (pose.datas[i].pose_point.score[p] < visual_thresh)
                    {
                        continue;
                    }

                    Cv2.Circle(image, pose.datas[i].pose_point.point[p], 2, colors[p], -1);
                    //Console.WriteLine(pose.point[p]);
                }
                // draw

                Cv2.Rectangle(image, pose.datas[i].box, new Scalar(0, 0, 255), 2, LineTypes.Link8);
                Cv2.Rectangle(image, new Point(pose.datas[i].box.TopLeft.X, pose.datas[i].box.TopLeft.Y + 30),
                    new Point(pose.datas[i].box.BottomRight.X, pose.datas[i].box.TopLeft.Y), new Scalar(0, 255, 255), -1);
                Cv2.PutText(image, CocoOption.lables[pose.datas[i].index] + "-" + pose.datas[i].score.ToString("0.00"),
                    new Point(pose.datas[i].box.X, pose.datas[i].box.Y + 25),
                    HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 0, 0), 2);

                for (int p = 0; p < 17; p++)
                {
                    if (pose.datas[i].pose_point.score[edgs[p, 0]] < visual_thresh ||
                        pose.datas[i].pose_point.score[edgs[p, 1]] < visual_thresh)
                    {
                        continue;
                    }

                    float[] point_x = new float[] { pose.datas[i].pose_point.point[edgs[p, 0]].X,
                        pose.datas[i].pose_point.point[edgs[p, 1]].X };
                    float[] point_y = new float[] { pose.datas[i].pose_point.point[edgs[p, 0]].Y,
                        pose.datas[i].pose_point.point[edgs[p, 1]].Y };

                    Point center_point = new Point((int)((point_x[0] + point_x[1]) / 2), (int)((point_y[0] + point_y[1]) / 2));
                    double length = Math.Sqrt(Math.Pow((double)(point_x[0] - point_x[1]), 2.0) + Math.Pow((double)(point_y[0] - point_y[1]), 2.0));
                    int stick_width = 2;
                    Size axis = new Size(length / 2, stick_width);
                    double angle = (Math.Atan2((double)(point_y[0] - point_y[1]), (double)(point_x[0] - point_x[1]))) * 180 / Math.PI;
                    Point[] polygon = Cv2.Ellipse2Poly(center_point, axis, (int)angle, 0, 360, 1);
                    Cv2.FillConvexPoly(image, polygon, colors[p]);

                }
            }
            return image;
           
        }

    }
}
