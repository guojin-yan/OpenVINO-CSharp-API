using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    /// <summary>
    /// ov::Layout represents the text information of tensor's dimensions/axes. E.g. layout `NCHW` means that 4D
    /// tensor `{-1, 3, 480, 640}` will have: 
    /// - 0: `N = -1`: batch dimension is dynamic 
    /// - 1: `C = 3`: number of channels is '3' 
    /// - 2: `H = 480`: image height is 480 
    /// - 3: `W = 640`: image width is 640 
    /// </summary>
    /// <example>
    /// `ov::Layout` can be specified for:
    /// - Preprocessing purposes. E.g.
    ///    - To apply normalization (means/scales) it is usually required to set 'C' dimension in a layout.
    ///    - To resize the image to specified width/height it is needed to set 'H' and 'W' dimensions in a layout
    ///    - To transpose image - source and target layout can be set (see
    ///    `ov::preprocess::PreProcessSteps::convert_layout`)
    /// - To set/get model's batch (see `ov::get_batch`/`ov::set_batch') it is required in general to specify 'N' dimension
    /// in layout for appropriate inputs
    /// </example>
    public class Layout : IDisposable
    {
        /// <summary>
        /// [private]Layout class pointer.
        /// </summary>
        private IntPtr m_ptr = IntPtr.Zero;
        /// <summary>
        /// [public]Layout class pointer.
        /// </summary>
        public IntPtr Ptr
        { 
            get { return m_ptr; } 
            set { m_ptr = value; }
        }

        /// <summary>
        /// Constructs a Layout with static or dynamic layout information based on string representation.
        /// </summary>
        /// <param name="layout_desc">
        /// The string used to construct Layout from.
        /// The string representation can be in the following form:
        /// - can define order and meaning for dimensions "NCHW"
        /// - partial layout specialization:
        ///   - "NC?" defines 3 dimensional layout, first two NC, 3rd one is not defined
        ///   - "N...C" defines layout with dynamic rank where 1st dimension is N, last one is C
        ///   - "NC..." defines layout with dynamic rank where first two are NC, others are not
        ///   defined
        /// - only order of dimensions "adbc" (0312)
        /// - Advanced syntax can be used for multi-character names like "[N,C,H,W,...,CustomName]"
        /// </param>
        public Layout(string layout_desc)
        {
            sbyte[] c_layout_desc = (sbyte[])((Array)System.Text.Encoding.Default.GetBytes(layout_desc));
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_layout_create(ref c_layout_desc[0], ref m_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("Layout init error : {0}!", status.ToString());
            }
        }

        /// <summary>
        /// Default deconstruction
        /// </summary>
        ~Layout()
        {
            Dispose(); 
        }

        /// <summary>
        /// Release unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (m_ptr == IntPtr.Zero)
            {
                return;
            }
            NativeMethods.ov_layout_free(m_ptr);
            m_ptr = IntPtr.Zero;
        }

        /// <summary>
        ///  String representation of Layout.
        /// </summary>
        /// <returns>String representation of Layout.</returns>
        public string to_string()
        {
            return NativeMethods.ov_layout_to_string(m_ptr);
        }
    }
}
