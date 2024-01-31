<div><center><b>
    <font color="34,63,93" size="7"> 
        在 Windows 上使用 OpenVINO™ C# API 部署 Yolov8-obb
    </font>
</b></center></div>





> <div><b>
> <font color=red size="5">&emsp;前言</font>
> </b></div>
> Ultralytics YOLOv8 基于深度学习和计算机视觉领域的尖端技术，在速度和准确性方面具有无与伦比的性能。其流线型设计使其适用于各种应用，并可轻松适应从边缘设备到云 API 等不同硬件平台。YOLOv8 OBB 模型是YOLOv8系列模型最新推出的任意方向的目标检测模型，可以检测任意方向的对象，大大提高了物体检测的精度。同时官方发布的模型已经支持 OpenVINO™ 部署工具加速模型推理，因此在该项目中，我们将结合之前开发的 OpenVINO™ C# API 部署YOLOv8 OBB 模型实现旋转物体对象检测。
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
> https://github.com/guojin-yan/OpenVINO-CSharp-API-Samples/tree/master/model_samples/yolov8/yolov8_obb_opencvsharp
> https://github.com/guojin-yan/OpenVINO-CSharp-API-Samples/tree/master/model_samples/yolov8/yolov8_obb_emgucv
> ```

## 1. 前言

### 1.1 OpenVINO™ C# API

英特尔发行版 OpenVINO™ 工具套件基于 oneAPI 而开发，可以加快高性能计算机视觉和深度学习视觉应用开发速度工具套件，适用于从边缘到云的各种英特尔平台上，帮助用户更快地将更准确的真实世界结果部署到生产系统中。通过简化的开发工作流程，OpenVINO™ 可赋能开发者在现实世界中部署高性能应用程序和算法。

<div align=center><img src="https://s2.loli.net/2024/01/18/vc5VeJ2BQknhitp.png" width=800></div>



OpenVINO™ 2023.2 于 2023 年 11 月 16 日发布，该工具包带来了挖掘生成人工智能全部潜力的新功能。更多的生成式 AI 覆盖和框架集成，以最大限度地减少代码更改，并且扩展了对直接 PyTorch 模型转换的模型支持。支持更多新的模型，包括 LLaVA、chatGLM、Bark 和 LCM 等著名模型。支持更广泛的大型语言模型（LLM）和更多模型压缩技术，支持运行时推理支持以下 Int4 模型压缩格式，通过神经网络压缩框架（NNCF） 进行本机 Int4 压缩等一系列新的功能。

OpenVINO™ C# API 是一个 OpenVINO™ 的 .Net wrapper，应用最新的 OpenVINO™ 库开发，通过 OpenVINO™ C API 实现 .Net 对 OpenVINO™ Runtime 调用，使用习惯与 OpenVINO™ C++ API 一致。OpenVINO™ C# API 由于是基于 OpenVINO™ 开发，所支持的平台与 OpenVINO™ 完全一致，具体信息可以参考 OpenVINO™。通过使用 OpenVINO™ C# API，可以在 .NET、.NET Framework等框架下使用 C# 语言实现深度学习模型在指定平台推理加速。

### 1.2 YOLOv8 OBB 模型

目标检测是计算机视觉中的一项基本任务，目前许多研究都是采用水平边界框来定位图像中的物体。然而，图像中的物体通常是任意方向的。因此，使用水平边界框来检测目标会引起物体检测框通常包含许多背景区域，检测框内存在过多的背景区域，不仅增加了分类任务的难度，而且会导致目标范围表示不准确的问题。其次，水平边界框会导致检测框之间出现重叠，降低检测精度。

<div align=center><img src="https://s2.loli.net/2024/01/29/hAxae1kUTXFv5dl.png" width=800></div>

通过使用带有角度信息的旋转检测框可以有效解决上述问题，它引入了一个额外的角度来更准确地定位图像中的物体。Ultralytics YOLOv8 基于深度学习和计算机视觉领域的尖端技术，在速度和准确性方面具有无与伦比的性能。其流线型设计使其适用于各种应用，并可轻松适应从边缘设备到云 API 等不同硬件平台。YOLOv8 OBB 模型是YOLOv8系列模型最新推出的任意方向的目标检测模型，其模型输出结果是一组旋转的边界框，这些边界框精确地包围了图像中的物体，同时还包括每个边界框的类标签和置信度分数。当你需要识别场景中感兴趣的物体，但又不需要知道物体的具体位置或确切形状时，物体检测是一个不错的选择。

<div align=center><img src="https://s2.loli.net/2024/01/29/tqrTsVJFjp8o9PQ.png" width=600></div>



## 2. YOLOv8 OBB模型下载与转换

### 2.1 安装模型下载与转换环境

此处主要安装 YOLOv8 OBB模型导出环境以及OpenVINO™模型转换环境，使用Anaconda进行环境创建，依次输入以下指令：

```shell
conda create -n yolo python=3.10
conda activate yolo
pip install ultralytics
pip install --upgrade openvino-nightly
```

此处只需要安装以上两个程序包即可。

### 2.2 导出 YOLOv8 OBB模型

接下来以Yolov8s-obb模型到处为例，演示如何快速导出官方提供的预训练模型，首先在创建的虚拟环境中输入以下命令：

```
yolo export model=yolov8s-obb.pt format=onnx 
```

下图展示了模型导出命令输出情况：

<div align=center><img src="https://s2.loli.net/2024/01/29/TlMqCvajdwrf6kY.png" width=800></div>

接下来查看导出的Yolov8s-obb模型的结构情况，通过Netron可以进行查看，如下所示：

<div align=center><img src="https://s2.loli.net/2024/01/29/bDHvxF5m6olsQ4E.png" width=800></div>

模型识别类别：由于官方模型是在[DOTAv1](https://github.com/ultralytics/ultralytics/blob/main/ultralytics/cfg/datasets/DOTAv1.yaml)数据集上预训练的，因此导出的模型可以识别出15种类别，分别是：plane 、ship storage tank、baseball diamond、 tennis court、basketball court、ground track field、harbor、 bridge、large vehicle、 small vehicle、helicopter、roundabout、soccer ball field、 swimming pool。

模型输入：模型输入名称为“image”，输入大小为1×3×1024×1024的归一化后的图像数据，数据类型为float。

模型输出：模型的输出名称为“output0”，输出大小为1×20×21504，输出数据为float。其中“20”表示为[x, y, w, h, sorce0, ···, sorce14, angle]的数组，数组中0~3这四个参数为预测框的矩形位置，数组中4~18这15个参数表示15各类别的分类置信度，数组中19这个参数表示预测框的旋转角度；“21504”表示输入为1024的模型三个检测头的输出大小，总共有21504(1024÷8=128，1024÷16=64，1024÷32=32，128×128+64×64+32×32=21504)种结果。

### 2.3 转换 IR 模型

接下来直接使用OpenVINO™工具直接进行模型转换，在CMD中输入以下指令即可：

```
ovc yolov8s-obb.onnx
```

模型导出输出如下图所示。

<div align=center><img src="https://s2.loli.net/2024/01/29/wWPDQvRoBCSisen.png" width=800></div>

## 3. YOLOv8 OBB项目配置(OpenCvSharp)

此处项目创建与运行演示使用dotnet工具进行，大家也可以通过Visual Studio等IDE工具进行项目创建与运行。首先讲解一下使用OpenCvSharp作为图像处理工具的代码程序。

### 3.1 项目创建

首先使用dotnet工具创建一个控制台应用，在CMD中输入以下指令：

```
dotnet new console --framework net6.0 --use-program-main -o yolov8_obb_opencvsharp
cd yolov8_obb_opencvsharp
```

项目创建后输出为：

<div align=center><img src="https://s2.loli.net/2024/01/29/aqBhSzonF6defjg.png" width=800></div>

### 3.2 添加项目依赖

此处以Windows平台为例安装项目依赖，首先是安装OpenVINO™ C# API项目依赖，在命令行中输入以下指令即可：

```
dotnet add package OpenVINO.CSharp.API
dotnet add package OpenVINO.runtime.win
dotnet add package OpenVINO.CSharp.API.Extensions
dotnet add package OpenVINO.CSharp.API.Extensions.OpenCvSharp 
```

接下来安装使用到的图像处理库OpenCvSharp，在命令行中输入以下指令即可：

```
dotnet add package OpenCvSharp4
dotnet add package OpenCvSharp4.Extensions
dotnet add package OpenCvSharp4.runtime.win
```

添加完成项目依赖后，项目的配置文件如下所示：

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="OpenCvSharp4" Version="4.9.0.20240103" />
    <PackageReference Include="OpenCvSharp4.Extensions" Version="4.9.0.20240103" />
    <PackageReference Include="OpenCvSharp4.runtime.win" Version="4.9.0.20240103" />
    <PackageReference Include="OpenVINO.CSharp.API" Version="2023.2.0.4" />
    <PackageReference Include="OpenVINO.CSharp.API.Extensions" Version="1.0.1" />
    <PackageReference Include="OpenVINO.CSharp.API.Extensions.OpenCvSharp" Version="1.0.4" />
    <PackageReference Include="OpenVINO.runtime.win" Version="2023.3.0.1" />
  </ItemGroup>

</Project>
```



