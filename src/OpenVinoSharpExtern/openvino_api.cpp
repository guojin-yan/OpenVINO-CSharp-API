// OpenVINO C++ dll code for C#
// ����Ŀ֧��ģ�͸�ʽ��
//   1. paddlepaddle�ɽ�ģ��(.pdmodel)
//   2. onnx�м��ʽ(.onnx)
//   3. OpenVINO��IR��ʽ(.xml)
// ��Ը���Ŀ��ʹ�ò������磺
//   1. PaddleClas ͼ�����ģ�� ��������ʶ�������(.pdmodel)��(.onnx)��(.xml)��ʽ��
//   2. Paddledetection Ŀ����ģ�� ����ʶ�������(.pdmodel)��ʽ��
// ONLY support batchsize = 1


#include"openvino_api.h"





// @brief ��ʼ��openvino���Ľṹ�壬��ȡ��������ģ�ͣ���ģ�ͼ��ص��豸����������������
// @note ��֧�ֵ�����ģ�͵ĸ�ʽΪ��(.pdmodel)��(.onnx)��(.xml)��ʽ
// @note ��֧���豸ѡ��AUTO���Զ�ѡ�񡢣�CPU������������GPU���Կ�
// @param w_model_dir ����ģ�ͱ��ص�ַ
// @param w_device �����豸����
// @param w_cache_dir ����·��
// @param return_ptr CoreStructָ��
// @return �쳣��־
void* core_init(const wchar_t* w_model_dir, const wchar_t* w_device,
    const wchar_t* w_cache_dir){
    //��ȡ�ӿ��������
    std::string model_dir = wchar_to_string(w_model_dir);// ����ģ�ͱ��ص�ַ
    std::string device = wchar_to_string(w_device);// �����豸����
    std::string cache_dir = wchar_to_string(w_cache_dir); // �����ַ
    // ��ʼ���������
    CoreStruct* openvino_core = new CoreStruct(); // ������������ָ��
    openvino_core->model_ptr = openvino_core->core.read_model(model_dir); // ��ȡ����ģ��
    //if (cache_dir != "") {
    //    openvino_core->core.set_property(ov::cache_dir(cache_dir)); // ���û���·��
    //}
    openvino_core->compiled_model = openvino_core->core.compile_model(openvino_core->model_ptr, device); // ��ģ�ͼ��ص��豸
    openvino_core->infer_request = openvino_core->compiled_model.create_infer_request(); // ������������
    return (void*)openvino_core;
}


// @brief Ϊ����tensor��������״������µ��ܴ�С����ǰһ������ȡ��֮ǰ������
// @param core_ptr �������ָ��
// @param w_node_name ����ڵ���
// @param input_shape ������״��������
// @param input_size ���������С
// @param return_ptr CoreStructָ��
// @return �쳣��־
void* set_input_sharp(void* core_ptr, const wchar_t* w_node_name, size_t* input_shape,
    int input_size) {
    // ��ȡ����ģ�͵�ַ
    CoreStruct* openvino_core = (CoreStruct*)core_ptr;
    std::string node_name = wchar_to_string(w_node_name);
    // ��ȡָ���ڵ��tensor
    ov::Tensor tensor = openvino_core->infer_request.get_tensor(node_name);
    ov::Shape shape(input_shape, input_shape + input_size);
    // ���ýڵ��������ݵ���״
    tensor.set_shape(shape);
    return (void*)openvino_core;
}


