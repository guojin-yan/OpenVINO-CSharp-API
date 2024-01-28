# OpenVINO™ C# API 最新版详细介绍

> 前言
>
> OpenVINO™ C# API项目自创建以来，就在不断维护与更新，并且随着对该项目的深入研究，目前已经形成了固定的API接口以及比较稳定的运行版本。在之前的版本中，项目实现主要是通过自定义封装固定的接口实现，自 OpenVINO™ 官方发布的版本扩展了 C API 之后， OpenVINO™ C# API 也同步更新了3.0版本，其接口也更加灵活。
>
> 目前，该项目已经完成了3.1版本的升级，在3.0基础上，又增加了许多新功能，并且修复了3.0代码中出现的问题，同时支持更多平台与使用案例。
> 

# 项目介绍
&emsp;    [OpenVINO™](www.openvino.ai) 是一个用于优化和部署 AI 推理的开源工具包。

- 提升深度学习在计算机视觉、自动语音识别、自然语言处理和其他常见任务中的性能
- 使用流行框架（如TensorFlow，PyTorch等）训练的模型
- 减少资源需求，并在从边缘到云的一系列英特尔®平台上高效部署

&emsp;    OpenVINO™ C# API 是一个 OpenVINO™ 的 .Net wrapper，应用最新的 OpenVINO™ 库开发，通过 OpenVINO™ C API 实现 .Net 对 OpenVINO™ Runtime 调用，使用习惯与 OpenVINO™ C++ API 一致。OpenVINO™ C# API 由于是基于 OpenVINO™ 开发，所支持的平台与 OpenVINO™ 完全一致，具体信息可以参考 OpenVINO™。通过使用 OpenVINO™ C# API，可以在 .NET、.NET Framework等框架下使用 C# 语言实现深度学习模型在指定平台推理加速。

# 项目特性
- **OpenVINO™ C# API 支持全部的 C API 功能，并通过 C# 特性进行封装，使用起来更加容易；**
  - 1. 项目实现原理是通过 OpenVINO™ C API 实现 .Net 对 OpenVINO™ Runtime 调用，因此项目中实现了全部了OpenVINO™ C API；
  - 2. 基于C#语言面向对象的特征，对转换后的API进行了上层封装，并参考OpenVINO™ C++ API 的接口特点，对接口进行了进一步定义和封装，对接触过 C++ API 的用户十分友好；

- **项目封装完全采用 C# 特性，所有转换都是采用的 C# 顶层接口实现，避免采用了不安全代码模式；**
  - 指针不是安全类型，因此在项目封装时涉及到指针的操作，尽量采用了C#提供的上层接口对指针进行操作，避免了使用不安全编程。且对非托管内存进行了管理，防止出现内存泄漏。

- **支持 Windows 10/11、Linux、Mac OS 三大平台，支持 Nuget Package 方式一键式部署，使用更加便利；**
  - 1. 当前版本已经完成了不同平台的测试，支持 Windows 10/11、Linux、Mac OS 等最常见的系统使用；
  - 2. 提供了不同平台 OpenVINO™ Runtime Nuget Package，可以实现 Windows 10/11、Linux、Mac OS 三大平台一键式部署，使用更加便利。

- **实现了完整的接口测试**
  - 当前版本提供了核心程序集接口的单元测试，已经完成了99%接口的测试，确保了接口使用的稳定性。
  
- **增加了常使用的扩展接口，同时支持使用``OpenCvSharp``和``Emgu.CV``进行图像数据处理；**
  - 1. 封装了在模型部署时常用的接口方法，例如常见的图片数据预处理、推理结果输出与绘制等接口；
  - 2. 封装了常见模型部署流程方法，例如Yolov8、PP-Yoloe、RT-DETR、PP-OCR等模型，开发者可以几行代码便可以实现模型的本地部署。
  - 3. 在封装时分别采用``OpenCvSharp``和``Emgu.CV``进行开发，给用户更多的选择。

- **提供了更多完整的的项目案例，支持更多的常见模型部署案例；**
  - 使用最新版的 OpenVINO™ C# API 开发了更完善的模型部署案例，并配备了更加完善的项目开发文档，可以让更多新手开发者快速上手。

# Nuget Package

NuGet 是免费、开源的包管理开发工具，专注于在 .NET 应用开发过程中，安装第三方的组件库。因此当前项目已经进行了 Nuget Package 封装，用户可以实现通过 Nuget Package 快速安装当前项目。

在该项目中，主要封装了一下四种类型的 Nuget Package：

