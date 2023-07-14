#pragma once


#include<time.h>
#include<iostream>
#include<map>
#include<string>
#include<vector>
#include "openvino/openvino.hpp"
#include "opencv2/opencv.hpp"
#include<windows.h>



// @brief ��wchar_t*�ַ���ָ��ת��Ϊstring�ַ�����ʽ
// @brief Convert wchar_ t* pointer to string format.
std::string wchar_to_string(const wchar_t* wchar);

// @brief ��ͼƬ�ľ�������ת��Ϊopencv��mat����
// @brief Convert the matrix data of the image into Mat data of OpenCV.
cv::Mat data_to_mat(uchar* data, size_t size);

// @brief �����������ΪͼƬ���ݵĽڵ���и�ֵ��ʵ��ͼƬ������������
// @brief Assigning values to nodes whose input is image data in the network to achieve image data input into the network.
void fill_tensor_data_image(ov::Tensor& input_tensor, const cv::Mat& input_image);

// @brief �����������Ϊfkloat���ݵĽڵ���и�ֵ��ʵ��float������������
// @brief Assign values to nodes whose input to the network is fkloat data to achieve float data input to the network.
void fill_tensor_data_float(ov::Tensor& input_tensor, float* input_data, int data_size);

// @brief ��������任����
// @brief Building a radiative transformation matrix.
cv::Mat get_affine_transform(cv::Point center, cv::Size input_size, int rot, cv::Size output_size,
	cv::Point2f shift = cv::Point2f(0, 0));