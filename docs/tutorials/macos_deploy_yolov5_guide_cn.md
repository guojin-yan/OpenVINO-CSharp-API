<div><center><b>
    <font color="34,63,93" size="7"> 
        在 MacOS 上使用 OpenVINO™ C# API 部署 Yolov5
    </font>
</b></center></div>


> <div><b>
> <font color=red size="5">前言</font>
> </b></div>
>
> YOLOv5 是革命性的 "单阶段"对象检测模型的第五次迭代，旨在实时提供高速、高精度的结果，是世界上最受欢迎的视觉人工智能模型，代表了Ultralytics对未来视觉人工智能方法的开源研究，融合了数千小时研发中积累的经验教训和最佳实践。同时官方发布的模型已经支持 OpenVINO™ 部署工具加速模型推理，因此在该项目中，我们将结合之前开发的 OpenVINO™ C# API 部署 YOLOv5 DET 模型实现物体对象检测。
>
> 项目链接为：
>
> ```
> https://github.com/guojin-yan/OpenVINO-CSharp-API
> ```
>
> 项目源码链接为：
>
> ```
> https://github.com/guojin-yan/OpenVINO-CSharp-API-Samples/tree/master/model_samples/yolov5/yolov5_det_opencvsharp
> https://github.com/guojin-yan/OpenVINO-CSharp-API-Samples/tree/master/model_samples/yolov5/yolov5_det_emgucv
> ```

## 1. 前言

### 1.1 OpenVINO™ C# API

英特尔发行版 OpenVINO™ 工具套件基于 oneAPI 而开发，可以加快高性能计算机视觉和深度学习视觉应用开发速度工具套件，适用于从边缘到云的各种英特尔平台上，帮助用户更快地将更准确的真实世界结果部署到生产系统中。通过简化的开发工作流程，OpenVINO™ 可赋能开发者在现实世界中部署高性能应用程序和算法。

<div align=center><img src="https://s2.loli.net/2024/01/18/vc5VeJ2BQknhitp.png" width=800></div>



OpenVINO™ 2023.2 于 2023 年 11 月 16 日发布，该工具包带来了挖掘生成人工智能全部潜力的新功能。更多的生成式 AI 覆盖和框架集成，以最大限度地减少代码更改，并且扩展了对直接 PyTorch 模型转换的模型支持。支持更多新的模型，包括 LLaVA、chatGLM、Bark 和 LCM 等著名模型。支持更广泛的大型语言模型（LLM）和更多模型压缩技术，支持运行时推理支持以下 Int4 模型压缩格式，通过神经网络压缩框架（NNCF） 进行本机 Int4 压缩等一系列新的功能。

OpenVINO™ C# API 是一个 OpenVINO™ 的 .Net wrapper，应用最新的 OpenVINO™ 库开发，通过 OpenVINO™ C API 实现 .Net 对 OpenVINO™ Runtime 调用，使用习惯与 OpenVINO™ C++ API 一致。OpenVINO™ C# API 由于是基于 OpenVINO™ 开发，所支持的平台与 OpenVINO™ 完全一致，具体信息可以参考 OpenVINO™。通过使用 OpenVINO™ C# API，可以在 .NET、.NET Framework等框架下使用 C# 语言实现深度学习模型在指定平台推理加速。


### 1.2 YOLOv5

​    YOLOv5 是革命性的 "单阶段"对象检测模型的第五次迭代，旨在实时提供高速、高精度的结果，是世界上最受欢迎的视觉人工智能模型，代表了Ultralytics对未来视觉人工智能方法的开源研究，融合了数千小时研发中积累的经验教训和最佳实践。

<div align=center><img src="https://s2.loli.net/2024/01/21/AlOgk92WGQBXMJ6.png" width=800></div>


## 2. 模型下载与转换

### 2.1 环境安装

首先创建Yolov5模型下载与转换环境，此处为了更好的管理环境，使用``Anaconda``创建一个虚拟环境用于安装Yolov5模型下载与转换环境，首先使用``conda``创建一个虚拟环境，在命令行中依次输入以下指令：

```shell
conda create -n yolo python=3.10
conda activate yolo
```

