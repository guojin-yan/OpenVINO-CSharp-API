# 行人摔倒检测 — 基于 OpenVINO C# API 部署PP-Human

&emsp;    随着人口老龄化问题的加重，独居老人、空巢老人数量在不断上升，因此如何保障独居老人、空巢老人健康生活和人身安全至关重要。而对于独居老人、空巢老人，如果出现摔倒等情况而不会及时发现，将会对其健康安全造成重大影响。本项目主要研究为开发一套摔倒自动识别报警平台，使用视频监控其采集多路视频流数据，使用行人检测算法、关键点检测算法以及摔倒检测算法实现对行人摔倒自动识别，并根据检测情况，对相关人员发送警报，实现对老人的及时看护。该装置可以布置在养老院等场所，通过算法自动判别，可以大大降低人力成本以及保护老人的隐私。该项目应用场景不知可以用到空巢老人，还可以用到家庭中的孕妇儿童、幼儿园等场景，实现对儿童的摔倒检测。
&emsp;    项目中采用OpenVINO部署行人检测算法、关键点检测算法以及摔倒检测算法实现对行人摔倒自动识别算法，并在

# 1.  英特尔开发套件

## 1.1 OpenVINO

&emsp;    英特尔发行版 [OpenVINO™](www.openvino.ai)工具套件基于oneAPI 而开发，可以加快高性能计算机视觉和深度学习视觉应用开发速度工具套件，适用于从边缘到云的各种英特尔平台上，帮助用户更快地将更准确的真实世界结果部署到生产系统中。通过简化的开发工作流程， OpenVINO™可赋能开发者在现实世界中部署高性能应用程序和算法。

![image-20230919110341141](https://s2.loli.net/2023/09/19/sNM7hleuwYv6mbG.png)

&emsp;    OpenVINO™ 2023.1于2023年9月18日发布，该工具包带来了挖掘生成人工智能全部潜力的新功能。生成人工智能的覆盖范围得到了扩展，通过PyTorch*等框架增强了体验，您可以在其中自动导入和转换模型。大型语言模型（LLM）在运行时性能和内存优化方面得到了提升。聊天机器人、代码生成等的模型已启用。OpenVINO更便携，性能更高，可以在任何需要的地方运行：在边缘、云中或本地。

##  1.2 AIxBoard 介绍

<div align=center><span><img src="https://s2.loli.net/2023/08/01/nvUgJ7Hwaj5cm12.png" height=300/></span></div>

###  产品定位

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

&emsp;    PP-Human提供了训练好的行人跟踪模型，此处只需要下载，并将其解压到指定文件夹中：

```shell
weget https://bj.bcebos.com/v1/paddledet/models/pipeline/mot_ppyoloe_l_36e_pipeline.zip
```



&emsp;    此处模型裁剪主要是在Paddle模型格式下进行裁剪，裁剪方式参考的jiangjiajun (https://github.com/jiangjiajun/PaddleUtils)提供的模型裁剪方式，为了方便使用，当前项目提供了模型裁剪工具包，在“./paddle_model_process/”文件夹下，利用命令进行模型裁剪：

```shell
python prune_paddle_model.py --model_dir mot_ppyoloe_l_36e_pipeline --model_filename model.pdmodel --params_filename model.pdiparams --output_names tmp_16 concat_14.tmp_0 --save_dir export_model
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
mo --input_model mot_ppyoloe_l_36e_pipeline.onnx
```

## 3.2 PP-TinyPose人体姿态识别

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
cd .\openvino\tools
mo --input_model paddle/model.pdmodel --input_shape [1,3,256,192]
```

&emsp;    经过上述指令模型转换后，可以在当前文件夹下找到转换后的三个文件。

&emsp;    由于OpenVINOTM支持FP16推理，此处为了对比推理时间，也已并将模型转为FP16格式：

```shell
mo --input_model paddle/model.pdmodel --data_type FP16 --input_shape [1,3,256,192]
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
cd .\openvino\tools
mo --input_model paddle/model.pdmodel
```

&emsp;    经过上述指令模型转换后，可以在当前文件夹下找到转换后的三个文件。

&emsp;    由于OpenVINOTM支持FP16推理，此处为了对比推理时间，也已并将模型转为FP16格式：

```
mo --input_model paddle/model.pdmodel --data_type FP16
```



 