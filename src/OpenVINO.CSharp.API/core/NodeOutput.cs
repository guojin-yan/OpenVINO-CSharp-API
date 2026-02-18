// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using static OpenVinoSharp.native.NativeMethods;
using OpenVinoSharp.Internal;

namespace OpenVinoSharp
{
    /// <summary>
    /// 节点输出端口类 / Node output port class
    /// <para>表示模型/节点的输出端口。/ Represents an output port of a model/node.</para>
    /// </summary>
    public class NodeOutput : DisposableOvObject
    {
        #region 构造函数 / Constructors

        /// <summary>
        /// 从原生指针构造 / Construct from native pointer
        /// </summary>
        /// <param name="ptr">原生节点输出指针 / Native node output pointer</param>
        public NodeOutput(IntPtr ptr) : base(ptr) { }

        #endregion

        #region 资源释放 / Resource Disposal

        /// <inheritdoc/>
        protected override void DisposeUnmanaged()
        {
            // 节点输出由父模型/节点管理 / Node outputs are managed by parent model/node
            // 仅在我们拥有指针时才释放 / Only free if we own the pointer
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
            ExceptionHandler.ThrowOnError(ov_output_get_element_type(_ptr, ref type));
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
                ExceptionHandler.ThrowOnError(ov_output_get_shape(_ptr, shape_ptr));
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
            Shape shape = get_shape();
            return new PartialShape(shape.get_dims());
        }

        /// <summary>
        /// 获取端口的名称 / Get the name of this port
        /// </summary>
        /// <returns>端口名称 / Port name</returns>
        public string get_any_name()
        {
            ThrowIfDisposed();
            IntPtr name_ptr = IntPtr.Zero;
            ExceptionHandler.ThrowOnError(ov_port_get_any_name(_ptr, ref name_ptr));
            string name = Marshal.PtrToStringAnsi(name_ptr) ?? string.Empty;
            ov_free(name_ptr);
            return name;
        }

        /// <summary>
        /// 获取端口的索引 / Get the index of this port
        /// </summary>
        /// <returns>端口索引 / Port index</returns>
        public ulong get_index()
        {
            ThrowIfDisposed();
            ulong idx = 0;
            ExceptionHandler.ThrowOnError(ov_output_get_index(_ptr, ref idx));
            return idx;
        }

        #endregion

        /// <summary>
        /// 获取原生指针（兼容属性）/ Get native pointer (compatibility property)
        /// </summary>
        public IntPtr Ptr => OvPtr;
    }
}
