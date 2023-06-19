# OpenVINOTM<sup>TM</sup> 2022 安装文档



[TOC]



## 更新日志

- **2023.1.26 更新**
  - 更新  OpenVINO<sup>TM </sup> 2022.1更新2022.3教程

- **2022.9.24 更新**
  - 更新  OpenVINO<sup>TM </sup> 2022.1更新2022.2教程

- **2022.9.19 更新**
  - 创建安装文档；
  - 更新 OpenVINOTM<sup>TM</sup> 2022.1 runtime time以及完整版安装教程



## 1. OpenVINOTM<sup>TM</sup> 安装环境和安装特性介绍

### 1.1 OpenVINOTM<sup>TM</sup> 安装环境

&emsp;OpenVINOTM<sup>TM</sup> 是一款依赖计算机或智能设备硬件的模型推理部署套件，在边缘启用基于 CNN 的深度学习推理，支持跨英特尔<sup>®</sup> CPU、英特尔® 集成显卡、英特尔<sup>®</sup> 神经计算棒 2 和英特尔<sup>®</sup>  视觉加速器设计与英特尔<sup>®</sup>  Movidius<sup>TM</sup> VPU 的异构执行。此处我们介绍在Windows平台上的安装和使用。

&emsp;由于OpenVINOTM<sup>TM</sup> 是一款依赖计算机或智能设备硬件的模型推理部署套件，因此它可以在自家的设备上很好的运行，经过测试，在Windows平台上，非 intel CPU设备也可以运行OpenVINOTM<sup>TM</sup>，例如AMD推出的CPU，此处安装使用的设备为AMD R7-5800H；对于显卡设备不支持英伟达等其他显卡，仅支持英特尔自家显卡。

- CPU：AMD R7-5800H

- GPU：NVIDIA GeForce RTX 3060 Laptop GPU 6G

- 操作系统：Windows 11

- 虚拟环境：Anaconda Navigator (anconda3)

- C++编译环境：Visual Studio 2022

- Python编译环境：Python 3.9

### 1.2 OpenVINOTM<sup>TM</sup> 安装特性

&emsp;OpenVINO<sup>TM</sup>最新版本为2022.1版本， OpenVINO<sup>TM</sup>安装分为完整版安装与不完整版(Runtime)安装。

&emsp;完整版安装会安装所有部件，包括模型优化器和推理部署套件运行工具(Runtime)，目前该方式支持PIP安装方式，因此建议采用PIP安装在虚拟环境中，防止安装出错照成电脑环境出现问题。该安装方式至此Python、C++同时使用，由于安装路径较长，对C++使用不太方便，因此该安装方式最好使用Python编程。

&emsp;非完整版安装主要安装编译环境(Runtime)，安装比较简单，且一般会安装到根目录下，方便使用，因此如果不使用模型优化且情况下，或者使用C++编译环境的话，建议选用此方式。

## 2.OpenVINOTM<sup>TM</sup>  Runtime安装

### 2.1 下载安装包

