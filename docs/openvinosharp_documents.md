# OpenVinoSharp 2.0 技术文档 

**编写说明：**

&emsp; OpenVinoSharp 是基于Intel推出的用于优化和部署深度学习模型的开源工具包[OpenVINO™](www.openvino.ai) ，利用动态链接库特性，重新封装模型部署API，将其转换到.NET平台。由于在进行封装时基于项目进行，因此应对更加复杂的情况或者特殊情况需要修改API封装，为方便大家进行重新封装，此处提供详细的技术文章，也期望大家可以帮助完善该项目。



[TOC]



## 开发环境

&emsp; 为了防止复现代码出现问题，列出以下代码开发环境，可以根据自己需求设置，注意OpenVINOTM一定是2022版本，其他依赖项可以根据自己的设置修改。

- 操作系统：Windows 11

- OpenVINOTM：2022.3

- OpenCV：4.5.5

- Visual Studio：2022

- C#框架：.NET 6.0

- OpenCvSharp：OpenCvSharp4

# 1.  创建OpenVINO<sup>TM </sup>方法C++动态链接库

## 1.1  新建解决方案以及项目文件

&emsp; 打开vs2022，首先新建一个C++空项目文件，并将同时新建一个解决方案命名为：OpenVinoSharp，用于存放后续其他项目文件。

&emsp; 添加一个C++空项目，命名为OpenVinoSharpExtern，并依次添加openvino_api.cpp、openvino_api.h、common.cpp以及common.h文件，如图所示：

