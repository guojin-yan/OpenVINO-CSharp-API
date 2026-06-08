//  ========================================================================
//  【项目名称】OpenVINO C# API
//  【项目描述】OpenVINO™ 的 C# 语言绑定库，提供高性能深度学习推理能力
//  【版权声明】© 2026-2025 Guojin Yan. All Rights Reserved.
//  【开源协议】Apache-2.0 License（请遵守许可证条款）
//  -----------------------------------------------------------------------
//  【功能简介】
//  1. 完整的 OpenVINO™ C API 封装，提供 C# 友好的面向对象接口。
//  2. 支持模型加载、编译、推理全流程操作。
//  3. 支持 CPU、GPU、VPU 等多种推理设备。
//  4. 支持同步推理和异步推理模式。
//  5. 支持预处理和后处理流水线配置。
//  6. 支持动态形状和批量推理。
//  7. 支持模型缓存和性能分析。
//  8. 支持远程上下文（Remote Context）和零拷贝推理。
//  9. 支持 .NET Framework 4.6.1+、.NET Core 2.0+、.NET 5/6/7/8/9+。
//  10. 提供推理请求对象池，优化高并发场景性能。
//  11. 提供完善的异常处理和日志记录机制。
//  12. 提供丰富的单元测试和集成测试用例。
//  -----------------------------------------------------------------------
//  【官方资源】
//  📌 GitHub仓库：https://github.com/guojin-yan/OpenVINO-CSharp-API
//  📌 NuGet包：https://www.nuget.org/packages/OpenVINO.CSharp.API
//  📌 在线文档：https://guojin-yan.github.io/OpenVINO-CSharp-API/index.html
//  📌 示例代码：https://github.com/guojin-yan/OpenVINO-CSharp-API/tree/csharp3.3/samples
//  -----------------------------------------------------------------------
//  【社区支持】
//  💬 QQ交流群：945057948（加入获取技术支持）
//  📱 微信公众号：CSharp与边缘模型部署（教程+案例）
//  📝 CSDN博客：https://guojin.blog.csdn.net（技术文章）
//  -----------------------------------------------------------------------
//  【联系我们】
//  ✉ 项目维护：guojin_yjs@cumt.edu.cn
//  💬 微信咨询：15253793309
//  🐛 Bug反馈：https://github.com/guojin-yan/OpenVINO-CSharp-API/issues
//  💡 功能建议：https://github.com/guojin-yan/OpenVINO-CSharp-API/discussions/landing
//  -----------------------------------------------------------------------
//  【致谢】
//  本项目基于 Intel® OpenVINO™ 工具包开发，感谢 Intel 提供的优秀开源项目。
//  OpenVINO™ 是 Intel Corporation 的商标。
//  ========================================================================
//  
//  【许可声明】
//  1. 本项目采用 Apache-2.0 License 开源协议，允许自由使用、修改和分发。
//  2. 使用本项目即表示您同意 Apache-2.0 License 许可证的所有条款。
//  3. 本项目按"原样"提供，不提供任何形式的担保。
//  4. 使用本项目产生的任何风险由使用者自行承担。
//  5. 修改或分发时请保留原始版权声明和许可声明。
//  ========================================================================
//

namespace OpenVinoSharp
{
    /// <summary>
    /// 设备配置属性键 / Property keys for device configuration
    /// </summary>
    public enum PropertyKey
    {
        /// <summary>
        /// 读写属性，用于设置/获取CPU插件使用的线程数 / Read-write property to set/get the number of threads used by CPU plugin
        /// </summary>
        CPU_THREADS_NUM,
        /// <summary>
        /// 读写属性，用于设置/获取线程绑定模式 / Read-write property to set/get the bind thread mode
        /// </summary>
        CPU_BIND_THREAD,
        /// <summary>
        /// 读写属性，用于设置/获取推理请求流数量 / Read-write property to set/get the number of inference requests
        /// </summary>
        CPU_THROUGHPUT_STREAMS,
        /// <summary>
        /// 只读属性，用于获取设备名称 / Read-only property to get the device name
        /// </summary>
        DEVICE_ID,
        /// <summary>
        /// 只读属性，用于获取支持的属性列表 / Read-only property to get the supported properties
        /// </summary>
        SUPPORTED_PROPERTIES,
        /// <summary>
        /// 只读属性，用于获取可用设备列表 / Read-only property to get the available devices
        /// </summary>
        AVAILABLE_DEVICES,
        /// <summary>
        /// 只读属性，用于获取设备全名 / Read-only property to get the device full name
        /// </summary>
        DEVICE_FULL_NAME,
        /// <summary>
        /// 读写属性，用于设置/获取缓存目录 / Read-write property to set/get the cache directory
        /// </summary>
        CACHE_DIR,
        /// <summary>
        /// 读写属性，用于启用/禁用模型缓存 / Read-write property to enable/disable model caching
        /// </summary>
        CACHE_ENABLE,
        /// <summary>
        /// 只读属性，用于获取最优推理请求数量 / Read-only property to get the optimal number of inference requests
        /// </summary>
        OPTIMAL_NUMBER_OF_INFER_REQUESTS,
        /// <summary>
        /// 只读属性，用于获取最大批处理大小 / Read-only property to get the maximum number of batches
        /// </summary>
        MAX_BATCH_SIZE,
        /// <summary>
        /// 只读属性，用于获取批处理范围 / Read-only property to get the range of batches
        /// </summary>
        BATCH_PROPERTY,
        /// <summary>
        /// 只读属性，用于获取指标键列表 / Read-only property to get the metric keys
        /// </summary>
        METRIC_KEYS,
        /// <summary>
        /// 只读属性，用于获取配置键列表 / Read-only property to get the configuration keys
        /// </summary>
        CONFIGURATION_KEYS,
    }
}
