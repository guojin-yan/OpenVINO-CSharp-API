![OpenVinoSharp](https://socialify.git.ci/guojin-yan/OpenVinoSharp/image?description=1&descriptionEditable=💞%20OpenVINO%20wrapper%20for%20.NET💞%20&forks=1&issues=1&logo=https%3A%2F%2Fs2.loli.net%2F2023%2F01%2F26%2FylE1K5JPogMqGSW.png&name=1&owner=1&pattern=Circuit%20Board&pulls=1&stargazers=1&theme=Light)

<p align="center">    
    <a href="./LICENSE.txt">
        <img src="https://img.shields.io/github/license/guojin-yan/openvinosharp.svg">
    </a>    
    <a >
        <img src="https://img.shields.io/badge/Framework-.NET5.0%2C%20.NET6.0%2C%20.NET48-pink.svg">
    </a>    

[简体中文](README-cn.md) | English

## This is OpenVINOSharp 3.0 version, which is still under construction and its features are not yet fully developed. If there are any issues during use, please feel free to contact me. If you are interested in this project, you can also join our development.🥰🥰🥰

# 📚 What is OpenVINOSharp ?

[OpenVINO™](www.openvino.ai)  is an open-source toolkit for optimizing and deploying AI inference.

- Boost deep learning performance in computer vision, automatic speech recognition, natural language processing and other common tasks
- Use models trained with popular frameworks like TensorFlow, PyTorch and more
- Reduce resource demands and efficiently deploy on a range of Intel® platforms from edge to cloud

&emsp;    However, OpenVINO™ No C# language interface provided, which is beneficial for using OpenVINO™ in C#  bringing a lot of trouble, therefore based on OpenVINO™ The tool kit has launched OpenVINOSharp, aiming to drive OpenVINO™ Application in the C # field. OpenVINOSharp is based on OpenVINO™ Development, supported platforms, and OpenVINO™ Consistent, please refer to OpenVINO™ for specific information.

&emsp;    Currently, OpenVINOSharp has been updated and iterated to version 3.0. Compared to the previous version, OpenVinoSharp version 3.0 has undergone a significant update, replacing the original refactoring of the C++API with directly reading OpenVINO\™  The official C API makes the application more flexible and supports a richer range of functions.

# <img title="NuGet" src="https://s2.loli.net/2023/08/08/jE6BHu59L4WXQFg.png" alt="" width="40">NuGet Package

### Managed Library

| Package               | Description                                                  | Link                                                         |
| --------------------- | ------------------------------------------------------------ | ------------------------------------------------------------ |
| **OpenVinoSharp.win** | OpenVinoSharp core libraries，comes with a complete OpenVINO 2023.0 dependency library | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVinoSharp.win.svg)](https://www.nuget.org/packages/OpenVinoSharp.win/) |

# ⚙ How to install OpenVINOSharp?

The following article provides installation methods for OpenVINOSharp on different platforms, which can be installed according to your own platform.

- [Windows](docs/en/windows_install.md)

- [Linux](docs/en/linux_install.md)

## 🏷How to use OpenVINOSharp?

- **Quick start**
  - [Deploying the Yolov8 full series model using OpenVINOSharp](demos/yolov8/README.md)
  
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
            Core core = new Core();
            Model model = core.read_model("./model.xml");
            CompiledModel compiled_model = core.compiled_model(model, "AUTO"); 
            InferRequest infer_request = compiled_model.create_infer_request(); 
            Tensor input_tensor = infer_request.get_tensor("images"); 
            infer_request.infer(); 
            Tensor output_tensor = infer_request.get_tensor("output0"); 
            core.free(); 
        }
    }
}
```

The classes and objects encapsulated in the project, such as Core, Model, Tensor, etc., are implemented by calling the C API interface and have unmanaged resources. They need to be handled by calling the **dispose() ** method, otherwise memory leakage may occur.

## 💻 Industrial tutorial examples



## 🗂 API Reference



## 🔃 Update log

#### 🔥 **2023.6.19 ： release OpenVinoSharp 3.0**

- 🗳OpenVinoSharp ：
  - Upgrade OpenVinoSharp 2.0 to OpenVinoSharp 3.0, changing from refactoring the C++API to directly reading OpenVino ™ The official C API makes the application more flexible and supports a richer range of functions.
- 🛹Application Cases：
  - OpenVinoSharp Deployment Yolov8 Model Example。
- 🔮NuGe：
  - Create and publish NuGet package, release * * OpenVinoSharp. win 3.0.120 * *, including OpenVino 2023.0 dependencies.

## 🎖 Contribute

&emsp; If you are interested in OpenVINO ™  Interested in using C # and contributing to the open source community, welcome to join us and develop OpenVinoSharp together.
&emsp; If you have any ideas or improvement ideas for this project, please feel free to contact us for guidance on our work.


## <img title="" src="https://s2.loli.net/2023/08/08/cijB2K9aDvthEQA.png" alt="" width="40"> License

The release of this project is certified under the [Apache 2.0 license](https://github.com/guojin-yan/OpenVINOSharp/blob/openvinosharp3.0/LICENSE) .
