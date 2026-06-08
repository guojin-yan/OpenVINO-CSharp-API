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

using System;
using System.Runtime.InteropServices;

namespace OpenVinoSharp
{
    /// <summary>
    /// [结构体] 版本信息，描述插件和 OpenVINO 库 / [struct] Represents version information that describes plugins and the OpenVINO library
    /// </summary>
    /// <ingroup>ov_runtime_c#_api</ingroup>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct Version
    {
        /// <summary>
        /// 构建号，以 null 结尾的字符串 / A null terminated string with build number
        /// </summary>
        public string buildNumber;
        /// <summary>
        /// 描述信息，以 null 结尾的字符串 / A null terminated description string
        /// </summary>
        public string description;
        /// <summary>
        /// 构造 Version 结构体 / Constructs a Version
        /// </summary>
        /// <param name="buildNumber">构建号 / Build number</param>
        /// <param name="description">描述信息 / Description</param>
        public Version(string buildNumber, string description)
        {
            this.buildNumber = buildNumber;
            this.description = description;
        }

        /// <summary>
        /// 将 Version 转换为输出字符串 / Convert Version to output string
        /// </summary>
        /// <returns>输出字符串 / Output string</returns>
        public string to_string()
        {
            string str = "";
            str += description;
            str += "\r\n    Version : ";
            str += buildNumber.Substring(0, buildNumber.IndexOf("-"));
            str += "\r\n    Build   : ";
            str += buildNumber;
            return str;
        }
    }

    /// <summary>
    /// [结构体] 版本信息，描述设备和 OV 运行时库 / [struct] Represents version information that describes device and ov runtime library
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct CoreVersion
    {
        /// <summary>
        /// 设备名称 / A device name
        /// </summary>
        public string device_name;
        /// <summary>
        /// OpenVINO 版本 / The OpenVINO version
        /// </summary>
        public Version version;
    }

    /// <summary>
    /// [结构体] 版本信息列表，描述所有设备和 OV 运行时库 / [struct] Represents version information that describes all devices and ov runtime library
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CoreVersionList
    {
        /// <summary>
        /// 设备版本数组指针 / An array of device versions
        /// </summary>
        public IntPtr core_version;
        /// <summary>
        /// 数组中的版本数量 / A number of versions in the array
        /// </summary>
        public ulong size;
    }
}
