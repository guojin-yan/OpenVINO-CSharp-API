## 在Windows上搭建OpenVINO™C#开发环境

- [在Windows上搭建OpenVINO™C#开发环境](#在Windows上搭建OpenVINO™C#开发环境)
  - [🧩简介](#🧩简介)
  - [🔮安装.NET运行环境](#🔮安装.NET运行环境)
  - [🎈配置C#开发环境](#🎈配置C#开发环境)
  - [🎨创建并配置C#项目](#🎨创建并配置C#项目)
    - [第一步：创建OpenVINO™C#项目](#第一步：创建OpenVINO™C#项目)
    - [第二步：添加项目依赖](#第二步：添加项目依赖)
  - [🎁测试OpenVINO™C#项目](#🎁测试OpenVINO™C#项目)
  - [🎯总结](#🎯总结)

### 🧩简介

本文将从零开始详述在**Windows10/11**上搭建**OpenVINO™ CSharp**开发环境，并对 **OpenVINO™ CSharp API **环境进行简单测试。

### 🔮安装.NET运行环境

**[.NET](https://learn.microsoft.com/zh-cn/dotnet/)** 是由 **Microsoft** 创建的一个免费的、跨平台的、开源开发人员平台，可以使用 C#、F# 或 Visual Basic 语言编写代码，用于构建许多不同类型的应用程序，可以在任何兼容的操作系统上(Windows、Linux、Mac OS等)运行。

 Microsoft官方提供了**.NET**环境的详细安装流程，大家可以参考以下文章进行安装：[在 Windows 上安装 .NET](https://learn.microsoft.com/zh-cn/dotnet/core/install/windows).

### 🎈配置C#开发环境

在Windows平台创建并编译C#代码可以使用的平台比较多，最容易使用以及最简单的是使用**Visual Studio IDE**，但是**Visual Studio IDE**目前只支持Windows环境，如果想实现跨平台使用，最好的组合为:

- 代码构建工具：**dotnet **
- 代码编辑工具：**Visual Studio Code**

所以在此处我们将讲解使用**Visual Studio IDE**方式编译并运行项目，在Linux以及MacOS系统中讲解使用**dotnet && Visual Studio Code **组合的方式。**Visual Studio IDE**安装方式可以参考Microsoft官方提供的安装教程：

- [Visual Studio 2022 IDE](https://visualstudio.microsoft.com/zh-hans/vs/)
- [在 Windows 上安装 .NET](https://learn.microsoft.com/zh-cn/dotnet/core/install/windows)

### 🎨创建并配置C#项目

#### 第一步：创建 OpenVINO™C# 项目

使用**Visual Studio 2022 IDE**创建一个 OpenVINO™  C# 测试项目，按照下图流程进行创建即可.

<div align=center><img src="https://s2.loli.net/2024/01/08/kInKFwbhU5tRPXp.png" width=800></div>

#### 第二步：添加项目依赖

 OpenVINO™ C# 项目所使用的依赖环境，此处可以完全使用 NuGet Package 安装所需程序集，其安装流程如下图所示：

<div align=center><img src="https://s2.loli.net/2024/01/08/m5In3luJe1H9PFt.png" width=800></div>

在此处，主要需要安装两类 NuGet 程序包，分别为：

- **OpenVINO CSharp API**
  - **OpenVINO.CSharp.API**：OpenVINO CSharp API 项目核心程序集。
  - **OpenVINO.runtime.win**：OpenVINO 在Windows平台运行所需依赖项。
- **OpenCvSharp**
  - **OpenCvSharp4**：OpenCvSharp4 项目核心程序集。
  - **OpenCvSharp4.runtime.win**：OpenCvSharp4 在Windows平台运行所需依赖项。

其中**OpenVINO CSharp API**是此处我们重点介绍的项目，**OpenCvSharp**是在C#中使用的一个开源视觉处理库。

### 🎁测试OpenVINO™C#项目

首先添加测试代码，用户可以直接将下述代码替换到上文所创建的项目中的**Program.cs**文件中。

```csharp
using OpenCvSharp;
using OpenVinoSharp;
namespace test_openvino_csharp
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

            // -------- 测试 OpenCvSharp 安装--------
            //创建一张大小为300*300颜色为绿色的三通道彩色图像
            Mat img = new Mat(300, 300, MatType.CV_8UC3, new Scalar(255, 0, 0));
            Cv2.ImShow("img", img);
            Cv2.WaitKey(0);
        }
    }
}
```

创建并配置好项目后，就可以直接运行该项目了，使用**Visual Studio 2022 IDE**可以直接点击运行案件运行程序，程序运行后输出如下所示：

<div align=center><img src="https://s2.loli.net/2024/01/08/zCWDTHmpG74A2rO.png" width=800></div>

此处主要输出了OpenVINO版本信息，并使用OpenCvSharp绘制了一张蓝色图片，如果出现以下结果，说明环境配置成功。

### 🎯总结

至此，我们就完成了在Windows上搭建OpenVINO™C#开发环境，欢迎大家使用，如需要更多信息，可以参考一下内容：

- [OpenVINO™](https://github.com/openvinotoolkit/openvino)
- [OpenVINO CSharp API](https://github.com/guojin-yan/OpenVINO-CSharp-API)

- [OpenVINO CSharp API Samples](https://github.com/guojin-yan/OpenVINO-CSharp-API-Samples)