# NuGet Trusted Publishing 配置

## 本次变更

2026-11-01：托管 API 和 Core/GenAI runtime 的 NuGet.org 发布改用 GitHub Actions
OIDC / Trusted Publishing，停止读取长期 `secrets.NUGET_API_KEY`。
本次仅修改发布认证，不更改 API 包版本、OpenVINO 对齐版本或已有包的兼容性。

`NuGet/login@v1` 在包下载完成后、上传开始前请求临时凭据，临时凭据有效期为一小时。
发布脚本仍通过 `NUGET_API_KEY` 环境变量接收该凭据；这个名字是临时输出的传递方式，
不再表示需要创建或更新同名 GitHub Secret。

| 发布内容 | 工作流 | 包范围 |
| --- | --- | --- |
| 托管 API | `.github/workflows/publish-api-nuget.yml` | `JYPPX.OpenVINO.CSharp.API` |
| Core 和 GenAI runtime | `.github/workflows/update-runtime-packages.yml` | `OpenVINO.runtime.*`、`JYPPX.OpenVINO.GenAI.runtime.*` |

## 1. 在 GitHub 配置变量和 Environment

仓库：<https://github.com/guojin-yan/OpenVINO-CSharp-API>。

在 **Settings → Secrets and variables → Actions → Variables** 添加仓库变量：

| 变量 | 值 |
| --- | --- |
| `NUGET_USER` | `GuojinYan` |

`GuojinYan` 是 NuGet.org 的账号 / profile 名称，三个包系列的公开 owner 均为该账号。
这里不能填写 GitHub 用户名 `guojin-yan`、邮箱或 API Key。
如果未来改由另一个个人账号维护，请填写创建信任策略、并具有相应包发布权限的实际 NuGet 用户名。

也可使用 GitHub CLI 设置该公开变量：

```powershell
gh variable set NUGET_USER --repo guojin-yan/OpenVINO-CSharp-API --body GuojinYan
```

在 **Settings → Environments** 创建名称为 **`nuget`** 的 Environment。
两条工作流的 `publish` job 均声明了 `environment: nuget`。

- 在 **Deployment branches and tags** 中选择 **Selected branches and tags**；
- 添加 **Branch** 规则 `csharp3.3`；
- 需要 runtime 每周自动发布时，不设置必须人工批准的 reviewer；如设置 reviewer，发布会等待批准。

工作流也只允许从 `refs/heads/csharp3.3` 执行发布。
`contents: read`、`packages: write` 和 `id-token: write` 权限仅分配给发布 job；
打包 job 不需要 OIDC 凭据。

## 2. 在 NuGet.org 创建两条信任策略

使用 `GuojinYan` 登录 <https://www.nuget.org>，在账号菜单打开
**Trusted Publishing**，创建下面两条策略。

| 页面字段 | 托管 API 策略 | Core/GenAI runtime 策略 |
| --- | --- | --- |
| Policy Name（可自行命名） | `openvino-csharp-api` | `openvino-runtime-packages` |
| Package Owner | `GuojinYan` | `GuojinYan` |
| Publisher | GitHub Actions | GitHub Actions |
| Repository Owner | `guojin-yan` | `guojin-yan` |
| Repository | `OpenVINO-CSharp-API` | `OpenVINO-CSharp-API` |
| Workflow File | `publish-api-nuget.yml` | `update-runtime-packages.yml` |
| Environment | `nuget` | `nuget` |

**Workflow File 只填文件名，不要包含 `.github/workflows/` 前缀，也不要填写工作流的显示名称。**
两条策略的 Environment 都必须填写 `nuget`，与 GitHub job 的名称完全一致。

在 **Select Scopes** 中启用 **Push**。托管 API 可仅允许已有包的新版本；runtime 建议允许
**Push new packages and package versions**，以覆盖新增平台包。发布流程不需要 Unlist 等权限。

托管 API 策略的 **Glob Patterns and Packages** 填写：

```text
JYPPX.OpenVINO.CSharp.API
```

runtime 策略在同一个输入框中**分成两行**填写：

```text
OpenVINO.runtime.*
JYPPX.OpenVINO.GenAI.runtime.*
```

这些前缀覆盖现有和新增平台，且不会将发布范围扩大到账号下的所有其他包。
如果 NuGet.org 将策略显示为暂时激活，应按页面提示在有效期内使用；
暂时激活窗口通常为七天，可在页面上重新激活。

## 3. 工作流行为