接下来安装Yolov5模型下载与转换环境，基础的Yolov5模型下载需要通过克隆官方源码实现，在命令行中依次输入以下指令实现环境的安装与配置即可：

```shell
git clone https://github.com/ultralytics/yolov5
cd yolov5
pip install -r requirements.txt
pip install --upgrade openvino-nightly
```

### 2.2 Yolov5 模型下载

Yolov5 官方提供了模型导出与转换的方式，用户只需要调用该接口便可以，在命令行中输入以下指令便可以直接导出Yolov5模型：

```shell
cd yolov5
python export.py --weights yolov5s.pt --include onnx
```

结果输出如下图所示：

<div align=center><img src="https://s2.loli.net/2024/02/01/HJmswpdlhBKiE8P.png" width=600></div>

使用Netron工具打开模型文件，查看模型结构，如下图所示：

<div align=center><img src="https://s2.loli.net/2024/02/01/ERklz2bQymIAr6Y.png" width=600></div>

官方预训练模型是在COCO数据集上训练的，因此导出的模型可以识别80种物体。模型输入节点为``images``，输入为归一化后的图像数据，其输入大小为640×640；模型的输出节点为``output0``，输出大小为25200×85，其中25200(640÷8=80，640÷16=40，640÷32=20，3×80×80+3×40×40+3×20×20=25200)表示识别结果个数，85表示[cx, cy, w, h, confidence, score0, ···，score79]，分别为识别框信息、识别结果中最大置信度以及80中类别结果的分数。

### 2.3 转换IR模型

接下来直接使用 OpenVINO™ 工具直接进行模型转换，在CMD中输入以下指令即可：

```shell
ovc yolov5s.onnx
```

<div align=center><img src="https://s2.loli.net/2024/02/01/Cr5UKHaQjkqwcz8.png" width=600></div>

## 3. Yolov5 DET 项目配置(OpenCvSharp版)

### 3.1 项目创建

如果开发者第一次在MacOS系统上使用C#编程语言，可以参考《在MacOS系统上配置OpenVINO™ C# API》文章进行配置。

首先使用dotnet创建一个测试项目，在终端中输入一下指令：

```shell
dotnet new console --framework net6.0 --use-program-main -o yolov5-det 
```

<div align=center><img src="https://s2.loli.net/2024/02/01/NvJaTo5XwG6YI7g.png" width=600></div>

### 3.2 添加项目依赖

MacOS系统目前主要分为两类，一类是使用intel处理器的X64位的系统，一类是使用M系列芯片的arm64位系统，目前OpenVINO官方针对这两种系统都提供了编译后的系统，所以目前OpenVINO.CSharp.API针对这两种系统都提供了支持。

此处以M系列处理器的MacOS平台为例安装项目依赖，首先是安装OpenVINO™ C# API项目依赖，在命令行中输入以下指令即可：

```shell
dotnet add package OpenVINO.CSharp.API
dotnet add package OpenVINO.runtime.macos-arm64
dotnet add package OpenVINO.CSharp.API.Extensions
dotnet add package OpenVINO.CSharp.API.Extensions.OpenCvSharp
```

关于在MacOS上搭建 OpenVINO™ C# API 开发环境请参考以下文章： [在MacOS上搭建OpenVINO™C#开发环境](..\inatall\Install_OpenVINO_CSharp_MacOS_cn.md) 

接下来安装使用到的图像处理库 OpenCvSharp，在命令行中输入以下指令即可：

```shell
dotnet add package OpenCvSharp4
dotnet add package OpenCvSharp4.Extensions
dotnet add package OpenCvSharp4.runtime.osx_arm64 --prerelease
```

