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

using OpenVinoSharp.Internal;
using OpenVinoSharp.native;
using System;
using System.Runtime.InteropServices;
using static OpenVinoSharp.native.NativeMethods;

namespace OpenVinoSharp
{
    /// <summary>
    /// 节点输出端口类 / Node output port class
    /// <para>表示模型/节点的输出端口。/ Represents an output port of a model/node.</para>
    /// </summary>
    public class Output : DisposableOvObject
    {
        private readonly bool _isConstPort;
        private IntPtr _constPortPtr;

        #region 构造函数 / Constructors

        /// <summary>
        /// 从原生指针构造 / Construct from native pointer
        /// </summary>
        /// <param name="ptr">原生节点输出指针 / Native node output pointer</param>
        public Output(IntPtr ptr) : this(ptr, false) { }

        internal Output(IntPtr ptr, bool isConstPort) : this(ptr, isConstPort, IntPtr.Zero) { }

        internal Output(IntPtr ptr, bool isConstPort, IntPtr constPortPtr) : base(ptr)
        {
            _isConstPort = isConstPort;
            _constPortPtr = constPortPtr;
        }

        #endregion

        #region 资源释放 / Resource Disposal

        /// <inheritdoc/>
        protected override void DisposeUnmanaged()
        {
            if (IsEnabledDispose)
            {
                if (_constPortPtr != IntPtr.Zero && _constPortPtr != _ptr)
                {
                    ov_output_const_port_free(_constPortPtr);
                    _constPortPtr = IntPtr.Zero;
                }

                if (_ptr != IntPtr.Zero)
                {
                    if (_isConstPort)
                        ov_output_const_port_free(_ptr);
                    else
                        ov_output_port_free(_ptr);
                }
            }
            base.DisposeUnmanaged();
        }

        #endregion

        #region 属性查询 / Property Queries

        /// <summary>
        /// 获取端口的元素类型 / Get the element type of this port
        /// </summary>
        /// <returns>OpenVINO类型 / OpenVINO type</returns>
        public OvType get_element_type()
        {
            ThrowIfDisposed();
            uint type = 0;
            ExceptionHandler.ThrowOnError(ov_port_get_element_type(ConstPortPtr, ref type));
            return new OvType((ElementType)type);
        }

        /// <summary>
        /// 获取端口的形状 / Get the shape of this port
        /// </summary>
        /// <returns>形状对象 / Shape object</returns>
        public Shape get_shape()
        {
            ThrowIfDisposed();
            int size = Marshal.SizeOf(typeof(Ov.ov_shape));
            IntPtr shape_ptr = Marshal.AllocHGlobal(size);
            try
            {
                ExceptionHandler.ThrowOnError(_isConstPort
                    ? ov_const_port_get_shape(_ptr, shape_ptr)
                    : ov_port_get_shape(_ptr, shape_ptr));
                return new Shape(shape_ptr);
            }
            catch
            {
                Marshal.FreeHGlobal(shape_ptr);
                throw;
            }
        }

        /// <summary>
        /// 获取端口的部分形状（支持动态维度）/ Get the partial shape of this port (supports dynamic dimensions)
        /// </summary>
        /// <returns>部分形状对象 / Partial shape object</returns>
        public PartialShape get_partial_shape()
        {
            ThrowIfDisposed();
            int size = Marshal.SizeOf(typeof(ov_partial_shape_t));
            IntPtr shape_ptr = Marshal.AllocHGlobal(size);
            try
            {
                ExceptionHandler.ThrowOnError(ov_port_get_partial_shape(ConstPortPtr, shape_ptr));
                return new PartialShape(shape_ptr);
            }
            catch
            {
                Marshal.FreeHGlobal(shape_ptr);
                throw;
            }
        }

        /// <summary>
        /// 获取端口的名称 / Get the name of this port
        /// </summary>
        /// <returns>端口名称 / Port name</returns>
        public string get_any_name()
        {
            ThrowIfDisposed();
            IntPtr name_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_port_get_any_name(ConstPortPtr, ref name_ptr));
            try
            {
                return StringUtils.Utf8PtrToString(name_ptr);
            }
            finally
            {
                if (name_ptr != IntPtr.Zero)
                    ov_free(name_ptr);
            }
        }

        /// <summary>
        /// 获取输出端口任意名称 / Gets any name of this output port.
        /// </summary>
        /// <returns>端口名称 / Port name.</returns>
        public string GetAnyName()
        {
            return get_any_name();
        }


        #endregion

        /// <summary>
        /// 获取原生指针（兼容属性）/ Get native pointer (compatibility property)
        /// </summary>
        public IntPtr Ptr => OvPtr;

        internal IntPtr ConstPortPtr
        {
            get
            {
                ThrowIfDisposed();
                return _isConstPort || _constPortPtr == IntPtr.Zero ? _ptr : _constPortPtr;
            }
        }
    }
}
