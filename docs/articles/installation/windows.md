# Windows 安装指南 / Windows Installation Guide

This page explains how to install and verify OpenVINO C# API on Windows.

本文说明如何在 Windows 上安装并验证 OpenVINO C# API。

## Requirements / 系统要求

- Windows 10/11 x64 or Windows Server 2019/2022
- .NET Framework 4.6.1+, .NET Core 3.1+, or .NET 5+
- A x64 process when using the `win-x64` runtime package

## Install Packages / 安装 NuGet 包

For core OpenVINO inference:

基础 OpenVINO 推理：

```bash
dotnet add package JYPPX.OpenVINO.CSharp.API
dotnet add package OpenVINO.runtime.win
```

For GenAI APIs:

GenAI API：

```bash
dotnet add package JYPPX.OpenVINO.CSharp.API
dotnet add package JYPPX.OpenVINO.GenAI.runtime.win
```

## Verify Installation / 验证安装

```csharp
using System;
using OpenVinoSharp;

class Program
{
    static void Main()
    {
        Version version = Ov.get_openvino_version();
        Console.WriteLine($"OpenVINO: {version.description} {version.buildNumber}");

        using Core core = new Core();
        foreach (string device in core.GetAvailableDevices())
        {
            Console.WriteLine(device);
        }
    }
}
```

## Unicode Paths / Unicode 路径

The default `Core`, `ReadModel`, and `CompileModel` APIs use explicit UTF-8 string marshalling and work across platforms.

默认 `Core`、`ReadModel`、`CompileModel` API 使用显式 UTF-8 字符串 marshalling，可跨平台使用。

Windows runtimes that export OpenVINO Unicode path APIs can also use explicit Unicode methods:

导出 OpenVINO Unicode 路径 API 的 Windows runtime 也可以使用显式 Unicode 方法：

```csharp
using Core core = new Core();
using Model model = core.ReadModelUnicode(@"D:\模型\yolo26n.xml");
using CompiledModel compiled = core.CompileModelUnicode(@"D:\模型\yolo26n.xml", "CPU");
```

If the installed runtime does not export the Unicode C API, these methods throw `PlatformNotSupportedException`. The normal UTF-8 APIs are unaffected.

如果已安装 runtime 未导出 Unicode C API，这些方法会抛出 `PlatformNotSupportedException`。普通 UTF-8 API 不受影响。

## Troubleshooting / 常见问题

- If native DLL loading fails, confirm the runtime package matches the process architecture.
- If C++ runtime dependencies are missing, install the Microsoft Visual C++ Redistributable.
- If only core APIs are used, do not install a GenAI runtime package unless you need `OpenVinoSharp.GenAI`.
