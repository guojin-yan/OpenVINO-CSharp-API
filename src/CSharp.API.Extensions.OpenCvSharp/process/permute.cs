
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.Extensions.process
{
    public static class Permute
    {
        public static float[] run(Mat im)
        {
            int rh = im.Rows;
            int rw = im.Cols;
            int rc = im.Channels();
            float[] res = new float[rh * rw * rc];

            GCHandle resultHandle = default;
            try
            {
                resultHandle = GCHandle.Alloc(res, GCHandleType.Pinned);
                IntPtr resultPtr = resultHandle.AddrOfPinnedObject();
                for (int i = 0; i < rc; ++i)
                {
                    using Mat dest = new(rh, rw, MatType.CV_32FC1, resultPtr + i * rh * rw * sizeof(float));
                    Cv2.ExtractChannel(im, dest, i);
                }
            }
            finally
            {
                resultHandle.Free();
            }
            return res;

        }
       
    }
    public static class PermuteBatch
    {
        public static float[] run(List<Mat> imgs)
        {
            int rh = imgs[0].Rows;
            int rw = imgs[0].Cols;
            int rc = imgs[0].Channels();
            float[] res = new float[rh * rw * rc * imgs.Count];

            GCHandle resultHandle = default;
            resultHandle = GCHandle.Alloc(res, GCHandleType.Pinned);
            IntPtr resultPtr = resultHandle.AddrOfPinnedObject();
            try
            {
                for (int j = 0; j < imgs.Count; j++)
                {
                    for (int i = 0; i < rc; ++i)
                    {
                        using Mat dest = new(rh, rw, MatType.CV_32FC1, resultPtr + (j * rc + i) * rh * rw * sizeof(float));
                        Cv2.ExtractChannel(imgs[j], dest, i);
                    }
                }
            }
            finally
            {
                resultHandle.Free();
            }
            return res;
        }
    }
}
