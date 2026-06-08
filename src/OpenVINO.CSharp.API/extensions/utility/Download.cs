//  ========================================================================
//  【项目名称】OpenVINO C# API
//  【项目描述】OpenVINO™ 的 C# 语言绑定库，提供高性能深度学习推理能力
//  【版权声明】© 2026-2025 Guojin Yan. All Rights Reserved.
//  【开源协议】Apache-2.0 License（请遵守许可证条款）
//  -----------------------------------------------------------------------
//  【功能简介】
//  1. 完整的 OpenVINO™ C API 封装，提供 C# 友好的面向对象接口。
//  2. 支持模型加载、编译、推理全流程操作。
//  3. 支持 CPU、GPU、VPU 等多种推理设备。
//  4. 支持同步推理和异步推理模式。
//  5. 支持预处理和后处理流水线配置。
//  6. 支持动态形状和批量推理。
//  7. 支持模型缓存和性能分析。
//  8. 支持远程上下文（Remote Context）和零拷贝推理。
//  9. 支持 .NET Framework 4.6.1+、.NET Core 2.0+、.NET 5/6/7/8/9+。
//  10. 提供推理请求对象池，优化高并发场景性能。
//  11. 提供完善的异常处理和日志记录机制。
//  12. 提供丰富的单元测试和集成测试用例。
//  -----------------------------------------------------------------------
//  【官方资源】
//  📌 GitHub仓库：https://github.com/guojin-yan/OpenVINO-CSharp-API
//  📌 NuGet包：https://www.nuget.org/packages/OpenVINO.CSharp.API
//  📌 在线文档：https://guojin-yan.github.io/OpenVINO-CSharp-API/index.html
//  📌 示例代码：https://github.com/guojin-yan/OpenVINO-CSharp-API/tree/csharp3.3/samples
//  -----------------------------------------------------------------------
//  【社区支持】
//  💬 QQ交流群：945057948（加入获取技术支持）
//  📱 微信公众号：CSharp与边缘模型部署（教程+案例）
//  📝 CSDN博客：https://guojin.blog.csdn.net（技术文章）
//  -----------------------------------------------------------------------
//  【联系我们】
//  ✉ 项目维护：guojin_yjs@cumt.edu.cn
//  💬 微信咨询：15253793309
//  🐛 Bug反馈：https://github.com/guojin-yan/OpenVINO-CSharp-API/issues
//  💡 功能建议：https://github.com/guojin-yan/OpenVINO-CSharp-API/discussions/landing
//  -----------------------------------------------------------------------
//  【致谢】
//  本项目基于 Intel® OpenVINO™ 工具包开发，感谢 Intel 提供的优秀开源项目。
//  OpenVINO™ 是 Intel Corporation 的商标。
//  ========================================================================
//  
//  【许可声明】
//  1. 本项目采用 Apache-2.0 License 开源协议，允许自由使用、修改和分发。
//  2. 使用本项目即表示您同意 Apache-2.0 License 许可证的所有条款。
//  3. 本项目按"原样"提供，不提供任何形式的担保。
//  4. 使用本项目产生的任何风险由使用者自行承担。
//  5. 修改或分发时请保留原始版权声明和许可声明。
//  ========================================================================
//

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
