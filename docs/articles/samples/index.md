# 案例应用 / Samples

本文档包含 OpenVINO C# API 的完整案例应用，展示如何在不同 .NET 框架版本中使用 API 进行深度学习模型推理。

## 案例列表 / Sample List

| 案例 | 框架 | 特性 | 说明 |
|------|------|------|------|
| [YOLO Detection (.NET 10.0)](yolo-net10.md) | .NET 10.0 | Span<T>, IAsyncEnumerable, Parallel.ForEachAsync | 最新 .NET 版本的高性能实现 |
| [YOLO Detection (.NET 4.8)](yolo-net48.md) | .NET Framework 4.8 | Span<T>, async/await | 完整功能的 .NET Framework 实现 |
| [YOLO Detection (.NET 4.6)](yolo-net46.md) | .NET Framework 4.6 | 传统异步模式 | 兼容旧版 .NET Framework 的实现 |
| [YOLO Detection (.NET Core 3.1)](yolo-netcoreapp31.md) | .NET Core 3.1 | Span<T>, Memory<T>, IAsyncEnumerable | 跨平台的 .NET Core 实现 |

## 案例概述 / Overview

所有案例均使用 YOLO 目标检测模型，展示以下功能：

- **模型加载** - 加载 OpenVINO IR 格式模型 (.xml/.bin)
- **图片预处理** - 使用 OpenCvSharp 进行图像预处理
- **同步推理** - 基础同步推理模式
- **异步推理** - 异步推理与回调机制
- **批量处理** - 批量图片推理
- **性能分析** - 详细的性能计时分析

## 框架特性对比 / Framework Feature Comparison

```
┌─────────────────────┬─────────┬─────────┬─────────┬─────────────┐
│ 特性 / Feature      │ net4.6  │ net4.8  │ netcore │ net10.0     │
├─────────────────────┼─────────┼─────────┼─────────┼─────────────┤
│ Span<T>             │   ✗     │   ✓     │   ✓     │   ✓         │
│ Memory<T>           │   ✗     │   ✓     │   ✓     │   ✓         │
│ async/await         │   ✓     │   ✓     │   ✓     │   ✓         │
│ IAsyncEnumerable    │   ✗     │   ✗     │   ✓     │   ✓         │
│ Parallel.ForEachAsync│  ✗     │   ✗     │   ✗     │   ✓         │
│ NativeLibrary       │   ✗     │   ✗     │   ✓     │   ✓         │
└─────────────────────┴─────────┴─────────┴─────────┴─────────────┘
```

## 运行案例 / Running Samples

### 先决条件 / Prerequisites

- 安装 [.NET SDK](https://dotnet.microsoft.com/download) (根据案例选择对应版本)
- 下载 YOLO 模型文件 (`yolo26n.xml` 和 `yolo26n.bin`)
- 准备测试图片

### 运行步骤 / Steps

```bash
# 进入案例目录
cd samples/Yolo26Det-net10.0

# 运行案例
dotnet run

# 或指定模型和图片路径
dotnet run -- --model ../../model/yolo26n.xml --image ../../images/bus.jpg
```

## 了解更多 / Learn More

- [API 参考文档](../../api/OpenVinoSharp.yml)
- [GitHub 仓库](https://github.com/guojin-yan/OpenVINO-CSharp-API)
