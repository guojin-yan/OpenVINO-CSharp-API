# Yolov8 模型部署示例

该示例将演示 Yolov8 系列模型如何使用 OpenVinoSharp 对图像进行预测，支持 Yolov8 官方下载的所有模型，示例对应代码：

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

## 项目安装

#### NuGet 包安装

- **OpenVinoSharp**

可以通过Visual Studio 自带的 NuGet 工具进行安装，或者直接下项目配置中直接加入下面代码，

```PackageReference
<ItemGroup>
	<PackageReference Include="OpenVinoSharp.win" Version="2.1.2" />
</ItemGroup>
```

- **OpenCvSharp**

可以通过Visual Studio 自带的 NuGet 工具进行安装，或者直接下项目配置中直接加入下面代码，

```PackageReference
<ItemGroup>
	 <PackageReference Include="OpenCvSharp4.Windows" Version="4.8.0.20230708" />
</ItemGroup>
```



## 模型获取
