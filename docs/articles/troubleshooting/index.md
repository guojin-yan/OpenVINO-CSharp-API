# 问题排查 / Troubleshooting

本文档总结了使用 OpenVINO C# API 时可能遇到的常见问题及解决方案。

## 问题分类 / Categories

### 安装问题 / Installation Issues

| 问题 / Issue | 解决方案 / Solution |
|-------------|-------------------|
| NuGet 包安装失败 | 检查网络连接，尝试使用国内镜像源 |
| 运行时找不到 DLL | 确保安装了对应的 runtime 包 |
| 版本冲突 | 确保所有相关包版本一致 |

### 运行问题 / Runtime Issues

| 问题 / Issue | 解决方案 / Solution |
|-------------|-------------------|
| 模型加载失败 | 检查模型文件路径和格式 |
| 推理速度慢 | 检查是否使用了正确的设备（CPU/GPU） |
| 内存泄漏 | 确保正确释放 OpenVINO 对象资源 |

## 常见问题 / FAQ

### Q: 如何检查 OpenVINO 是否正确安装？

```csharp
using OpenVinoSharp;

try
{
    using Core core = new Core();
    Console.WriteLine("OpenVINO 版本: " + core.get_version());
    
    // 列出可用设备
    foreach (var device in core.get_available_devices())
    {
        Console.WriteLine("可用设备: " + device);
    }
}
catch (Exception ex)
{
    Console.WriteLine("初始化失败: " + ex.Message);
}
```

### Q: 如何切换推理设备？

```csharp
using Core core = new Core();
Model model = core.read_model("model.xml");

// 使用 GPU 推理（如果可用）
CompiledModel compiled = core.compile_model(model, "GPU");

// 或使用 AUTO 模式自动选择最佳设备
CompiledModel compiled = core.compile_model(model, "AUTO");
```

### Q: 如何处理图像预处理？

推荐使用 OpenCvSharp4 扩展包：

```csharp
using OpenVinoSharp.Extensions.process;

// 使用扩展方法快速预处理
Mat image = Cv2.ImRead("image.jpg");
Tensor input_tensor = image.to_tensor();
```

## 错误代码 / Error Codes

| 错误代码 | 说明 | 解决方案 |
|---------|------|---------|
| -1 | 通用错误 | 检查日志获取详细信息 |
| -2 | 文件未找到 | 确认文件路径正确 |
| -3 | 参数无效 | 检查输入参数格式 |
| -6 | 内存不足 | 减少批量大小或释放资源 |
| -9 | 设备不可用 | 检查设备驱动是否正确安装 |

## 提交问题 / Submitting Issues

如果在文档中没有找到解决方案，请在 GitHub 提交 Issue：

1. 描述问题现象
2. 提供复现步骤
3. 提供环境信息（OS, .NET 版本, 包版本）
4. 提供相关错误日志

[提交新 Issue](https://github.com/guojin-yan/OpenVINO-CSharp-API/issues/new)

## 相关资源 / Resources

- [OpenVINO 官方文档](https://docs.openvino.ai/)
- [案例应用 / Samples](../samples/)
- [API 文档](../../api/OpenVinoSharp.yml)
