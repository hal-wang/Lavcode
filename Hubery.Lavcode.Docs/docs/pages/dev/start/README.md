---
title: 开始运行
---

本项目遵循 MIT 开源许可协议，您可以在自己的项目中自由使用部分或全部功能，但需注明[来源](https://github.com/hbrwang/Lavcode)

## 获取代码

### GitHub

- git clone https://github.com/hbrwang/Lavcode.git
- 转至[GitHub](https://github.com/hbrwang/Lavcode)

### 码云

国内用户推荐使用码云，速度更快

- git clone https://gitee.com/hbrwang/Lavcode.git
- 转至[Gitee](https://gitee.com/hbrwang/Lavcode)

## 运行环境

1. Visual Studio 2019（包含 UWP 开发环境）
2. Windows 10 SDK (10.0.18362)

## 跑起来

下载本项目后，使用 vs 2019 打开 `Hubery.Lavcode.Uwp.sln` 文件，编译并运行。

## 目录结构

```
├── Assets
│   ├── Images
│   │   ├── Visual（UWP中的图标资源）
│   │   ├── *.jpg/png（其他图片）
├── Controls （可复用控件，后面详细介绍）
│   ├── Comment
│   ├── IconControl
│   ├── ...
├── Helpers 帮助类
│   ├── Sqlite（操作Sqlite的帮助类）
│   ├── ...
├── Model （一些Model类）
│   ├── Folder
│   ├── Password
│   ├── ...
├── Resources （样式资源）
│   ├── Colors.xaml
│   ├── ...
├── View （展现内容主要都在这里，后面详细介绍）
│   ├── FolderList
│   ├── PasswordDetail
│   ├── PasswordList
│   ├── ...
├── App（UWP的入口）
├── Global.cs（会被全局调用的静态类）
├── Program.cs（App的入口，包含Main函数，主要在这里实现的多实例应用）
```

::: tip
虽然本项目尽量遵守 MVVM 设计模式，使用了 MVVMLight，但我受 vue 影响，不喜欢严格按照 UWP、WPF 的 MVVM 目录格式，而习惯于将组件模块化放在一起。所以对于 MVVM 初学者，可能会被带偏。**目前我还不知道哪种更好，希望有大佬给解个惑。**
:::