// @brief ��ͼƬ���ݼ��ص�tensor�е������ڴ���
// @param core_ptr �������ָ��
// @param w_node_name ����ڵ���
// @param image_data ����ͼƬ���ݾ���
// @param image_size ͼƬ���󳤶�
// @param return_ptr CoreStructָ��
// @return �쳣��־
void* load_image_input_data(void* core_ptr, const wchar_t* w_node_name, uchar* image_data,
    size_t image_size,int type) { 
    
    // ��ȡ����ģ�͵�ַ
    CoreStruct* openvino_core = (CoreStruct*)core_ptr;
    std::string node_name = wchar_to_string(w_node_name);
    // ��ȡ����ڵ�tensor
    ov::Tensor input_image_tensor = openvino_core->infer_request.get_tensor(node_name);
    int input_H = input_image_tensor.get_shape()[2]; //���"image"�ڵ��Height
    int input_W = input_image_tensor.get_shape()[3]; //���"image"�ڵ��Width

    // ������ͼƬ����Ԥ����
    cv::Mat input_image = data_to_mat(image_data, image_size); // ��ȡ����ͼƬ
    cv::Mat blob_image;
    cv::cvtColor(input_image, blob_image, cv::COLOR_BGR2RGB); // ��ͼƬͨ���� BGR תΪ RGB
    if (type == 0) {
        // ������ͼƬ����tensor����Ҫ���������
        cv::resize(blob_image, blob_image, cv::Size(input_W, input_H), 0, 0, cv::INTER_LINEAR);
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
    }
    else if (type == 1) {
        // ������ͼƬ����tensor����Ҫ���������
        cv::resize(blob_image, blob_image, cv::Size(input_W, input_H), 0, 0, cv::INTER_LINEAR);
        // ͼ�����ݹ�һ��
        std::vector<float> std_values{ 1.0 * 255, 1.0 * 255, 1.0 * 255 };
        std::vector<cv::Mat> rgb_channels(3);
        cv::split(blob_image, rgb_channels); // ����ͼƬ����ͨ��
        for (auto i = 0; i < rgb_channels.size(); i++) {
            //��ͨ�����˶�ÿһ��ͨ�����ݽ��й�һ������
            rgb_channels[i].convertTo(rgb_channels[i], CV_32FC1, 1.0 / std_values[i], 0);
        }
        cv::merge(rgb_channels, blob_image); // �ϲ�ͼƬ����ͨ��
    }
    else if (type == 2) {
        // ������ͼƬ����tensor����Ҫ���������
        cv::resize(blob_image, blob_image, cv::Size(input_W, input_H), 0, 0, cv::INTER_LINEAR);
        // ͼ�����ݹ�һ��
        std::vector<float> std_values{ 1.0, 1.0, 1.0 };
        std::vector<cv::Mat> rgb_channels(3);
        cv::split(blob_image, rgb_channels); // ����ͼƬ����ͨ��
        for (auto i = 0; i < rgb_channels.size(); i++) {
            //��ͨ�����˶�ÿһ��ͨ�����ݽ��й�һ������
            rgb_channels[i].convertTo(rgb_channels[i], CV_32FC1, 1.0 / std_values[i], 0);
        }
        cv::merge(rgb_channels, blob_image); // �ϲ�ͼƬ����ͨ��
    }
    else if (type == 3) {
        // ��ȡ����任��Ϣ
        cv::Point center(blob_image.cols / 2, blob_image.rows / 2); // �任����
        cv::Size input_size(blob_image.cols, blob_image.rows); // ����ߴ�
        int rot = 0; // �Ƕ�
        cv::Size output_size(input_W, input_H); // ����ߴ�

        // ��ȡ����任����
        cv::Mat warp_mat(2, 3, CV_32FC1);
        warp_mat = get_affine_transform(center, input_size, rot, output_size);
        // ����仯
        cv::warpAffine(blob_image, blob_image, warp_mat, output_size, cv::INTER_LINEAR);
        // ͼ�����ݹ�һ��
        std::vector<float> mean_values{ 0.485 * 255, 0.456 * 255, 0.406 * 255 };
        std::vector<float> std_values{ 0.229 * 255, 0.224 * 255, 0.225 * 255 };
        std::vector<cv::Mat> rgb_channels(3);
        cv::split(blob_image, rgb_channels); // ����ͼƬ����ͨ��
        for (auto i = 0; i < rgb_channels.size(); i++) {
            //��ͨ�����˶�ÿһ��ͨ�����ݽ��й�һ������
            rgb_channels[i].convertTo(rgb_channels[i], CV_32FC1, 1.0 / std_values[i], (0.0 - mean_values[i]) / std_values[i]);
        }
        cv::merge(rgb_channels, blob_image); // �ϲ�ͼƬ����ͨ��
    }
    else if (type == 4) {
        // ��ȡ����任��Ϣ
        cv::Point center(blob_image.cols / 2, blob_image.rows / 2); // �任����
        cv::Size input_size(blob_image.cols, blob_image.rows); // ����ߴ�
        int rot = 0; // �Ƕ�
        cv::Size output_size(input_W, input_H); // ����ߴ�

        // ��ȡ����任����
        cv::Mat warp_mat;
        warp_mat = get_affine_transform(center, input_size, rot, output_size);
        // ����仯
        cv::warpAffine(blob_image, blob_image, warp_mat, output_size, cv::INTER_LINEAR);
        // ͼ�����ݹ�һ��
        std::vector<float> std_values{ 1.0 * 255, 1.0 * 255, 1.0 * 255 };
        std::vector<cv::Mat> rgb_channels(3);
        cv::split(blob_image, rgb_channels); // ����ͼƬ����ͨ��
        for (auto i = 0; i < rgb_channels.size(); i++) {
            //��ͨ�����˶�ÿһ��ͨ�����ݽ��й�һ������
            rgb_channels[i].convertTo(rgb_channels[i], CV_32FC1, 1.0 / std_values[i], 0);
        }
        cv::merge(rgb_channels, blob_image); // �ϲ�ͼƬ����ͨ��
    }
    else if (type == 5) {
        // ��ȡ����任��Ϣ
        cv::Point center(blob_image.cols / 2, blob_image.rows / 2); // �任����
        cv::Size input_size(blob_image.cols, blob_image.rows); // ����ߴ�
        int rot = 0; // �Ƕ�
        cv::Size output_size(input_W, input_H); // ����ߴ�

        // ��ȡ����任����
        cv::Mat warp_mat;
        warp_mat = get_affine_transform(center, input_size, rot, output_size);
        // ����仯
        cv::warpAffine(blob_image, blob_image, warp_mat, output_size, cv::INTER_LINEAR);
        // ͼ�����ݹ�һ��
        std::vector<float> std_values{ 1.0, 1.0, 1.0 };
        std::vector<cv::Mat> rgb_channels(3);
        cv::split(blob_image, rgb_channels); // ����ͼƬ����ͨ��
        for (auto i = 0; i < rgb_channels.size(); i++) {
            //��ͨ�����˶�ÿһ��ͨ�����ݽ��й�һ������
            rgb_channels[i].convertTo(rgb_channels[i], CV_32FC1, 1.0 / std_values[i], 0);
        }
        cv::merge(rgb_channels, blob_image); // �ϲ�ͼƬ����ͨ��
    }
    // ��ͼƬ������䵽tensor�����ڴ���
    fill_tensor_data_image(input_image_tensor, blob_image);

    return (void*)openvino_core;
    
}

