// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using System.Collections.Generic;

namespace OpenVinoSharp.Tests
{
    /// <summary>
    /// 测试数据生成器 / Test data generator
    /// </summary>
    public static class TestDataGenerator
    {
        /// <summary>
        /// 生成随机浮点数组
        /// Generate random float array
        /// </summary>
        public static float[] GenerateRandomFloatArray(int length, float min = -1.0f, float max = 1.0f)
        {
            var random = new Random(42); // 固定种子以获得可重复的结果
            float[] data = new float[length];
            for (int i = 0; i < length; i++)
            {
                data[i] = (float)(random.NextDouble() * (max - min) + min);
            }
            return data;
        }

        /// <summary>
        /// 生成顺序浮点数组
        /// Generate sequential float array
        /// </summary>
        public static float[] GenerateSequentialFloatArray(int length)
        {
            float[] data = new float[length];
            for (int i = 0; i < length; i++)
            {
                data[i] = i;
            }
            return data;
        }

        /// <summary>
        /// 生成常量浮点数组
        /// Generate constant float array
        /// </summary>
        public static float[] GenerateConstantFloatArray(int length, float value)
        {
            float[] data = new float[length];
            for (int i = 0; i < length; i++)
            {
                data[i] = value;
            }
            return data;
        }

        /// <summary>
        /// 生成测试用的形状数组
        /// Generate test shape arrays
        /// </summary>
        public static IEnumerable<object[]> GetTestShapes()
        {
            yield return new object[] { new long[] { 1, 3, 224, 224 } }; // NCHW 图像
            yield return new object[] { new long[] { 1, 224, 224, 3 } }; // NHWC 图像
            yield return new object[] { new long[] { 1, 512 } };         // 特征向量
            yield return new object[] { new long[] { 2, 3, 4, 5 } };     // 4D 张量
        }

        /// <summary>
        /// 生成测试用的元素类型
        /// Generate test element types
        /// </summary>
        public static IEnumerable<object[]> GetTestElementTypes()
        {
            yield return new object[] { ElementType.F32 };
            yield return new object[] { ElementType.F16 };
            yield return new object[] { ElementType.I32 };
            yield return new object[] { ElementType.I64 };
        }
    }
}
