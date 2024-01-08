## 在Linux上搭建OpenVINO™C#开发环境

- [在Linux上搭建OpenVINO™C#开发环境](#在Linux上搭建OpenVINO™C#开发环境)
  - [🧩简介](#🧩简介)
  - [🔮安装.NET运行环境](#🔮安装.NET运行环境)
    - [第一步安装.NET6.0SDK](#第一步安装.NET6.0SDK)
    - [第二步安装.NET6.0Runtime](#第二步安装.NET6.0Runtime)
  - [🎈配置C#开发环境](#🎈配置C#开发环境)
  - [🎨创建并配置C#项目](#🎨创建并配置C#项目)
    - [第一步：创建OpenVINO™C#项目](#第一步：创建OpenVINO™C#项目)
    - [第二步：添加项目依赖](#第二步：添加项目依赖)
    - [第三步：安装OpenVINO其他依赖](#第三步：安装OpenVINO其他依赖)
  - [🎁测试OpenVINO™C#项目](#🎁测试OpenVINO™C#项目)
  - [🎯总结](#🎯总结)



### 🧩简介

本文将从零开始详述在**Linux(Ubuntu 22.04)**上搭建**OpenVINO™ CSharp**开发环境，并对 **OpenVINO™ CSharp API **环境进行简单测试。

### 🔮安装.NET运行环境

**[.NET](https://learn.microsoft.com/zh-cn/dotnet/)** 是由 **Microsoft** 创建的一个免费的、跨平台的、开源开发人员平台，可以使用 C#、F# 或 Visual Basic 语言编写代码，用于构建许多不同类型的应用程序，可以在任何兼容的操作系统上(Windows、Linux、Mac OS等)运行。

Microsoft官方提供了**.NET**环境的详细安装流程，大家可以参考以下文章进行安装：[在 Linux 上安装 .NET](https://learn.microsoft.com/zh-cn/dotnet/core/install/linux)。

由于当前Linux提供的系统较多，接下来将基于**Linux(Ubuntu 20.04)**系统安装**.NET 6.0**进行项目演示。

#### 第一步安装.NET6.0SDK

创建一个Terminal，输入以下指令安装 **.NET 6.0 SDK**。

```shell
sudo apt-get install -y dotnet-sdk-6.0
```

<div align=center><img src="https://s2.loli.net/2024/01/08/yq7jt1FCTw8paRY.png" width=500></div>

#### 第二步安装.NET6.0Runtime

一般情况下安装的 **.NET 6.0 SDK**中已经包含了 **.NET 6.0 Runtime**，但是如果版本不一致，可能无法使用，因此用户可以在输入以下指令，单独安装**.NET 6.0 Runtime**。

```shell
sudo apt-get install -y dotnet-runtime-6.0
```

<div align=center><img src="https://s2.loli.net/2024/01/08/S5DBkfa2yeR9hzK.png" width=500></div>

### 🎈配置C#开发环境

在Linux环境下我们可以使用以下组合进行C#代码开发：

- 代码构建工具：**dotnet **
- 代码编辑工具：**Visual Studio Code**

在上文中我们安装**.NET 6.0 SDK**时已经同时安装了**dotnet **工具。**Visual Studio Code **是一款功能强大的代码编辑器，并且支持更多第三方插件，同时支持C#代码开发，**Visual Studio Code**安装比较简单，只需从[VS Code官网](https://code.visualstudio.com/)下载安装文件，按照默认选项完成安装。

然后配置C#编辑环境，在扩展商店中搜索C#，安装C#扩展，如下图所示。

<div align=center><img src="https://s2.loli.net/2024/01/08/1QY8UR4IEPrZvaf.png" width=800></div>

### 🎨创建并配置C#项目

#### 第一步：创建 OpenVINO™C# 项目

使用**dotnet**创建一个测试项目，在Terminal中输入以下指令进行项目创建：

```shell
dotnet new console -o test_openvino_csharp --framework net6.0
```

<div align=center><img src="https://s2.loli.net/2024/01/08/6bjKwRMHoW9uOsC.png" width=500></div>

#### 第二步：添加项目依赖

接下来依次输入以下指令，使用**Visual Studio Code**打开项目文件：

```shell
cd test_openvino_csharp
code .
```

接下来在**Visual Studio Code**下方的终端窗口输入以下指令，添加``OpenVINO.CSharp.API``以及``OpenVINO.runtime.ubuntu.22-x86_64``项目依赖包，其输出如下图所示：

```shell
dotnet add package OpenVINO.CSharp.API
dotnet add package OpenVINO.runtime.ubuntu.22-x86_64
```

- **OpenVINO.CSharp.API**：OpenVINO™ CSharp API 项目核心程序集。
- **OpenVINO.runtime.ubuntu.22-x86_64**：OpenVINO™ 在Ubuntu 22.04 平台运行所需依赖项。

<div align=center><img src="https://s2.loli.net/2024/01/08/EPQ4RGdTr1saCuJ.png" width=800></div>

如果用户使用的是其他Linux环境，则需要安装其他平台下的OpenVINO™运行依赖，目前发布的OpenVINO CSharp API对应的Linux运行依赖包如下表中所示，用户可以根据自己的系统要求进行安装。

| Package                               | Description                          | Link                                                         |
| ------------------------------------- | ------------------------------------ | ------------------------------------------------------------ |
| **OpenVINO.runtime.ubuntu.22-x86_64** | Native bindings for ubuntu.22-x86_64 | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.22-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.22-x86_64/) |
| **OpenVINO.runtime.ubuntu.20-x86_64** | Native bindings for ubuntu.20-x86_64 | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.20-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.20-x86_64/) |
| **OpenVINO.runtime.ubuntu.18-x86_64** | Native bindings for ubuntu.18-x86_64 | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.ubuntu.18-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.ubuntu.18-x86_64/) |
| **OpenVINO.runtime.debian9-arm64**    | Native bindings for debian9-arm64    | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.win.svg)](https://www.nuget.org/packages/OpenVINO.runtime.win/) |
| **OpenVINO.runtime.debian9-armhf **   | Native bindings for debian9-armhf    | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.debian9-armhf.svg)](https://www.nuget.org/packages/OpenVINO.runtime.debian9-armhf/) |
| **OpenVINO.runtime.centos7-x86_64**   | Native bindings for centos7-x86_64   | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.centos7-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.centos7-x86_64/) |
| **OpenVINO.runtime.rhel8-x86_64**     | Native bindings for rhel8-x86_64     | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVINO.runtime.rhel8-x86_64.svg)](https://www.nuget.org/packages/OpenVINO.runtime.rhel8-x86_64/) |



#### 第三步：安装OpenVINO其他依赖

如果用户主机上之前没用安装过OpenVINO C++，而是第一次使用 OpenVINO™，则需要安装其他依赖项。

首先输入``dotnet run``运行项目，然后打开``[Project File]/bin/Debug/net6.0/runtimes/ubuntu.22-x86_64/native``目录，找到``install_openvino_dependencies.sh``文件，然后在该目录中打开终端，输入以下指令：

```shell
sudo -E install_openvino_dependencies.sh
```

运行后输出如下图所示：

<div align=center><img src="https://s2.loli.net/2024/01/08/K7UDMkdT1PEqOzj.png" width=600></div>

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

创建并配置好项目后，就可以直接运行该项目了首先添加一个临时环境变量，主要是设置``OpenVINO.runtime.ubuntu.22-x86_64``的依赖项路径：

```shell
export LD_LIBRARY_PATH=[Project File]/bin/Debug/net6.0/runtimes/ubuntu.22-x86_64/native
```

然后在终端中输入``dotnet run``运行程序即可，结果如下图所示：

<div align=center><img src="https://s2.loli.net/2024/01/08/yQZCTrdSXGpFKLn.png" width=800></div>

此处主要输出了OpenVINO版本信息，如果出现以下结果，说明环境配置成功。

### 🎯总结

至此，我们就完成了在Linux上搭建OpenVINO™C#开发环境，欢迎大家使用，如需要更多信息，可以参考一下内容：

- [OpenVINO™](https://github.com/openvinotoolkit/openvino)
- [OpenVINO CSharp API](https://github.com/guojin-yan/OpenVINO-CSharp-API)

- [OpenVINO CSharp API Samples](https://github.com/guojin-yan/OpenVINO-CSharp-API-Samples)