![image-20230126204533253](https://s2.loli.net/2023/01/26/26rHvxk1AbBPsg8.png)

## 1.2  配置C++项目属性

&emsp; 右击项目，点击属性，进入到属性设置，此处需要设置项目的配置类型包含目录、库目录以及附加依赖项，本次项目选择Release模式下运行，因此以Release情况进行配置。

#### (1) 设置配置与平台

&emsp; 进入属性设置后，在最上面，将配置改为Release，平台改为x64。具体操作如图所示。

![image-20230126204824499](https://s2.loli.net/2023/01/26/zJExyt4IZXToHfD.png)

#### (2) 设置常规属性

常规设置下，将目标文件名修改为：``OpenVinoSharpExtern``；最后将配置类型改为：``动态库(.dll)``，让其生成dll文件。具体操作如图所示。

![image-20230126204918854](https://s2.loli.net/2023/01/26/fDFlLw1OTqECW97.png)

#### (3) 设置包含目录和库目录

点击VC++目录，然后点击包含目录，进行编辑，在弹出的新页面中，添加以下路径：

```shell
E:\OpenCV Source\opencv-4.5.5\build\include
E:\OpenCV Source\opencv-4.5.5\build\include\opencv2
C:\Program Files (x86)\Intel\openvino_2022.3.0.9052\runtime\include
C:\Program Files (x86)\Intel\openvino_2022.3.0.9052\runtime\include\ie
```

其中路径``E:\OpenCV Source\opencv-4.5.5\``为OpenCV的安装路径，``C:\Program Files (x86)\Intel\``为OpenVINO安装路径，个人根据自己安装的位置及版本确定。

点击库目录，进行编辑，在弹出的新页面中，添加以下路径：

```
E:\OpenCV Source\opencv-4.5.5\build\x64\vc15\lib
C:\Program Files (x86)\Intel\openvino_2022.3.0.9052\runtime\lib\intel64\Release
```

具体操作如图所示。

![image-20230126205143323](https://s2.loli.net/2023/01/26/tBwOCfUgv2GQSNY.png)

#### (4) 设置附加依赖项

点击展开链接器，点击输入，在附加依赖项中点击编辑，在弹出来的新的页面，添加以下文件名：

```
opencv_world455.lib

openvino.lib
```

具体操作步骤参考如图所示。新版OpenCV与OpenVINO<sup>TM </sup>都将依赖库文件合成到了一个文件中，这极大地简化了使用，如果使用老版本的，需要将所有的.lib文件放置在此处即可。

![image-20230127125311834](https://s2.loli.net/2023/01/27/fU9kGmM23IZVhBE.png)



## 1.3  编写C++代码

#### （1）推理引擎结构体

``Core``是OpenVINO<sup>TM</sup>工具套件里的推理核心类，该类下包含多个方法，可用于创建推理中所使用的其他类。在此处，需要在各个方法中传递的仅仅是所使用的几个变量，因此选择构建一个推理引擎结构体，用于存放各个变量。

```c++
// @brief 推理核心结构体
typedef struct openvino_core_struct {
	ov::Core core; // core对象
	std::shared_ptr<ov::Model> model_ptr; // 读取模型指针
	ov::CompiledModel compiled_model; // 模型加载到设备对象
	ov::InferRequest infer_request; // 推理请求对象
} CoreStruct;
```



其中``Core``是OpenVINO<sup>TM </sup>工具套件里的推理机核心，该模块只需要初始化； ``shared_ptr<ov::Model>``是读取本地模型的方法，新版更新后，该方法发生了较大改动，可支持读取Paddlepaddle飞桨模型、onnx模型以及IR模型；``CompiledModel``指的是一个已编译的模型类，其主要是将读取的本地模型应用多个优化转换，然后映射到计算内核，由所指定的设备编译模型；``InferRequest``是一个推理请求类，在推理中主要用于对推理过程的操作。

#### （2）接口方法规划

经典的OpenVINO<sup>TM </sup>进行模型推理，一般需要八个步骤，主要是：初始化``Core``对象、读取本地推理模型、配置模型输入&输出、载入模型到执行硬件、创建推理请求、准备输入数据、执行推理计算以及处理推理计算结果。我们根据原有的八个步骤，对步骤进行重新整合，并根据推理步骤，调整方法接口。

对于方法接口，主要设置为：推理初始化、配置输入数据形状、配置输入数据、模型推理、读取推理结果数据以及删除内存地址六个大类，配置输入数据要细分为配置图片输入数据与配置普通数据输入，读取推理结果数据细分为读取float数据、int数据以及log long数据，因此，总共有6类方法接口，9个方法接口。

#### （3）初始化推理模型

OpenVINO<sup>TM </sup>推理引擎结构体是联系各个方法的桥梁，后续所有操作都是在推理引擎结构体中的变量上操作的，为了实现数据在各个方法之间的传输，因此在创建推理引擎结构体时，采用的是创建结构体指针，并将创建的结构体地址作为函数返回值返回。推理初始化接口主要整合了原有推理的初始化Core对象、读取本地推理模型、载入模型到执行硬件和创建推理请求步骤，并将这些步骤所创建的变量放在推理引擎结构体中。

初始化推理模型接口方法为：

```c++
// @brief 初始化openvino核心结构体，读取本地推理模型，将模型加载到设备，并创建推理请求
EXTERN_C __MIDL_DECLSPEC_DLLEXPORT void* STDMETHODCALLTYPE core_init(
	const wchar_t* w_model_dir, const wchar_t* w_device, const wchar_t* w_cache_dir);
```

该方法返回值为``CoreStruct``结构体指针，其中``w_model_dir``为推理模型本地地址字符串指针，``w_device``为模型运行设备名指针，``w_cache_dir``为缓存路径，在后面使用上述变量时，需要将其转换为string字符串，利用``wchar_to_string()``方法可以实现将其转换为字符串格式：

```c++
std::string model_dir = wchar_to_string(w_model_dir);// 推理模型本地地址
std::string device = wchar_to_string(w_device);// 加载设备名称
std::string cache_dir = wchar_to_string(w_cache_dir); // 缓存地址
```

模型初始化功能主要包括：初始化推理引擎结构体和对结构体里面定义的其他变量进行赋值操作，其主要是利用``CoreStruct``中创建的``Core``类中的方法，对各个变量进行初始化操作：

```c++
// 初始化推理核心
CoreStruct* openvino_core = new CoreStruct(); // 创建推理引擎指针
openvino_core->model_ptr = openvino_core->core.read_model(model_dir); // 读取推理模型
if (cache_dir != "") {
	openvino_core->core.set_property(ov::cache_dir(cache_dir)); // 设置缓存路径
}
openvino_core->compiled_model = openvino_core->core.compile_model(openvino_core->model_ptr, device); // 将模型加载到设备
openvino_core->infer_request = openvino_core->compiled_model.create_infer_request(); // 创建推理请求
```

#### （4）配置输入数据形状

在新版OpenVINO<sup>TM </sup>2022中，新增加了对Paddlepaddle模型以及onnx模型的支持，Paddlepaddle模型不支持指定指定默认bath通道数量，因此需要在模型使用时指定其输入；其次，对于onnx模型，也可以在转化时不指定固定形状，因此在配置输入数据前，需要配置输入节点数据形状。其方法接口为：

```c++
// @brief 为输入tensor设置新形状，如果新的总大小大于前一个，则取消之前的设置
EXTERN_C __MIDL_DECLSPEC_DLLEXPORT void* STDMETHODCALLTYPE set_input_sharp(
	void* core_ptr, const wchar_t* w_node_name, size_t * input_shape, int input_size);
```

该方法返回值是``CoreStruct``结构体指针，但该指针所对应的数据中已经包含了对输入形状的设置。第一个输入参数``core_ptr``是``CoreStruct``指针，在当前方法中，我们要读取该指针，并将其转换为``CoreStruct``类型;；并将输入节点名``w_node_name``转为``string``格式。

```
CoreStruct* openvino_core = (CoreStruct*)core_ptr;
std::string node_name = wchar_to_string(w_node_name);
```

``w_node_name ``为待设置网络节点名，``input_shape ``为形状数据数组。对图片数据，需要设置`` [batch, dim, height, width]`` 四个维度大小;所以``input_size``数组传入4个数据；对于其他类型的数据，其长度未知，因此为了更加方便，此处输入``input_size``来指定数据长度。其设置在形状主要使用``Tensor``类下的``set_shape()``方法：

```c++
// 获取指定节点的tensor
ov::Tensor tensor = openvino_core->infer_request.get_tensor(node_name);
ov::Shape shape(input_shape, input_shape + input_size);
// 设置节点输入数据的形状
tensor.set_shape(shape);
```

#### （5）配置输入数据

在新版OpenVINO<sup>TM </sup>中，``Tensor``类的``T* data()``方法，其返回值为当前节点``Tensor``的数据内存地址，通过填充``Tensor``的数据内存，实现推理数据的输入。对于图片数据，其最终也是将其转为一维数据进行输入，不过为方便使用，此处提供了配置图片数据和普通数据的接口，对于输入为图片的方法接口：

```c++
// @brief 将图片数据加载到tensor中的数据内存上
EXTERN_C __MIDL_DECLSPEC_DLLEXPORT void* STDMETHODCALLTYPE load_image_input_data(
	void* core_ptr, const wchar_t* w_node_name, uchar * image_data, size_t image_size, int type);
// @brief 将其他数据加载到tensor中的数据内存上
EXTERN_C __MIDL_DECLSPEC_DLLEXPORT void* STDMETHODCALLTYPE load_input_data(
	void* core_ptr, const wchar_t* w_node_name, float* input_data);
```

该方法返回值是``CoreStruct``结构体指针，但该指针所对应的数据中已经包含了加载的图片数据。第一个输入参数``core_ptr``是``CoreStruct``指针，在当前方法中，我们要读取该指针，并将其转换为``CoreStruct``类型；第二个输入参数``w_node_name``为待填充节点名，先将其转为string字符串：

```c++
// 读取推理模型地址
CoreStruct* openvino_core = (CoreStruct*)core_ptr;
std::string node_name = wchar_to_string(w_node_name);
```

##### （5.1）图片数据

在该项目中，我们主要使用的是以图片作为模型输入的推理网络，模型主要的输入为图片的输入。其图片数据主要存储在矩阵``image_data``和矩阵长度``image_size``两个变量中。需要对图片数据进行整合处理，利用创建的``data_to_mat () ``方法，将图片数据读取到OpenCV中：

```c++
cv::Mat input_image = data_to_mat(image_data, image_size); // 读取输入图片
```

接下来就是配置网络图片数据输入，对于节点输入是图片数据的网络节点，其配置网络输入主要分为以下几步：

**(a)  获取网络输入图片大小**

使用``CoreStruct``类中的``get_tensor ()``方法，获取指定网络节点的``Tensor``，其节点要求输入大小在``Shape``容器中，通过获取该容器，得到图片的长宽信息：

```c++
// 获取输入节点tensor
ov::Tensor input_image_tensor = openvino_core->infer_request.get_tensor(node_name);
int input_H = input_image_tensor.get_shape()[2]; //获得"image"节点的Height
int input_W = input_image_tensor.get_shape()[3]; //获得"image"节点的Width
```

 **(b)  处理推理数据**

在这一步，我们除了要按照输入大小对图片进行放缩之外，还要对模型输入进行处理。因此处理图片其主要分为交换RGB通道、放缩图片以及对图片进行归一化处理。在此处我们借助OpenCV来实现。

OpenCV读取图片数据并将其放在``Mat``类中，其读取的图片数据是BGR通道格式，模型要求输入格式为RGB通道格式，其通道转换主要靠一下方式实现：

```c++
 cv::cvtColor(input_image, blob_image, cv::COLOR_BGR2RGB); // 将图片通道由 BGR 转为 RGB
```

接下来就是根据网络输入要求，对图片相应处理，主要包括图片缩放以及归一化处理，图片缩放有普通缩放以及仿射变换缩放两种，而归一化方式也有多种，因此此处通过``type``进行选择，以实现不同的处理方式。

- **type=0:  插值缩放、均方差归一化**

```c++
// 对输入图片按照tensor输入要求进行缩放
cv::resize(blob_image, blob_image, cv::Size(input_W, input_H), 0, 0, cv::INTER_LINEAR);
// 图像数据归一化，减均值mean，除以方差std
// PaddleDetection模型使用imagenet数据集的均值 Mean = [0.485, 0.456, 0.406]和方差 std = [0.229, 0.224, 0.225]
std::vector<float> mean_values{ 0.485 * 255, 0.456 * 255, 0.406 * 255 };
std::vector<float> std_values{ 0.229 * 255, 0.224 * 255, 0.225 * 255 };
std::vector<cv::Mat> rgb_channels(3);
cv::split(blob_image, rgb_channels); // 分离图片数据通道
for (auto i = 0; i < rgb_channels.size(); i++) {
    //分通道依此对每一个通道数据进行归一化处理
    rgb_channels[i].convertTo(rgb_channels[i], CV_32FC1, 1.0 / std_values[i], (0.0 - mean_values[i]) / std_values[i]);
}
cv::merge(rgb_channels, blob_image); // 合并图片数据通道
```

- **type=1:  插值缩放、普通归一化(1/255)**

```c++
// 对输入图片按照tensor输入要求进行缩放
cv::resize(blob_image, blob_image, cv::Size(input_W, input_H), 0, 0, cv::INTER_LINEAR);
// 图像数据归一化
std::vector<float> std_values{ 1.0 * 255, 1.0 * 255, 1.0 * 255 };
std::vector<cv::Mat> rgb_channels(3);
cv::split(blob_image, rgb_channels); // 分离图片数据通道
for (auto i = 0; i < rgb_channels.size(); i++) {
    //分通道依此对每一个通道数据进行归一化处理
    rgb_channels[i].convertTo(rgb_channels[i], CV_32FC1, 1.0 / std_values[i], 0);
}
cv::merge(rgb_channels, blob_image); // 合并图片数据通道
```

- **type=2:  插值缩放、不归一化**

```c++
// 对输入图片按照tensor输入要求进行缩放
cv::resize(blob_image, blob_image, cv::Size(input_W, input_H), 0, 0, cv::INTER_LINEAR);
// 图像数据归一化
std::vector<float> std_values{ 1.0, 1.0, 1.0 };
std::vector<cv::Mat> rgb_channels(3);
cv::split(blob_image, rgb_channels); // 分离图片数据通道
for (auto i = 0; i < rgb_channels.size(); i++) {
    //分通道依此对每一个通道数据进行归一化处理
    rgb_channels[i].convertTo(rgb_channels[i], CV_32FC1, 1.0 / std_values[i], 0);
}
cv::merge(rgb_channels, blob_image); // 合并图片数据通道
```

- **type=3:  仿射变换、均方差归一化**

```c++
// 获取仿射变换信息
cv::Point center(blob_image.cols / 2, blob_image.rows / 2); // 变换中心
cv::Size input_size(blob_image.cols, blob_image.rows); // 输入尺寸
int rot = 0; // 角度
cv::Size output_size(input_W, input_H); // 输出尺寸

// 获取仿射变换矩阵
cv::Mat warp_mat(2, 3, CV_32FC1);
warp_mat = get_affine_transform(center, input_size, rot, output_size);
// 仿射变化
cv::warpAffine(blob_image, blob_image, warp_mat, output_size, cv::INTER_LINEAR);
// 图像数据归一化
std::vector<float> mean_values{ 0.485 * 255, 0.456 * 255, 0.406 * 255 };
std::vector<float> std_values{ 0.229 * 255, 0.224 * 255, 0.225 * 255 };
std::vector<cv::Mat> rgb_channels(3);
cv::split(blob_image, rgb_channels); // 分离图片数据通道
for (auto i = 0; i < rgb_channels.size(); i++) {
    //分通道依此对每一个通道数据进行归一化处理
    rgb_channels[i].convertTo(rgb_channels[i], CV_32FC1, 1.0 / std_values[i], (0.0 - mean_values[i]) / std_values[i]);
}
cv::merge(rgb_channels, blob_image); // 合并图片数据通道
```

- **type=4:  仿射变换、普通归一化(1/255)**

```c++
// 获取仿射变换信息
cv::Point center(blob_image.cols / 2, blob_image.rows / 2); // 变换中心
cv::Size input_size(blob_image.cols, blob_image.rows); // 输入尺寸
int rot = 0; // 角度
cv::Size output_size(input_W, input_H); // 输出尺寸

// 获取仿射变换矩阵
cv::Mat warp_mat;
warp_mat = get_affine_transform(center, input_size, rot, output_size);
// 仿射变化
cv::warpAffine(blob_image, blob_image, warp_mat, output_size, cv::INTER_LINEAR);
// 图像数据归一化
std::vector<float> std_values{ 1.0 * 255, 1.0 * 255, 1.0 * 255 };
std::vector<cv::Mat> rgb_channels(3);
cv::split(blob_image, rgb_channels); // 分离图片数据通道
for (auto i = 0; i < rgb_channels.size(); i++) {
    //分通道依此对每一个通道数据进行归一化处理
    rgb_channels[i].convertTo(rgb_channels[i], CV_32FC1, 1.0 / std_values[i], 0);
}
cv::merge(rgb_channels, blob_image); // 合并图片数据通道
```

- **type=5:  仿射变换、均方差归一化**

```c++
// 获取仿射变换信息
cv::Point center(blob_image.cols / 2, blob_image.rows / 2); // 变换中心
cv::Size input_size(blob_image.cols, blob_image.rows); // 输入尺寸
int rot = 0; // 角度
cv::Size output_size(input_W, input_H); // 输出尺寸

// 获取仿射变换矩阵
cv::Mat warp_mat;
warp_mat = get_affine_transform(center, input_size, rot, output_size);
// 仿射变化
cv::warpAffine(blob_image, blob_image, warp_mat, output_size, cv::INTER_LINEAR);
// 图像数据归一化
std::vector<float> std_values{ 1.0, 1.0, 1.0 };
std::vector<cv::Mat> rgb_channels(3);
cv::split(blob_image, rgb_channels); // 分离图片数据通道
for (auto i = 0; i < rgb_channels.size(); i++) {
    //分通道依此对每一个通道数据进行归一化处理
    rgb_channels[i].convertTo(rgb_channels[i], CV_32FC1, 1.0 / std_values[i], 0);
}
cv::merge(rgb_channels, blob_image); // 合并图片数据通道
```

**（c）加载到推理通道**

在此处，我们重写了网络赋值方法，并将其封装到`` fill_tensor_data_image(ov::Tensor& input_tensor, const cv::Mat& input_image)``方法中，``input_tensor为``模型输入节点``Tensor``类，``input_image``为处理过的图片Mat数据。因此节点赋值只需要调用该方法即可：

```c++
// 将图片数据填充到tensor数据内存中
fill_tensor_data_image(input_image_tensor, blob_image);
```

##### （5.2）普通数据

​    与配置图片数据不同点，在于输入数据只需要输入``input_data``数组即可。其数据处理哦在外部实现，只需要将处理后的数据填充到输入节点的数据内存中即可，通过调用自定义的``fill_tensor_data_float(ov::Tensor& input_tensor, float* input_data, int data_size) ``方法即可实现：

```c++
// 读取指定节点tensor
ov::Tensor input_image_tensor = openvino_core->infer_request.get_tensor(input_node_name);
std::vector<size_t> input_shape = input_image_tensor.get_shape(); //获得输入节点的形状
int input_size = std::accumulate(input_shape.begin(), input_shape.end(), 1, std::multiplies<int>()); // 获取长度
// 将数据填充到tensor数据内存上
fill_tensor_data_float(input_image_tensor, input_data, input_size);
```

#### （6）模型推理

上一步中我们将推理内容的数据输入到了网络中，在这一步中，我们需要进行数据推理，这一步中我们留有一个推理接口：

```c++
// @brief 对加载好的推理模型进行推理
EXTERN_C __MIDL_DECLSPEC_DLLEXPORT void* STDMETHODCALLTYPE core_infer(void* core_ptr);
```

进行模型推理，只需要调用``CoreStruct``结构体中的``infer_request``对象中的``infer()``方法即可：

```c++
// 读取推理模型地址
CoreStruct* openvino_core = (CoreStruct*)core_ptr;
// 模型预测
openvino_core->infer_request.infer();
```

#### （7）读取推理数据

上一步我们对数据进行了推理，这一步就需要查询上一步推理的结果。对于我们所使用的模型输出，主要有float数据和int数据，对此，留有了两种数据的查询接口，其方法为：

```c++
// @brief 查询float类型的推理结果
EXTERN_C __MIDL_DECLSPEC_DLLEXPORT void STDMETHODCALLTYPE read_infer_result_F32(
	void* core_ptr, const wchar_t* w_node_name, float* infer_result);
// @brief 查询int类型的推理结果
EXTERN_C __MIDL_DECLSPEC_DLLEXPORT void STDMETHODCALLTYPE read_infer_result_I32(
	void* core_ptr, const wchar_t* w_node_name, int* infer_result);
// @brief 查询long long类型的推理结果
EXTERN_C __MIDL_DECLSPEC_DLLEXPORT void STDMETHODCALLTYPE read_infer_result_I64(
	void* core_ptr, const wchar_t* w_node_name, long long* infer_result);
```

其中``w_node_name``为输出节点名称，``infer_result ``为输出数组指针。读取推理结果数据与加载推理数据方式相似，依旧是读取输出节点处数据内存的地址：

```
// 读取推理模型地址
CoreStruct* openvino_core = (CoreStruct*)core_ptr;
std::string output_node_name = wchar_to_string(w_node_name);
// 读取指定节点的tensor
const ov::Tensor& output_tensor = openvino_core->infer_request.get_tensor(output_node_name);
std::vector<size_t> input_shape = output_tensor.get_shape(); //获得输入节点的形状
int output_size = std::accumulate(input_shape.begin(), input_shape.end(), 1, std::multiplies<int>()); // 获取长度
// 获取网络节点数据地址
const float* results = output_tensor.data<const float>();
```

针对读取整形数据，其方法一样，只是在转换类型时，需要将其转换为整形数据即可。我们读取的初始数据为二进制数据，因此要根据指定类型转换，否则数据会出现错误。将数据读取出来后，将其放在数据结果指针中，并将所有结果赋值到输出数组中：

```
// 将输出结果复制到输出地址指针中
for (int i = 0; i < output_size; i++) {
    *infer_result = results[i];
    infer_result++;
}
```

#### （8）删除推理核心结构体指针

推理完成后，我们需要将在内存中创建的推理核心结构地址删除，防止造成内存泄露，影响电脑性能，其接口该方法为：

```c++
// @brief 删除推理核心结构体指针，释放占用内存
EXTERN_C __MIDL_DECLSPEC_DLLEXPORT void STDMETHODCALLTYPE core_delet(void* core_ptr);
```

在该方法中，我们只需要调用``delete``命令，将结构体指针删除即可。

## 1.4  生成dll文件

前面我们将项目配置输出设置为了生成dll文件，因此该项目不是可以执行的exe文件，只能生成不能运行。右键项目，选择重新生成/生成。在没有错误的情况下，会看到项目成功的提示。可以看到dll文件在解决方案同级目录下\x64\Release\文件夹下。

使用dll文件查看器打开dll文件，如图所示；可以看到，我们创建的四个方法接口已经暴露在dll文件中。

![image-20230127191059706](https://s2.loli.net/2023/01/27/1XmWwcCUIpKbkJ9.png)



# 2. 创建OpenVinoSharp类库

## 2.1  新建C#类库

右击解决方案，添加->新建项目，选择添加C#类库，项目名命名为OpenVinoSharp，项目框架根据电脑中的框架选择，此处使用的是.NET 5.0。新建完成后，然后右击项目，选择添加->新建项，选择类文件，添加Core.cs和NativeMethods.cs两个类文件。

![image-20230127191648102](https://s2.loli.net/2023/01/27/D5jOKoTndLkRbG3.png)

## 2.2 引入dll文件中的方法

在NativeMethods.cs文件下，我们通过[DllImport()]方法，将dll文件中所有的方法读取到C#中。读取方式如下：

```c#
private const string dll_extern = "OpenVinoSharpExtern.dll";

[DllImport(dll_extern, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
public extern static IntPtr core_init(string model_dir, string device, string w_cache_dir);
```

其中dll_extern为dll文件路径，``CharSet = CharSet.Unicode``代表支持中文编码格式字符串，``CallingConvention = CallingConvention.Cdecl``指示入口点的调用约定为调用方清理堆栈。

上述所列出的为初始化推理模型，dll文件接口在匹配时，是通过方法名字匹配的，因此，方法名要保证与dll文件中一致。其次就是方法的参数类型要进行对应，在上述方法中，函数的返回值在C++中为``void*`` ，在C#中对应的为``IntPtr``类型，输入参数中，在C++中为``wchar_t* ``字符指针，在C#中对应的为``string``字符串。通过方法名与参数类型一一对应，在C#可以实现对方法的调用。

|            | C++            | C#              |
| ---------- | -------------- | --------------- |
| 返回值类型 | void*          | IntPtr          |
| 方法名     | set_inputsharp | set_input_sharp |
| 参数1      | void*          | IntPtr          |
| 参数2      | wchar_t*       | string          |
| 参数3      | size_t *       | ref ulong       |

基于以上方法，我们将动态链接库中的所有方法引入到C#中。

```c#
[DllImport(dll_extern, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
public extern static IntPtr core_init(string model_dir, string device, string w_cache_dir);

[DllImport(dll_extern, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
public extern static IntPtr set_input_sharp(IntPtr core, string node_name,
    ref ulong input_shape, int input_size);

[DllImport(dll_extern, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
public extern static IntPtr load_image_input_data(IntPtr core, string input_node_name,
    ref byte image_data, ulong image_size, int type);

[DllImport(dll_extern, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
public extern static IntPtr load_input_data(IntPtr core, string input_node_name, ref float input_data);

[DllImport(dll_extern, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
public extern static IntPtr core_infer(IntPtr core);

[DllImport(dll_extern, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
public extern static void read_infer_result_F32(IntPtr core, string node_name, ref float result);

[DllImport(dll_extern, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
public extern static void read_infer_result_I32(IntPtr core, string node_name, ref int result);
[DllImport(dll_extern, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
public extern static void read_infer_result_I64(IntPtr core, string node_name, ref long result);

[DllImport(dll_extern, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
public extern static void core_delet(IntPtr core);
```

## 2.3 C#构建Core类

上一步我们引入了封装的OpenVINOTM动态链接库，为了更方便的使用，将其封装到Core类中。在不同方法之间，主要通过推理核心结构体指针在各个方法之间传递，在C#是没有指针这个说法的，不过可以通过IntPtr结构体来接收这个指针，为了防止该指针被篡改，将其封装在类中作为私有成员使用。

根据模型推理的步骤，构建模型推理类：

#### （1）构造函数

```c#
// @brief core类默认初始化方法
public Core() { }
// @brief core类参数输入初始化方法
// @param model_file 本地推理模型地址路径
// @param device_name 设备名称
public Core(string model, string device, string cache_dir = "")
{
    // 初始化推理核心
    core_ptr = NativeMethods.core_init(model, device, cache_dir);
}
```

在该方法中，主要是调用推理核心初始化方法，初始化推理核心，读取本地模型，将模型加载到设备、创建推理请求等模型推理步骤。

#### （2）设置模型输入形状

  ```c#
  // @brief 设置推理模型的输入节点的大小
  // @param input_node_name 输入节点名
  // @param input_size 输入形状大小数组
  public void set_input_sharp(string node_name, ulong[] input_size)
  {
      // 获取输入数组长度
      int length = input_size.Length;
      core_ptr = NativeMethods.set_input_sharp(core_ptr, node_name, ref input_size[0], length);
  }
  ```

OpenVINO<sup>TM</sup> 2022支持模型动态输入，读入模型可以不固定输入大小，在使用时固定模型的输入大小，并且可以随时修改输入形状。

#### （3）加载推理数据

```c#
// @brief 加载推理数据
// @param input_node_name 输入节点名
// @param input_data 输入数据数组
public void load_input_data(string node_name, float[] input_data)
{
    core_ptr = NativeMethods.load_input_data(core_ptr, node_name, ref input_data[0]);
}
// @brief 加载图片推理数据
// @param input_node_name 输入节点名
// @param image_data 图片矩阵
// @param image_size 图片矩阵长度
public void load_input_data(string node_name, byte[] image_data, ulong image_size, int type)
{
    core_ptr = NativeMethods.load_image_input_data(core_ptr, node_name, ref image_data[0], image_size, type);
}
```

加载推理数据主要包含图片数据和普通的矩阵数据，其中对于图片的预处理，也已经在C++中进行封装，保证了图片数据在传输中的稳定性。

#### （4）模型推理

```c#
// @brief 模型推理
public void infer()
{
    core_ptr = NativeMethods.core_infer(core_ptr);
}
```

#### （5）读取推理结果数据

    ```c#
    // @brief 读取推理结果数据
    // @param output_node_name 输出节点名
    // @param data_size 输出数据长度
    // @return 推理结果数组
    public T[] read_infer_result<T>(string node_name, int data_size)
    {
        // 获取设定类型
        string t = typeof(T).ToString();
        // 新建返回值数组
        T[] result = new T[data_size];
        if (t == "System.Int32")
        { // 读取数据类型为整形数据
            int[] inference_result = new int[data_size];
            NativeMethods.read_infer_result_I32(core_ptr, node_name, ref inference_result[0]);
            result = (T[])Convert.ChangeType(inference_result, typeof(T[]));
            return result;
        }
        else if (t == "System.Int64")
        {
            long[] inference_result = new long[data_size];
            NativeMethods.read_infer_result_I64(core_ptr, node_name, ref inference_result[0]);
            result = (T[])Convert.ChangeType(inference_result, typeof(T[]));
            return result;
        }
        else
        { // 读取数据类型为浮点型数据
            float[] inference_result = new float[data_size];
            NativeMethods.read_infer_result_F32(core_ptr, node_name, ref inference_result[0]);
            result = (T[])Convert.ChangeType(inference_result, typeof(T[]));
            return result;
        }
    }
    ```

在读取模型推理结果时，支持读取整形数据和浮点型数据，且需要知晓模型输出数据的大小，这就要求我们对自己所使用的模型有很好的把握。

#### （6）清除地址

```c#
// @brief 删除创建的地址
public void delet()
{
    NativeMethods.core_delet(core_ptr);
}
```

此处的清除地址需要调用封装方法地址删除方法实现，不可以直接删除C#中创建的IntPtr，这样会导致内存泄漏，影响程序性能。

通过上面的封装，比可以在C#平台下，调用Core类，间接调用OpenVINOTM推理套件部署自己的模型了。

## 2.4  编译Core类库

右击项目，点击生成/重新生成，出现如下图所示，表示编译成功。

![image-20230127200157035](https://s2.loli.net/2023/01/27/7WiVBypNmbSX94c.png)