// @brief ���������ݼ��ص�tensor�е������ڴ���
// @param core_ptr �������ָ��
// @param w_node_name ����ڵ���
// @param input_data ������������
// @param [out] return_ptr CoreStructָ��
// @return �쳣��־
void* load_input_data(void* core_ptr, const wchar_t* w_node_name, float* input_data) { 
    // ��ȡ����ģ�͵�ַ
    CoreStruct* openvino_core = (CoreStruct*)core_ptr;
    std::string input_node_name = wchar_to_string(w_node_name);
    // ��ȡָ���ڵ�tensor
    ov::Tensor input_image_tensor = openvino_core->infer_request.get_tensor(input_node_name);
    std::vector<size_t> input_shape = input_image_tensor.get_shape(); //�������ڵ����״
    int input_size = std::accumulate(input_shape.begin(), input_shape.end(), 1, std::multiplies<int>()); // ��ȡ����
    // ��������䵽tensor�����ڴ���
    fill_tensor_data_float(input_image_tensor, input_data, input_size);
    return (void*)openvino_core;
}

// @brief �Լ��غõ�����ģ�ͽ�������
// @param core_ptr �������ָ��
// @param return_ptr CoreStructָ��
// @return �쳣��־
void* core_infer(void* core_ptr){
    // ��ȡ����ģ�͵�ַ
    CoreStruct* openvino_core = (CoreStruct*)core_ptr;
    // ģ��Ԥ��
    openvino_core->infer_request.infer();

    return (void*)openvino_core;
}

