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
    public static class Normalize
    {
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
