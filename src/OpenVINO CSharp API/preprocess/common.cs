using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.preprocess
{
    /// <summary>
    /// This enum contains enumerations for color format.
    /// </summary>
    public enum ColorFormat : uint
    {
        /// <summary>
        /// Undefine color format
        /// </summary>
        UNDEFINE = 0U,
        /// <summary>
        /// Image in NV12 format as single tensor
        /// </summary>
        NV12_SINGLE_PLANE,
        /// <summary>
        /// Image in NV12 format represented as separate tensors for Y and UV planes.
        /// </summary>
        NV12_TWO_PLANES,
        /// <summary>
        /// Image in I420 (YUV) format as single tensor
        /// </summary>
        I420_SINGLE_PLANE,
        /// <summary>
        /// Image in I420 format represented as separate tensors for Y, U and V planes.
        /// </summary>
        I420_THREE_PLANES,
        /// <summary>
        /// Image in RGB interleaved format (3 channels)
        /// </summary>
        RGB,
        /// <summary>
        /// Image in BGR interleaved format (3 channels)
        /// </summary>
        BGR,
        /// <summary>
        /// Image in GRAY format (1 channel)
        /// </summary>
        GRAY,
        /// <summary>
        /// Image in RGBX interleaved format (4 channels)
        /// </summary>
        RGBX,
        /// <summary>
        /// Image in BGRX interleaved format (4 channels)
        /// </summary>
        BGRX
    };
    /// <summary>
    /// This enum contains codes for all preprocess resize algorithm.
    /// </summary>
    public enum ResizeAlgorithm
    {
        /// <summary>
        /// linear algorithm
        /// </summary>
        RESIZE_LINEAR,
        /// <summary>
        /// cubic algorithm
        /// </summary>
        RESIZE_CUBIC,
        /// <summary>
        ///  nearest algorithm
        /// </summary>
        RESIZE_NEAREST
    };
}
