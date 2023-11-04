# Pedestrian fall detection - Deploying PP-Human based on OpenVINO C # API

&emsp;    With the aggravation of population aging, the number of elderly people living alone and empty-nesters is constantly increasing. Therefore, it is crucial to ensure the healthy life and personal safety of elderly people living alone and empty-nesters. For elderly people living alone and empty-nesters, if they fall and are not detected in a timely manner, it will have a significant impact on their health and safety. The main research of this project is to develop an automatic recognition and alarm platform for falls, which uses video surveillance to collect multi-channel video stream data. Pedestrian detection algorithms, key point detection algorithms, and fall detection algorithms are used to achieve automatic recognition of pedestrian falls. Based on the detection situation, alerts are sent to relevant personnel to achieve timely care for the elderly. This device can be installed in nursing homes and other places, and can be automatically identified through algorithms, greatly reducing labor costs and protecting the privacy of the elderly. The application scenario of this project is unknown, which can include empty nest elderly people, pregnant women and children in families, kindergartens, and other scenarios to achieve fall detection for children.
&emsp;    In the project, OpenVINO is used to deploy pedestrian detection algorithms, key point detection algorithms, and fall detection algorithms to achieve automatic recognition of pedestrian falls. Multiple models are deployed on the AIxBoard development board using the OpenVINO C # API combined with application scenarios.

