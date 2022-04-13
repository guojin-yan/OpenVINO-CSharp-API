#!/usr/bin/env python
# -*- coding: utf-8 -*-
# @Time    : 2022/4/9 19:21
# @Author  : 颜国进
# @File    : flower_clas.py.py
# @Software: PyCharm
# @function:
from openvino.runtime import Core
import numpy as np
import cv2
import time
# 检测次数
n=1
# 模型路径
model_path = "E:/Text_Model/flowerclas/inference.pdmodel"
# 检测时间
total_time = []
pre_time = []
infer_time = []
nms_time = []
for i in range(0,n):
    start_time1 = time.time()
    # 初始化模型
    ie = Core()
    model = ie.read_model(model=model_path)
    compiled_model = ie.compile_model(model=model, device_name="CPU")
    request = compiled_model.create_infer_request()
    input_tensor = next(iter(compiled_model.inputs))
    end_time1 = time.time()

    start_time2 = time.time()
    # 处理测试数据
    src = cv2.imread(r"E:/Text_dataset/flowers102/jpg/image_00001.jpg")
    image = cv2.cvtColor(src, cv2.COLOR_BGR2RGB)
    image = image / 255.0
    image -= [0.485, 0.456, 0.406]
    image /= [0.229, 0.224, 0.225]
    image = cv2.resize(image, (224, 224), interpolation=cv2.INTER_AREA)  # [82 202 255]>[51 228 254]
    image = image.transpose(2, 0, 1)
    # 模型推理
    res = request.infer(inputs={input_tensor: [image]})
    end_time2 = time.time()

    start_time3 = time.time()
    # 读取推理数据
    output_tensor = request.get_tensor("softmax_1.tmp_0")
    res = output_tensor.data
    res1 = res[0]
    res2 = np.argsort(res1)
    end_time3 = time.time()

    total_time.append(end_time3-start_time1)
    pre_time.append(end_time1-start_time1)
    infer_time.append(end_time2-start_time2)
    nms_time.append(end_time3-start_time3)
    print("第" + str(i) + "次：总时间：" + str(total_time[i]) + "，初始化时间：" + str(pre_time[i])+
          "，推理时间：" + str(infer_time[i]) + "，后处理时间：" + str(nms_time[i]))

print("总时间："+str(np.mean(total_time))+"，初始化时间："+str(np.mean(pre_time))+"，推理时间："+
      str(np.mean(infer_time))+"，后处理时间："+str(np.mean(nms_time)))

