using HTools.Uwp.Helpers;
using Lavcode.Uwp.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Lavcode.Uwp.View
{
    public sealed partial class AuthPage : Page
    {
        public AuthPage()
        {
            DataContext = VM;
            this.InitializeComponent();
            TitleBarHelper.SetTitleBar();
        }

        public AuthViewModel VM { get; } = new AuthViewModel();

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                await VM.Init();
            }
        }

        private void ReferenceBtn_Click(object sender, RoutedEventArgs e)
        {
            ReferenceTip.IsOpen = true;
        }
    }
}