&emsp;   All the code used in the project is open source on GitHub, and the project link is：[PP-Human_Fall_Detection](https://github.com/guojin-yan/OpenVINO-CSharp-API/tree/csharp3.0/tutorial_examples/PP-Human_Fall_Detection)

# 1.  Intel Development Suite

## 1.1 OpenVINO

&emsp;    Intel Distribution [OpenVINO ™](www.openvino. ai) tool kit is developed based on the oneAPI and can accelerate the development speed of high-performance computer vision and deep learning visual applications. It is suitable for various Intel platforms from the edge to the cloud, helping users deploy more accurate real-world results into production systems faster. By simplifying the development workflow, OpenVINO ™ Enable developers to deploy high-performance applications and algorithms in the real world.

![image-20230919110341141](https://s2.loli.net/2023/09/19/sNM7hleuwYv6mbG.png)

&emsp;    OpenVINO™  Released on September 18, 2023, 2023.1 brings new features to tap into the full potential of generating artificial intelligence. The coverage of generating artificial intelligence has been expanded, and the experience has been enhanced through frameworks such as PyTorch *, where you can automatically import and transform models. The Large Language Model (LLM) has been improved in terms of runtime performance and memory optimization. The models for chat robots, code generation, etc. have been enabled. OpenVINO is more portable, with higher performance, and can run anywhere needed: at the edge, in the cloud, or locally.

##  1.2 AIxBoard 

<div align=center><span><img src="https://s2.loli.net/2023/08/01/nvUgJ7Hwaj5cm12.png" height=300/></span></div>

###  Product Positioning

&emsp;    英特尔开发套件 AIxBoard(爱克斯板)是[英特尔开发套件](https://www.intel.cn/content/www/cn/zh/developer/topic-technology/edge-5g/hardware/lan-wa-aixboard-edge-dev-kit.html)官方序列中的一员，专为入门级人工智能应用和边缘智能设备而设计。爱克斯板能完美胜人工智能学习、开发、实训、应用等不同应用场景。该套件预装了英特尔OpenVINO™工具套件、模型仓库和演示案例，便于您轻松快捷地开始应用开发。

&emsp;    套件主要接口与Jetson Nano载板兼容，GPIO与树莓派兼容，能够最大限度地复用成熟的生态资源。这使得套件能够作为边缘计算引擎，为人工智能产品验证和开发提供强大支持；同时，也可以作为域控核心，为机器人产品开发提供技术支撑。

&emsp;    使用AIxBoard(爱克斯板)开发套件，您将能够在短时间内构建出一个出色的人工智能应用应用程序。无论是用于科研、教育还是商业领域，爱克斯板都能为您提供良好的支持。借助 OpenVINO™ 工具套件，CPU、iGPU 都具备强劲的 AI 推理能力，支持在图像分类、目标检测、分割和语音处理等应用中并行运行多个神经网络。

###  产品参数

| 主控     | 英特尔赛扬N5105 2.0-2.9GHz (formerly Jasper Lake) |
| -------- | ------------------------------------------------- |
| 内存     | 板载LPDDR4x 2933MHz, 4GB/6GB/8GB                  |
| 存储     | 板载 64GB eMMC存储                                |
| 存储扩展 | 1个M.2 Key-M 2242扩展槽, 支持SATA&NVME协议        |
| BIOS     | AMI UEFI BIOS                                     |
| 系统支持 | Ubuntu20.04 LTS                                   |
|          | Winodws 10/11                                     |

### AI推理单元

&emsp;    借助OpenVINO工具，能够实现CPU+iGPU异构计算推理，IGPU算力约为0.6TOPS

| CPU  | INT8/FP16/FP32    |
| ---- | ----------------- |
| iGPU | INT8/FP16 0.6TOPS |
| GNA  | 高斯及神经加速器  |

# 2. PaddleDetection实时行人分析工具PP-Human

&emsp;    飞桨(PaddlePaddle)是集深度学习核心框架、工具组件和服务平台为一体的技术先进、功能完备的开源深度学习平台，已被中国企业广泛使用，深度契合企业应用需求，拥有活跃的开发者社区生态。提供丰富的官方支持模型集合，并推出全类型的高性能部署和集成方案供开发者使用。是中国首个自主研发、功能丰富、开源开放的产业级深度学习平台。

&emsp;    PaddleDetection是一个基于PaddlePaddle的目标检测端到端开发套件，内置30+模型算法及250+预训练模型，覆盖目标检测、实例分割、跟踪、关键点检测等方向，其中包括服务器端和移动端高精度、轻量级产业级SOTA模型、冠军方案和学术前沿算法，并提供配置化的网络模块组件、十余种数据增强策略和损失函数等高阶优化支持和多种部署方案。在提供丰富的模型组件和测试基准的同时，注重端到端的产业落地应用，通过打造产业级特色模型|工具、建设产业应用范例等手段，帮助开发者实现数据准备、模型选型、模型训练、模型部署的全流程打通，快速进行落地应用。

![image-20230919110426327](https://s2.loli.net/2023/09/19/fAxM5OPvlDGytna.png)

&emsp;    在实际应用中，打架、摔倒、异常闯入等异常行为的发生率高、后果严重，使得其成为了安防领域中重点监控的场景。飞桨目标检测套件PaddleDetection中开源的行人分析工具PP-Human提供了五大异常行为识别、26种人体属性分析、人流计数、跨镜ReID四大产业级功能，其中异常行为识别功能覆盖了对摔倒、打架、打电话、抽烟、闯入等行为的检测。

![image-20230919110449020](https://s2.loli.net/2023/09/19/uO5eZ9d6nLt1mEq.png)

&emsp;    如图所示，PP-Human支持单张图片、图片文件夹单镜头视频和多镜头视频输入，经目标检测以及特征关联，实现属性识别、关键点检测、轨迹/流量计数以及行为识别等功能。此处基于OpenVINOTM模型部署套件，进行多种模型联合部署，实现实时行人行为识别，此处主要实现行人摔倒识别。

![image-20230919110654821](https://s2.loli.net/2023/09/19/UNFM1KGbP8DWjXh.png)

# 3. 预测模型获取与转换

## 3.1 PP-YOLOE行人跟踪

### 模型介绍

&emsp;    PP-YOLOE是基于PP-YOLOv2的卓越的单阶段Anchor-free模型，超越了多种流行的YOLO模型，可以通过width multiplier和depth multiplier配置。PP-YOLOE避免了使用诸如Deformable Convolution或者Matrix NMS之类的特殊算子，以使其能轻松地部署在多种多样的硬件上。此处主要利用PP-Yoloe模型进行行人跟踪。

表 2 PP-Yoloe Paddle格式模型信息

|          | Input             | Output       |                         |                         |
| -------- | ----------------- | ------------ | ----------------------- | ----------------------- |
| 名称     | image             | scale_factor | multiclass_nms3_0.tmp_0 | multiclass_nms3_0.tmp_2 |
| 形状     | [-1, 3, 640, 640] | [-1, 2]      | [8400, 6]               | [-1]                    |
| 数据类型 | Float32           | Float32      | Float32                 | Int32                   |

&emsp;    表 2 为Paddle格式下PP-YOLOE模型的输入与输出相关信息，该模型包括两个输入与两个输出，可以实现行人识别，该模型可以直接在飞桨平台下载。但由于PP-Yoloe模型无法在OpenVINOTM平台直接部署，需要进行节点裁剪，即裁剪掉scale_factor输入节点，裁剪后模型结构如表 3 所示，具体如何裁剪后续讲解。

表 3  PP-YOLOE Paddle格式模型信息

|          | Input             | Output          |               |
| -------- | ----------------- | --------------- | ------------- |
| 名称     | image             | concat_14.tmp_0 | tmp_20        |
| 形状     | [-1, 3, 640, 640] | [-1, 1, 8400]   | [-1, 8400, 4] |
| 数据类型 | Float32           | Float32         | Float32       |

### 模型下载与转换

#### （1）PaddlePaddle模型下载与裁剪：

&emsp;    PP-Human提供了训练好的行人跟踪模型，此处只需要下载，并将其解压到model文件夹中：

```shell
wget https://bj.bcebos.com/v1/paddledet/models/pipeline/mot_ppyoloe_l_36e_pipeline.zip
```

&emsp;    此处模型裁剪主要是在Paddle模型格式下进行裁剪，裁剪方式参考的jiangjiajun (https://github.com/jiangjiajun/PaddleUtils)提供的模型裁剪方式，为了方便使用，当前项目提供了模型裁剪工具包，在“./paddle_model_process/”文件夹下，利用命令进行模型裁剪，下面为项目裁剪模型文件目录：

```
PP-Human_Fall_Detection
	|
	├──── model
	|	├──── infer_cfg.yml
	|	├──── model.pdiparams
	|	├──── model.pdiparams.info
	|	├──── model.pdmodel
	├──── paddle_model_process
	|	├──── paddle_infer_shape.py
	|	├──── prune_paddle_model.py
```

下面面为模型裁剪指令：

```shell
python ./paddle_model_process/prune_paddle_model.py --model_dir ./model/mot_ppyoloe_l_36e_pipeline --model_filename model.pdmodel --params_filename model.pdiparams --output_names tmp_16 concat_14.tmp_0 --save_dir ./model/export_model
```

&emsp;    如表 4 所示，提供了模型裁剪命令说明，大家可以根据自己设置进行模型裁剪，当前命令裁剪的模型目前已经进行测试，完全符合当前阶段的要求。

表 4 模型裁剪命令说明

| 标志位            | 说明             | 输入                       |
| ----------------- | ---------------- | -------------------------- |
| --model_dir       | 模型文件路径     | mot_ppyoloe_l_36e_pipeline |
| --model_filename  | 静态图模型文件   | model.pdmodel              |
| --params_filename | 模型配置文件信息 | model.pdiparams            |
| --output_names    | 输出节点名       | tmp_16 concat_14.tmp_0     |
| --save_dir        | 模型保存路径     | export_model               |

#### （2）转换为ONNX格式：

&emsp;    该方式需要安装paddle2onnx和onnxruntime模块。导出方式比较简单，可以进行模型输入固定，此处使用的都为bath_size=1，在命令行中输入以下指令进行转换：

```shell
paddle2onnx --model_dir mot_ppyoloe_l_36e_pipeline --model_filename model.pdmodel --params_filename model.pdiparams --input_shape_dict "{'image':[1,3,640,640]}" --opset_version 11 --save_file mot_ppyoloe_l_36e_pipeline.onnx
```

#### （3）转成IR格式

&emsp;    IR格式为OpenVINOTM原生支持的模型格式，此处主要通过OpenVINOTM工具进行转换，直接在命令行输入以下指令即可：

```shell
ovc mot_ppyoloe_l_36e_pipeline.onnx --output_model mot_ppyoloe_l_36e_pipeline
```

## 3.2 PP-TinyPose 人体姿态识别

###  模型介绍

&emsp;    PP-TinyPose 是PaddlePaddle提供了关键点识别模型，PP-TinyPose在单人和多人场景均达到性能SOTA，同时对检测人数无上限，并且在微小目标场景有卓越效果，助力开发者高性能实现异常行为识别、智能健身、体感互动游戏、人机交互等任务。同时扩大数据集，减小输入尺寸，预处理与后处理加入AID、UDP和DARK等策略，保证模型的高性能。实现速度在FP16下122FPS的情况下，精度也可达到51.8%AP，不仅比其他类似实现速度更快，精度更是提升了130%。此处使用的是dark_hrnet_w32_256x192模型，该模型输入与输出如下表所示

表 5 dark_hrnet_w32_256x192 Paddle 模型信息

|          | Input                     | Output                  |                |
| -------- | ------------------------- | ----------------------- | -------------- |
| 名称     | image                     | conv2d_585.tmp_1        | argmax_0.tmp_0 |
| 形状     | [bath_size, 3, 256,  192] | [bath_size, 17, 64, 48] | [bath_size,17] |
| 数据类型 | Float32                   | Float32                 | Int64          |

&emsp;    表 5为Paddle格式下dark_hrnet_w32_256x192模型的输入与输出相关信息，除此以外，飞桨还提供了输入大小为128×96的模型，这两类模型在部署时所有操作基本一致，主要差别就是输入与输出的形状不同。分析模型的输入和输出可以获取以下几个点：

&emsp;    第一模型的输入与conv2d_585.tmp_1节点输出形状，呈现倍数关系，具体是输入的长宽是输出的四倍， 因此我们可以通过输入形状来推算输出的大小。

&emsp;    第二模型argmax_0.tmp_00节点输出为预测出的17个点的灰度图，因此后续在进行数据处理是，只需要寻找到最大值所在位置，就可以找到近似的位置。

### 模型下载与转换

#### （1）PaddlePaddle模型下载方式：

&emsp;    命令行直接输入以下代码，或者浏览器输入后面的网址即可。

```shell
wget https://bj.bcebos.com/v1/paddledet/models/pipeline/dark_hrnet_w32_256x192.zip
```

&emsp;    下载好后将其解压到文件夹中，便可以获得Paddle格式的推理模型。

#### （2）转换为ONNX格式：

&emsp;    该方式需要安装paddle2onnx和onnxruntime模块。在命令行中输入以下指令进行转换，其中转换时需要指定input_shape，否者推理时间会很长：

```shell
paddle2onnx --model_dir dark_hrnet_w32_256x192 --model_filename model.pdmodel --params_filename model.pdiparams --input_shape_dict "{'image':[1,3,256,192]}" --opset_version 11 --save_file dark_hrnet_w32_256x192.onnx
```

#### （3）转换为IR格式

&emsp;    利用OpenVINOTM模型优化器，可以实现将ONNX模型转为IR格式。在OpenVINOTM环境下，切换到模型优化器文件夹，直接使用下面指令便可以进行转换。

```shell
ovc dark_hrnet_w32_256x192.onnx --output_model dark_hrnet_w32_256x192
```

&emsp;    经过上述指令模型转换后，可以在当前文件夹下找到转换后的三个文件。

&emsp;    由于OpenVINOTM支持FP16推理，此处为了对比推理时间，也已并将模型转为FP16格式：

```shell
ovc dark_hrnet_w32_256x192.onnx --output_model dark_hrnet_w32_256x192 --compress_to_fp16=True
```

## 3.3 ST-GCN基于关键点的行为识别

### 模型介绍

&emsp;    摔倒行为识别模型使用了ST-GCN，并基于PaddleVideo套件完成模型训练，此处可以直接下载飞桨提供的训练好的模型。

表 6 ST-GCN Paddle 模型信息

|          | Input             | Output            |
| -------- | ----------------- | ----------------- |
| 名称     | data_batch_0      | reshape2_34.tmp_0 |
| 形状     | [1, 2, 50, 17, 1] | [1, 2]            |
| 数据类型 | Float32           | Float32           |

&emsp;    表 6为Paddle格式下ST-GCN模型的输入与输出相关信息，该模型输入为人体骨骼关键识别，由于摔倒是一个连续的过程，因此需要同时输入50帧的关键点结果，因此该模型不支持单张图片的预测，只支持视频的推理预测；其模型输出为是否摔倒的概率。

### 模型下载与转换

#### （1）PaddlePaddle模型下载方式：

&emsp;    命令行直接输入以下代码，或者浏览器输入后面的网址即可。

```shell
wget https://bj.bcebos.com/v1/paddledet/models/pipeline/STGCN.zip
```



下载好后将其解压到文件夹中，便可以获得Paddle格式的推理模型。

#### （2）转换为ONNX格式：

&emsp;    该方式需要安装paddle2onnx和onnxruntime模块。在命令行中输入以下指令进行转换：

```shell
paddle2onnx --model_dir STGCN --model_filename model.pdmodel --params_filename model.pdiparams --opset_version 11 --save_file STGCN.onnx
```



#### （3）转换为IR格式

&emsp;    利用OpenVINOTM模型优化器，可以实现将ONNX模型转为IR格式。在OpenVINOTM环境下，切换到模型优化器文件夹，直接使用下面指令便可以进行转换。

```shell
ovc STGCN.onnx --output_model STGCN --compress_to_fp16=True
```

&emsp;    经过上述指令模型转换后，可以在当前文件夹下找到转换后的三个文件。

&emsp;    由于OpenVINOTM支持FP16推理，此处为了对比推理时间，也已并将模型转为FP16格式：

```shell
ovc STGCN.onnx --output_model STGCN --compress_to_fp16=True
```

# 4. 配置 PP-Human_Fall_Detection 项目

&emsp;    项目中所使用的代码已经放在GitHub仓库[PP-Human_Fall_Detection](https://github.com/guojin-yan/OpenVINO-CSharp-API/tree/csharp3.0/tutorial_examples/PP-Human_Fall_Detection)，大家可以根据情况自行下载和使用，下面我将会从头开始一步步构建PP-Human_Fall_Detection项目。

## 4.1 环境配置

&emsp;    在该项目中主要需要配置.NET编译运行环境、OpenVINO Runtime、OpenCvSharp环境，其配置流程可以参考上一篇文章：[【2023 Intel有奖征文】爱克斯开发板使用OpenVINO C# API部署Yolov8模型 ](https://www.51openlab.com/article/477/)。

## 4.2 创建 AlxBoard_deploy_yolov8 项目

&emsp;    在该项目中，我们需要使用OpenCvSharp，该依赖目前在Ubutun平台最高可以支持.NET Core 3.1，因此我们此处创建一个.NET Core 3.1的项目，使用Terminal输入以下指令创建并打开项目文件：

```shell
dotnet new console --framework "netcoreapp3.1" -o PP-Human_Fall_Detection
cd PP-Human_Fall_Detection
```

<div align=center><span><img src="https://s2.loli.net/2023/09/19/erVUhqJW2mjR81Q.png" height=200/></span></div>

## 4.3 添加项目源码

&emsp;    前文中我们已经提供了项目源码链接，大家可以直接再在源码使用，此处由于篇幅限制，因此此处不对源码做太多的讲解，只演示如何使用项目源码配置当前项目。将项目源码中的``PP-Human``文件夹和``HumanFallDown.cs``、``Program.cs``文件复制到当前项目中，最后项目的路径关系如下所示：

```
PP-Human_Fall_Detection
   ├──── PP-Human
   |    ├──── Common.cs
   |    ├──── PP-TinyPose.cs
   |    ├──── PP-YOLOE.cs
   |    └──── STGCN.cs
   ├──── HumanFallDown.cs
   ├──── PP-Human_Fall_Detection.csproj
   └──── Program.cs
```

## 4.4 添加 OpenVINO C# API

&emsp;    OpenVINO C# API 目前只支持克隆源码的方式实现，首先使用Git克隆以下源码，只需要在Terminal输入以下命令：

```shell
git clone https://github.com/guojin-yan/OpenVINO-CSharp-API.git
```

<div align=center><span><img src="https://s2.loli.net/2023/09/19/1s5S3ILxwhlvTPK.png" height=200/></span></div>

&emsp;    然后将该项目文件夹下的除了src文件夹之外的文件都删除掉，然后项目的文件路径入下所示：

```
PP-Human_Fall_Detection
   ├──── OpenVINO-CSharp-API
   |    ├──── src
   |         └──── CSharpAPI
   ├──── PP-Human
   |    ├──── Common.cs
   |    ├──── PP-TinyPose.cs
   |    ├──── PP-YOLOE.cs
   |    └──── STGCN.cs
   ├──── HumanFallDown.cs
   ├──── PP-Human_Fall_Detection.csproj
   └──── Program.cs
```

&emsp;    最后在当前项目中添加项目引用，只需要在Terminal输入以下命令：

```
dotnet add reference ./OpenVINO-CSharp-API/src/CSharpAPI/CSharpAPI.csproj
```

<div align=center><span><img src="https://s2.loli.net/2023/09/19/bDZMj3oHRtuUynT.png" height=200/></span></div>

## 4.5 添加 OpenCvSharp

- 安装NuGet Package

  OpenCvSharp可以通过NuGet Package安装，只需要在Terminal输入以下命令：

  ```
  dotnet add package OpenCvSharp4_.runtime.ubuntu.20.04-x64
  dotnet add package OpenCvSharp4
  ```

<div align=center><span><img src="https://s2.loli.net/2023/09/19/9TZaryvWtm7CRS4.png" weight=500/></span></div>

<div align=center><span><img src="https://s2.loli.net/2023/09/19/oc5CeBs84XKxvta.png" weight=500/></span></div>

- 添加环境变量

  将以下路径添加到环境变量中：

  ```shell
  export LD_LIBRARY_PATH=/home/ygj/Program/OpenVINOSharp/tutorial_examples/AlxBoard_deploy_yolov8/bin/Debug/netcoreapp3.1/runtimes/ubuntu.20.04-x64/native
  ```

  ``/bin/Debug/netcoreapp3.1/runtimes/ubuntu.20.04-x64/native``是项目编译后生成的路径，该路径下存放了``libOpenCvSharpExtern.so``文件，该文件主要是封装的OpenCV中的各种接口。也可以将该文件拷贝到项目运行路径下。

# 5. 测试 PP-Human_Fall_Detection 项目

## 5.1 创建视频读取器

当前项目测试内容为视频，此处主要通过OpenCV的VideoCapture类进行读取，实现逐帧读取测试图片。

```c#
// 视频路径
string test_video = @"E:\Git_space\基于Csharp和OpenVINO部署PP-Human\demo\摔倒.mp4";
// string test_video = @"E:\Git_space\基于Csharp和OpenVINO部署PP-Human\demo\摔倒2.mp4";
// 视频读取器
VideoCapture video_capture = new VideoCapture(test_video);
// 视频帧率
double fps = video_capture.Fps;
// 视频帧数
int frame_count = video_capture.FrameCount;
Console.WriteLine("video fps: {0}, frame_count: {1}", Math.Round(fps), frame_count);
```



## 5.2 行人识别

&emsp;    利用创建好的视频读取器逐帧读取视频图片，将其带入到yoloe_predictor预测器中进行预测，并将预测结果绘制到图片上，期预测结果存放到ResBboxs类中，方便进行数据传输。

```c#
// 读取视频帧
if (!video_capture.Read(frame))
{
    Console.WriteLine("视频读取完毕！！{0}", frame_id);
    break;
}
// 复制可视化图片
visualize_frame = frame.Clone();
// 行人识别
ResBboxs person_result = yoloe_predictor.predict(frame);
// 判断是否识别到人
if (person_result.bboxs.Count < 1)
{
    continue;
}
// 绘制行人区域
yoloe_predictor.draw_boxes(person_result, ref visualize_frame);
```



&emsp;    通过上述代码，可以实现视频所有帧图片预测，将预测结果保存到本地，如图 所示，经过预测器预测，可以很好的捕获到运动的行人。

<div align=center><span><img src="https://guojin-yan.github.io/resources/gif/pedestrian_detection_results.gif" weight=500/></span></div>

## 5.3 关键点识别

&emsp;    上一步通过行人跟踪，捕捉到了行人，由于行人是在不断运动的，因此在进行关键点预测时，需要先进行裁剪，将行人区域按照指定要求裁剪下来，并根据裁剪结果对行人关键点进行预测，此处使用的是bath_size=1的预测，适合单人预测，如果出现多人时，可以采用同时预测。

```C#
// 裁剪行人区域
List<Rect> point_rects;
List<Mat> person_rois = tinyPose_predictor.get_point_roi(frame, person_result.bboxs, out point_rects);
for (int p = 0; p < person_rois.Count; p++)
{
    // 关键点识别
    float[,] person_point = tinyPose_predictor.predict(person_rois[p]);
    KeyPoints key_point = new KeyPoints(frame_id, person_point, point_rects[p]);
    //Console.WriteLine(key_point.bbox);
    flag_stgcn = mot_point.add_point(key_point);
    tinyPose_predictor.draw_poses(key_point, ref visualize_frame);
} 
```

&emsp;    经过模型预测，第一会将预测结果存到结果容器“mot_point”中，用于后面的摔倒识别；另一点将模型预测结果绘制到图像中，如图所示。

<div align=center><span><img src="https://guojin-yan.github.io/resources/gif/pedestrian_key_point_detection_results.gif" weight=500/></span></div>

## 5.4  摔倒识别

&emsp;    摔倒识别需要同时输入50帧人体关键点识别结果，所以在开始阶段需要积累50帧的关键点识别结果，此处采用自定义的结果保存容器“MotPoint”实现，该容器可以实现保存关键点结果，并将关键点识别结果与上一帧结果进行匹配，当容器已满会返回推理标志，当满足识别条件是，就会进行依次模型预测；同时会清理前20帧数据，继续填充识别结果等待下一次满足条件。

```c#
if (flag_stgcn)
{
    List<List<KeyPoints>> predict_points = mot_point.get_points();
    for (int p = 0; p < predict_points.Count; p++)
    {
        Console.WriteLine(predict_points[p].Count);
        fall_down_result = stgcn_predictor.predict(predict_points[p]);
    }
}
stgcn_predictor.draw_result(ref visualize_frame, fall_down_result, person_result.bboxs[0]); 
```



&emsp;    摔倒识别结果为是否摔倒以及对应的权重，此处主要是在满足条件的情况下，进行一次行为识别，并将识别结果绘制到图像上。

## 5.5 模型联合部署实现行人摔倒识别

&emsp;    通过行人跟踪、关键点识别以及行为识别三个模型联合预测，可以实现行人的行为识别，其识别效果如图 14 所示。在该图中分别包含了三个模型的识别结果：行人位置识别与跟踪是通过PP-YOLOE模型实现的，该模型为下一步关键点识别提供了图像范围，保证了关键点识别的结果；人体骨骼关键点识别时通过dark_hrnet模型实现，为后续行为识别提供了输入；最终的行为识别通过ST-GCN模型实现，其识别结果会知道了行人预测框下部，可以看到预测结果与行人是否摔倒一致。

<div align=center><span><img src="https://guojin-yan.github.io/resources/gif/pedestrian_ffall_detection_results.gif" weight=500/></span></div>



## 6. 总结

&emsp;    在该项目中，基于C#和OpenVINO联合部署PP-YOLOE行人检测模型、dark_hrnet人体关键点识别模型以及ST-GCN行为识别模型，实现行人摔倒检测。

&emsp;    在该项目中，主要存在的难点一是PP-YOLOE模型无法直接使用OpenVINO部署，需要进行裁剪，裁剪掉无法使用的节点，并根据裁剪的节点，处理模型的输出数据；难点二是处理好行人预测与关键点模型识别内容的关系，在进行多人识别时，要结合行人识别模型进行对应的人体关键点识别，并且要当前帧识别结果要对应上一帧行人识别结果才可以保证识别的连续性。