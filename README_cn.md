![OpenVINO™ C# API](https://socialify.git.ci/guojin-yan/OpenVINO-CSharp-API/image?description=1&descriptionEditable=💞%20OpenVINO%20wrapper%20for%20.NET💞%20&forks=1&issues=1&logo=https%3A%2F%2Fs2.loli.net%2F2023%2F01%2F26%2FylE1K5JPogMqGSW.png&name=1&owner=1&pattern=Circuit%20Board&pulls=1&stargazers=1&theme=Light)

<p align="center">    
    <a href="./LICENSE.txt">
        <img src="https://img.shields.io/github/license/guojin-yan/openvinosharp.svg">
    </a>    
    <a >
        <img src="https://img.shields.io/badge/Framework-.NET5.0%2C%20.NET6.0%2C%20.NET48-pink.svg">
    </a>    

简体中文| [English](README.md)

## 这是OpenVINO™ C# API，该项目还在建设中，功能还未完善，如使用中有问题，欢迎与我沟通联系。如果对该项目感兴趣，也可以加入到我们的开发中来。🥰🥰🥰🥰



## 📚 简介

[OpenVINO™ ](www.openvino.ai)是一个用于优化和部署 AI 推理的开源工具包。

- 提升深度学习在计算机视觉、自动语音识别、自然语言处理和其他常见任务中的性能
- 使用流行框架（如TensorFlow，PyTorch等）训练的模型
- 减少资源需求，并在从边缘到云的一系列英特尔®平台上高效部署

&emsp;    该项目基于OpenVINO™工具套件推出了 OpenVINO™ C# API，旨在推动 OpenVINO™在C#领域的应用。OpenVINO™ C# API 由于是基于 OpenVINO™ 开发，所支持的平台与OpenVINO™ 一致，具体信息可以参考 OpenVINO™。

## <img title="NuGet" src="https://s2.loli.net/2023/01/26/ks9BMwXaHqQnKZP.png" alt="" width="40">NuGet 包

### Managed libraries

| Package                     | Description                    | Link                                                         |
| --------------------------- | ------------------------------ | ------------------------------------------------------------ |
| **OpenVINO.CSharp.API**     | OpenVINO C# API core libraries | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.CSharp.API.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.API/) |
| **OpenVINO.CSharp.Windows** | All-in-one package for Windows | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.CSharp.Windows.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.Windows/) |

### Native bindings

| Package                  | Description                 | Link                                                         |
| ------------------------ | --------------------------- | ------------------------------------------------------------ |
| **OpenVINO.runtime.win** | Native bindings for Windows | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.win.svg)](https://www.nuget.org/packages/OpenVINO.runtime.win/) |
|                          |                             |                                                              |



## ⚙ 如何安装

以下文章提供了OpenVINO™ C# API在不同平台的安装方法，可以根据自己使用平台进行安装。

- [Windows](docs/cn/windows_install.md)

- [Linux](docs/cn/linux_install.md)

## 🏷开始使用

- **快速体验**

  [使用OpenVINO™ C# API部署Yolov8全系列模型](demos/yolov8/README_cn.md)

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

- [爱克斯开发板使用OpenVINO™ C# API部署Yolov8模型](tutorial_examples/AlxBoard_deploy_yolov8/README_cn.md)
-  [行人摔倒检测 — 基于 OpenVINO C# API 部署PP-Human](tutorial_examples\PP-Human_Fall_Detection\README_cn.md) 
- [基于 OpenVINO 部署 RT-DETR](https://github.com/guojin-yan/RT-DETR-OpenVINO)

## 🗂 API 文档

如果想了解更多信息，可以参阅：[OpenVINO™ C# API API Documented](https://guojin-yan.github.io/OpenVINO-CSharp-API.docs/index.html)

## 🔃 更新日志

#### 🔥 **2023.10.22 ：更新OpenVINO™ C# API **

- 🗳 **OpenVINO™ C# API 库：**
  - 修改OpenVINO™ C# API 中的错误，并对代码板块进行整合，添加异常处理机制。
- 🛹**应用案例：**
  - 行人摔倒检测 — 基于 OpenVINO C# API 部署PP-Human
  - 基于 OpenVINO 部署 RT-DETR
- 🔮 **NuGet包：**
  - 废除之前发布的NuGet包，发布更新新的安装包，发布三类NuGet包，包括**OpenVINO.CSharp.API**：核心代码包，**OpenVINO.CSharp.Windows**：Windows平台整合包、**OpenVINO.runtime.win**：Windows平台运行库包。

####  **2023.6.19 ：发布 OpenVINO™ C# API 3.0**

- 🗳 **OpenVINO™ C# API 库：**
  - 升级OpenVINO™ C# API 2.0 到 OpenVINO™ C# API 3.0 版本，由原来的重构 C++ API 改为直接读取 OpenVINO™ 官方 C API，使得应用更加灵活，所支持的功能更加丰富。
- 🛹**应用案例：**
  - OpenVINO™ C# API部署Yolov8模型实例。
- 🔮 **NuGet包：**
  - 制作并发布NuGet包，发布**OpenVINO™ C# API.win 3.0.120**  ，包含OpenVINO 2023.0 依赖项。



## 🎖 贡献

&emsp;    如果您对OpenVINO™ 在C#使用感兴趣，有兴趣对开源社区做出自己的贡献，欢迎加入我们，一起开发OpenVINO™ C# API。

&emsp;    如果你对该项目有一些想法或改进思路，欢迎联系我们，指导下我们的工作。

## <img title="" src="https://user-images.githubusercontent.com/48054808/157835345-f5d24128-abaf-4813-b793-d2e5bdc70e5a.png" alt="" width="40"> 许可证书

本项目的发布受[Apache 2.0 license](https://github.com/guojin-yan/OpenVINO-CSharp-API/blob/csharp3.0/LICENSE.txt)许可认证。

