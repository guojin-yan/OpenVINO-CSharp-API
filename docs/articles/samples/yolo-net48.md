# YOLO 目标检测 - .NET Framework 4.8

## 简介 / Introduction

本案例展示如何在 .NET Framework 4.8 中使用 OpenVINO C# API 进行 YOLO 目标检测。.NET Framework 4.8 是 .NET Framework 的最新版本，通过 NuGet 包支持 `Span<T>` 和 `Memory<T>` 等现代特性。

## 框架特性 / Framework Features

- ✅ **Span<T>** - 通过 System.Memory 包支持零拷贝内存操作
- ✅ **Memory<T>** - 通过 System.Memory 包支持安全的内存抽象
- ✅ **async/await** - 完整的异步编程模型支持
- ❌ **IAsyncEnumerable<T>** - 需要 .NET Core 3.0+
- ❌ **Parallel.ForEachAsync** - 需要 .NET 6.0+
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

### 异步推理

```csharp
// .NET Framework 4.8 支持 async/await
public async Task RunInferenceAsync(string imagePath)
{
    using var request = model.create_infer_request();
    
    // 在后台线程执行预处理
    await Task.Run(() => Preprocess(imagePath, request));
    
    // 执行异步推理
    request.start_async();
    
    // 等待完成
    await Task.Run(() => request.wait());
    
    // 处理结果
    var results = Postprocess(request.get_output_tensor());
}
```

### 批量处理

```csharp
// 使用 Parallel.For 进行并行处理
Parallel.ForEach(imageFiles, new ParallelOptions { MaxDegreeOfParallelism = 4 },
    imageFile =>
    {
        using var request = model.create_infer_request();
        Preprocess(imageFile, request);
        request.infer();
        var results = Postprocess(request.get_output_tensor());
        lock (lockObj)
        {
            allResults.Add(imageFile, results);
        }
    });
```

## 性能优化 / Performance Optimization

### 使用 Span<T> (需要 System.Memory 包)

```csharp
// 通过 NuGet 安装 System.Memory 后可以使用 Span
void ProcessWithSpan(float[] data)
{
    Span<float> span = data.AsSpan();
    // 使用 Span 进行零拷贝操作
    for (int i = 0; i < span.Length; i++)
    {
        span[i] = span[i] / 255.0f;
    }
}
```

### 对象池复用

```csharp
// 简单的对象池实现
public class InferRequestPool
{
    private readonly ConcurrentBag<InferRequest> _pool;
    private readonly CompiledModel _model;
    
    public InferRequestPool(CompiledModel model, int size)
    {
        _model = model;
        _pool = new ConcurrentBag<InferRequest>();
        for (int i = 0; i < size; i++)
        {
            _pool.Add(model.create_infer_request());
        }
    }
    
    public InferRequest Rent()
    {
        if (_pool.TryTake(out var request))
            return request;
        return _model.create_infer_request();
    }
    
    public void Return(InferRequest request)
    {
        _pool.Add(request);
    }
}
```

## 项目文件 / Project File

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net48</TargetFramework>
    <LangVersion>8.0</LangVersion>
  </PropertyGroup>
  
  <ItemGroup>
    <PackageReference Include="JYPPX.OpenVINO.CSharp.API" Version="3.3.1" />
    <PackageReference Include="OpenCvSharp4" Version="4.10.0" />
    <PackageReference Include="OpenCvSharp4.runtime.win" Version="4.10.0" />
    <!-- Span<T> 和 Memory<T> 支持 -->
    <PackageReference Include="System.Memory" Version="4.5.5" />
  </ItemGroup>
</Project>
```

## 运行案例 / Running the Sample

```bash
# 进入项目目录
cd samples/Yolo26Det-net4.8

# 运行
dotnet run

# 或指定模型和图片
dotnet run -- --model ../../model/yolo26n.xml --image ../../images/bus.jpg
```

## 注意事项 / Notes

- .NET Framework 4.8 仅支持 Windows 平台
- 需要通过 NuGet 安装 System.Memory 包来使用 Span<T>
- 异步操作使用 Task.Run 包装同步方法

## 相关链接 / See Also

- [.NET Framework 4.8 文档](https://learn.microsoft.com/dotnet/framework/whats-new/)
- [System.Memory NuGet 包](https://www.nuget.org/packages/System.Memory/)
- [API 参考](../../api/OpenVinoSharp.yml)
