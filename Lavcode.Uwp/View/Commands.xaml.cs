using HTools.Uwp.Helpers;
using Lavcode.Uwp.SqliteSync.View;
using Lavcode.Uwp.View.Feedback;
using Lavcode.Uwp.View.Notices;
using Lavcode.Uwp.View.Setting;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.View
{
    public sealed partial class Commands : UserControl
    {
        public Commands()
        {
            this.InitializeComponent();
        }

        public bool HaveLogin
        {
            get { return (bool)GetValue(HaveLoginProperty); }
            set { SetValue(HaveLoginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HaveLogin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HaveLoginProperty =
            DependencyProperty.Register("HaveLogin", typeof(bool), typeof(Commands), new PropertyMetadata(false));

        private void FeedbackBtn_Click(object sender, RoutedEventArgs e)
        {
            App.Frame.Navigate(typeof(FeedbackPage));
        }

        private void GitFlyout_Opened(object sender, object e)
        {
            FindName(nameof(GitInfo));
        }

        private void NoticeBtn_Click(object sender, RoutedEventArgs e)
        {
            App.Frame.Navigate(typeof(NoticesPage));
        }

        private void Rating_Opened(object sender, object e)
        {
            FindName(nameof(Rating));
        }

        private async void Sync_Click(object sender, RoutedEventArgs e)
        {
            await new SyncDialog().QueueAsync();
        }

        private async void HelpBtn_Click(object sender, RoutedEventArgs e)
        {
            await new HelpDialog().QueueAsync();
        }

        private async void SettingBtn_Click(object sender, RoutedEventArgs e)
        {
            await new SettingDialog().QueueAsync();
        }
    }
}
