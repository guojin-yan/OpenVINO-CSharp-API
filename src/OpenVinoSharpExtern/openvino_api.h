#pragma once
//#include<time.h>
//#include<iostream>
#include<map>
#include<string>
#include<vector>
#include "openvino/openvino.hpp"
#include "opencv2/opencv.hpp"
#include<windows.h>

#include"common.h"

// @brief ������Ľṹ��
typedef struct openvino_core_struct {
	ov::Core core; // core����
	std::shared_ptr<ov::Model> model_ptr; // ��ȡģ��ָ��
	ov::CompiledModel compiled_model; // ģ�ͼ��ص��豸����
	ov::InferRequest infer_request; // �����������
} CoreStruct;



// @brief ��ʼ��openvino���Ľṹ�壬��ȡ��������ģ�ͣ���ģ�ͼ��ص��豸����������������
EXTERN_C __MIDL_DECLSPEC_DLLEXPORT void* STDMETHODCALLTYPE core_init(
	const wchar_t* w_model_dir, const wchar_t* w_device, const wchar_t* w_cache_dir);

// @brief Ϊ����tensor��������״������µ��ܴ�С����ǰһ������ȡ��֮ǰ������
EXTERN_C __MIDL_DECLSPEC_DLLEXPORT void* STDMETHODCALLTYPE set_input_sharp(
	void* core_ptr, const wchar_t* w_node_name, size_t * input_shape, int input_size);

// @brief ��ͼƬ���ݼ��ص�tensor�е������ڴ���
EXTERN_C __MIDL_DECLSPEC_DLLEXPORT void* STDMETHODCALLTYPE load_image_input_data(
	void* core_ptr, const wchar_t* w_node_name, uchar * image_data, size_t image_size, int type);

// @brief ���������ݼ��ص�tensor�е������ڴ���
EXTERN_C __MIDL_DECLSPEC_DLLEXPORT void* STDMETHODCALLTYPE load_input_data(
	void* core_ptr, const wchar_t* w_node_name, float* input_data);

// @brief �Լ��غõ�����ģ�ͽ�������
EXTERN_C __MIDL_DECLSPEC_DLLEXPORT void* STDMETHODCALLTYPE core_infer(void* core_ptr);

// @brief ��ѯfloat���͵�������
EXTERN_C __MIDL_DECLSPEC_DLLEXPORT void STDMETHODCALLTYPE read_infer_result_F32(
	void* core_ptr, const wchar_t* w_node_name, float* infer_result);

// @brief ��ѯint���͵�������
EXTERN_C __MIDL_DECLSPEC_DLLEXPORT void STDMETHODCALLTYPE read_infer_result_I32(
	void* core_ptr, const wchar_t* w_node_name, int* infer_result);

// @brief ��ѯlong long���͵�������
EXTERN_C __MIDL_DECLSPEC_DLLEXPORT void STDMETHODCALLTYPE read_infer_result_I64(
	void* core_ptr, const wchar_t* w_node_name, long long* infer_result);

// @brief ɾ��������Ľṹ��ָ�룬�ͷ�ռ���ڴ�
EXTERN_C __MIDL_DECLSPEC_DLLEXPORT void STDMETHODCALLTYPE core_delet(void* core_ptr);
// @brief ���Ժ���
EXTERN_C __MIDL_DECLSPEC_DLLEXPORT int STDMETHODCALLTYPE test(int a, int b) { return a + b; }
