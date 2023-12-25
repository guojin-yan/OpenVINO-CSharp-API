using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.Extensions.result
{
    /// <summary>
    /// Object segmentation result data.
    /// </summary>
    public class SegData : DetData, IResultData
    {
        /// <summary>
        /// Split Region
        /// </summary>
        public Mat mask;
        public SegData() { }
        /// <summary>
        /// Parameter construction.
        /// </summary>
        /// <param name="index">Target segmentation recognition result  number.</param>
        /// <param name="lable">Target segmentation recognition result  label.</param>
        /// <param name="score">Target segmentation recognition result  score.</param>
        /// <param name="box">Target segmentation recognition result  box.</param>
        /// <param name="mask">Target segmentation recognition result split region.</param>
        public SegData(int index, string lable, float score, Rect box, Mat mask)
        {
            this.index = index;
            this.lable = lable;
            this.score = score;
            this.box = box;
            this.mask = mask;
        }
        /// <summary>
        /// Parameter construction.
        /// </summary>
        /// <param name="index">Target segmentation recognition result  number.</param>
        /// <param name="score">Target segmentation recognition result  score.</param>
        /// <param name="box">Target segmentation recognition result  box.</param>
        /// <param name="mask">Target segmentation recognition result split region.</param>
        public SegData(int index, float score, Rect box, Mat mask)
            : this(index, null, score, box, mask)
        { }
        /// <summary>
        /// Update lable.
        /// </summary>
        /// <param name="lables">Lable array.</param>
        /// <returns>DetData class.</returns>
        public SegData update_lable(List<string> lables)
        {
            this.lable = lables[this.index];
            return this;
        }
        /// <summary>
        /// Update lable.
        /// </summary>
        /// <param name="lables">Lable array.</param>
        /// <returns>DetData class.</returns>
        public SegData update_lable(string[] lables)
        {
            this.lable = lables[this.index];
            return this;
        }
        /// <summary>
        /// Converts the numeric value of this instance to its equivalent string representation.
        /// </summary>
        /// <param name="format">A numeric format string.</param>
        /// <returns>SegData string.</returns>
        public string to_string(string format = "0.00")
        {
            string msg = "";
            msg += ("index: " + index.ToString() + "\t");
            if (lable != null)
                msg += ("lable: " + lable.ToString() + "\t");
            msg += ("score: " + score.ToString(format) + "\t");
            msg += ("box: " + box.ToString() + "\t");
            return msg;
        }
    };
    /// <summary>
    /// Object segmentation result class.
    /// </summary>
    public class SegResult : Result<SegData>
    {
        /// <summary>
        /// Add data.
        /// </summary>
        /// <param name="index">Target segmentation recognition result  number.</param>
        /// <param name="score">Target segmentation recognition result  score.</param>
        /// <param name="box">Target segmentation recognition result  box.</param>
        /// <param name="mask">Target segmentation recognition result split region.</param>
        public void add(int index, float score, Rect box, Mat mask)
        {
            SegData data = new SegData(index, score, box, mask);
            this.add(data);
        }
        /// <summary>
        /// Add data.
        /// </summary>
        /// <param name="index">Target segmentation recognition result  number.</param>
        /// <param name="lable">Target segmentation recognition result  label.</param>
        /// <param name="score">Target segmentation recognition result  score.</param>
        /// <param name="box">Target segmentation recognition result  box.</param>
        /// <param name="mask">Target segmentation recognition result split region.</param>
        public void add(int index, string lable, float score, Rect box, Mat mask)
        {
            SegData data = new SegData(index, lable, score, box, mask);
            this.add(data);
        }
        /// <summary>
        /// Sorts the index elements in the entire inference results using the default comparer.
        /// </summary>
        /// <param name="flag"></param>
        public void sort_by_index(bool flag = true)
        {
            if (flag)
                this.sort((x, y) => x.index.CompareTo(y.index));
            else
                this.sort((x, y) => y.index.CompareTo(x.index));
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
            foreach (SegData data in this.datas)
            {
                INFO(data.to_string(format));
            }
        }
    }
}
