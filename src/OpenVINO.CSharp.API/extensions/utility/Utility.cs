// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using System.Collections.Generic;

namespace OpenVinoSharp.extensions.utility
{
    /// <summary>
    /// 通用工具类 / General utility class
    /// </summary>
    public static partial class Utility
    {
        /// <summary>
        /// 获取排序后的原始索引 / Obtain the original position of the arranged array
        /// </summary>
        /// <param name="array">原始数组 / The original array</param>
        /// <returns>排序后的索引列表 / The position after arrangement</returns>
        public static List<int> argsort(List<float> array)
        {
            int array_len = array.Count;

            // 生成值和索引的列表 / Generate value and index list
            List<float[]> new_array = new List<float[]>();
            for (int i = 0; i < array_len; i++)
            {
                new_array.Add(new float[] { array[i], i });
            }
            // 对列表按照值从大到小进行排序 / Sort list by value in descending order
            new_array.Sort((a, b) => b[0].CompareTo(a[0]));
            // 获取排序后的原索引 / Get original indices after sorting
            List<int> array_index = new List<int>();
            foreach (float[] item in new_array)
            {
                array_index.Add((int)item[1]);
            }
            return array_index;
        }

        /// <summary>
        /// 获取排序后的原始索引 / Obtain the original position of the arranged array
        /// </summary>
        /// <param name="array">原始数组 / The original array</param>
        /// <returns>排序后的索引列表 / The position after arrangement</returns>
        public static List<int> argsort(float[] array)
        {
            return argsort(new List<float>(array));
        }
    }
}
