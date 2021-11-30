using GalaSoft.MvvmLight.Ioc;
using HTools.Uwp.Helpers;
using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.Notices
{
    public sealed partial class NoticesPage : Page
    {
        public NoticesPage()
        {
            DataContext = VM;
            this.InitializeComponent();

            Loaded += NoticesPage_Loaded;
        }

        public NoticesViewModel VM { get; } = SimpleIoc.Default.GetInstance<NoticesViewModel>();

        private async void NoticesPage_Loaded(object sender, RoutedEventArgs e)
        {
            await VM.Init();
        }

        public string NoticesUrl { get; } = $"{Global.ReposUrl}/issues/{Global.NoticeIssueNumber}";
        private async void Git_Click(object sender, RoutedEventArgs e)
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
