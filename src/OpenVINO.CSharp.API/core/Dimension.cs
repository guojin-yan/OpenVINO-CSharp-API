// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using OpenVinoSharp.native;
using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp
{
    /// <summary>
    /// 维度结构 / Dimension structure
    /// <para>表示具有最小/最大边界的维度。/ Represents a dimension with min/max bounds.</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Dimension : IEquatable<Dimension>
    {
        #region 字段 / Fields

        /// <summary>
        /// 维度的下界（包含）/ The lower inclusive limit for the dimension
        /// </summary>
        public long min;

        /// <summary>
        /// 维度的上界（包含）/ The upper inclusive limit for the dimension
        /// </summary>
        public long max;

        #endregion

        #region 构造函数 / Constructors

        /// <summary>
        /// 构造静态维度 / Construct a static dimension
        /// </summary>
        /// <param name="value">维度值 / Dimension value</param>
        public Dimension(long value)
        {
            min = value;
            max = value;
        }

        /// <summary>
        /// 构造具有最小/最大边界的维度 / Construct a dimension with min/max bounds
        /// </summary>
        /// <param name="minVal">最小值 / Minimum value</param>
        /// <param name="maxVal">最大值 / Maximum value</param>
        public Dimension(long minVal, long maxVal)
        {
            min = minVal;
            max = maxVal;
        }

        #endregion

        #region 动态性检查 / Dynamic Checks

        /// <summary>
        /// 检查此维度是否为动态 / Check if this dimension is dynamic
        /// </summary>
        /// <returns>是否为动态 / Whether dynamic</returns>
        public bool is_dynamic()
        {
            return min != max || min == -1 || max == -1;
        }

        /// <summary>
        /// 检查此维度是否为静态 / Check if this dimension is static
        /// </summary>
        /// <returns>是否为静态 / Whether static</returns>
        public bool is_static()
        {
            return min == max && min != -1 && max != -1;
        }

        /// <summary>
        /// 获取静态维度值（如果是静态的）/ Get static dimension value (if static)
        /// </summary>
        /// <returns>维度值 / Dimension value</returns>
        public long get_length()
        {
            if (is_dynamic())
                throw new InvalidOperationException("无法获取动态维度的长度 / Cannot get length of dynamic dimension");
            return min;
        }

        #endregion

        #region 工厂方法 / Factory Methods

        /// <summary>
        /// 创建任意值的动态维度 / Create a dynamic dimension with any value
        /// </summary>
        /// <returns>动态维度 / Dynamic dimension</returns>
        public static Dimension dynamic()
        {
            return new Dimension(-1, -1);
        }

        /// <summary>
        /// 创建有界范围的动态维度 / Create a dynamic dimension with bounded range
        /// </summary>
        /// <param name="min">最小值 / Minimum value</param>
        /// <param name="max">最大值 / Maximum value</param>
        /// <returns>有界维度 / Bounded dimension</returns>
        public static Dimension bounded(long min, long max)
        {
            return new Dimension(min, max);
        }

        #endregion

        #region 相等性比较 / Equality Comparison

        /// <inheritdoc/>
        public bool Equals(Dimension other)
        {
            return min == other.min && max == other.max;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is Dimension other && Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + min.GetHashCode();
                hash = hash * 31 + max.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// 相等运算符 / Equality operator
        /// </summary>
        public static bool operator ==(Dimension left, Dimension right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// 不等运算符 / Inequality operator
        /// </summary>
        public static bool operator !=(Dimension left, Dimension right)
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
            if (min == -1 && max == -1)
                return "?";
            return $"{min}..{max}";
        }

        #endregion

        #region 转换方法 / Conversion Methods

        /// <summary>
        /// 转换为 ov_dimension_t 结构体 / Convert to ov_dimension_t structure
        /// </summary>
        /// <returns>ov_dimension_t 结构体 / ov_dimension_t structure</returns>
        internal ov_dimension_t ToNativeStruct()
        {
            return new ov_dimension_t { min = this.min, max = this.max };
        }

        #endregion
    }
}
