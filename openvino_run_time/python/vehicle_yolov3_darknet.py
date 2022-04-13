#!/usr/bin/env python
# -*- coding: utf-8 -*-
# @Time    : 2022/4/9 20:57
# @Author  : 颜国进
# @File    : vehicle_yolov3_darknet.py.py
# @Software: PyCharm
# @function:
from openvino.runtime import Core
import numpy as np
import cv2
import time
# 检测次数
n=1
# 模型地址
model_path = "E:/Text_Model/vehicle_yolov3_darknet/model.pdmodel"
LABELS = [ "car ", "truck", "bus", "motorbike", "tricycle", "carplate" ]
# 检测时间
total_time = []
pre_time = []
infer_time = []
nms_time = []
for i in range(0,n):
    start_time1 = time.time()
    # 初始化推理核心
    ie = Core()
    model = ie.read_model(model=model_path)
    compiled_model = ie.compile_model(model=model, device_name="CPU")
    request = compiled_model.create_infer_request()
    input_tensor = next(iter(compiled_model.inputs))
    end_time1 = time.time()

    start_time2 = time.time()
    # 配置模型数额如
    src = cv2.imread(r"E:/Text_dataset/vehicle_yolov3_darknet/006.jpg")
    image = cv2.cvtColor(src, cv2.COLOR_BGR2RGB)
    scale_h = 608 / float(image.shape[0])
    scale_w = 608 / float(image.shape[1])
    image = image / 255.0
    image -= [0.485, 0.456, 0.406]
    image /= [0.229, 0.224, 0.225]
    image = cv2.resize(image, (608, 608), interpolation=cv2.INTER_AREA)  # [82 202 255]>[51 228 254]
    image = image.transpose(2, 0, 1)
    image = np.expand_dims(image, axis=0)
    # 准备输入数据字典
    inputs = dict()
    inputs["image"] = image
    inputs["im_shape"] = np.array([[608,608]]).astype("float32")
    inputs["scale_factor"] = np.array([[scale_h, scale_w]]).astype("float32")
    # 模型推理
    res = request.infer(inputs=inputs)
    end_time2 = time.time()

    start_time3 = time.time()
    # 读出推理结果
    output_tensor1 = request.get_tensor("multiclass_nms3_0.tmp_2")
    num_arr = output_tensor1.data
    num = num_arr[0]
    output_tensor2 = request.get_tensor("multiclass_nms3_0.tmp_0")
    boxes = output_tensor2.data
    # 过滤类别ID小于0的结果
    filtered_boxes = boxes[boxes[:, 0] > -1e-06]
    # 过滤低置信度结果
    filtered_boxes = filtered_boxes[filtered_boxes[:, 1] >= 0.2]

    # 显示检测框,检测框格式 [class_id, score, x1, y1, x2, y2]
    for box in filtered_boxes:
        cv2.rectangle(src, (int(box[2]), int(box[3])), (int(box[4]), int(box[5])), (255, 0, 0))
        label = LABELS[int(box[0])]
        conf = "{:.4f}".format(box[1])
        cv2.putText(src, label + conf, (int(box[2]), int(box[3]) - 5), cv2.FONT_HERSHEY_COMPLEX, 0.5, (255, 0, 0))

    end_time3 = time.time()

    total_time.append(end_time3-start_time1)
    pre_time.append(end_time1-start_time1)
    infer_time.append(end_time2-start_time2)
    nms_time.append(end_time3-start_time3)
    print("第" + str(i) + "次：总时间：" + str(total_time[i]) + "，初始化时间：" + str(pre_time[i])+
          "，推理时间：" + str(infer_time[i]) + "，后处理时间：" + str(nms_time[i]))

print("总时间："+str(np.mean(total_time))+"，初始化时间："+str(np.mean(pre_time))+"，推理时间："+
      str(np.mean(infer_time))+"，后处理时间："+str(np.mean(nms_time)))