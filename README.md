![OpenVinoSharp](https://socialify.git.ci/guojin-yan/OpenVinoSharp/image?description=1&descriptionEditable=💞%20OpenVINO%20wrapper%20for%20.NET💞%20&forks=1&issues=1&logo=https%3A%2F%2Fs2.loli.net%2F2023%2F01%2F26%2FylE1K5JPogMqGSW.png&name=1&owner=1&pattern=Circuit%20Board&pulls=1&stargazers=1&theme=Light)

<p align="center">    
    <a href="./LICENSE.txt">
        <img src="https://img.shields.io/github/license/guojin-yan/openvinosharp.svg">
    </a>    
    <a >
        <img src="https://img.shields.io/badge/Framework-.NET5.0%2C%20.NET6.0%2C%20.NET48-pink.svg">
    </a>    

[简体中文](README-cn.md) | English

## This is OpenVinoSharp 3.0 version, which is still under construction and its features are not yet fully developed. If there are any issues during use, please feel free to contact me. If you are interested in this project, you can also join our development.🥰🥰🥰

## <img title="更新日志" src="https://s2.loli.net/2023/01/26/RJ1znO78bygCcKj.png" alt="" width="40">Update log

#### 🔥 **2023.6.19 ： release OpenVinoSharp 3.0**

- 🗳 **OpenVinoSharp ：**
  - Upgrade OpenVinoSharp 2.0 to OpenVinoSharp 3.0, changing from refactoring the C++API to directly reading OpenVino ™  The official C API makes the application more flexible and supports a richer range of functions.
- 🛹**Application Cases：**
  - OpenVinoSharp Deployment Yolov8 Model Example。
- 🔮 **NuGe：**
  - Create and publish NuGet package, release * * OpenVinoSharp. win 3.0.120 * *, including OpenVino 2023.0 dependencies.

## <img title="更新日志" src="https://s2.loli.net/2023/01/26/Zs1VFUT4BGQgfE9.png" alt="" width="40"> Introduction

&emsp;    Intel Distribution [OpenVINO ™](www.openvino. ai) The tool suite is developed based on oneAPI, which can accelerate the development of high-performance computer vision and deep learning visual applications. The tool suite is applicable to various Intel platforms from the edge to the cloud, helping users deploy more accurate real-world results to production systems more quickly. By simplifying the development workflow, OpenVINO ™ Enable developers to deploy high-performance applications and algorithms in the real world.
&emsp;    On the inference backend, thanks to OpenVINO ™  The tool suite provides the feature of "write once, deploy any", and the converted model can run on different Intel hardware platforms without the need to rebuild, effectively simplifying the construction and migration process. It can be said that if developers want to achieve the best inference performance on the Intel platform and have multi-platform adaptation and compatibility, OpenVINO ™  It is an indispensable deployment tool of choice. OpenVINO ™ The latest version 2023.0 introduces a series of new features, improvements, and deprecations aimed at enhancing the developer experience, highlighting the improvement of the developer journey by minimizing offline conversions, expanding model support, and promoting hardware optimization.



<div align=center><span><img src="https://s2.loli.net/2023/01/26/LdbeOYGgwZvHcBQ.png" height=300/></span></div>

&emsp; However, OpenVINO ™ No C # language interface provided, which is beneficial for using OpenVINO in C # ™ Bringing a lot of trouble, OpenVinoSharp was launched to promote OpenVino ™ In the application of C #, OpenVinoSharp has been updated and iterated to version 3.0. Compared to the previous version, OpenVinoSharp version 3.0 has undergone a significant update, changing from refactoring the C++API to directly reading OpenVino ™  The official C API makes the application more flexible and supports a richer range of functions. OpenVinoSharp 3.0 API interface with multiple references to OpenVino ™  C++API implementation, therefore it is closer to the C++API when used, which will be more friendly to friends who are familiar with using the C++API.

## <img title="NuGet" src="https://s2.loli.net/2023/01/26/ks9BMwXaHqQnKZP.png" alt="" width="40">NuGet

### Managed Library

| Package               | Description                                                  | Link                                                         |
| --------------------- | ------------------------------------------------------------ | ------------------------------------------------------------ |
| **OpenVinoSharp.win** | OpenVinoSharp core libraries，Comes with a complete OpenVINO 2023.0 dependency library | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVinoSharp.win.svg)](https://www.nuget.org/packages/OpenVinoSharp.win/) |



## <img title="安装" src="https://s2.loli.net/2023/01/26/bm6WsE5cfoVvj7i.png" alt="" width="50"> Installation

&emsp;   The project has been packaged into a NuGet package and published to the NuGet platform. Users can install and download it through the NuGet package feature of Visual Studio. The current earliest version is version 3.0.1.

<div align=center><span><img src="https://s2.loli.net/2023/07/31/UFAgRbBuhcsqOEv.png" height=500/></span></div>

&emsp;    If you use dotnet compilation, you can install it in the following ways:

```
dotnet add package OpenVinoSharp.win --version 3.0.115
```

&emsp;    **Note: ** Currently, there may be issues with the installation and use the **. NET Framework 4.8**. Therefore, after the project is generated, it is necessary to move the files except for **openvino_c.dll** from the openvino2023.0 folder in the program directory to the program directory, as shown in the figure.

<div align=center><span><img src="https://s2.loli.net/2023/07/27/yNAUTqfw8azXg6i.png" height=500/></span></div>



## 🏷Usage

If you don't know how to use it, you can refer to our project case or simply learn the usage method through the following code.

```c#
using OpenVinoSharp;  // using namespace.
namespace test 
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Core core = new Core();  // Initialize Core
            Model model = core.read_model("./model.xml");  // Read model.
            CompiledModel compiled_model = core.compiled_model(model, "AUTO");  // Load model onto device
            InferRequest infer_request = compiled_model.create_infer_request();  // Create infer request.
            Tensor input_tensor = infer_request.get_tensor("images");  // Get input node of Tensor.
            infer_request.infer();  // infer
            Tensor output_tensor = infer_request.get_tensor("output0");  // Get output node of Tensor.
            core.free();  // Clean Core Unmanaged Memory
        }
    }
}
```

The classes and objects encapsulated in the project, such as Core, Model, Tensor, are implemented by the directors by calling the C api interface. They have unmanaged resources and need to call the **dispose() ** method for processing. Otherwise, Memory leak will occur.

## <img title="" src="https://s2.loli.net/2023/02/09/2ApTvzLDwlYS6Ks.png" alt="" width="40"> Application Cases

[OpenVinoSharp Deployment Yolov8 Model Example](https://github.com/guojin-yan/OpenVinoSharp/tree/openvinosharp3.0/demos/yolov8)

## 🎖 Contribute

&emsp; If you are interested in OpenVINO ™  Interested in using C # and contributing to the open source community, welcome to join us and develop OpenVinoSharp together.
&emsp; If you have any ideas or improvement ideas for this project, please feel free to contact us for guidance on our work.


## <img title="" src="https://user-images.githubusercontent.com/48054808/157835345-f5d24128-abaf-4813-b793-d2e5bdc70e5a.png" alt="" width="40"> License

The release of this project is certified under the [Apache 2.0 license](LICENSE) .

