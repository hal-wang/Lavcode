using HTools.Uwp.Helpers;
using Lavcode.Common;
using Lavcode.Model;
using Lavcode.Uwp.Helpers;
using Lavcode.Uwp.Modules.Auth;
using System;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.FirstUse
{
    public sealed partial class FirstUsePage : Page
    {
        public FirstUsePage()
        {
            this.InitializeComponent();
            TitleBarHelper.SetTitleBar();

            Loaded += FirstUsePage_Loaded;
        }

        private void FirstUsePage_Loaded(object sender, RoutedEventArgs e)
        {
            Logo.Visibility = SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility == AppViewBackButtonVisibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        public bool IsPpChecked
        {
            get { return (bool)GetValue(IsPpCheckedProperty); }
            set { SetValue(IsPpCheckedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPpChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPpCheckedProperty =
            DependencyProperty.Register("IsPpChecked", typeof(bool), typeof(FirstUsePage), new PropertyMetadata(false));

        private async void Pp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = await Launcher.LaunchUriAsync(new Uri(CommonConstant.PpUrl));
                if (!result)
                {
                    MessageHelper.ShowDanger($"打开失败,请使用浏览器打开\n{CommonConstant.PpUrl}", 0);
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }

        private async void OnProviderSelect(ProviderSelectButton sender, Provider args)
        {
            if (!IsPpChecked)
            {
                await PopupHelper.ShowTeachingTipAsync(PpCheckBox, "未勾选隐私政策", "请阅读并同意隐私政策", Microsoft.UI.Xaml.Controls.TeachingTipPlacementMode.Top);
                return;
            }

            SettingHelper.Instance.Provider = args;
            Frame.Navigate(typeof(AuthPage));
        }
    }
}


