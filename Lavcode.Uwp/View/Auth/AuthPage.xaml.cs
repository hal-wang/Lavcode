using Hubery.Tools.Uwp.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Hubery.Lavcode.Uwp.View.Auth
{
    public sealed partial class AuthPage : Page
    {
        public AuthPage()
        {
            this.InitializeComponent();
            TitleBarHelper.SetTitleBar();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                await Model.Init();
            }
        }

        private void ReferenceBtn_Click(object sender, RoutedEventArgs e)
        {
            ReferenceTip.IsOpen = true;
        }
    }
}