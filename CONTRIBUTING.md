# 🏅Contributing to OpenVINOSharp 

&emsp;   Welcome everyone to give us valuable feedback🥰!  We look forward to everyone contributing to OpenVINOSharp by:

- **⁉Report debug / issues**

  If you encounter incorrect behavior in OpenVINOSharp or its components, you can create a new issue in the [GitHub Issue Tracker](https://github.com/guojin-yan/OpenVINOSharp/issues).

- **🔖Propose new products and features**

  If you have any suggestions or want to share your thoughts on OpenVINOSharp, you can open a new GitHub [Discussions](https://github.com/guojin-yan/OpenVINOSharp/discussions/landing) . If your idea is clearly defined, you can also create a feature request question. In both cases, please provide detailed explanations, including use cases, advantages, and potential challenges.

- 🎯**Fix debug or develop new features**

  If you find code errors or other content errors in the warehouse, as well as new features or application case development, you can create [Pull requests](https://github.com/guojin-yan/OpenVINOSharp/pulls) Please ensure that the code style and document style are consistent with the code repository when resubmitting.

- 🕹**Becoming a maintainer**

  If you are interested in OpenVINOSharp and accept the project work, and have the spare time to assist in the development of the OpenVINOSharp library, you can contact me by [guojin_yjs@cumt.edu.cn](mailto:guojin_yjs@cumt.edu.cn)。



## ⭕Submit Pull Request (PR)

&emsp;  It's easy to submit a PR😀!  You can contribute by submitting PR to propose new products and features, and submit code fixes. Here are two ways to submit PR:

### 🔻Method 1: Submit directly online

####   1. Select files to update

&emsp;   Select the update by clicking on it in GitHub, using the ``README. md`` file as an example:

<div align=center><span><img src="https://s2.loli.net/2023/08/26/1SQHsF8UXJkc2df.png" height=500/></span></div>



####  2. Click on 'Edit this file'

&emsp;   This button is located in the upper right corner.

<div align=center><span><img src="https://s2.loli.net/2023/08/26/Z8pWy4hUSGK5EPB.png" height=500/></span></div>

If you do not have the Fork project, you need to Fork the project first.

<div align=center><span><img src="https://s2.loli.net/2023/08/26/Lw7HrquabZRnxv3.jpg" height=500/></span></div>

####   3. Modify file content

&emsp;   Add two more 🥰 Symbols.

<div align=center><span><img src="https://s2.loli.net/2023/08/26/phOdqD85I93stuN.png" height=500/></span></div>

&emsp;   After modifying the file content, click * * Commit changes * * to submit the changes and fill in the log according to the changes.

<div align=center><span><img src="https://s2.loli.net/2023/08/26/PAXH4vzLUNFikBY.png" height=300/></span></div>

####   4. Creat Pull Request

&emsp;   After modifying the file, the modified content only exists in the current branch of the modifier and needs to be submitted to the original author's warehouse through a Pull Request. Click on * * Create pull request * * to create a PR.

<div align=center><span><img src="https://s2.loli.net/2023/08/26/Ki7aloTVCqA9EbP.png" height=500/></span></div>



<div align=center><span><img src="https://s2.loli.net/2023/08/26/u8WFxcjbLI1ozP6.png" height=500/></span></div>

&emsp;    After submitting as required, wait for the code warehouse management personnel to review and approve the PR you submitted.

### 🔻Method 2: Local modification submission

&emsp;   Local modification submission is suitable for situations such as significant changes, addition of new files, debugging code, etc. This method requires installation of Git

#### 1. fork Project

&emsp;   Find the project to submit PR and first fork it into your own code repository.

####  2. Clone open source project

  Clone the project that needs to submit PR locally.

```
git clone https://github.com/guojin-yan/OpenVINOSharp.git
```

#### 3.  Create a new branch

&emsp;   When submitting PR, it is necessary to create a branch here to prevent modifications on the main branch from affecting the main branch code.

```
cd OpenVINOSharp
git checkout -b mybranch 
git branch 
git checkout mybranch
```

&emsp;   After switching the branches, you can directly modify the project according to your own needs, as shown in the above figure.

#### 4. 修改提交项目代码

&emsp;   After modifying the code, execute the ``git status`` command to see which files have been modified. Then use the ``git add`` modified file name 'to add it to the temporary storage area. Finally, use the ``git commit - m 'log information'  file name `` to submit it to the local library.

```
git status 
git add `file name`
git commit -m "log information" `file name`
```

&emsp;   Finally, submit the local project code to the remote GitHub;

```
git push --set-upstream main mybranch
```

&emsp;   Enter the GitHub project, switch to the mybranch branch, and check if the modification was successful.

&emsp;   Switch to the **main branch ** and merge the branch mybranch code into the main branch to see if it can be successfully merged with the main branch.

```
git checkout main 
git merge mybranch 
```

&emsp;   After the merge is successful, push the main branch to the code repository.

```
git add . 
git commit -m "log information" .
git push origin main
```

&emsp;   Switch to the master branch in GitHub and check if the merge was successful

#### 4. Submit PR 

&emsp; Enter your ``fork`` project and click on ``Pull requests``
&emsp; Click on ``Create pull requests``
&emsp; Finally, click on ``Create pull request``, and after submitting, the open-source person will receive your merge request.

## ⭕Coding specification

&emsp;   To ensure consistency in project coding style, when submitting a PR, it is necessary to comply with the project coding specifications.

##### 🔻Code style

&emsp;   All of our code follows the Google Open Source Project Style Guide, including C/C++.

&emsp;   🔸C++ Code style：[English](https://google.github.io/styleguide/cppguide.html)

## ⭕Licence

&emsp;   The contribution you submitted assumes that you agree to adopt the [Apache-2.0 license](https://github.com/PaddlePaddle/Paddle/blob/develop/LICENSE) 