### 3.3 定义预测方法

Yolov8 Obb模型部署流程与方式与Yolov8 Det基本一致，其主要不同点在与其结果的后处理方式，本项目所定义的Yolov8 Obb模型推理代码如下所示：

```csharp
static void yolov8_obb(string model_path, string image_path, string device)
{
    // -------- Step 1. Initialize OpenVINO Runtime Core --------
    Core core = new Core();
    // -------- Step 2. Read inference model --------
    Model model = core.read_model(model_path);
    OvExtensions.printf_model_info(model);
    // -------- Step 3. Loading a model to the device --------
    CompiledModel compiled_model = core.compile_model(model, device);
    // -------- Step 4. Create an infer request --------
    InferRequest infer_request = compiled_model.create_infer_request();
    // -------- Step 5. Process input images --------
    Mat image = new Mat(image_path); // Read image by opencvsharp
    int max_image_length = image.Cols > image.Rows ? image.Cols : image.Rows;
    Mat max_image = Mat.Zeros(new OpenCvSharp.Size(max_image_length, max_image_length), MatType.CV_8UC3);
    Rect roi = new Rect(0, 0, image.Cols, image.Rows);
    image.CopyTo(new Mat(max_image, roi));
    float factor = (float)(max_image_length / 1024.0);
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
    Mat result_data = new Mat(20, 21504, MatType.CV_32F, output_data);
    result_data = result_data.T();
    float[] d = new float[output_length];
    result_data.GetArray<float>(out d);
    // Storage results list
    List<Rect2d> position_boxes = new List<Rect2d>();
    List<int> class_ids = new List<int>();
    List<float> confidences = new List<float>();
    List<float> rotations = new List<float>();
    // Preprocessing output results
    for (int i = 0; i < result_data.Rows; i++)
    {
        Mat classes_scores = new Mat(result_data, new Rect(4, i, 15, 1));
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
            double x = (cx - 0.5 * ow) * factor;
            double y = (cy - 0.5 * oh) * factor;
            double width = ow * factor;
            double height = oh * factor;
            Rect2d box = new Rect2d();
            box.X = x;
            box.Y = y;
            box.Width = width;
            box.Height = height;
            position_boxes.Add(box);
            class_ids.Add(max_classId_point.X);
            confidences.Add((float)max_score);
            rotations.Add(result_data.At<float>(i, 19));
        }
    }
    // NMS non maximum suppression
    int[] indexes = new int[position_boxes.Count];
    CvDnn.NMSBoxes(position_boxes, confidences, 0.25f, 0.7f, out indexes);
    List<RotatedRect> rotated_rects = new List<RotatedRect>();
    for (int i = 0; i < indexes.Length; i++)
    {
        int index = indexes[i];
        float w = (float)position_boxes[index].Width;
        float h = (float)position_boxes[index].Height;
        float x = (float)position_boxes[index].X + w / 2;
        float y = (float)position_boxes[index].Y + h / 2;
        float r = rotations[index];
        float w_ = w > h ? w : h;
        float h_ = w > h ? h : w;
        r = (float)((w > h ? r : (float)(r + Math.PI / 2)) % Math.PI);
        RotatedRect rotate = new RotatedRect(new Point2f(x, y), new Size2f(w_, h_), (float)(r * 180.0 / Math.PI));
        rotated_rects.Add(rotate);
    }
    for (int i = 0; i < indexes.Length; i++)
    {
        int index = indexes[i];
        Point2f[] points = rotated_rects[i].Points();
        for (int j = 0; j < 4; j++)
        {
            Cv2.Line(image, (Point)points[j], (Point)points[(j + 1) % 4], new Scalar(255, 100, 200), 2);
        }
        Cv2.PutText(image, class_lables[class_ids[index]] + "-" + confidences[index].ToString("0.00"),
            (Point)points[0], HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 0, 0), 2);
    }
    string output_path = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(image_path)),
        Path.GetFileNameWithoutExtension(image_path) + "_result.jpg");
    Cv2.ImWrite(output_path, image);
    Slog.INFO("The result save to " + output_path);
    Cv2.ImShow("Result", image);
    Cv2.WaitKey(0);
}
```

