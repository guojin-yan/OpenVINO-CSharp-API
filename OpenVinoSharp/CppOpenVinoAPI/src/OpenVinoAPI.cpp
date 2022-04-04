// OpenVINO C++ dll code for C#
// 该项目支持模型格式：
//   1. paddlepaddle飞桨模型(.pdmodel)
//   2. onnx中间格式(.onnx)
//   3. OpenVINO的IR格式(.xml)
// 针对该项目所使用测试网络：
//   1. PaddleClas 图像分类模型 花卉种类识别网络的(.pdmodel)、(.onnx)、(.xml)格式；
//   2. Paddledetection 目标检测模型 车辆识别网络的(.pdmodel)格式；
// ONLY support batchsize = 1


#include<time.h>

#include<iostream>
#include<map>
#include<string>
#include<vector>

#include "openvino/openvino.hpp"
#include "opencv2/opencv.hpp"

#include<windows.h>


// @brief 将wchar_t*字符串指针转换为string字符串格式
// @param wchar 输入字符指针
// @return 转换出的string字符串 
std::string wchar_to_string(const wchar_t* wchar) {
    // 获取输入指针的长度
    int path_size = WideCharToMultiByte(CP_OEMCP, 0, wchar, wcslen(wchar), NULL, 0, NULL, NULL);
    char* chars = new char[path_size + 1];
    // 将双字节字符串转换成单字节字符串
    WideCharToMultiByte(CP_OEMCP, 0, wchar, wcslen(wchar), chars, path_size, NULL, NULL);
    chars[path_size] = '\0';
    std::string pattern = chars;
    delete chars; //释放内存
    return pattern;
}

// @brief 将图片的矩阵数据转换为opencv的mat数据
// @param data 图片矩阵
// @param size 图片矩阵长度
// @return 转换后的mat数据
cv::Mat data_to_mat(uchar* data, size_t size) {
    //将图片数组数据读取到容器中
    std::vector<uchar> buf;
    for (int i = 0; i < size; i++) {
        buf.push_back(*data);
        data++;
    }
    // 利用图片解码，将容器中的数据转换为mat类型
    return cv::imdecode(cv::Mat(buf), 1);
}

// @brief 对网络的输入为图片数据的节点进行赋值，实现图片数据输入网络
// @param input_tensor 输入节点的tensor
// @param inpt_image 输入图片数据
void fill_tensor_data_image(ov::Tensor& input_tensor, const cv::Mat& input_image) {
    // 获取输入节点要求的输入图片数据的大小
    ov::Shape tensor_shape = input_tensor.get_shape();
    const size_t width = tensor_shape[3]; // 要求输入图片数据的宽度
    const size_t height = tensor_shape[2]; // 要求输入图片数据的高度
    const size_t channels = tensor_shape[1]; // 要求输入图片数据的维度
    // 读取节点数据内存指针
    float* input_tensor_data = input_tensor.data<float>();
    // 将图片数据填充到网络中
    // 原有图片数据为 H、W、C 格式，输入要求的为 C、H、W 格式
    for (size_t c = 0; c < channels; c++) {
        for (size_t h = 0; h < height; h++) {
            for (size_t w = 0; w < width; w++) {
                input_tensor_data[c * width * height + h * width + w] = input_image.at<cv::Vec<float, 3>>(h, w)[c];
            }
        }
    }
}
// @brief 对网络的输入为fkloat数据的节点进行赋值，实现float数据输入网络
// @param input_tensor 输入节点的tensor
// @param input_data 输入数据数组
// @param data_size 输入数组长度
void fill_tensor_data_float(ov::Tensor& input_tensor, float* input_data, int data_size) {
    // 读取节点数据内存指针
    float* input_tensor_data = input_tensor.data<float>();
    // 将图片数据填充到网络中
    for (int i = 0; i < data_size; i++) {
        input_tensor_data[i] = input_data[i];
    }
}



// @brief 推理核心结构体
typedef struct openvino_infer_engine {
    ov::Core core; // core对象
    std::shared_ptr<ov::Model> model_ptr; // 读取模型指针
    ov::CompiledModel compiled_model; // 模型加载到设备对象
    ov::InferRequest infer_request; // 推理请求对象
} InferEngineStruct;


