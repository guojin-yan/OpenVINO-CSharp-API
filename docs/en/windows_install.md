# Windows Installation OpenVINOSharp

&emsp;    OpenVINOSharp is mainly based on OpenVINO™  Developed with C #, supports Windows 10/11 version, and has been tested under x64 architecture.

## C# Environmental Configuration

&emsp;    C# is a new programming language that is not only object-oriented, but also type safe. Developers can use C # to generate multiple secure and reliable applications running in. NET. The C# environment installation can be configured according to the following article.

- [Install .NET on Windows - .NET | Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/core/install/windows?tabs=net70)

- [.NET Framework installation guide - .NET Framework | Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/framework/install/)

## OpenVINOSharp Installation

&emsp;    Due to the convenience of developing the C # language in the Windows environment, OpenVINOSharp's NuGet Package has been developed, which can be installed directly through the C # NuGet Package during use. When packaging NuGet Package, also include OpenVINO ™  The officially compiled Dongta Link Library file is packaged into the NuGet Package, so you only need to add OpenVINOSharp here to use it. The following demonstrates the installation under two different compilation methods:

- **Visual Studio Platform**

&emsp;   The Visual Studio editor comes with the **NuGet Package ** management feature of C #, so it can be installed directly through the **NuGet Package **.

<div align=center><span><img src="https://s2.loli.net/2023/07/31/UFAgRbBuhcsqOEv.png" height=200/></span></div>

- **dotnet**

&emsp;    Dotnet is a compilation platform for the C # language, which can quickly compile C # projects from the command line. If using dotnet compilation, OpenVINOSharp can be installed by:

```
dotnet add package OpenVinoSharp.win
```

&emsp;    **Note: ** Currently, there may be issues with the installation and use of **. NET Framework version 4.8 **. Therefore, after the project is generated, it is necessary to move the **opencv_c.dll ** file from the openvino2023.0 folder in the program directory to the program directory, as shown in the figure:

<div align=center><span><img src="https://s2.loli.net/2023/07/27/yNAUTqfw8azXg6i.png" height=200/></span></div>

