using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
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
            int rc = 3;
            int len = rh * rw;
            float[]  res = new float[rh * rw * rc];

            //// 创建单通道图像以存储提取的通道  
            //Mat c_0 = new Mat(im.Size, DepthType.Cv32F,1);
            //Mat c_1 = new Mat(im.Size, DepthType.Cv32F, 1);
            //Mat c_2 = new Mat(im.Size, DepthType.Cv32F, 1);

            //// 提取通道  
            //CvInvoke.ExtractChannel(im, c_0, 0);
            //CvInvoke.ExtractChannel(im, c_1, 1);
            //CvInvoke.ExtractChannel(im, c_2, 2);

            //// 创建一个一维数组来存储所有通道的数据  

            //// 将每个通道的数据复制到一维数组中  
            //Buffer.BlockCopy(c_0.GetData(), 0, res, 0, len);
            //Buffer.BlockCopy(c_1.GetData(), 0, res, len, len);
            //Buffer.BlockCopy(c_2.GetData(), 0, res, len * 2, len);

            //return res;
            GCHandle resultHandle = default;
            try
            {
                resultHandle = GCHandle.Alloc(res, GCHandleType.Pinned);
                IntPtr resultPtr = resultHandle.AddrOfPinnedObject();
                for (int i = 0; i < rc; ++i)
                {
                    using Mat dest = new Mat(rh, rw, DepthType.Cv32F, 1, resultPtr + i * rh * rw * 4, rw * 4);
                    CvInvoke.ExtractChannel(im, dest, i);
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
            int rc = 3;
            int len = rh * rw;
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
                        using Mat dest = new Mat(rh, rw, DepthType.Cv32F, 1, resultPtr + (i + j * rc) * rh * rw * 4, rw * 4);
                        CvInvoke.ExtractChannel(imgs[j], dest, i);
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
