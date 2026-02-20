// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
#if NET46_OR_GREATER || NET47_OR_GREATER || NET48_OR_GREATER || NET5_0_OR_GREATER
using System.Net.Http;
#endif
using System.Threading.Tasks;

namespace OpenVinoSharp.extensions.utility
{
    /// <summary>
    /// 文件下载工具类 / File download utility class
    /// </summary>
    public static class Download
    {
        /// <summary>
        /// 异步下载文件 / Download file asynchronously
        /// </summary>
        /// <param name="url">文件URL / File URL</param>
        /// <param name="file_path">保存路径 / Save path</param>
        /// <param name="confirm">是否需要确认 / Whether to confirm</param>
        /// <returns>状态码 / Status code</returns>
        public static async Task<int> download_file_async(string url, string file_path, bool confirm = false)
        {
#if NET46_OR_GREATER || NET47_OR_GREATER || NET48_OR_GREATER || NET5_0_OR_GREATER
            HttpClient client = new HttpClient();
            Stopwatch stopwatch = Stopwatch.StartNew();

            await Console.Out.WriteLineAsync(
                $"<{TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds).ToString(@"hh\:mm\:ss")}> 发送HTTP请求到 / Sending http request to {url}.");

            var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

            await Console.Out.WriteLineAsync(
                $"<{TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds).ToString(@"hh\:mm\:ss")}> HTTP响应已获取 / Http Response Acquired.");

            long? content_len = response.Content.Headers.ContentLength;
            long total_len = content_len.HasValue ? content_len.Value : -1;

            await Console.Out.WriteLineAsync(
                $"<{TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds).ToString(@"hh\:mm\:ss")}> 总下载长度 / Total download length is {((float)total_len / (1024.0f * 1024.0f)).ToString("0.00")} Mb.");

            if (confirm)
            {
                await Console.Out.WriteAsync("继续下载? Y/N / Continue download? Y/N:");
                var k = Console.ReadKey();
                while (k.KeyChar != 'y' && k.KeyChar != 'Y')
                {
                    return -1;
                }
                await Console.Out.WriteLineAsync();
            }

            await Console.Out.WriteLineAsync(
                $"<{TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds).ToString(@"hh\:mm\:ss")}> 开始下载 / Download Started.");

            File.Delete(file_path);
            var download_file = File.Create(file_path);

            await Console.Out.WriteLineAsync(
                $"<{TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds).ToString(@"hh\:mm\:ss")}> 文件已创建 / File created.");

            using (var download = await response.Content.ReadAsStreamAsync())
            {
                var buffer = new byte[81920];
                long total_bytes_read = 0;
                int bytes_read;
                DownloadConsole console = new DownloadConsole(total_len);

                console.progress_bar(0, total_len);
                while ((bytes_read = await download.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) != 0)
                {
                    await download_file.WriteAsync(buffer, 0, bytes_read).ConfigureAwait(false);
                    total_bytes_read += bytes_read;
                    console.progress_bar(total_bytes_read, stopwatch.ElapsedMilliseconds, true);
                }
            }

            download_file.Dispose();
            await Console.Out.WriteLineAsync();
            await Console.Out.WriteLineAsync(
                $"<{TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds).ToString(@"hh\:mm\:ss")}> 文件已下载，保存在 / File Downloaded, saved in {Path.GetFullPath(file_path)}.");

            stopwatch.Stop();
            return 0;
#else
            throw new NotSupportedException("下载功能需要 .NET Framework 4.6+ 或 .NET 5+ / Download feature requires .NET Framework 4.6+ or .NET 5+");
#endif
        }

        /// <summary>
        /// 解压文件 / Unzip file
        /// </summary>
        /// <param name="file_path">压缩文件路径 / Compressed file path</param>
        /// <param name="extract_path">解压目标路径 / Extraction destination path</param>
        public static void unzip(string file_path, string extract_path)
        {
            string extension = Path.GetExtension(file_path);
            if (extension == ".zip")
            {
                ZipFile.ExtractToDirectory(file_path, extract_path);
            }
            else if (extension == ".tar")
            {
                // 简单实现，完整实现需要额外依赖 / Simple implementation, full implementation requires additional dependencies
                throw new NotSupportedException("TAR格式解压需要额外依赖 / TAR format decompression requires additional dependencies.");
            }
            else if (extension == ".rar")
            {
                throw new NotSupportedException("RAR格式解压需要额外依赖 / RAR format decompression requires additional dependencies.");
            }
            else
            {
                throw new NotSupportedException("当前不支持此格式文件的解压 / Decompression of this format file is currently not supported.");
            }
        }
    }

    /// <summary>
    /// 下载进度控制台显示类 / Download progress console display class
    /// </summary>
    public class DownloadConsole
    {
        const char _block = '■';
        const string _back = "\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b";
        const string _twirl = "-\\|/";

        float total_m;
        long total_len;

        float last_down = 0;
        long last_time = 0;
        int num = 0;

        /// <summary>
        /// 构造函数 / Constructor
        /// </summary>
        /// <param name="total_len">总长度 / Total length</param>
        public DownloadConsole(long total_len)
        {
            this.total_m = (float)total_len / (1024.0f * 1024.0f);
            this.total_len = total_len;
        }

        /// <summary>
        /// 显示进度条 / Display progress bar
        /// </summary>
        /// <param name="down_len">已下载长度 / Downloaded length</param>
        /// <param name="time">时间 / Time</param>
        /// <param name="update">是否更新 / Whether to update</param>
        public void progress_bar(long down_len, long time, bool update = false)
        {
            int percent = (int)(((float)down_len / (float)total_len) * 100);
            float down = down_len / (1024.0f * 1024.0f);
            if (update)
                Console.Write(_back);
            Console.Write("<{0}> 下载中 / Downloading: [", TimeSpan.FromMilliseconds(time).ToString(@"hh\:mm\:ss"));
            var p = (int)((percent / 10f) + .5f);
            for (var i = 0; i < 10; ++i)
            {
                if (i > p)
                    Console.Write("  ");
                else if (i == p)
                    Console.Write(_twirl[percent % _twirl.Length]);
                else
                    Console.Write(_block);
            }
            Console.Write("] {0,3:##0}%", percent);

            if (num > 1000)
            {
                float down_speed = (down - last_down) / (time - last_time) * 1000;
                string s = string.Format(" <{0} Mb/s> {1} Mb/{2} Mb 已下载 / downloaded.",
                    down_speed.ToString("0.00"), down.ToString("0.00"), total_m.ToString("0.00"));
                Console.Write(s);
                num = 0;
                last_down = down;
                last_time = time;
            }
            else
            {
                float down_speed = (down - last_down) / (time - last_time) * 1000;
                TimeSpan time_now = TimeSpan.FromMilliseconds(time);
                string formattedTime = time_now.ToString(@"hh\:mm\:ss");
                string s = string.Format(" <{0} {1} Mb/s> {2} Mb/{3} Mb 已下载 / downloaded.",
                                    formattedTime, down_speed.ToString("0.00"), down.ToString("0.00"), total_m.ToString("0.00"));
                Console.Write(s);
            }
            num++;
        }
    }
}
