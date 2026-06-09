# OpenVINO GenAI Runtime Package / GenAI 运行时包

This page describes the OpenVINO GenAI runtime NuGet workflow added in csharp3.3.

本文说明 csharp3.3 新增的 OpenVINO GenAI runtime NuGet 自动打包流程。

## Packages / 包

Use the pure OpenVINO runtime package when your application only needs the core OpenVINO C API:

```bash
dotnet add package JYPPX.OpenVINO.CSharp.API
dotnet add package OpenVINO.runtime.win
```

Use the GenAI runtime package when your application calls `OpenVinoSharp.GenAI`:

```bash
dotnet add package JYPPX.OpenVINO.CSharp.API
dotnet add package JYPPX.OpenVINO.GenAI.runtime.win
```

The GenAI runtime package contains OpenVINO Core, OpenVINO GenAI C API, tokenizers, frontends, device plugins, and native runtime dependencies.

GenAI runtime 包包含 OpenVINO Core、OpenVINO GenAI C API、tokenizers、frontends、设备插件和原生运行时依赖。

If an application only uses `Core`, `Model`, `Tensor`, `CompiledModel`, or `InferRequest`, install only the normal `OpenVINO.runtime.*` package. The managed assembly contains GenAI wrappers, but `openvino_genai_c` is loaded only when `OpenVinoSharp.GenAI` APIs are called.

如果应用只使用 `Core`、`Model`、`Tensor`、`CompiledModel` 或 `InferRequest`，只安装普通 `OpenVINO.runtime.*` 包即可。托管程序集虽然包含 GenAI 封装，但只有调用 `OpenVinoSharp.GenAI` API 时才会加载 `openvino_genai_c`。

## GitHub Packaging / GitHub 自动打包

The package is built in GitHub Actions, mirroring the existing pure OpenVINO runtime workflow:

打包流程在 GitHub Actions 中执行，并与已有的纯 OpenVINO runtime 自动打包流程保持一致：

- Workflow: `.github/workflows/update-genai-runtime-packages.yml`
- Discovery script: `.github/scripts/discover_genai.py`
- Pack script: `.github/scripts/build_genai_runtime_nupkg.py`
- Template files: `nuget/runtime/templates/genai.package.*.tmpl`

The workflow discovers official archives from:

```text
https://storage.openvinotoolkit.org/repositories/openvino_genai/packages/
```

For every package build, the script downloads the official archive and the matching `.sha256` sidecar. The archive hash is verified before extraction and packaging. No local runtime directory is used as the formal package source.

每次构建都会下载官方 archive 及其匹配的 `.sha256` 校验文件，校验通过后才会解压并打包。正式打包源不是本地 runtime 目录。

For OpenVINO GenAI 2026.2, the official CDN exposes these packageable platforms:

以 OpenVINO GenAI 2026.2 为例，官方 CDN 提供以下可打包平台：

- `JYPPX.OpenVINO.GenAI.runtime.win`
- `JYPPX.OpenVINO.GenAI.runtime.ubuntu.24-x86_64`
- `JYPPX.OpenVINO.GenAI.runtime.ubuntu.22-x86_64`
- `JYPPX.OpenVINO.GenAI.runtime.ubuntu.22-arm64`
- `JYPPX.OpenVINO.GenAI.runtime.rhel8-x86_64`
- `JYPPX.OpenVINO.GenAI.runtime.macos-arm64`

If an older or newer official release contains more platform archives, such as `macos-x86_64`, the workflow will include them automatically.

如果旧版或新版官方 release 包含更多平台 archive，例如 `macos-x86_64`，workflow 会自动纳入矩阵。

Manual dry-run example:

```powershell
$env:PKG_ID = "win"
$env:PKG_VERSION = "<version>"
$env:ARCHIVE_URL = "https://storage.openvinotoolkit.org/repositories/openvino_genai/packages/<version-dir>/windows/openvino_genai_windows_<version>.0_x86_64.zip"
$env:SHA256_URL = "$env:ARCHIVE_URL.sha256"
$env:RID = "win-x64"
$env:KIND = "zip"
$env:DRY_RUN = "true"

python .github/scripts/build_genai_runtime_nupkg.py
```

The workflow can also be started manually from GitHub with an optional version input. When `dry_run` is enabled, it builds artifacts but skips publishing and GitHub Release creation.

也可以在 GitHub 页面手动触发 workflow，并可指定版本。启用 `dry_run` 时只构建 artifact，不执行 NuGet 发布，也不会创建 GitHub Release。

## File Layout / 文件布局

The GenAI runtime package uses the same NuGet layout as the core runtime package:

GenAI runtime 包使用与基础 runtime 包一致的 NuGet 布局：

```text
runtimes/<rid>/native/<native libraries>
build/net/<package id>.props
lib/net/_._
LICENSE.txt
README.md
logo.jpg
```

Windows packages include files such as:

Windows 包会包含以下文件：

- `openvino_genai_c.dll`
- `openvino_genai.dll`
- `openvino_tokenizers.dll`
- `openvino.dll`
- `openvino_c.dll`
- `openvino_*_plugin.dll`
- `openvino_*_frontend.dll`
- `tbb12.dll`
- `tbbbind_2_5.dll`
- `tbbmalloc.dll`
- `tbbmalloc_proxy.dll`

Debug libraries are excluded. Native libraries are packaged as flat runtime assets so platform loader dependency resolution can find them from the same native directory.

Debug 库不会进入包。原生动态库会平铺放入 native 目录，便于平台加载器从同一目录解析依赖。

## Loading / 加载

For .NET 5+, NuGet restores runtime assets under `runtimes/<rid>/native`; `GenAINativeLibraryLoader` searches that layout automatically.

对于 .NET 5+，NuGet 会把运行时资产还原到 `runtimes/<rid>/native`；`GenAINativeLibraryLoader` 会自动搜索该布局。

For .NET Framework 4.x, the package includes `build/net/<package id>.props`, which copies native files to the output directory.

对于 .NET Framework 4.x，包内包含 `build/net/<package id>.props`，用于将原生文件复制到输出目录。

Manual fallback for local development:

```csharp
OpenVinoSharp.GenAI.GenAI.Initialize(
    @"E:\OpenVINOSharp\openvino\openvino_genai_windows_2026.2.0.0_x86_64\runtime\bin\intel64\Release\openvino_genai_c.dll");
```

Or set:

```powershell
$env:OPENVINO_GENAI_RUNTIME_DIR = "E:\OpenVINOSharp\openvino\openvino_genai_windows_2026.2.0.0_x86_64"
```

The fallback is only for development and diagnostics. The NuGet runtime package should be produced from the official archive URL in GitHub Actions.

上述 fallback 仅用于本地开发和诊断。正式 NuGet runtime 包应由 GitHub Actions 从官方 archive URL 生成。

## Diagnostics / 诊断

If loading fails, check:

- `openvino_genai_c` exists under `runtimes/<rid>/native`.
- Runtime dependencies exist in the same native directory.
- The process architecture matches the installed runtime package.
- The package version matches the GenAI C API wrapper version used by the managed library.
- The exception message includes searched paths and native load errors.
