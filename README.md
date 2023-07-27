![OpenVinoSharp](https://socialify.git.ci/guojin-yan/OpenVinoSharp/image?description=1&descriptionEditable=💞%20OpenVINO%20wrapper%20for%20.NET💞%20&forks=1&issues=1&logo=https%3A%2F%2Fs2.loli.net%2F2023%2F01%2F26%2FylE1K5JPogMqGSW.png&name=1&owner=1&pattern=Circuit%20Board&pulls=1&stargazers=1&theme=Light)

简体中文| [English](README-en.md)

## 这是OpenVinoSharp 3.0 版本，该版本还在建设中，功能还未完善，如使用中有问题，欢迎与我沟通联系。如果对该项目感兴趣，也可以加入到我们的开发中来。🥰🥰🥰🥰

## <img title="更新日志" src="https://s2.loli.net/2023/01/26/RJ1znO78bygCcKj.png" alt="" width="40">更新日志



## <img title="更新日志" src="https://s2.loli.net/2023/01/26/Zs1VFUT4BGQgfE9.png" alt="" width="40"> 简介

&emsp;    英特尔发行版 [OpenVINO™](www.openvino.ai)工具套件基于oneAPI 而开发，可以加快高性能计算机视觉和深度学习视觉应用开发速度工具套件，适用于从边缘到云的各种英特尔平台上，帮助用户更快地将更准确的真实世界结果部署到生产系统中。通过简化的开发工作流程， OpenVINO™可赋能开发者在现实世界中部署高性能应用程序和算法。
&emsp;    在推理后端，得益于  OpenVINO™ 工具套件提供的“一次编写，任意部署”的特性，转换后的模型能够在不同的英特尔硬件平台上运行，而无需重新构建，有效简化了构建与迁移过程。可以说，如果开发者希望在英特尔平台上实现最佳的推理性能，并具备多平台适配和兼容性，  OpenVINO™ 是不可或缺的部署工具首选。 OpenVINO™最新版本2023.0，引入了一系列旨在增强开发人员体验的新功能、改进和弃用，突出亮点是通过最大限度地减少离线转换、扩大模型支持和推进硬件优化来改善开发者之旅。

&emsp;    然而 OpenVINO™未提供C#语言接口，这对在C#中使用 OpenVINO™带来了很多麻烦，对此推出了 OpenVinoSharp，旨在推动 OpenVINO™在C#领域的应用，目前OpenVinoSharp已经更新迭代起到3.0版本，相比于之前版本，OpenVinoSharp 3.0 版本做了较大程度上的更新，由原来的重构 C++ API 改为直接读取 OpenVINO™ 官方 C API，使得应用更加灵活，所支持的功能更加丰富。OpenVinoSharp 3.0 API 接口多参考 OpenVINO™ C++ API 实现，因此在使用时更加接近C++ API，这对熟悉使用C++ API的朋友会更加友好。

&emsp;

<div align=center><span><img src="https://s2.loli.net/2023/01/26/LdbeOYGgwZvHcBQ.png" height=300/></span></div>



## <img title="NuGet" src="https://s2.loli.net/2023/01/26/ks9BMwXaHqQnKZP.png" alt="" width="40">NuGet包

### 托管库

| Package               | Description                                                  | Link                                                         |
| --------------------- | ------------------------------------------------------------ | ------------------------------------------------------------ |
| **OpenVinoSharp.win** | OpenVinoSharp core libraries，附带完整的OpenVINO 2023.0依赖库 | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVinoSharp.win.svg)](https://www.nuget.org/packages/OpenVinoSharp.win/) |



## <img title="安装" src="https://s2.loli.net/2023/01/26/bm6WsE5cfoVvj7i.png" alt="" width="50"> 安装

&emsp;    该项目已经打包成NuGet程序包并发布到NuGet平台，用户可以通过Visual Studio的NuGet程序包功能安装下载，当前最先版本为3.0.1版本。

<div align=center><span><img src="https://s2.loli.net/2023/07/27/OdEMVKmeZ8JYuxn.png" height=500/></span></div>

&emsp;    如果使用dotnet编译，可以通过以下方式安装：

```
dotnet add package OpenVinoSharp.win --version 3.0.115
```

&emsp;    **说明：**目前**.NETFramework 4.8**版本安装使用会出在问题，因此在项目生成后，需要将程序目录下openvino2023.0文件夹中的除**opencv_c.dll**文件移动到程序目录下，如图所示。

<div align=center><span><img src="https://s2.loli.net/2023/07/27/yNAUTqfw8azXg6i.png" height=500/></span></div>



## 🏷使用方法

如果你不知道如何使用，可以参考我们项目案例，或者通过下面代码简单了解使用方法。

```c#
using OpenVinoSharp;  // 引用命名空间
namespace test 
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Core core = new Core();  // 初始化 Core 核心
            Model model = core.read_model("./model.xml");  // 读取模型文件
            CompiledModel compiled_model = core.compiled_model(model, "AUTO");  // 将模型加载到设备
            InferRequest infer_request = compiled_model.create_infer_request();  // 创建推理通道
            Tensor input_tensor = infer_request.get_tensor("images");  // 获取输入节点Tensor
            infer_request.infer();  // 模型推理
            Tensor output_tensor = infer_request.get_tensor("output0");  // 获取输出节点Tensor
            core.free();  // 清理 Core 非托管内存
        }
    }
}
```

项目中所封装的类、对象例如Core、Model、Tensor等，董事通过调用 C api 接口实现，具有非托管资源，需要调用**dispose()**方法处理，否则就会出现内存泄漏。

## 🎖 贡献

&emsp;    如果您对OpenVINO™ 在C#使用感兴趣，有兴趣对开源社区做出自己的贡献，欢迎加入我们，一起开发OpenVinoSharp。

&emsp;    如果你对该项目有一些想法或改进思路，欢迎联系我们，指导下我们的工作。


## <img title="" src="https://user-images.githubusercontent.com/48054808/157835345-f5d24128-abaf-4813-b793-d2e5bdc70e5a.png" alt="" width="40"> 许可证书

本项目的发布受[Apache 2.0 license](LICENSE)许可认证。

