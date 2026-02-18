// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Text;
using OpenVinoSharp.Internal;

namespace OpenVinoSharp
{
    /// <summary>
    /// 部分形状类 / Partial shape class
    /// <para>表示可能部分或完全动态的形状。/ Represents a shape that may be partially or totally dynamic.</para>
    /// </summary>
    public class PartialShape : DisposableOvObject
    {
        #region 字段 / Fields

        /// <summary>
        /// 秩（维度数量）/ Rank (number of dimensions)
        /// </summary>
        private Rank _rank;

        /// <summary>
        /// 维度数组 / Dimension array
        /// </summary>
        private Dimension[] _dims = new Dimension[0];

        #endregion

        #region 构造函数 / Constructors

        /// <summary>
        /// 默认构造函数 - 创建动态形状 / Default constructor - creates dynamic shape
        /// </summary>
        public PartialShape() : base()
        {
            _rank = Rank.dynamic();
        }

        /// <summary>
        /// 从原生指针构造 / Construct from native pointer
        /// </summary>
        /// <param name="ptr">原生部分形状指针 / Native partial shape pointer</param>
        public PartialShape(IntPtr ptr) : base(ptr) { }

        /// <summary>
        /// 从秩和维度构造 / Construct from rank and dimensions
        /// </summary>
        /// <param name="rank">秩 / Rank</param>
        /// <param name="dims">维度数组 / Dimension array</param>
        public PartialShape(Rank rank, Dimension[] dims) : base()
        {
            _rank = rank;
            _dims = dims ?? new Dimension[0];
        }

        /// <summary>
        /// 从静态维度数组构造 / Construct from static dimension array
        /// </summary>
        /// <param name="dims">维度数组 / Dimension array</param>
        public PartialShape(long[] dims) : base()
        {
            _rank = new Rank(dims.Length);
            _dims = new Dimension[dims.Length];
            for (int i = 0; i < dims.Length; i++)
                _dims[i] = new Dimension(dims[i]);
        }

        #endregion

        #region 属性 / Properties

        /// <summary>
        /// 原生结构指针 / Native struct pointer
        /// </summary>
        public IntPtr NativePtr => OvPtr;

        /// <summary>
        /// 获取秩 / Get the rank
        /// </summary>
        public Rank rank => _rank;

        /// <summary>
        /// 获取维度 / Get dimensions
        /// </summary>
        public Dimension[] dims => _dims;

        #endregion

        #region 动态性检查 / Dynamic Checks

        /// <summary>
        /// 检查形状是否为动态 / Check if shape is dynamic
        /// </summary>
        /// <returns>是否为动态 / Whether dynamic</returns>
        public bool is_dynamic()
        {
            if (_rank.is_dynamic()) return true;
            foreach (var dim in _dims)
                if (dim.is_dynamic()) return true;
            return false;
        }

        /// <summary>
        /// 检查形状是否为静态 / Check if shape is static
        /// </summary>
        /// <returns>是否为静态 / Whether static</returns>
        public bool is_static()
        {
            return !is_dynamic();
        }

        /// <summary>
        /// 检查秩是否为动态 / Check if rank is dynamic
        /// </summary>
        /// <returns>秩是否为动态 / Whether rank is dynamic</returns>
        public bool rank_is_dynamic()
        {
            return _rank.is_dynamic();
        }

        /// <summary>
        /// 转换为静态形状（如果可能）/ Convert to static shape (if possible)
        /// </summary>
        /// <returns>静态形状 / Static shape</returns>
        public Shape to_shape()
        {
            if (!is_static())
                throw new InvalidOperationException("无法将动态部分形状转换为静态形状 / Cannot convert dynamic partial shape to static shape");
            
            long[] shape_dims = new long[_dims.Length];
            for (int i = 0; i < _dims.Length; i++)
                shape_dims[i] = _dims[i].get_length();
            
            return new Shape(shape_dims);
        }

        #endregion

        #region 工厂方法 / Factory Methods

        /// <summary>
        /// 创建静态部分形状 / Create static partial shape
        /// </summary>
        /// <param name="dims">维度数组 / Dimension array</param>
        /// <returns>部分形状 / Partial shape</returns>
        public static PartialShape static_shape(long[] dims)
        {
            return new PartialShape(dims);
        }

        /// <summary>
        /// 创建任意秩的动态部分形状 / Create dynamic partial shape with any rank
        /// </summary>
        /// <returns>动态部分形状 / Dynamic partial shape</returns>
        public static PartialShape dynamic_shape()
        {
            return new PartialShape { _rank = Rank.dynamic(), _dims = new Dimension[0] };
        }

        /// <summary>
        /// 创建固定秩但动态维度的部分形状 / Create partial shape with static rank but dynamic dimensions
        /// </summary>
        /// <param name="rank">维度数量 / Number of dimensions</param>
        /// <returns>部分形状 / Partial shape</returns>
        public static PartialShape dynamic_shape_with_rank(int rank)
        {
            Dimension[] dims = new Dimension[rank];
            for (int i = 0; i < rank; i++)
                dims[i] = Dimension.dynamic();
            return new PartialShape { _rank = new Rank(rank), _dims = dims };
        }

        /// <summary>
        /// 创建标量部分形状 / Create scalar partial shape
        /// </summary>
        /// <returns>标量部分形状 / Scalar partial shape</returns>
        public static PartialShape scalar()
        {
            return new PartialShape(new long[0]);
        }

        /// <summary>
        /// 创建无限秩的动态形状 / Create dynamic shape with infinite rank
        /// </summary>
        /// <returns>无限秩动态形状 / Dynamic shape with infinite rank</returns>
        public static PartialShape infinite_rank_dynamic()
        {
            return new PartialShape { _rank = Rank.dynamic(), _dims = new Dimension[0] };
        }

        #endregion

        #region 对象方法 / Object Methods

        /// <inheritdoc/>
        public override string ToString()
        {
            if (_rank.is_dynamic())
                return "?";
            
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            for (int i = 0; i < _dims.Length; i++)
            {
                if (i > 0) sb.Append(",");
                sb.Append(_dims[i].ToString());
            }
            sb.Append("}");
            return sb.ToString();
        }

        /// <inheritdoc/>
        protected override void DisposeUnmanaged()
        {
            // 清理原生资源（如果已分配）/ Clean up native resources if allocated
            base.DisposeUnmanaged();
        }

        #endregion
    }
}
