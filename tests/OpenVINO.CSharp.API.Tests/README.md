# OpenVINO C# API 测试项目

本测试项目包含 OpenVINO C# API 的全面测试套件。

## 测试结构

```
OpenVINO.CSharp.API.Tests/
├── TestHelpers/              # 测试辅助类
│   ├── TestCategories.cs    # 测试类别常量
│   ├── OpenVINOFactAttribute.cs  # OpenVINO 条件测试特性
│   └── TestDataGenerator.cs  # 测试数据生成器
├── UnitTests/               # 单元测试（无需 OpenVINO 运行时）
│   ├── ShapeTests.cs        # Shape 类测试
│   ├── OvLoggerTests.cs       # OvLogger 类测试
│   ├── ExceptionTests.cs    # 异常处理测试
│   ├── ElementTypeTests.cs  # ElementType 枚举测试
│   ├── VersionTests.cs      # Version 结构体测试
│   ├── ModelCacheTests.cs   # ModelCache 测试
│   ├── InferRequestPoolTests.cs  # InferRequestPool 测试
│   ├── DisposableObjectTests.cs  # DisposableObject 基类测试
│   ├── ExceptionStatusTests.cs   # ExceptionStatus 枚举测试
│   └── OvTests.cs           # Ov 静态类测试
├── IntegrationTests/        # 集成测试（需要 OpenVINO 运行时）
│   ├── CoreIntegrationTests.cs   # Core 类集成测试
│   ├── CoreAdvancedTests.cs      # Core 类高级测试
│   ├── TensorIntegrationTests.cs # Tensor 类集成测试
│   ├── TensorAdvancedTests.cs    # Tensor 类高级测试
│   ├── CompiledModelIntegrationTests.cs  # CompiledModel 测试
│   ├── InferRequestIntegrationTests.cs   # InferRequest 测试
│   └── PrePostProcessorIntegrationTests.cs  # PrePostProcessor 测试
└── Benchmarks/              # 性能基准测试
    └── TensorBenchmarks.cs  # Tensor 操作基准测试
```

## 接口覆盖情况

| 接口/类 | 单元测试 | 集成测试 | 基准测试 | 覆盖率 |
|--------|---------|---------|---------|-------|
| **Core** | ✅ | ✅ 基础+高级 | - | 高 |
| **CompiledModel** | - | ✅ 基础 | - | 中 |
| **InferRequest** | - | ✅ 基础 | - | 中 |
| **Tensor** | - | ✅ 基础+高级 | ✅ | 高 |
| **Shape** | ✅ 完整 | - | ✅ | 高 |
| **Model** | - | ✅ (通过 Core) | - | 中 |
| **ModelCache** | ✅ 完整 | - | - | 高 |
| **InferRequestPool** | ✅ 完整 | - | - | 高 |
| **PrePostProcessor** | - | ✅ 基础 | - | 低 |
| **OvLogger** | ✅ 完整 | - | ✅ | 高 |
| **ElementType** | ✅ 完整 | - | - | 高 |
| **ExceptionStatus** | ✅ 完整 | - | - | 高 |
| **Version** | ✅ | - | - | 高 |
| **Ov** | ✅ | - | - | 中 |
| **DisposableObject** | ✅ 完整 | - | - | 高 |

## 运行测试

### 运行所有测试
```bash
dotnet test tests/OpenVINO.CSharp.API.Tests
```

### 只运行单元测试（无需 OpenVINO）
```bash
dotnet test tests/OpenVINO.CSharp.API.Tests --filter "Category=Unit"
```

### 只运行集成测试（需要 OpenVINO）
```bash
dotnet test tests/OpenVINO.CSharp.API.Tests --filter "Category=Integration"
```

### 运行性能基准测试
```bash
dotnet run --project tests/OpenVINO.CSharp.API.Tests --filter "Category=Performance"
```

### 运行特定测试类
```bash
dotnet test tests/OpenVINO.CSharp.API.Tests --filter "FullyQualifiedName~ShapeTests"
```

## 测试数据

集成测试需要实际的模型文件。测试会检查 `model/yolo26n.xml` 文件是否存在，如果不存在则跳过相关测试。

要运行完整的集成测试，请提供测试模型文件：
1. 将模型文件（`model/yolo26n.xml` ）放在测试输出目录
2. 或在测试代码中修改模型路径

## 添加新测试

1. 确定测试类型（单元测试/集成测试/基准测试）
2. 在相应目录创建测试类
3. 继承正确的基类或使用适当的特性
4. 添加 `[Trait("Category", TestCategories.xxx)]` 标记
5. 对于需要 OpenVINO 的测试，使用 `[OpenVINOFact]` 特性
