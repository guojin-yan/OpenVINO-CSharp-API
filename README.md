![OpenVINO™ C# API](https://socialify.git.ci/guojin-yan/OpenVINO-CSharp-API/image?description=1&descriptionEditable=💞%20OpenVINO%20wrapper%20for%20.NET💞%20&forks=1&issues=1&logo=https%3A%2F%2Fs2.loli.net%2F2023%2F01%2F26%2FylE1K5JPogMqGSW.png&name=1&owner=1&pattern=Circuit%20Board&pulls=1&stargazers=1&theme=Light)

<p align="center">    
    <a href="./LICENSE.txt">
        <img src="https://img.shields.io/github/license/guojin-yan/openvinosharp.svg">
    </a>    
    <a >
        <img src="https://img.shields.io/badge/Framework-.NET 8.0%2C%20.NET 6.0%2C%20.NET 5.0%2C%20.NET Framework 4.8%2C%20.NET Framework 4.7.2%2C%20.NET Framework 4.6%2C%20.NET Core 3.1-pink.svg">
    </a>    
</p>

[简体中文](README_cn.md) | English

## 📚 What is OpenVINO™ C# API ?

[OpenVINO™](www.openvino.ai)  is an open-source toolkit for optimizing and deploying AI inference.

- Boost deep learning performance in computer vision, automatic speech recognition, natural language processing and other common tasks
- Use models trained with popular frameworks like TensorFlow, PyTorch and more
- Reduce resource demands and efficiently deploy on a range of Intel® platforms from edge to cloud

&emsp;    This project is based on OpenVINO™ The tool kit has launched OpenVINO™  C # API, aimed at driving OpenVINO™ Application in the C # field. OpenVINO ™  The C # API is based on OpenVINO™  Development, supported platforms, and OpenVINO ™  Consistent, please refer to OpenVINO™ for specific information。

## <img title="NuGet" src="https://s2.loli.net/2023/08/08/jE6BHu59L4WXQFg.png" alt="" width="40">NuGet Package

### Core Managed Libraries

| Package                                        | Description                                               | Link                                                         |
| ---------------------------------------------- | --------------------------------------------------------- | ------------------------------------------------------------ |
| **OpenVINO.CSharp.API**                        | OpenVINO C# API core libraries                            | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.CSharp.API.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.API/) |
| **OpenVINO.CSharp.API.Extensions**             | OpenVINO C# API core extensions libraries                 | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.CSharp.API.Extensions.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.API.Extensions/) |
| **OpenVINO.CSharp.API.Extensions.OpenCvSharp** | OpenVINO C# API core extensions libraries use OpenCvSharp | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.CSharp.API.Extensions.OpenCvSharp.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.API.Extensions.OpenCvSharp/) |
| **OpenVINO.CSharp.API.Extensions.EmguCV**      | OpenVINO C# API core extensions libraries use EmguCV      | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.CSharp.API.Extensions.EmguCV.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.API.Extensions.EmguCV/) |

### Native Runtime Libraries

| Package                               | Description                          | Link                                                         |
| ------------------------------------- | ------------------------------------ | ------------------------------------------------------------ |
| **OpenVINO.runtime.win**              | Native bindings for Windows          | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.win.svg)](https://www.nuget.org/packages/OpenVINO.runtime.win/) |
| **OpenVINO.runtime.ubuntu.22-x86_64** | Native bindings for ubuntu.22-x86_64 | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.22-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.22-x86_64/) |
| **OpenVINO.runtime.ubuntu.20-x86_64** | Native bindings for ubuntu.20-x86_64 | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.20-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.20-x86_64/) |
| **OpenVINO.runtime.ubuntu.18-x86_64** | Native bindings for ubuntu.18-x86_64 | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.18-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.18-x86_64/) |
| **OpenVINO.runtime.debian9-arm64**    | Native bindings for debian9-arm64    | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.win.svg)](https://www.nuget.org/packages/OpenVINO.runtime.win/) |
| **OpenVINO.runtime.debian9-armhf **   | Native bindings for debian9-armhf    | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.debian9-armhf.svg)](https://www.nuget.org/packages/OpenVINO.runtime.debian9-armhf/) |
| **OpenVINO.runtime.centos7-x86_64**   | Native bindings for centos7-x86_64   | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.centos7-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.centos7-x86_64/) |
| **OpenVINO.runtime.rhel8-x86_64**     | Native bindings for rhel8-x86_64     | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.rhel8-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.rhel8-x86_64/) |
| **OpenVINO.runtime.macos-x86_64**     | Native bindings for macos-x86_64     | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.macos-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.macos-x86_64/) |
| **OpenVINO.runtime.macos-arm64**      | Native bindings for macos-arm64      | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.macos-arm64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.macos-arm64/) |

