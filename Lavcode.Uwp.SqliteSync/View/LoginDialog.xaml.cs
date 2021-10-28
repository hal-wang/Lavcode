using GalaSoft.MvvmLight.Ioc;
using Lavcode.Uwp.SqliteSync.ViewModel;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.SqliteSync.View
{
    public sealed partial class LoginDialog : ContentDialog
    {
        public LoginDialog()
        {
            DataContext = VM;
            this.InitializeComponent();

            VM.View = this;
        }

        public LoginViewModel VM { get; } = SimpleIoc.Default.GetInstance<LoginViewModel>();

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
