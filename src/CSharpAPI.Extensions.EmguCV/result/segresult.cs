using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.Extensions
{
    public struct SegData : IResultData
    {
        /// <summary>
        /// Identification result class
        /// </summary>
        public int classe;
        /// <summary>
        /// Confidence value
        /// </summary>
        public float score;
        /// <summary>
        /// Prediction box
        /// </summary>
        public Rectangle box;

        public string to_string()
        {
            return "";
        }
    }
    public class SegResult : List<SegData>, IResult
    {
        public string print()
        {
            return "";
        }
    }
}
