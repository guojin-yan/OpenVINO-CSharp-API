// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System.Buffers.Binary;
using OpenVinoSharp;

namespace GenAI.Common;

/// <summary>
/// Dependency-free image loader for VLM samples.
/// VLM 示例使用的无额外依赖图像读取器。
/// </summary>
public static class ImageTensorLoader
{
    /// <summary>
    /// Loads a BMP or binary PPM image as a NHWC U8 Tensor.
    /// 将 BMP 或二进制 PPM 图像加载为 NHWC U8 Tensor。
    /// </summary>
    public static Tensor LoadRgbTensor(string path)
    {
        string extension = Path.GetExtension(path).ToLowerInvariant();
        ImageData image = extension switch
        {
            ".bmp" => LoadBmp(path),
            ".ppm" or ".pnm" => LoadPpm(path),
            _ => throw new NotSupportedException("This sample loader supports .bmp and binary .ppm/.pnm. Use the conda/Pillow conversion command in the README for jpg/png files.")
        };

        using Shape shape = new(new long[] { 1, image.Height, image.Width, 3 });
        Tensor tensor = new(shape, ElementType.U8);
        tensor.SetData(image.Rgb);
        return tensor;
    }

    private static ImageData LoadBmp(string path)
    {
        byte[] bytes = File.ReadAllBytes(path);
        if (bytes.Length < 54 || bytes[0] != 'B' || bytes[1] != 'M')
            throw new InvalidDataException("Invalid BMP file. / BMP 文件无效。");

        int pixelOffset = BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan(10, 4));
        int dibSize = BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan(14, 4));
        if (dibSize < 40)
            throw new NotSupportedException("Only BITMAPINFOHEADER BMP files are supported. / 仅支持 BITMAPINFOHEADER BMP。");

        int width = BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan(18, 4));
        int signedHeight = BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan(22, 4));
        ushort planes = BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(26, 2));
        ushort bitsPerPixel = BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(28, 2));
        int compression = BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan(30, 4));

        if (planes != 1 || compression != 0 || (bitsPerPixel != 24 && bitsPerPixel != 32))
            throw new NotSupportedException("Only uncompressed 24-bit or 32-bit BMP files are supported. / 仅支持未压缩 24-bit 或 32-bit BMP。");

        bool topDown = signedHeight < 0;
        int height = Math.Abs(signedHeight);
        int bytesPerPixel = bitsPerPixel / 8;
        int stride = ((width * bytesPerPixel + 3) / 4) * 4;
        byte[] rgb = new byte[checked(width * height * 3)];

        for (int y = 0; y < height; y++)
        {
            int sourceY = topDown ? y : height - 1 - y;
            int rowOffset = pixelOffset + sourceY * stride;
            for (int x = 0; x < width; x++)
            {
                int source = rowOffset + x * bytesPerPixel;
                int target = (y * width + x) * 3;
                rgb[target] = bytes[source + 2];
                rgb[target + 1] = bytes[source + 1];
                rgb[target + 2] = bytes[source];
            }
        }

        return new ImageData(width, height, rgb);
    }

    private static ImageData LoadPpm(string path)
    {
        byte[] bytes = File.ReadAllBytes(path);
        int offset = 0;
        string magic = ReadToken(bytes, ref offset);
        if (magic != "P6")
            throw new NotSupportedException("Only binary PPM P6 images are supported. / 仅支持二进制 PPM P6 图像。");

        int width = int.Parse(ReadToken(bytes, ref offset), System.Globalization.CultureInfo.InvariantCulture);
        int height = int.Parse(ReadToken(bytes, ref offset), System.Globalization.CultureInfo.InvariantCulture);
        int maxValue = int.Parse(ReadToken(bytes, ref offset), System.Globalization.CultureInfo.InvariantCulture);
        if (maxValue != 255)
            throw new NotSupportedException("Only 8-bit PPM files are supported. / 仅支持 8-bit PPM。");

        SkipWhitespaceAndComments(bytes, ref offset);
        int expected = checked(width * height * 3);
        if (bytes.Length - offset < expected)
            throw new InvalidDataException("PPM pixel data is truncated. / PPM 像素数据不完整。");

        byte[] rgb = new byte[expected];
        Buffer.BlockCopy(bytes, offset, rgb, 0, expected);
        return new ImageData(width, height, rgb);
    }

    private static string ReadToken(byte[] bytes, ref int offset)
    {
        SkipWhitespaceAndComments(bytes, ref offset);
        int start = offset;
        while (offset < bytes.Length && !char.IsWhiteSpace((char)bytes[offset]))
            offset++;
        return System.Text.Encoding.ASCII.GetString(bytes, start, offset - start);
    }

    private static void SkipWhitespaceAndComments(byte[] bytes, ref int offset)
    {
        while (offset < bytes.Length)
        {
            while (offset < bytes.Length && char.IsWhiteSpace((char)bytes[offset]))
                offset++;
            if (offset < bytes.Length && bytes[offset] == '#')
            {
                while (offset < bytes.Length && bytes[offset] != '\n')
                    offset++;
                continue;
            }
            break;
        }
    }

    private readonly record struct ImageData(int Width, int Height, byte[] Rgb);
}
