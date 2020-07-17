using Hubery.Lavcode.Uwp.Helpers;
using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Hubery.Lavcode.Uwp.View.Notices
{
    public sealed partial class NoticesPage : Page
    {
        public NoticesPage()
        {
            this.InitializeComponent();

            Loaded += NoticesPage_Loaded;
        }

        private async void NoticesPage_Loaded(object sender, RoutedEventArgs e)
        {
            await VM.Init();
        }

        public string NoticesUrl { get; } = $"https://github.com/{Global.GitHubAccount}/{Global.Repos}/issues/{Global.NoticeIssueNumber}";
        private async void GitHub_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var uriBing = new Uri(NoticesUrl);
                var success = await Launcher.LaunchUriAsync(new Uri(NoticesUrl));
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }
    }
}
