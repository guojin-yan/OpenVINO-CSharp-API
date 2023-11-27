using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenVinoSharp.Extensions
{
    public struct DetData : IResultData
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
        /// Prediction box.
        /// </summary>
        public Rectangle box;
        public DetData() { }
        public DetData(int index, string lable, float score, Rectangle box) 
        { 
            this.index = index;
            this.lable = lable;
            this.score = score;
            this.box = box;
        }
        public DetData(int index, float score, Rectangle box) 
            :this(index, null, score, box) 
        { }
        public DetData update_lable(List<string> lables) 
        {
            this.lable = lables[this.index];
            return this;
        }
        public DetData update_lable(string[] lables)
        {
            this.lable = lables[this.index];
            return this;
        }

        public string to_string(string format = "0.00") {
            string msg = "";
            msg += ("index: " + index.ToString() + "\t");
            if(lable!=null)
                msg += ("lable: " + lable.ToString() + "\t");
            msg += ("score: " + score.ToString(format) + "\t");
            msg += ("box: " + box.ToString() + "\t");
            return msg;
        }
    };
    public class DetResult : Result<DetData>
    {


        public void add(int index, float score, Rectangle box)
        {
            DetData data = new DetData(index, score, box);
            this.add(data);
        }
        public void add(int index, string lable, float score, Rectangle box)
        {
            DetData data = new DetData(index, lable, score, box);
            this.add(data);
        }

        public void sort_by_index(bool flag) 
        {
            if (flag)
                this.Sort((x, y) => x.index.CompareTo(y.index));
            else
                this.Sort((x, y) => y.index.CompareTo(y.index));
        }
        public string print([StringSyntax("NumericFormat")] string format = "0.00") {
            return "";
        }
    }
}
