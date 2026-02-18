// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;

namespace OpenVinoSharp.Internal
{
    /// <summary>
    /// DisposableObject + IOvPtrHolder
    /// Base class for all OpenVINO wrapper objects.
    /// </summary>
    public abstract class DisposableOvObject : DisposableObject, IOvPtrHolder
    {
        /// <summary>
        /// Data pointer
        /// </summary>
        protected IntPtr _ptr;

        /// <summary>
        /// Default constructor
        /// </summary>
        protected DisposableOvObject()
            : this(true)
        {
        }

        /// <summary>
        /// Constructor with pointer
        /// </summary>
        /// <param name="ptr"></param>
        protected DisposableOvObject(IntPtr ptr)
            : this(ptr, true)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="isEnabledDispose"></param>
        protected DisposableOvObject(bool isEnabledDispose)
            : this(IntPtr.Zero, isEnabledDispose)
        {
        }

        /// <summary>
        /// Constructor with pointer and dispose flag
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="isEnabledDispose"></param>
        protected DisposableOvObject(IntPtr ptr, bool isEnabledDispose)
            : base(isEnabledDispose)
        {
            this._ptr = ptr;
        }

        /// <summary>
        /// releases unmanaged resources
        /// </summary>
        protected override void DisposeUnmanaged()
        {
            _ptr = IntPtr.Zero;
            base.DisposeUnmanaged();
        }

        /// <summary>
        /// Native pointer of OpenVINO structure
        /// </summary>
        public IntPtr OvPtr
        {
            get
            {
                ThrowIfDisposed();
                return _ptr;
            }
        }

        /// <summary>
        /// Check if the pointer is valid (not null)
        /// </summary>
        public bool IsValid => _ptr != IntPtr.Zero;

        /// <summary>
        /// Release method as explicit alias for Dispose
        /// </summary>
        public void Release()
        {
            Dispose();
        }
    }
}
