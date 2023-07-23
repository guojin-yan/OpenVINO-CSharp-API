[简体中文](README.md)| English

# Yolov8 Model Deployment Example

This example will demonstrate how the Yolov8 series of models can use OpenVinoSharp to predict images, supporting all models officially downloaded by Yolov8. The example corresponds to the code：[OpenVinoSharp/demo/yolov8_simple_demo](https://github.com/guojin-yan/OpenVinoSharp/tree/openvinosharp2.1/demo/yolov8_simple_demo)

The model will use the following C# API interfaces:

| Function                                                  | API                                                          |
| --------------------------------------------------------- | ------------------------------------------------------------ |
| Initialize the inference core and basic inference process | public Core(string model, string device, string cache_dir = "") |
| Load data to be inferred                                  | public void load_input_data(string node_name, byte[] image_data, ulong image_size, int type) |
| Model infer                                               | public void infer()                                          |
| Read inference results                                    | public T[] read_infer_result<T>(string node_name, int data_size) |
| Clear inference memory                                    | public void delet()                                          |

In order to better handle the reasoning results of Yolov8, a result processing class has been customized here. Under namespace Yolov8, for the convenience of everyone's understanding, a brief explanation of each class is provided here:

|       Class        | Explain                                                      |
| :----------------: | ------------------------------------------------------------ |
|       Result       | Inference result class, mainly storing inference results; Support object detection results (category, confidence, prediction box), image classification results (category, confidence), human key point recognition results (category, confidence, prediction box, human key point results), region segmentation results (category, confidence, prediction box, segmentation area) |
|     ResultBase     | The inference result processing base class defines some common attributes and methods for result processing. |
|  DetectionResult   | Yolov8 Object detection and recognition result processing class. |
|     ClasResult     | Yolov8 image classification result processing class.         |
|     PoseResult     | Yolov8 human keypoint recognition result processing class.   |
| SegmentationResult | Yolov8 region segmentation result processing class.          |

The information listed below has been verified and tested by code running. If there are other successful testing environments, please feel free to supplement:

| **Options**           | **Values**                                                   |
| --------------------- | ------------------------------------------------------------ |
| Validated Models      | Yolov8-det、Yolov8-cls、Yolov8-pose、Yolov8-seg              |
| Model Format          | OpenVINO™ toolkit Intermediate Representation (*.xml + *.bin), ONNX (*.onnx) |
| Supported devices     | CPU、iGPU、dGPU(Not tested)                                  |
| Operating environment | Window 10 、Window 11;                                       |
| Building environment  | Visual Studio 11，.NET 6.0                                   |



## How It Works

When the project runs, the sample program will read the user specified path model, test images, and category files to prepare relevant data for model inference testing; Load the specified model and image into OpenVINO ™  Reasoning the core and performing synchronous reasoning, then loading the obtained reasoning data into a custom Yolov8 data processing class for result processing.
OpenVINO used in the project ™ The relevant components have been encapsulated in OpenVinoSharp, and there is no need to install OpenVino separately ™。

## Dependent installation

All dependencies in the project can be installed through the **NuGet** package:

- **OpenVinoSharp**

You can install it through the NuGet tool that comes with Visual Studio, or directly add the following code to the project configuration,

```PackageReference
<ItemGroup>
	<PackageReference Include="OpenVinoSharp.win" Version="2.1.2" />
</ItemGroup>
```

If the project is compiled through **dotnet**, the corresponding package can be added using the following statement:

```
dotnet add package OpenVinoSharp.win --version 2.1.2
```

- **OpenCvSharp**

You can install it through the NuGet tool that comes with Visual Studio, or directly add the following code to the project configuration,

```PackageReference
<ItemGroup>
	 <PackageReference Include="OpenCvSharp4.Windows" Version="4.8.0.20230708" />
</ItemGroup>
```

If the project is compiled through **dotnet**, the corresponding package can be added using the following statement:

```
dotnet add package OpenCvSharp4.Windows --version 4.8.0.20230708
```

## Building

Add all files in the source code to the current project and add the relevant NuGet package as required above.

<div align=center><span><img src="https://s2.loli.net/2023/07/23/wdRoNZc5Qb8eAuE.png" height=300/></span></div>

Projects can be built and compiled through Visual Studio, simply **right-click on the project **->**Generate**.
If the project is compiled through **dotnet**, run the following commands in sequence:

```
dotnet add package OpenVinoSharp.win --version 2.1.2 # add OpenVinoSharp
dotnet add package OpenCvSharp4.Windows --version 4.8.0.20230708 # add OpenCvSharp4
dotnet build  # building project
```

After the project is compiled, an executable file will be generated in the ``bin\Debug\net6.0'``directory.

## Runing

```
yolov8_simple_demo <prediction_type> <path_to_model> <image_to_path> <path_to_lable> <device_name>
```

If you need to run the example, you need to specify both the model prediction type, model path, image file path, and label file path.
**The model file can be obtained through the following methods:**

#### Model acquisition

All the models used in the project were downloaded from the **ultra tics** platform. The following are download examples:

1. Installing Ultralytics

   ```
   pip install ultralytics
   ```

2. Export Yolov8 model

   ```
   yolo export model=yolov8s.pt format=onnx  #yolov8-det
   yolo export model=yolov8s-cls.pt format=onnx  #yolov8-cls
   yolo export model=yolov8s-pose.pt format=onnx  #yolov8-pose
   yolo export model=yolov8s-seg.pt format=onnx  #yolov8-seg
   ```

3. Convert to IR format

   IR format here via OpenVINO ™ The model optimization tool implementation requires the installation of OpenVINO ™  Python version, specific implementation can refer to [Model Preparation OpenVINO ™ Documentation](https://docs.openvino.ai/2023.0/openvino_docs_model_processing_introduction.html)  , can also be achieved through the command line:

   ```
   mo -input_model yolov8s.onnx
   ```



#### Results Display

The program will output the inference time in each stage of model inference and the processed inference results:

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