关于在MacOS上搭建 OpenCvSharp 开发环境请参考以下文章： [【OpenCV】在MacOS上使用OpenCvSharp](https://mp.weixin.qq.com/s/8njRodtg7lRMggBfpZDHgw) 

添加完成项目依赖后，项目的配置文件如下所示：

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
    <RootNamespace>yolov5_det</RootNamespace>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="OpenCvSharp4" Version="4.9.0.20240103" />
    <PackageReference Include="OpenCvSharp4.Extensions" Version="4.9.0.20240103" />
    <PackageReference Include="OpenCvSharp4.runtime.osx_arm64" Version="4.8.1-rc" />
    <PackageReference Include="OpenVINO.CSharp.API" Version="2023.2.0.4" />
    <PackageReference Include="OpenVINO.CSharp.API.Extensions" Version="1.0.1" />
    <PackageReference Include="OpenVINO.CSharp.API.Extensions.OpenCvSharp" Version="1.0.4" />
    <PackageReference Include="OpenVINO.runtime.macos-arm64" Version="2023.3.0.1" />
  </ItemGroup>

</Project>
```

### 3.3 定义预测方法

#### (1) 使用常规方式部署模型

Yolov5 属于比较经典单阶段目标检测模型，其模型输入为640*640的归一化处理后的图像数据，输出为未进行NMS的推理结果，因此在获取推理结果后，需要进行NMS，其实现代码如下所示：

```csharp
static void yolov5_det(string model_path, string image_path, string device)
{
    // -------- Step 1. Initialize OpenVINO Runtime Core --------
    Core core = new Core();
    // -------- Step 2. Read inference model --------
    Model model = core.read_model(model_path);
    OvExtensions.printf_model_info(model);
    // -------- Step 3. Loading a model to the device --------
    start = DateTime.Now;
    CompiledModel compiled_model = core.compile_model(model, device);
    // -------- Step 4. Create an infer request --------
    InferRequest infer_request = compiled_model.create_infer_request();
    // -------- Step 5. Process input images --------
    Mat image = new Mat(image_path); // Read image by opencvsharp
    int max_image_length = image.Cols > image.Rows ? image.Cols : image.Rows;
    Mat max_image = Mat.Zeros(new OpenCvSharp.Size(max_image_length, max_image_length), MatType.CV_8UC3);
    Rect roi = new Rect(0, 0, image.Cols, image.Rows);
    image.CopyTo(new Mat(max_image, roi));
    float factor = (float)(max_image_length / 640.0);
    // -------- Step 6. Set up input data --------
    Tensor input_tensor = infer_request.get_input_tensor();
    Shape input_shape = input_tensor.get_shape();
    Mat input_mat = CvDnn.BlobFromImage(max_image, 1.0 / 255.0, new OpenCvSharp.Size(input_shape[2], input_shape[3]), 0, true, false);
    float[] input_data = new float[input_shape[1] * input_shape[2] * input_shape[3]];
    Marshal.Copy(input_mat.Ptr(0), input_data, 0, input_data.Length);
    input_tensor.set_data<float>(input_data);
    // -------- Step 7. Do inference synchronously --------
    infer_request.infer();
    // -------- Step 8. Get infer result data --------
    Tensor output_tensor = infer_request.get_output_tensor();
    int output_length = (int)output_tensor.get_size();
    float[] output_data = output_tensor.get_data<float>(output_length);

    // -------- Step 9. Process reault  --------
    Mat result_data = new Mat(25200, 85, MatType.CV_32F, output_data);
    // Storage results list
    List<Rect> position_boxes = new List<Rect>();
    List<int> class_ids = new List<int>();
    List<float> confidences = new List<float>();
    // Preprocessing output results
    for (int i = 0; i < result_data.Rows; i++)
    {
        float confidence = result_data.At<float>(i, 4);
        if (confidence < 0.5)
        {
            continue;
        }
        Mat classes_scores = new Mat(result_data, new Rect(5, i, 80, 1));
        OpenCvSharp.Point max_classId_point, min_classId_point;
        double max_score, min_score;
        // Obtain the maximum value and its position in a set of data
        Cv2.MinMaxLoc(classes_scores, out min_score, out max_score,
            out min_classId_point, out max_classId_point);
        // Confidence level between 0 ~ 1
        // Obtain identification box information
        if (max_score > 0.25)
        {
            float cx = result_data.At<float>(i, 0);
            float cy = result_data.At<float>(i, 1);
            float ow = result_data.At<float>(i, 2);
            float oh = result_data.At<float>(i, 3);
            int x = (int)((cx - 0.5 * ow) * factor);
            int y = (int)((cy - 0.5 * oh) * factor);
            int width = (int)(ow * factor);
            int height = (int)(oh * factor);
            Rect box = new Rect();
            box.X = x;
            box.Y = y;
            box.Width = width;
            box.Height = height;

            position_boxes.Add(box);
            class_ids.Add(max_classId_point.X);
            confidences.Add((float)confidence);
        }
    }
    // NMS non maximum suppression
    int[] indexes = new int[position_boxes.Count];
    CvDnn.NMSBoxes(position_boxes, confidences, 0.5f, 0.5f, out indexes);
    for (int i = 0; i < indexes.Length; i++)
    {
        int index = indexes[i];
        Cv2.Rectangle(image, position_boxes[index], new Scalar(0, 0, 255), 2, LineTypes.Link8);
        Cv2.Rectangle(image, new OpenCvSharp.Point(position_boxes[index].TopLeft.X, position_boxes[index].TopLeft.Y + 30),
            new OpenCvSharp.Point(position_boxes[index].BottomRight.X, position_boxes[index].TopLeft.Y), new Scalar(0, 255, 255), -1);
        Cv2.PutText(image, class_ids[index] + "-" + confidences[index].ToString("0.00"),
            new OpenCvSharp.Point(position_boxes[index].X, position_boxes[index].Y + 25),
            HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 0, 0), 2);
    }
    string output_path = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(image_path)),
        Path.GetFileNameWithoutExtension(image_path) + "_result.jpg");
    Cv2.ImWrite(output_path, image);
    Slog.INFO("The result save to " + output_path);
    Cv2.ImShow("Result", image);
    Cv2.WaitKey(0);
}
```

#### (2) 使用模型结构处理处理数据

目前 OpenVINO™ 已经支持在模型结构中增加数据的前后处理流程，并且在 OpenVINO™ C# API 中也已经实现了该功能接口，所以在此处演示了如何将模型输入数据处理流程封装到模型中，通过 OpenVINO™ 进行数据处理的加速处理，如下面代码所示:

```csharp
static void yolov5_det_with_process(string model_path, string image_path, string device)
{
    ······
    // -------- Step 2. Read inference model --------
    start = DateTime.Now;
    Model model = core.read_model(model_path);
    OvExtensions.printf_model_info(model);
    PrePostProcessor processor = new PrePostProcessor(model);
    Tensor input_tensor_pro = new Tensor(new OvType(ElementType.U8), new Shape(1, 640, 640, 3));
    InputInfo input_info = processor.input(0);
    InputTensorInfo input_tensor_info = input_info.tensor();
    input_tensor_info.set_from(input_tensor_pro).set_layout(new Layout("NHWC")).set_color_format(ColorFormat.BGR);
    PreProcessSteps process_steps = input_info.preprocess();
    process_steps.convert_color(ColorFormat.RGB).resize(ResizeAlgorithm.RESIZE_LINEAR)
        .convert_element_type(new OvType(ElementType.F32)).scale(255.0f).convert_layout(new Layout("NCHW"));
    Model new_model = processor.build();
    // -------- Step 3. Loading a model to the device --------
    CompiledModel compiled_model = core.compile_model(new_model, device);
    // -------- Step 4. Create an infer request --------
    InferRequest infer_request = compiled_model.create_infer_request();
    // -------- Step 5. Process input images --------
    Mat image = new Mat(image_path); // Read image by opencvsharp
    int max_image_length = image.Cols > image.Rows ? image.Cols : image.Rows;
    Mat max_image = Mat.Zeros(new OpenCvSharp.Size(max_image_length, max_image_length), MatType.CV_8UC3);
    Rect roi = new Rect(0, 0, image.Cols, image.Rows);
    image.CopyTo(new Mat(max_image, roi));
    Cv2.Resize(max_image, max_image, new OpenCvSharp.Size(640, 640));
    float factor = (float)(max_image_length / 640.0);
    // -------- Step 6. Set up input data --------
    Tensor input_tensor = infer_request.get_input_tensor();
    Shape input_shape = input_tensor.get_shape();
    byte[] input_data = new byte[input_shape[1] * input_shape[2] * input_shape[3]];
    Marshal.Copy(max_image.Ptr(0), input_data, 0, input_data.Length);
    IntPtr destination = input_tensor.data();
    Marshal.Copy(input_data, 0, destination, input_data.Length);
    // -------- Step 7. Do inference synchronously --------
    ······
}
```

由于目前还没有完全实现所有的 OpenVINO™ 的预处理接口，因此只能实现部分预处理过程封装到模型中，此处主要是做了以下处理：

- 数据类型转换：byte->float
- 数据维度转换：NHWC->NCHW
- 图像色彩空间转换：BGR->RGB
- 数据归一化处理：[0,1]->[0,255]

因此将一些数据处理流程封装到模型中后，在进行模型推理时，只需要将读取到的图片数据Resize为640*640后，就可以直接将数据加载到模型即可。


#### (3) 使用 OpenVINO™ C# API 封装的接口

 YOLOv5 是当前工业领域十分流行的目标检测模型，因此在封装 OpenVINO™ C# API 时，提供了快速部署 Yolov5 模型的接口，实现代码如下所示：

```csharp
static void yolov5_det_using_extensions(string model_path, string image_path, string device)
{
    Yolov5DetConfig config = new Yolov5DetConfig();
    config.set_model(model_path);
    Yolov5Det yolov8 = new Yolov5Det(config);
    Mat image = Cv2.ImRead(image_path);
    DetResult result = yolov8.predict(image);
    Mat result_im = Visualize.draw_det_result(result, image);
    Cv2.ImShow("Result", result_im);
    Cv2.WaitKey(0);
}
```

### 3.4 预测方法调用

定义好上述方法后，便可以直接在主函数中调用该方法，只需要在主函数中增加以下代码即可：

```csharp
yolov5_det("yolov5s.xml", "test_image.png", "AUTO");
yolov5_det_with_process("yolov5s.xml", "test_image.png", "AUTO");
yolov5_det_using_extensions("yolov5s.xml", "test_image.png", "AUTO");
```

如果开发者自己没有进行模型下载与转换，又同时想快速体验该项目，我此处提供了在线的转换后的模型以及带预测图片，开发者可以直接在主函数中增加以下代码，便可以直接自动下载模型以及推理数据，并调用推理方法，实现程序直接运行。

```csharp
static void Main(string[] args)
{
    string model_path = "";
    string image_path = "";
    string device = "AUTO";
    if (args.Length == 0)
    {
        if (!Directory.Exists("./model"))
        {
            Directory.CreateDirectory("./model");
        }
        if (!File.Exists("./model/yolov5s.bin") && !File.Exists("./model/yolov5s.bin"))
        {
            if (!File.Exists("./model/yolov5s.tar"))
            {
                _ = Download.download_file_async("https://github.com/guojin-yan/OpenVINO-CSharp-API-Samples/releases/download/Model/yolov5s.tar",
                    "./model/yolov5s.tar").Result;
            }
            Download.unzip("./model/yolov585s.tar", "./model/");
        }

        if (!File.Exists("./model/test_image.jpg"))
        {
            _ = Download.download_file_async("https://github.com/guojin-yan/OpenVINO-CSharp-API-Samples/releases/download/Image/test_det_02.jpg",
                "./model/test_image.jpg").Result;
        }
        model_path = "./model/yolov5s.xml";
        image_path = "./model/test_image.jpg";
    }
    else if (args.Length >= 2)
    {
        model_path = args[0];
        image_path = args[1];
        device = args[2];
    }
    else
    {
        Console.WriteLine("Please enter the correct command parameters, for example:");
        Console.WriteLine("> 1. dotnet run");
        Console.WriteLine("> 2. dotnet run <model path> <image path> <device name>");
    }
    // -------- Get OpenVINO runtime version --------

    OpenVinoSharp.Version version = Ov.get_openvino_version();

    Slog.INFO("---- OpenVINO INFO----");
    Slog.INFO("Description : " + version.description);
    Slog.INFO("Build number: " + version.buildNumber);

    Slog.INFO("Predict model files: " + model_path);
    Slog.INFO("Predict image  files: " + image_path);
    Slog.INFO("Inference device: " + device);
    Slog.INFO("Start yolov8 model inference.");

    yolov5_det(model_path, image_path, device);
    //yolov5_det_with_process(model_path, image_path, device);
    //yolov5_det_using_extensions(model_path, image_path, device);
}
```


> 为了减少文章篇幅，所以此处只提供了有差异的代码，如果想获取完整代码，请访问GitHub代码仓库，获取项目源码，链接为：
>
> ```
> https://github.com/guojin-yan/OpenVINO-CSharp-API-Samples/tree/master/model_samples/yolov5/yolov5_det_opencvsharp
> ```

## 4. Yolov5 DET 项目配置(Emgu.CV 版)

同样地，为了满足Emgu.CV开发者的需求，此处同样地提供了Emgu.CV版本的Yolov5的模型部署代码以及使用流程，此处为了简化文章内容，对于和上文重复的步骤不在进行展开讲述。

### 4.1 添加项目依赖

首先是安装OpenVINO™ C# API项目依赖，在命令行中输入以下指令即可：

```shell
dotnet add package OpenVINO.CSharp.API
dotnet add package OpenVINO.runtime.macos-arm64
dotnet add package OpenVINO.CSharp.API.Extensions
dotnet add package OpenVINO.CSharp.API.Extensions.EmguCV
```


接下来安装使用到的图像处理库 Emgu.CV，在命令行中输入以下指令即可：

```shell
dotnet add package Emgu.CV
dotnet add package Emgu.CV.runtime.mini.macos
```

关于在MacOS上搭建 OpenCvSharp 开发环境请参考以下文章： [【OpenCV】在MacOS上使用Emgu.CV](https://mp.weixin.qq.com/s/0qO-bAn-96qinF1HYcbJ_g) 

添加完成项目依赖后，项目的配置文件如下所示：

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
    <RootNamespace>yolov5_det</RootNamespace>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Emgu.CV" Version="4.8.1.5350" />
    <PackageReference Include="Emgu.CV.runtime.mini.macos" Version="4.8.1.5350" />
    <PackageReference Include="OpenVINO.CSharp.API" Version="2023.2.0.4" />
    <PackageReference Include="OpenVINO.CSharp.API.Extensions" Version="1.0.1" />
    <PackageReference Include="OpenVINO.CSharp.API.Extensions.EmguCV" Version="1.0.4.1" />
    <PackageReference Include="OpenVINO.runtime.macos-arm64" Version="2023.3.0.1" />
  </ItemGroup>

</Project>
```

