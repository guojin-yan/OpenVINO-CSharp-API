# YOLO 目标检测 - .NET 10.0

## 简介 / Introduction

本案例展示如何在 .NET 10.0 中使用 OpenVINO C# API 进行 YOLO 目标检测。.NET 10.0 提供了最新的语言特性和性能优化，包括 `Span<T>`、`IAsyncEnumerable<T>` 和 `Parallel.ForEachAsync` 等。

## 框架特性 / Framework Features

- ✅ **Span<T>** - 零拷贝内存操作
- ✅ **Memory<T>** - 安全的内存抽象
- ✅ **async/await** - 异步编程模型
- ✅ **IAsyncEnumerable<T>** - 异步流处理
- ✅ **Parallel.ForEachAsync** - 并行异步循环
- ✅ **NativeLibrary** - 原生库加载
- ✅ **Index/Range** - 索引和范围语法

## 代码示例 / Code Example

### 同步推理

```csharp
using var core = new Core();
var model = core.compile_model("model.xml", "CPU");
using var request = model.create_infer_request();

// 使用 Span<T> 设置输入数据
ReadOnlySpan<float> inputData = GetInputData();
using var inputTensor = new Tensor(shape, inputData);
request.set_input_tensor(inputTensor);

// 执行推理
request.infer();

// 获取输出
var output = request.get_output_tensor();
Span<float> results = output.get_float_data_span();
```

### 异步推理

```csharp
// 使用 IAsyncEnumerable 处理异步流
await foreach (var result in RunInferenceAsync(request, imageFiles))
{
    Console.WriteLine($"检测到 {result.Count} 个目标");
}

async IAsyncEnumerable<List<Detection>> RunInferenceAsync(
    InferRequest request, 
    string[] images,
    [EnumeratorCancellation] CancellationToken ct = default)
{
    foreach (var image in images)
    {
        ct.ThrowIfCancellationRequested();
        
        Preprocess(image, request);
        await request.infer_async();
        await request.wait_async();
        
        yield return Postprocess(request.get_output_tensor());
    }
}
```

### 并行批量处理

```csharp
// 使用 Parallel.ForEachAsync 进行并行异步推理
await Parallel.ForEachAsync(imageFiles, async (imageFile, ct) =>
{
    using var request = model.create_infer_request();
    Preprocess(imageFile, request);
    await request.infer_async();
    var results = Postprocess(request.get_output_tensor());
    SaveResults(imageFile, results);
});
```

## 性能优化 / Performance Optimization

### 使用 Span<T> 避免内存拷贝

```csharp
// 高效：直接使用 Span
Span<float> data = stackalloc float[3 * 640 * 640];
FillInputData(data);
using var tensor = new Tensor(shape, data);

// 低效：数组拷贝
float[] data = new float[3 * 640 * 640];
FillInputData(data);
using var tensor = new Tensor(shape, data); // 内部再次拷贝
```

### 对象池复用

```csharp
// 创建推理请求池
using var pool = new InferRequestPool(model, initialSize: 4, maxSize: 16);

// 使用对象池
await pool.RunInferenceAsync(
    setInput: (request, state) =>
    {
        Preprocess(state.ImagePath, request);
        return ValueTask.CompletedTask;
    },
    processOutput: (request, state) =>
    {
        var results = Postprocess(request.get_output_tensor());
        SaveResults(state.ImagePath, results);
        return ValueTask.CompletedTask;
    },
    state: new { ImagePath = imagePath },
    cancellationToken: cancellationToken
);
```

## 项目文件 / Project File

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <LangVersion>13.0</LangVersion>
  </PropertyGroup>
  
  <ItemGroup>
    <PackageReference Include="JYPPX.OpenVINO.CSharp.API" Version="3.3.1" />
    <PackageReference Include="OpenCvSharp4" Version="4.10.0" />
    <PackageReference Include="OpenCvSharp4.runtime.win" Version="4.10.0" />
  </ItemGroup>
</Project>
```

## 运行案例 / Running the Sample

```bash
# 进入项目目录
cd samples/Yolo26Det-net10.0

# 运行
dotnet run

# 性能分析模式
dotnet run -- --profile

# 批量处理
dotnet run -- --batch ../../images/*.jpg
```

## 相关链接 / See Also

- [.NET 10.0 新特性](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-10)
- [Span<T> 最佳实践](https://learn.microsoft.com/dotnet/api/system.span-1)
- [API 参考](../../api/OpenVinoSharp.yml)
