![OpenVinoSharp](https://socialify.git.ci/guojin-yan/OpenVinoSharp/image?description=1&descriptionEditable=💞%20OpenVINO%20wrapper%20for520.NET💞%20&forks=1&issues=1&logo=https%3A%2F%2Fs2.loli.net%2F2023%2F01%2F26%2FylE1K5JPogMqGSW.png&name=1&owner=1&pattern=Circuit%20Board&pulls=1&stargazers=1&theme=Light)

 [简体中文](README.md) | English

## <img title="更新日志" src="https://s2.loli.net/2023/01/26/RJ1znO78bygCcKj.png" alt="" width="40">Update 

#### 🔥 **2023.6.19 ：Release  OpenVinoSharp 2.1**

- 🗳 **OpenVinoSharp ：**
  - Upgrade OpenVinoSharp to support OpenVino 2023.0 version.
- 💡  **Technical documentation：**
  - Add Technical documentation Deploying Yolov8 Series Models Based on C # and OpenVINO2023.0.
- 🛹**Application Cases：**
  - Add Yolov8-det, Yolov8-seg, Yolov8-pose, and Yolov8-cls model test cases.
- 🔮 **NuGet：**
  - Create and publish NuGet packages, including the **OpenVinoSharp. win 2.1.1** installation package, which includes OpenVino 2023.0 dependencies.

#### 🔥 **2023.1.23 ：Release OpenVinoSharp 2.0**

- 🗳 **OpenVinoSharp ：**
  - Based on OpenVinoSharp 1.0 and the issues encountered during use, some issues have been modified;
  - Add data processing methods.

- 🔮 **NuGet：**
  - Create and publish NuGet packages, including the **OpenVinoSharp. win** installation package.

- 💡  **Technical documentation：**
  - 无



## <img title="更新日志" src="https://s2.loli.net/2023/01/26/Zs1VFUT4BGQgfE9.png" alt="" width="40"> Introduction

&emsp;    [OpenVINO™](www.openvino.ai)is an open source toolkit for optimizing and deploying deep learning models. It is a tool suite developed by Intel based on its existing hardware platform that can speed up the development of high-performance computer vision and deep learning visual applications. It is used to quickly develop applications and solutions to solve various tasks (including human visual simulation, automatic speech recognition, Natural language processing, recommendation systems, etc.).

&emsp;    Officially released  [OpenVINO™](www.openvino.ai) does not provide a C # programming language interface, so it is not possible to utilize [OpenVINO™](www.openvino.ai) in C # during use for model deployment. In this project, the Dynamic-link library function is used to call the official dependency library to deploy the deep learning model in C #. For the convenience of use, NuGet package is provided in this project. For the convenience of development on this basis, the project provides detailed Technical documentation.

<div align=center><span><img src="https://s2.loli.net/2023/01/26/LdbeOYGgwZvHcBQ.png" height=300/></span></div>



## <img title="NuGet" src="https://s2.loli.net/2023/01/26/ks9BMwXaHqQnKZP.png" alt="" width="40">NuGet

### Managed Library

| Package               | Description                                                  | Link                                                         |
| --------------------- | ------------------------------------------------------------ | ------------------------------------------------------------ |
| **OpenVinoSharp.win** | OpenVinoSharp core libraries with complete OpenCV 4.5.5 and OpenVino 2022.3 dependency libraries | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVinoSharp.win.svg)](https://www.nuget.org/packages/OpenVinoSharp.win/) |



## <img title="安装" src="https://s2.loli.net/2023/01/26/bm6WsE5cfoVvj7i.png" alt="" width="50"> Install

### OpenVINO Install

For OpenVINO installation, please refer to the installation guide document for [openvino installation. md](.  docs  openvino installation. md).

### OpenVinoSharp NuGet Install

    Using the NuGet management package that comes with Visual Studio, search for OpenVinoSharp.win, find the corresponding package, and install it into the project. The latest version after 2.1.1 supports OpenVINO2023.0 and includes the required dependencies for OpenVINO. Users do not need to install OpenVINO and can use it directly after installing the NuGet package.

<div align=center><span><img src="https://s2.loli.net/2023/06/24/ieXuhIrYJNWjt3s.png" height=300/></span></div>




## <img title="API文档" src="https://s2.loli.net/2023/02/09/zVyS1Z6dm45n2RB.png" alt="" width="30">技术文档

&emsp;  Since the project is developed based on the author's own use requirements and combined with API functions encapsulated by common model deployment methods, in order to meet the needs of other developers, detailed Technical documentation are disclosed here. If there is a need, you can modify it according to the Technical documentation [openvinosharp_documents](.\docs\openvinosharp_documents.md).



## <img title="" src="https://s2.loli.net/2023/02/09/2ApTvzLDwlYS6Ks.png" alt="" width="40"> 应用案例

- #### 基于C#和OpenVINO部署PP-Human

​			**|** [GitHub代码库](https://github.com/guojin-yan/Csharp_and_OpenVINO_deploy_PP-Human) **|**  [CSDN博客]() **|**  [微信推文](https://mp.weixin.qq.com/s/0pIoO3aATCu_mFZAQUARtA) **|** 


- #### 基于C#和OpenVINOTM部署PP-TinyPose

​			**|** [GitHub代码库](https://github.com/guojin-yan/Csharp_and_OpenVINO_deploy_PP-TinyPose) **|**  [CSDN博客]() **|**  [微信推文](https://mp.weixin.qq.com/s/ynCE5J-bJxq0faIPwNnWDA) **|** 


- #### 基于C#和OpenVINO部署PaddleOCR模型

​			**|** [GitHub代码库](https://github.com/guojin-yan/Csharp_and_OpenVINO_deploy_PaddleOCR) **|**  [CSDN博客]() **|**  



## <img title="" src="https://user-images.githubusercontent.com/48054808/157835345-f5d24128-abaf-4813-b793-d2e5bdc70e5a.png" alt="" width="40"> License 

The release of this project is certified under the [Apache 2.0 ](LICENSE) license.



## <img title="API文档" src="https://s2.loli.net/2023/07/14/SFJb8U7hsiV1e5Y.png" alt="" width="50">API document

### Namespace

```c#
using OpenVinoSharp;
```

### API

<table>
	<tr>
	    <th width="7%" align="center" bgcolor=#FF7A68>Num</th>
	    <th width="35%" colspan="2" align="center" bgcolor=#FF7A68>API</th>
	    <th width="43%" align="center" bgcolor=#FF7A68>Parameter Description</th>  
        <th width="15%" align="center" bgcolor=#FF7A68>Explain</th>
	</tr >
	<tr >
	    <td rowspan="4" align="center">1</td>
	    <td align="center">Method</td>
        <td>Core()</td>
        <td>Constructor</td>
        <td rowspan="4">Initialize the inference core, read the local model, load it onto the device, and create the inference channel</td>
	</tr>
    <tr >
	    <td rowspan="3" align="center">Param</td>
        <td><font color=blue>string</font> model</td>
        <td>Model Path</td>
	</tr>
    <tr >
        <td><font color=blue>string</font> device</td>
        <td>Device Name</td>
	</tr>
    <tr >
        <td><font color=blue>string</font> cache_dir</td>
        <td>Cache Path</td>
	</tr>
	<tr >
	    <td rowspan="3" align="center">2</td>
	    <td align="center">Method</td>
        <td><font color=blue>void</font> set_input_shape()</td>
        <td>Set input node shape</td>
        <td rowspan="3">Set according to node dimensions</td>
	</tr>
    <tr >
	    <td rowspan="2" align="center">Param</td>
        <td><font color=blue>string</font> node_name</td>
        <td>Node name</td>
	</tr>
    <tr >
        <td><font color=blue>int[]</font> input_shape</td>
        <td>Shape array</td>
	</tr>
	<tr >
	    <td rowspan="6" align="center">3</td>
	    <td align="center">Method</td>
        <td><font color=blue>void</font> load_input_data()</td>
        <td>Set Image/Normal Input Data</td>
        <td rowspan="6">Method overload</td>
	</tr>
    <tr >
	    <td rowspan="2" align="center">Param</td>
        <td><font color=blue>string</font> node_name</td>
        <td>Input node name.</td>
	</tr>
    <tr >
        <td><font color=blue>float[]</font> input_data</td>
        <td>Input data.</td>
	</tr>
    <tr >
	    <td rowspan="3" align="center">Param</td>
        <td><font color=blue>string</font> node_name</td>
        <td>Input node name</td>
	</tr>
    <tr >
        <td><font color=blue>byte[]</font> image_data</td>
        <td>Image data</td>
	</tr>
    <tr >
        <td><font color=blue>int</font> type</td>
        <td>Data processing type：<br>type = 0: Normalization of mean variance and conventional scaling<br>type = 1: Normal normalization (1/255), regular scaling<br>type = 2: Non normalization, regular scaling<br>type = 0: Normalization of mean variance, Affine transformation<br>type = 1: Normal normalization (1/255), Affine transformation<br>type = 2: Non normalization, Affine transformation</td>
	</tr>
	<tr >
	    <td rowspan="1" align="center">4</td>
	    <td align="center">Method</td>
        <td><font color=blue>void</font> infer()</td>
        <td>Model-based reasoning</td>
        <td rowspan="1"></td>
	</tr>
	<tr >
	    <td rowspan="3" align="center">5</td>
	    <td align="center">Method</td>
        <td><font color=blue>void</font> <font color=green>T</font>[] read_infer_result &lt<font color=green>T</font>&gt()</td>
        <td>Read inference result data</td>
        <td rowspan="3">Supports reading data in Float32, Int32, and Int64 formats</td>
	</tr>
    <tr >
	    <td rowspan="2" align="center">Param</td>
        <td><font color=blue>string</font> output_name</td>
        <td>Output Node Name</td>
	</tr>
    <tr >
        <td><font color=blue>int</font> data_size</td>
        <td>Output data length</td>
	</tr>
	<tr >
	    <td rowspan="1" align="center">6</td>
	    <td align="center">Method</td>
        <td><font color=blue>void</font> delet()</td>
        <td>Delete Memory Address</td>
        <td rowspan="1"></td>
	</tr>

