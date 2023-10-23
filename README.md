![OpenVINO™ C# API](https://socialify.git.ci/guojin-yan/OpenVINO-CSharp-API/image?description=1&descriptionEditable=💞%20OpenVINO%20wrapper%20for%20.NET💞%20&forks=1&issues=1&logo=https%3A%2F%2Fs2.loli.net%2F2023%2F01%2F26%2FylE1K5JPogMqGSW.png&name=1&owner=1&pattern=Circuit%20Board&pulls=1&stargazers=1&theme=Light)

<p align="center">    
    <a href="./LICENSE.txt">
        <img src="https://img.shields.io/github/license/guojin-yan/openvinosharp.svg">
    </a>    
    <a >
        <img src="https://img.shields.io/badge/Framework-.NET5.0%2C%20.NET6.0%2C%20.NET48-pink.svg">
    </a>    
</p>

[简体中文](README_cn.md) | English

## This is OpenVINO ™  C # API, this project is still under construction and its functions are not yet fully developed. If you have any problems using it, please feel free to communicate with me. If you are interested in this project, you can also join our development.🥰🥰🥰🥰🥰

## 📚 What is OpenVINO™ C# API ?

[OpenVINO™](www.openvino.ai)  is an open-source toolkit for optimizing and deploying AI inference.

- Boost deep learning performance in computer vision, automatic speech recognition, natural language processing and other common tasks
- Use models trained with popular frameworks like TensorFlow, PyTorch and more
- Reduce resource demands and efficiently deploy on a range of Intel® platforms from edge to cloud

&emsp;    This project is based on OpenVINO™ The tool kit has launched OpenVINO™  C # API, aimed at driving OpenVINO™ Application in the C # field. OpenVINO ™  The C # API is based on OpenVINO™  Development, supported platforms, and OpenVINO ™  Consistent, please refer to OpenVINO™ for specific information。

## <img title="NuGet" src="https://s2.loli.net/2023/08/08/jE6BHu59L4WXQFg.png" alt="" width="40">NuGet Package

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



## ⚙ How to install OpenVINO™ C# API?

The following article provides installation methods for OpenVINO™ C# API on different platforms, which can be installed according to your own platform.

- [Windows](docs/en/windows_install.md)

- [Linux](docs/en/linux_install.md)

## 🏷How to use OpenVINO™ C# API?

- **Quick start**
  - [Deploying the Yolov8 full series model using OpenVINO™ C# API](demos/yolov8/README.md)
- **Simple usage**

If you don't know how to use it, simply understand the usage method through the following code.

```c#
using OpenVINO™ C# API;
namespace test 
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Core core = new Core();
            Model model = core.read_model("./model.xml");
            CompiledModel compiled_model = core.compile_model(model, "AUTO"); 
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

## 💻 Tutorial Examples

- [Using OpenVINO™ C# API to Deploy the Yolov8 Model on the AIxBoard](tutorial_examples/AlxBoard_deploy_yolov8/README.md)
- [Pedestrian fall detection - Deploying PP-Human based on OpenVINO C # API](tutorial_examples\PP-Human_Fall_Detection\README.md) 
- [Deploying RT-DETR based on OpenVINO](https://github.com/guojin-yan/RT-DETR-OpenVINO)

## 🗂 API Reference

If you want to learn more information, you can refer to: [OpenVINO™ C# API API Documented](https://guojin-yan.github.io/OpenVINO-CSharp-API.docs/index.html)

## 🔃 Update log

#### 🔥 **2023.10.22 ：Update OpenVINO™ C# API **

- 🗳 **OpenVINO™ C# API ：**
  - Modify OpenVINO™  errors in the C # API, and integration of code sections to add exception handling mechanisms.
- 🛹**Application Cases：**
  - Pedestrian fall detection - Deploying PP-Human based on OpenVINO C # API
  - Deploying RT-DETR based on OpenVINO
- 🔮 **NuGet：**
  - Abolish the previously released NuGet package, release updated installation packages, and release three types of NuGet packages, including **OpenVINO. CSharp. API **: core code package, **OpenVINO. CSharp. Windows **: Windows platform integration package, and **OpenVINO. runtime. win **: Windows platform runtime package.

####  **2023.6.19 ： release OpenVINO™ C# API 3.0**

- 🗳OpenVINO™ C# API ：
  - Upgrade OpenVINO™ C# API 2.0 to OpenVINO™ C# API 3.0, changing from refactoring the C++API to directly reading OpenVino ™ The official C API makes the application more flexible and supports a richer range of functions.
- 🛹Application Cases：
  - OpenVINO™ C# API Deployment Yolov8 Model Example。
- 🔮NuGet：
  - Create and publish NuGet package, release * * OpenVINO™ C# API. win 3.0.120 * *, including OpenVino 2023.0 dependencies.

## 🎖 Contribute

&emsp; If you are interested in OpenVINO ™  Interested in using C # and contributing to the open source community, welcome to join us and develop OpenVINO™ C# API together.
&emsp; If you have any ideas or improvement ideas for this project, please feel free to contact us for guidance on our work.

## <img title="" src="https://s2.loli.net/2023/08/08/cijB2K9aDvthEQA.png" alt="" width="40"> License

The release of this project is certified under the [Apache 2.0 license](https://github.com/guojin-yan/OpenVINO™ C# API/blob/OpenVINO™ C# API3.0/LICENSE) .
