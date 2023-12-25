
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.Extensions.result
{
    public struct PosePoint
    {
        /// <summary>
        /// Key point prediction score
        /// </summary>
        public float[] score;
        /// <summary>
        /// Key point prediction results.
        /// </summary>
        public List<Point> point;
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="data">Key point prediction results.</param>
        /// <param name="scales">Image scaling ratio.</param>
        public PosePoint(float[] data, float scales)
        {
            score = new float[data.Length];
            point = new List<Point>();
            for (int i = 0; i < 17; i++)
            {
                Point p = new Point((int)(data[3 * i] * scales), (int)(data[3 * i + 1] * scales));
                this.point.Add(p);
                this.score[i] = data[3 * i + 2];
            }
        }
        /// <summary>
        /// Convert PoseData to string.
        /// </summary>
        /// <returns>PoseData string.</returns>
        public string to_string(string format = "0.00")
        {
            string[] point_str = new string[] { "Nose", "Left Eye", "Right Eye", "Left Ear", "Right Ear",
                "Left Shoulder", "Right Shoulder", "Left Elbow", "Right Elbow", "Left Wrist", "Right Wrist",
                "Left Hip", "Right Hip", "Left Knee", "Right Knee", "Left Ankle", "Right Ankle" };
            string ss = "";
            for (int i = 0; i < point.Count; i++)
            {
                ss += point_str[i] + ": (" + point[i].X.ToString(format) + " ," + point[i].Y.ToString(format)
                    + " ," + score[i].ToString(format) + ") ";
            }
            return ss;
        }
    }
    public class PoseData : IResultData
    {
        /// <summary>
        /// Identification result class index.
        /// </summary>
        public int index = 1;
        /// <summary>
        /// Identification result class lable.
        /// </summary>
        public string lable = "human";
        /// <summary>
        /// Confidence value.
        /// </summary>
        public float score;
        /// <summary>
        /// Prediction box.
        /// </summary>
        public Rectangle box;

        public PosePoint pose_point;
        public PoseData() { }
        public PoseData(int index, string lable, float score, Rectangle box, PosePoint pose)
        {
            this.index = index;
            this.lable = lable;
            this.score = score;
            this.box = box;
            this.pose_point = pose;
        }
        public PoseData(int index, string lable, float score, Rectangle box, float[] pose_data, float scales)
        {
            this.index = index;
            this.lable = lable;
            this.score = score;
            this.box = box;
            PosePoint pose = new PosePoint(pose_data, scales);
            this.pose_point = pose;
        }

        public PoseData( float score, Rectangle box, PosePoint pose)
        {
            this.index = 1;
            this.lable = "human";
            this.score = score;
            this.box = box;
            this.pose_point = pose;
        }

        public PoseData(float score, Rectangle box, float[] pose_data, float scales)
        {
            this.index = 1;
            this.lable = "human";
            this.score = score;
            this.box = box;
            PosePoint pose = new PosePoint(pose_data, scales);
            this.pose_point = pose;
        }
        /// <summary>
        /// Converts the numeric value of this instance to its equivalent string representation.
        /// </summary>
        /// <param name="format">A numeric format string.</param>
        /// <returns>DetData string.</returns>
        public string to_string(string format = "0.00")
        {
            string msg = "";
            msg += ("index: " + index.ToString() + "\t");
            if (lable != null)
                msg += ("lable: " + lable.ToString() + "\t");
            msg += ("score: " + score.ToString(format) + "\t");
            msg += ("box: " + box.ToString() + "\t");
            msg += ("pose: " + pose_point.to_string(format));
            return msg;
        }

    }
    public class PoseResult : Result<PoseData>
    {
        /// <summary>
        /// Add data.
        /// </summary>
        /// <param name="index">Identification result number.</param>
        /// <param name="lable">Identification result label.</param>
        /// <param name="score">Identification result score.</param>
        /// <param name="box">Identification result box.</param>
        public void add(int index, string lable, float score, Rectangle box, PosePoint point)
        {
            PoseData data = new PoseData(index, lable, score, box, point);
            this.add(data);
        }

        public void add(int index, string lable, float score, Rectangle box, float[] pose_data, float scales)
        {
            PoseData data = new PoseData(index, lable, score, box,  pose_data, scales);
            this.add(data);
        }
        public void add(float score, Rectangle box, PosePoint point)
        {
            PoseData data = new PoseData( score, box, point);
            this.add(data);
        }

        public void add(float score, Rectangle box, float[] pose_data, float scales)
        {
            PoseData data = new PoseData( score, box, pose_data, scales);
            this.add(data);
        }

        /// <summary>
        /// Sorts the score elements in the entire inference results using the default comparer.
        /// </summary>
        /// <param name="flag"></param>
        public void sort_by_score(bool flag = true)
        {
            if (flag)
                this.sort((x, y) => x.score.CompareTo(y.score));
            else
                this.sort((x, y) => y.score.CompareTo(x.score));
        }
        /// <summary>
        /// Sorts the box elements in the entire inference results using the default comparer.
        /// </summary>
        /// <param name="flag"></param>
        public void sort_by_bbox(bool flag)
        {
            datas.OrderBy(t => t.box.Location.X).ThenBy(t => t.box.Location.Y).ToList();
        }

        /// <summary>
        /// Print the inference results.
        /// </summary>
        /// <param name="format">A numeric format string.</param>
        public override void print(string format = "0.00")
        {
            INFO("Detection results:");
            foreach (PoseData data in this.datas)
            {
                INFO(data.to_string(format));
            }
        }
    }
}
