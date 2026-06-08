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

using System;

namespace OpenVinoSharp.Internal
{
    /// <summary>
    /// OpenVINO 可释放对象基类 / OpenVINO disposable object base class
    /// <para>继承自 DisposableObject，实现 IOvPtrHolder 接口，是所有 OpenVINO 包装对象的基类。/ Inherits from DisposableObject, implements IOvPtrHolder interface, base class for all OpenVINO wrapper objects.</para>
    /// <para>管理原生 OpenVINO 对象的指针生命周期。/ Manages the lifecycle of native OpenVINO object pointers.</para>
    /// </summary>
    /// <example>
    /// 使用示例 / Usage example:
    /// <code>
    /// public class OvTensor : DisposableOvObject
    /// {
    ///     public OvTensor(IntPtr ptr) : base(ptr) { }
    ///     
    ///     protected override void DisposeUnmanaged()
    ///     {
    ///         if (IsValid)
    ///         {
    ///             ov_tensor_free(_ptr);
    ///             _ptr = IntPtr.Zero;
    ///         }
    ///         base.DisposeUnmanaged();
    ///     }
    /// }
    /// </code>
    /// </example>
    public abstract class DisposableOvObject : DisposableObject, IOvPtrHolder
    {
        /// <summary>
        /// 原生 OpenVINO 对象指针 / Native OpenVINO object pointer
        /// <para>此指针引用原生 C++ OpenVINO 对象。/ This pointer references the native C++ OpenVINO object.</para>
        /// </summary>
        protected IntPtr _ptr;

        /// <summary>
        /// 默认构造函数 / Default constructor
        /// <para>创建指针为空的实例。/ Creates instance with null pointer.</para>
        /// </summary>
        /// <remarks>
        /// 使用此构造函数创建的实例需要后续设置指针。/ Instances created with this constructor require pointer to be set later.
        /// </remarks>
        protected DisposableOvObject()
            : this(true)
        {
        }

        /// <summary>
        /// 带指针的构造函数 / Constructor with pointer
        /// </summary>
        /// <param name="ptr">原生 OpenVINO 对象指针。/ Native OpenVINO object pointer.</param>
        /// <remarks>
        /// 使用此方法包装已有的原生对象指针。/ Use this to wrap an existing native object pointer.
        /// </remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// IntPtr nativeTensor = ov_tensor_create(...);
        /// var tensor = new OvTensor(nativeTensor);
        /// </code>
        /// </example>
        protected DisposableOvObject(IntPtr ptr)
            : this(ptr, true)
        {
        }

        /// <summary>
        /// 构造函数 / Constructor
        /// </summary>
        /// <param name="isEnabledDispose">如果允许GC释放此类则为 true。/ true if you permit disposing this class by GC.</param>
        protected DisposableOvObject(bool isEnabledDispose)
            : this(IntPtr.Zero, isEnabledDispose)
        {
        }

        /// <summary>
        /// 带指针和释放标志的构造函数 / Constructor with pointer and dispose flag
        /// </summary>
        /// <param name="ptr">原生 OpenVINO 对象指针。/ Native OpenVINO object pointer.</param>
        /// <param name="isEnabledDispose">如果允许GC释放此类则为 true。/ true if you permit disposing this class by GC.</param>
        /// <remarks>
        /// 这是完整的构造函数，其他构造函数都委托给此构造函数。/ This is the full constructor, other constructors delegate to this one.
        /// </remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// // 创建不由GC管理的包装对象 / Create wrapper not managed by GC
        /// IntPtr nativeObj = CreateNativeObject();
        /// var obj = new MyOvObject(nativeObj, false);
        /// </code>
        /// </example>
        protected DisposableOvObject(IntPtr ptr, bool isEnabledDispose)
            : base(isEnabledDispose)
        {
            this._ptr = ptr;
        }

        /// <summary>
        /// 释放非托管资源 / Releases unmanaged resources
        /// <para>将指针置为空，基类会释放其他资源。/ Sets pointer to null, base class releases other resources.</para>
        /// </summary>
        /// <remarks>
        /// 子类应重写此方法以释放特定的 OpenVINO 对象。/ Subclasses should override this method to release specific OpenVINO objects.
        /// <para>注意：在调用 base.DisposeUnmanaged() 之前重置 _ptr，以防止重复释放。/ Note: Reset _ptr before calling base.DisposeUnmanaged() to prevent double-free.</para>
        /// </remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// protected override void DisposeUnmanaged()
        /// {
        ///     if (_ptr != IntPtr.Zero)
        ///     {
        ///         ov_core_free(_ptr); // 释放原生对象 / Free native object
        ///         _ptr = IntPtr.Zero; // 置空指针 / Nullify pointer
        ///     }
        ///     base.DisposeUnmanaged();
        /// }
        /// </code>
        /// </example>
        protected override void DisposeUnmanaged()
        {
            _ptr = IntPtr.Zero;
            base.DisposeUnmanaged();
        }

        /// <summary>
        /// OpenVINO 结构的指针 / Native pointer of OpenVINO structure
        /// <para>如果对象已被释放，访问此属性会抛出 ObjectDisposedException。/ Throws ObjectDisposedException if object has been disposed.</para>
        /// </summary>
        /// <value>原生 OpenVINO 对象指针。/ Native OpenVINO object pointer.</value>
        /// <exception cref="ObjectDisposedException">当对象已被释放时抛出。/ Thrown when object has been disposed.</exception>
        /// <remarks>
        /// 使用此属性将托管对象传递给原生 OpenVINO API。/ Use this property to pass managed objects to native OpenVINO API.
        /// </remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// using (var tensor = new OvTensor(ptr))
        /// {
        ///     IntPtr nativePtr = tensor.OvPtr;
        ///     ov_tensor_get_shape(nativePtr, out shape);
        /// }
        /// </code>
        /// </example>
        public IntPtr OvPtr
        {
            get
            {
                ThrowIfDisposed();
                return _ptr;
            }
        }

        /// <summary>
        /// 检查指针是否有效（不为空）/ Check if the pointer is valid (not null)
        /// </summary>
        /// <value>如果指针不为空则为 true，否则为 false。/ true if pointer is not null; otherwise, false.</value>
        /// <remarks>
        /// 在访问原生对象之前应检查此属性。/ Check this property before accessing native objects.
        /// </remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// if (obj.IsValid)
        /// {
        ///     // 安全地访问原生对象 / Safely access native object
        /// }
        /// </code>
        /// </example>
        public bool IsValid => _ptr != IntPtr.Zero;

        /// <summary>
        /// 显式释放方法，作为 Dispose 的别名 / Release method as explicit alias for Dispose
        /// <para>与原生 OpenVINO API 的命名风格保持一致。/ Consistent with native OpenVINO API naming style.</para>
        /// </summary>
        /// <remarks>
        /// 此方法与调用 Dispose() 完全等效。/ This method is completely equivalent to calling Dispose().
        /// </remarks>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// var obj = new OvTensor(ptr);
        /// try
        /// {
        ///     // 使用对象 / Use object
        /// }
        /// finally
        /// {
        ///     obj.Release(); // 与 Dispose() 相同 / Same as Dispose()
        /// }
        /// </code>
        /// </example>
        public void Release()
        {
            Dispose();
        }
    }
}
