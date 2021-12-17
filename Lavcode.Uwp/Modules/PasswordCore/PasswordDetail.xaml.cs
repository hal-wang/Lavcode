using HTools;
using Lavcode.Uwp.Helpers;
using Lavcode.Uwp.Modules.Guide;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.PasswordCore
{
    public sealed partial class PasswordDetail : UserControl
    {
        public PasswordDetail()
        {
            DataContext = VM;
            this.InitializeComponent();
            Loaded += PasswordDetail_Loaded;
            Unloaded += PasswordDetail_Unloaded;

            VM.CalcTextBlock = CalcTextBlock; // 用于计算Key宽度
        }

        private void PasswordDetail_Unloaded(object sender, RoutedEventArgs e)
        {
            StrongReferenceMessenger.Default.UnregisterAll(this);
        }

        private void PasswordDetail_Loaded(object sender, RoutedEventArgs e)
        {
            StrongReferenceMessenger.Default.Register<PasswordDetail, object, string>(this, "AddNewPassword", (sender, obj) => AddNewPassword());
        }

        public PasswordDetailViewModel VM { get; } = ServiceProvider.Services.GetService<PasswordDetailViewModel>();

        private async void AddNewPassword()
        {
            await new GuideHandler()
            {
                SettingField = nameof(SettingHelper.AddPasswordTaught),
                Total = 6,
                Index = 2,
                Type = "添加记录",
            }
            .Add(new GuideItem()
            {
                Title = "密码标题",
                Content = "标题作为密码项的标识，应输入具有代表性且便于识别的内容，在密码列表中容易查找",
                Target = TitleTextBox,
            })
            .Add(() =>
            {
                VM.Title = "测试标题";
                VM.Remark = "这条记录是用来教学的，完成后可以自行删除";
            })
            .Add(new GuideItem()
            {
                Title = "生成密码",
                Content = "点击此按钮能随机生成复杂密码，当创建账号或修改密码时，能够使用复杂密码",
                Target = PasswordGeneratorBtn,
            })
            .Add(async () =>
            {
                PasswordGeneratorTip.IsOpen = true;
                await TaskExtend.SleepAsync();
            })
            .Add(new GuideItem()
            {
                Title = "生成完成",
                Content = "配置完成后，点击 生成 按钮即可",
                Target = PasswordGenerator,
            })
            .Add(() =>
            {
                PasswordGeneratorTip.IsOpen = false;
                VM.Value = "Lavcode";
            })
            .Add(new GuideItem()
            {
                Title = "关联内容",
                Content = "可以无限制添加多条内容，每项内容都可自定义名称，便于管理与账号相关的信息",
                Target = AddKvpBtn,
            })
            .Add(new GuideItem()
            {
                Title = "编辑完成",
                Content = "编辑完成，别忘记保存哦！（虽然有退出提醒，但手动保存是个好习惯）",
                Target = SaveBtn,
            })
            .Add(() =>
            {
                VM.HandleSave();
            })
            .End()
            .RunAsync();
        }

        private void SelectKey_Click(object sender, RoutedEventArgs e)
        {
            ((sender as MenuFlyoutItem).DataContext as PasswordKeyValuePairItem).Key = (sender as MenuFlyoutItem).Text;
        }

        private async void CustomKey_Click(object sender, RoutedEventArgs e)
        {
            await VM.CustomKey((sender as MenuFlyoutItem).DataContext as PasswordKeyValuePairItem);
        }

        private async void DeleteKey_Click(object sender, RoutedEventArgs e)
        {
            await VM.DeleteKey((sender as MenuFlyoutItem).DataContext as PasswordKeyValuePairItem);
        }

        private void PasswordGenerator_Click(object sender, RoutedEventArgs e)
        {
            if (!PasswordGeneratorTip.IsOpen && !VM.ReadOnly)
            {
                PasswordGeneratorTip.IsOpen = true;
            }
        }

        private void PasswordGenerator_PasswordChanged(PasswordGenerator sender, string args)
        {
            VM.Value = args;
        }

        private void CopyKeyValue_Click(object sender, RoutedEventArgs e)
        {
            VM.CopyKeyValue((sender as Button).DataContext as PasswordKeyValuePairItem, sender as Button);
        }

        private void OnKeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            var ctrlState = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control);

            switch (e.Key)
            {
                case VirtualKey.S:
                    if ((ctrlState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down)
                    {
                        VM.HandleSave();
                    }
                    break;

                default:
                    break;
            }
        }

        private void KeyTrimmedChanged(TextBlock sender, IsTextTrimmedChangedEventArgs args)
        {
            if (sender.IsTextTrimmed)
            {
                (sender.Tag as Button).Margin = new Thickness(12, 0, -12, 0);
            }
            else
            {
                (sender.Tag as Button).Margin = new Thickness(0);
            }
        }
    }
}
