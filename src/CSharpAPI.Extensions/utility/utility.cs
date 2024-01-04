using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.Extensions.utility
{
    public static partial class Utility
    {
        /// <summary>
        /// Obtain the original position of the arranged array.
        /// </summary>
        /// <param name="array">The original array.</param>
        /// <returns>The position after arrangement.</returns>
        public static List<int> argsort(List<float> array)
        {
            int array_len = array.Count;

            //生成值和索引的列表
            List<float[]> new_array = new List<float[]> { };
            for (int i = 0; i < array_len; i++)
            {
                new_array.Add(new float[] { array[i], i });
            }
            //对列表按照值大到小进行排序
            new_array.Sort((a, b) => b[0].CompareTo(a[0]));
            //获取排序后的原索引
            List<int> array_index = new List<int>();
            foreach (float[] item in new_array)
            {
                array_index.Add((int)item[1]);
            }
            return array_index;
        }
        /// <summary>
        /// Obtain the original position of the arranged array.
        /// </summary>
        /// <param name="array">The original array.</param>
        /// <returns>The position after arrangement.</returns>
        public static List<int> argsort(float[] array) 
        {
            return argsort(new List<float>(array));
        }
    }
}
