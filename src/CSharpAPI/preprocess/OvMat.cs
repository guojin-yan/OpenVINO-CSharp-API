using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp.preprocess
{
    public class OvMat
    {
        public byte[] mat_data { get; set; }
        public ulong mat_data_size { get; set; }
        public int mat_width { get; set; }
        public int mat_height { get; set; }
        public int mat_channels { get; set; }
        public ElementType mat_type { get; set; } = ElementType.U8;

        public OvMat() { }
        public OvMat(byte[] mat_data, ulong mat_data_size, int mat_width, int mat_height, int mat_channels, ElementType mat_type)
        {
            this.mat_data = mat_data;
            this.mat_data_size = mat_data_size;
            this.mat_width = mat_width;
            this.mat_height = mat_height;
            this.mat_channels = mat_channels;
            this.mat_type = mat_type;
        }
        public OvMat(string image_path) 
        {
            Bitmap img = new Bitmap(image_path);
            Rectangle rect = new Rectangle(0, 0, img.Width, img.Height);
            BitmapData bit = img.LockBits(rect, ImageLockMode.ReadWrite, img.PixelFormat);
            byte[] byte_data = new byte[bit.Width * bit.Height * 3];
            Marshal.Copy(bit.Scan0, byte_data, 0, byte_data.Length);
            this.mat_data = byte_data;
            this.mat_data_size = (ulong)(img.Height * img.Width * 3);
            this.mat_width = img.Width;
            this.mat_height = img.Height;
            this.mat_channels = 3;
            this.mat_type = ElementType.U8;
            img.Dispose();
            
        }
        public static OvMat read(string image_path) 
        {
            Bitmap img = new Bitmap(image_path);
            Rectangle rect = new Rectangle(0, 0, img.Width, img.Height);
            BitmapData bit = img.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            img.UnlockBits(bit);
            byte[] byte_data = new byte[bit.Width * bit.Height * 3];
            Marshal.Copy(bit.Scan0, byte_data, 0, byte_data.Length);
            OvMat mat = new OvMat(byte_data, (ulong)(img.Height * img.Width * 3), img.Width, img.Height, 3, ElementType.U8);
           
            img.Dispose();
            return mat;
            //return new OvMat(image_path);
        }
    }
}