- **Core Managed Libraries**

  **OpenVINO.CSharp.API**是该项目的核心程序集，主要封装了[OpenVINOCSharpAPI](https://github.com/guojin-yan/OpenVINO-CSharp-API/tree/csharp3.1/src/CSharpAPI)程序集，在项目使用时是必须安装的程序集。

| Package                                        | Description                                               | Link                                                         |
| ---------------------------------------------- | --------------------------------------------------------- | ------------------------------------------------------------ |
| **OpenVINO.CSharp.API**                        | OpenVINO C# API core libraries                            | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.CSharp.API.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.API/) |


- **Native Runtime Libraries**

**Native Runtime Libraries**打包了 OpenVINO™ 官方发布的不同平台的程序集文件，其中主要包含了 OpenVINO™ Runtime 动态链接库以及所使用的第三方依赖项的动态链接库文件。用户在使用时，可以根据使用平台类型进行安装，截止到目前，已经实现了**Windows (10/11)**、**Linux (Ubuntu/Centos/Debain/Rhel)**、**MacOS (x64/Arm64)** 等平台的支持。
| Package                               | Description                          | Link                                                         |
| ------------------------------------- | ------------------------------------ | ------------------------------------------------------------ |
| **OpenVINO.runtime.win**              | Native bindings for Windows          | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.win.svg)](https://www.nuget.org/packages/OpenVINO.runtime.win/) |
| **OpenVINO.runtime.ubuntu.22-x86_64** | Native bindings for ubuntu.22-x86_64 | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.22-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.22-x86_64/) |
| **OpenVINO.runtime.ubuntu.20-x86_64** | Native bindings for ubuntu.20-x86_64 | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.20-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.20-x86_64/) |
| **OpenVINO.runtime.ubuntu.18-x86_64** | Native bindings for ubuntu.18-x86_64 | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.18-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.18-x86_64/) |
| **OpenVINO.runtime.debian9-arm64**    | Native bindings for debian9-arm64    | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.win.svg)](https://www.nuget.org/packages/OpenVINO.runtime.win/) |
| **OpenVINO.runtime.debian9-armhf**   | Native bindings for debian9-armhf    | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.debian9-armhf.svg)](https://www.nuget.org/packages/OpenVINO.runtime.debian9-armhf/) |
| **OpenVINO.runtime.centos7-x86_64**   | Native bindings for centos7-x86_64   | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.centos7-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.centos7-x86_64/) |
| **OpenVINO.runtime.rhel8-x86_64**     | Native bindings for rhel8-x86_64     | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.rhel8-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.rhel8-x86_64/) |
| **OpenVINO.runtime.macos-x86_64**     | Native bindings for macos-x86_64     | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.macos-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.macos-x86_64/) |
| **OpenVINO.runtime.macos-arm64**      | Native bindings for macos-arm64      | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.macos-arm64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.macos-arm64/) |


- **Core Extensions Managed Libraries**

**Core Extensions Managed Libraries** 是该项目的核心程序集的扩展，主要封装了[OpenVINOCSharpAPI.Extensions](https://github.com/guojin-yan/OpenVINO-CSharp-API/tree/csharp3.1/src/CSharpAPI.Extensions)、[OpenVINOCSharpAPI.Extensions.OpenCvSharp](https://github.com/guojin-yan/OpenVINO-CSharp-API/tree/csharp3.1/src/CSharp.API.Extensions.OpenCvSharp)、[OpenVINOCSharpAPI.API.Extensions.EmguCV](https://github.com/guojin-yan/OpenVINO-CSharp-API/tree/csharp3.1/src/CSharpAPI.Extensions.EmguCV)程序集，用户可以根据自己的使用需求进行安装。
| Package                                        | Description                                               | Link                                                         |
| ---------------------------------------------- | --------------------------------------------------------- | ------------------------------------------------------------ |
| **OpenVINO.CSharp.API.Extensions**             | OpenVINO C# API core extensions libraries                 | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.CSharp.API.Extensions.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.API.Extensions/) |
| **OpenVINO.CSharp.API.Extensions.OpenCvSharp** | OpenVINO C# API core extensions libraries use OpenCvSharp | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.CSharp.API.Extensions.OpenCvSharp.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.API.Extensions.OpenCvSharp/) |
| **OpenVINO.CSharp.API.Extensions.EmguCV**      | OpenVINO C# API core extensions libraries use EmguCV      | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.CSharp.API.Extensions.EmguCV.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.API.Extensions.EmguCV/) |

- **Integration Library**

  该程序集主要封装了不同平台的集合包，目前已经分发了Windows平台集合包，主要是包含了``OpenVINO.CSharp.API``、``OpenVINO.runtime.win``两个程序集，方便用户使用。

| Package                     | Description                    | Link                                                         |
| --------------------------- | ------------------------------ | ------------------------------------------------------------ |
| **OpenVINO.CSharp.Windows** | All-in-one package for Windows | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.CSharp.Windows.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.Windows/) |


