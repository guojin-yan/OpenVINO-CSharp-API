# YOLO 目标检测 - .NET Core 3.1

## 简介 / Introduction

本案例展示如何在 .NET Core 3.1 中使用 OpenVINO C# API 进行 YOLO 目标检测。.NET Core 3.1 是长期支持(LTS)版本，提供了 `Span<T>`、`Memory<T>`、`IAsyncEnumerable<T>` 和 `Index/Range` 等现代特性。

## 框架特性 / Framework Features

- ✅ **Span<T>** - 零拷贝内存操作
- ✅ **Memory<T>** - 安全的内存抽象
- ✅ **async/await** - 完整的异步编程模型
- ✅ **IAsyncEnumerable<T>** - 异步流处理
- ✅ **Index/Range** - 索引和范围语法糖
- ✅ **NativeLibrary** - 原生库加载API
- ❌ **Parallel.ForEachAsync** - 需要 .NET 6.0+

## 代码示例 / Code Example

### 同步推理与 Span<T>

```csharp
using var core = new Core();
var model = core.compile_model("model.xml", "CPU");
using var request = model.create_infer_request();

// 使用 Span<T> 高效处理输入数据
Span<float> inputData = stackalloc float[3 * 640 * 640];
FillInputData(inputData);

using var inputTensor = new Tensor(shape, inputData);
request.set_input_tensor(inputTensor);

// 执行推理
request.infer();

// 使用 Span 获取输出，避免数组分配
var output = request.get_output_tensor();
Span<float> results = output.get_data_span<float>();
ProcessResults(results);
```

### 异步流处理 (IAsyncEnumerable)

```csharp
// 使用 IAsyncEnumerable 处理批量图片
await foreach (var detection in DetectObjectsAsync(request, imageFiles))
{
    Console.WriteLine($"检测到 {detection.Count} 个目标");
}

async IAsyncEnumerable<List<Detection>> DetectObjectsAsync(
    InferRequest request,
    IEnumerable<string> imagePaths,
    [EnumeratorCancellation] CancellationToken ct = default)
{
    foreach (var path in imagePaths)
    {
        ct.ThrowIfCancellationRequested();
        
        // 异步预处理
        await Task.Run(() => Preprocess(path, request), ct);
        
        // 异步推理
        request.start_async();
        await Task.Run(() => request.wait(), ct);
        
        yield return Postprocess(request.get_output_tensor());
    }
}
```

### 使用 Index/Range 语法

```csharp
// C# 8.0 Index/Range 语法
var output = request.get_output_tensor();
Span<float> data = output.get_data_span<float>();

// 使用 ^ 运算符从末尾索引
var lastElement = data[^1];

// 使用范围语法切片
var firstBatch = data[..1000];
var middleBatch = data[1000..2000];
var lastBatch = data[^1000..];
```

### 零拷贝 Tensor 操作

```csharp
// 使用 Memory<T> 进行安全的内存共享
async Task ProcessWithMemory(InferRequest request, Memory<float> inputData)
{
    // Memory<T> 可以安全地跨异步方法传递
    using var inputTensor = new Tensor(shape, inputData.Span);
    request.set_input_tensor(inputTensor);
    
    request.start_async();
    await Task.Run(() => request.wait());
    
    // 输出也可以直接使用 Memory
    var output = request.get_output_tensor();
    Memory<float> outputData = output.get_data_memory<float>();
    await ProcessResultsAsync(outputData);
}
```

## 性能优化 / Performance Optimization

### Span<T> vs 数组

```csharp
// 高效：stackalloc + Span
void FastPreprocess(byte[] imageData)
{
    Span<float> normalized = stackalloc float[imageData.Length];
    for (int i = 0; i < imageData.Length; i++)
    {
        normalized[i] = imageData[i] / 255.0f;
    }
    using var tensor = new Tensor(shape, normalized);
}

// 低效：堆分配数组
void SlowPreprocess(byte[] imageData)
{
    float[] normalized = new float[imageData.Length];
    for (int i = 0; i < imageData.Length; i++)
    {
        normalized[i] = imageData[i] / 255.0f;
    }
    using var tensor = new Tensor(shape, normalized);
}
```

### 批量并行处理

```csharp
// 使用 Task.WhenAll 进行并行推理
async Task ProcessBatchAsync(string[] imageFiles)
{
    var requests = Enumerable.Range(0, 4)
        .Select(_ => model.create_infer_request())
        .ToList();
    
    var tasks = imageFiles.Select((file, index) => 
        ProcessSingleAsync(requests[index % requests.Count], file));
    
    await Task.WhenAll(tasks);
}

async Task ProcessSingleAsync(InferRequest request, string file)
{
    Preprocess(file, request);
    request.start_async();
    await Task.Run(() => request.wait());
    SaveResults(file, request.get_output_tensor());
}
```

## 跨平台运行 / Cross-Platform

.NET Core 3.1 支持 Windows、Linux 和 macOS：

```bash
# Windows
dotnet run

# Linux (Ubuntu 20.04)
dotnet run

# macOS
dotnet run
```

## 项目文件 / Project File

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <LangVersion>8.0</LangVersion>
  </PropertyGroup>
  
  <ItemGroup>
    <PackageReference Include="JYPPX.OpenVINO.CSharp.API" Version="3.3.1" />
    <PackageReference Include="OpenCvSharp4" Version="4.6.0" />
    <PackageReference Include="OpenCvSharp4.runtime.ubuntu.20.04-x64" 
                      Version="4.6.0" Condition="$([MSBuild]::IsOSPlatform('Linux'))" />
    <PackageReference Include="OpenCvSharp4.runtime.win" 
                      Version="4.6.0" Condition="$([MSBuild]::IsOSPlatform('Windows'))" />
  </ItemGroup>
</Project>
```

## 运行案例 / Running the Sample

```bash
# 进入项目目录
cd samples/Yolo26Det-netcoreapp3.1

# 运行
dotnet run

# 发布独立部署（包含运行时）
dotnet publish -c Release -r win-x64 --self-contained true
dotnet publish -c Release -r linux-x64 --self-contained true
```

## 与 .NET 5+ 的区别 / Differences from .NET 5+

| 特性 | .NET Core 3.1 | .NET 5.0+ |
|------|---------------|-----------|
| Span<T> | ✅ | ✅ |
| IAsyncEnumerable | ✅ | ✅ |
| NativeLibrary | ✅ | ✅ |
| Parallel.ForEachAsync | ❌ | ✅ (.NET 6+) |
| 性能 | 优秀 | 更优秀 |

## 相关链接 / See Also

- [.NET Core 3.1 文档](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-core-3-1)
- [C# 8.0 新特性](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-8)
- [Span<T> 编程指南](https://learn.microsoft.com/dotnet/standard/memory-and-spans/)
- [API 参考](../../api/OpenVinoSharp.yml)
