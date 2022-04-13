// openvino_text.cpp : 此文件包含 "main" 函数。程序执行将在此处开始并结束。
//

#include <iterator>
#include <memory>
#include <sstream>
#include <string>
#include <vector>
#include<ctime>

#include "openvino/openvino.hpp"
#include "opencv2/opencv.hpp"

void FillBlobImage(ov::Tensor& inputBlob, const cv::Mat& frame) {
    ov::Shape blob_shape = inputBlob.get_shape();
    const size_t width = blob_shape[3];
    const size_t height = blob_shape[2];
    const size_t channels = blob_shape[1];

    float* inputBlobData = inputBlob.data<float>();
    //hwc -> chw, Write image data into node's blob
    for (size_t c = 0; c < channels; c++) {
        for (size_t h = 0; h < height; h++) {
            for (size_t w = 0; w < width; w++) {
                inputBlobData[c * width * height + h * width + w] = frame.at<cv::Vec<float, 3>>(h, w)[c];
            }
        }
    }
}

void FillBlobImInfo(ov::Tensor& inputBlob, std::pair<float, float> image_info) {

    float* inputBlobData = inputBlob.data<float>();
    // Write image info into node's blob
    inputBlobData[0] = static_cast<float>(image_info.first);
    inputBlobData[1] = static_cast<float>(image_info.second);
}


typedef struct openvino_core {
    ov::Core core;                     //ie对象
    std::shared_ptr<ov::Model> model;
    ov::CompiledModel compiled_model;
    ov::InferRequest infer_request;
} CoreStruct;

int* find_array_max(const float* result)
{

    float temp_result[102];
    for (int i = 0; i < 102; i++) {
        temp_result[i] = result[i];
    }
    for (int i = 0; i < 102; i++) {
        float max = temp_result[i];
        for (int j = i + 1; j < 102; j++) {
            if (max < temp_result[j]) {
                float temp = temp_result[j];
                temp_result[j] = max;
                max = temp;
            }
        }
        temp_result[i] = max;
    }
    int index[5];
    for (int i = 0; i < 5; i++) {
        int s;
        for (s = 0; s < 102; s++) {
            if (result[s] == temp_result[i])
                break;
        }
        index[i] = s;
    }
    return index;
}

void flower_clas() {
    // 时间数组
    double total_time[100];
    double pre_time[100];
    double infer_time[100];
    double nms_time[100];
    // 检测次数
    int n = 1;

    // 模型地址
    std::string model_xml_file = "E:\\Text_Model\\flowerclas\\inference.pdmodel";//"E:\\Text_Model\\flower_classifacition_FP32\\flowers_rec.xml";
    std::string imagePath = "E:\\Text_dataset\\flowers102\\jpg\\image_00010.jpg";//"E:\\Text_dataset\\flowers102\jpg\\image_00001.jpg";
    for (int i = 0; i < n; i++) {
        clock_t start_time1, start_time2, start_time3, end_time1, end_time2, end_time3;
        start_time1 = std::clock();
        // 初始化推理核心
        CoreStruct* p = new CoreStruct();
        p->model = p->core.read_model(model_xml_file);
        p->compiled_model = p->core.compile_model(p->model, "CPU");
        p->infer_request = p->compiled_model.create_infer_request();
        end_time1 = std::clock();

        start_time2 = std::clock();


        // 配置图片输入
        ov::Tensor input_image_blob = p->infer_request.get_tensor("x");
        input_image_blob.set_shape({ 1,3,224,224 });
        auto input_H = input_image_blob.get_shape()[2]; //获得"image"节点的Height
        auto input_W = input_image_blob.get_shape()[3]; //获得"image"节点的Width
        cv::Mat input_image_mat = cv::imread(imagePath);
        cv::Mat blob_image;
        cv::cvtColor(input_image_mat, blob_image, cv::COLOR_BGRA2RGB); //Convert RGBA to RGB
        // 放缩图片到(input_H,input_W)
        cv::resize(blob_image, blob_image, cv::Size(input_H, input_W), 0, 0, cv::INTER_LINEAR);
        // 图像数据归一化，减均值mean，除以方差std
        // PaddleDetection模型使用imagenet数据集的均值 Mean = [0.485, 0.456, 0.406]和方差 std = [0.229, 0.224, 0.225]
        std::vector<float> mean_values{ 0.485 * 255, 0.456 * 255, 0.406 * 255 };
        std::vector<float> std_values{ 0.229 * 255, 0.224 * 255, 0.225 * 255 };
        std::vector<cv::Mat> rgb_channels(3);
        cv::split(blob_image, rgb_channels);// 分离数据通道
        for (auto i = 0; i < rgb_channels.size(); i++) {
            rgb_channels[i].convertTo(rgb_channels[i], CV_32FC1, 1.0 / std_values[i], (0.0 - mean_values[i]) / std_values[i]);
        }
        cv::merge(rgb_channels, blob_image);
        FillBlobImage(input_image_blob, blob_image);
        p->infer_request.infer();

        end_time2 = std::clock();
        start_time3 = std::clock();
        // 读取推理结果
        const ov::Tensor& output_tensor = p->infer_request.get_output_tensor();
        const auto result = output_tensor.data<const float>();
        int* index = find_array_max(result);
        end_time3 = std::clock();

        // 删除推理核心地址
        delete p;

        double ts0 = (double)(end_time3-start_time1);
        double ts1 = (double)(end_time1 - start_time1);
        double ts2 = (double)(end_time2 - start_time2);
        double ts3 = (double)(end_time3 - start_time3);

        total_time[i] = ts0;
        pre_time[i] = ts1;
        infer_time[i] = ts2;
        nms_time[i] = ts3;

        std::cout << "第" << i << "次： " << "总时间：" << ts0 << " 预处理时间：" << ts1 << " 推理时间：" << ts2 << " 后处理时间：" << ts3 << std::endl;
    
    }
    double total = 0.0, pre = 0.0, infer = 0.0, nms = 0.0;
    for (int i = 0; i < n; i++) {
        total += total_time[i];
        pre += pre_time[i];
        infer += infer_time[i];
        nms += nms_time[i];
    }
    
    std::cout  << "总时间：" << total / n << " 预处理时间：" << pre / n << " 推理时间：" << infer / n << " 后处理时间：" << nms / n << std::endl;

}

