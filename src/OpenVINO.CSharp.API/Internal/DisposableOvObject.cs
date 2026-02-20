// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

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