### Integration Library

| Package                     | Description                    | Link                                                         |
| --------------------------- | ------------------------------ | ------------------------------------------------------------ |
| **OpenVINO.CSharp.Windows** | All-in-one package for Windows | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.CSharp.Windows.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.Windows/) |



## ⚙ How to install OpenVINO™ C# API?

&emsp;    The following provides OpenVINO ™  The installation method of C # API on different platforms can be customized according to the platform you are using.

### 	**Windows**

&emsp;    Install the following package through the ``dotnet add package`` command or through Visual Studio

```shell
dotnet add package OpenVINO.CSharp.API
dotnet add package OpenVINO.runtime.win
Or install =》
dotnet add package OpenVINO.CSharp.Windows
```

### 	**Linux**

&emsp;    We have created the corresponding NuGet Package for the **Linux ** platform based on the official compiled platform,  For example, using **ubuntu.22-x86_64**  is installed using the ``dotnet add package`` command:

```shell
dotnet add package OpenVINO.CSharp.API
dotnet add package OpenVINO.runtime.ubuntu.22-x86_64
```

&emsp;    After running the program once, add environment variables:

```
export LD_LIBRARY_PATH={Program generated executable file directory}/runtimes/ubuntu.22-x86_64/native
such as =》
export LD_LIBRARY_PATH=/home/ygj/Program/sample1/bin/Debug/net6.0/runtimes/ubuntu.22-x86_64/native
```

&emsp;    If for a brand new platform (without installing OpenVINO C++), it is necessary to install a dependent environment and switch to ``{Program generated executable file directory}/runtimes/ubuntu.22-x86'_ 64/native ``directory, run the following command:

```shell
sudo -E ./install_openvino_dependencies.sh
```

## Mac OS

Install the following package using the ``dotnet add package``command

```shell
dotnet add package OpenVINO.CSharp.API
dotnet add package OpenVINO.runtime.macos-arm64
```

## 🏷开始使用

## 🏷How to use OpenVINO™ C# API?

- **Simple usage**

If you don't know how to use it, simply understand the usage method through the following code.

```c#
using OpenVinoSharp;
namespace test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using Core core = new Core();
            using Model model = core.read_model("./model.xml");
            using CompiledModel compiled_model = core.compiled_model(model, "AUTO");
            using InferRequest infer_request = compiled_model.create_infer_request();
            using Tensor input_tensor = infer_request.get_tensor("images");
            infer_request.infer();
            using Tensor output_tensor = infer_request.get_tensor("output0");
        }
    }
}
```

The classes and objects encapsulated in the project, such as Core, Model, Tensor, etc., are implemented by calling the C API interface and have unmanaged resources. They need to be handled by calling the **Dispose() ** method or `using` statement, otherwise memory leakage may occur.

## 💻 Tutorial Examples



## 🗂 API Reference

If you want to learn more information, you can refer to: [OpenVINO™ C# API API Documented](https://guojin-yan.github.io/OpenVINO-CSharp-API.docs/index.html)

## 🎖 Contribute

&emsp; If you are interested in OpenVINO ™  Interested in using C # and contributing to the open source community, welcome to join us and develop OpenVINO™ C# API together.
&emsp; If you have any ideas or improvement ideas for this project, please feel free to contact us for guidance on our work.

## <img title="" src="https://s2.loli.net/2023/08/08/cijB2K9aDvthEQA.png" alt="" width="40"> License

The release of this project is certified under the [Apache 2.0 license](https://github.com/guojin-yan/OpenVINO-CSharp-API/blob/csharp3.0/LICENSE.txt) .
