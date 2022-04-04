// OpenVINO C++ dll code for C#
// ����Ŀ֧��ģ�͸�ʽ��
//   1. paddlepaddle�ɽ�ģ��(.pdmodel)
//   2. onnx�м��ʽ(.onnx)
//   3. OpenVINO��IR��ʽ(.xml)
// ��Ը���Ŀ��ʹ�ò������磺
//   1. PaddleClas ͼ�����ģ�� ��������ʶ�������(.pdmodel)��(.onnx)��(.xml)��ʽ��
//   2. Paddledetection Ŀ����ģ�� ����ʶ�������(.pdmodel)��ʽ��
// ONLY support batchsize = 1


#include<time.h>

#include<iostream>
#include<map>
#include<string>
#include<vector>

#include "openvino/openvino.hpp"
#include "opencv2/opencv.hpp"

#include<windows.h>


// @brief ��wchar_t*�ַ���ָ��ת��Ϊstring�ַ�����ʽ
// @param wchar �����ַ�ָ��
// @return ת������string�ַ��� 
std::string wchar_to_string(const wchar_t* wchar) {
    // ��ȡ����ָ��ĳ���
    int path_size = WideCharToMultiByte(CP_OEMCP, 0, wchar, wcslen(wchar), NULL, 0, NULL, NULL);
    char* chars = new char[path_size + 1];
    // ��˫�ֽ��ַ���ת���ɵ��ֽ��ַ���
    WideCharToMultiByte(CP_OEMCP, 0, wchar, wcslen(wchar), chars, path_size, NULL, NULL);
    chars[path_size] = '\0';
    std::string pattern = chars;
    delete chars; //�ͷ��ڴ�
    return pattern;
}

// @brief ��ͼƬ�ľ�������ת��Ϊopencv��mat����
// @param data ͼƬ����
// @param size ͼƬ���󳤶�
// @return ת�����mat����
cv::Mat data_to_mat(uchar* data, size_t size) {
    //��ͼƬ�������ݶ�ȡ��������
    std::vector<uchar> buf;
    for (int i = 0; i < size; i++) {
        buf.push_back(*data);
        data++;
    }
    // ����ͼƬ���룬�������е�����ת��Ϊmat����
    return cv::imdecode(cv::Mat(buf), 1);
}

// @brief �����������ΪͼƬ���ݵĽڵ���и�ֵ��ʵ��ͼƬ������������
// @param input_tensor ����ڵ��tensor
// @param inpt_image ����ͼƬ����
void fill_tensor_data_image(ov::Tensor& input_tensor, const cv::Mat& input_image) {
    // ��ȡ����ڵ�Ҫ�������ͼƬ���ݵĴ�С
    ov::Shape tensor_shape = input_tensor.get_shape();
    const size_t width = tensor_shape[3]; // Ҫ������ͼƬ���ݵĿ��
    const size_t height = tensor_shape[2]; // Ҫ������ͼƬ���ݵĸ߶�
    const size_t channels = tensor_shape[1]; // Ҫ������ͼƬ���ݵ�ά��
    // ��ȡ�ڵ������ڴ�ָ��
    float* input_tensor_data = input_tensor.data<float>();
    // ��ͼƬ������䵽������
    // ԭ��ͼƬ����Ϊ H��W��C ��ʽ������Ҫ���Ϊ C��H��W ��ʽ
    for (size_t c = 0; c < channels; c++) {
        for (size_t h = 0; h < height; h++) {
            for (size_t w = 0; w < width; w++) {
                input_tensor_data[c * width * height + h * width + w] = input_image.at<cv::Vec<float, 3>>(h, w)[c];
            }
        }
    }
}
// @brief �����������Ϊfkloat���ݵĽڵ���и�ֵ��ʵ��float������������
// @param input_tensor ����ڵ��tensor
// @param input_data ������������
// @param data_size �������鳤��
void fill_tensor_data_float(ov::Tensor& input_tensor, float* input_data, int data_size) {
    // ��ȡ�ڵ������ڴ�ָ��
    float* input_tensor_data = input_tensor.data<float>();
    // ��ͼƬ������䵽������
    for (int i = 0; i < data_size; i++) {
        input_tensor_data[i] = input_data[i];
    }
}



// @brief ������Ľṹ��
typedef struct openvino_infer_engine {
    ov::Core core; // core����
    std::shared_ptr<ov::Model> model_ptr; // ��ȡģ��ָ��
    ov::CompiledModel compiled_model; // ģ�ͼ��ص��豸����
    ov::InferRequest infer_request; // �����������
} InferEngineStruct;


