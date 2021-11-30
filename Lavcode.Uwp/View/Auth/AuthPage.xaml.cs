using HTools.Uwp.Helpers;
using Lavcode.Uwp.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.View.Auth
{
    public sealed partial class AuthPage : Page
    {
        public AuthPage()
        {
            DataContext = VM;
            this.InitializeComponent();
            TitleBarHelper.SetTitleBar();

            Loaded += AuthPage_Loaded;
        }

        private async void AuthPage_Loaded(object sender, RoutedEventArgs e)
        {
            await VM.Init();
        }

        public WindowsHelloAuthViewModel VM { get; } = new WindowsHelloAuthViewModel();

        private void ReferenceBtn_Click(object sender, RoutedEventArgs e)
        {
            ReferenceTip.IsOpen = true;
        }
    }
}