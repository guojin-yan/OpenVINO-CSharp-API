简体中文| [English](README-en.md)

# Yolov8 模型部署示例

该示例将演示 Yolov8 系列模型如何使用 OpenVinoSharp 对图像进行预测，支持 Yolov8 官方下载的所有模型，示例对应代码：[OpenVinoSharp/demo/yolov8_simple_demo](https://github.com/guojin-yan/OpenVinoSharp/tree/openvinosharp2.1/demo/yolov8_simple_demo)

模型会使用以下 C# API 接口：

| 功能                         | API                                                          |
| ---------------------------- | ------------------------------------------------------------ |
| 初始化推理核心，基本推理流程 | public Core(string model, string device, string cache_dir = "") |
| 加载待推理数据               | public void load_input_data(string node_name, byte[] image_data, ulong image_size, int type) |
| 模型推理                     | public void infer()                                          |
| 读取推理结果                 | public T[] read_infer_result<T>(string node_name, int data_size) |
| 清理推理内存                 | public void delet()                                          |

为了更好地处理Yolov8推理结果，此处自定义了结果处理类，在namespace Yolov8下，为方便大家理解，此处简单对各个类作相关解释：

|        类名        | 解释                                                         |
| :----------------: | ------------------------------------------------------------ |
|       Result       | 推理结果类，主要存放推理结果；支持存放目标检测结果(类别、置信度、预测框)、图像分类结果(类别、置信度)、人体关键点识别结果(类别、置信度、预测框、人体关键点结果)、区域分割结果(类别、置信度、预测框、分割区域) |
|     ResultBase     | 推理结果处理基类，定义了一些结果处理的通用属性和方法。       |
|  DetectionResult   | Yolov8 目标检测识别结果处理类。                              |
|     ClasResult     | Yolov8 图像分类结果处理类。                                  |
|     PoseResult     | Yolov8 人体关键点识别结果处理类。                            |
| SegmentationResult | Yolov8 区域分割结果处理类。                                  |

下方所列出信息已经经过代码运行验证测试，如有其他环境测试成功欢迎大家进行补充：

| 选项     | 值                                                      |
| -------- | ------------------------------------------------------- |
| 支持模型 | Yolov8-det、Yolov8-cls、Yolov8-pose、Yolov8-seg         |
| 模型格式 | OpenVINO™ 工具包中间表示(\*.xml,\*.bin)，ONNX (\*.onnx) |
| 支持设备 | CPU、iGPU、dGPU(未测试)                                 |
| 运行环境 | Window 10 、Window 11;                                  |
| 编译环境 | Visual Studio 11，.NET 6.0                              |



## 工作原理

项目运行时，示例程序会读取用户指定路径模型、测试图片以及类别文件，准备模型推理测试的相关数据；将指定模型和图像加载到OpenVINO™ 推理核心并进行同步推理，然后将获取的推理数据加载到自定义的Yolov8数据处理类中进行结果处理。

项目中使用的OpenVINO™相关组件已经封装到OpenVinoSharp中，无需安装在单独安装OpenVINO™。

## 依赖安装

项目中所有依赖项均可以通过NuGet 包安装：

- **OpenVinoSharp**

可以通过Visual Studio 自带的 NuGet 工具进行安装，或者直接下项目配置中直接加入下面代码，

```PackageReference
<ItemGroup>
	<PackageReference Include="OpenVinoSharp.win" Version="2.1.2" />
</ItemGroup>
```

如果项目是通过**dotnet**编译，可以通过下面语句添加对应的包：

```
dotnet add package OpenVinoSharp.win --version 2.1.2
```

- **OpenCvSharp**

可以通过Visual Studio 自带的 NuGet 工具进行安装，或者直接下项目配置中直接加入下面代码，

```PackageReference
<ItemGroup>
	 <PackageReference Include="OpenCvSharp4.Windows" Version="4.8.0.20230708" />
</ItemGroup>
```

如果项目是通过**dotnet**编译，可以通过下面语句添加对应的包：

```
dotnet add package OpenCvSharp4.Windows --version 4.8.0.20230708
```

## 构建

将源码中所有文件添加到当前项目中，并按照上面要求添加相关的NuGet包。

<div align=center><span><img src="https://s2.loli.net/2023/07/23/wdRoNZc5Qb8eAuE.png" height=300/></span></div>

项目可以通过Visual Studio构建和编译，只需要通过右击项目->生成即可。

如果项目通过**dotnet**编译，依次运行以下命令：

```
dotnet add package OpenVinoSharp.win --version 2.1.2 # 添加OpenVinoSharp包
dotnet add package OpenCvSharp4.Windows --version 4.8.0.20230708 # 添加OpenCvSharp4包
dotnet build  # 编译项目
```

项目编译后，会在``\bin\Debug\net6.0``目录下生成可执行文件。

## 运行

```
yolov8_simple_demo <prediction_type> <path_to_model> <image_to_path> <path_to_lable> <device_name>
```

如果需要运行示例，需要同时指定模型预测类型、模型路径、图片文件路径以及标签文件路径。

**模型文件可以通过以下方式获取：**

#### 模型获取

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



#### 结果展示

程序运行会输出模型推理各个环节中的推理时间以及处理后的推理结果：

```
> yolov8_simple_demo det yolov8s.xml demo_2.jpg COCO_lable.txt
-------------------------------------------------------------------
Yolov8 simple demo!
----Yolov8 detection model deploy OpnenVinoSharp-----

[ INFO ] Loading model file: E:\Git_space\OpenVinoSharp\model\yolov8s.xml.
[ INFO ] Loading and building model time: 311.7975 ms
[ INFO ] Reading image file: E:\Git_space\OpenVinoSharp\dataset\image\demo_2.jpg.
[ INFO ] Reading and loading image time: 51.4401 ms
[ INFO ] Infering model time: 91.7445 ms
[ INFO ] Result processing time: 18.2718 ms
[ INFO ] Clear model memory time: 45.4845 ms
```

<div align=center><span><img src="https://s2.loli.net/2023/07/23/YhXH543WkLEJAPS.png" height=300/></span></div>

