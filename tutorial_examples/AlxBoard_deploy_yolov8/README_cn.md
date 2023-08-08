# 爱克斯开发板使用OpenVinoSharp部署Yolov8模型

&emsp;    英特尔发行版 [OpenVINO™](www.openvino.ai)工具套件基于oneAPI 而开发，可以加快高性能计算机视觉和深度学习视觉应用开发速度工具套件，适用于从边缘到云的各种英特尔平台上，帮助用户更快地将更准确的真实世界结果部署到生产系统中。通过简化的开发工作流程， OpenVINO™可赋能开发者在现实世界中部署高性能应用程序和算法。

&emsp;    C#是由C和C++衍生出来的一种安全的、稳定的、简单的、优雅的面向对象编程语言。C#综合了VB简单的可视化操作和C++的高运行效率，以其强大的操作能力、优雅的语法风格、创新的语言特性和便捷的面向组件编程的支持成为.NET开发的首选语言。然而 OpenVINO™未提供C#语言接口，这对在C#中使用 OpenVINO™带来了很多麻烦，在之前的工作中，我们推出了[OpenVinoSharp](https://github.com/guojin-yan/OpenVinoSharp/tree/openvinosharp3.0)，旨在推动 OpenVINO™在C#领域的应用，目前已经成功在Window平台实现使用。在本文中，我们将介绍如何在 AIxBoard开发板上基于Linux系统实现OpenVinoSharp。

&emsp;    项目中所使用的代码已上传至OpenVinoSharp仓库中，GitHub网址为：

```
https://github.com/guojin-yan/OpenVINOSharp/blob/openvinosharp3.0/tutorial_examples/AlxBoard_deploy_yolov8/Program.cs
```

## 一、英特尔开发套件 AIxBoard 介绍

<div align=center><span><img src="https://s2.loli.net/2023/08/01/nvUgJ7Hwaj5cm12.png" height=300/></span></div>

### 1.  产品定位

