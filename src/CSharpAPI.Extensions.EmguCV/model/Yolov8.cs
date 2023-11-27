using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OpeOpenVinoSharp.Extensions.model.Yolov8
{
    ///// <summary>
    ///// Key point data
    ///// </summary>
    //public class Result
    //{
    //    /// <summary>
    //    /// Get Result Length
    //    /// </summary>
    //    public int length
    //    {
    //        get
    //        {
    //            return scores.Count;
    //        }
    //    }

    //    /// <summary>
    //    /// Identification result class
    //    /// </summary>
    //    public List<int> classes = new List<int>();
    //    /// <summary>
    //    /// Confidence value
    //    /// </summary>
    //    public List<float> scores = new List<float>();
    //    /// <summary>
    //    /// Prediction box
    //    /// </summary>
    //    public List<Rect> rects = new List<Rect>();
    //    /// <summary>
    //    /// Split Region
    //    /// </summary>
    //    public List<Mat> masks = new List<Mat>();
    //    /// <summary>
    //    /// Key points of the human body
    //    /// </summary>
    //    public List<PoseData> poses = new List<PoseData>();

    //    /// <summary>
    //    /// object detection
    //    /// </summary>
    //    /// <param name="score">Predictiveness scores</param>
    //    /// <param name="rect">Identification box</param>
    //    /// <param name="cla">Identification class</param>
    //    public void add(float score, Rect rect, int cla)
    //    {
    //        scores.Add(score);
    //        rects.Add(rect);
    //        classes.Add(cla);
    //    }
    //    /// <summary>
    //    /// 物体分割
    //    /// </summary>
    //    /// <param name="score">Predictiveness scores</param>
    //    /// <param name="rect">Identification box </param>
    //    /// <param name="cla">Identification class</param>
    //    /// <param name="mask">Semantic segmentation results</param>
    //    public void add(float score, Rect rect, int cla, Mat mask)
    //    {
    //        scores.Add(score);
    //        rects.Add(rect);
    //        classes.Add(cla);
    //        masks.Add(mask);
    //    }
    //    /// <summary>
    //    /// Key point prediction
    //    /// </summary>
    //    /// <param name="score">Predictiveness scores</param>
    //    /// <param name="rect">Identification box</param>
    //    /// <param name="pose">Key point data</param>
    //    public void add(float score, Rect rect, PoseData pose)
    //    {
    //        scores.Add(score);
    //        rects.Add(rect);
    //        poses.Add(pose);
    //    }
    //}
    ///// <summary>
    ///// Human Key Point Data
    ///// </summary>
    //public struct PoseData
    //{
    //    /// <summary>
    //    /// Key point prediction score
    //    /// </summary>
    //    public float[] score;
    //    /// <summary>
    //    /// Key point prediction results.
    //    /// </summary>
    //    public List<Point> point;
    //    /// <summary>
    //    /// Default Constructor
    //    /// </summary>
    //    /// <param name="data">Key point prediction results.</param>
    //    /// <param name="scales">Image scaling ratio.</param>
    //    public PoseData(float[] data, float[] scales)
    //    {
    //        score = new float[data.Length];
    //        point = new List<Point>();
    //        for (int i = 0; i < 17; i++)
    //        {
    //            Point p = new Point((int)(data[3 * i] * scales[0]), (int)(data[3 * i + 1] * scales[1]));
    //            this.point.Add(p);
    //            this.score[i] = data[3 * i + 2];
    //        }
    //    }
    //    /// <summary>
    //    /// Convert PoseData to string.
    //    /// </summary>
    //    /// <returns>PoseData string.</returns>
    //    public string to_string() 
    //    {
    //        string[] point_str = new string[] { "Nose", "Left Eye", "Right Eye", "Left Ear", "Right Ear", 
    //            "Left Shoulder", "Right Shoulder", "Left Elbow", "Right Elbow", "Left Wrist", "Right Wrist",
    //            "Left Hip", "Right Hip", "Left Knee", "Right Knee", "Left Ankle", "Right Ankle" };
    //        string ss = "";
    //        for (int i = 0; i < point.Count; i++) 
    //        {
    //            ss += point_str[i] + ": (" + point[i].X.ToString("0") + " ," + point[i].Y.ToString("0") + " ," + score[i].ToString("0.00") + ") ";
    //        }
    //        return ss;
    //    }
    //}

    ///// <summary>
    ///// Yolov8 model inference result processing method.
    ///// </summary>
    //public class ResultProcess 
    //{
    //    /// <summary>
    //    /// Identify Result Types
    //    /// </summary>
    //    public string[] class_names;
    //    /// <summary>
    //    /// Image information scaling ratio h, scaling ratio h, height, width
    //    /// </summary>
    //    public float[] scales;
    //    /// <summary>
    //    /// Confidence threshold
    //    /// </summary>
    //    public float score_threshold;
    //    /// <summary>
    //    /// Non maximum suppression threshold
    //    /// </summary>
    //    public float nms_threshold;
    //    /// <summary>
    //    /// Number of categories
    //    /// </summary>
    //    public int categ_nums = 0;


    //    /// <summary>
    //    /// SegmentationResult processing class construction
    //    /// </summary>
    //    /// <param name="scales">scaling ratio h, scaling ratio h, height, width</param>
    //    /// <param name="score_threshold">score threshold</param>
    //    /// <param name="nms_threshold">nms threshold</param>
    //    public ResultProcess(float[] scales, int categ_nums, float score_threshold = 0.3f, float nms_threshold = 0.5f)
    //    {
    //        this.scales = scales;
    //        this.score_threshold = score_threshold;
    //        this.nms_threshold = nms_threshold;
    //        this.categ_nums = categ_nums;
    //    }

    //    /// <summary>
    //    /// Read local recognition result type file to memory
    //    /// </summary>
    //    /// <param name="path">file path</param>
    //    /// <remarks>
    //    ///     <para>Only the. txt file format is supported, and the content format for this category is as follows:</para>
    //    ///     <para>sea lion</para>
    //    ///     <para>Scottish deerhound</para>
    //    ///     <para>tiger cat</para>
    //    ///     <para>···</para>
    //    /// </remarks>
    //    public void read_class_names(string path)
    //    {

    //        List<string> str = new List<string>();
    //        StreamReader sr = new StreamReader(path);
    //        string line;
    //        while ((line = sr.ReadLine()) != null)
    //        {
    //            str.Add(line);
    //        }

    //        class_names = str.ToArray();
    //    }

    //    /// <summary>
    //    /// Result process
    //    /// </summary>
    //    /// <param name="result">Model prediction output</param>
    //    /// <returns>Model recognition results</returns>
    //    public KeyValuePair<int, float>[] process_cls_result(float[] result)
    //    {
    //        List<float[]> new_list = new List<float[]> { };
    //        for (int i = 0; i < result.Length; i++)
    //        {
    //            new_list.Add(new float[] { (float)result[i], i });
    //        }
    //        new_list.Sort((a, b) => b[0].CompareTo(a[0]));

    //        KeyValuePair<int, float>[] cls = new KeyValuePair<int, float>[10];
    //        for (int i = 0; i < 10; ++i) 
    //        {
    //            cls[i] = new KeyValuePair<int, float>((int)new_list[i][1], new_list[i][0]);
    //        }
    //        return cls;
    //    }

    //    /// <summary>
    //    /// Result drawing
    //    /// </summary>
    //    /// <param name="result">recognition result</param>
    //    /// <param name="image">source image</param>
    //    /// <returns>result image</returns>
    //    public Mat draw_cls_result(KeyValuePair<int, float> result, Mat image)
    //    {
    //        Cv2.PutText(image, class_names[result.Key] + ":  " + result.Value.ToString("0.00"),
    //                            new Point(25, 30), HersheyFonts.HersheySimplex, 1, new Scalar(0, 0, 255), 2);
    //        return image;
    //    }



    //    /// <summary>
    //    /// Result process
    //    /// </summary>
    //    /// <param name="result">Model prediction output</param>
    //    /// <returns>Model recognition results</returns>
    //    public Result process_det_result(float[] result)
    //    {
    //        Mat result_data = new Mat(4 + categ_nums, 8400, MatType.CV_32F, result);
    //        result_data = result_data.T();

    //        // Storage results list
    //        List<Rect> position_boxes = new List<Rect>();
    //        List<int> class_ids = new List<int>();
    //        List<float> confidences = new List<float>();
    //        // Preprocessing output results
    //        for (int i = 0; i < result_data.Rows; i++)
    //        {
    //            Mat classes_scores = result_data.Row(i).ColRange(4, 4 + categ_nums);//GetArray(i, 5, classes_scores);
    //            Point max_classId_point, min_classId_point;
    //            double max_score, min_score;
    //            // Obtain the maximum value and its position in a set of data
    //            Cv2.MinMaxLoc(classes_scores, out min_score, out max_score,
    //                out min_classId_point, out max_classId_point);
    //            // Confidence level between 0 ~ 1
    //            // Obtain identification box information
    //            if (max_score > 0.25)
    //            {
    //                float cx = result_data.At<float>(i, 0);
    //                float cy = result_data.At<float>(i, 1);
    //                float ow = result_data.At<float>(i, 2);
    //                float oh = result_data.At<float>(i, 3);
    //                int x = (int)((cx - 0.5 * ow) * this.scales[0]);
    //                int y = (int)((cy - 0.5 * oh) * this.scales[1]);
    //                int width = (int)(ow * this.scales[0]);
    //                int height = (int)(oh * this.scales[1]);
    //                Rect box = new Rect();
    //                box.X = x;
    //                box.Y = y;
    //                box.Width = width;
    //                box.Height = height;

    //                position_boxes.Add(box);
    //                class_ids.Add(max_classId_point.X);
    //                confidences.Add((float)max_score);
    //            }
    //        }

    //        // NMS non maximum suppression
    //        int[] indexes = new int[position_boxes.Count];
    //        CvDnn.NMSBoxes(position_boxes, confidences, this.score_threshold, this.nms_threshold, out indexes);

    //        Result re_result = new Result();
    //        // 
    //        for (int i = 0; i < indexes.Length; i++)
    //        {
    //            int index = indexes[i];
    //            re_result.add(confidences[index], position_boxes[index], class_ids[index]);
    //        }
    //        return re_result;
    //    }

    //    /// <summary>
    //    /// Result drawing
    //    /// </summary>
    //    /// <param name="result">recognition result</param>
    //    /// <param name="image">image</param>
    //    /// <returns></returns>
    //    public Mat draw_det_result(Result result, Mat image)
    //    {

    //        // Draw recognition results on the image
    //        for (int i = 0; i < result.length; i++)
    //        {
    //            //Console.WriteLine(result.rects[i]);
    //            Cv2.Rectangle(image, result.rects[i], new Scalar(0, 0, 255), 2, LineTypes.Link8);
    //            Cv2.Rectangle(image, new Point(result.rects[i].TopLeft.X, result.rects[i].TopLeft.Y + 30),
    //                new Point(result.rects[i].BottomRight.X, result.rects[i].TopLeft.Y), new Scalar(0, 255, 255), -1);
    //            Cv2.PutText(image, class_names[ result.classes[i]] + "-" + result.scores[i].ToString("0.00"),
    //                new Point(result.rects[i].X, result.rects[i].Y + 25),
    //                HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 0, 0), 2);
    //        }
    //        return image;
    //    }

    //    /// <summary>
    //    /// sigmoid
    //    /// </summary>
    //    /// <param name="a"></param>
    //    /// <returns></returns>
    //    private float sigmoid(float a)
    //    {
    //        float b = 1.0f / (1.0f + (float)Math.Exp(-a));
    //        return b;
    //    }

    //    /// <summary>
    //    /// Result process
    //    /// </summary>
    //    /// <param name="detect">detection output</param>
    //    /// <param name="proto">segmentation output</param>
    //    /// <returns></returns>
    //    public Result process_seg_result(float[] detect, float[] proto)
    //    {
    //        Mat detect_data = new Mat(36 + categ_nums, 8400, MatType.CV_32F, detect);
    //        Mat proto_data = new Mat(32, 25600, MatType.CV_32F, proto);
    //        detect_data = detect_data.T();
    //        List<Rect> position_boxes = new List<Rect>();
    //        List<int> class_ids = new List<int>();
    //        List<float> confidences = new List<float>();
    //        List<Mat> masks = new List<Mat>();
    //        for (int i = 0; i < detect_data.Rows; i++)
    //        {

    //            Mat classes_scores = detect_data.Row(i).ColRange(4, 4 + categ_nums);//GetArray(i, 5, classes_scores);
    //            Point max_classId_point, min_classId_point;
    //            double max_score, min_score;
    //            Cv2.MinMaxLoc(classes_scores, out min_score, out max_score,
    //                out min_classId_point, out max_classId_point);

    //            if (max_score > 0.25)
    //            {
    //                //Console.WriteLine(max_score);

    //                Mat mask = detect_data.Row(i).ColRange(4 + categ_nums, categ_nums + 36);

    //                float cx = detect_data.At<float>(i, 0);
    //                float cy = detect_data.At<float>(i, 1);
    //                float ow = detect_data.At<float>(i, 2);
    //                float oh = detect_data.At<float>(i, 3);
    //                int x = (int)((cx - 0.5 * ow) * this.scales[0]);
    //                int y = (int)((cy - 0.5 * oh) * this.scales[1]);
    //                int width = (int)(ow * this.scales[0]);
    //                int height = (int)(oh * this.scales[1]);
    //                Rect box = new Rect();
    //                box.X = x;
    //                box.Y = y;
    //                box.Width = width;
    //                box.Height = height;

    //                position_boxes.Add(box);
    //                class_ids.Add(max_classId_point.X);
    //                confidences.Add((float)max_score);
    //                masks.Add(mask);
    //            }
    //        }


    //        int[] indexes = new int[position_boxes.Count];
    //        CvDnn.NMSBoxes(position_boxes, confidences, this.score_threshold, this.nms_threshold, out indexes);

    //        Result re_result = new Result(); // Output Result Class
    //        // RGB images with colors
    //        Mat rgb_mask = Mat.Zeros(new Size((int)scales[3], (int)scales[2]), MatType.CV_8UC3);
    //        Random rd = new Random(); // Generate Random Numbers
    //        for (int i = 0; i < indexes.Length; i++)
    //        {
    //            int index = indexes[i];
    //            // Division scope
    //            Rect box = position_boxes[index];
    //            int box_x1 = Math.Max(0, box.X);
    //            int box_y1 = Math.Max(0, box.Y);
    //            int box_x2 = Math.Max(0, box.BottomRight.X);
    //            int box_y2 = Math.Max(0, box.BottomRight.Y);

    //            // Segmentation results
    //            Mat original_mask = masks[index] * proto_data;
    //            for (int col = 0; col < original_mask.Cols; col++)
    //            {
    //                original_mask.At<float>(0, col) = sigmoid(original_mask.At<float>(0, col));
    //            }
    //            // 1x25600 -> 160x160 Convert to original size
    //            Mat reshape_mask = original_mask.Reshape(1, 160);

    //            //Console.WriteLine("m1.size = {0}", m1.Size());

    //            // Split size after scaling
    //            int mx1 = Math.Max(0, (int)((box_x1 / scales[0]) * 0.25));
    //            int mx2 = Math.Max(0, (int)((box_x2 / scales[0]) * 0.25));
    //            int my1 = Math.Max(0, (int)((box_y1 / scales[1]) * 0.25));
    //            int my2 = Math.Max(0, (int)((box_y2 / scales[1]) * 0.25));
    //            // Crop Split Region
    //            Mat mask_roi = new Mat(reshape_mask, new OpenCvSharp.Range(my1, my2), new OpenCvSharp.Range(mx1, mx2));
    //            // Convert the segmented area to the actual size of the image
    //            Mat actual_maskm = new Mat();
    //            Cv2.Resize(mask_roi, actual_maskm, new Size(box_x2 - box_x1, box_y2 - box_y1));
    //            // Binary segmentation region
    //            for (int r = 0; r < actual_maskm.Rows; r++)
    //            {
    //                for (int c = 0; c < actual_maskm.Cols; c++)
    //                {
    //                    float pv = actual_maskm.At<float>(r, c);
    //                    if (pv > 0.5)
    //                    {
    //                        actual_maskm.At<float>(r, c) = 1.0f;
    //                    }
    //                    else
    //                    {
    //                        actual_maskm.At<float>(r, c) = 0.0f;
    //                    }
    //                }
    //            }

    //            // 预测
    //            Mat bin_mask = new Mat();
    //            actual_maskm = actual_maskm * 200;
    //            actual_maskm.ConvertTo(bin_mask, MatType.CV_8UC1);
    //            if ((box_y1 + bin_mask.Rows) >= scales[2])
    //            {
    //                box_y2 = (int)scales[2] - 1;
    //            }
    //            if ((box_x1 + bin_mask.Cols) >= scales[3])
    //            {
    //                box_x2 = (int)scales[3] - 1;
    //            }
    //            // Obtain segmentation area
    //            Mat mask = Mat.Zeros(new Size((int)scales[3], (int)scales[2]), MatType.CV_8UC1);
    //            bin_mask = new Mat(bin_mask, new OpenCvSharp.Range(0, box_y2 - box_y1), new OpenCvSharp.Range(0, box_x2 - box_x1));
    //            Rect roi = new Rect(box_x1, box_y1, box_x2 - box_x1, box_y2 - box_y1);
    //            bin_mask.CopyTo(new Mat(mask, roi));
    //            // Color segmentation area
    //            Cv2.Add(rgb_mask, new Scalar(rd.Next(0, 255), rd.Next(0, 255), rd.Next(0, 255)), rgb_mask, mask);

    //            re_result.add(confidences[index], position_boxes[index], class_ids[index], rgb_mask.Clone());

    //        }

    //        return re_result;
    //    }

    //    /// <summary>
    //    /// Result drawing
    //    /// </summary>
    //    /// <param name="result">recognition result</param>
    //    /// <param name="image">image</param>
    //    /// <returns></returns>
    //    public Mat draw_seg_result(Result result, Mat image)
    //    {
    //        Mat masked_img = new Mat();
    //        // Draw recognition results on the image
    //        for (int i = 0; i < result.length; i++)
    //        {
    //            Cv2.Rectangle(image, result.rects[i], new Scalar(0, 0, 255), 2, LineTypes.Link8);
    //            Cv2.Rectangle(image, new Point(result.rects[i].TopLeft.X, result.rects[i].TopLeft.Y + 30),
    //                new Point(result.rects[i].BottomRight.X, result.rects[i].TopLeft.Y), new Scalar(0, 255, 255), -1);
    //            Cv2.PutText(image, class_names[result.classes[i]] + "-" + result.scores[i].ToString("0.00"),
    //                new Point(result.rects[i].X, result.rects[i].Y + 25),
    //                HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 0, 0), 2);
    //            Cv2.AddWeighted(image, 0.5, result.masks[i], 0.5, 0, masked_img);
    //        }
    //        return masked_img;
    //    }

    //    /// <summary>
    //    /// Result process
    //    /// </summary>
    //    /// <param name="result">Model prediction output</param>
    //    /// <returns>Model recognition results</returns>
    //    public Result process_pose_result(float[] result)
    //    {
    //        Mat result_data = new Mat(56, 8400, MatType.CV_32F, result);
    //        result_data = result_data.T();
    //        List<Rect> position_boxes = new List<Rect>();
    //        List<float> confidences = new List<float>();
    //        List<PoseData> pose_datas = new List<PoseData>();
    //        for (int i = 0; i < result_data.Rows; i++)
    //        {
    //            if (result_data.At<float>(i, 4) > 0.25)
    //            {
    //                //Console.WriteLine(max_score);
    //                float cx = result_data.At<float>(i, 0);
    //                float cy = result_data.At<float>(i, 1);
    //                float ow = result_data.At<float>(i, 2);
    //                float oh = result_data.At<float>(i, 3);
    //                int x = (int)((cx - 0.5 * ow) * this.scales[0]);
    //                int y = (int)((cy - 0.5 * oh) * this.scales[1]);
    //                int width = (int)(ow * this.scales[0]);
    //                int height = (int)(oh * this.scales[1]);
    //                Rect box = new Rect();
    //                box.X = x;
    //                box.Y = y;
    //                box.Width = width;
    //                box.Height = height;
    //                Mat pose_mat = result_data.Row(i).ColRange(5, 56);
    //                float[] pose_data = new float[51];
    //                pose_mat.GetArray<float>(out pose_data);
    //                PoseData pose = new PoseData(pose_data, this.scales);

    //                position_boxes.Add(box);

    //                confidences.Add((float)result_data.At<float>(i, 4));
    //                pose_datas.Add(pose);
    //            }
    //        }

    //        int[] indexes = new int[position_boxes.Count];
    //        CvDnn.NMSBoxes(position_boxes, confidences, this.score_threshold, this.nms_threshold, out indexes);

    //        Result re_result = new Result();
    //        for (int i = 0; i < indexes.Length; i++)
    //        {
    //            int index = indexes[i];
    //            re_result.add(confidences[index], position_boxes[index], pose_datas[index]);
    //            //Console.WriteLine("rect: {0}, score: {1}", position_boxes[index], confidences[index]);
    //        }
    //        return re_result;

    //    }
    //    /// <summary>
    //    /// Result drawing
    //    /// </summary>
    //    /// <param name="result">recognition result</param>
    //    /// <param name="image">image</param>
    //    /// <returns></returns>
    //    public Mat draw_pose_result(Result result, Mat image, double visual_thresh)
    //    {

    //        // 将识别结果绘制到图片上
    //        for (int i = 0; i < result.length; i++)
    //        {
    //            Cv2.Rectangle(image, result.rects[i], new Scalar(0, 0, 255), 2, LineTypes.Link8);

    //            draw_poses(result.poses[i], ref image, visual_thresh);
    //        }
    //        return image;
    //    }
    //    /// <summary>
    //    /// Key point result drawing
    //    /// </summary>
    //    /// <param name="pose">Key point data</param>
    //    /// <param name="image">image</param>
    //    public void draw_poses(PoseData pose, ref Mat image, double visual_thresh)
    //    {
    //        // Connection point relationship
    //        int[,] edgs = new int[17, 2] { { 0, 1 }, { 0, 2}, {1, 3}, {2, 4}, {3, 5}, {4, 6}, {5, 7}, {6, 8},
    //             {7, 9}, {8, 10}, {5, 11}, {6, 12}, {11, 13}, {12, 14},{13, 15 }, {14, 16 }, {11, 12 } };
    //        // Color Library
    //        Scalar[] colors = new Scalar[18] { new Scalar(255, 0, 0), new Scalar(255, 85, 0), new Scalar(255, 170, 0),
    //            new Scalar(255, 255, 0), new Scalar(170, 255, 0), new Scalar(85, 255, 0), new Scalar(0, 255, 0),
    //            new Scalar(0, 255, 85), new Scalar(0, 255, 170), new Scalar(0, 255, 255), new Scalar(0, 170, 255),
    //            new Scalar(0, 85, 255), new Scalar(0, 0, 255), new Scalar(85, 0, 255), new Scalar(170, 0, 255),
    //            new Scalar(255, 0, 255), new Scalar(255, 0, 170), new Scalar(255, 0, 85) };
    //        // Draw Keys
    //        for (int p = 0; p < 17; p++)
    //        {
    //            if (pose.score[p] < visual_thresh)
    //            {
    //                continue;
    //            }

    //            Cv2.Circle(image, pose.point[p], 2, colors[p], -1);
    //            //Console.WriteLine(pose.point[p]);
    //        }
    //        // draw
    //        for (int p = 0; p < 17; p++)
    //        {
    //            if (pose.score[edgs[p, 0]] < visual_thresh || pose.score[edgs[p, 1]] < visual_thresh)
    //            {
    //                continue;
    //            }

    //            float[] point_x = new float[] { pose.point[edgs[p, 0]].X, pose.point[edgs[p, 1]].X };
    //            float[] point_y = new float[] { pose.point[edgs[p, 0]].Y, pose.point[edgs[p, 1]].Y };

    //            Point center_point = new Point((int)((point_x[0] + point_x[1]) / 2), (int)((point_y[0] + point_y[1]) / 2));
    //            double length = Math.Sqrt(Math.Pow((double)(point_x[0] - point_x[1]), 2.0) + Math.Pow((double)(point_y[0] - point_y[1]), 2.0));
    //            int stick_width = 2;
    //            Size axis = new Size(length / 2, stick_width);
    //            double angle = (Math.Atan2((double)(point_y[0] - point_y[1]), (double)(point_x[0] - point_x[1]))) * 180 / Math.PI;
    //            Point[] polygon = Cv2.Ellipse2Poly(center_point, axis, (int)angle, 0, 360, 1);
    //            Cv2.FillConvexPoly(image, polygon, colors[p]);

    //        }
    //    }
    //    /// <summary>
    //    /// Print and output image classification results
    //    /// </summary>
    //    /// <param name="result">classification results</param>
    //    public void print_result(KeyValuePair<int, float>[] result)
    //    {
    //        Console.WriteLine("\n Classification Top 10 result : \n");
    //        Console.WriteLine("classid probability");
    //        Console.WriteLine("------- -----------");
    //        for (int i = 0; i < 10; ++i)
    //        {
    //            Console.WriteLine("{0}     {1}", result[i].Key.ToString("0"), result[i].Value.ToString("0.000000"));
    //        }
    //    }
    //    /// <summary>
    //    /// Print out image prediction results
    //    /// </summary>
    //    /// <param name="result">prediction results</param>
    //    public void print_result(Result result)
    //    {
    //        if (result.poses.Count != 0) 
    //        {
    //            Console.WriteLine("\n Classification  result : \n");
    //            for (int i = 0; i < result.length; ++i) 
    //            {
    //                string ss = (i + 1).ToString() + ": 1   " + result.scores[i].ToString("0.00") + "   " + result.rects[i].ToString()
    //                    +"  " + result.poses[i].to_string();
    //                Console.WriteLine(ss);
    //            }
    //            return;
    //        }

    //        if (result.masks.Count != 0) 
    //        {
    //            Console.WriteLine("\n  Segmentation  result : \n");
    //            for (int i = 0; i < result.length; ++i)
    //            {
    //                string ss = (i + 1).ToString() + ": " + result.classes[i]+ "\t" + result.scores[i].ToString("0.00") + "   " + result.rects[i].ToString();
    //                Console.WriteLine(ss);
    //            }
    //            return;
    //        }
    //        Console.WriteLine("\n  Detection  result : \n");
    //        for (int i = 0; i < result.length; ++i)
    //        {
    //            string ss = (i + 1).ToString() + ": " + result.classes[i] + "\t" + result.scores[i].ToString("0.00") + "   " + result.rects[i].ToString();
    //            Console.WriteLine(ss);
    //        }
        
    //    }

    //};

}
