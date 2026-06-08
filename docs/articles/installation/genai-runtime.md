# OpenVINO GenAI Runtime Package / GenAI 运行时包

This page describes the Windows OpenVINO GenAI runtime NuGet workflow added in csharp3.3 Phase 3.

本文说明 csharp3.3 Phase 3 新增的 Windows OpenVINO GenAI runtime NuGet 自动打包流程。

## Packages / 包

Use the pure OpenVINO runtime package when your application only needs the core OpenVINO C API:

```bash
dotnet add package JYPPX.OpenVINO.CSharp.API
dotnet add package OpenVINO.runtime.win
```

Use the GenAI runtime package when your application uses `OpenVinoSharp.GenAI`:

```bash
dotnet add package JYPPX.OpenVINO.CSharp.API
dotnet add package JYPPX.OpenVINO.GenAI.runtime.win
```

The GenAI runtime package contains OpenVINO Core, OpenVINO GenAI C API, tokenizers, frontends, device plugins, and TBB runtime dependencies.

GenAI runtime 包包含 OpenVINO Core、OpenVINO GenAI C API、tokenizers、frontends、设备插件和 TBB 运行时依赖。

## GitHub Packaging / GitHub 自动打包

The package is built in GitHub Actions, mirroring the existing pure OpenVINO runtime workflow:

打包流程在 GitHub Actions 中执行，与已有的纯 OpenVINO runtime 自动打包流程保持一致：

- Workflow: `.github/workflows/update-genai-runtime-packages.yml`
- Discovery script: `.github/scripts/discover_genai.py`
- Pack script: `.github/scripts/build_genai_runtime_nupkg.py`
- Template files: `nuget/runtime/templates/genai.package.*.tmpl`

The workflow discovers official archives from:

```text
https://storage.openvinotoolkit.org/repositories/openvino_genai/packages/
```

For every package build, the script downloads:

- The official `openvino_genai_windows_<version>_x86_64.zip` archive.
- The matching `.sha256` sidecar.

The archive hash is verified before extraction and packaging. No local runtime directory is used as the package source.

每次构建都会下载官方 `openvino_genai_windows_<version>_x86_64.zip` 归档及其 `.sha256` 校验文件，校验通过后才会解压并打包。打包源不是本地 runtime 目录。

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

The workflow can also be started manually from GitHub with an optional version input. When `dry_run` is enabled, it builds artifacts but skips publishing.

也可以在 GitHub 页面手动触发 workflow，并可指定版本。启用 `dry_run` 时只构建 artifact，不执行发布。

## File Layout / 文件布局

The package places runtime files in:

```text
runtimes\win-x64\native
```

Important files include:

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
- `cache.json`

Debug DLLs are excluded. Release DLLs and TBB release dependencies are packaged as flat native assets so Windows DLL dependency resolution can find them from the same directory.

Debug DLL 不进入包。Release DLL 和 TBB release 依赖以平铺方式放入 native 目录，便于 Windows DLL 依赖解析。

License and third-party notice files are placed under:

```text
licenses\
```

## Loading / 加载

For .NET 5+, NuGet restores runtime assets under `runtimes/win-x64/native`; `GenAINativeLibraryLoader` searches that layout automatically.

For .NET Framework 4.x, the package includes `build/JYPPX.OpenVINO.GenAI.runtime.win.props`, which copies native files to `dll/win-x64`.

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

- `openvino_genai_c.dll` exists under `runtimes/win-x64/native`.
- TBB DLLs exist in the same native directory.
- The process is x64.
- The package version matches the GenAI C API wrapper version used by the managed library.
- The exception message includes searched paths and Win32 load errors.
