using Windows.UI.Xaml.Controls;

namespace Lavcode.View.Sync
{
    public sealed partial class LoginDialog : ContentDialog
    {
        public LoginDialog()
        {
            this.InitializeComponent();

            Model.View = this;
        }

        private void LayoutDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = !Model.Finish();
        }

        private void Help_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            WebDavHelpTeachingTip.IsOpen = true;
        }
    }
}