Yolov8 Obb模型的一条预测结果输出为 [x, y, w, h, sorce0, ···, sorce14, angle]，其中前19个数据与Yolov8 Det模型的数据处理方式是一致的，主要是多了一个预测框旋转角度，因此在处理时需要同时记录旋转角度这一个数据。

### 3.4 预测方法调用

定义好上述方法后，便可以直接在主函数中调用该方法，只需要在主函数中增加以下代码即可：

```csharp
yolov8_obb("yolov8s-obb.xml", "test_image.png", "AUTO");
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
        if (!File.Exists("./model/yolov8s-obb.bin") && !File.Exists("./model/yolov8s-obb.bin"))
        {
            if (!File.Exists("./model/yolov8s-obb.tar"))
            {
                _ = Download.download_file_async("https://github.com/guojin-yan/OpenVINO-CSharp-API-Samples/releases/download/Model/yolov8s-obb.tar",
                    "./model/yolov8s-obb.tar").Result;
            }
            Download.unzip("./model/yolov8s-obb.tar", "./model/");
        }
        if (!File.Exists("./model/plane.png"))
        {
            _ = Download.download_file_async("https://github.com/guojin-yan/OpenVINO-CSharp-API-Samples/releases/download/Image/plane.png",
                "./model/plane.png").Result;
        }
        model_path = "./model/yolov8s-obb.xml";
        image_path = "./model/plane.png";
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
    yolov8_obb(model_path, image_path, device);
}
```

