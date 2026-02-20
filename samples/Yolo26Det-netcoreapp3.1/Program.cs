//========================================================================
//  
//  【示例名称】YOLOv8/v9/v10 目标检测完整示例
//  【示例说明】演示 OpenVINO C# API 的完整功能
//  【功能特性】
//  1. 同步/异步推理 (Synchronous/Asynchronous Inference)
//  2. 批量图片处理 (Batch Image Processing)
//  3. 推理请求池优化 (InferRequest Pool Optimization)
//  4. 性能分析 (Performance Profiling)
//  5. 模型缓存 (Model Caching - OpenVINO CACHE_DIR)
//  6. 预处理/后处理流水线 (Preprocessing/Postprocessing Pipeline)
//  
//========================================================================

using OpenCvSharp;
using OpenVinoSharp;
using OpenVinoSharp.extensions;
using OpenVinoSharp.Internal;
using OpenVinoSharp.preprocess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using Version = OpenVinoSharp.Version;

namespace Yolo26Det_netcoreapp3_1
{
    /// <summary>
    /// 检测目标结果 / Detection result
    /// </summary>
    public struct DetectionResult
    {
        public float Confidence;  // 置信度
        public int Category;      // 类别ID
        public Rect Bounds;       // 边界框
        public string Label;      // 类别名称
    }


    internal class Program
    {
        // 类别名称映射 (COCO 数据集)
        private static readonly string[] COCO_CLASSES = new string[]
        {
            "person", "bicycle", "car", "motorcycle", "airplane", "bus", "train", "truck", "boat", "traffic light",
            "fire hydrant", "stop sign", "parking meter", "bench", "bird", "cat", "dog", "horse", "sheep", "cow",
            "elephant", "bear", "zebra", "giraffe", "backpack", "umbrella", "handbag", "tie", "suitcase", "frisbee",
            "skis", "snowboard", "sports ball", "kite", "baseball bat", "baseball glove", "skateboard", "surfboard",
            "tennis racket", "bottle", "wine glass", "cup", "fork", "knife", "spoon", "bowl", "banana", "apple",
            "sandwich", "orange", "broccoli", "carrot", "hot dog", "pizza", "donut", "cake", "chair", "couch",
            "potted plant", "bed", "dining table", "toilet", "tv", "laptop", "mouse", "remote", "keyboard",
            "cell phone", "microwave", "oven", "toaster", "sink", "refrigerator", "book", "clock", "vase",
            "scissors", "teddy bear", "hair drier", "toothbrush"
        };

        static void Main(string[] args)
        {
            Console.WriteLine("========================================================================");
            Console.WriteLine("  OpenVINO C# API - YOLO 目标检测示例");
            Console.WriteLine("  OpenVINO C# API - YOLO Object Detection Demo");
            Console.WriteLine("========================================================================\n");

            // 解析命令行参数
            string imagePath = args.Length > 0 ? args[0] : "./images/bus.jpg";
            string modelPath = args.Length > 1 ? args[1] : "./model/yolo26n.xml";
            string device = args.Length > 2 ? args[2] : "CPU";

            // 检查模型文件
            if (!File.Exists(modelPath))
            {
                Console.WriteLine($"错误：模型文件不存在 / Error: Model file not found: {modelPath}");
                Console.WriteLine("请将模型文件放置在正确位置 / Please place model file in correct location");
                return;
            }

            try
            {
                // 演示 1: 基础同步推理
                Console.WriteLine("\n【演示 1】基础同步推理 / Basic Synchronous Inference");
                Console.WriteLine("--------------------------------------------------------");
                DemoBasicInference(modelPath, imagePath, device);

                // 演示 2: 异步推理
                Console.WriteLine("\n【演示 2】异步推理 / Asynchronous Inference");
                Console.WriteLine("--------------------------------------------------------");
                DemoAsyncInference(modelPath, imagePath, device);

                // 演示 3: 批量推理
                Console.WriteLine("\n【演示 3】批量推理 / Batch Inference");
                Console.WriteLine("--------------------------------------------------------");
                DemoBatchInference(modelPath, imagePath, device);

                // 演示 4: 推理请求池
                Console.WriteLine("\n【演示 4】推理请求池 / Infer Request Pool");
                Console.WriteLine("--------------------------------------------------------");
                DemoRequestPool(modelPath, imagePath, device);

                // 演示 5: 性能分析
                Console.WriteLine("\n【演示 5】性能分析 / Performance Profiling");
                Console.WriteLine("--------------------------------------------------------");
                DemoProfiling(modelPath, imagePath, device);

                // 演示 6: 模型缓存 (OpenVINO 磁盘缓存)
                Console.WriteLine("\n【演示 6】模型缓存 (OpenVINO Cache)");
                Console.WriteLine("--------------------------------------------------------");
                DemoModelCache(modelPath, imagePath, device);

                Console.WriteLine("\n========================================================================");
                Console.WriteLine("  所有演示完成 / All demos completed");
                Console.WriteLine("========================================================================");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n错误 / Error: {ex.Message}");
                Console.WriteLine($"堆栈 / StackTrace: {ex.StackTrace}");
            }

            Console.WriteLine("\n按任意键退出 / Press any key to exit...");
            Console.ReadKey();
        }

