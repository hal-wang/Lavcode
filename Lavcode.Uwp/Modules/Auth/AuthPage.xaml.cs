using HTools.Uwp.Helpers;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Lavcode.Uwp.Modules.Auth
{
    public sealed partial class AuthPage : Page
    {
        public AuthPage()
        {
            this.InitializeComponent();
            TitleBarHelper.SetTitleBar();

            Loaded += AuthPage_Loaded;
        }

        private async void AuthPage_Loaded(object sender, RoutedEventArgs e)
        {
            await VM.Init();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if(e.Parameter is IStorageFile)
            {

            }
        }

        public AuthViewModel VM { get; } = new AuthViewModel();
    }
}