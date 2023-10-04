using OpenVinoSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.preprocess
{
    /// <summary>
    /// Preprocessing steps. Each step typically intends adding of some operation to input parameter
    /// User application can specify sequence of preprocessing steps in a builder-like manner
    /// </summary>
    public class PreProcessSteps : IDisposable
    {
        /// <summary>
        /// [private]PreProcessSteps class pointer.
        /// </summary>
        public IntPtr m_ptr = IntPtr.Zero;

        /// <summary>
        /// [public]PreProcessSteps class pointer.
        /// </summary>
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }

        /// <summary>
        /// Default construction through PreProcessSteps pointer.
        /// </summary>
        /// <param name="ptr">PreProcessSteps pointer.</param>
        public PreProcessSteps(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                System.Diagnostics.Debug.WriteLine("PreProcessSteps init error : ptr is null!");
                return;
            }
            this.m_ptr = ptr;
        }
        /// <summary>
        /// Default destructor
        /// </summary>
        ~PreProcessSteps() { Dispose(); }
        /// <summary>
        /// Release unmanaged resources
        /// </summary>
        public void Dispose()
        {
            if (m_ptr == IntPtr.Zero)
            {
                return;
            }
            NativeMethods.ov_preprocess_preprocess_steps_free(m_ptr);
           
            m_ptr = IntPtr.Zero;
        }

        /// <summary>
        /// Add resize operation to model's dimensions.
        /// </summary>
        /// <param name="resize">esize algorithm.</param>
        /// <returns>Reference to 'this' to allow chaining with other calls in a builder-like manner.</returns>
        public PreProcessSteps resize(ResizeAlgorithm resize) 
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_preprocess_steps_resize(
                m_ptr, (int)resize);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PreProcessSteps resize error : {0}!", status.ToString());
            }
            return this;
        }


        /// <summary>
        /// Add scale preprocess operation. Divide each element of input by specified value.
        /// </summary>
        /// <param name="value">Scaling value.</param>
        /// <returns>Reference to 'this' to allow chaining with other calls in a builder-like manner.</returns>
        public PreProcessSteps scale(float value)
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_preprocess_steps_scale(
                m_ptr, value);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PreProcessSteps resize error : {0}!", status.ToString());
            }
            return this;
        }

        /// <summary>
        /// Add mean preprocess operation. Subtract specified value from each element of input.
        /// </summary>
        /// <param name="value"> Value to subtract from each element.</param>
        /// <returns>Reference to 'this' to allow chaining with other calls in a builder-like manner.</returns>
        public PreProcessSteps mean(float value)
        {
            ExceptionStatus status = (ExceptionStatus)NativeMethods.ov_preprocess_preprocess_steps_mean(
                m_ptr, value);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PreProcessSteps mean error : {0}!", status.ToString());
            }
            return this;
        }

        /// <summary>
        /// Crop input tensor between begin and end coordinates. Under the hood, inserts `opset8::Slice` operation to
        /// execution graph. It is recommended to use to together with `ov::preprocess::InputTensorInfo::set_shape` to set
        /// original input shape before cropping
        /// </summary>
        /// <param name="begin">Begin indexes for input tensor cropping. Negative values represent counting elements from the end 
        /// of input tensor</param>
        /// <param name="end">End indexes for input tensor cropping. End indexes are exclusive, which means values including end
        /// edge are not included in the output slice. Negative values represent counting elements from the end of input tensor</param>
        /// <returns>Reference to 'this' to allow chaining with other calls in a builder-like manner.</returns>
        public PreProcessSteps crop(int[] begin, int[] end)
        {
            ExceptionStatus status = NativeMethods.ov_preprocess_preprocess_steps_crop(
                m_ptr, ref begin[0], begin.Length, ref end[0], end.Length);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PreProcessSteps crop error : {0}!", status.ToString());
            }
            return this;
        }
        /// <summary>
        /// Crop input tensor between begin and end coordinates. Under the hood, inserts `opset8::Slice` operation to
        /// execution graph. It is recommended to use to together with `ov::preprocess::InputTensorInfo::set_shape` to set
        /// original input shape before cropping
        /// </summary>
        /// <param name="begin">Begin indexes for input tensor cropping. Negative values represent counting elements from the end
        /// of input tensor</param>
        /// <param name="end">End indexes for input tensor cropping. End indexes are exclusive, which means values including end
        /// edge are not included in the output slice. Negative values represent counting elements from the end of input
        /// tensor</param>
        /// <returns>Reference to 'this' to allow chaining with other calls in a builder-like manner.</returns>
        public PreProcessSteps crop(List<int> begin, List<int> end)
        {
            ExceptionStatus status = NativeMethods.ov_preprocess_preprocess_steps_crop(
                m_ptr, ref begin.ToArray()[0], begin.Count, ref end.ToArray()[0], end.Count);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PreProcessSteps crop error : {0}!", status.ToString());
            }
            return this;
        }

        /// <summary>
        /// Add 'convert layout' operation to specified layout.
        /// </summary>
        /// <param name="layout">New layout after conversion. If not specified - destination layout is obtained from
        /// appropriate model input properties.</param>
        /// <returns>Reference to 'this' to allow chaining with other calls in a builder-like manner.</returns>
        /// <remarks>
        /// Adds appropriate 'transpose' operation between user layout and target layout.
        /// Current implementation requires source and destination layout to have same number of dimensions
        /// </remarks>
        /// <example>
        /// when user data has 'NHWC' layout (example is RGB image, [1, 224, 224, 3]) but model expects
        /// planar input image ('NCHW', [1, 3, 224, 224]). Preprocessing may look like this:
        /// <code>
        /// var proc = PrePostProcessor(model);
        /// proc.input().tensor().set_layout("NHWC"); // User data is NHWC
        /// proc.input().preprocess().convert_layout("NCHW")) // model expects input as NCHW
        /// </code>
        /// </example>
        public PreProcessSteps convert_layout(Layout layout)
        {
            ExceptionStatus status = NativeMethods.ov_preprocess_preprocess_steps_convert_layout(
                m_ptr, layout.Ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PreProcessSteps convert_layout error : {0}!", status.ToString());
            }
            return this;
        }

        /// <summary>
        /// Reverse channels operation.
        /// </summary>
        /// <returns>Reference to 'this' to allow chaining with other calls in a builder-like manner.</returns>
        /// <remarks>
        /// Adds appropriate operation which reverses channels layout. Operation requires layout having 'C'
        /// dimension Operation convert_color (RGB-BGR) does reversing of channels also, but only for NHWC layout
        /// </remarks>
        /// <example>
        /// when user data has 'NCHW' layout (example is [1, 3, 224, 224] RGB order) but model expects
        /// BGR planes order. Preprocessing may look like this:
        /// <code>
        /// var proc = PrePostProcessor(function);
        /// proc.input().preprocess().convert_layout({0, 3, 1, 2});
        /// </code>
        /// </example>
        public PreProcessSteps reverse_channels()
        {
            ExceptionStatus status = NativeMethods.ov_preprocess_preprocess_steps_reverse_channels(
                m_ptr);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PreProcessSteps reverse_channels error : {0}!", status.ToString());
            }
            return this;
        }

        /// <summary>
        /// Converts color format for user's input tensor. Requires source color format to be specified by 
        /// nputTensorInfo::set_color_format.
        /// </summary>
        /// <param name="format">Destination color format of input image.</param>
        /// <returns>Reference to 'this' to allow chaining with other calls in a builder-like manner.</returns>
        public PreProcessSteps convert_color(ColorFormat format) 
        {
            ExceptionStatus status = NativeMethods.ov_preprocess_preprocess_steps_convert_color(
                m_ptr, (uint)format);
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PreProcessSteps convert_element_type error : {0}!", status.ToString());
            }
            return this;
        }

        /// <summary>
        /// Add convert element type preprocess operation.
        /// </summary>
        /// <param name="type">Desired type of input.</param>
        /// <returns>Reference to 'this' to allow chaining with other calls in a builder-like manner.</returns>
        public PreProcessSteps convert_element_type(OvType type)
        {
            ExceptionStatus status = NativeMethods.ov_preprocess_preprocess_steps_convert_element_type(
                m_ptr, (uint)type.get_type());
            if (status != 0)
            {
                System.Diagnostics.Debug.WriteLine("PreProcessSteps convert_element_type error : {0}!", status.ToString());
            }
            return this;
        }
    }
}
