# Lavcode —— 完全开源免费的密码管理软件

- [主页](https://lavcode.hubery.wang)
- [GitHub 介绍](https://github.com/hbrwang/Lavcode)
- [Gitee 介绍](https://gitee.com/hbrwang/Lavcode)（推荐国内用户）

本项目遵循 MIT 开源许可协议，您可以在自己的项目中自由使用，但需注明[来源](https://github.com/hbrwang)

## 一　　获取代码

1. GitHub

- git clone https://github.com/hbrwang/Lavcode.git
- 转至[GitHub](https://github.com/hbrwang/Lavcode)

2. 码云

- git clone https://gitee.com/hbrwang/Lavcode.git
- 转至[Gitee](https://gitee.com/hbrwang/Lavcode)

## 二　　运行环境

1. Visual Studio 2019
2. Windows 10 SDK (10.0.18362)

## 三　　基础库

基础库是本人收集总结的各种帮助类、自定义控件等。比如以下功能：

1. 自定义 UWP 弹窗。系统支持的 ContentDialog 只能同时存在一个，而且关闭时机不能自由控制（比如异步情况，在按钮事件中，args.Cancel 可能无效）
2. 自定义消息提示。目前分 Info/Primary/Warning/Danger 四种，有不同背景色和字体色。可以在顶部以列表形式显示，也可在控件附近显示
3. ...

更多信息请转至 [Hubery.Common](https://github.com/hbrwang/Common)

## 四　　跑起来

下载基础库和本项目代码，目录结构如下所示：

```
├── Hubery
│   ├── Common (基础库)
│   │   ├── Base
│   │   ├── Uwp
│   │   ├── ...
│   ├── Lavcode
│   │   ├── Uwp (本项目)
```

**首次运行时，目录结构及目录名称不能错，否则会找不到**

使用 vs 2019 打开本项目中的 `Hubery.Lavcode.Uwp.sln` 文件，编译并运行。

## 五　　目录结构

虽然本项目尽量遵守 MVVM 设计模式，而且使用了 MVVMLight，但作者受 vue 影响，不喜欢严格按照 UWP、WPF 的 MVVM 目录格式，而习惯于将组件模块化放在一起。所以对于 MVVM 初学者，可能会被带偏。_目前我还不知道哪种更好，希望有大佬给解个惑_。

```
├── Assets
│   ├── Images
│   │   ├── Visual（UWP中的图标资源）
│   │   ├── *.jpg/png（其他图片）
├── Components （模块化组件，后面详细介绍）
│   ├── Comment
│   ├── IconControl
│   ├── ...
├── Helpers 帮助类（很多帮助类都在基础库中）
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

## 六　　页面整体布局介绍

主页面如下图所示

![主页面](./ReadmeImgs/main.png)

主页面主要由以下四部分组成

![四部分](./ReadmeImgs/main_division.png)

1. 文件夹列表
2. 密码列表
3. 密码内容，编辑和查看复用同一部分
4. 菜单

这四部分，123 都在 View 文件夹中，4 菜单也会在验证页面中使用，因此 4 在 Components 文件夹中。
<br>

### 6.1 　　各部分的逻辑关系

4 和 123 没什么联系，主要是 123 直接的逻辑。我使用了 MVVMLight 的 Messenger，使各部分处于弱连接状态。下面说几条主要逻辑关系。

#### 6.1.1 　　选中文件夹

选中某个文件夹时，会发出消息

```
Messenger.Default.Send(
  index < FolderItems.Count ? FolderItems[index] : null
  , "FolderSelected"); // index是选中的0基文件夹序号
```

2 和 3 同时注册 Messenger，以监听文件夹选择

```
Messenger.Default.Register<FolderItem>(this, "FolderSelected", FolderSelected);
```

因此选中文件夹时，能够触发以下操作：

1. 在 3 中，会记录选择的文件夹 ID，用于编辑或新建密码。
2. 在 2 中，会根据选中的文件夹，查询该文件夹下的密码，并展现在列表中。

#### 6.1.2 　　选中密码

如果选中密码列表中的一个密码，会发出消息

```
Messenger.Default.Send(passwordItem, "PasswordSelectedChanged");
```

这个`"PasswordSelectedChanged"`消息已被 3 注册，3 接收到后会展现密码详情。
<br>
其实在选中文件夹时，2 也会发送这个消息，但`passwordItem`是空值，3 接收到空消息会清空当前显示，从而实现选中文件夹即清空密码详情部分。

#### 6.1.3 　　保存密码

添加或编辑密码，保存成功后会发出消息

```
Messenger.Default.Send(newPassword, "PasswordAddOrEdited");
```

在 2 中注册了这个消息，收到这个消息会在列表中添加一条记录，此时接收到的密码，已经成功保存在数据库中，因此只需在界面展现即可。

#### 6.1.4 　　在密码列表操作密码

在 2 中做以下操作，会生成添加密码的消息通知

- 点下方“添加”
- 点右键菜单的“添加”

  ![添加密码](./ReadmeImgs/add_pswd.png)

```
Messenger.Default.Send<object>(null, "AddNewPassword");
```

3 会保存正在编辑的，保存成功后新建一条空内容。

#### 6.1.5 　　在密码详情页删除密码

在 3 中删除密码，会生成消息通知

```
Messenger.Default.Send(password.Id, "PasswordDeleted");
```

![添加密码](./ReadmeImgs/delete_pswd.png)

2 接收到后删除列表中对应的密码

### 6.2 　　文件夹

文件夹使用了`TabView`，重写`TabItemTemplate`

```
        <muxc:TabView Style="{StaticResource TabViewStyle}"
                      Grid.Row="1"
                      MinHeight="40"
                      VerticalAlignment="Top"
                      CanDragTabs="True"
                      TabDragCompleted="{x:Bind Model.DragCompleted}"
                      TabItemsSource="{x:Bind Model.FolderItems}"
                      SelectedIndex="{x:Bind Model.SelectedIndex,Mode=TwoWay}"
                      AddTabButtonClick="{x:Bind Model.HandleAddFolder}">
            <muxc:TabView.TabItemTemplate>
                <DataTemplate>
                    <muxc:TabViewItem Header="{Binding Name}"
                                      IsClosable="False"
                                      Height="40">
                        <muxc:TabViewItem.Template>
                            <ControlTemplate TargetType="muxc:TabViewItem">
                                <!--
                                ...
                                -->
                            </ControlTemplate>
                        </muxc:TabViewItem.Template>
                    </muxc:TabViewItem>
                </DataTemplate>
            </muxc:TabView.TabItemTemplate>
        </muxc:TabView>
```

### 6.3 　　密码列表

密码列表是`ListView` + `Microsoft.Toolkit.Uwp.UI.Controls`中的`Expander`，`Expander`是下方添加、批量编辑。

```
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="*"/>
            <RowDefinition Height="auto"/>
        </Grid.RowDefinitions>

        <ListView>
        </ListView>
        <controls:Expander Grid.Row="1">
        </controls:Expander>
    </Grid>
```

### 6.4 　　密码详情

这部分就是控件的堆叠，包含大量 View 与 ViewModel 的绑定。键值对部分是`ListView`，具体查看相关代码

## 七　　独立组件介绍

与页面相关的组件，在 View 中可以找到。比如反馈页面在 `View/Feedback`，反馈页面上方的按钮在 `View/Feedback/Icon`。这些组件后面介绍页面时再说，此处仅介绍 Components 文件夹中的组件。

![反馈上方按钮](./ReadmeImgs/fb_btns.png)

以下组件均位于 Components 文件夹中

### 7.1 　　 Comment

获取某个 Issue 下的 Comments，展现为列表。`CommentSource`类实现了`IIncrementalSource`接口的`GetPagedItemsAsync`函数，如果列表过多会延迟加载。

### 7.2 　　 GitHub

GitHub：上方菜单点击 GitHub 图标，弹出的内容。

![GitHub](./ReadmeImgs/github.png)

这里的三个图标，都被封装成了 Icon 子组件，在同一目录下。

### 7.3 　　 IconControl

应用支持的图标控件，在以下几个地方使用：

1. 文件夹列表中，每个文件夹的图标
2. 密码列表中，每个密码的图标
3. 添加/编辑文件夹时，显示的图标
4. 添加/编辑/查看密码时，显示的图标
5. 移动密码时，文件夹列表图标

目前支持三种图标

1. 路径图（SVG）
2. 图片
3. Segoe MDL2 Assets 字体

### 7.4 　　 IconSelecter

图标选择控件，使用了 7.3 的 IconControl。点击即弹出图标选择的 Popup。该控件在以下几个地方使用：

1. 添加/编辑文件夹
2. 添加/编辑密码

![IconSelecter](./ReadmeImgs/icon_selecter.png)

### 7.5 　　 Sync

同步的操作与界面都在这里，包括备份与恢复的弹窗、历史记录、登录弹窗、密码验证、上传操作、下载操作、合并等

![同步](./ReadmeImgs/sync.png)

![同步登录](./ReadmeImgs/sync_login.png)

### 7.6 　　 BackSvg

验证页面和主页面的背景图，使用的 Path，因此颜色可跟随主题改变

### 7.7 　　 Commands

顶部的菜单，也就是在第六部分介绍的 4。这里的每个菜单，都已经写成了组件的形式。此处再封装有三个作用：

1. 设置各菜单图标和提示内容
2. 控制组件加载时机
3. 便于主页面和验证页面复用

### 7.8 　　 FirstUseDialog

用户首次使用出现的提示框

### 7.9 　　 Header

页面的头部，为了统一化各页面的头部显示

![Header](./ReadmeImgs/header.png)

### 7.10 　　 HelpDialog

“帮助”弹窗

### 7.11 　　 Logo

APP 的 logo，是 Path 形式，可任意控制颜色和大小。在以下几个地方使用：

1. 主页面左上角
2. 出现弹窗时，APP 左上角

### 7.12 　　 Rating

打分与打赏的弹出内容

![打分](./ReadmeImgs/rating.png)

### 7.13 　　 Version

版本显示，便于验证页和主页面统一显示版本信息

## 八　　主要页面介绍（View）

主要页面都在 View 中，和页面特有的子组件也都在这里。

### 8.1 　　 Auth

验证页面

### 8.2 　　 Feedback

反馈页面，列表使用了 7.1 的 `Comment`，点“反馈”会跳转的网页，在网页中使用了 GitTalk。也可在 本项目的 [Issues](https://github.com/hbrwang/Lavcode.Uwp/issues/3) 看到。
这里的反馈列表，就是这条 Issue 中的 Comments

### 8.2 　　 FolderList

_未完，待续_

## 九　　备份与恢复

## 十　　评论与通知
