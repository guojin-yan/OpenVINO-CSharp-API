# **简介(Introduction)**

&emsp;    英特尔发行版 [OpenVINO™](www.openvino.ai)工具套件基于oneAPI 而开发，可以加快高性能计算机视觉和深度学习视觉应用开发速度工具套件，适用于从边缘到云的各种英特尔平台上，帮助用户更快地将更准确的真实世界结果部署到生产系统中。通过简化的开发工作流程， OpenVINO™可赋能开发者在现实世界中部署高性能应用程序和算法。 

&emsp;    然而 OpenVINO™未提供 C# 语言接口，这对在 C# 中使用 OpenVINO™ 带来了很多麻烦，对此推出了 OpenVINO™ C# API，旨在推动 OpenVINO™在C#领域的应用，目前 OpenVINO™ C# API 已经更新迭代起到 **csharp3.1** 版本，相比于之前版本，OpenVINO™ C# API 3.1 版本做了较大程度上的更新.

&emsp;    在OpenVINO™ C# API 3.0 版本时，由原来的重构 C++ API 改为直接读取 OpenVINO™ 官方 C API，使得应用更加灵活，所支持的功能更加丰富。OpenVINO™ C# API 3.0 接口参考 OpenVINO™ C++ API 来实现，因此在使用时更加接近 C++ API，这对熟悉使用C++ API的朋友会更加友好。

&emsp;    当前推出的最新版本为 **OpenVINO™ C# API 3.1** 预发布版本，该版本在 3.0 版本上进行了进一步更新，完善了所有测试代码，并改进了一些错误，后续将根据最新版推出相关的案例项目与应用。

&emsp;    最后，如果各位在使用中有什么问题，可以与我沟通联系，也欢迎广大C#开发者加入到OpenVINO™ C# API 开发中。

&emsp;    Intel Distribution [OpenVINO™](www.openvino. ai) tool suite is developed based on the oneAPI and can accelerate the development speed of high-performance computer vision and deep learning visual applications. It is suitable for various Intel platforms from the edge to the cloud, helping users deploy more accurate real-world results to production systems faster. By simplifying the development workflow, OpenVINO™ Empowering developers to deploy high-performance applications and algorithms in the real world.

&emsp;    However, OpenVINO™ does not provide a C # language interface, which brings a lot of trouble to using OpenVINO™ in C #. Therefore, OpenVINO™ C# API has been launched to promote the application of OpenVINO™ in the C # field. Currently, OpenVINO™ C# API has been updated and iterated to **csharp3.1 ** version, and compared to the previous version, OpenVINO™ C# API 3.1 has undergone significant updates

&emsp;    In version 3.0 of OpenVINO™ C# API, the original refactoring of the C++API was changed to directly reading the official C API of OpenVINO™, making the application more flexible and supporting a richer range of functions. The OpenVINO™ C# API 3.0 interface is implemented based on OpenVINO™ C# API, so it is closer to the C++API when used, which is more friendly for friends who are familiar with using the C++API.

&emsp;    The latest version currently released is **OpenVINO™ C# API 3.1 ** pre release version, which has been further updated on version 3.0, improving all test codes and addressing some errors. Relevant case projects and applications will be released based on the latest version in the future.

&emsp;    Finally, if you have any questions during use, you can communicate and contact me. We also welcome C # developers to join us in OpenVINO™ C# API development.

# **联系方式(Contact Details)**

**以下是我的联系方式：**

&emsp;    **E-mail：**guojin_yjs@cumt.edu.cn

&emsp;    **CSDN博客：**https://guojin.blog.csdn.net

&emsp;    **GitHub：**https://github.com/guojin-yan

&emsp;    **Gitee：**https://gitee.com/guojin-yan

&emsp;    **开源项目OpenVINO CSharp API：**https://github.com/guojin-yan/OpenVINO-CSharp-API

&emsp;    **视频网站：**https://space.bilibili.com/222626791

&emsp;    **微信公众号：**CSharp与边缘模型部署



# **NuGet Package**

### **Core Managed Libraries**

| Package                                        | Description                                               | Link                                                         |
| ---------------------------------------------- | --------------------------------------------------------- | ------------------------------------------------------------ |
| **OpenVINO.CSharp.API**                        | OpenVINO C# API core libraries                            | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.CSharp.API.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.API/) |
| **OpenVINO.CSharp.API.Extensions**             | OpenVINO C# API core extensions libraries                 | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.CSharp.API.Extensions.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.API.Extensions/) |
| **OpenVINO.CSharp.API.Extensions.OpenCvSharp** | OpenVINO C# API core extensions libraries use OpenCvSharp | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.CSharp.API.Extensions.OpenCvSharp.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.API.Extensions.OpenCvSharp/) |
| **OpenVINO.CSharp.API.Extensions.EmguCV**      | OpenVINO C# API core extensions libraries use EmguCV      | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.CSharp.API.Extensions.EmguCV.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.API.Extensions.EmguCV/) |

### **Native Runtime Libraries**

| Package                               | Description                          | Link                                                         |
| ------------------------------------- | ------------------------------------ | ------------------------------------------------------------ |
| **OpenVINO.runtime.win**              | Native bindings for Windows          | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.win.svg)](https://www.nuget.org/packages/OpenVINO.runtime.win/) |
| **OpenVINO.runtime.ubuntu.22-x86_64** | Native bindings for ubuntu.22-x86_64 | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.22-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.22-x86_64/) |
| **OpenVINO.runtime.ubuntu.20-x86_64** | Native bindings for ubuntu.20-x86_64 | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.20-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.20-x86_64/) |
| **OpenVINO.runtime.ubuntu.18-x86_64** | Native bindings for ubuntu.18-x86_64 | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.18-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.18-x86_64/) |
| **OpenVINO.runtime.debian9-arm64**    | Native bindings for debian9-arm64    | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.win.svg)](https://www.nuget.org/packages/OpenVINO.runtime.win/) |
| **OpenVINO.runtime.centos7-x86_64**   | Native bindings for centos7-x86_64   | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.centos7-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.centos7-x86_64/) |
| **OpenVINO.runtime.macos-x86_64**     | Native bindings for macos-x86_64     | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.macos-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.macos-x86_64/) |
| **OpenVINO.runtime.macos-arm64**      | Native bindings for macos-arm64      | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.macos-arm64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.macos-arm64/) |

## **Integration Library**

| Package                     | Description                    | Link                                                         |
| --------------------------- | ------------------------------ | ------------------------------------------------------------ |
| **OpenVINO.CSharp.Windows** | All-in-one package for Windows | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.CSharp.Windows.svg)](https://www.nuget.org/packages/OpenVINO.CSharp.Windows/) |