// @brief 初始化openvino核心结构体，读取本地推理模型，将模型加载到设备，并创建推理请求
// @note 可支持的推理模型的格式为：(.pdmodel)、(.onnx)、(.xml)格式
// @note 可支持设备选择（AUTO）自动选择、（CPU）处理器、（GPU）显卡
// @param model_file_wchar 推理模型本地地址
// @param device_name_wchar 加载设备名称
// @return 推理核心结构体指针
extern "C" __declspec(dllexport) void* __stdcall inference_engine_init(const wchar_t* model_file_wchar, const wchar_t* device_name_wchar) {

    //读取接口输入参数
    std::string model_file_path = wchar_to_string(model_file_wchar);// 推理模型本地地址
    std::string device_name = wchar_to_string(device_name_wchar);// 加载设备名称
    // 初始化推理核心
    InferEngineStruct* p = new InferEngineStruct(); // 创建推理引擎指针
    p->model_ptr = p->core.read_model(model_file_path); // 读取推理模型
    p->compiled_model = p->core.compile_model(p->model_ptr, "CPU"); // 将模型加载到设备
    p->infer_request = p->compiled_model.create_infer_request(); // 创建推理请求

    return (void*)p;
}


// @brief 为输入为图片数据的tensor设置新形状，如果新的总大小大于前一个，则取消之前的设置
// @param inference_engine 推理核心指针
// @param input_node_name_wchar 输入节点名
// @param input_size 输入形状数据数组
// @return 推理核心结构体指针
extern "C"  __declspec(dllexport) void* __stdcall set_input_image_sharp(void* inference_engine, const wchar_t* input_node_name_wchar, size_t * input_size) {
    // 读取推理模型地址
    InferEngineStruct* p = (InferEngineStruct*)inference_engine;
    std::string input_node_name = wchar_to_string(input_node_name_wchar);
    // 获取指定节点的tensor
    ov::Tensor input_image_tensor = p->infer_request.get_tensor(input_node_name);
    // 设置节点输入数据的形状
    input_image_tensor.set_shape({ input_size[0],input_size[1],input_size[2],input_size[3] });
    return (void*)p;
}

// @brief 为输入为float数据的tensor设置新形状，如果新的总大小大于前一个，则取消之前的设置
// @param inference_engine 推理核心指针
// @param input_node_name_wchar 输入节点名
// @param input_size 输入形状数据数组
// @return 推理核心结构体指针
extern "C"  __declspec(dllexport) void* __stdcall set_input_data_sharp(void* inference_engine, const wchar_t* input_node_name_wchar, size_t * input_size) {
    // 读取推理模型地址
    InferEngineStruct* p = (InferEngineStruct*)inference_engine;
    std::string input_node_name = wchar_to_string(input_node_name_wchar);
    // 获取指定节点的tensor
    ov::Tensor input_image_tensor = p->infer_request.get_tensor(input_node_name);
    // 重新设置数据长度
    input_image_tensor.set_shape({ input_size[0] , input_size[1] });
    return (void*)p;
}