        #region 演示 1: 基础同步推理

        /// <summary>
        /// 基础同步推理演示 / Basic synchronous inference demo
        /// </summary>
        static void DemoBasicInference(string modelPath, string imagePath, string device)
        {
            var totalStopwatch = Stopwatch.StartNew();
            var profiler = new InferenceProfiler();

            // Step 1: 初始化 OpenVINO 运行时
            totalStopwatch.Restart();
            using (var core = new Core())
            {
                totalStopwatch.Stop();
                OvLogger.Info($"1. 初始化 Core / Initialize Core: {totalStopwatch.ElapsedMilliseconds}ms");

                // 获取设备版本信息
                var version = core.get_versions(device);
                OvLogger.Info($"   设备 / Device: {version.Key}");
                OvLogger.Info($"   版本 / Version: {version.Value.description}");

                // Step 2: 读取模型
                totalStopwatch.Restart();
                using (var model = core.read_model(modelPath))
                {
                    totalStopwatch.Stop();
                    OvLogger.Info($"2. 读取模型 / Read model: {totalStopwatch.ElapsedMilliseconds}ms");

                    // 打印模型信息
                    OvExtensions.printf_model_info(model);

                    // Step 3: 编译模型
                    totalStopwatch.Restart();
                    using (var compiledModel = core.compile_model(model, device))
                    {
                        totalStopwatch.Stop();
                        OvLogger.Info($"3. 编译模型 / Compile model: {totalStopwatch.ElapsedMilliseconds}ms");

                        // Step 4: 创建推理请求
                        totalStopwatch.Restart();
                        using (var inferRequest = compiledModel.create_infer_request())
                        {
                            totalStopwatch.Stop();
                            OvLogger.Info($"4. 创建推理请求 / Create infer request: {totalStopwatch.ElapsedMilliseconds}ms");

                            // 处理单张图片（第一次，带详细输出）
                            if (File.Exists(imagePath))
                            {
                                OvLogger.Info("\n   首次推理（详细阶段）/ First inference (detailed stages):");
                                ProcessImageWithProfiler(inferRequest, imagePath, "同步推理结果 / Sync Result", profiler);

                                // 多次推理进行性能统计
                                int repeatCount = 10;
                                OvLogger.Info($"\n   连续推理 {repeatCount} 次进行统计 / Running {repeatCount} inferences for statistics:");

                                for (int i = 0; i < repeatCount; i++)
                                {
                                    ProcessImageWithProfiler(inferRequest, imagePath, null, profiler);
                                }

                                // 打印性能报告
                                profiler.PrintReport("推理性能统计报告 / Inference Performance Statistics");
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region 演示 2: 异步推理

        /// <summary>
        /// 异步推理演示 / Asynchronous inference demo
        /// </summary>
        static void DemoAsyncInference(string modelPath, string imagePath, string device)
        {
            using (var core = new Core())
            using (var model = core.read_model(modelPath))
            using (var compiledModel = core.compile_model(model, device))
            using (var inferRequest = compiledModel.create_infer_request())
            {
                // 设置回调函数
                bool inferenceComplete = false;
                inferRequest.set_callback(() =>
                {
                    OvLogger.Info("   异步推理完成回调 / Async inference callback triggered");
                    inferenceComplete = true;
                });

                if (!File.Exists(imagePath)) return;

                // 预处理图像
                var inputData = PreprocessImage(imagePath, new Size(640, 640), out float scale, out int offsetX, out int offsetY);

                // 设置输入数据
                using (var inputTensor = inferRequest.get_input_tensor())
                {
                    inputTensor.set_data(inputData);
                }

                // 启动异步推理
                OvLogger.Info("   启动异步推理 / Starting async inference...");
                inferenceComplete = false;
                inferRequest.start_async();

                // 等待推理完成（带超时）
                int waitCount = 0;
                while (!inferenceComplete && waitCount < 100)
                {
                    Thread.Sleep(10);
                    waitCount++;
                }

                if (inferenceComplete)
                {
                    // 获取结果
                    using (var outputTensor = inferRequest.get_output_tensor())
                    {
                        var results = Postprocess(outputTensor, scale, offsetX, offsetY);
                        OvLogger.Info($"   检测到 {results.Count} 个目标 / Detected {results.Count} objects");
                    }
                }

                // 清除回调
                inferRequest.set_callback(null);
            }
        }

        #endregion

        #region 演示 3: 批量推理

        /// <summary>
        /// 批量推理演示 / Batch inference demo
        /// </summary>
        static void DemoBatchInference(string modelPath, string imagePath, string device)
        {
            using (var core = new Core())
            using (var model = core.read_model(modelPath))
            using (var compiledModel = core.compile_model(model, device))
            {
                // 获取所有图片
                var imageFiles = Directory.GetFiles(Path.GetDirectoryName(imagePath), "*.jpg")
                    .Concat(Directory.GetFiles(Path.GetDirectoryName(imagePath), "*.png"))
                    .Take(4) // 最多处理4张
                    .ToArray();

                if (imageFiles.Length == 0)
                {
                    OvLogger.Info("   未找到图片 / No images found");
                    return;
                }

                OvLogger.Info($"   处理 {imageFiles.Length} 张图片 / Processing {imageFiles.Length} images");

                var stopwatch = Stopwatch.StartNew();

                // 串行处理
                foreach (var imageFile in imageFiles)
                {
                    using (var inferRequest = compiledModel.create_infer_request())
                    {
                        var inputData = PreprocessImage(imageFile, new Size(640, 640), out float scale, out int offsetX, out int offsetY);

                        using (var inputTensor = inferRequest.get_input_tensor())
                            inputTensor.set_data(inputData);

                        inferRequest.infer();

                        using (var outputTensor = inferRequest.get_output_tensor())
                        {
                            var results = Postprocess(outputTensor, scale, offsetX, offsetY);
                            OvLogger.Info($"   {Path.GetFileName(imageFile)}: {results.Count} 个目标 / objects");
                        }
                    }
                }

                stopwatch.Stop();
                OvLogger.Info($"   批量处理完成 / Batch processing completed: {stopwatch.ElapsedMilliseconds}ms");
                OvLogger.Info($"   平均每张 / Average per image: {stopwatch.ElapsedMilliseconds / imageFiles.Length}ms");
            }
        }

        #endregion

        #region 演示 4: 推理请求池

        /// <summary>
        /// 推理请求池演示 / Infer request pool demo
        /// </summary>
        static void DemoRequestPool(string modelPath, string imagePath, string device)
        {
            using (var core = new Core())
            using (var model = core.read_model(modelPath))
            using (var compiledModel = core.compile_model(model, device))
            {
                // 创建推理请求池
                using (var pool = new InferRequestPool(compiledModel, initialSize: 2, maxSize: 4))
                {
                    OvLogger.Info($"   池大小 / Pool size: {pool.Count}");

                    var imageFiles = Directory.GetFiles(Path.GetDirectoryName(imagePath), "*.jpg")
                        .Concat(Directory.GetFiles(Path.GetDirectoryName(imagePath), "*.png"))
                        .Take(6)
                        .ToArray();

                    var stopwatch = Stopwatch.StartNew();

                    // 并行处理（使用线程池）
                    Parallel.ForEach(imageFiles, new ParallelOptions { MaxDegreeOfParallelism = 4 }, imageFile =>
                    {
                        // 从池中获取请求
                        var request = pool.Rent();
                        try
                        {
                            var inputData = PreprocessImage(imageFile, new Size(640, 640), out float scale, out int offsetX, out int offsetY);

                            using (var inputTensor = request.get_input_tensor())
                                inputTensor.set_data(inputData);

                            request.infer();

                            using (var outputTensor = request.get_output_tensor())
                            {
                                var results = Postprocess(outputTensor, scale, offsetX, offsetY);
                                OvLogger.Info($"   {Path.GetFileName(imageFile)}: {results.Count} 个目标");
                            }
                        }
                        finally
                        {
                            // 归还请求到池中
                            pool.Return(request);
                        }
                    });

                    stopwatch.Stop();
                    OvLogger.Info($"   使用对象池处理完成 / Pool processing completed: {stopwatch.ElapsedMilliseconds}ms");
                }
            }
        }

        #endregion

        #region 演示 5: 性能分析

        /// <summary>
        /// 性能分析演示 / Performance profiling demo
        /// </summary>
        static void DemoProfiling(string modelPath, string imagePath, string device)
        {
            // 启用性能分析需要设置属性
            using (var core = new Core())
            {
                // 设置性能分析模式
                core.set_property(device, "PERF_COUNT", "YES");

                using (var model = core.read_model(modelPath))
                using (var compiledModel = core.compile_model(model, device))
                using (var inferRequest = compiledModel.create_infer_request())
                {
                    if (!File.Exists(imagePath)) return;

                    // 预热
                    for (int i = 0; i < 3; i++)
                    {
                        var inputData = PreprocessImage(imagePath, new Size(640, 640), out float scale, out int offsetX, out int offsetY);
                        using (var inputTensor = inferRequest.get_input_tensor())
                            inputTensor.set_data(inputData);
                        inferRequest.infer();
                    }

                    // 正式推理并获取性能数据
                    {
                        var inputData = PreprocessImage(imagePath, new Size(640, 640), out float scale, out int offsetX, out int offsetY);
                        using (var inputTensor = inferRequest.get_input_tensor())
                            inputTensor.set_data(inputData);

                        var sw = Stopwatch.StartNew();
                        inferRequest.infer();
                        sw.Stop();

                        OvLogger.Info($"   推理时间 / Inference time: {sw.ElapsedMilliseconds}ms");

                        // 获取详细性能分析
                        try
                        {
                            var profilingInfo = inferRequest.get_profiling_info();
                            if (profilingInfo != null && profilingInfo.Length > 0)
                            {
                                OvLogger.Info($"   层数 / Layer count: {profilingInfo.Length}");

                                // 显示前5个最耗时的层
                                var topLayers = profilingInfo
                                    .OrderByDescending(p => p.real_time)
                                    .Take(5);

                                foreach (var info in topLayers)
                                {
                                    OvLogger.Info($"   - {info.node_name}: {info.real_time}μs ({info.exec_type})");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            OvLogger.Info($"   性能分析不可用 / Profiling not available: {ex.Message}");
                        }
                    }
                }
            }
        }

        #endregion

        #region 演示 6: 模型缓存 (OpenVINO CACHE_DIR)

        /// <summary>
        /// 模型缓存演示 - 使用 OpenVINO 原生磁盘缓存 / Model caching demo - Using OpenVINO native disk cache
        /// <para>
        /// 通过设置 CACHE_DIR 属性，OpenVINO 会自动将编译后的模型缓存到磁盘，
        /// 下次加载时可显著减少编译时间。
        /// </para>
        /// </summary>
        static void DemoModelCache(string modelPath, string imageDir, string device)
        {
            string cacheDir = "./model_cache";
            Directory.CreateDirectory(cacheDir);

            // 第一次加载（无缓存）
            OvLogger.Info("   第一次加载模型（磁盘无缓存）/ First load (no disk cache):");
            var sw = Stopwatch.StartNew();

            using (var core1 = new Core())
            {
                // 设置缓存目录
                core1.set_property(device, "CACHE_DIR", cacheDir);
                using (var model = core1.read_model(modelPath))
                using (var compiledModel = core1.compile_model(model, device))
                {
                    sw.Stop();
                    OvLogger.Info($"   编译时间 / Compile time: {sw.ElapsedMilliseconds}ms");
                }
            }

            // 第二次加载（有磁盘缓存）
            OvLogger.Info("   第二次加载模型（磁盘有缓存）/ Second load (with disk cache):");
            sw.Restart();

            using (var core2 = new Core())
            {
                core2.set_property(device, "CACHE_DIR", cacheDir);
                using (var model = core2.read_model(modelPath))
                using (var compiledModel = core2.compile_model(model, device))
                {
                    sw.Stop();
                    OvLogger.Info($"   编译时间 / Compile time: {sw.ElapsedMilliseconds}ms");
                }
            }

            // 显示缓存文件
            var cacheFiles = Directory.GetFiles(cacheDir, "*", SearchOption.AllDirectories);
            OvLogger.Info($"   缓存文件数量 / Cache file count: {cacheFiles.Length}");

            // 清理缓存
            try
            {
                Directory.Delete(cacheDir, true);
                OvLogger.Info("   磁盘缓存已清理 / Disk cache cleaned");
            }
            catch { }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 处理单张图片 / Process single image
        /// </summary>
        static void ProcessImage(InferRequest inferRequest, string imagePath, string windowName)
        {
            ProcessImageWithProfiler(inferRequest, imagePath, windowName, null);
        }

        /// <summary>
        /// 处理单张图片（带性能分析）/ Process single image with profiling
        /// </summary>
        static void ProcessImageWithProfiler(InferRequest inferRequest, string imagePath, string windowName, InferenceProfiler profiler)
        {
            var totalStopwatch = Stopwatch.StartNew();
            var stageStopwatch = new Stopwatch();

            // 阶段 1: 图像加载 / Stage 1: Image loading
            stageStopwatch.Restart();
            profiler?.Start("图像加载 / Image Load");
            using (var image = Cv2.ImRead(imagePath))
            {
                stageStopwatch.Stop();
                double loadTime = stageStopwatch.Elapsed.TotalMilliseconds;

                profiler?.Stop();
                OvLogger.Info($"   [1] 图像加载 / Image Load: {loadTime:F3}ms");
                profiler?.Start("预处理 / Preprocess");


                // 阶段 2: 预处理 / Stage 2: Preprocessing
                stageStopwatch.Restart();
                var inputData = PreprocessImageWithDetail(image, new Size(640, 640), out double resizeTime, out double convertTime, out double normalizeTime, out float scale, out int offsetX, out int offsetY);
                stageStopwatch.Stop();
                double preprocessTime = stageStopwatch.Elapsed.TotalMilliseconds;

                profiler?.Stop();
                OvLogger.Info($"   [2] 预处理 / Preprocess: {preprocessTime:F3}ms (缩放/Resize: {resizeTime:F3}ms, 转换/Convert: {convertTime:F3}ms, 归一化/Norm: {normalizeTime:F3}ms)");
                profiler?.Start("设置输入 / Set Input");

                // 阶段 3: 设置输入数据 / Stage 3: Set input data
                stageStopwatch.Restart();
                using (var inputTensor = inferRequest.get_input_tensor())
                {
                    inputTensor.set_data(inputData);
                }
                stageStopwatch.Stop();
                double setInputTime = stageStopwatch.Elapsed.TotalMilliseconds;

                profiler?.Stop();
                OvLogger.Info($"   [3] 设置输入 / Set Input: {setInputTime:F3}ms");
                profiler?.Start("推理 / Inference");


                // 阶段 4: 推理 / Stage 4: Inference
                stageStopwatch.Restart();
                inferRequest.infer();
                stageStopwatch.Stop();
                double inferenceTime = stageStopwatch.Elapsed.TotalMilliseconds;

                profiler?.Stop();
                OvLogger.Info($"   [4] 推理 / Inference: {inferenceTime:F3}ms");
                profiler?.Start("获取输出 / Get Output");


                // 阶段 5: 获取输出 / Stage 5: Get output
                stageStopwatch.Restart();
                using (var outputTensor = inferRequest.get_output_tensor())
                {
                    stageStopwatch.Stop();
                    double getOutputTime = stageStopwatch.Elapsed.TotalMilliseconds;

                    profiler?.Stop();
                    OvLogger.Info($"   [5] 获取输出 / Get Output: {getOutputTime:F3}ms");
                    profiler?.Start("后处理 / Postprocess");


                    // 阶段 6: 后处理 / Stage 6: Postprocessing
                    stageStopwatch.Restart();
                    var results = Postprocess(outputTensor, scale, offsetX, offsetY);
                    stageStopwatch.Stop();
                    double postprocessTime = stageStopwatch.Elapsed.TotalMilliseconds;

                    profiler?.Stop();
                    OvLogger.Info($"   [6] 后处理 / Postprocess: {postprocessTime:F3}ms");
                    profiler?.Start("绘制结果 / Draw Results");


                    // 阶段 7: 结果绘制 / Stage 7: Draw results
                    stageStopwatch.Restart();
                    DrawResults(image, results);
                    stageStopwatch.Stop();
                    double drawTime = stageStopwatch.Elapsed.TotalMilliseconds;

                    profiler?.Stop();
                    OvLogger.Info($"   [7] 绘制结果 / Draw Results: {drawTime:F3}ms");

                    // 总时间
                    totalStopwatch.Stop();
                    double totalTime = totalStopwatch.Elapsed.TotalMilliseconds;
                    OvLogger.Info($"   ────────────────────────────────────────");
                    OvLogger.Info($"   [总计] 检测到 {results.Count} 个目标 / Total: {totalTime:F3}ms, {results.Count} objects detected");

                    // 计算各阶段占比
                    OvLogger.Info($"   阶段占比 / Stage breakdown:");
                    OvLogger.Info($"     - 预处理 / Preprocess: {(preprocessTime / totalTime) * 100:F1}%");
                    OvLogger.Info($"     - 推理 / Inference: {(inferenceTime / totalTime) * 100:F1}%");
                    OvLogger.Info($"     - 后处理 / Postprocess: {(postprocessTime / totalTime) * 100:F1}%");

                    // 显示结果（仅在指定窗口名称时）
                    if (!string.IsNullOrEmpty(windowName))
                    {
                        Cv2.ImShow(windowName, image);
                        Cv2.WaitKey(500);
                    }
                }
            }
        }

        /// <summary>
        /// 图片预处理 / Image preprocessing
        /// </summary>
        static float[] PreprocessImage(string imagePath, Size targetSize, out float scale, out int offsetX, out int offsetY)
        {
            using (var image = Cv2.ImRead(imagePath))
            {
                if (image.Empty())
                    throw new FileNotFoundException($"图片不存在 / Image not found: {imagePath}");

                // 计算缩放比例
                scale = Math.Min(
                    (float)targetSize.Width / image.Width,
                    (float)targetSize.Height / image.Height);

                var scaledSize = new Size(
                    (int)(image.Width * scale),
                    (int)(image.Height * scale));

                // 缩放图片
                using (var resized = new Mat())
                {
                    Cv2.Resize(image, resized, scaledSize);

                    // 创建目标图像（黑色填充）
                    using (var output = new Mat(targetSize.Height, targetSize.Width, MatType.CV_8UC3, Scalar.Black))
                    {
                        // 计算居中偏移
                        offsetX = (targetSize.Width - scaledSize.Width) / 2;
                        offsetY = (targetSize.Height - scaledSize.Height) / 2;

                        // 复制到目标图像
                        using (var roi = new Mat(output, new Rect(offsetX, offsetY, scaledSize.Width, scaledSize.Height)))
                        {
                            resized.CopyTo(roi);
                        }

                        // 转换为浮点并归一化
                        output.ConvertTo(output, MatType.CV_32FC3, 1.0 / 255.0);

                        // HWC to CHW
                        int channels = 3;
                        int height = output.Rows;
                        int width = output.Cols;
                        float[] result = new float[channels * height * width];

                        GCHandle handle = GCHandle.Alloc(result, GCHandleType.Pinned);
                        try
                        {
                            IntPtr ptr = handle.AddrOfPinnedObject();
                            for (int c = 0; c < channels; c++)
                            {
                                using (var channel = Mat.FromPixelData(height, width, MatType.CV_32FC1, ptr + c * height * width * sizeof(float)))
                                {
                                    Cv2.ExtractChannel(output, channel, c);
                                }
                            }
                        }
                        finally
                        {
                            handle.Free();
                        }

                        return result;
                    }
                }
            }
        }

        /// <summary>
        /// 图片预处理（详细时间统计）/ Image preprocessing with detailed timing
        /// </summary>
        static float[] PreprocessImageWithDetail(Mat image, Size targetSize, out double resizeTime, out double convertTime, out double normalizeTime, out float scale, out int offsetX, out int offsetY)
        {
            var stopwatch = new Stopwatch();
            resizeTime = convertTime = normalizeTime = 0;

            // 计算缩放比例
            scale = Math.Min(
                (float)targetSize.Width / image.Width,
                (float)targetSize.Height / image.Height);

            var scaledSize = new Size(
                (int)(image.Width * scale),
                (int)(image.Height * scale));

            // 缩放图片
            stopwatch.Restart();
            using (var resized = new Mat())
            {
                Cv2.Resize(image, resized, scaledSize);
                stopwatch.Stop();
                resizeTime = stopwatch.Elapsed.TotalMilliseconds;

                // 创建目标图像（黑色填充）
                using (var output = new Mat(targetSize.Height, targetSize.Width, MatType.CV_8UC3, Scalar.Black))
                {
                    // 计算居中偏移
                    offsetX = (targetSize.Width - scaledSize.Width) / 2;
                    offsetY = (targetSize.Height - scaledSize.Height) / 2;

                    // 复制到目标图像
                    using (var roi = new Mat(output, new Rect(offsetX, offsetY, scaledSize.Width, scaledSize.Height)))
                    {
                        resized.CopyTo(roi);
                    }

                    // 转换为浮点并归一化
                    stopwatch.Restart();
                    output.ConvertTo(output, MatType.CV_32FC3, 1.0 / 255.0);
                    stopwatch.Stop();
                    convertTime = stopwatch.Elapsed.TotalMilliseconds;

                    // HWC to CHW
                    stopwatch.Restart();
                    int channels = 3;
                    int height = output.Rows;
                    int width = output.Cols;
                    float[] result = new float[channels * height * width];

                    GCHandle handle = GCHandle.Alloc(result, GCHandleType.Pinned);
                    try
                    {
                        IntPtr ptr = handle.AddrOfPinnedObject();
                        for (int c = 0; c < channels; c++)
                        {
                            using (var channel = Mat.FromPixelData(height, width, MatType.CV_32FC1, ptr + c * height * width * sizeof(float)))
                            {
                                Cv2.ExtractChannel(output, channel, c);
                            }
                        }
                    }
                    finally
                    {
                        handle.Free();
                    }
                    stopwatch.Stop();
                    normalizeTime = stopwatch.Elapsed.TotalMilliseconds;

                    return result;
                }
            }
        }

        /// <summary>
        /// 后处理 / Postprocessing
        /// </summary>
        static List<DetectionResult> Postprocess(Tensor outputTensor, float scale, int offsetX, int offsetY)
        {
            var results = new List<DetectionResult>();

            // 获取输出数据
            int outputLength = (int)outputTensor.size;
            float[] outputData = outputTensor.get_data<float>(outputLength);

            // 获取输出形状
            Shape outputShape = outputTensor.shape;
            int numDetections = (int)outputShape[1];
            int detectionLength = (int)outputShape[2];

            // 解析检测结果
            for (int i = 0; i < numDetections; i++)
            {
                int idx = i * detectionLength;
                float confidence = outputData[idx + 4];

                // 过滤低置信度
                if (confidence < 0.25f)
                    continue;

                // 解析坐标
                float x1 = (outputData[idx + 0] - offsetX) / scale;
                float y1 = (outputData[idx + 1] - offsetY) / scale;
                float x2 = (outputData[idx + 2] - offsetX) / scale;
                float y2 = (outputData[idx + 3] - offsetY) / scale;

                int classId = (int)outputData[idx + 5];

                results.Add(new DetectionResult
                {
                    Bounds = new Rect((int)x1, (int)y1, (int)(x2 - x1), (int)(y2 - y1)),
                    Confidence = confidence,
                    Category = classId,
                    Label = classId < COCO_CLASSES.Length ? COCO_CLASSES[classId] : $"class_{classId}"
                });
            }

            // NMS (Non-Maximum Suppression)
            results = ApplyNMS(results, 0.5f);

            return results;
        }

        /// <summary>
        /// 非极大值抑制 / Non-Maximum Suppression
        /// </summary>
        static List<DetectionResult> ApplyNMS(List<DetectionResult> detections, float iouThreshold)
        {
            var sorted = detections.OrderByDescending(d => d.Confidence).ToList();
            var keep = new List<DetectionResult>();

            while (sorted.Count > 0)
            {
                var current = sorted[0];
                keep.Add(current);
                sorted.RemoveAt(0);

                sorted.RemoveAll(d => CalculateIoU(current.Bounds, d.Bounds) > iouThreshold);
            }

            return keep;
        }

        /// <summary>
        /// 计算 IoU / Calculate Intersection over Union
        /// </summary>
        static float CalculateIoU(Rect a, Rect b)
        {
            int x1 = Math.Max(a.X, b.X);
            int y1 = Math.Max(a.Y, b.Y);
            int x2 = Math.Min(a.X + a.Width, b.X + b.Width);
            int y2 = Math.Min(a.Y + a.Height, b.Y + b.Height);

            if (x2 <= x1 || y2 <= y1)
                return 0;

            float intersection = (x2 - x1) * (y2 - y1);
            float areaA = a.Width * a.Height;
            float areaB = b.Width * b.Height;

            return intersection / (areaA + areaB - intersection);
        }

        /// <summary>
        /// 绘制检测结果 / Draw detection results
        /// </summary>
        static void DrawResults(Mat image, List<DetectionResult> results)
        {
            // 随机颜色
            var random = new Random(42);
            var colors = new Dictionary<int, Scalar>();

            foreach (var result in results)
            {
                // 获取或生成颜色
                if (!colors.ContainsKey(result.Category))
                {
                    colors[result.Category] = new Scalar(
                        random.Next(0, 256),
                        random.Next(0, 256),
                        random.Next(0, 256));
                }
                var color = colors[result.Category];

                // 绘制边界框
                Cv2.Rectangle(image, result.Bounds, color, 2);

                // 绘制标签背景
                string label = $"{result.Label}: {result.Confidence:F2}";
                int baseline;
                var textSize = Cv2.GetTextSize(label, HersheyFonts.HersheySimplex, 0.5, 1, out baseline);
                var labelRect = new Rect(
                    result.Bounds.X,
                    result.Bounds.Y - textSize.Height - 5,
                    textSize.Width + 10,
                    textSize.Height + 10);

                // 确保标签不超出图像边界
                if (labelRect.Y < 0)
                {
                    labelRect.Y = result.Bounds.Y + result.Bounds.Height;
                }

                Cv2.Rectangle(image, labelRect, color, -1);
                Cv2.PutText(image, label,
                    new Point(labelRect.X + 5, labelRect.Y + textSize.Height + 5),
                    HersheyFonts.HersheySimplex, 0.5, Scalar.White, 1);
            }
        }

        #endregion
    }


    /// <summary>
    /// 推理性能分析器 / Inference performance profiler
    /// </summary>
    public class InferenceProfiler
    {
        private readonly Dictionary<string, List<double>> _timings = new Dictionary<string, List<double>>();
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private string _currentStage;

        /// <summary>
        /// 开始记录阶段 / Start recording stage
        /// </summary>
        public void Start(string stageName)
        {
            if (_stopwatch.IsRunning)
            {
                Stop();
            }
            _currentStage = stageName;
            _stopwatch.Restart();
        }

        /// <summary>
        /// 停止当前阶段 / Stop current stage
        /// </summary>
        public void Stop()
        {
            if (_stopwatch.IsRunning)
            {
                _stopwatch.Stop();
                double elapsedMs = _stopwatch.Elapsed.TotalMilliseconds;

                if (!_timings.ContainsKey(_currentStage))
                {
                    _timings[_currentStage] = new List<double>();
                }
                _timings[_currentStage].Add(elapsedMs);
            }
        }

        /// <summary>
        /// 重置所有记录 / Reset all records
        /// </summary>
        public void Reset()
        {
            _timings.Clear();
            _stopwatch.Reset();
        }

        /// <summary>
        /// 获取阶段平均时间 / Get stage average time
        /// </summary>
        public double GetAverage(string stageName)
        {
            if (_timings.TryGetValue(stageName, out var timings) && timings.Count > 0)
            {
                return timings.Average();
            }
            return 0;
        }

        /// <summary>
        /// 获取阶段总时间 / Get stage total time
        /// </summary>
        public double GetTotal(string stageName)
        {
            if (_timings.TryGetValue(stageName, out var timings))
            {
                return timings.Sum();
            }
            return 0;
        }

        /// <summary>
        /// 获取阶段调用次数 / Get stage call count
        /// </summary>
        public int GetCount(string stageName)
        {
            if (_timings.TryGetValue(stageName, out var timings))
            {
                return timings.Count;
            }
            return 0;
        }

        /// <summary>
        /// 打印详细报告 / Print detailed report
        /// </summary>
        public void PrintReport(string title = "推理性能报告 / Inference Performance Report")
        {
            Console.WriteLine("\n" + new string('=', 70));
            Console.WriteLine($"  {title}");
            Console.WriteLine(new string('=', 70));
            Console.WriteLine($"  {"阶段 / Stage",-20} {"次数",-8} {"平均(ms)",-12} {"总计(ms)",-12} {"占比 %",-10}");
            Console.WriteLine(new string('-', 70));

            double totalTime = _timings.Values.SelectMany(t => t).Sum();

            foreach (var kvp in _timings)
            {
                string stage = kvp.Key;
                int count = kvp.Value.Count;
                double avg = kvp.Value.Average();
                double sum = kvp.Value.Sum();
                double percentage = totalTime > 0 ? (sum / totalTime) * 100 : 0;

                Console.WriteLine($"  {stage,-20} {count,-8} {avg,12:F3} {sum,12:F3} {percentage,10:F1}");
            }

            Console.WriteLine(new string('-', 70));
            Console.WriteLine($"  {"总计 / Total",-20} {" ",-8} {" ",-12} {totalTime,12:F3} {"100.0",-10}");
            Console.WriteLine(new string('=', 70));
        }

        /// <summary>
        /// 获取所有阶段名称 / Get all stage names
        /// </summary>
        public IEnumerable<string> GetStageNames()
        {
            return _timings.Keys;
        }
    }

}