// @brief ��ʼ��openvino���Ľṹ�壬��ȡ��������ģ�ͣ���ģ�ͼ��ص��豸����������������
// @note ��֧�ֵ�����ģ�͵ĸ�ʽΪ��(.pdmodel)��(.onnx)��(.xml)��ʽ
// @note ��֧���豸ѡ��AUTO���Զ�ѡ�񡢣�CPU������������GPU���Կ�
// @param model_file_wchar ����ģ�ͱ��ص�ַ
// @param device_name_wchar �����豸����
// @return ������Ľṹ��ָ��
extern "C" __declspec(dllexport) void* __stdcall inference_engine_init(const wchar_t* model_file_wchar, const wchar_t* device_name_wchar) {

    //��ȡ�ӿ��������
    std::string model_file_path = wchar_to_string(model_file_wchar);// ����ģ�ͱ��ص�ַ
    std::string device_name = wchar_to_string(device_name_wchar);// �����豸����
    // ��ʼ���������
    InferEngineStruct* p = new InferEngineStruct(); // ������������ָ��
    p->model_ptr = p->core.read_model(model_file_path); // ��ȡ����ģ��
    p->compiled_model = p->core.compile_model(p->model_ptr, "CPU"); // ��ģ�ͼ��ص��豸
    p->infer_request = p->compiled_model.create_infer_request(); // ������������

    return (void*)p;
}


// @brief Ϊ����ΪͼƬ���ݵ�tensor��������״������µ��ܴ�С����ǰһ������ȡ��֮ǰ������
// @param inference_engine �������ָ��
// @param input_node_name_wchar ����ڵ���
// @param input_size ������״��������
// @return ������Ľṹ��ָ��
extern "C"  __declspec(dllexport) void* __stdcall set_input_image_sharp(void* inference_engine, const wchar_t* input_node_name_wchar, size_t * input_size) {
    // ��ȡ����ģ�͵�ַ
    InferEngineStruct* p = (InferEngineStruct*)inference_engine;
    std::string input_node_name = wchar_to_string(input_node_name_wchar);
    // ��ȡָ���ڵ��tensor
    ov::Tensor input_image_tensor = p->infer_request.get_tensor(input_node_name);
    // ���ýڵ��������ݵ���״
    input_image_tensor.set_shape({ input_size[0],input_size[1],input_size[2],input_size[3] });
    return (void*)p;
}

// @brief Ϊ����Ϊfloat���ݵ�tensor��������״������µ��ܴ�С����ǰһ������ȡ��֮ǰ������
// @param inference_engine �������ָ��
// @param input_node_name_wchar ����ڵ���
// @param input_size ������״��������
// @return ������Ľṹ��ָ��
extern "C"  __declspec(dllexport) void* __stdcall set_input_data_sharp(void* inference_engine, const wchar_t* input_node_name_wchar, size_t * input_size) {
    // ��ȡ����ģ�͵�ַ
    InferEngineStruct* p = (InferEngineStruct*)inference_engine;
    std::string input_node_name = wchar_to_string(input_node_name_wchar);
    // ��ȡָ���ڵ��tensor
    ov::Tensor input_image_tensor = p->infer_request.get_tensor(input_node_name);
    // �����������ݳ���
    input_image_tensor.set_shape({ input_size[0] , input_size[1] });
    return (void*)p;
}