&emsp;OpenVINOTM<sup>TM</sup> 官方网站为``openvino.ai``或者直接访问[Intel® Distribution of OpenVINO™ Toolkit](https://www.intel.com/content/www/us/en/developer/tools/openvino-toolkit/overview.html)网站，进入后点击``Free Douwnload``，进入下载页面。

![image-20220919193047292](https://s2.loli.net/2023/01/26/4xICi5AmKdE6qjz.png)



&emsp;选择安装依赖，依次选择``Runtime``、``Windows``、``2022.1``、``C++``、``Offline Installer``安装选择，最后选择``Download``，下载安装包。根据OpenVINOTM<sup>TM</sup>  更新，当前页面可能会发生变化。

![image-20220919193655398](https://s2.loli.net/2023/01/26/gEARXLuvzcBwOWF.png)

&emsp;下载完为以下的安装文件。

![image-20220919200938500](https://s2.loli.net/2023/01/26/L1Z4BKD6gQEhq7R.png)

### 2.2 软件安装

&emsp;（1）双击打开安装文件，首先会提示一个安装文件解压，随便新建一个位置，只能装后会及时删除。

![1](https://s2.loli.net/2023/01/26/NXS3zv6p2VhqfkU.png)

&emsp;（2）解压完成后会自动弹出安装页面，如下图所示，点击``continue``

![3](https://s2.loli.net/2023/01/26/PxQMhogpAw6fUGu.png)

&emsp;（3）Summary中展示的是安装的硬件要求以及安装位置，安装硬件环境无需考虑；安装位置此处不可以修改，将其安装到指定位置，方便后面的使用。选择解释许可协议，选择``Recommended Installation``安装方式。

![4](https://s2.loli.net/2023/01/26/fyu9VXKlPJLWg1v.png)

&emsp;（4）选择接收相关信息，点击``Install``，进行安装。

![5](https://s2.loli.net/2023/01/26/5amesCqru3iZ9tG.png)



&emsp;（5）此处会提示几个警告，不用管，可以保存截图后续安装。

&emsp;第一条和第三条是CPU和GPU，无需考虑；第二个为C++编译器，在编译自带的源码时会使用，使用Visual Studio 2019和Visual Studio 2022都可以。

![6](https://s2.loli.net/2023/01/26/rUZMD3gcGyV8bqx.png)

&emsp;（6）安装后点击关闭即可

![7](https://s2.loli.net/2023/01/26/L37QSM2cqsnkTwe.png)

&emsp;（7）安装完成后会出现以下文件

![9](https://s2.loli.net/2023/01/26/FD5IJsOc7emrVzl.png)

### 2.3 添加环境变量

&emsp;在系统环境变量中，增加以下三个环境变量。

```shell
C:\Program Files (x86)\Intel\openvino_2022.1.0.643\runtime\3rdparty\tbb\bin
C:\Program Files (x86)\Intel\openvino_2022.1.0.643\runtime\bin\intel64\Release
C:\Program Files (x86)\Intel\openvino_2022.1.0.643\runtime\bin\intel64\Debug
```



![image-20220919202633500](https://s2.loli.net/2023/01/26/bxRShBpKJTiHENM.png)

## 3. OpenVINOTM<sup>TM</sup>完整版安装

&emsp;完整版安装需要许多依赖项，我们此处采用虚拟环境安装。

### 3.1 创建虚拟环境

&emsp;在Anaconda 3 中创建一个虚拟环境，命名为``openvino2022_1``，并打开环境的命令窗口

![image-20220919202935803](https://s2.loli.net/2023/01/26/w4oR8Ix2gbylaOA.png)

![image-20220919203427329](https://s2.loli.net/2023/01/26/tNZJyIHg5Oxp8dB.png)

### 3.2 安装命令

&emsp;在OpenVINO<sup>TM</sup>页面，依次选择``Dev Tools``、``Windows``、``2022.1``、``PIP``、``ONNX``(深度学习框架，可以按照需求选择)，最后复制下面的安装命令到我们的虚拟环境的命令窗口，进行安装。

![image-20220919203257668](https://s2.loli.net/2023/01/26/obD7WsPqRypGlA1.png)

![image-20220919203710772](https://s2.loli.net/2023/01/26/2F7zWTkQmtVnov8.png)

&emsp;输入命令后，会根据安装要求，自动下载并安装到虚拟环境中。

![image-20220919203933874](https://s2.loli.net/2023/01/26/q64WOZNvG3LInzE.png)

&emsp;安装时间会比较长，且有可能因为网速问题安装失败，如果失败就重新输入上述命令进行安装。

&emsp;安装结束后如下图所示。

![image-20220919210854124](https://s2.loli.net/2023/01/26/vU8XcgF9PdmKIR5.png)

## 4. OpenVINO<sup>TM </sup> 2022.1更新2022.2教程

&emsp;OpenVINO<sup>TM</sup> 工具套件2022.1版于2022年3月22日正式发布，与以往版本相比发生了重大革新，提供预处理API函数、ONNX前端API、AUTO 设备插件，并且支持直接读入飞桨模型，在推理中中支持动态改变模型的形状，这极大地推动了不同网络的应用落地。

&emsp;2022年9月23日，OpenVINO<sup>TM</sup> 工具套件2022.2版推出，对2022.1进行了微调，以包括对英特尔最新 CPU 和离散 GPU 的支持，以实现更多的人工智能创新和机会。

&emsp;由于2022.2版同属2022系列，除了增加对英特尔最新 CPU 和离散 GPU 的支持，其他API函数没有发生变化，因此我们可以直接对其进行更新，这样不会影响我们的以前的项目使用。

&emsp;如果2022.1版大家选择的Runtime C++本地安装，这里推荐给大家一个简单的升级办法，无需卸载原有版本，可以实现两个版本共存。

### 4.1  新版文件下载与设置

&emsp;首先进入[storage.openvinotoolkit.org](https://storage.openvinotoolkit.org/repositories/openvino/packages/2022.2/windows/)网站，下载最新版的Cmake后的文件：

![image-20230126160448463](https://s2.loli.net/2023/01/26/H7zqtEDvMoy2LpY.png)

&emsp;按照自己的系统下载自己需要的文件。文件下载好后解压该文件，并将该文件夹修改名为``openvino_2022.2.0.7713``，最后将文件夹拷贝到下面路径中：

```
C:\Program Files (x86)\Intel
```

&emsp;该路径为Openvino2022.1安装的路径

![image-20220924102036288](https://s2.loli.net/2023/01/26/A7TcoLCbkZRrWgy.png)

### 4.2 配置环境变量

&emsp;在系统环境变量下，增加新的版本路径：

```
C:\Program Files (x86)\Intel\openvino_2022.2.0.7713\runtime\bin\intel64\Debug
C:\Program Files (x86)\Intel\openvino_2022.2.0.7713\runtime\bin\intel64\Release
C:\Program Files (x86)\Intel\openvino_2022.2.0.7713\runtime\3rdparty\tbb\bin
```

![image-20220924102410848](https://s2.loli.net/2023/01/26/nKY5gvprEWByOAf.png)

&emsp;完成以上步骤，2022.2版OpenVINO<sup>TM</sup>就可以使用了。

## 5. OpenVINO<sup>TM </sup> 2022.1更新2022.3教程

### 5.1  新版文件下载与设置

&emsp;首先进入[Download Intel® Distribution of OpenVINO™ Toolkit](https://www.intel.com/content/www/us/en/developer/tools/openvino-toolkit/download.html)网站，下载最新版的文件。

&emsp;选择安装依赖，依次选择``Runtime``、``Windows``、``2022.1``、``C++``、``OpenVINO Archives``安装选择，最后选择``Download Archives``，下载安装包。根据OpenVINOTM<sup>TM</sup>  更新，当前页面可能会发生变化。

![image-20230126160953995](https://s2.loli.net/2023/01/26/ADnmj5fOoqpxTkP.png)

&emsp;进入以下页面后，点击最新版安装包进行下载。

![image-20230126161144925](https://s2.loli.net/2023/01/26/WejE8BsMnCGgJ9m.png)

&emsp;按照自己的系统下载自己需要的文件。文件下载好后解压该文件，并将该文件夹修改名为``openvino_openvino_2022.3.0.9052``，最后将文件夹拷贝到下面路径中：

```
C:\Program Files (x86)\Intel
```

&emsp;该路径为Openvino2022.1安装的路径

![image-20230126161442229](https://s2.loli.net/2023/01/26/pbLxHDTjC8udAyX.png)



### 4.2 配置环境变量

&emsp;在系统环境变量下，增加新的版本路径：

```
C:\Program Files (x86)\Intel\openvino_2022.3.0.9052\runtime\bin\intel64\Debug
C:\Program Files (x86)\Intel\openvino_2022.3.0.9052\runtime\bin\intel64\Release
C:\Program Files (x86)\Intel\openvino_2022.3.0.9052\runtime\3rdparty\tbb\bin
```



![image-20230126161600124](https://s2.loli.net/2023/01/26/4N6uXR2yrPTamkY.png)

&emsp;完成以上步骤，2022.3版OpenVINO<sup>TM</sup>就可以使用了。