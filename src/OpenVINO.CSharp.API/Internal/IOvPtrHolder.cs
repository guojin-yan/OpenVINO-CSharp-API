// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;

namespace OpenVinoSharp.Internal
{
    /// <summary>
    /// OpenVINO原生指针持有者接口 / OpenVINO native pointer holder interface
    /// <para>表示具有原生指针的OpenVINO类。/ Represents an OpenVINO based class which has a native pointer.</para>
    /// </summary>
    public interface IOvPtrHolder
    {
        /// <summary>
        /// 非托管OpenVINO数据指针 / Unmanaged OpenVINO data pointer
        /// </summary>
        IntPtr OvPtr { get; }
    }
}
