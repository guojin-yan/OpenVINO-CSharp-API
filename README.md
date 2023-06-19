![OpenVinoSharp](https://socialify.git.ci/guojin-yan/OpenVinoSharp/image?description=1&descriptionEditable=💞OpenVINO%20wrapper%20for%20.NET💞&forks=1&issues=1&logo=https%3A%2F%2Fs2.loli.net%2F2023%2F01%2F26%2FylE1K5JPogMqGSW.png&name=1&owner=1&pattern=Circuit%20Board&pulls=1&stargazers=1&theme=Light)

## <img title="更新日志" src="https://s2.loli.net/2023/01/26/RJ1znO78bygCcKj.png" alt="" width="40">更新日志

- #### 🔥 **2023.1.23 ：发布 OpenVinoSharp 2.0**

  - 🗳 **OpenVinoSharp 库：**
    - 基于OpenVinoSharp 1.0 以及使用中所出现的问题，将一些问题进行了修改；
    - 增加数据处理方式。

  - 🔮 **NuGet包：**
    - 制作并发布NuGet包，包括**OpenVinoSharp.win**  、**OpenVinoSharp.win.simplify**  两个安装包。

  - 💡  **技术文档：**
    - 无



## <img title="更新日志" src="https://s2.loli.net/2023/01/26/Zs1VFUT4BGQgfE9.png" alt="" width="40"> 简介

&emsp;    [OpenVINO™](www.openvino.ai)是一个用于优化和部署深度学习模型的开源工具包，是英特尔基于自身现有的硬件平台开发的一种可以加快高性能计算机视觉和深度学习视觉应用开发速度工具套件，用于快速开发应用程序和解决方案，以解决各种任务（包括人类视觉模拟、自动语音识别、自然语言处理和推荐系统等）。

&emsp;    官方发行的[OpenVINO™](www.openvino.ai)未提供C#编程语言接口，因此在使用时无法实现在C#中利用[OpenVINO™](www.openvino.ai)进行模型部署。在该项目中，利用动态链接库功能，调用官方依赖库，实现在C#中部署深度学习模型，为方便使用，在该项目中提供了NuGet包方便使用，为了方便大家再此基础上进行开发，该项目提供了详细的技术文档。

<img title="更新日志" src="https://s2.loli.net/2023/01/26/LdbeOYGgwZvHcBQ.png" alt="" width="300">

## <img title="NuGet" src="https://s2.loli.net/2023/01/26/ks9BMwXaHqQnKZP.png" alt="" width="40">NuGet包

### 托管库

| Package               | Description                                                  | Link                                                         |
| --------------------- | ------------------------------------------------------------ | ------------------------------------------------------------ |
| **OpenVinoSharp.win** | OpenVinoSharp core libraries，附带完整的OpenCV 4.5.5、OpenVINO 2022.3依赖库 | [![NuGet Gallery ](https://badge.fury.io/nu/OpenVinoSharp.win.svg)](https://www.nuget.org/packages/OpenVinoSharp.win/) |



## <img title="安装" src="https://s2.loli.net/2023/01/26/bm6WsE5cfoVvj7i.png" alt="" width="50"> 安装

### OpenVINO安装

&emsp;OpenVINO安装，请参考[openvino_installation.md](.\docs\openvino_installation.md)安装指导文档。

### OpenVinoSharp NuGet包安装

##### （1）下载NuGet包

&emsp;使用Visual Studio自带的NuGet管理包，搜索OpenVinoSharp.win，找到对应的包，并将其安装到项目中。

<img title="nuget" src="https://s2.loli.net/2023/02/09/vpkBefE3bSlGuVU.png" alt="" width="500">

##### （2）复制依赖项

&emsp;由于项目依赖较多的外部依赖项，因此为了方便使用，此处提供了所需的依赖。

&emsp;打开下载的NuGet下载路径，路径一般为``C:\Users\(用户名)\.nuget\packages\``，并找到``openvinosharp.win``，将``external lib``文件夹中的所有文件复制到程序运行目录中。

<img title="nuget" src="https://s2.loli.net/2023/02/09/CpdNnRMXJhUz19Y.png" alt="" width="500">






## <img title="API文档" src="https://s2.loli.net/2023/02/09/zVyS1Z6dm45n2RB.png" alt="" width="30">技术文档

&emsp;由于该项目是基于作者自身使用需求开发，结合常见模型部署方式封装的API函数，因此为了满足其他开发者的需求，此处公开详细的技术文档，如有需求，可以自行根据技术文档[openvinosharp_documents](.\docs\openvinosharp_documents.md)修改。



## <img title="" src="https://s2.loli.net/2023/02/09/2ApTvzLDwlYS6Ks.png" alt="" width="40"> 应用案例

- #### 基于C#和OpenVINO部署PP-Human

​			**|** [GitHub代码库](https://github.com/guojin-yan/Csharp_and_OpenVINO_deploy_PP-Human) **|**  [CSDN博客]() **|**  [微信推文](https://mp.weixin.qq.com/s/0pIoO3aATCu_mFZAQUARtA) **|** 


- #### 基于C#和OpenVINOTM部署PP-TinyPose

​			**|** [GitHub代码库](https://github.com/guojin-yan/Csharp_and_OpenVINO_deploy_PP-TinyPose) **|**  [CSDN博客]() **|**  [微信推文](https://mp.weixin.qq.com/s/ynCE5J-bJxq0faIPwNnWDA) **|** 


- #### 基于C#和OpenVINO部署PaddleOCR模型

​			**|** [GitHub代码库](https://github.com/guojin-yan/Csharp_and_OpenVINO_deploy_PaddleOCR) **|**  [CSDN博客]() **|**  


- #### 

## <img title="" src="https://user-images.githubusercontent.com/48054808/157835345-f5d24128-abaf-4813-b793-d2e5bdc70e5a.png" alt="" width="40"> 许可证书

本项目的发布受[Apache 2.0 license](LICENSE)许可认证。



## <img title="API文档" src="https://s2.loli.net/2023/01/26/CNgHGrJ2DyvsaP4.png" alt="" width="50">API文档

### 命名空间

```c#
using OpenVinoSharp;
```

### 模型推理API

<table>
	<tr>
	    <th width="7%" align="center" bgcolor=#FF7A68>序号</th>
	    <th width="35%" colspan="2" align="center" bgcolor=#FF7A68>API</th>
	    <th width="43%" align="center" bgcolor=#FF7A68>参数解释</th>  
        <th width="15%" align="center" bgcolor=#FF7A68>说明</th>
	</tr >
	<tr >
	    <td rowspan="4" align="center">1</td>
	    <td align="center">方法</td>
        <td>Core()</td>
        <td>构造函数</td>
        <td rowspan="4">初始化推理核心，读取本地模型，加载到设备，并创建推理通道</td>
	</tr>
    <tr >
	    <td rowspan="3" align="center">参数</td>
        <td><font color=blue>string</font> model</td>
        <td>模型路径</td>
	</tr>
    <tr >
        <td><font color=blue>string</font> device</td>
        <td>设备名称</td>
	</tr>
    <tr >
        <td><font color=blue>string</font> cache_dir</td>
        <td>缓存路径</td>
	</tr>
	<tr >
	    <td rowspan="3" align="center">2</td>
	    <td align="center">方法</td>
        <td><font color=blue>void</font> set_input_shape()</td>
        <td>设置输入节点形状</td>
        <td rowspan="3">根据节点维度设置</td>
	</tr>
    <tr >
	    <td rowspan="2" align="center">参数</td>
        <td><font color=blue>string</font> node_name</td>
        <td>节点名称</td>
	</tr>
    <tr >
        <td><font color=blue>int[]</font> input_shape</td>
        <td>形状数组</td>
	</tr>
	<tr >
	    <td rowspan="6" align="center">3</td>
	    <td align="center">方法</td>
        <td><font color=blue>void</font> load_input_data()</td>
        <td>设置图片/普通输入数据</td>
        <td rowspan="6">方法重载</td>
	</tr>
    <tr >
	    <td rowspan="2" align="center">参数</td>
        <td><font color=blue>string</font> node_name</td>
        <td>输入节点名称</td>
	</tr>
    <tr >
        <td><font color=blue>float[]</font> input_data</td>
        <td>输入数据</td>
	</tr>
    <tr >
	    <td rowspan="3" align="center">参数</td>
        <td><font color=blue>string</font> node_name</td>
        <td>输入节点名称</td>
	</tr>
    <tr >
        <td><font color=blue>byte[]</font> image_data</td>
        <td>图片数据</td>
	</tr>
    <tr >
        <td><font color=blue>int</font> type</td>
        <td>数据处理类型：<br>type = 0: 均值方差归一化、常规缩放<br>type = 1: 普通归一化(1/255)、常规缩放<br>type = 2: 不归一化、常规缩放<br>type = 0: 均值方差归一化、仿射变换<br>type = 1: 普通归一化(1/255)、仿射变换<br>type = 2: 不归一化、仿射变换</td>
	</tr>
	<tr >
	    <td rowspan="1" align="center">4</td>
	    <td align="center">方法</td>
        <td><font color=blue>void</font> infer()</td>
        <td>模型推理</td>
        <td rowspan="1"></td>
	</tr>
	<tr >
	    <td rowspan="3" align="center">5</td>
	    <td align="center">方法</td>
        <td><font color=blue>void</font> <font color=green>T</font>[] read_infer_result &lt<font color=green>T</font>&gt()</td>
        <td>读取推理结果数据</td>
        <td rowspan="3">支持读取Float32、Int32、Int64格式数据</td>
	</tr>
    <tr >
	    <td rowspan="2" align="center">参数</td>
        <td><font color=blue>string</font> output_name</td>
        <td>输出节点名</td>
	</tr>
    <tr >
        <td><font color=blue>int</font> data_size</td>
        <td>输出数据长度</td>
	</tr>
	<tr >
	    <td rowspan="1" align="center">6</td>
	    <td align="center">方法</td>
        <td><font color=blue>void</font> delet()</td>
        <td>删除内存地址</td>
        <td rowspan="1"></td>
	</tr>

