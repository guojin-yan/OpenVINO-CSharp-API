![OpenVINO™ C# API](https://socialify.git.ci/guojin-yan/OpenVINO-CSharp-API/image?description=1&descriptionEditable=💞%20OpenVINO%20wrapper%20for%20.NET💞%20&forks=1&issues=1&logo=https%3A%2F%2Fs2.loli.net%2F2023%2F01%2F26%2FylE1K5JPogMqGSW.png&name=1&owner=1&pattern=Circuit%20Board&pulls=1&stargazers=1&theme=Light)

<p align="center">    
    <a href="./LICENSE.txt">
        <img src="https://img.shields.io/github/license/guojin-yan/openvinosharp.svg">
    </a>    
    <a >
        <img src="https://img.shields.io/badge/Framework-.NET 8.0%2C%20.NET 6.0%2C%20.NET 5.0%2C%20.NET Framework 4.8%2C%20.NET Framework 4.7.2%2C%20.NET Framework 4.6%2C%20.NET Core 3.1-pink.svg">
    </a>    
</p>


简体中文| [English](README.md)

# 📚 简介

[OpenVINO™ ](www.openvino.ai)是一个用于优化和部署 AI 推理的开源工具包。

- 提升深度学习在计算机视觉、自动语音识别、自然语言处理和其他常见任务中的性能
- 使用流行框架（如TensorFlow，PyTorch等）训练的模型
- 减少资源需求，并在从边缘到云的一系列英特尔®平台上高效部署

&emsp;    OpenVINO™ C# API 是一个 OpenVINO™ 的 .Net wrapper，应用最新的 OpenVINO™ 库开发，通过 OpenVINO™ C API 实现 .Net 对 OpenVINO™ Runtime 调用，使用习惯与 OpenVINO™ C++ API 一致。OpenVINO™ C# API 由于是基于 OpenVINO™ 开发，所支持的平台与 OpenVINO™ 完全一致，具体信息可以参考 OpenVINO™。通过使用 OpenVINO™ C# API，可以在 .NET、.NET Framework等框架下使用 C# 语言实现深度学习模型在指定平台推理加速。

# <img title="NuGet" src="https://s2.loli.net/2023/08/08/jE6BHu59L4WXQFg.png" alt="" width="40">NuGet Package

## Core Managed Libraries

| Package                                        | Description                                               | Link                                                         |
| ---------------------------------------------- | --------------------------------------------------------- | ------------------------------------------------------------ |
| **OpenVINO.CSharp.API**                        | OpenVINO C# API core libraries                            | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.CSharp.API.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.API/) |
| **OpenVINO.CSharp.API.Extensions**             | OpenVINO C# API core extensions libraries                 | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.CSharp.API.Extensions.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.API.Extensions/) |
| **OpenVINO.CSharp.API.Extensions.OpenCvSharp** | OpenVINO C# API core extensions libraries use OpenCvSharp | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.CSharp.API.Extensions.OpenCvSharp.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.API.Extensions.OpenCvSharp/) |
| **OpenVINO.CSharp.API.Extensions.EmguCV**      | OpenVINO C# API core extensions libraries use EmguCV      | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.CSharp.API.Extensions.EmguCV.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.API.Extensions.EmguCV/) |

## Native Runtime Libraries

| Package                               | Description                          | Link                                                         |
| ------------------------------------- | ------------------------------------ | ------------------------------------------------------------ |
| **OpenVINO.runtime.win**              | Native bindings for Windows          | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.win.svg)](https://www.nuget.org/packages/OpenVINO.runtime.win/) |
| **OpenVINO.runtime.ubuntu.22-x86_64** | Native bindings for ubuntu.22-x86_64 | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.22-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.22-x86_64/) |
| **OpenVINO.runtime.ubuntu.20-x86_64** | Native bindings for ubuntu.20-x86_64 | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.20-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.20-x86_64/) |
| **OpenVINO.runtime.ubuntu.18-x86_64** | Native bindings for ubuntu.18-x86_64 | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.18-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.18-x86_64/) |
| **OpenVINO.runtime.debian9-arm64**    | Native bindings for debian9-arm64    | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.win.svg)](https://www.nuget.org/packages/OpenVINO.runtime.win/) |
| **OpenVINO.runtime.debian9-armhf**    | Native bindings for debian9-armhf    | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.debian9-armhf.svg)](https://www.nuget.org/packages/OpenVINO.runtime.debian9-armhf/) |
| **OpenVINO.runtime.centos7-x86_64**   | Native bindings for centos7-x86_64   | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.centos7-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.centos7-x86_64/) |
| **OpenVINO.runtime.rhel8-x86_64**     | Native bindings for rhel8-x86_64     | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.rhel8-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.rhel8-x86_64/) |
| **OpenVINO.runtime.macos-x86_64**     | Native bindings for macos-x86_64     | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.macos-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.macos-x86_64/) |
| **OpenVINO.runtime.macos-arm64**      | Native bindings for macos-arm64      | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.macos-arm64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.macos-arm64/) |