&emsp;    英特尔开发套件 AIxBoard(爱克斯板)是[英特尔开发套件](https://www.intel.cn/content/www/cn/zh/developer/topic-technology/edge-5g/hardware/lan-wa-aixboard-edge-dev-kit.html)官方序列中的一员，专为入门级人工智能应用和边缘智能设备而设计。爱克斯板能完美胜人工智能学习、开发、实训、应用等不同应用场景。该套件预装了英特尔OpenVINO™工具套件、模型仓库和演示案例，便于您轻松快捷地开始应用开发。

&emsp;    套件主要接口与Jetson Nano载板兼容，GPIO与树莓派兼容，能够最大限度地复用成熟的生态资源。这使得套件能够作为边缘计算引擎，为人工智能产品验证和开发提供强大支持；同时，也可以作为域控核心，为机器人产品开发提供技术支撑。

&emsp;    使用AIxBoard(爱克斯板)开发套件，您将能够在短时间内构建出一个出色的人工智能应用应用程序。无论是用于科研、教育还是商业领域，爱克斯板都能为您提供良好的支持。借助 OpenVINO™ 工具套件，CPU、iGPU 都具备强劲的 AI 推理能力，支持在图像分类、目标检测、分割和语音处理等应用中并行运行多个神经网络。

### 2. 产品参数

| 主控     | 英特尔赛扬N5105 2.0-2.9GHz (formerly Jasper Lake) |
| -------- | ------------------------------------------------- |
| 内存     | 板载LPDDR4x 2933MHz, 4GB/6GB/8GB                  |
| 存储     | 板载 64GB eMMC存储                                |
| 存储扩展 | 1个M.2 Key-M 2242扩展槽, 支持SATA&NVME协议        |
| BIOS     | AMI UEFI BIOS                                     |
| 系统支持 | Ubuntu20.04 LTS                                   |
|          | Winodws 10/11                                     |

### 3. AI推理单元

&emsp;    借助OpenVINO工具，能够实现CPU+iGPU异构计算推理，IGPU算力约为0.6TOPS

| CPU  | INT8/FP16/FP32    |
| ---- | ----------------- |
| iGPU | INT8/FP16 0.6TOPS |
| GNA  | 高斯及神经加速器  |

## 二、配置 .NET 环境

&emsp;    .NET 是一个免费的跨平台开源开发人员平台 ，用于构建多种应用程序。下面将演示 AIxBoard 如何在 Ubuntu 20.04 上安装 .NET环境，支持 .NET  Core 2.0-3.1 系列 以及.NET 5-8 系列 ，如果你的 AIxBoard 使用的是其他Linux系统，你可以参考[在 Linux 发行版上安装 .NET - .NET | Microsoft Learn](https://learn.microsoft.com/zh-cn/dotnet/core/install/linux)。

### 1. 添加 Microsoft 包存储库

&emsp;    使用 APT 进行安装可通过几个命令来完成。 安装 .NET 之前，请运行以下命令，将 Microsoft 包签名密钥添加到受信任密钥列表，并添加包存储库。

&emsp;    打开终端并运行以下命令：

```bash
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
```

&emsp;    下图为输入上面命令后控制台的输出：

<div align=center><span><img src="https://s2.loli.net/2023/08/01/2PGvUJbrR68axWt.png" height=300/></span></div>

### 2. 安装 SDK

&emsp;    .NET SDK 使你可以通过 .NET 开发应用。 如果安装 .NET SDK，则无需安装相应的运行时。 若要安装 .NET SDK，请运行以下命令：

```bash
sudo apt-get update
sudo apt-get install -y dotnet-sdk-3.1
```

&emsp;    下图为安装后控制台的输出：

<div align=center><span><img src="https://s2.loli.net/2023/08/08/tKY3oASu4Tf2dib.png" height=300/></span></div>


### 3. 测试安装

&emsp;    通过命令行可以检查 SDK 版本以及Runtime时版本。

```
dotnet --list-sdks
dotnet --list-runtimes
```

&emsp;    下图为输入测试命令后控制台的输出：

<div align=center><span><img src="https://s2.loli.net/2023/08/08/DQnli6Z3xOpVYvh.png" height=300/></span></div>

### 4. 测试控制台项目

&emsp;    在linux环境下，我们可以通过**dotnet**命令来创建和编译项目，项目创建命令为：

```
dotnet new <project_type> -o <project name>
```

&emsp;    此处我们创建一个简单测试控制台项目：

```
dotnet new console -o test_net6.0
cd test_net6.0
dotnet run
```

&emsp;    下图为输入测试命令后控制台的输出以及项目文件夹文件情况，C#项目会自动创建一个**Program.cs**程序文件，里面包含了程序运行入口主函数，同时还会创建一个**\*.csproj**文件，负责指定项目编译中的一些配置。 

<div align=center><span><img src="https://s2.loli.net/2023/08/01/4WjvIPOZFHsnihQ.png" height=300/><img src="https://s2.loli.net/2023/08/01/FSkNPvLpVt1qbGX.png" height=100/></span></div>

&emsp;    以上就是.NET环境的配置步骤，如果你的环境与本文不匹配，可以通过[.NET 文档 | Microsoft Learn](https://learn.microsoft.com/zh-cn/dotnet/) 获取更多安装步骤。

## 三、安装 OpenVINO Runtime

&emsp;    OpenVINO™ 有两种安装方式: OpenVINO Runtime和OpenVINO Development Tools。OpenVINO Runtime包含用于在处理器设备上运行模型部署推理的核心库。OpenVINO Development Tools是一组用于处理OpenVINO和OpenVINO模型的工具，包括模型优化器、OpenVINO Runtime、模型下载器等。在此处我们只需要安装OpenVINO Runtime即可。

### 1. 下载 OpenVINO Runtime

&emsp;    访问[Download the Intel Distribution of OpenVINO Toolkit](https://www.intel.com/content/www/us/en/developer/tools/openvino-toolkit/download.html?ENVIRONMENT=DEV_TOOLS&OP_SYSTEM=WINDOWS&VERSION=v_2023_0_1&DISTRIBUTION=PIP)页面，按照下面流程选择相应的安装选项，在下载页面，由于我们的设备使用的是**Ubuntu20.04**，因此下载时按照指定的编译版本下载即可。

<div align=center><span><img src="https://s2.loli.net/2023/08/01/BJ9SaVZmz8TUx4l.jpg" height=300/><img src="https://s2.loli.net/2023/08/01/GJbCHiSTtwdj791.jpg" height=200/></span></div>

### 2. 解压安装包

&emsp;    我们所下载的 OpenVINO Runtime 本质是一个C++依赖包，因此我们把它放到我们的系统目录下，这样在编译时会根据设置的系统变量获取依赖项。首先在系统文件夹下创建一个文件夹：

```bash
sudo mkdir -p /opt/intel
```

&emsp;    然后解压缩我们下载的安装文件，并将其移动到指定文件夹下：

```bash
tar -xvzf l_openvino_toolkit_ubuntu20_2023.0.1.11005.fa1c41994f3_x86_64.tgz
sudo mv l_openvino_toolkit_ubuntu20_2023.0.1.11005.fa1c41994f3_x86_64 /opt/intel/openvino_2022.3.0
```

### 3. 安装依赖

&emsp;    接下来我们需要安装 OpenVINO Runtime 所许雅的依赖项，通过命令行输入以下命令即可：

```bash
cd /opt/intel/openvino_2022.3.0/
sudo -E ./install_dependencies/install_openvino_dependencies.sh
```

<div align=center><span><img src="https://s2.loli.net/2023/08/01/B9ehCPf8KvXURFg.png" height=300/></span></div>

### 4. 配置环境变量

&emsp;    安装完成后，我们需要配置环境变量，以保证在调用时系统可以获取对应的文件，通过命令行输入以下命令即可：

```bash
source /opt/intel/openvino_2022.3.0/setupvars.sh
```

&emsp;    以上就是 OpenVINO Runtime 环境的配置步骤，如果你的环境与本文不匹配，可以通过[Install OpenVINO™ Runtime — OpenVINO™ documentation — Version(2023.0)](https://docs.openvino.ai/2023.0/openvino_docs_install_guides_install_runtime.html)获取更多安装步骤。

## 四、配置 AlxBoard_deploy_yolov8 项目

&emsp;    项目中所使用的代码已经放在GitHub仓库[AlxBoard_deploy_yolov8](https://github.com/guojin-yan/OpenVINOSharp/blob/openvinosharp3.0/tutorial_examples/AlxBoard_deploy_yolov8/Program.cs)，大家可以根据情况自行下载和使用，下面我将会从头开始一步步构建AlxBoard_deploy_yolov8项目。

### 1. 创建 AlxBoard_deploy_yolov8 项目

&emsp;    在该项目中，我们需要使用OpenCvSharp，该依赖目前在Ubutun平台最高可以支持.NET Core 3.1，因此我们此处创建一个.NET Core 3.1的项目，使用Terminal输入以下指令创建并打开项目文件：

```shell
dotnet new console --framework "netcoreapp3.1" -o AlxBoard_deploy_yolov8
cd AlxBoard_deploy_yolov8
```

<div align=center><span><img src="https://s2.loli.net/2023/08/08/Pln5v6NmeWpg4JG.png" height=200/></span></div>

&emsp;    创建完项目后，将[AlxBoard_deploy_yolov8](https://github.com/guojin-yan/OpenVINOSharp/blob/openvinosharp3.0/tutorial_examples/AlxBoard_deploy_yolov8/Program.cs)的代码内容替换到创建的项目中的**Program.cs**文件中.

### 2. 添加 OpenVINOSharp 依赖

&emsp;    由于OpenVINOSharp 当前正处于开发阶段，还未创建Linux版本的NuGet Package，因此需要通过下载项目源码以项目引用的方式使用。

- **下载源码**

  通过Git下载项目源码，新建一个Terminal，并输入以下命令克隆远程仓库，将该项目放置在AlxBoard_deploy_yolov8同级目录下。

  ```
  git clone https://github.com/guojin-yan/OpenVINOSharp.git
  cd OpenVINOSharp
  ```

  本文的项目目录为：

  ```
  Program--
  		|-AlxBoard_deploy_yolov8
  		|-OpenVINOSharp
  ```

- **修改OpenVINO™ 依赖**

  由于项目源码的OpenVINO™ 依赖与本文设置不同，因此需要修改OpenVINO™ 依赖项的路径，主要通过修改``OpenVINOSharp/src/OpenVINOSharp/native_methods/ov_base.cs``文件即可，修改内容如下：

  ```
  private const string dll_extern = "./openvino2023.0/openvino_c.dll";
  ---修改为--->
  private const string dll_extern = "libopenvino_c.so";
  ```

- **添加项目依赖**

  在Terminal输入以下命令，即可将OpenVINOSharp添加到AlxBoard_deploy_yolov8项目引用中。

  ```
  dotnet add reference ./../OpenVINOSharp/src/OpenVINOSharp/OpenVINOSharp.csproj
  ```

- 添加环境变量

  该项目需要调用OpenVINO™动态链接库，因此需要在当前环境下增加OpenVINO™动态链接库路径：

  ```
  export LD_LIBRARY_PATH=/opt/intel/openvino_2023.0/runtime/lib/intel64
  ```

### 3. 添加OpenCvSharp

- 安装NuGet Package

  OpenCvSharp可以通过NuGet Package安装，只需要在Terminal输入以下命令：

  ```
  dotnet add package OpenCvSharp4_.runtime.ubuntu.20.04-x64
  dotnet add package OpenCvSharp4
  ```

- 添加环境变量

  将以下路径添加到环境变量中：

  ```shell
  export LD_LIBRARY_PATH=/home/ygj/Program/OpenVINOSharp/tutorial_examples/AlxBoard_deploy_yolov8/bin/Debug/netcoreapp3.1/runtimes/ubuntu.20.04-x64/native
  ```

  ``/bin/Debug/netcoreapp3.1/runtimes/ubuntu.20.04-x64/native``是AlxBoard_deploy_yolov8编译后生成的路径，该路径下存放了``libOpenCvSharpExtern.so``文件，该文件主要是封装的OpenCV中的各种接口。也可以将该文件拷贝到项目运行路径下。

- 检测libOpenCvSharpExtern依赖

  由于libOpenCvSharpExtern.so是在其他环境下编译好的动态链接库，本机电脑可能会缺少相应的依赖，因此可以通过``ldd``命令检测。

  ```shell
  ldd libOpenCvSharpExtern.so
  ```

  

  <div align=center><span><img src="https://s2.loli.net/2023/08/08/wZbkAmeg9dIv6BT.png" height=200/></span></div>

  如果输出内容中没有``no found``的，说明不缺少依赖，如果存在，则需要安装缺少的依赖项才可以正常使用。



&emsp;    添加完项目依赖以及NuGet Package后，项目的配置文件内容为：

```properties
<Project Sdk="Microsoft.NET.Sdk">

  <ItemGroup>
    <ProjectReference Include="..\OpenVINOSharp\src\OpenVINOSharp\OpenVINOSharp.csproj" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="OpenCvSharp4" Version="4.8.0.20230708" />
    <PackageReference Include="OpenCvSharp4_.runtime.ubuntu.20.04-x64" Version="4.8.0.20230708" />
  </ItemGroup>

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.1</TargetFramework>
  </PropertyGroup>

</Project>
```



## 五、运行AlxBoard_deploy_yolov8 项目

&emsp;    该项目测试所使用的模型与文件都可以在OpenVINOSharp中找到，因此下面我们通过OpenVINOSharp 仓库下的模型与文件进行测试。

&emsp;    通过dotnet运行，只需要运行以下命令即可

```shell
dotnet run <args>
```

\<args>参数设指的是模型预测类型、模型路径、图片文件路径参数，预测类型输入包括： 'det'、'seg'、'pose'、'cls' 四种类型；默认推理设备设置为'AUTO'，对于'det'、'seg'预测，可以设置<path_to_lable>参数，如果设置该参数，会将结果绘制到图片上，如果未设置，会通过控制台打印出来

### 1. 编译运行 Yolov8-det 模型

编译运行命令为:

```
dotnet run det /home/ygj/Program/OpenVINOSharp/model/yolov8/yolov8s.xml /home/ygj/Program/OpenVINOSharp/dataset/image/demo_2.jpg GPU.0 /home/ygj/Program/OpenVINOSharp/dataset/lable/COCO_lable.txt
```

模型推理输出结果为：

```
---- OpenVINO INFO----
Description : OpenVINO Runtime
Build number: 2023.0.1-11005-fa1c41994f3-releases/2023/0
Set inference device  GPU.0.
[INFO] Loading model files: /home/ygj/Program/OpenVINOSharp/model/yolov8/yolov8s.xml
[INFO] model name: torch_jit
[INFO]    inputs:
[INFO]      input name: images
[INFO]      input type: f32
[INFO]      input shape: Shape : [1, 3, 640, 640]
[INFO]    outputs:
[INFO]      output name: output0
[INFO]      output type: f32
[INFO]      output shape: Shape : [1, 84, 8400]
[INFO] Read image  files: /home/ygj/Program/OpenVINOSharp/dataset/image/demo_2.jpg


  Detection  result : 

1: 0 0.89   (x:744 y:43 width:388 height:667)
2: 0 0.88   (x:149 y:202 width:954 height:507)
3: 27 0.72   (x:435 y:433 width:98 height:284)
```

<div align=center><span><img src="https://s2.loli.net/2023/07/23/YhXH543WkLEJAPS.png" height=300/></span></div>

### 2. 编译运行 Yolov8-cls 模型

编译运行命令为:

```
dotnet run cls /home/ygj/Program/OpenVINOSharp/model/yolov8/yolov8s-cls.xml /home/ygj/Program/OpenVINOSharp/dataset/image/demo_7.jpg GPU.0
```

模型推理输出结果为：

```
---- OpenVINO INFO----
Description : OpenVINO Runtime
Build number: 2023.0.1-11005-fa1c41994f3-releases/2023/0
Set inference device  GPU.0.
[INFO] Loading model files: /home/ygj/Program/OpenVINOSharp/model/yolov8/yolov8s-cls.xml
[INFO] model name: torch_jit
[INFO]    inputs:
[INFO]      input name: images
[INFO]      input type: f32
[INFO]      input shape: Shape : [1, 3, 224, 224]
[INFO]    outputs:
[INFO]      output name: output0
[INFO]      output type: f32
[INFO]      output shape: Shape : [1, 1000]
[INFO] Read image  files: /home/ygj/Program/OpenVINOSharp/dataset/image/demo_7.jpg


 Classification Top 10 result : 

classid probability
------- -----------
294     0.992173
269     0.002861
296     0.002111
295     0.000714
270     0.000546
276     0.000432
106     0.000159
362     0.000147
260     0.000078
272     0.000070
```

### 3. 编译运行 Yolov8-pose 模型

编译运行命令为:

```
dotnet run pose /home/ygj/Program/OpenVINOSharp/model/yolov8/yolov8s-pose.xml /home/ygj/Program/OpenVINOSharp/dataset/image/demo_9.jpg GPU.0
```

模型推理输出结果为：

```
---- OpenVINO INFO----
Description : OpenVINO Runtime
Build number: 2023.0.1-11005-fa1c41994f3-releases/2023/0
Set inference device  GPU.0.
[INFO] Loading model files: /home/ygj/Program/OpenVINOSharp/model/yolov8/yolov8s-pose.xml
[INFO] model name: torch_jit
[INFO]    inputs:
[INFO]      input name: images
[INFO]      input type: f32
[INFO]      input shape: Shape : [1, 3, 640, 640]
[INFO]    outputs:
[INFO]      output name: output0
[INFO]      output type: f32
[INFO]      output shape: Shape : [1, 56, 8400]
[INFO] Read image  files: /home/ygj/Program/OpenVINOSharp/dataset/image/demo_9.jpg


 Classification  result : 

1: 1   0.94   (x:104 y:22 width:152 height:365)  Nose: (188 ,60 ,0.93) Left Eye: (192 ,53 ,0.83) Right Eye: (180 ,54 ,0.90) Left Ear: (196 ,53 ,0.50) Right Ear: (167 ,56 ,0.76) Left Shoulder: (212 ,92 ,0.93) Right Shoulder: (151 ,93 ,0.94) Left Elbow: (230 ,146 ,0.90) Right Elbow: (138 ,142 ,0.93) Left Wrist: (244 ,199 ,0.89) Right Wrist: (118 ,187 ,0.92) Left Hip: (202 ,192 ,0.97) Right Hip: (168 ,193 ,0.97) Left Knee: (184 ,272 ,0.96) Right Knee: (184 ,276 ,0.97) Left Ankle: (174 ,357 ,0.87) Right Ankle: (197 ,354 ,0.88) 
```

<div align=center><span><img src="https://s2.loli.net/2023/07/31/tebOc4qRljZ3riz.png" height=300/></span></div>

### 4. 编译运行 Yolov8-seg 模型

编译运行命令为:

```
dotnet run seg /home/ygj/Program/OpenVINOSharp/model/yolov8/yolov8s-seg.xml /home/ygj/Program/OpenVINOSharp/dataset/image/demo_2.jpg GPU.0 /home/ygj/Program/OpenVINOSharp/dataset/lable/COCO_lable.txt
```

模型推理输出结果为：

```
---- OpenVINO INFO----
Description : OpenVINO Runtime
Build number: 2023.0.1-11005-fa1c41994f3-releases/2023/0
Set inference device  GPU.0.
[INFO] Loading model files: /home/ygj/Program/OpenVINOSharp/model/yolov8/yolov8s-seg.xml
47
[INFO] model name: torch_jit
[INFO]    inputs:
[INFO]      input name: images
[INFO]      input type: f32
[INFO]      input shape: Shape : [1, 3, 640, 640]
[INFO]    outputs:
[INFO]      output name: output0
[INFO]      output type: f32
[INFO]      output shape: Shape : [1, 116, 8400]
[INFO] Read image  files: /home/ygj/Program/OpenVINOSharp/dataset/image/demo_2.jpg
 

  Segmentation  result : 

1: 0 0.90   (x:745 y:42 width:403 height:671)
2: 0 0.86   (x:121 y:196 width:1009 height:516)
3: 27 0.69   (x:434 y:436 width:90 height:280)
```

<div align=center><span><img src="https://s2.loli.net/2023/07/31/CxNgWVLy79ADpKn.png" height=300/></span></div>



## 六、模型运行时间

&emsp;    AIxBoard开发板板载了英特尔赛扬N5105 CPU以及英特尔11代集成显卡，此处对CPU、GPU的推理情况做了一个简单测试，主要检测了模型推理时间，并使用英特尔幻影峡谷进行了同步测试，测试结果如表所示.

|   Device    | CPU: N5105 | GPU: Intel 11th 集显 | CPU: i7-1165G7 | GPU: lntel(R) Iris(R) Xe Graphics |
| :---------: | :--------: | :------------------: | :------------: | :-------------------------------: |
| Yolov8-det  |  586.3ms   |        83.1ms        |    127.1ms     |              19.5ms               |
| Yolov8-seg  |  795.6ms   |       112.5ms        |    140.1ms     |              25.0ms               |
| Yolov8-pose |  609.8ms   |        95.1ms        |    117.2ms     |              23.3ms               |
| Yolov8-cls  |   33.1ms   |        9.2ms         |     6.1ms      |               2.7ms               |

&emsp;    可以看出，英特尔赛扬N5105 CPU在模型推理性能是十分强大的，且搭配的英特尔11代集成显卡，将推理速度提升了6倍左右，针对Yolov8模型，平均处理速度可以达到10FPs。而相比于幻影峡谷的推理速度，AIxBoard开发板推理性能大约为其五分之一，这相比一般的开发板，AIxBoard开发板的算力还是十分强大的。

## 七、总结

&emsp;    在该项目中，我们基于Ubutn 20.04 系统，成功实现了在C#环境下调用OpenVINO™部署深度学习模型，验证了在Linux环境下OpenVINOSharp项目的的可行性，这对后面在Linux环境下开发OpenVINOSharp具有很重要的意义。

&emsp;    除此之外，我们还使用OpenVINOSharp检验了AIxBoard开发板的模型推理能力，最总针对Yolov8模型，平均处理速度可以达到10FPs，这对目前大多数开发板来说，已经达到了很高的推理速度。后续我还会将继续使用OpenVINOSharp在AIxBoard开发板部署更多的深度学习模型。

