using Lavcode.Uwp.Modules.SqliteSync.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.SqliteSync.View
{
    public sealed partial class LoginDialog : ContentDialog
    {
        public LoginDialog()
        {
            DataContext = VM;
            this.InitializeComponent();

            VM.View = this;
        }

        public LoginViewModel VM { get; } = ServiceProvider.Services.GetService<LoginViewModel>();

        private void LayoutDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = !VM.Finish();
        }

        private void Help_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            WebDavHelpTeachingTip.IsOpen = true;
        }
    }
}