#pragma region vehicle_yolov3_darknet

int vehicle_yolov3_darknet() {
    // 时间数组
    double total_time[100];
    double pre_time[100];
    double infer_time[100];
    double nms_time[100];
    // 测试次数
    int n = 1;

    // 模型相关信息
    // 设备名称
    std::string DEVICE = "CPU";
    // 模型路径，请将下载模型放置在英文路径下
    std::string IR_FileXML = "E:/Text_Model/vehicle_yolov3_darknet/model.pdmodel";
    // 测试图片路径
    std::string IMAGE_FILE = "E:/Text_dataset/vehicle_yolov3_darknet/006.jpg";
    float CONF_THRESHOLD = 0.2; //取值0~1
    // 模型输出节点名
    std::string bbox_name = "multiclass_nms3_0.tmp_0";
    std::string bbox_num_name = "multiclass_nms3_0.tmp_2";
    // 标签输入
    std::vector<std::string> LABELS = { "car ","truck","bus","motorbike","tricycle","carplate" }; 

    // 循环测试nc次
    for (int i = 0; i < n; i++) {
        // 使用clock_t方法进行时间检测
        clock_t start_time1, start_time2, start_time3, end_time1, end_time2, end_time3;
        start_time1 = std::clock();
        // 初始化推理核心
        CoreStruct* p = new CoreStruct();
        p->model = p->core.read_model(IR_FileXML);
        p->compiled_model = p->core.compile_model(p->model, "CPU");
        p->infer_request = p->compiled_model.create_infer_request();

        end_time1 = std::clock();
        start_time2 = std::clock();
        // 配置输入数据
        ov::Tensor input_image_blob = p->infer_request.get_tensor("image");
        input_image_blob.set_shape({ 1,3,608,608 });
        input_image_blob.set_shape({ 1,3,608,608 });

        auto input_H = input_image_blob.get_shape()[2]; //获得"image"节点的Height
        auto input_W = input_image_blob.get_shape()[3]; //获得"image"节点的Width

        cv::Mat img = cv::imread(IMAGE_FILE, cv::IMREAD_COLOR); //从图像文件读入数据

        // 交换RB通道
        cv::Mat blob;
        cv::cvtColor(img, blob, cv::COLOR_BGR2RGB); //Convert BGR to RGB
        // 放缩图片到(input_H,input_W)
        cv::resize(blob, blob, cv::Size(input_H, input_W), 0, 0, cv::INTER_LINEAR);
        std::vector<float> mean_values{ 0.485 * 255, 0.456 * 255, 0.406 * 255 };
        std::vector<float> std_values{ 0.229 * 255, 0.224 * 255, 0.225 * 255 };
        std::vector<cv::Mat> rgbChannels(3);
        split(blob, rgbChannels);
        for (auto i = 0; i < rgbChannels.size(); i++)
        {
            rgbChannels[i].convertTo(rgbChannels[i], CV_32FC1, 1.0 / std_values[i], (0.0 - mean_values[i]) / std_values[i]);
        }
        merge(rgbChannels, blob);

        FillBlobImage(input_image_blob, blob);
        const float scale_h = float(input_H) / float(img.rows);
        const float scale_w = float(input_W) / float(img.cols);
        const std::pair<float, float> scale_factor(scale_h, scale_w);
        auto scale_factor_blob = p->infer_request.get_tensor("scale_factor");
        scale_factor_blob.set_shape({ 1,2 });
        FillBlobImInfo(scale_factor_blob, scale_factor);  //scale_factor node's precision is float32
        const std::pair<float, float> im_shape(input_H, input_W);
        auto im_shape_blob = p->infer_request.get_tensor("im_shape");
        im_shape_blob.set_shape({ 1,2 });
        FillBlobImInfo(im_shape_blob, im_shape);  //im_shape node's precision is float32

        // 模型推理
        p->infer_request.infer();

        end_time2 = std::clock();
        start_time3 = std::clock();

        // 读取推理结果
        const ov::Tensor& output_tensor1 = p->infer_request.get_tensor(bbox_name);
        const ov::Tensor& output_tensor2 = p->infer_request.get_tensor(bbox_num_name);
        const float* detections = output_tensor1.data<const float>();
        const int* bbox_nums = output_tensor2.data<const int>();
        auto bbox_num = bbox_nums[0];
        for (int i = 0; i < bbox_num; i++) {
            auto class_id = static_cast<int> (detections[i * 6 + 0]);
            float score = detections[i * 6 + 1];
            if (score > CONF_THRESHOLD) {
                float x1 = detections[i * 6 + 2];
                float y1 = detections[i * 6 + 3];
                float x2 = detections[i * 6 + 4];
                float y2 = detections[i * 6 + 5];
                //cout << i<<"class_id:" << class_id << "; score:" << score << "; x1:" << x1 << "; y1:" << y1 << "; x2:" << x2 << "; y2:" << y2 << "; i:" << i << endl;
                std::ostringstream conf;
                conf << ":" << std::fixed << std::setprecision(3) << score;
                cv::putText(img, (LABELS[class_id] + conf.str()),
                    cv::Point2f(x1, y1 - 5), cv::FONT_HERSHEY_COMPLEX, 0.4,
                    cv::Scalar(255, 0, 0), 1);
                cv::rectangle(img, cv::Point2f(x1, y1), cv::Point2f(x2, y2), cv::Scalar(255, 0, 0));
            }
        }

        end_time3 = std::clock();

        // 删除推理核心地址
        delete p;

        double ts0 = (double)(end_time3 - start_time1);
        double ts1 = (double)(end_time1 - start_time1);
        double ts2 = (double)(end_time2 - start_time2);
        double ts3 = (double)(end_time3 - start_time3);

        total_time[i] = ts0;
        pre_time[i] = ts1;
        infer_time[i] = ts2;
        nms_time[i] = ts3;

        std::cout << "第" << i << "次： " << "总时间：" << ts0 << " 预处理时间：" << ts1 << " 推理时间：" << ts2 << " 后处理时间：" << ts3 << std::endl;

    }
    double total = 0.0, pre = 0.0, infer = 0.0, nms = 0.0;
    for (int i = 0; i < n; i++) {
        total += total_time[i];
        pre += pre_time[i];
        infer += infer_time[i];
        nms += nms_time[i];
    }

    std::cout << "总时间：" << total / n << " 预处理时间：" << pre / n << " 推理时间：" << infer / n << " 后处理时间：" << nms / n << std::endl;

    return 0;
}

#pragma endregion

int main()
{
    std::cout << "Hello World!\n";
    flower_clas();
    vehicle_yolov3_darknet();
}

