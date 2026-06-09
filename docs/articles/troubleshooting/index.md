# 问题排查 / Troubleshooting

本文档总结使用 OpenVINO C# API 时可能遇到的常见问题和解决方案。

This page summarizes common issues and fixes for the OpenVINO C# API.

## 问题分类 / Categories

### 安装问题 / Installation Issues

| 问题 / Issue | 解决方案 / Solution |
| --- | --- |
| NuGet 包安装失败 | 检查网络连接，并确认 NuGet 源可访问。 |
| 运行时找不到 DLL | 确认安装了与目标平台匹配的 runtime 包。 |
| 版本冲突 | 确认 API 包和 runtime 包版本兼容。 |

### 运行问题 / Runtime Issues

| 问题 / Issue | 解决方案 / Solution |
| --- | --- |
| 模型加载失败 | 检查模型文件路径、格式和依赖文件是否完整。 |
| 推理速度较慢 | 确认设备名称、插件和 runtime 包是否正确。 |
| 内存持续增长 | 确认 `Core`、`Model`、`Tensor`、`CompiledModel`、`InferRequest` 等对象已及时释放。 |
| GenAI runtime 加载失败 | 安装 `JYPPX.OpenVINO.GenAI.runtime.*`，或设置 `OPENVINO_GENAI_RUNTIME_DIR`。 |

## 常见问题 / FAQ

### Q: 如何检查 OpenVINO 是否正确安装？

```csharp
using OpenVinoSharp;

try
{
    using Core core = new Core();
    Console.WriteLine("OpenVINO version: " + core.get_version());

    foreach (var device in core.get_available_devices())
    {
        Console.WriteLine("Available device: " + device);
    }
}
catch (Exception ex)
{
    Console.WriteLine("OpenVINO initialization failed: " + ex.Message);
}
```

### Q: 如何切换推理设备？

```csharp
using Core core = new Core();
Model model = core.read_model("model.xml");

CompiledModel gpuCompiled = core.compile_model(model, "GPU");
CompiledModel autoCompiled = core.compile_model(model, "AUTO");
```

### Q: 只使用基础 OpenVINO API 是否需要 GenAI runtime？

No. Core APIs do not load `openvino_genai_c`. Install the GenAI runtime package only when calling `OpenVinoSharp.GenAI`.

不需要。基础 API 不会加载 `openvino_genai_c`。只有调用 `OpenVinoSharp.GenAI` 时才需要安装 GenAI runtime 包。

## 错误代码 / Error Codes

| 错误代码 / Code | 说明 / Description | 解决方案 / Solution |
| --- | --- | --- |
| -1 | General error / 通用错误 | 查看异常消息和 runtime 日志。 |
| -2 | File not found / 文件未找到 | 确认文件路径正确。 |
| -3 | Invalid parameter / 参数无效 | 检查输入参数格式。 |
| -6 | Out of bounds / 越界 | 检查索引、数组长度和 native size 参数。 |
| -9 | Device unavailable / 设备不可用 | 检查设备插件和驱动。 |

## 提交问题 / Submitting Issues

如果文档中没有找到解决方案，请在 GitHub 提交 issue，并提供：

If the documentation does not solve the problem, open a GitHub issue with:

1. 问题现象 / Symptom
2. 复现步骤 / Reproduction steps
3. 环境信息 / Environment information
4. 相关错误日志 / Relevant logs

[提交新 Issue](https://github.com/guojin-yan/OpenVINO-CSharp-API/issues/new)

## 相关资源 / Resources

- [OpenVINO 官方文档](https://docs.openvino.ai/)
- [案例应用 / Samples](../samples/index.md)
- [API 文档](../../api/OpenVinoSharp.yml)
