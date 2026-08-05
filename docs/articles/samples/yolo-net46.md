# YOLO 目标检测 - .NET Framework 4.6

## 简介 / Introduction

本案例展示如何在 .NET Framework 4.6 中使用 OpenVINO C# API 进行 YOLO 目标检测。.NET Framework 4.6 是较旧的框架版本，使用传统的异步模式（回调 + 轮询），不支持 `Span<T>` 和 `Memory<T>`。

## 框架特性 / Framework Features

- ❌ **Span<T>** - 需要 .NET Core 2.1+ 或 .NET Framework 4.7.2+
- ❌ **Memory<T>** - 需要 .NET Core 2.1+ 或 .NET Framework 4.7.2+
- ✅ **async/await** - 完整的异步编程模型支持
- ❌ **IAsyncEnumerable<T>** - 需要 .NET Core 3.0+
- ❌ **NativeLibrary** - 需要 .NET Core 3.0+

## 代码示例 / Code Example

### 同步推理

```csharp
using var core = new Core();
var model = core.compile_model("model.xml", "CPU");
using var request = model.create_infer_request();

// 准备输入数据
float[] inputData = LoadInputData();
using var inputTensor = new Tensor(shape, inputData);
request.set_input_tensor(inputTensor);

// 执行推理
request.infer();

// 获取输出
var output = request.get_output_tensor();
float[] results = output.get_data<float>((int)output.size);
```

### 传统异步模式（回调 + 轮询）

```csharp
// .NET 4.6 使用回调和轮询实现异步
public void RunInferenceAsync(string imagePath, Action<List<Detection>> callback)
{
    var request = model.create_infer_request();
    Preprocess(imagePath, request);
    
    bool completed = false;
    
    // 设置回调
    request.set_callback((InferRequest req, UserContext ctx) =>
    {
        var results = Postprocess(req.get_output_tensor());
        callback(results);
        completed = true;
    });
    
    // 启动异步推理
    request.start_async();
    
    // 轮询等待完成
    while (!completed)
    {
        Thread.Sleep(10);
    }
}
```

### 使用 TaskCompletionSource 包装回调

```csharp
// 将回调式 API 包装为 async/await
public Task<List<Detection>> RunInferenceAsync(string imagePath)
{
    var tcs = new TaskCompletionSource<List<Detection>>();
    
    using var request = model.create_infer_request();
    Preprocess(imagePath, request);
    
    request.set_callback((InferRequest req, UserContext ctx) =>
    {
        try
        {
            var results = Postprocess(req.get_output_tensor());
            tcs.SetResult(results);
        }
        catch (Exception ex)
        {
            tcs.SetException(ex);
        }
    });
    
    request.start_async();
    
    return tcs.Task;
}
```

## 图片预处理

```csharp
static void PreprocessImage(string imagePath, Size targetSize,
    out float[] data, out float scale, out int offsetX, out int offsetY)
{
    using (var image = Cv2.ImRead(imagePath))
    {
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
            using (var output = new Mat(targetSize.Height, targetSize.Width, 
                MatType.CV_8UC3, Scalar.Black))
            {
                // 计算居中偏移
                offsetX = (targetSize.Width - scaledSize.Width) / 2;
                offsetY = (targetSize.Height - scaledSize.Height) / 2;

                // 复制到目标图像
                using (var roi = new Mat(output, new Rect(offsetX, offsetY, 
                    scaledSize.Width, scaledSize.Height)))
                {
                    resized.CopyTo(roi);
                }

                // 转换为浮点并归一化
                output.ConvertTo(output, MatType.CV_32FC3, 1.0 / 255.0);

                // HWC to CHW
                data = MatToChw(output);
            }
        }
    }
}
```

## 项目文件 / Project File

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net461</TargetFramework>
    <LangVersion>7.3</LangVersion>
  </PropertyGroup>
  
  <ItemGroup>
    <PackageReference Include="JYPPX.OpenVINO.CSharp.API" Version="3.3.1" />
    <!-- 使用 4.5.3 版本支持 net461 -->
    <PackageReference Include="OpenCvSharp4" Version="4.5.3.20211228" />
    <PackageReference Include="OpenCvSharp4.runtime.win" Version="4.5.3.20211228" />
  </ItemGroup>
</Project>
```

## 运行案例 / Running the Sample

```bash
# 进入项目目录
cd samples/Yolo26Det-net4.6

# 运行
dotnet run

# 或指定模型和图片路径
dotnet run -- --model ../../model/yolo26n.xml --image ../../images/bus.jpg
```

## 注意事项 / Notes

- .NET Framework 4.6 仅支持 Windows 平台
- 不支持 Span<T> 和 Memory<T>，使用传统数组操作
- 异步推理使用回调 + 轮询模式
- 需要较旧版本的 OpenCvSharp (4.5.3) 以兼容 net461

## 升级建议 / Upgrade Recommendation

如果可能，建议升级到 .NET Framework 4.8 或 .NET 6.0+：

| 特性 | .NET 4.6 | .NET 4.8 | .NET 6.0+ |
|------|----------|----------|-----------|
| Span<T> | ❌ | ✅ | ✅ |
| Memory<T> | ❌ | ✅ | ✅ |
| 性能 | 基准 | +20% | +50% |
| 跨平台 | ❌ | ❌ | ✅ |

## 相关链接 / See Also

- [.NET Framework 4.6 文档](https://learn.microsoft.com/dotnet/framework/migration-guide/versions-and-dependencies#net-framework-46)
- [async/await 最佳实践](https://learn.microsoft.com/dotnet/csharp/asynchronous-programming/async-scenarios)
- [API 参考](../../api/OpenVinoSharp.yml)
