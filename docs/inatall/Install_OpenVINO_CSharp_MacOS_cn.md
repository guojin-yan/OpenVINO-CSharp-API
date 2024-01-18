## 在MacOS上搭建OpenVINO™C#开发环境

- [在MacOS上搭建OpenVINO™C#开发环境](#在MacOS上搭建OpenVINO™C#开发环境)
  - [🧩简介](#🧩简介)
  - [🔮安装.NET运行环境](#🔮安装.NET运行环境)
  - [🎈配置C#开发环境](#🎈配置C#开发环境)
  - [🎨创建并配置C#项目](#🎨创建并配置C#项目)
    - [第一步：创建OpenVINO™C#项目](#第一步：创建OpenVINO™C#项目)
    - [第二步：添加项目依赖](#第二步：添加项目依赖)
  - [🎁测试OpenVINO™C#项目](#🎁测试OpenVINO™C#项目)
  - [🎯总结](#🎯总结)

### 🧩简介

当前MacOS系统主要分为两个比较大的版本，一个是2020年之前的苹果系统，该MacOS系统使用的时Intel系列的CPU，另一个是2020年后推出的苹果M系列芯片的系统。在该教程中将从零开始详述在**MacOS(M2)**上搭建**OpenVINO™ CSharp**开发环境，并对 **OpenVINO™ CSharp API **环境进行简单测试。

### 🔮安装.NET运行环境

**[.NET](https://learn.microsoft.com/zh-cn/dotnet/)** 是由 **Microsoft** 创建的一个免费的、跨平台的、开源开发人员平台，可以使用 C#、F# 或 Visual Basic 语言编写代码，用于构建许多不同类型的应用程序，可以在任何兼容的操作系统上(Windows、Linux、Mac OS等)运行。

Microsoft官方提供了**.NET**环境的详细安装流程，大家可以参考以下文章进行安装：[在 macOS 上安装 .NET](https://learn.microsoft.com/zh-cn/dotnet/core/install/macos)。

首先访问网站[下载 .NET](https://dotnet.microsoft.com/zh-cn/download)，具体下载选择如下所示，

<div align=center><img src="https://s2.loli.net/2024/01/08/n2wVCSYoFm8JgzP.png" width=500></div>

文件下载完后，通过双击安装文件进行环境的安装：

<div align=center><img src="https://s2.loli.net/2024/01/08/De5XQlPk4Fxr1Ly.png" width=500></div>

打开安装文件后，如下图所示，此处无需设置其他配置，只需要按照默认步骤进行安装即可。

<div align=center><img src="https://s2.loli.net/2024/01/08/S17VTvHwnOPhxt8.png" width=400></div>

### 🎈配置C#开发环境

在Linux环境下我们可以使用以下组合进行C#代码开发：

- 代码构建工具：**dotnet **
- 代码编辑工具：**Visual Studio Code**

在上文中我们安装**.NET**时已经同时安装了**dotnet **工具。**Visual Studio Code **是一款功能强大的代码编辑器，并且支持更多第三方插件，同时支持C#代码开发，**Visual Studio Code**安装比较简单，只需从[VS Code官网](https://code.visualstudio.com/)下载安装文件，按照默认选项完成安装。

然后配置C#编辑环境，在扩展商店中搜索C#，安装C#扩展，如下图所示。

<div align=center><img src="https://s2.loli.net/2024/01/08/to9sSw2vGbchJIZ.png" width=800></div>

### 🎨创建并配置C#项目

#### 第一步：创建 OpenVINO™C# 项目

使用**dotnet**创建一个测试项目，在Terminal中输入以下指令进行项目创建：

```shell
dotnet new console -o test_openvino_csharp --framework net6.0
```

<div align=center><img src="https://s2.loli.net/2024/01/08/ZbmSRdEDVA7yK8w.png" width=500></div>

#### 第二步：添加项目依赖

接下来使用**Visual Studio Code**打开项目文件，在**Visual Studio Code**下方的终端窗口输入以下指令，添加``OpenVINO.CSharp.API``以及``	OpenVINO.runtime.macos-arm64``项目依赖包，其输出如下图所示：

```shell
dotnet add package OpenVINO.CSharp.API
dotnet add package OpenVINO.runtime.macos-arm64
```

- **OpenVINO.CSharp.API**：OpenVINO™ CSharp API 项目核心程序集。
- **OpenVINO.runtime.macos-arm64**：OpenVINO™ 在MacOS M系列平台运行所需依赖项。
- **OpenVINO.runtime.macos-x86_64**：OpenVINO™ 在MacOS Intel CPU系列平台运行所需依赖项。

<div align=center><img src="https://s2.loli.net/2024/01/08/KBy74UngiFkIDuM.png" width=800></div>

### 🎁测试OpenVINO™C#项目

首先添加测试代码，用户可以直接将下述代码替换到上文所创建的项目中的**Program.cs**文件中。

```csharp
using OpenVinoSharp;
namespace test_openvino_csharp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // -------- 测试 OpenVINO CSharp API 安装 --------
            OpenVinoSharp.Version version = Ov.get_openvino_version();
            Console.WriteLine("---- OpenVINO INFO----");
            Console.WriteLine("Description : " + version.description);
            Console.WriteLine("Build number: " + version.buildNumber);
        }
    }
}
```

创建并配置好项目后，然后在终端中输入``dotnet run``运行程序即可，结果如下图所示：

<div align=center><img src="https://s2.loli.net/2024/01/08/cIXCWJLrgjVOZT2.png" width=800></div>

此处主要输出了OpenVINO版本信息，如果出现以下结果，说明环境配置成功。

### 🎯总结

至此，我们就完成了在MacOS上搭建OpenVINO™C#开发环境，欢迎大家使用，如需要更多信息，可以参考一下内容：

- [OpenVINO™](https://github.com/openvinotoolkit/openvino)
- [OpenVINO CSharp API](https://github.com/guojin-yan/OpenVINO-CSharp-API)

- [OpenVINO CSharp API Samples](https://github.com/guojin-yan/OpenVINO-CSharp-API-Samples)