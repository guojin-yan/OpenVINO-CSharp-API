#include"common.h"


// @brief Convert wchar_ t* pointer to string format.
// @param wchar The input wchar.
// @return The converted string.
std::string wchar_to_string(const wchar_t* wchar) {
    // Obtain the length of the input pointer.
    int path_size = WideCharToMultiByte(CP_OEMCP, 0, wchar, wcslen(wchar), NULL, 0, NULL, NULL);
    char* chars = new char[path_size + 1];
    // Convert double bute string to single byte string.
    WideCharToMultiByte(CP_OEMCP, 0, wchar, wcslen(wchar), chars, path_size, NULL, NULL);
    chars[path_size] = '\0';
    std::string pattern = chars;
    delete chars; // free memory
    return pattern; 
}

// @brief Convert the matrix data of the image into Mat data of OpenCV.
// @param data The image data.
// @param size The length of image data.
// @return The opencv mat.
cv::Mat data_to_mat(uchar* data, size_t size) {
    // Reading image array data into a vector container.
    std::vector<uchar> buf;
    for (int i = 0; i < size; i++) {
        buf.push_back(*data);
        data++;
    }
    // Using imdecode() to convert the data in the container to MAT type.
    return cv::imdecode(cv::Mat(buf), 1);
}

// @brief Assigning values to nodes whose input is image data in the network to achieve
//        image data input into the network.
// @param input_tensor The input node tensor.
// @param input_image The input image data.
void fill_tensor_data_image(ov::Tensor& input_tensor, const cv::Mat& input_image) {
    // Get the input size.
    ov::Shape tensor_shape = input_tensor.get_shape();
    const size_t width = tensor_shape[3]; // input weidth
    const size_t height = tensor_shape[2]; // input hight
    const size_t channels = tensor_shape[1]; // input dimension 
    // Get Tensor data memory pointer.
    float* input_tensor_data = input_tensor.data<float>();
    // Fill image data into the Tensor data.
    // (H、W、C) to (C、H、W) 
    for (size_t c = 0; c < channels; c++) {
        for (size_t h = 0; h < height; h++) {
            for (size_t w = 0; w < width; w++) {
                input_tensor_data[c * width * height + h * width + w] = input_image.at<cv::Vec<float, 3>>(h, w)[c];
            }
        }
    }
}
// @brief Assign values to nodes whose input to the network is fkloat data to achieve float
//       data input to the network.
// @param input_tensor The input node tensor.
// @param input_data The input data array
// @param data_size the input length.
void fill_tensor_data_float(ov::Tensor& input_tensor, float* input_data, int data_size) {
    // Get Tensor data memory pointer.
    float* input_tensor_data = input_tensor.data<float>();
    // Fill data into the Tensor data.
    for (int i = 0; i < data_size; i++) {
        input_tensor_data[i] = input_data[i];
    }
}

// @brief Building a radiative transformation matrix.
// @param center The center point.
// @param input_size The input size.
// @param rot The angle.
// @param output_size The output size.
// @param shift 
// @rrturn The transformation matrix.
cv::Mat get_affine_transform(cv::Point center, cv::Size input_size, int rot, cv::Size output_size, 
    cv::Point2f shift) {
    int src_w = input_size.width;
    int dst_w = output_size.width;
    int dst_h = output_size.height;

    float rot_rad = 3.1715926f * rot / 180.0;
    int pt = (int)src_w * -0.5;
    float sn = std::sin(rot_rad);
    float cs = std::cos(rot_rad);

    cv::Point2f src_dir(-1.0 * pt * sn, pt * cs);
    cv::Point2f dst_dir(0.0, dst_w * -0.5);
    cv::Point2f src[3];
    src[0] = cv::Point2f(center.x + input_size.width * shift.x, center.y + input_size.height * shift.y);
    src[1] = cv::Point2f(center.x + src_dir.x + input_size.width * shift.x, center.y + src_dir.y + input_size.height * shift.y);
    cv::Point2f direction = src[0] - src[1];
    src[2] = cv::Point2f(src[1].x - direction.y, src[1].y - direction.x);

    cv::Point2f dst[3];
    dst[0] = cv::Point2f(dst_w * 0.5, dst_h * 0.5);
    dst[1] = cv::Point2f(dst_w * 0.5 + dst_dir.x, dst_h * 0.5 + dst_dir.y);
    direction = dst[0] - dst[1];
    dst[2] = cv::Point2f(dst[1].x - direction.y, dst[1].y - direction.x);

    return cv::getAffineTransform(src, dst);

}
