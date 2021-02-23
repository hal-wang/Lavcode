using HTools.Uwp.Controls;
using System;
using Windows.UI.Xaml;

namespace Lavcode.Uwp.View
{
    public sealed partial class GitHubLoginDialog : LayoutDialog
    {
        public GitHubLoginDialog(Uri authUri)
        {
            this.AuthUri = authUri;
            this.InitializeComponent();
        }

        public Uri AuthUri
        {
            get { return (Uri)GetValue(AuthUriProperty); }
            set { SetValue(AuthUriProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AuthUri.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AuthUriProperty =
            DependencyProperty.Register("AuthUri", typeof(Uri), typeof(GitHubLoginDialog), new PropertyMetadata(null));

        public string Token { get; private set; }

        private void WebView_NavigationCompleted(Windows.UI.Xaml.Controls.WebView sender, Windows.UI.Xaml.Controls.WebViewNavigationCompletedEventArgs args)
        {
            if (sender.DocumentTitle.IndexOf(Global.AuthTitleKey) != 0) return;

            this.Token = sender.DocumentTitle.Substring(
                Global.AuthTitleKey.Length + 1,
                sender.DocumentTitle.Length - Global.AuthTitleKey.Length - 1);
            this.Result = Windows.UI.Xaml.Controls.ContentDialogResult.Primary;
            this.IsOpen = false;
        }
    }
}
