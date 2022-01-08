using HTools.Uwp.Helpers;
using Lavcode.Common;
using Lavcode.Uwp.Helpers;
using Lavcode.Uwp.Modules.Feedback;
using Microsoft.Extensions.DependencyInjection;
using System;
using Windows.System;

namespace Lavcode.Uwp.Modules.Setting
{
    public sealed partial class SettingSplitView : HTools.Uwp.Controls.Setting.SettingSplitView
    {
        public SettingSplitView()
        {
            DataContext = VM;
            this.InitializeComponent();
            Loaded += (s, e) => App.Frame.Navigated += Frame_Navigated;
            Unloaded += (s, e) => App.Frame.Navigated -= Frame_Navigated;
        }

        private void Frame_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            if (this.IsPaneOpen)
            {
                this.IsPaneOpen = false;
            }
        }

        public SettingViewModel VM { get; } = ServiceProvider.Services.GetService<SettingViewModel>();

        private void OnChangeProvider(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            App.Frame.Navigate(typeof(FirstUse.FirstUsePage));
            IsPaneOpen = false;
        }

        private async void OnSignout(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var cdr = await PopupHelper.ShowDialog($"该操作只会清除当前已登录信息，但不会删除任何用户数据。\n以后你也可以随时重新登录，确认注销？", "注销登录", "确认", "点错了");
            if (cdr != Windows.UI.Xaml.Controls.ContentDialogResult.Primary) return;

            CleanLoginInfo();
            App.Frame.Navigate(typeof(Auth.AuthPage), false);
            IsPaneOpen = false;
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

        private async void OnCleanLoginInfo(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var cdr = await PopupHelper.ShowDialog($"该操作只会清除当前已登录信息，但不会删除任何用户数据。确认清除？", "清除登录信息", "确认", "点错了");
            if (cdr != Windows.UI.Xaml.Controls.ContentDialogResult.Primary) return;

            CleanLoginInfo();
            IsPaneOpen = false;
            MessageHelper.ShowPrimary("已清除");
        }

        private void CleanLoginInfo()
        {
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
        }

        private async void OnViewPP(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            try
            {
                var result = await Launcher.LaunchUriAsync(new Uri(CommonConstant.PpUrl));
                if (!result)
                {
                    MessageHelper.ShowDanger($"打开失败，请使用浏览器打开\n{CommonConstant.PpUrl}", 0);
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }
    }
}
