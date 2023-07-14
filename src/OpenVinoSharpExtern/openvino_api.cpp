// OpenVINO C++ dll code for C#

#include"openvino_api.h"





// @brief Initialize the openvino core structure, read the local inference model,
//         load the model onto the device, and create inference requests.
// @note The format of the supported inference model is (.pdmodel)、(.onnx)、(.xml).
// @param w_model_dir The model path.
// @param w_device The device name.
// @param w_cache_dir The cache Path.
// @return The CoreStruct pointer.
void* core_init(const wchar_t* w_model_dir, const wchar_t* w_device,
    const wchar_t* w_cache_dir){
    std::string model_dir = wchar_to_string(w_model_dir);// Get string format model path.
    std::string device = wchar_to_string(w_device);// Get string format device name.
    std::string cache_dir = wchar_to_string(w_cache_dir); // Get string format cache path.
    // Initialize CoreStruct.
    CoreStruct* openvino_core = new CoreStruct(); 
    openvino_core->model_ptr = openvino_core->core.read_model(model_dir); // Read inference model.
    //if (cache_dir != "") {
    //    openvino_core->core.set_property(ov::cache_dir(cache_dir)); // Set cache path.
    //}
    openvino_core->compiled_model = openvino_core->core.compile_model(openvino_core->model_ptr, device); // Get compile model.
    openvino_core->infer_request = openvino_core->compiled_model.create_infer_request(); // Create inference request.
    return (void*)openvino_core;
}


// @brief Set a new shape for the input tensor. If the new total size is greater
//        than the previous one, cancel the previous setting.
// @param core_ptr The CoreStruct pointer.
// @param w_node_name The input node name.
// @param input_shape The input shape array.
// @param input_size The length of array.
// @return The CoreStruct pointer.
void* set_input_sharp(void* core_ptr, const wchar_t* w_node_name, size_t* input_shape,
    int input_size) {
    CoreStruct* openvino_core = (CoreStruct*)core_ptr;
    std::string node_name = wchar_to_string(w_node_name);
    // Gets an input tensor for inference by tensor name.
    ov::Tensor tensor = openvino_core->infer_request.get_tensor(node_name);
    ov::Shape shape(input_shape, input_shape + input_size);
    // Set new shape for tensor, deallocate/allocate if new total size is bigger than previous one.
    tensor.set_shape(shape);
    return (void*)openvino_core;
}


