# Windows 安装 OpenVINOSharp

OpenVINOSharp 主要基于 OpenVINO™ 和 C# 开发，支持 Windows 10/11版本，目前已经在 x64 架构下完成测试。



## C# 环境配置

C# 是一种新式编程语言，不仅面向对象，还类型安全。 开发人员利用 C# 能够生成在 .NET 中运行的多种安全可靠的应用程序。C#环境安装可以参考下面的文章进行配置。

- [.NET 安装指南 - .NET | Microsoft Learn](https://learn.microsoft.com/zh-cn/dotnet/core/install/windows?tabs=net70)

- [.NET Framework 安装指南 - .NET Framework | Microsoft Learn](https://learn.microsoft.com/zh-cn/dotnet/framework/install/)



# OpenVINOSharp 安装

<div align=center><span><img src="https://s2.loli.net/2023/07/31/UFAgRbBuhcsqOEv.png" height=500/></span></div>

&emsp;    如果使用dotnet编译，可以通过以下方式安装：

```
dotnet add package OpenVinoSharp.win --version 3.0.115
```

&emsp;    **说明：**目前**.NET Framework 4.8**版本安装使用会出在问题，因此在项目生成后，需要将程序目录下openvino2023.0文件夹中的除**opencv_c.dll**文件移动到程序目录下，如图所示。

<div align=center><span><img src="https://s2.loli.net/2023/07/27/yNAUTqfw8azXg6i.png" height=500/></span></div>

