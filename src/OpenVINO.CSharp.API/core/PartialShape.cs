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
//  📌 示例代码：https://github.com/guojin-yan/OpenVINO-CSharp-API/tree/csharp3.2/samples
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

using System;
using System.Runtime.InteropServices;
using System.Text;
using OpenVinoSharp.Internal;
using OpenVinoSharp.native;

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

        #region 转换方法 / Conversion Methods

        /// <summary>
        /// 转换为 ov_partial_shape_t 结构体 / Convert to ov_partial_shape_t structure
        /// </summary>
        /// <returns>ov_partial_shape_t 结构体 / ov_partial_shape_t structure</returns>
        internal ov_partial_shape_t ToNativeStruct()
        {
            int rank = _dims.Length;
            
            // 如果秩是动态的 / If rank is dynamic
            if (_rank.is_dynamic())
            {
                return new ov_partial_shape_t
                {
                    rank = ov_rank_t.Dynamic,
                    dims = IntPtr.Zero
                };
            }

            // 分配维度数组内存 / Allocate dimension array memory
            ov_dimension_t[] dimensions = new ov_dimension_t[rank];
            for (int i = 0; i < rank; i++)
            {
                dimensions[i] = _dims[i].ToNativeStruct();
            }

            IntPtr dimsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ov_dimension_t)) * rank);
            for (int i = 0; i < rank; i++)
            {
                IntPtr offset = new IntPtr(dimsPtr.ToInt64() + Marshal.SizeOf(typeof(ov_dimension_t)) * i);
                Marshal.StructureToPtr(dimensions[i], offset, false);
            }

            return new ov_partial_shape_t
            {
                rank = ov_rank_t.Static(rank),
                dims = dimsPtr
            };
        }

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