### 4.2 定义预测方法

模型部署流程与上一节中使用OpenCvSharp的基本一致，主要是替换了图像处理的工具，同时提供了如上一节中所展示的三种部署方式。此处为了减少文章篇幅，此处不在展示详细的部署代码，如果想获取相关代码，请访问项目GitHub，下载所有的测试代码，项目链接为：

```
https://github.com/guojin-yan/OpenVINO-CSharp-API-Samples/tree/master/model_samples/yolov5/yolov5_det_emgucv
```

## 5. 项目运行与演示

### 5.1 项目编译

接下来输入项目编译指令进行项目编译，输入以下指令即可：

```
dotnet build
```

程序编译后输出为：

<div align=center><img src="https://s2.loli.net/2024/02/01/gejy9DoWSRZ7YdO.png" width=600></div>

### 5.2 项目文件运行

接下来运行编译后的程序文件，在CMD中输入以下指令，运行编译后的项目文件：

```
dotnet run --no-build
```

运行后项目输出为：

<div align=center><img src="https://s2.loli.net/2024/02/01/lGRr1diWtHn6pYk.png" width=600></div>

<div align=center><img src="https://s2.loli.net/2024/02/01/wPvx5W1HQdNynrR.jpg" width=600></div>




## 6. 总结

在该项目中，我们结合之前开发的 OpenVINO C# API 项目部署YOLOv5模型，成功实现了对象目标检测，并且根据不同开发者的使用习惯，同时提供了OpenCvSharp以及Emgu.CV两种版本，供各位开发者使用。最后如果各位开发者在使用中有任何问题，欢迎大家与我联系。

<div align=center><img src="https://s2.loli.net/2024/01/29/VIPU1MSwjEh2QAY.png" width=800></div>
