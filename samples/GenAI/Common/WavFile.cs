// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System.Buffers.Binary;

namespace GenAI.Common;

/// <summary>
/// Minimal WAV reader for Whisper samples.
/// Whisper 示例使用的最小 WAV 读取器。
/// </summary>
public static class WavFile
{
    /// <summary>
    /// Reads WAV samples, converts to mono float, and resamples to targetSampleRate when needed.
    /// 读取 WAV，转为 mono float，并在需要时重采样到目标采样率。
    /// </summary>
    public static float[] ReadMonoFloat(string path, int targetSampleRate = 16000)
    {
        byte[] bytes = File.ReadAllBytes(path);
        if (bytes.Length < 44 || ReadAscii(bytes, 0, 4) != "RIFF" || ReadAscii(bytes, 8, 4) != "WAVE")
            throw new InvalidDataException("Only RIFF/WAVE files are supported. / 仅支持 RIFF/WAVE 文件。");

        int offset = 12;
        ushort audioFormat = 0;
        ushort channels = 0;
        int sampleRate = 0;
        ushort bitsPerSample = 0;
        int dataOffset = -1;
        int dataSize = 0;

        while (offset + 8 <= bytes.Length)
        {
            string chunkId = ReadAscii(bytes, offset, 4);
            int chunkSize = BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan(offset + 4, 4));
            int chunkData = offset + 8;

            if (chunkData + chunkSize > bytes.Length)
                throw new InvalidDataException("Invalid WAV chunk size. / WAV chunk size 无效。");

            if (chunkId == "fmt ")
            {
                audioFormat = BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(chunkData, 2));
                channels = BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(chunkData + 2, 2));
                sampleRate = BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan(chunkData + 4, 4));
                bitsPerSample = BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(chunkData + 14, 2));
            }
            else if (chunkId == "data")
            {
                dataOffset = chunkData;
                dataSize = chunkSize;
            }

            offset = chunkData + chunkSize + (chunkSize & 1);
        }

        if (dataOffset < 0 || channels == 0 || sampleRate <= 0)
            throw new InvalidDataException("Missing WAV format or data chunk. / WAV 缺少格式或数据 chunk。");

        float[] mono = DecodeMono(bytes.AsSpan(dataOffset, dataSize), audioFormat, channels, bitsPerSample);
        return sampleRate == targetSampleRate ? mono : ResampleLinear(mono, sampleRate, targetSampleRate);
    }

    private static float[] DecodeMono(ReadOnlySpan<byte> data, ushort audioFormat, ushort channels, ushort bitsPerSample)
    {
        int bytesPerSample = bitsPerSample / 8;
        if (bytesPerSample <= 0)
            throw new NotSupportedException("Unsupported WAV bit depth. / 不支持的 WAV 位深。");

        int frames = data.Length / (bytesPerSample * channels);
        float[] mono = new float[frames];

        for (int frame = 0; frame < frames; frame++)
        {
            double sum = 0;
            for (int channel = 0; channel < channels; channel++)
            {
                int index = (frame * channels + channel) * bytesPerSample;
                sum += DecodeSample(data.Slice(index, bytesPerSample), audioFormat, bitsPerSample);
            }
            mono[frame] = (float)(sum / channels);
        }

        return mono;
    }

    private static float DecodeSample(ReadOnlySpan<byte> sample, ushort audioFormat, ushort bitsPerSample)
    {
        if (audioFormat == 3 && bitsPerSample == 32)
            return BitConverter.Int32BitsToSingle(BinaryPrimitives.ReadInt32LittleEndian(sample));

        if (audioFormat != 1)
            throw new NotSupportedException("Only PCM integer and IEEE float WAV files are supported. / 仅支持 PCM 整数和 IEEE float WAV。");

        return bitsPerSample switch
        {
            8 => (sample[0] - 128) / 128f,
            16 => BinaryPrimitives.ReadInt16LittleEndian(sample) / 32768f,
            24 => DecodeInt24(sample) / 8388608f,
            32 => BinaryPrimitives.ReadInt32LittleEndian(sample) / 2147483648f,
            _ => throw new NotSupportedException($"Unsupported WAV bit depth: {bitsPerSample}")
        };
    }

    private static int DecodeInt24(ReadOnlySpan<byte> sample)
    {
        int value = sample[0] | (sample[1] << 8) | (sample[2] << 16);
        if ((value & 0x800000) != 0)
            value |= unchecked((int)0xFF000000);
        return value;
    }

    private static float[] ResampleLinear(float[] input, int sourceRate, int targetRate)
    {
        int outputLength = Math.Max(1, (int)Math.Round(input.Length * (double)targetRate / sourceRate));
        float[] output = new float[outputLength];
        double ratio = (double)sourceRate / targetRate;

        for (int i = 0; i < output.Length; i++)
        {
            double sourceIndex = i * ratio;
            int left = Math.Min(input.Length - 1, (int)Math.Floor(sourceIndex));
            int right = Math.Min(input.Length - 1, left + 1);
            double t = sourceIndex - left;
            output[i] = (float)(input[left] * (1.0 - t) + input[right] * t);
        }

        return output;
    }

    private static string ReadAscii(byte[] bytes, int offset, int count)
    {
        return System.Text.Encoding.ASCII.GetString(bytes, offset, count);
    }
}
