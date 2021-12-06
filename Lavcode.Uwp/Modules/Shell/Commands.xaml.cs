using GalaSoft.MvvmLight.Ioc;
using HTools.Uwp.Helpers;
using Lavcode.Model;
using Lavcode.Uwp.Helpers;
using Lavcode.Uwp.Modules.Feedback;
using Lavcode.Uwp.Modules.Notices;
using Lavcode.Uwp.Modules.Setting;
using Lavcode.Uwp.Modules.SqliteSync;
using Lavcode.Uwp.Modules.SqliteSync.View;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.Shell
{
    public sealed partial class Commands : UserControl
    {
        public Commands()
        {
            this.InitializeComponent();
        }

        public bool IsLaunchFile => SimpleIoc.Default.IsRegistered<SqliteFileService>()
                    && SimpleIoc.Default.GetInstance<SqliteFileService>().OpenedFile != null;
        public bool IsSyncVisible => SettingHelper.Instance.Provider == Provider.Sqlite && !IsLaunchFile;

        public Provider Provider => SettingHelper.Instance.Provider;

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
            await new SettingSplitView().ShowAsync();
        }
    }
}