1. 解析版本、构建并上传 GitHub Actions artifacts。
2. 发布 job 进入 `nuget` Environment，检查 `NUGET_USER`。
3. 下载包文件后执行 `NuGet/login@v1`，将 GitHub OIDC token 换成临时 NuGet 凭据。
4. `.github/scripts/publish.sh` 使用临时凭据向 NuGet.org 上传包；
   继续使用 GitHub 内置 `GITHUB_TOKEN` 同步到 GitHub Packages。
5. 两个源都发布成功后，后续 job 才会创建或更新 GitHub Release。

上游仓库必须具有 NuGet 临时凭据，缺失配置或交换失败会终止发布，
不能仅凭 GitHub Packages 上传成功创建正式 Release。
空 artifacts 目录同样按失败处理；`--skip-duplicate` 继续支持安全重跑。

Fork 未设置 `NUGET_USER` 时，仍可只使用 `GITHUB_TOKEN` 发布到自身 GitHub Packages。
Fork 若要发布到 NuGet.org，需要使用自己的 NuGet 账号、策略和 Environment 分支规则。

`dry_run=true` 时只构建 / 上传 artifacts，不执行 OIDC 登录、NuGet 上传和 GitHub Release。
runtime 若指定的版本已有 release tag，需要同时设置 `force_republish=true` 才会重新构建。
`force_republish` 不会覆盖 NuGet.org 上的已有包版本。

## 4. 验证与迁移

先完成 GitHub 变量、Environment 和 NuGet.org 两条策略的配置，再将工作流变更合入
`csharp3.3`。本次不修改 `.csproj` 版本，因此不会自动触发托管 API 发布；
runtime 工作流仍保留每周定时和手动触发入口。

可先以 `dry_run=true` 手动运行工作流验证打包。**dry run 不验证 OIDC 策略**；
实际运行发布 job 才能验证身份交换和 NuGet.org 的授权。

正式发布时检查 Actions 中的 **NuGet login with GitHub OIDC** 与 **Publish** 步骤，
并核对 NuGet.org 对应包版本和 GitHub Release。
如果临时凭据过期，重新运行发布以获取新凭据；不要保存或复制临时凭据到仓库 Secret。
确认迁移成功后，可以删除已过期的 `NUGET_API_KEY` Secret，新工作流不会再读取它。

发布脚本的回归测试完全使用模拟 `dotnet` 和假凭据，不会上传任何包：

```bash
python3 .github/scripts/test_publish.py
```

## 常见配置错误

| 现象 | 检查项 |
| --- | --- |
| 提示缺少 `NUGET_USER` | 创建 **Repository variable**，不是同名 Secret；值为 `GuojinYan` |
| NuGet login 找不到匹配策略 | 核对 Repository Owner、Repository、Workflow File、Environment 和 NuGet 用户名 |
| 无法获取 GitHub OIDC token | 核对发布 job 的 `id-token: write` 权限 |
| 等待 Environment 批准或被分支规则拦截 | 检查 `nuget` 的 reviewer 和 `csharp3.3` Branch 规则 |
| API 包成功而 runtime 包授权失败 | runtime 策略需要同时包含两行包前缀，并允许所需的 Push scope |
| dry run 成功而正式发布失败 | dry run 跳过了身份交换；需单独核对 NuGet.org 策略 |

## English Summary

Both publishing workflows now use `NuGet/login@v1` and GitHub OIDC instead of a
long-lived `NUGET_API_KEY` repository secret. Set the **repository variable**
`NUGET_USER=GuojinYan` and create the GitHub Environment **`nuget`**, restricted
to the `csharp3.3` branch.

Create two NuGet.org GitHub Actions policies for repository owner **`guojin-yan`**
and repository **`OpenVINO-CSharp-API`**, with package owner **`GuojinYan`** and
Environment **`nuget`**:

- **`publish-api-nuget.yml`**: package `JYPPX.OpenVINO.CSharp.API`.
- **`update-runtime-packages.yml`**: patterns `OpenVINO.runtime.*` and
  `JYPPX.OpenVINO.GenAI.runtime.*`, entered on separate lines.

Use only the workflow filename in each policy. Enable the appropriate Push
scope; the runtime workflow needs permission to publish new platform packages.
The login action runs immediately before uploading and returns a temporary
one-hour credential. NuGet.org failure prevents release creation. Forks without
`NUGET_USER` can still publish to their own GitHub Packages feed.

A dry run does not request OIDC credentials and therefore cannot validate the
trust policy. Confirm a real publishing run succeeds before removing the old
expired secret. This maintenance change does not change any package version.

## 官方参考

- [NuGet.org Trusted Publishing](https://learn.microsoft.com/nuget/nuget-org/trusted-publishing)
- [NuGet/login 官方 Action](https://github.com/NuGet/login)
