using OpenCvSharp;
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
            im.ConvertTo(im, MatType.CV_32FC3, e);
            Mat[] bgr_channels = new Mat[3];
            Cv2.Split(im, out bgr_channels);
            for (var i = 0; i < bgr_channels.Length; i++)
            {
                bgr_channels[i].ConvertTo(bgr_channels[i], MatType.CV_32FC1, 1.0 * scale[i],
                    (0.0 - mean[i]) * scale[i]);
            }
            Mat re = new Mat();
            Cv2.Merge(bgr_channels, re);
            return re;
        }

        public static Mat run(Mat im, bool is_scale)
        {
            double e = 1.0;
            if (is_scale)
            {
                e /= 255.0;
            }
            im.ConvertTo(im, MatType.CV_32FC3, e);
            return im;
        }

    }
}
