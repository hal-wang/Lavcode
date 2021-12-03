using GalaSoft.MvvmLight.Messaging;
using System.Collections.Specialized;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.Auth
{
    public sealed partial class OAuthLoadingDialog : ContentDialog
    {
        private readonly string _msg;
        public NameValueCollection Result { get; private set; } = null;

        public OAuthLoadingDialog(string msg)
        {
            this._msg = msg;
            this.InitializeComponent();
            Messenger.Default.Register<NameValueCollection>(this, _msg, (col) =>
            {
                Result = col;
                this.Hide();
            });
            this.Closed += OAuthLoadingDialog_Closed;
        }

        private void OAuthLoadingDialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            Messenger.Default.Unregister(this);
        }
    }
}
