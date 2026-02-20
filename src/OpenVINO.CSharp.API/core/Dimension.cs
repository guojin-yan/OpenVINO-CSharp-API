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
    /// <example>
    /// 使用示例 / Usage example:
    /// <code>
    /// // 创建静态维度 / Create static dimension
    /// Dimension dim1 = new Dimension(224);
    /// 
    /// // 创建动态维度 / Create dynamic dimension
    /// Dimension dim2 = Dimension.dynamic();
    /// 
    /// // 创建有界维度 / Create bounded dimension
    /// Dimension dim3 = Dimension.bounded(1, 10);
    /// 
    /// // 检查维度类型 / Check dimension type
    /// bool isStatic = dim1.is_static(); // true
    /// bool isDynamic = dim2.is_dynamic(); // true
    /// </code>
    /// </example>
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
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// Dimension dim = new Dimension(224);
        /// Console.WriteLine(dim); // 输出 / Output: 224
        /// </code>
        /// </example>
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
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// // 创建表示批次大小为1到8的动态维度 / Create dynamic dimension for batch size 1-8
        /// Dimension batchDim = new Dimension(1, 8);
        /// Console.WriteLine(batchDim); // 输出 / Output: 1..8
        /// </code>
        /// </example>
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
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// Dimension staticDim = new Dimension(224);
        /// Dimension dynamicDim = Dimension.dynamic();
        /// bool isStaticDynamic = staticDim.is_dynamic(); // false
        /// bool isDynamicDynamic = dynamicDim.is_dynamic(); // true
        /// </code>
        /// </example>
        public bool is_dynamic()
        {
            return min != max || min == -1 || max == -1;
        }

        /// <summary>
        /// 检查此维度是否为静态 / Check if this dimension is static
        /// </summary>
        /// <returns>是否为静态 / Whether static</returns>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// Dimension staticDim = new Dimension(224);
        /// Dimension dynamicDim = Dimension.dynamic();
        /// bool isStatic = staticDim.is_static(); // true
        /// bool isDynamicStatic = dynamicDim.is_static(); // false
        /// </code>
        /// </example>
        public bool is_static()
        {
            return min == max && min != -1 && max != -1;
        }

        /// <summary>
        /// 获取静态维度值（如果是静态的）/ Get static dimension value (if static)
        /// </summary>
        /// <returns>维度值 / Dimension value</returns>
        /// <exception cref="InvalidOperationException">当维度为动态时抛出 / Thrown when dimension is dynamic</exception>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// Dimension staticDim = new Dimension(224);
        /// long length = staticDim.get_length(); // 224
        /// </code>
        /// </example>
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
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// Dimension dynamicDim = Dimension.dynamic();
        /// Console.WriteLine(dynamicDim); // 输出 / Output: ?
        /// </code>
        /// </example>
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
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// // 创建批次维度，支持1到16的动态批次 / Create batch dimension with dynamic range 1-16
        /// Dimension batch = Dimension.bounded(1, 16);
        /// Console.WriteLine(batch); // 输出 / Output: 1..16
        /// </code>
        /// </example>
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
        /// <param name="left">左操作数 / Left operand</param>
        /// <param name="right">右操作数 / Right operand</param>
        /// <returns>是否相等 / Whether equal</returns>
        public static bool operator ==(Dimension left, Dimension right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// 不等运算符 / Inequality operator
        /// </summary>
        /// <param name="left">左操作数 / Left operand</param>
        /// <param name="right">右操作数 / Right operand</param>
        /// <returns>是否不等 / Whether not equal</returns>
        public static bool operator !=(Dimension left, Dimension right)
        {
            return !left.Equals(right);
        }

        #endregion

        #region 对象方法 / Object Methods

        /// <inheritdoc/>
        /// <returns>字符串表示 / String representation</returns>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// Console.WriteLine(new Dimension(224));      // "224"
        /// Console.WriteLine(Dimension.dynamic());     // "?"
        /// Console.WriteLine(Dimension.bounded(1, 8)); // "1..8"
        /// </code>
        /// </example>
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