> 备注：
>
> 上述项目中的完整代码全部放在GitHub上开源，项目链接为：
>
> ```
> https://github.com/guojin-yan/OpenVINO-CSharp-API-Samples/tree/master/model_samples/yolov8/yolov8_obb_opencvsharp
> ```

## 4. YOLOv8 OBB项目配置(Emgu.CV)

相信有不少开发者在C#中进行图像处理时，使用的是Emgu.CV工具，因此，在此处我们同时提供了使用 Emgu.CV作为图像处理工具的YOLOv8 OBB模型部署代码。项目的创建方式与流程与上一节中一致，此处不再进行演示：

### 4.1 添加项目依赖

此处以Windows平台为例安装项目依赖，首先是安装OpenVINO™ C# API项目依赖，在命令行中输入以下指令即可：

```
dotnet add package OpenVINO.CSharp.API
dotnet add package OpenVINO.runtime.win
dotnet add package OpenVINO.CSharp.API.Extensions
dotnet add package OpenVINO.CSharp.API.Extensions.EmguCV
```

接下来安装使用到的图像处理库Emgu.CV，在命令行中输入以下指令即可：

```
dotnet add package Emgu.CV
dotnet add package Emgu.CV.runtime.windows
```

添加完成项目依赖后，项目的配置文件如下所示：

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Emgu.CV" Version="4.8.1.5350" />
    <PackageReference Include="Emgu.CV.runtime.windows" Version="4.8.1.5350" />
    <PackageReference Include="OpenVINO.CSharp.API" Version="2023.2.0.4" />
    <PackageReference Include="OpenVINO.CSharp.API.Extensions" Version="1.0.1" />
    <PackageReference Include="OpenVINO.CSharp.API.Extensions.EmguCV" Version="1.0.4.1" />
    <PackageReference Include="OpenVINO.runtime.win" Version="2023.3.0.1" />
  </ItemGroup>

