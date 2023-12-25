using Emgu.CV;
using Emgu.CV.CvEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenVinoSharp.Extensions.result;
using OpenVinoSharp.Extensions.model;
using Emgu.CV.Structure;
using System.Drawing;

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
                CvInvoke.Rectangle(image, result.datas[i].box, new MCvScalar(0, 0, 255), 2, LineType.Filled);
                CvInvoke.Rectangle(image, new Rectangle(new Point(result.datas[i].box.X, result.datas[i].box.Y),
                 new Size(result.datas[i].box.Width, 30)), new MCvScalar(0, 255, 255), -1);
                CvInvoke.PutText(image, CocoOption.lables[result.datas[i].index] + "-" + result.datas[i].score.ToString("0.00"),
                    new Point(result.datas[i].box.X, result.datas[i].box.Y + 25),
                    FontFace.HersheySimplex, 0.8, new MCvScalar(0, 0, 0), 2);
            }
            return image;
        }
        public static Mat draw_seg_result(SegResult result, Mat image)
        {
            Mat masked_img = new Mat(image.Size,DepthType.Cv8U,3);
            // Draw recognition results on the image
            for (int i = 0; i < result.count; i++)
            {
                CvInvoke.Rectangle(image, result.datas[i].box, new MCvScalar(0, 0, 255), 2, LineType.Filled);
                CvInvoke.Rectangle(image, new Rectangle(new Point(result.datas[i].box.X, result.datas[i].box.Y),
                 new Size(result.datas[i].box.Width, 30)), new MCvScalar(0, 255, 255), -1);
                CvInvoke.PutText(image, CocoOption.lables[result.datas[i].index] + "-" + result.datas[i].score.ToString("0.00"),
                    new Point(result.datas[i].box.X, result.datas[i].box.Y + 25),
                    FontFace.HersheySimplex, 0.8, new MCvScalar(0, 0, 0), 2);
                CvInvoke.AddWeighted(masked_img, 1, result.datas[i].mask, 1, 0, masked_img);
            }
            CvInvoke.AddWeighted(masked_img, 0.5, image, 0.5, 0, masked_img);
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
            MCvScalar[] colors = new MCvScalar[18] { new MCvScalar(255, 0, 0), new MCvScalar(255, 85, 0), new MCvScalar(255, 170, 0),
                new MCvScalar(255, 255, 0), new MCvScalar(170, 255, 0), new MCvScalar(85, 255, 0), new MCvScalar(0, 255, 0),
                new MCvScalar(0, 255, 85), new MCvScalar(0, 255, 170), new MCvScalar(0, 255, 255), new MCvScalar(0, 170, 255),
                new MCvScalar(0, 85, 255), new MCvScalar(0, 0, 255), new MCvScalar(85, 0, 255), new MCvScalar(170, 0, 255),
                new MCvScalar(255, 0, 255), new MCvScalar(255, 0, 170), new MCvScalar(255, 0, 85) };
            for (int i = 0; i < pose.count; ++i)
            {
                // Draw Keys
                for (int p = 0; p < 17; p++)
                {
                    if (pose.datas[i].pose_point.score[p] < visual_thresh)
                    {
                        continue;
                    }

                    CvInvoke.Circle(image, pose.datas[i].pose_point.point[p], 2, colors[p], -1);
                    //Console.WriteLine(pose.point[p]);
                }
                // draw

                CvInvoke.Rectangle(image, pose.datas[i].box, new MCvScalar(0, 0, 255), 2, LineType.Filled);
                CvInvoke.Rectangle(image, new Rectangle(new Point(pose.datas[i].box.X, pose.datas[i].box.Y),
                                 new Size(pose.datas[i].box.Width, 30)), new MCvScalar(0, 255, 255), -1);
                CvInvoke.PutText(image, CocoOption.lables[pose.datas[i].index] + "-" + pose.datas[i].score.ToString("0.00"),
                    new Point(pose.datas[i].box.X, pose.datas[i].box.Y + 25),
                    FontFace.HersheySimplex, 0.8, new MCvScalar(0, 0, 0), 2);

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
                    Size axis = new Size((int)length / 2, stick_width);
                    double angle = (Math.Atan2((double)(point_y[0] - point_y[1]), (double)(point_x[0] - point_x[1]))) * 180 / Math.PI;
                    CvInvoke.Ellipse(image, center_point, axis, (int)angle, 0, 360, colors[p],-1);
                    //CvInvoke.FillConvexPoly(image, polygon, colors[p]);

                }
            }
            return image;
           
        }

    }
}