// @brief 将图片数据加载到tensor中的数据内存上
// @param inference_engine 推理核心指针
// @param input_node_name_wchar 输入节点名
// @param image_data 输入图片数据矩阵
// @param image_size 图片矩阵长度
// @return 推理核心结构体指针
extern "C"  __declspec(dllexport) void* __stdcall load_image_input_data(void* inference_engine, const wchar_t* input_node_name_wchar, uchar * image_data, size_t image_size) {
    // 读取推理模型地址
    InferEngineStruct* p = (InferEngineStruct*)inference_engine;
    std::string input_node_name = wchar_to_string(input_node_name_wchar);
    // 获取输入节点tensor
    ov::Tensor input_image_tensor = p->infer_request.get_tensor(input_node_name);
    int input_H = input_image_tensor.get_shape()[2]; //获得"image"节点的Height
    int input_W = input_image_tensor.get_shape()[3]; //获得"image"节点的Width

    // 对输入图片进行预处理
    cv::Mat input_image = data_to_mat(image_data, image_size); // 读取输入图片
    cv::Mat blob_image;
    cv::cvtColor(input_image, blob_image, cv::COLOR_BGR2RGB); // 将图片通道由 BGR 转为 RGB
    // 对输入图片按照tensor输入要求进行缩放
    cv::resize(blob_image, blob_image, cv::Size(input_H, input_W), 0, 0, cv::INTER_LINEAR);
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
    // 将图片数据填充到tensor数据内存中
    fill_tensor_data_image(input_image_tensor, blob_image);

    return (void*)p;
}
// @brief 将其他数据加载到tensor中的数据内存上
// @param inference_engine 推理核心指针
// @param input_node_name_wchar 输入节点名
// @param input_data 输入数据数组
// @return 推理核心结构体指针
extern "C"  __declspec(dllexport) void* __stdcall load_input_data(void* inference_engine, const wchar_t* input_node_name_wchar, float* input_data) {
    // 读取推理模型地址
    InferEngineStruct* p = (InferEngineStruct*)inference_engine;
    std::string input_node_name = wchar_to_string(input_node_name_wchar);
    // 读取指定节点tensor
    ov::Tensor input_image_tensor = p->infer_request.get_tensor(input_node_name);
    int input_size = input_image_tensor.get_shape()[1]; //获得输入节点的长度
    // 将数据填充到tensor数据内存上
    fill_tensor_data_float(input_image_tensor, input_data, input_size);

    return (void*)p;
}

// @brief 对加载好的推理模型进行推理
// @param inference_engine 推理核心指针
// @return 推理核心结构体指针
extern "C"  __declspec(dllexport) void* __stdcall inference_engine_infer(void* inference_engine) {
    // 读取推理模型地址
    InferEngineStruct* p = (InferEngineStruct*)inference_engine;
    // 模型预测
    p->infer_request.infer();

    return (void*)p;
}

// @brief 查询float类型的推理结果
// @param inference_engine 推理核心指针
// @param output_node_name_wchar 输出节点名
// @param data_size 数据长度
// @param [out]  inference_result 推理结果数组
extern "C"  __declspec(dllexport) void __stdcall read_inference_result_F32(void* inference_engine, const wchar_t* output_node_name_wchar, int data_size, float* inference_result) {
    // 读取推理模型地址
    InferEngineStruct* p = (InferEngineStruct*)inference_engine;
    std::string output_node_name = wchar_to_string(output_node_name_wchar);
    // 读取指定节点的tensor
    const ov::Tensor& output_tensor = p->infer_request.get_tensor(output_node_name);
    // 获取网络节点数据地址
    const float* results = output_tensor.data<const float>();
    // 将输出结果复制到输出地址指针中
    for (int i = 0; i < data_size; i++) {
        *inference_result = results[i];
        inference_result++;
    }
}
// @brief 查询int类型的推理结果
// @param inference_engine 推理核心指针
// @param output_node_name_wchar 输出节点名
// @param data_size 数据长度
// @param [out]  inference_result 推理结果数组
extern "C"  __declspec(dllexport) void __stdcall read_inference_result_I32(void* inference_engine, const wchar_t* output_node_name_wchar, int data_size, int* inference_result) {
    // 读取推理模型地址
    InferEngineStruct* p = (InferEngineStruct*)inference_engine;
    std::string output_node_name = wchar_to_string(output_node_name_wchar);
    // 读取指定节点的tensor
    const ov::Tensor& output_tensor = p->infer_request.get_tensor(output_node_name);
    // 获取网络节点数据地址
    const int* results = output_tensor.data<const int>();
    // 将输出结果赋值到输出地址指针中
    for (int i = 0; i < data_size; i++) {
        *inference_result = results[i];
        inference_result++;
    }
}

// @brief 删除推理核心结构体指针，释放占用内存
// @param inference_engine 推理核心指针
extern "C"  __declspec(dllexport) void __stdcall inference_engine_delet(void* inference_engine) {
    //读取推理模型地址
    InferEngineStruct* p = (InferEngineStruct*)inference_engine;
    // 删除占用内存
    delete p;
}









