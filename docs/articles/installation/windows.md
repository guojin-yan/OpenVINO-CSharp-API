# Windows 安装指南 / Windows Installation Guide

本文档介绍如何在 Windows 平台上安装 OpenVINO C# API。

## 系统要求 / System Requirements

- Windows 10/11 (x64)
- Windows Server 2019/2022
- .NET Framework 4.6.1+ 或 .NET Core 3.1+ 或 .NET 5+

## 安装步骤 / Installation Steps

### 1. 安装 .NET SDK

从 [.NET 官网](https://dotnet.microsoft.com/download) 下载并安装 .NET SDK。

### 2. 安装 NuGet 包

在项目中安装以下 NuGet 包：

```bash
dotnet add package JYPPX.OpenVINO.CSharp.API
dotnet add package OpenVINO.runtime.win
```

### 3. 验证安装

创建一个测试程序验证安装：

```csharp
using OpenVinoSharp;

class Program
{
    static void Main()
    {
        using Core core = new Core();
        Console.WriteLine("OpenVINO 版本: " + core.get_version());
        Console.WriteLine("安装成功！");
    }
}
```

## 常见问题 / Troubleshooting

- 如遇 DLL 加载错误，请检查 Visual C++ Redistributable 是否已安装
- 确保项目目标平台为 x64

---

*文档完善中... / Documentation in progress...*
