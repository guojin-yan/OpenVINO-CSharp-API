# OpenVINO GenAI Runtime Package / GenAI 运行时包

This page describes the OpenVINO GenAI runtime NuGet workflow added in `csharp3.3`.

本文说明 `csharp3.3` 新增的 OpenVINO GenAI runtime NuGet 自动打包流程。

## Packages / 包选择

Use the normal OpenVINO runtime package when an application only calls the core OpenVINO APIs:

如果应用只调用基础 OpenVINO API，请安装普通 OpenVINO runtime 包：

```bash
dotnet add package JYPPX.OpenVINO.CSharp.API
dotnet add package OpenVINO.runtime.win
```

Install a GenAI runtime package only when the application calls `OpenVinoSharp.GenAI` APIs:

只有应用调用 `OpenVinoSharp.GenAI` API 时，才需要安装 GenAI runtime 包：

```bash
dotnet add package JYPPX.OpenVINO.CSharp.API
dotnet add package JYPPX.OpenVINO.GenAI.runtime.win
```

The managed API assembly can contain both core and GenAI wrappers. Core APIs such as `Core`, `Model`, `Tensor`, `CompiledModel`, and `InferRequest` do not load `openvino_genai_c`.

托管 API 程序集可以同时包含基础 OpenVINO 与 GenAI 封装。`Core`、`Model`、`Tensor`、`CompiledModel`、`InferRequest` 等基础 API 不会加载 `openvino_genai_c`。

## GitHub Packaging / GitHub 自动打包

The GenAI runtime package is built in GitHub Actions, using the same packaging model as the core OpenVINO runtime package.

GenAI runtime 包在 GitHub Actions 中构建，打包模型与基础 OpenVINO runtime 包保持一致。

- Workflow: `.github/workflows/update-genai-runtime-packages.yml`
- Discovery script: `.github/scripts/discover_genai.py`
- Pack script: `.github/scripts/build_genai_runtime_nupkg.py`
- Templates: `nuget/runtime/templates/genai.package.*.tmpl`

The workflow discovers official archives from:

workflow 会从以下官方地址发现并下载 archive：

```text
https://storage.openvinotoolkit.org/repositories/openvino_genai/packages/
```

Each build downloads the official archive and the matching `.sha256` file, verifies the hash, extracts the archive, then builds the NuGet package. The discovery step reads the official file tree and also probes deterministic archive URLs because some OpenVINO GenAI archives can be available on storage before they appear in `filetree.json`. The formal GitHub workflow does not package a local runtime directory.

每次构建都会下载官方 archive 和匹配的 `.sha256` 文件，校验哈希后再解压并打包。发现步骤会读取官方 file tree，并额外探测确定性 archive URL，因为部分 OpenVINO GenAI archive 可能已经存在于 storage，但尚未出现在 `filetree.json` 中。正式 GitHub workflow 不使用本地 runtime 目录作为打包源。

For OpenVINO GenAI 2026.3.0, the workflow builds the packages that have both an official archive and a valid `.sha256` sidecar:

以 OpenVINO GenAI 2026.3.0 为例，workflow 只会构建同时具备官方 archive 和有效 `.sha256` 校验文件的平台包：

- `JYPPX.OpenVINO.GenAI.runtime.win`
- `JYPPX.OpenVINO.GenAI.runtime.ubuntu.24-x86_64`
- `JYPPX.OpenVINO.GenAI.runtime.ubuntu.22-x86_64`
- `JYPPX.OpenVINO.GenAI.runtime.ubuntu.22-arm64`
- `JYPPX.OpenVINO.GenAI.runtime.rhel8-x86_64`
- `JYPPX.OpenVINO.GenAI.runtime.macos-arm64`

OpenVINO GenAI 2026.3.0 still returns an HTML directory page instead of a checksum sidecar for the Ubuntu 20.04 and macOS x86_64 GenAI archives, so those platforms are intentionally skipped. This differs from the core OpenVINO runtime package set because the upstream GenAI release artifacts are different.

