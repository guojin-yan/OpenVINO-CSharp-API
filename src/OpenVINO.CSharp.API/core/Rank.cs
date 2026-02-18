// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp
{
    /// <summary>
    /// 秩结构 / Rank structure
    /// <para>表示形状的维度数量。/ Represents the number of dimensions in a shape.</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Rank : IEquatable<Rank>
    {
        #region 字段 / Fields

        /// <summary>
        /// 秩的下界（包含）/ The lower inclusive limit for the rank
        /// </summary>
        public long min;

        /// <summary>
        /// 秩的上界（包含）/ The upper inclusive limit for the rank
        /// </summary>
        public long max;

        #endregion

        #region 构造函数 / Constructors

        /// <summary>
        /// 构造静态秩 / Construct a static rank
        /// </summary>
        /// <param name="value">秩值 / Rank value</param>
        public Rank(long value)
        {
            min = value;
            max = value;
        }

        /// <summary>
        /// 构造具有最小/最大边界的秩 / Construct a rank with min/max bounds
        /// </summary>
        /// <param name="minVal">最小值 / Minimum value</param>
        /// <param name="maxVal">最大值 / Maximum value</param>
        public Rank(long minVal, long maxVal)
        {
            min = minVal;
            max = maxVal;
        }

        #endregion

        #region 动态性检查 / Dynamic Checks

        /// <summary>
        /// 检查此秩是否为动态 / Check if this rank is dynamic
        /// </summary>
        /// <returns>是否为动态 / Whether dynamic</returns>
        public bool is_dynamic()
        {
            return min != max;
        }

        /// <summary>
        /// 检查此秩是否为静态 / Check if this rank is static
        /// </summary>
        /// <returns>是否为静态 / Whether static</returns>
        public bool is_static()
        {
            return min == max;
        }

        /// <summary>
        /// 获取静态秩值（如果是静态的）/ Get static rank value (if static)
        /// </summary>
        /// <returns>秩值 / Rank value</returns>
        public long get_length()
        {
            if (is_dynamic())
                throw new InvalidOperationException("无法获取动态秩的长度 / Cannot get length of dynamic rank");
            return min;
        }

        #endregion

        #region 工厂方法 / Factory Methods

        /// <summary>
        /// 创建动态秩（任意）/ Create a dynamic rank (any)
        /// </summary>
        /// <returns>动态秩 / Dynamic rank</returns>
        public static Rank dynamic()
        {
            return new Rank(-1, -1);
        }

        #endregion

        #region 相等性比较 / Equality Comparison

        /// <inheritdoc/>
        public bool Equals(Rank other)
        {
            return min == other.min && max == other.max;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is Rank other && Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + (int)(min ^ (min >> 32));
                hash = hash * 31 + (int)(max ^ (max >> 32));
                return hash;
            }
        }

        /// <summary>
        /// 相等运算符 / Equality operator
        /// </summary>
        public static bool operator ==(Rank left, Rank right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// 不等运算符 / Inequality operator
        /// </summary>
        public static bool operator !=(Rank left, Rank right)
        {
            return !left.Equals(right);
        }

        #endregion

        #region 对象方法 / Object Methods

        /// <inheritdoc/>
        public override string ToString()
        {
            if (is_static())
                return min.ToString();
            return "?";
        }

        #endregion
    }
}