</Project>
```

### 4.2 定义预测方法

Yolov8 Obb模型部署流程与上一节中的流程一致，主要是替换了图像处理方式，其实现代码如下：

```csharp
static void yolov8_obb(string model_path, string image_path, string device)
{
    // -------- Step 1. Initialize OpenVINO Runtime Core --------
    Core core = new Core();
    // -------- Step 2. Read inference model --------
    OpenVinoSharp.Model model = core.read_model(model_path);
    OvExtensions.printf_model_info(model);
    // -------- Step 3. Loading a model to the device --------
    CompiledModel compiled_model = core.compile_model(model, device);
    // -------- Step 4. Create an infer request --------
    InferRequest infer_request = compiled_model.create_infer_request();
    // -------- Step 5. Process input images --------
    Mat image = new Mat(image_path); // Read image by opencvsharp
    int max_image_length = image.Cols > image.Rows ? image.Cols : image.Rows;
    Mat max_image = Mat.Zeros(max_image_length, max_image_length, DepthType.Cv8U, 3);
    Rectangle roi = new Rectangle(0, 0, image.Cols, image.Rows);
    image.CopyTo(new Mat(max_image, roi));
    float factor = (float)(max_image_length / 1024.0);
    // -------- Step 6. Set up input data --------
    Tensor input_tensor = infer_request.get_input_tensor();
    Shape input_shape = input_tensor.get_shape();
    Mat input_mat = DnnInvoke.BlobFromImage(max_image, 1.0 / 255.0, new Size((int)input_shape[2], (int)input_shape[3]), new MCvScalar(0), true, false);
    float[] input_data = new float[input_shape[1] * input_shape[2] * input_shape[3]];
    //Marshal.Copy(input_mat.Ptr, input_data, 0, input_data.Length);
    input_mat.CopyTo<float>(input_data);
    input_tensor.set_data<float>(input_data);
    // -------- Step 7. Do inference synchronously --------
    infer_request.infer();
    // -------- Step 8. Get infer result data --------
    Tensor output_tensor = infer_request.get_output_tensor();
    int output_length = (int)output_tensor.get_size();
    float[] output_data = output_tensor.get_data<float>(output_length);
    // -------- Step 9. Process reault  --------
    Mat result_data = new Mat(20, 21504, DepthType.Cv32F, 1,
                   Marshal.UnsafeAddrOfPinnedArrayElement(output_data, 0), 4 * 21504);
    result_data = result_data.T();
    List<Rectangle> position_boxes = new List<Rectangle>();
    List<int> class_ids = new List<int>();
    List<float> confidences = new List<float>();
    List<float> rotations = new List<float>();
    // Preprocessing output results
    for (int i = 0; i < result_data.Rows; i++)
    {
        Mat classes_scores = new Mat(result_data, new Rectangle(4, i, 15, 1));//GetArray(i, 5, classes_scores);
        Point max_classId_point = new Point(), min_classId_point = new Point();
        double max_score = 0, min_score = 0;
        CvInvoke.MinMaxLoc(classes_scores, ref min_score, ref max_score,
            ref min_classId_point, ref max_classId_point);
        if (max_score > 0.25)
        {
            Mat mat = new Mat(result_data, new Rectangle(0, i, 20, 1));
            float[,] data = (float[,])mat.GetData();
            float cx = data[0, 0];
            float cy = data[0, 1];
            float ow = data[0, 2];
            float oh = data[0, 3];
            int x = (int)((cx - 0.5 * ow) * factor);
            int y = (int)((cy - 0.5 * oh) * factor);
            int width = (int)(ow * factor);
            int height = (int)(oh * factor);
            Rectangle box = new Rectangle();
            box.X = x;
            box.Y = y;
            box.Width = width;
            box.Height = height;

            position_boxes.Add(box);
            class_ids.Add(max_classId_point.X);
            confidences.Add((float)max_score);
            rotations.Add(data[0, 19]);
        }
    }

    // NMS non maximum suppression
    int[] indexes = DnnInvoke.NMSBoxes(position_boxes.ToArray(), confidences.ToArray(), 0.5f, 0.5f);

    List<RotatedRect> rotated_rects = new List<RotatedRect>();
    for (int i = 0; i < indexes.Length; i++)
    {
        int index = indexes[i];

        float w = (float)position_boxes[index].Width;
        float h = (float)position_boxes[index].Height;
        float x = (float)position_boxes[index].X + w / 2;
        float y = (float)position_boxes[index].Y + h / 2;
        float r = rotations[index];
        float w_ = w > h ? w : h;
        float h_ = w > h ? h : w;
        r = (float)((w > h ? r : (float)(r + Math.PI / 2)) % Math.PI);
        RotatedRect rotate = new RotatedRect(new PointF(x, y), new SizeF(w_, h_), (float)(r * 180.0 / Math.PI));
        rotated_rects.Add(rotate);
    }
    for (int i = 0; i < indexes.Length; i++)
    {
        int index = indexes[i];

        PointF[] points = rotated_rects[i].GetVertices();
        for (int j = 0; j < 4; j++)
        {
            CvInvoke.Line(image, new Point((int)points[j].X, (int)points[j].Y),
                new Point((int)points[(j + 1) % 4].X, (int)points[(j + 1) % 4].Y), new MCvScalar(255, 100, 200), 2);
        }
        CvInvoke.PutText(image, class_lables[class_ids[index]] + "-" + confidences[index].ToString("0.00"),
            new Point((int)points[0].X, (int)points[0].Y), FontFace.HersheySimplex, 0.8, new MCvScalar(0, 0, 0), 2);
    }
    string output_path = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(image_path)),
        Path.GetFileNameWithoutExtension(image_path) + "_result.jpg");
    CvInvoke.Imwrite(output_path, image);
    Slog.INFO("The result save to " + output_path);
    CvInvoke.Imshow("Result", image);
    CvInvoke.WaitKey(0);
}
```

> 备注：
>
> 上述项目中的完整代码全部放在GitHub上开源，项目链接为：
>
> ```
> https://github.com/guojin-yan/OpenVINO-CSharp-API-Samples/tree/master/model_samples/yolov8/yolov8_obb_emgucv
> ```

## 5. 项目编译与运行

### 5.1 项目文件编译

接下来输入项目编译指令进行项目编译，输入以下指令即可：

```
dotnet build
```

程序编译后输出为：

<div align=center><img src="https://s2.loli.net/2024/01/29/aQXG6r4sgLCYiUc.png" width=800></div>

### 5.2 项目文件运行

接下来运行编译后的程序文件，在CMD中输入以下指令，运行编译后的项目文件：

```
dotnet run --no-build
```

运行后项目输出为：

<div align=center><img src="https://s2.loli.net/2024/01/29/CJtc9akIBFRdYzE.png" width=800></div>

<div align=center><img src="https://s2.loli.net/2024/01/29/6gPH3IlBfTXVnsN.jpg" width=800></div>

## 6. 总结

在该项目中，我们结合之前开发的 OpenVINO™ C# API 项目部署YOLOv8 OBB 模型，成功实现了旋转对象目标检测，并且根据不同开发者的使用习惯，同时提供了OpenCvSharp以及Emgu.CV两种版本，供各位开发者使用。最后如果各位开发者在使用中有任何问题，欢迎大家与我联系。

<div align=center><img src="https://s2.loli.net/2024/01/29/VIPU1MSwjEh2QAY.png" width=800></div>
