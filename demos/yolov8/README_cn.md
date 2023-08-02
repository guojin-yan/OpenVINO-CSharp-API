![OpenVinoSharp](https://socialify.git.ci/guojin-yan/OpenVinoSharp/image?description=1&descriptionEditable=💞%20OpenVINO%20wrapper%20for%20.NET💞%20&forks=1&issues=1&logo=https%3A%2F%2Fs2.loli.net%2F2023%2F01%2F26%2FylE1K5JPogMqGSW.png&name=1&owner=1&pattern=Circuit%20Board&pulls=1&stargazers=1&theme=Light)

简体中文| [English](README-en.md)

# OpenVinoSharp部署Yolov8模型实例

&emsp;    OpenVinoSharp 3.0 版本较2.0版本做了较大程度上的更新，由原来的重构 C++ API 改为直接读取 OpenVINO™ 官方 C API，使得应用更加灵活，所支持的功能更加丰富。OpenVinoSharp 3.0 API 接口多参考 OpenVINO™ C++ API 实现，因此在使用时更加接近C++ API，这对熟悉使用C++ API的朋友会更加友好。

&emsp;    此示例演示了如何使用OpenVinoSharp 3.0 版本 API 部署Yolov8 全系列模型。

&emsp;    该示例支持Yolov8全系列模型，并且支持官方预训练模型以及个人训练模型。

&emsp;    示例中主要会使用以下C# API:

|         Feature          | API                                                          | Description                                                  |
| :----------------------: | ------------------------------------------------------------ | ------------------------------------------------------------ |
| OpenVINO Runtime Version | Ov.get_openvino_version()                                    | Get Openvino API version.                                    |
|     Basic Infer Flow     | Core.read_model(), core.compiled_model(), CompiledModel.create_infer_request(), InferRequest.get_input_tensor(), InferRequest.get_output_tensor(), InferRequest.get_tensor() | Common API to do inference: read and compile a model, create an infer request, configure input and output tensors. |
|    Synchronous Infer     | InferRequest.infer()                                         | Do synchronous inference.                                    |
|     Model Operations     | Model.get_friendly_name(), Model.get_const_input(), Model.get_const_output() | Get inputs and outputs of a model.                           |
|     Node Operations      | Node.get_name(), Node.get_type(), Node.get_shape(            | Get node message.                                            |
|    Tensor Operations     | Tensor.get_shape(), Tensor.set_data<T>(), Tensor.get_size(), Tensor.get_data<T>() | Get a tensor shape, size, data and  set data.                |
|      Yolov8 Process      | ResultProcess.process_det_result(), ResultProcess.process_seg_result(), ResultProcess.process_pose_result, ResultProcess.process_cls_result(), ResultProcess.read_class_names(), ResultProcess.draw_det_result(), ResultProcess.draw_pose_result(), ResultProcess.draw_seg_result(), ResultProcess.print_result() | Process and draw yolov8 result.                              |

下方所列出信息已经经过代码运行验证测试，如有其他环境测试成功欢迎大家进行补充：

| 选项     | 值                                                      |
| -------- | ------------------------------------------------------- |
| 支持模型 | Yolov8-det、Yolov8-cls、Yolov8-pose、Yolov8-seg         |
| 模型格式 | OpenVINO™ 工具包中间表示(\*.xml,\*.bin)，ONNX (\*.onnx) |
| 支持设备 | CPU、iGPU、dGPU(未测试)                                 |
| 运行环境 | Window 10 、Window 11;                                  |
| 编译环境 | Visual Studio 11，.NET 6.0                              |

## 工作原理

&emsp;    项目运行时，示例程序会读取用户指定路径模型、测试图片以及类别文件，准备模型推理测试的相关数据；将指定模型和图像加载到OpenVINO™ 推理核心并进行同步推理，然后将获取的推理数据加载到自定义的Yolov8数据处理类中进行结果处理。

&emsp;    项目中使用的OpenVINO™相关组件已经封装到OpenVinoSharp中，无需安装在单独安装OpenVINO™。

## 依赖安装

&emsp;    项目中所有依赖项均可以通过NuGet 包安装：

- **OpenVinoSharp**

&emsp;    可以通过Visual Studio 自带的 NuGet 工具进行安装，或者直接下项目配置中直接加入下面代码，

<div align=center><span><img src="https://s2.loli.net/2023/07/31/UFAgRbBuhcsqOEv.png" height=300/></span></div>

&emsp;    如果项目是通过**dotnet**编译，可以通过下面语句添加对应的包：

```
dotnet add package OpenVinoSharp.win
```

## 构建

将源码**Program.cs**添加到当前项目中，并按照上面要求添加相关的NuGet包。

<div align=center><span><img src="https://s2.loli.net/2023/07/31/Bal8QopgDmbePAJ.png" height=300/></span></div>

项目可以通过Visual Studio构建和编译，只需要通过右击项目->生成即可。

如果项目通过**dotnet**编译，依次运行以下命令：

```
dotnet add package OpenVinoSharp.win # 添加OpenVinoSharp包
dotnet build  # 编译项目
```

项目编译后，会在``\bin\Debug\net6.0``目录下生成可执行文件。

## 运行

```
dotnet run <type> <path_to_model> <image_to_path> <device_name> <path_to_lable>
```

如果需要运行示例，需要同时指定模型预测类型、模型路径、图片文件路径，预测类型输入包括：'det'、'seg'、'pose'、'cls'四种类型；默认推理设备设置为'AUTO'，对于'det'、'seg'预测，可以设置<path_to_lable>参数，如果设置该参数，会将结果绘制到图片上，如果未设置，指挥姐姐过打印出来。

**模型文件可以通过以下方式获取：**

### 模型获取

项目中所使用的模型全部由**ultralytics**平台下载，下面是下载示例：

1. 安装ultralytics

   ```
   pip install ultralytics
   ```

2. 导出 Yolov8模型

   ```
   yolo export model=yolov8s.pt format=onnx  #yolov8-det
   yolo export model=yolov8s-cls.pt format=onnx  #yolov8-cls
   yolo export model=yolov8s-pose.pt format=onnx  #yolov8-pose
   yolo export model=yolov8s-seg.pt format=onnx  #yolov8-seg
   ```

3. 转为IR格式

   IR格式此处通过OpenVINO™的模型优化工具实现，需要安装OpenVINO™ Python 版本，具体实现可以参考[Model Preparation — OpenVINO™ documentation](https://docs.openvino.ai/2023.0/openvino_docs_model_processing_introduction.html)，也可以通过命令行实现：

   ```
   mo -input_model yolov8s.onnx
   ```

### 结果展示

程序运行会输出模型推理信息和推理结果:

#### Yolov8-det 模型推理结果

```
E:\GitSpace\OpenVinoSharp\demos\yolov8>dotnet run det E:\GitSpace\OpenVinoSharp\model\yolov8s.xml E:\GitSpace\OpenVinoSharp\dataset\image\demo_2.jpg CPU E:\GitSpace\OpenVinoSharp\dataset\lable\COCO_lable.txt
---- OpenVINO INFO----
Description : OpenVINO Runtime
Build number: 2023.0.1-11005-fa1c41994f3-releases/2023/0
Set inference device  CPU.
[INFO] Loading model files: E:\GitSpace\OpenVinoSharp\model\yolov8s.xml
[INFO] model name: torch_jit
[INFO]    inputs:
[INFO]      input name: images
[INFO]      input type: f32
[INFO]      input shape: Shape : [1, 3, 640, 640]
[INFO]    outputs:
[INFO]      output name: output0
[INFO]      output type: f32
[INFO]      output shape: Shape : [1, 84, 8400]
[INFO] Read image  files: E:\GitSpace\OpenVinoSharp\dataset\image\demo_2.jpg

  Detection  result :

1: 0    0.89   (x:744 y:43 width:388 height:667)
2: 0    0.88   (x:149 y:202 width:954 height:507)
3: 27   0.72   (x:435 y:433 width:98 height:284)
```

<div align=center><span><img src="https://s2.loli.net/2023/07/23/YhXH543WkLEJAPS.png" height=300/></span></div>

#### Yolov8-pose 模型推理结果

```
E:\GitSpace\OpenVinoSharp\demos\yolov8>dotnet run pose E:\GitSpace\OpenVinoSharp\model\yolov8s-pose.xml E:\GitSpace\OpenVinoSharp\dataset\image\demo_9.jpg CPU
---- OpenVINO INFO----
Description : OpenVINO Runtime
Build number: 2023.0.1-11005-fa1c41994f3-releases/2023/0
Set inference device  CPU.
[INFO] Loading model files: E:\GitSpace\OpenVinoSharp\model\yolov8s-pose.xml
[INFO] model name: torch_jit
[INFO]    inputs:
[INFO]      input name: images
[INFO]      input type: f32
[INFO]      input shape: Shape : [1, 3, 640, 640]
[INFO]    outputs:
[INFO]      output name: output0
[INFO]      output type: f32
[INFO]      output shape: Shape : [1, 56, 8400]
[INFO] Read image  files: E:\GitSpace\OpenVinoSharp\dataset\image\demo_9.jpg

 Classification  result :

1: 1   0.94   (x:104 y:22 width:151 height:365)  Nose: (188 ,60 ,0.92) Left Eye: (192 ,52 ,0.83) Right Eye: (179 ,54 ,0.89) Left Ear: (197 ,52 ,0.48) Right Ear: (166 ,56 ,0.75) Left Shoulder: (212 ,91 ,0.92) Right Shoulder: (151 ,94 ,0.94) Left Elbow: (230 ,145 ,0.89) Right Elbow: (138 ,143 ,0.92) Left Wrist: (244 ,199 ,0.88) Right Wrist: (118 ,187 ,0.91) Left Hip: (202 ,191 ,0.97) Right Hip: (169 ,193 ,0.97) Left Knee: (183 ,271 ,0.96) Right Knee: (183 ,275 ,0.96) Left Ankle: (174 ,358 ,0.87) Right Ankle: (197 ,354 ,0.88)
```

<div align=center><span><img src="https://s2.loli.net/2023/07/31/tebOc4qRljZ3riz.png" height=300/></span></div>

#### Yolov8-seg 模型推理结果

```
E:\GitSpace\OpenVinoSharp\demos\yolov8>dotnet run seg E:\GitSpace\OpenVinoSharp\model\yolov8s-seg.xml E:\GitSpace\OpenVinoSharp\dataset\image\demo_2.jpg CPU E:\GitSpace\OpenVinoSharp\dataset\lable\COCO_lable.txt
---- OpenVINO INFO----
Description : OpenVINO Runtime
Build number: 2023.0.1-11005-fa1c41994f3-releases/2023/0
Set inference device  CPU.
[INFO] Loading model files: E:\GitSpace\OpenVinoSharp\model\yolov8s-seg.xml
[INFO] model name: torch_jit
[INFO]    inputs:
[INFO]      input name: images
[INFO]      input type: f32
[INFO]      input shape: Shape : [1, 3, 640, 640]
[INFO]    outputs:
[INFO]      output name: output0
[INFO]      output type: f32
[INFO]      output shape: Shape : [1, 116, 8400]
[INFO] Read image  files: E:\GitSpace\OpenVinoSharp\dataset\image\demo_2.jpg

  Segmentation  result :

1: 0    0.90   (x:745 y:41 width:402 height:671)
2: 0    0.86   (x:118 y:196 width:1011 height:515)
3: 27   0.70   (x:434 y:436 width:90 height:280)
```

<div align=center><span><img src="https://s2.loli.net/2023/07/31/CxNgWVLy79ADpKn.png" height=300/></span></div>



#### Yolov8-cls 模型推理结果

```
E:\GitSpace\OpenVinoSharp\demos\yolov8>dotnet run cls E:\GitSpace\OpenVinoSharp\model\yolov8s-cls.xml E:\GitSpace\OpenVinoSharp\dataset\image\demo_7.jpg CPU
---- OpenVINO INFO----
Description : OpenVINO Runtime
Build number: 2023.0.1-11005-fa1c41994f3-releases/2023/0
Set inference device  CPU.
[INFO] Loading model files: E:\GitSpace\OpenVinoSharp\model\yolov8s-cls.xml
[INFO] model name: torch_jit
[INFO]    inputs:
[INFO]      input name: images
[INFO]      input type: f32
[INFO]      input shape: Shape : [1, 3, 224, 224]
[INFO]    outputs:
[INFO]      output name: output0
[INFO]      output type: f32
[INFO]      output shape: Shape : [1, 1000]
[INFO] Read image  files: E:\GitSpace\OpenVinoSharp\dataset\image\demo_7.jpg

 Classification Top 10 result :

classid probability
------- -----------
294     0.992171
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

