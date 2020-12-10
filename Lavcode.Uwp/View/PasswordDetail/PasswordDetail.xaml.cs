using GalaSoft.MvvmLight.Messaging;
using Lavcode.Uwp.Helpers;
using Hubery.Tools;
using Hubery.Tools.Uwp.Helpers;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Core.Preview;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.View.PasswordDetail
{
    public sealed partial class PasswordDetail : UserControl
    {
        public PasswordDetail()
        {
            this.InitializeComponent();
            Messenger.Default.Register<object>(this, "AddNewPassword", (obj) => AddNewPassword());

            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += this.OnCloseRequest;
            Model.CalcTextBlock = CalcTextBlock; // 用于计算Key宽度
        }

        ~PasswordDetail()
        {
            Messenger.Default.Unregister(this);
        }

        private async void AddNewPassword()
        {
            if (SettingHelper.Instance.AddPasswordTaught)
            {
                return;
            }
            await PopupHelper.ShowTeachingTip(TitleTextBox, "密码标题（添加记录 2/6）", "标题作为密码项的标识，应输入具有代表性且便于识别的内容，在密码列表中容易查找");

            Model.Title = "测试标题";
            Model.Remark = "这条记录是用来教学的，完成后可以自行删除";
            await PopupHelper.ShowTeachingTip(PasswordGeneratorBtn, "生成密码（添加记录 3/6）", "点击此按钮能随机生成复杂密码，当创建账号或修改密码时，能够使用复杂密码");
            PasswordGeneratorTip.IsOpen = true;
            await TaskExtend.SleepAsync();
            await PopupHelper.ShowTeachingTip(PasswordGenerator, "生成完成（添加记录 4/6）", "配置完成后，点击 生成 按钮即可");
            PasswordGeneratorTip.IsOpen = false;
            Model.Value = "Lavcode";
            await PopupHelper.ShowTeachingTip(AddKvpBtn, "关联内容（添加记录 5/6）", "可以无限制添加多条内容，每项内容都可自定义名称，便于管理与账号相关的信息");
            await PopupHelper.ShowTeachingTip(SaveBtn, "编辑完成（添加记录 6/6）", "编辑完成，别忘记保存哦！（虽然有退出提醒，但手动保存是个好习惯）");
            SettingHelper.Instance.AddPasswordTaught = true;
            Model.HandleSave();
        }

        private async void OnCloseRequest(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            if (!Model.IsEdited)
            {
                return;
            }

            e.Handled = true;
            try
            {
                await Model.OnCloseRequest();
            }
            catch { }
        }

        private void SelectKey_Click(object sender, RoutedEventArgs e)
        {
            ((sender as MenuFlyoutItem).DataContext as KeyValuePairItem).Key = (sender as MenuFlyoutItem).Text;
        }

        private async void CustomKey_Click(object sender, RoutedEventArgs e)
        {
            await Model.CustomKey((sender as MenuFlyoutItem).DataContext as KeyValuePairItem);
        }

        private async void DeleteKey_Click(object sender, RoutedEventArgs e)
        {
            await Model.DeleteKey((sender as MenuFlyoutItem).DataContext as KeyValuePairItem);
        }

        private void PasswordGenerator_Click(object sender, RoutedEventArgs e)
        {
            if (!PasswordGeneratorTip.IsOpen && !Model.ReadOnly)
            {
                PasswordGeneratorTip.IsOpen = true;
            }
        }

        private void PasswordGenerator_PasswordChanged(PasswordGenerator sender, string args)
        {
            Model.Value = args;
        }

        private void CopyKeyValue_Click(object sender, RoutedEventArgs e)
        {
            Model.CopyKeyValue((sender as Button).DataContext as KeyValuePairItem, sender as Button);
        }

        private void OnKeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            var ctrlState = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control);

            switch (e.Key)
            {
                case VirtualKey.S:
                    if ((ctrlState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down)
                    {
                        Model.HandleSave();
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
