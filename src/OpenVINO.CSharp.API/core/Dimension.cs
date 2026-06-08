//  ========================================================================
//  【项目名称】OpenVINO C# API
//  【项目描述】OpenVINO™ 的 C# 语言绑定库，提供高性能深度学习推理能力
//  【版权声明】© 2026-2025 Guojin Yan. All Rights Reserved.
//  【开源协议】Apache-2.0 License（请遵守许可证条款）
//  -----------------------------------------------------------------------
//  【功能简介】
//  1. 完整的 OpenVINO™ C API 封装，提供 C# 友好的面向对象接口。
//  2. 支持模型加载、编译、推理全流程操作。
//  3. 支持 CPU、GPU、VPU 等多种推理设备。
//  4. 支持同步推理和异步推理模式。
//  5. 支持预处理和后处理流水线配置。
//  6. 支持动态形状和批量推理。
//  7. 支持模型缓存和性能分析。
//  8. 支持远程上下文（Remote Context）和零拷贝推理。
//  9. 支持 .NET Framework 4.6.1+、.NET Core 2.0+、.NET 5/6/7/8/9+。
//  10. 提供推理请求对象池，优化高并发场景性能。
//  11. 提供完善的异常处理和日志记录机制。
//  12. 提供丰富的单元测试和集成测试用例。
//  -----------------------------------------------------------------------
//  【官方资源】
//  📌 GitHub仓库：https://github.com/guojin-yan/OpenVINO-CSharp-API
//  📌 NuGet包：https://www.nuget.org/packages/OpenVINO.CSharp.API
//  📌 在线文档：https://guojin-yan.github.io/OpenVINO-CSharp-API/index.html
//  📌 示例代码：https://github.com/guojin-yan/OpenVINO-CSharp-API/tree/csharp3.3/samples
//  -----------------------------------------------------------------------
//  【社区支持】
//  💬 QQ交流群：945057948（加入获取技术支持）
//  📱 微信公众号：CSharp与边缘模型部署（教程+案例）
//  📝 CSDN博客：https://guojin.blog.csdn.net（技术文章）
//  -----------------------------------------------------------------------
//  【联系我们】
//  ✉ 项目维护：guojin_yjs@cumt.edu.cn
//  💬 微信咨询：15253793309
//  🐛 Bug反馈：https://github.com/guojin-yan/OpenVINO-CSharp-API/issues
//  💡 功能建议：https://github.com/guojin-yan/OpenVINO-CSharp-API/discussions/landing
//  -----------------------------------------------------------------------
//  【致谢】
//  本项目基于 Intel® OpenVINO™ 工具包开发，感谢 Intel 提供的优秀开源项目。
//  OpenVINO™ 是 Intel Corporation 的商标。
//  ========================================================================
//  
//  【许可声明】
//  1. 本项目采用 Apache-2.0 License 开源协议，允许自由使用、修改和分发。
//  2. 使用本项目即表示您同意 Apache-2.0 License 许可证的所有条款。
//  3. 本项目按"原样"提供，不提供任何形式的担保。
//  4. 使用本项目产生的任何风险由使用者自行承担。
//  5. 修改或分发时请保留原始版权声明和许可声明。
//  ========================================================================
//

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
