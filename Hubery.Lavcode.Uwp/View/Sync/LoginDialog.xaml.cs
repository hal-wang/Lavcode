using Hubery.Tools.Uwp.Controls.Dialog;

namespace Hubery.Lavcode.Uwp.View.Sync
{
    public sealed partial class LoginDialog : LayoutDialog
    {
        public LoginDialog()
        {
            this.InitializeComponent();

            Model.View = this;
        }

        private void LayoutDialog_PrimaryButtonClick(LayoutDialog sender, LayoutDialogButtonClickEventArgs args)
        {
            args.Cancel = !Model.Finish();
        }

        private void Help_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            WebDavHelpTeachingTip.IsOpen = true;
        }
    }
}
