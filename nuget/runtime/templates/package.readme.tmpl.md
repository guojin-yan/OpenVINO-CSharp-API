# {NUGET_ID}

Native OpenVINO **{VERSION}** runtime libraries for **{PLATFORM_LABEL}**.

This package bundles the binaries from the upstream Intel release at
[storage.openvinotoolkit.org]({ARCHIVE_URL}) and ships them in the
standard .NET runtime layout: `runtimes/{RID}/native/`. For .NET 5+
consumers the binaries are auto-resolved at build time; for .NET
Framework 4.x consumers the included `build/net46/{NUGET_ID}.props`
copies them to the output directory. The package also includes
`build/netstandard2.0` and empty `lib` placeholders so SDK-style projects
restore it without legacy framework fallback warnings.

本包使用标准 `.NET` runtime 布局：`runtimes/{RID}/native/`。SDK-style
项目通过 `lib/netstandard2.0/_._` 明确兼容性，避免恢复时触发旧版
`.NET Framework` 兼容回退警告；`.NET Framework 4.x` 项目则通过
`build/net46/{NUGET_ID}.props` 显式复制 native 库到输出目录。

This is a companion native package to
[`JYPPX.OpenVINO.CSharp.API`](https://www.nuget.org/packages/JYPPX.OpenVINO.CSharp.API/).
Install both to use OpenVINO from your C# project:

```sh
dotnet add package JYPPX.OpenVINO.CSharp.API
dotnet add package {NUGET_ID}
```

## Provenance

- **Source:** `{ARCHIVE_URL}`
- **SHA-256:** verified against `{ARCHIVE_URL}.sha256` at package time.
- **Upstream release tag:** [`{VERSION}`](https://github.com/openvinotoolkit/openvino/releases/tag/{VERSION})