# 程序集介绍

## 核心程序集

在OpenVINO™ C# API中，主要包含以下几个命名空间：
```csharp
// 包含 OpenVINO™ C# API 核心程序集
using OpenVinoSharp;
// 包含 OpenVINO™ 数据类型
using OpenVinoSharp.element;
// 包含 OpenVINO™ C# API 模型处理方法
using OpenVinoSharp.preprocess;
```
其中，在``OpenVinoSharp``命名空间下，包含了 OpenVINO™ 部署深度学习模型的主要对象，在进行封装时，主要参考了 C++ API 接口定义方式，因此其使用时基本与 C++ API 一致，如下表所示：
|Class|C++ API|C# API|说明|
| ----------- | ------------ | ------------- | -------------- |
| Core class | ov::Core | Core| OpenVINO™运行时核心实体类 |
| Model class | ov::Model | Model | 用户自定义模型类 |
| CompiledModel class | ov::CompiledModel|CompiledModel|编译后的模型类|
| Node class | ov::Node | Node | 模型节点实体类 |
| Output class | ov::Output | Output | 模型输出节点实体类 |
| Input class | ov::Input | Input | 模型输入节点实体类 |
| InferRequest class | ov::InferRequest | InferRequest| 模型推理请求类 |
| Tensor class | ov::Tensor | Tensor| 推理请求节点张量 |
| Shape class | ov::Shape | Shape| 节点张量形状类 |
| PartialShape class | ov::PartialShape | PartialShape | 节点张量动态形状类 |

## 核心扩展程序集

OpenVINO™ C# API 核心扩展程序集主要是封装了一些常用的函数方法以及常见模型的模型部署接口，主要包含了图片数据处理方法、Yolov8、PP-Yoloe、RT-DETR、PP-OCR等模型的结果对象以及推理方式。同时在封装时充分考虑了图片数据的处理方式，分别使用 **OpenCvSharp** 以及 **Emgu.CV** 两种开源库。

该扩展程序集主要包含以下命名空间：

```csharp
// 包含使用 OpenVINO™ C# API 封装的扩展程序
using OpenVinoSharp.Extensions;
// 包含自定义的一些扩展方法
using OpenVinoSharp.Extensions.utility;
// 包含使用 OpenVINO™ C# API 封装的常见模型部署接口
using OpenVinoSharp.Extensions.model;
// 包含使用 OpenCvSharp 或 Emgu.CV 封装的常见图片处理接口
using OpenVinoSharp.Extensions.process;
// 包含使用 OpenCvSharp 或 Emgu.CV 封装的常见模型结果类
using OpenVinoSharp.Extensions.result;
```

此处主要是简单介绍了一下``OpenVinoSharp``命名空间下主要对象，有关详细介绍可以参考以下文章：[《OpenVINO™ C# API 详解与演示(基础接口)》]()、[《OpenVINO™ C# API 详解与演示(预处理接口)》]()、[《OpenVINO™ C# API 详解与演示(扩展接口)》]()。


# 使用案例


# 贡献
目前该项目尚在开发阶段，基本已经完成了对当前 OpenVINO™ 官方封装的 C API，但是目前 OpenVINO™ C API 并没有完全实现 C++ API。因此如果大家有兴趣，可以为 OpenVINO™ 官方源码的 C API 提交 issues 或着提交 Pr，继续丰富官方的 C API。关于在 OpenVINO™ 中提交贡献可以参考一下文章：[CONTRIBUTING](https://github.com/openvinotoolkit/openvino/blob/master/CONTRIBUTING.md)

此外，如果您对该项目有兴趣，也可以向该项目提交贡献，可以对当前项目代码进行优化、完善接口测试、添加扩展接口等内容。关于在 OpenVINO™ C# API 做贡献，可以参考以下文章：[为 OpenVINO™ C# API 做贡献](https://github.com/guojin-yan/OpenVINO-CSharp-API/blob/csharp3.1/CONTRIBUTING_cn.md)


# 联系方式
如果您在使用过程中有问题，可以通过提交 issues 进行解决，由于本人精力有限，可能来不及回复，可以通过以下方式获取更多的信息或者添加我的联系方式：


# 参考
想要获取更多信息，请参考：
- [OpenVINO GitHub](https://github.com/openvinotoolkit/openvino)

- [OpenVINO Document](https://docs.openvino.ai/)

- [OpenVINO™ C# API](https://github.com/guojin-yan/OpenVINO-CSharp-API)

- [OpenVINO™ C# API Samples](https://github.com/guojin-yan/OpenVINO-CSharp-API-Samples)