// @brief ��ͼƬ���ݼ��ص�tensor�е������ڴ���
// @param inference_engine �������ָ��
// @param input_node_name_wchar ����ڵ���
// @param image_data ����ͼƬ���ݾ���
// @param image_size ͼƬ���󳤶�
// @return ������Ľṹ��ָ��
extern "C"  __declspec(dllexport) void* __stdcall load_image_input_data(void* inference_engine, const wchar_t* input_node_name_wchar, uchar * image_data, size_t image_size) {
    // ��ȡ����ģ�͵�ַ
    InferEngineStruct* p = (InferEngineStruct*)inference_engine;
    std::string input_node_name = wchar_to_string(input_node_name_wchar);
    // ��ȡ����ڵ�tensor
    ov::Tensor input_image_tensor = p->infer_request.get_tensor(input_node_name);
    int input_H = input_image_tensor.get_shape()[2]; //���"image"�ڵ��Height
    int input_W = input_image_tensor.get_shape()[3]; //���"image"�ڵ��Width

    // ������ͼƬ����Ԥ����
    cv::Mat input_image = data_to_mat(image_data, image_size); // ��ȡ����ͼƬ
    cv::Mat blob_image;
    cv::cvtColor(input_image, blob_image, cv::COLOR_BGR2RGB); // ��ͼƬͨ���� BGR תΪ RGB
    // ������ͼƬ����tensor����Ҫ���������
    cv::resize(blob_image, blob_image, cv::Size(input_H, input_W), 0, 0, cv::INTER_LINEAR);
    // ͼ�����ݹ�һ��������ֵmean�����Է���std
    // PaddleDetectionģ��ʹ��imagenet���ݼ��ľ�ֵ Mean = [0.485, 0.456, 0.406]�ͷ��� std = [0.229, 0.224, 0.225]
    std::vector<float> mean_values{ 0.485 * 255, 0.456 * 255, 0.406 * 255 };
    std::vector<float> std_values{ 0.229 * 255, 0.224 * 255, 0.225 * 255 };
    std::vector<cv::Mat> rgb_channels(3);
    cv::split(blob_image, rgb_channels); // ����ͼƬ����ͨ��
    for (auto i = 0; i < rgb_channels.size(); i++) {
        //��ͨ�����˶�ÿһ��ͨ�����ݽ��й�һ������
        rgb_channels[i].convertTo(rgb_channels[i], CV_32FC1, 1.0 / std_values[i], (0.0 - mean_values[i]) / std_values[i]);
    }
    cv::merge(rgb_channels, blob_image); // �ϲ�ͼƬ����ͨ��
    // ��ͼƬ������䵽tensor�����ڴ���
    fill_tensor_data_image(input_image_tensor, blob_image);

    return (void*)p;
}
// @brief ���������ݼ��ص�tensor�е������ڴ���
// @param inference_engine �������ָ��
// @param input_node_name_wchar ����ڵ���
// @param input_data ������������
// @return ������Ľṹ��ָ��
extern "C"  __declspec(dllexport) void* __stdcall load_input_data(void* inference_engine, const wchar_t* input_node_name_wchar, float* input_data) {
    // ��ȡ����ģ�͵�ַ
    InferEngineStruct* p = (InferEngineStruct*)inference_engine;
    std::string input_node_name = wchar_to_string(input_node_name_wchar);
    // ��ȡָ���ڵ�tensor
    ov::Tensor input_image_tensor = p->infer_request.get_tensor(input_node_name);
    int input_size = input_image_tensor.get_shape()[1]; //�������ڵ�ĳ���
    // ��������䵽tensor�����ڴ���
    fill_tensor_data_float(input_image_tensor, input_data, input_size);

    return (void*)p;
}

// @brief �Լ��غõ�����ģ�ͽ�������
// @param inference_engine �������ָ��
// @return ������Ľṹ��ָ��
extern "C"  __declspec(dllexport) void* __stdcall inference_engine_infer(void* inference_engine) {
    // ��ȡ����ģ�͵�ַ
    InferEngineStruct* p = (InferEngineStruct*)inference_engine;
    // ģ��Ԥ��
    p->infer_request.infer();

    return (void*)p;
}

// @brief ��ѯfloat���͵�������
// @param inference_engine �������ָ��
// @param output_node_name_wchar ����ڵ���
// @param data_size ���ݳ���
// @param [out]  inference_result ����������
extern "C"  __declspec(dllexport) void __stdcall read_inference_result_F32(void* inference_engine, const wchar_t* output_node_name_wchar, int data_size, float* inference_result) {
    // ��ȡ����ģ�͵�ַ
    InferEngineStruct* p = (InferEngineStruct*)inference_engine;
    std::string output_node_name = wchar_to_string(output_node_name_wchar);
    // ��ȡָ���ڵ��tensor
    const ov::Tensor& output_tensor = p->infer_request.get_tensor(output_node_name);
    // ��ȡ����ڵ����ݵ�ַ
    const float* results = output_tensor.data<const float>();
    // �����������Ƶ������ַָ����
    for (int i = 0; i < data_size; i++) {
        *inference_result = results[i];
        inference_result++;
    }
}
// @brief ��ѯint���͵�������
// @param inference_engine �������ָ��
// @param output_node_name_wchar ����ڵ���
// @param data_size ���ݳ���
// @param [out]  inference_result ����������
extern "C"  __declspec(dllexport) void __stdcall read_inference_result_I32(void* inference_engine, const wchar_t* output_node_name_wchar, int data_size, int* inference_result) {
    // ��ȡ����ģ�͵�ַ
    InferEngineStruct* p = (InferEngineStruct*)inference_engine;
    std::string output_node_name = wchar_to_string(output_node_name_wchar);
    // ��ȡָ���ڵ��tensor
    const ov::Tensor& output_tensor = p->infer_request.get_tensor(output_node_name);
    // ��ȡ����ڵ����ݵ�ַ
    const int* results = output_tensor.data<const int>();
    // ����������ֵ�������ַָ����
    for (int i = 0; i < data_size; i++) {
        *inference_result = results[i];
        inference_result++;
    }
}

// @brief ɾ��������Ľṹ��ָ�룬�ͷ�ռ���ڴ�
// @param inference_engine �������ָ��
extern "C"  __declspec(dllexport) void __stdcall inference_engine_delet(void* inference_engine) {
    //��ȡ����ģ�͵�ַ
    InferEngineStruct* p = (InferEngineStruct*)inference_engine;
    // ɾ��ռ���ڴ�
    delete p;
}









