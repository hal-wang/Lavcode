using GalaSoft.MvvmLight.Ioc;
using HTools.Uwp.Helpers;
using Lavcode.Uwp.Helpers;
using Lavcode.Uwp.Modules.Feedback;
using System;
using Windows.ApplicationModel.Core;

namespace Lavcode.Uwp.Modules.Setting
{
    public sealed partial class SettingSplitView : HTools.Uwp.Controls.Setting.SettingSplitView
    {
        public SettingSplitView()
        {
            DataContext = VM;
            this.InitializeComponent();
        }

        public SettingViewModel VM { get; } = SimpleIoc.Default.GetInstance<SettingViewModel>();

        private void OnChangeProvider(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            App.Frame.Navigate(typeof(FirstUse.FirstUsePage));
            IsPaneOpen = false;
        }

        private async void OnSignout(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var cdr = await PopupHelper.ShowDialog($"注销登录会清除当前登录信息，但不会清除任何数据。\n以后你也可以随时重新登录，确认注销？", "注销登录", "确认", "点错了");
            if (cdr != Windows.UI.Xaml.Controls.ContentDialogResult.Primary) return;

            switch (SettingHelper.Instance.Provider)
            {
                case Model.Provider.GitHub:
                    SettingHelper.Instance.GitHubToken = null;
                    break;
                case Model.Provider.Gitee:
                    SettingHelper.Instance.GiteeToken = null;
                    SettingHelper.Instance.GiteeRefreeToken = null;
                    break;
            }

            SettingHelper.Instance.IsAuthOpen = true;
            SettingHelper.Instance.IsFirstInited = false;
            await CoreApplication.RequestRestartAsync("R");
        }

        private async void OnHelp(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            await new HelpDialog().QueueAsync();
        }

        private void OnFeedback(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            App.Frame.Navigate(typeof(FeedbackPage));
            IsPaneOpen = false;
        }

        private void OnRating(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            FindName(nameof(Rating));
            this.RatingFlyout.ShowAt(RatingButton);
        }
    }
}