// @brief Load image data into data in Tensor.
// @param core_ptr The CoreStruct pointer.
// @param w_node_name The input node name.
// @param image_data The input imaage data.
// @param image_size The length of image data.
// @return The CoreStruct pointer
void* load_image_input_data(void* core_ptr, const wchar_t* w_node_name, uchar* image_data,
    size_t image_size,int type) { 

    CoreStruct* openvino_core = (CoreStruct*)core_ptr;
    std::string node_name = wchar_to_string(w_node_name);
    // Gets an input tensor for inference by tensor name.
    ov::Tensor input_image_tensor = openvino_core->infer_request.get_tensor(node_name);
    int input_H = input_image_tensor.get_shape()[2]; // Get input height.
    int input_W = input_image_tensor.get_shape()[3]; // Get input width.

    // Preprocess input images
    cv::Mat input_image = data_to_mat(image_data, image_size); // Get image data from array.
    cv::Mat blob_image;
    cv::cvtColor(input_image, blob_image, cv::COLOR_BGR2RGB); // Convert the image channel from BGR to RGB.
    if (type == 0) {
        // Scale the input image according to Tensor input requirements
        cv::resize(blob_image, blob_image, cv::Size(input_W, input_H), 0, 0, cv::INTER_LINEAR);
        // Normalize image data, subtract mean, divide by variance std.
        // The PaddleDetection model uses the mean of the imagenet dataset.
        // Mean = [0.485, 0.456, 0.406] std = [0.229, 0.224, 0.225].
        std::vector<float> mean_values{ 0.485 * 255, 0.456 * 255, 0.406 * 255 };
        std::vector<float> std_values{ 0.229 * 255, 0.224 * 255, 0.225 * 255 };
        std::vector<cv::Mat> rgb_channels(3);
        cv::split(blob_image, rgb_channels); // Merge image data channels.
        for (auto i = 0; i < rgb_channels.size(); i++) {
            // Normalize the data of each channel based on this.
            rgb_channels[i].convertTo(rgb_channels[i], CV_32FC1, 1.0 / std_values[i], (0.0 - mean_values[i]) / std_values[i]);
        }
        cv::merge(rgb_channels, blob_image); // Merge image data channels
    }
    else if (type == 1) {
        // Scale the input image according to Tensor input requirements.
        cv::resize(blob_image, blob_image, cv::Size(input_W, input_H), 0, 0, cv::INTER_LINEAR);
        // 图像数据归一化
        std::vector<float> std_values{ 1.0 * 255, 1.0 * 255, 1.0 * 255 };
        std::vector<cv::Mat> rgb_channels(3);
        cv::split(blob_image, rgb_channels); // Merge image data channels.
        for (auto i = 0; i < rgb_channels.size(); i++) {
            // Normalize the data of each channel based on this.
            rgb_channels[i].convertTo(rgb_channels[i], CV_32FC1, 1.0 / std_values[i], 0);
        }
        cv::merge(rgb_channels, blob_image);  // Merge image data channels.
    }
    else if (type == 2) {
        // Scale the input image according to Tensor input requirements.
        cv::resize(blob_image, blob_image, cv::Size(input_W, input_H), 0, 0, cv::INTER_LINEAR);
        // Normalization of image data
        std::vector<float> std_values{ 1.0, 1.0, 1.0 };
        std::vector<cv::Mat> rgb_channels(3);
        cv::split(blob_image, rgb_channels); // Merge image data channels.
        for (auto i = 0; i < rgb_channels.size(); i++) {
            // Normalize the data of each channel based on this.
            rgb_channels[i].convertTo(rgb_channels[i], CV_32FC1, 1.0 / std_values[i], 0);
        }
        cv::merge(rgb_channels, blob_image);  // Merge image data channels.
    }
    else if (type == 3) {
        // Get Affine transformation Information.
        cv::Point center(blob_image.cols / 2, blob_image.rows / 2); 
        cv::Size input_size(blob_image.cols, blob_image.rows); 
        int rot = 0; 
        cv::Size output_size(input_W, input_H); 

        cv::Mat warp_mat(2, 3, CV_32FC1);
        warp_mat = get_affine_transform(center, input_size, rot, output_size);
        // affine transformation
        cv::warpAffine(blob_image, blob_image, warp_mat, output_size, cv::INTER_LINEAR);
        // 图像数据归一化
        std::vector<float> mean_values{ 0.485 * 255, 0.456 * 255, 0.406 * 255 };
        std::vector<float> std_values{ 0.229 * 255, 0.224 * 255, 0.225 * 255 };
        std::vector<cv::Mat> rgb_channels(3);
        cv::split(blob_image, rgb_channels); // Merge image data channels.
        for (auto i = 0; i < rgb_channels.size(); i++) {
            // Normalize the data of each channel based on this.
            rgb_channels[i].convertTo(rgb_channels[i], CV_32FC1, 1.0 / std_values[i], (0.0 - mean_values[i]) / std_values[i]);
        }
        cv::merge(rgb_channels, blob_image); // Merge image data channels.
    }
    else if (type == 4) {
        // 获取仿射变换信息
        cv::Point center(blob_image.cols / 2, blob_image.rows / 2);
        cv::Size input_size(blob_image.cols, blob_image.rows);
        int rot = 0; 
        cv::Size output_size(input_W, input_H); 

        cv::Mat warp_mat;
        warp_mat = get_affine_transform(center, input_size, rot, output_size);
        cv::warpAffine(blob_image, blob_image, warp_mat, output_size, cv::INTER_LINEAR);
        std::vector<float> std_values{ 1.0 * 255, 1.0 * 255, 1.0 * 255 };
        std::vector<cv::Mat> rgb_channels(3);
        cv::split(blob_image, rgb_channels); // Merge image data channels.
        for (auto i = 0; i < rgb_channels.size(); i++) {
            // Normalize the data of each channel based on this.
            rgb_channels[i].convertTo(rgb_channels[i], CV_32FC1, 1.0 / std_values[i], 0);
        }
        cv::merge(rgb_channels, blob_image); // Merge image data channels.
    }
    else if (type == 5) {

        cv::Point center(blob_image.cols / 2, blob_image.rows / 2);
        cv::Size input_size(blob_image.cols, blob_image.rows);
        int rot = 0; 
        cv::Size output_size(input_W, input_H); 
        cv::Mat warp_mat;
        warp_mat = get_affine_transform(center, input_size, rot, output_size);
        
        cv::warpAffine(blob_image, blob_image, warp_mat, output_size, cv::INTER_LINEAR);
        std::vector<float> std_values{ 1.0, 1.0, 1.0 };
        std::vector<cv::Mat> rgb_channels(3);
        cv::split(blob_image, rgb_channels); // Merge image data channels.
        for (auto i = 0; i < rgb_channels.size(); i++) {
            // Normalize the data of each channel based on this.
            rgb_channels[i].convertTo(rgb_channels[i], CV_32FC1, 1.0 / std_values[i], 0);
        }
        cv::merge(rgb_channels, blob_image); // Merge image data channels.
    }
    // Fill data into Tensor data memory
    fill_tensor_data_image(input_image_tensor, blob_image);

    return (void*)openvino_core;
    
}