## Integration Library

| Package                     | Description                    | Link                                                         |
| --------------------------- | ------------------------------ | ------------------------------------------------------------ |
| **OpenVINO.CSharp.Windows** | All-in-one package for Windows | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.CSharp.Windows.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.Windows/) |

# ⚙ 如何安装

以下提供了OpenVINO™ C# API在不同平台的安装方法，可以根据自己使用平台进行安装。

## 	Windows

通过``dotnet add package``指令安装或通过Visual Studio安装以下程序包

```shell
dotnet add package OpenVINO.CSharp.API
dotnet add package OpenVINO.runtime.win
或者安装集成包——>
dotnet add package OpenVINO.CSharp.Windows
```

## 	Linux

&emsp;    **linux**平台我们根据官方编译的平台制作了对应的NuGet Package，以**ubuntu.22-x86_64**为例，通过``dotnet add package``指令安装：

```shell
dotnet add package OpenVINO.CSharp.API
dotnet add package OpenVINO.runtime.ubuntu.22-x86_64
```

&emsp;    运行一次程序后，添加环境变量：

```shell
export LD_LIBRARY_PATH={Program generated executable file directory}/runtimes/ubuntu.22-x86_64/native
例如——>
export LD_LIBRARY_PATH=/home/ygj/Program/sample1/bin/Debug/net6.0/runtimes/ubuntu.22-x86_64/native
```

&emsp;    如果对于一个全新平台(未安装过OpenVINO C++)，需要安装一下依赖环境，切换到``{Program generated executable file directory}/runtimes/ubuntu.22-x86_64/native``目录下，运行以下指令：

```shell
sudo -E ./install_openvino_dependencies.sh
```

## Mac OS

通过``dotnet add package``指令安装以下程序包

```shell
dotnet add package OpenVINO.CSharp.API
dotnet add package OpenVINO.runtime.macos-arm64
```

## 🏷开始使用

- **使用方法**

如果你不知道如何使用，通过下面代码简单了解使用方法。

```c#
using OpenVinoSharp;  // 引用命名空间
namespace test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using Core core = new Core();  // 初始化 Core 核心
            using Model model = core.read_model("./model.xml");  // 读取模型文件
            using CompiledModel compiled_model = core.compiled_model(model, "AUTO");  // 将模型加载到设备
            using InferRequest infer_request = compiled_model.create_infer_request();  // 创建推理通道
            using Tensor input_tensor = infer_request.get_tensor("images");  // 获取输入节点Tensor
            infer_request.infer();  // 模型推理
            using Tensor output_tensor = infer_request.get_tensor("output0");  // 获取输出节点Tensor
        }
    }
}
```

项目中所封装的类、对象例如Core、Model、Tensor等，通过调用 C api 接口实现，具有非托管资源，需要调用**Dispose()**方法处理或者使用**using**，否则就会出现内存泄漏。

## 💻 应用案例

获取耕读应用案例请参考：[OpenVINO-CSharp-API-Samples](https://github.com/guojin-yan/OpenVINO-CSharp-API-Samples)

## 🗂 API 文档

如果想了解更多信息，可以参阅：[OpenVINO™ C# API API Documented](https://guojin-yan.github.io/OpenVINO-CSharp-API.docs/index.html)

## 🎖 贡献

&emsp;    如果您对OpenVINO™ 在C#使用感兴趣，有兴趣对开源社区做出自己的贡献，欢迎加入我们，一起开发OpenVINO™ C# API。

&emsp;    如果你对该项目有一些想法或改进思路，欢迎联系我们，指导下我们的工作。

## <img title="" src="https://user-images.githubusercontent.com/48054808/157835345-f5d24128-abaf-4813-b793-d2e5bdc70e5a.png" alt="" width="40"> 许可证书

本项目的发布受[Apache 2.0 license](https://github.com/guojin-yan/OpenVINO-CSharp-API/blob/csharp3.0/LICENSE.txt)许可认证。

