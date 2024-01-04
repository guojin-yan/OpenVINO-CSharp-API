using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.Extensions.process
{
    /// <summary>
    /// Normalize data classes using EmguCV.
    /// </summary>
    public static class Normalize
    {
        /// <summary>
        /// Run normalize data classes.
        /// </summary>
        /// <param name="im">The image mat.</param>
        /// <param name="mean">Channel mean.</param>
        /// <param name="scale">Channel variance.</param>
        /// <param name="is_scale">Whether to divide by 255.</param>
        /// <returns>The normalize data.</returns>
        public static Mat run(Mat im, float[] mean, float[] scale, bool is_scale)
        {
            double e = 1.0;
            if (is_scale)
            {
                e /= 255.0;
            }
            im.ConvertTo(im, DepthType.Cv32F, e);
            VectorOfMat bgr_channels = new VectorOfMat();

            CvInvoke.Split(im, bgr_channels);
            for (var i = 0; i < bgr_channels.Length; i++)
            {
                bgr_channels[i].ConvertTo(bgr_channels[i], DepthType.Cv32F, 1.0 * scale[i],
                    (0.0 - mean[i]) * scale[i]);
            }
            Mat re = new Mat();
            CvInvoke.Merge(bgr_channels, re);
            return re;
        }
        /// <summary>
        /// Run normalize data classes.
        /// </summary>
        /// <param name="im">The image mat.</param>
        /// <param name="is_scale">Whether to divide by 255.</param>
        /// <returns>The normalize data.</returns>
        public static Mat run(Mat im, bool is_scale)
        {
            double e = 1.0;
            if (is_scale)
            {
                e /= 255.0;
            }
            im.ConvertTo(im, DepthType.Cv32F, e);
            return im;
        }

    }
}
