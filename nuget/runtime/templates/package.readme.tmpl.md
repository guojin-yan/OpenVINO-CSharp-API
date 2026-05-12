# {NUGET_ID}

Native OpenVINO **{VERSION}** runtime libraries for **{PLATFORM_LABEL}**.

This package bundles the binaries from the upstream Intel release at
[storage.openvinotoolkit.org]({ARCHIVE_URL}) and ships them in the
standard .NET runtime layout: `runtimes/{RID}/native/`. For .NET 5+
consumers the binaries are auto-resolved at build time; for .NET
Framework 4.x consumers the included `build/net/{NUGET_ID}.props`
copies them to the output directory.

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
