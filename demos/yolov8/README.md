![OpenVinoSharp](https://socialify.git.ci/guojin-yan/OpenVinoSharp/image?description=1&descriptionEditable=💞%20OpenVINO%20wrapper%20for%20.NET💞%20&forks=1&issues=1&logo=https%3A%2F%2Fs2.loli.net%2F2023%2F01%2F26%2FylE1K5JPogMqGSW.png&name=1&owner=1&pattern=Circuit%20Board&pulls=1&stargazers=1&theme=Light)

[简体中文](README_cn.md) | English

# OpenVinoSharp Deployment Yolov8 Model Example

&emsp;    OpenVinoSharp version 3.0 has undergone significant updates compared to version 2.0, changing from refactoring the C++API to directly reading OpenVino ™  The official C API makes the application more flexible and supports a richer range of functions. OpenVinoSharp 3.0 API interface with multiple references to OpenVino ™  C++API implementation, therefore it is closer to the C++API when used, which will be more friendly to friends who are familiar with using the C++API.
&emsp; This example demonstrates how to deploy the Yolov8 full series model using the OpenVinoSharp 3.0 API.
&emsp; This example supports the full range of Yolov8 models, as well as official pre training models and personal training models.
&emsp; The following C # APIs will be mainly used in the example:

|         Feature          | API                                                          | Description                                                  |
| :----------------------: | ------------------------------------------------------------ | ------------------------------------------------------------ |
| OpenVINO Runtime Version | Ov.get_openvino_version()                                    | Get Openvino API version.                                    |
|     Basic Infer Flow     | Core.read_model(), core.compiled_model(), CompiledModel.create_infer_request(), InferRequest.get_input_tensor(), InferRequest.get_output_tensor(), InferRequest.get_tensor() | Common API to do inference: read and compile a model, create an infer request, configure input and output tensors. |
|    Synchronous Infer     | InferRequest.infer()                                         | Do synchronous inference.                                    |
|     Model Operations     | Model.get_friendly_name(), Model.get_const_input(), Model.get_const_output() | Get inputs and outputs of a model.                           |
|     Node Operations      | Node.get_name(), Node.get_type(), Node.get_shape(            | Get node message.                                            |
|    Tensor Operations     | Tensor.get_shape(), Tensor.set_data<T>(), Tensor.get_size(), Tensor.get_data<T>() | Get a tensor shape, size, data and  set data.                |
|      Yolov8 Process      | ResultProcess.process_det_result(), ResultProcess.process_seg_result(), ResultProcess.process_pose_result, ResultProcess.process_cls_result(), ResultProcess.read_class_names(), ResultProcess.draw_det_result(), ResultProcess.draw_pose_result(), ResultProcess.draw_seg_result(), ResultProcess.print_result() | Process and draw yolov8 result.                              |

&emsp;    The information listed below has been verified and tested by code running. If there are other successful testing environments, please feel free to supplement:

| **Options**           | **Values**                                                   |
| --------------------- | ------------------------------------------------------------ |
| Validated Models      | Yolov8-det、Yolov8-cls、Yolov8-pose、Yolov8-seg              |
| Model Format          | OpenVINO™ toolkit Intermediate Representation (*.xml + *.bin), ONNX (*.onnx) |
| Supported devices     | CPU、iGPU、dGPU(Not tested)                                  |
| Operating environment | Window 10 、Window 11;                                       |
| Building environment  | Visual Studio 11，.NET 6.0                                   |



## How It Works

&emsp;    When the project runs, the sample program will read the user specified path model, test images, and category files to prepare relevant data for model inference testing; Load the specified model and image into OpenVINO ™  Reasoning the core and performing synchronous reasoning, then loading the obtained reasoning data into a custom Yolov8 data processing class for result processing.
OpenVINO used in the project ™ The relevant components have been encapsulated in OpenVinoSharp, and there is no need to install OpenVino separately ™。

## 依赖安装

&emsp;    All dependencies in the project can be installed through the **NuGet** package:

- **OpenVinoSharp**

&emsp;    You can install it through the NuGet tool that comes with Visual Studio.

<div align=center><span><img src="https://s2.loli.net/2023/07/31/UFAgRbBuhcsqOEv.png" height=300/></span></div>

&emsp;    If the project is compiled through **dotnet**, the corresponding package can be added using the following statement:

```
dotnet add package OpenVinoSharp.win
```

## Building

&emsp;    Add all files in the source code to the current project and add the relevant NuGet package as required above.

<div align=center><span><img src="https://s2.loli.net/2023/07/31/Bal8QopgDmbePAJ.png" height=300/></span></div>

&emsp;    Projects can be built and compiled through Visual Studio, simply **right-click on the project **->**Generate**.
&emsp;    If the project is compiled through **dotnet**, run the following commands in sequence:

```
dotnet add package OpenVinoSharp.win # add OpenVinoSharp
dotnet build  # building project
```

&emsp;    After the project is compiled, an executable file will be generated in the ``bin\Debug\net6.0'``directory.

## Run

```
dotnet run <type> <path_to_model> <image_to_path> <device_name> <path_to_lable>
```

&emsp;    If you need to run the example, you need to specify both the model prediction type, model path, and image file path. The prediction type input includes four types: 'det', 'seg', 'pose', and 'cls'; The default inference device is set to 'AUTO'. For 'det' and 'seg' predictions, the <path_to_lable> parameter can be set. If this parameter is set, the results will be plotted on the image. If it is not set, the command sister will print it out.

**The model file can be obtained through the following methods:**

### Model acquisition

&emsp;    All the models used in the project were downloaded from the **ultra tics** platform. The following are download examples:

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

   &emsp;    IR format here via OpenVINO ™ The model optimization tool implementation requires the installation of OpenVINO ™  Python version, specific implementation can refer to [Model Preparation OpenVINO ™ Documentation](https://docs.openvino.ai/2023.0/openvino_docs_model_processing_introduction.html)  , can also be achieved through the command line:

   ```
   mo -input_model yolov8s.onnx
   ```



### Results Display

&emsp;    The program will output model inference information and inference results:

#### Yolov8-det inference results

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

#### Yolov8-pose inference results

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

#### Yolov8-seg inference results

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



#### Yolov8-cls inference results

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

