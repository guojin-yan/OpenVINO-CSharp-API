# 🏅为 OpenVINO™ C# API 做贡献

&emsp;   欢迎大家对我们提出宝贵意见🥰！我们期待大家为 OpenVINO™ C# API 做出贡献，可以通过以下方式：

- **⁉报告错误/问题**

  如果您在 OpenVINO™ C# API 或其组件中遇到错误行为，您可以在 GitHub 问题跟踪器中[创建新问题](https://github.com/guojin-yan/OpenVINOSharp/issues)。

- **🔖提出新的产品和功能**

  如果你对 OpenVINO™ C# API 有相关建议或想分享您的想法，您可以打开一个新的[ Discussions](https://github.com/guojin-yan/OpenVINOSharp/discussions/landing) GitHub 讨论。 如果您的想法已经明确定义，您还可以创建一个功能请求问题，在这两种情况下，请提供详细说明，包括用例、优势和潜在挑战。

- 🎯**修复代码错误或开发新功能**

  如果您发现仓库中有代码错误或则其他内容错误，以及有新的功能或者应用案例开发，可以通过创建 [Pull requests](https://github.com/guojin-yan/OpenVINOSharp/pulls)实现，再提交时，请注意代码风格以及文档风格与代码仓保持一致。

- 🕹**成为维护者**

  如果你对 OpenVINO™ C# API 感兴趣，并接受该项目工作，有余力协助 OpenVINO™ C# API 库开发，可以与我联系[guojin_yjs@cumt.edu.cn](mailto:guojin_yjs@cumt.edu.cn)。



## ⭕提交拉取请求 (PR)

&emsp;   提交 PR 很容易😀！你可以通过提交pr提出新的产品和功能、提交代码修复等贡献，此处演示两种提交PR的方式：

### 🔻方式一：在线直接提交

####   1. 选择要更新的文件

&emsp;   通过在 GitHub 中单击它来选择更新，以`README.md`文件为例：

<div align=center><span><img src="https://s2.loli.net/2023/08/26/1SQHsF8UXJkc2df.png" height=500/></span></div>



####  2. 点击“编辑此文件”

&emsp;   该按钮位于右上角。

<div align=center><span><img src="https://s2.loli.net/2023/08/26/Z8pWy4hUSGK5EPB.png" height=500/></span></div>

如果你没有Fork该项目，需要先Fork该项目。

<div align=center><span><img src="https://s2.loli.net/2023/08/26/Lw7HrquabZRnxv3.jpg" height=500/></span></div>

####   3. 修改文件内容

&emsp;   增加两个🥰符号。

<div align=center><span><img src="https://s2.loli.net/2023/08/26/phOdqD85I93stuN.png" height=500/></span></div>

&emsp;   修改完文件内容后，点击**Commit changes**提交更改，并按照更改内容填写日志。

<div align=center><span><img src="https://s2.loli.net/2023/08/26/PAXH4vzLUNFikBY.png" height=300/></span></div>

####   4. 创建 Pull Request

&emsp;   修改完该文件后，修改内容只存在于修改者当前分支，需要通过 Pull Request 提交到原作者仓库才可以。点击 **Create pull request**，创建PR。

<div align=center><span><img src="https://s2.loli.net/2023/08/26/Ki7aloTVCqA9EbP.png" height=500/></span></div>



<div align=center><span><img src="https://s2.loli.net/2023/08/26/u8WFxcjbLI1ozP6.png" height=500/></span></div>

&emsp;    按要求提交后，等待代码仓库管理人员审核并通过你提交的PR。

### 🔻方式二：本地修改提交

&emsp;   本地修改提交适合较大的改动或增加新文件、调试代码等情况，该方法要求按装Git.

#### 1. fork开源项目

&emsp;   找到要提交PR的项目，先将该项目fork自己的代码仓。

####  2. 克隆开源项目

  将需要提交PR的项目克隆到本地。

```
//打开CMD或者打开Git Bash Here
git clone https://github.com/guojin-yan/OpenVINOSharp.git
```

#### 3.创建新的分支

&emsp;   提交PR时需要.为了防止在主分支上修改影响主分支代码，此处创建一个分支用于代码的修改。

```
cd PaddleGame // 切换到项目路径
git checkout -b mybranch //创建名为mybranch的分支
git branch //查看已经创建的分支 如图有mybranch和main两个分支
git checkout mybranch // 切换到分支
```

&emsp;   切换好分支后就可以直接根据自己需求修改项目,如上图所示。

#### 4. 修改提交项目代码

&emsp;   将代码修改后，执行`git status` 命令查看修改了哪些文件，接着使用`git add 修改的文件名`添加到暂存区，最后使用`git commit -m "日志信息" 文件名`提交到本地库。

```
git status // 查看库状态
git add 文件名 // 将修改的文件存放到暂存区
git commit -m "日志信息" 文件名 // 将修改的文件提交到本地库
```

&emsp;   最后将本地项目代码提交到远程GitHub上

```
git push --set-upstream main mybranch
```

&emsp;   进入GitHub项目，切换到mybranch分支，查看是否修改成功。

&emsp;   切换到**主分支**，将分支mybranch代码合并到主分支，查看是否可以与主分支合并成功。

```
git checkout main // 切换到主分支
git merge mybranch  // 合并派生分支到主分支
```

&emsp;   合并成功后，将主分支推送到代码仓。

```
git add .  // 将修改的文件存放到暂存区
git commit -m "日志信息" // 将修改的文件提交到本地库
git push origin main // 推送到远程仓库
```

&emsp;   在GitHub切换到master主分支，查看是否合并成功

#### 4.提交pr请求

&emsp;   进入自己`fork`的项目中，点击`Pull requests`

&emsp;   点击`Create pull requests`

&emsp;   最后点击`Create pull request`，提交后开源人将会收到你的合并请求。

## ⭕编码规范

&emsp;   为保证项目编码风格一致，在提交PR时，要遵守该项目编码规范。

##### 🔻代码样式

&emsp;   我们的所有代码遵循Google 开源项目风格指南，包括C/C++。

&emsp;   🔸C++ 风格指南：[English](https://google.github.io/styleguide/cppguide.html)

## ⭕许可证

&emsp;   您所提交的贡献，默认您同意采用[Apache-2.0 license](https://github.com/PaddlePaddle/Paddle/blob/develop/LICENSE).