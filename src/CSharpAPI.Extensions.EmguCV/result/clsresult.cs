using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.Extensions.result
{
    /// <summary>
    /// Object detection result data.
    /// </summary>
    public class ClsData : IResultData
    {
        /// <summary>
        /// Identification result class index.
        /// </summary>
        public int index;
        /// <summary>
        /// Identification result class lable.
        /// </summary>
        public string lable;
        /// <summary>
        /// Confidence value.
        /// </summary>
        public float score;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ClsData() { }
        /// <summary>
        /// Parameter construction.
        /// </summary>
        /// <param name="index">Identification result number.</param>
        /// <param name="lable">Identification result label.</param>
        /// <param name="score">Identification result score.</param>
        /// <param name="box">Identification result box.</param>
        public ClsData(int index, string lable, float score)
        {
            this.index = index;
            this.lable = lable;
            this.score = score;
        }
        public ClsData(int index, float score)
        {
            this.index = index;
            this.score = score;
        }
        /// <summary>
        /// Update lable.
        /// </summary>
        /// <param name="lables">Lable array.</param>
        /// <returns>DetData class.</returns>
        public ClsData update_lable(List<string> lables)
        {
            this.lable = lables[this.index];
            return this;
        }
        /// <summary>
        /// Update lable.
        /// </summary>
        /// <param name="lables">Lable array.</param>
        /// <returns>DetData class.</returns>
        public ClsData update_lable(string[] lables)
        {
            this.lable = lables[this.index];
            return this;
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
            return msg;
        }
    };
    public class ClsResult : Result<ClsData>
    {
        /// <summary>
        /// Add data.
        /// </summary>
        /// <param name="index">Identification result number.</param>
        /// <param name="score">Identification result score.</param>
        /// <param name="box">Identification result box.</param>
        public void add(int index, float score)
        {
            ClsData data = new ClsData(index, score);
            this.add(data);
        }
        /// <summary>
        /// Add data.
        /// </summary>
        /// <param name="index">Identification result number.</param>
        /// <param name="lable">Identification result label.</param>
        /// <param name="score">Identification result score.</param>
        /// <param name="box">Identification result box.</param>
        public void add(int index, string lable, float score)
        {
            ClsData data = new ClsData(index, lable, score);
            this.add(data);
        }

        /// <summary>
        /// Update lable.
        /// </summary>
        /// <param name="lables">Lable array.</param>
        /// <returns>DetData class.</returns>
        public void update_lable(List<string> lables)
        {
            foreach (ClsData data in this.datas)
            {
                data.update_lable(lables);
            }
        }
        /// <summary>
        /// Update lable.
        /// </summary>
        /// <param name="lables">Lable array.</param>
        /// <returns>DetData class.</returns>
        public void update_lable(string[] lables)
        {
            foreach (ClsData data in this.datas)
            {
                data.update_lable(lables);
            }
        }
        /// <summary>
        /// Print the inference results.
        /// </summary>
        /// <param name="format">A numeric format string.</param>
        public override void print(string format = "0.00")
        {
            INFO(string.Format("\n Classification Top {0} result : \n",count));
            INFO("classid probability");
            INFO("------- -----------");
            foreach (ClsData data in this.datas)
            {
                INFO(data.to_string(format));
            }
        }
    }
}
