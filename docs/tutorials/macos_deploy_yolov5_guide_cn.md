<div><center><b>
    <font color="34,63,93" size="7"> 
        在 MacOS 上使用 OpenVINO C# API 部署 Yolov5 （OpenCvSharp 版）
    </font>
</b></center></div>



> <div><b>
> <font color=red size="5">&emsp;前言</font>
> </b></div>
>
> 

## 1. 前言

英特尔发行版 OpenVINO™ 工具套件基于 oneAPI 而开发，可以加快高性能计算机视觉和深度学习视觉应用开发速度工具套件，适用于从边缘到云的各种英特尔平台上，帮助用户更快地将更准确的真实世界结果部署到生产系统中。通过简化的开发工作流程，OpenVINO™ 可赋能开发者在现实世界中部署高性能应用程序和算法。

![image-20240118194333790](https://s2.loli.net/2024/01/18/vc5VeJ2BQknhitp.png)

​    OpenVINO™ C# API 是一个 OpenVINO™ 的 .Net wrapper，应用最新的 OpenVINO™ 库开发，通过 OpenVINO™ C API 实现 .Net 对 OpenVINO™ Runtime 调用，使用习惯与 OpenVINO™ C++ API 一致。OpenVINO™ C# API 由于是基于 OpenVINO™ 开发，所支持的平台与 OpenVINO™ 完全一致，具体信息可以参考 OpenVINO™。通过使用 OpenVINO™ C# API，可以在 .NET、.NET Framework等框架下使用 C# 语言实现深度学习模型在指定平台推理加速。

​    YOLOv5是革命性的 "单阶段"对象检测模型的第五次迭代，旨在实时提供高速、高精度的结果，是世界上最受欢迎的视觉人工智能模型，代表了Ultralytics对未来视觉人工智能方法的开源研究，融合了数千小时研发中积累的经验教训和最佳实践。

![image-20240121214213149](https://s2.loli.net/2024/01/21/AlOgk92WGQBXMJ6.png)

## 2. 环境搭建

项目所需环境全部可以通过**Nuget Package**进行安装，主要包含以下两类程序包：

- **OpenVINO.CSharp.API**

  - OpenVINO.CSharp.API

  - OpenVINO.CSharp.API.Extensions

  - OpenVINO.CSharp.API.Extensions.OpenCvSharp

  - OpenVINO.runtime.macos-arm64

关于在MacOS上搭建OpenVINO™C# 开发环境请参考以下文章： [在MacOS上搭建OpenVINO™C#开发环境](..\inatall\Install_OpenVINO_CSharp_MacOS_cn.md) 

- **OpenCvSharp**

  - OpenCvSharp4

  - OpenCvSharp4.Extensions

  - OpenCvSharp4.runtime.osx_arm64

关于在MacOS上搭建OpenCvSharp 开发环境请参考以下文章： [【OpenCV】在MacOS上使用OpenCvSharp](https://mp.weixin.qq.com/s/8njRodtg7lRMggBfpZDHgw) 

## 3.模型获取

使用