// @brief ��ѯfloat���͵�������
// @param core_ptr �������ָ��
// @param w_node_name ����ڵ���
// @param [out]  inference_result ����������
// @return �쳣��־
void read_infer_result_F32(void* core_ptr, const wchar_t* w_node_name, float* infer_result) {
    // ��ȡ����ģ�͵�ַ
    CoreStruct* openvino_core = (CoreStruct*)core_ptr;
    std::string output_node_name = wchar_to_string(w_node_name);
    // ��ȡָ���ڵ��tensor
    const ov::Tensor& output_tensor = openvino_core->infer_request.get_tensor(output_node_name);
    std::vector<size_t> input_shape = output_tensor.get_shape(); //�������ڵ����״
    int output_size = std::accumulate(input_shape.begin(), input_shape.end(), 1, std::multiplies<int>()); // ��ȡ����
    // ��ȡ����ڵ����ݵ�ַ
    const float* results = output_tensor.data<const float>();
    // �����������Ƶ������ַָ����
    for (int i = 0; i < output_size; i++) {
        *infer_result = results[i];
        infer_result++;
    }
}

// @brief ��ѯint���͵�������
// @param core_ptr �������ָ��
// @param w_node_name ����ڵ���
// @param [out]  inference_result ����������
// @return �쳣��־
void read_infer_result_I32(void* core_ptr, const wchar_t* w_node_name, int* infer_result) {
    // ��ȡ����ģ�͵�ַ
    CoreStruct* openvino_core = (CoreStruct*)core_ptr;
    std::string output_node_name = wchar_to_string(w_node_name);
    // ��ȡָ���ڵ��tensor
    const ov::Tensor& output_tensor = openvino_core->infer_request.get_tensor(output_node_name);
    std::vector<size_t> input_shape = output_tensor.get_shape(); //�������ڵ����״
    int output_size = std::accumulate(input_shape.begin(), input_shape.end(), 1, std::multiplies<int>()); // ��ȡ����
    // ��ȡ����ڵ����ݵ�ַ
    const int* results = output_tensor.data<const int>();
    // ����������ֵ�������ַָ����
    for (int i = 0; i < output_size; i++) {
        *infer_result = results[i];
        infer_result++;
    }
}

// @brief ��ѯlong long���͵�������
// @param core_ptr �������ָ��
// @param w_node_name ����ڵ���
// @param [out]  inference_result ����������
// @return �쳣��־
void read_infer_result_I64(void* core_ptr, const wchar_t* w_node_name, long long* infer_result) {
    // ��ȡ����ģ�͵�ַ
    CoreStruct* openvino_core = (CoreStruct*)core_ptr;
    std::string output_node_name = wchar_to_string(w_node_name);
    // ��ȡָ���ڵ��tensor
    const ov::Tensor& output_tensor = openvino_core->infer_request.get_tensor(output_node_name);
    std::vector<size_t> input_shape = output_tensor.get_shape(); //�������ڵ����״
    int output_size = std::accumulate(input_shape.begin(), input_shape.end(), 1, std::multiplies<int>()); // ��ȡ����
    // ��ȡ����ڵ����ݵ�ַ
    const long long* results = output_tensor.data<const long long>();
    // ����������ֵ�������ַָ����
    for (int i = 0; i < output_size; i++) {
        *infer_result = results[i];
        infer_result++;
    }
}


// @brief ɾ��������Ľṹ��ָ�룬�ͷ�ռ���ڴ�
// @param core_ptr �������ָ��
// @return �쳣��־
void core_delet(void* core_ptr) {
    //��ȡ����ģ�͵�ַ
    CoreStruct* openvino_core = (CoreStruct*)core_ptr;
    // ɾ��ռ���ڴ�
    delete openvino_core;
}