OpenVINO GenAI 2026.3.0 当前在 Ubuntu 20.04 和 macOS x86_64 GenAI archive 的 `.sha256` 地址上返回 HTML 目录页，而不是校验文件，因此这些平台会被有意跳过。它与基础 OpenVINO runtime 包数量不同，原因是官方 GenAI 发布物本身不同。

If a future official release adds more platform archives with valid `.sha256` sidecars, the discovery script can include them automatically.

如果未来官方 release 增加更多平台 archive 并提供有效 `.sha256` 校验文件，发现脚本可以自动纳入。

Manual dry-run example:

手动 dry-run 示例：

```powershell
gh workflow run update-genai-runtime-packages.yml --ref csharp3.3 -f dry_run=true -f force_republish=true
```

When `dry_run=true`, the workflow uploads artifacts but skips NuGet publishing and GitHub Release creation.

当 `dry_run=true` 时，workflow 只上传 artifacts，不执行 NuGet 发布，也不会创建 GitHub Release。

## File Layout / 文件布局

The GenAI runtime package intentionally uses the same NuGet layout as the core runtime package:

GenAI runtime 包刻意使用与基础 runtime 包一致的 NuGet 布局：

```text
runtimes/<rid>/native/<native libraries>
build/net46/<package id>.props
build/netstandard2.0/<package id>.props
lib/net46/_._
lib/netstandard2.0/_._
licenses/**
manifest.txt
LICENSE.txt
README.md
logo.jpg
```

Windows packages include GenAI libraries, tokenizers, OpenVINO core libraries, plugins, frontends, and release TBB libraries, for example:

Windows 包会包含 GenAI 库、tokenizers、OpenVINO 基础库、插件、frontends 和 release TBB 库，例如：

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

Debug libraries and duplicated nested directories such as `runtimes/runtimes/**` or `build/build/**` are excluded.

Debug 库以及 `runtimes/runtimes/**`、`build/build/**` 这类重复嵌套目录不会进入包。

## Loading / 加载

For SDK-style projects, NuGet restores runtime assets under `runtimes/<rid>/native`, and the GenAI loader searches that layout automatically.

对于 SDK 风格项目，NuGet 会把 runtime assets 还原到 `runtimes/<rid>/native`，GenAI loader 会自动搜索该布局。

For .NET Framework projects, the package includes `build/net46/<package id>.props` to copy native files into the output directory.

对于 .NET Framework 项目，包内包含 `build/net46/<package id>.props`，用于将 native 文件复制到输出目录。

For local development and diagnostics, you may also set:

本地开发和诊断时，也可以设置：

```powershell
$env:OPENVINO_GENAI_RUNTIME_DIR = "D:\local-runtimes\openvino_genai"
```

or initialize with an explicit library path:

或者使用显式库路径初始化：

```csharp
OpenVinoSharp.GenAI.GenAI.Initialize(
    @"D:\local-runtimes\openvino_genai\runtime\bin\intel64\Release\openvino_genai_c.dll");
```

The fallback paths above are only for development and diagnostics. NuGet runtime packages should be produced from official archives in GitHub Actions.

以上 fallback 路径仅用于本地开发和诊断。NuGet runtime 包应由 GitHub Actions 从官方 archive 生成。

## Diagnostics / 诊断

If loading fails, check:

如果加载失败，请检查：

- `openvino_genai_c` exists under `runtimes/<rid>/native`.
- Runtime dependencies exist in the same native directory.
- The process architecture matches the installed runtime package.
- The GenAI runtime package version matches the OpenVINO GenAI native version expected by this wrapper, for example 2026.3.x for the csharp3.3 release. / GenAI runtime 包版本应匹配当前封装期望的 OpenVINO GenAI 原生版本，例如 csharp3.3 对应 2026.3.x。
- The exception message includes searched paths and native load errors.
