#pragma once


#include<time.h>
#include<iostream>
#include<map>
#include<string>
#include<vector>
#include "openvino/openvino.hpp"
#include "opencv2/opencv.hpp"
#include<windows.h>



// @breaf �쳣״̬
enum class ExceptionStatus : int {
	NotOccurred = 0, // δ�����쳣
	Occurred = 1 // �����쳣
};


// @brief ��wchar_t*�ַ���ָ��ת��Ϊstring�ַ�����ʽ
std::string wchar_to_string(const wchar_t* wchar);

// @brief ��ͼƬ�ľ�������ת��Ϊopencv��mat����
cv::Mat data_to_mat(uchar* data, size_t size);

// @brief �����������ΪͼƬ���ݵĽڵ���и�ֵ��ʵ��ͼƬ������������
void fill_tensor_data_image(ov::Tensor& input_tensor, const cv::Mat& input_image);

// @brief �����������Ϊfkloat���ݵĽڵ���и�ֵ��ʵ��float������������
void fill_tensor_data_float(ov::Tensor& input_tensor, float* input_data, int data_size);

// @brief ��������任����
cv::Mat get_affine_transform(cv::Point center, cv::Size input_size, int rot, cv::Size output_size,
	cv::Point2f shift = cv::Point2f(0, 0));