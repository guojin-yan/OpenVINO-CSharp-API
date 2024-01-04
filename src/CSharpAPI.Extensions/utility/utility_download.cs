using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers.Tar;

namespace OpenVinoSharp.Extensions.utility
{
    public static class Download
    {
        public static async Task<int> download_file_async(string url, string file_path, bool confirm = false)
        {

            HttpClient client = new HttpClient();
            Stopwatch stopwatch = Stopwatch.StartNew();


            await Console.Out.WriteLineAsync(
            $"<{TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds).ToString(@"hh\:mm\:ss")}> Sending http request to {url}.");

            using var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

            await Console.Out.WriteLineAsync(
                $"<{TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds).ToString(@"hh\:mm\:ss")}> Http Response Accquired.");

            long? content_len = response.Content.Headers.ContentLength;
            long total_len = content_len.HasValue ? content_len.Value : -1;

            await Console.Out.WriteLineAsync(
                $"<{TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds).ToString(@"hh\:mm\:ss")}> Total download length is {((float)total_len / (1024.0f * 1024.0f)).ToString("0.00")} Mb.");
            if (confirm)
            {
                await Console.Out.WriteAsync(
                    "Continue download? Y/N:");

                var k = Console.ReadKey();

                while (k.KeyChar != 'y' && k.KeyChar != 'Y')
                {
                    return -1;
                }
                await Console.Out.WriteLineAsync();
            }


            await Console.Out.WriteLineAsync(
                $"<{TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds).ToString(@"hh\:mm\:ss")}> Download Started.");
            File.Delete(file_path);
            using var download_file = File.Create(file_path);

            await Console.Out.WriteLineAsync(
                $"<{TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds).ToString(@"hh\:mm\:ss")}> File created.");

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
            await Console.Out.WriteLineAsync();
            await Console.Out.WriteLineAsync(
                $"<{TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds).ToString(@"hh\:mm\:ss")}> File Downloaded, saved in {Path.GetFullPath(file_path)}.");

            stopwatch.Stop();
            return 0;
        }

        public static void unzip(string file_path, string extract_path)
        {
            string extension = Path.GetExtension(file_path);
            if (extension == ".zip")
            {
                ZipFile.ExtractToDirectory(file_path, extract_path);
            }
            else if (extension == ".tar")
            {
                using (Stream stream = File.OpenRead(file_path))
                {
                    using (var tar_reader = TarReader.Open(stream))
                    {
                        while (tar_reader.MoveToNextEntry())
                        {
                            if (!tar_reader.Entry.IsDirectory)
                            {
                                string entry_path = Path.Combine(extract_path, tar_reader.Entry.Key);
                                Directory.CreateDirectory(Path.GetDirectoryName(entry_path));

                                using (Stream entry_stream = tar_reader.OpenEntryStream())
                                {
                                    using (Stream output_stream = File.Create(entry_path))
                                    {
                                        entry_stream.CopyTo(output_stream);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (extension == ".rar")
            {
                using (var archive = RarArchive.Open(file_path))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (!entry.IsDirectory)
                        {
                            entry.WriteToDirectory(extract_path, new ExtractionOptions()
                            {
                                ExtractFullPath = true,
                                Overwrite = true
                            });
                        }
                    }
                }
            }
            else { throw new NotSupportedException("Decompression of this format file is currently not supported."); }
        }

    }

    public class DownloadConsole
    {
        const char _block = '■';
        const string _back = "\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b";
        const string _twirl = "-\\|/";

        float total_m;
        long total_len;

        float last_down = 0;
        long last_time = 0;
        int num = 0;

        public DownloadConsole(long total_len)
        {
            this.total_m = (float)total_len / (1024.0f * 1024.0f);
            this.total_len = total_len;
        }
        public void progress_bar(long down_len, long time, bool update = false)
        {
            int percent = (int)(((float)down_len / (float)total_len) * 100);
            float down = down_len / (1024.0f * 1024.0f);
            if (update)
                Console.Write(_back);
            Console.Write("<{0}> Downloading: [", TimeSpan.FromMilliseconds(time).ToString(@"hh\:mm\:ss"));
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
                string s = string.Format(" <{0} Mb/s> {1} Mb/{2} Mb downloaded.",
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
                string s = string.Format(" <{0} {1} Mb/s> {2} Mb/{3} Mb downloaded.",
                                    formattedTime, down_speed.ToString("0.00"), down.ToString("0.00"), total_m.ToString("0.00"));
                Console.Write(s);
            }
            num++;
        }
    }
}
