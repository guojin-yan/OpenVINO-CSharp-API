// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

namespace OpenVinoSharp.preprocess
{
    /// <summary>
    /// Color format enumeration
    /// </summary>
    public enum ColorFormat : uint
    {
        UNDEFINE = 0U,
        NV12_SINGLE_PLANE,
        NV12_TWO_PLANES,
        I420_SINGLE_PLANE,
        I420_THREE_PLANES,
        RGB,
        BGR,
        GRAY,
        RGBX,
        BGRX
    }

    /// <summary>
    /// Resize algorithm enumeration
    /// </summary>
    public enum ResizeAlgorithm : uint
    {
        RESIZE_LINEAR,
        RESIZE_CUBIC,
        RESIZE_NEAREST
    }

    /// <summary>
    /// Padding mode enumeration
    /// </summary>
    public enum PaddingMode : uint
    {
        CONSTANT = 0,
        EDGE,
        REFLECT,
        SYMMETRIC
    }
}
