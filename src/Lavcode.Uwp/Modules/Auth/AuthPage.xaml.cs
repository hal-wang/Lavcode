using HTools.Uwp.Helpers;
using Lavcode.Uwp.Helpers;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Lavcode.Uwp.Modules.Auth
{
    public sealed partial class AuthPage : Page
    {
        private bool _autoLogin = true;

        public AuthPage()
        {
            this.InitializeComponent();
            TitleBarHelper.SetTitleBar();

            Loaded += AuthPage_Loaded;
        }

        private void AuthPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (_autoLogin && SettingHelper.Instance.IsAutoLogin)
            {
                VM.TryLogin();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is bool b)
            {
                _autoLogin = b;
            }

            Frame.BackStack.Clear();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        public AuthViewModel VM { get; } = new AuthViewModel();
    }
}