// @brief Load other data into data in Tensor.
// @param core_ptr The CoreStruct pointer.
// @param w_node_name The input node name.
// @param input_data The input data array.
// @return The CoreStruct pointer
void* load_input_data(void* core_ptr, const wchar_t* w_node_name, float* input_data) { 

    CoreStruct* openvino_core = (CoreStruct*)core_ptr;
    std::string input_node_name = wchar_to_string(w_node_name);
    // Gets an input tensor for inference by tensor name.
    ov::Tensor input_image_tensor = openvino_core->infer_request.get_tensor(input_node_name);
    std::vector<size_t> input_shape = input_image_tensor.get_shape(); // Get input tensor shape
    int input_size = std::accumulate(input_shape.begin(), input_shape.end(), 
        1, std::multiplies<int>());  // Get input data length.
    // Fill data into Tensor data memory.
    fill_tensor_data_float(input_image_tensor, input_data, input_size);
    return (void*)openvino_core;
}

// @brief The model infer.
// @param core_ptr The CoreStruct pointer.
// @return The CoreStruct pointer
void* core_infer(void* core_ptr){

    CoreStruct* openvino_core = (CoreStruct*)core_ptr;
    openvino_core->infer_request.infer();

    return (void*)openvino_core;
}

// @brief Get float type of infer result.
// @param core_ptr The CoreStruct pointer.
// @param w_node_name The output node name.
// @param [out]  inference_result The infer result array.
void read_infer_result_F32(void* core_ptr, const wchar_t* w_node_name, float* infer_result) {
    CoreStruct* openvino_core = (CoreStruct*)core_ptr;
    std::string output_node_name = wchar_to_string(w_node_name);
    // Gets an output tensor for inference by tensor name.
    const ov::Tensor& output_tensor = openvino_core->infer_request.get_tensor(output_node_name);
    std::vector<size_t> input_shape = output_tensor.get_shape(); // Get output tensor shape
    int output_size = std::accumulate(input_shape.begin(), input_shape.end(), 
        1, std::multiplies<int>()); // Get output data length.
    // Get tensor data address
    const float* results = output_tensor.data<const float>();
    for (int i = 0; i < output_size; i++) {
        *infer_result = results[i];
        infer_result++;
    }
}

// @brief Get int type of infer result.
// @param core_ptr The CoreStruct pointer.
// @param w_node_name The output node name.
// @param [out]  inference_result The infer result array.
void read_infer_result_I32(void* core_ptr, const wchar_t* w_node_name, int* infer_result) {
    CoreStruct* openvino_core = (CoreStruct*)core_ptr;
    std::string output_node_name = wchar_to_string(w_node_name);
    // Gets an output tensor for inference by tensor name.
    const ov::Tensor& output_tensor = openvino_core->infer_request.get_tensor(output_node_name);
    std::vector<size_t> input_shape = output_tensor.get_shape();  // Get output tensor shape
    int output_size = std::accumulate(input_shape.begin(), input_shape.end(), 
        1, std::multiplies<int>()); // Get output data length.
    // Get tensor data address
    const int* results = output_tensor.data<const int>();
    for (int i = 0; i < output_size; i++) {
        *infer_result = results[i];
        infer_result++;
    }
}

// @brief Get long long type of infer result.
// @param core_ptr The CoreStruct pointer.
// @param w_node_name The output node name.
// @param [out]  inference_result The infer result array.
void read_infer_result_I64(void* core_ptr, const wchar_t* w_node_name, long long* infer_result) {
    CoreStruct* openvino_core = (CoreStruct*)core_ptr;
    std::string output_node_name = wchar_to_string(w_node_name);
    // Gets an output tensor for inference by tensor name.
    const ov::Tensor& output_tensor = openvino_core->infer_request.get_tensor(output_node_name);
    std::vector<size_t> input_shape = output_tensor.get_shape(); // Get output tensor shape
    int output_size = std::accumulate(input_shape.begin(), input_shape.end(), 
        1, std::multiplies<int>()); // Get output data length.
    // Get tensor data address
    const long long* results = output_tensor.data<const long long>();
    for (int i = 0; i < output_size; i++) {
        *infer_result = results[i];
        infer_result++;
    }
}


// @brief Delete the inference core structure pointer and free up the occupied memory
// @param core_ptr The CoreStruct pointer.
void core_delet(void* core_ptr) {
    CoreStruct* openvino_core = (CoreStruct*)core_ptr;
    delete openvino_core;
}
