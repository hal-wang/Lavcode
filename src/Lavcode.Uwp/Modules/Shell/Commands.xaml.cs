using HTools.Uwp.Helpers;
using Lavcode.Model;
using Lavcode.Uwp.Helpers;
using Lavcode.Uwp.Modules.Notices;
using Lavcode.Uwp.Modules.Setting;
using Lavcode.Uwp.Modules.SqliteSync;
using Lavcode.Uwp.Modules.SqliteSync.View;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.Shell
{
    [INotifyPropertyChanged]
    public sealed partial class Commands : UserControl
    {
        public Commands()
        {
            this.InitializeComponent();
        }


        public bool IsSearchOpen
        {
            get { return (bool)GetValue(IsSearchOpenProperty); }
            set { SetValue(IsSearchOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSearchOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSearchOpenProperty =
            DependencyProperty.Register("IsSearchOpen", typeof(bool), typeof(Commands), new PropertyMetadata(false, OnIsSearchOpenChanged));

        private static void OnIsSearchOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Commands).OnPropertyChanged(nameof(IsSearchVisible));
        }

        public bool IsLaunchFile => ServiceProvider.Services.GetService<SqliteFileService>()?.OpenedFile != null;
        public bool IsSyncVisible => SettingHelper.Instance.Provider == Provider.Sqlite && !IsLaunchFile && App.Frame.CurrentSourcePageType == typeof(ShellPage);
        public bool IsSearchVisible => App.Frame.CurrentSourcePageType == typeof(ShellPage) && !IsSearchOpen;

        public Provider Provider => SettingHelper.Instance.Provider;

        private void GitFlyout_Opened(object sender, object e)
        {
            FindName(nameof(GitInfo));
        }

        private void NoticeBtn_Click(object sender, RoutedEventArgs e)
        {
            App.Frame.Navigate(typeof(NoticesPage));
        }

        private async void Sync_Click(object sender, RoutedEventArgs e)
        {
            await new SyncDialog().QueueAsync();
        }

        private async void SettingBtn_Click(object sender, RoutedEventArgs e)
        {
            await new SettingSplitView().ShowAsync();
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            IsSearchOpen = true;
            OnPropertyChanged(nameof(IsSearchVisible));
        }
    }
}
