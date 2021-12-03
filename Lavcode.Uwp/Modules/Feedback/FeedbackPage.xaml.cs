using GalaSoft.MvvmLight.Ioc;
using HTools.Uwp.Helpers;
using Lavcode.Common;
using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.Feedback
{
    public sealed partial class FeedbackPage : Page
    {
        public FeedbackPage()
        {
            DataContext = VM;
            this.InitializeComponent();

            Loaded += FeedbackPage_Loaded;
        }

        public FeedbackViewModel VM { get; } = SimpleIoc.Default.GetInstance<FeedbackViewModel>();

        private async void FeedbackPage_Loaded(object sender, RoutedEventArgs e)
        {
            await this.VM.Init();
        }

        public string Email { get; } = CommonConstant.Email;

        private async void Email_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var uriBing = new Uri("mailto://" + Email);
                var success = await Launcher.LaunchUriAsync(uriBing);
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }

        private async void Rating_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingHelper.Show();

                await PopupHelper.ShowRating();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
            finally
            {
                LoadingHelper.Hide();
            }
        }

        public string FeedbackUrl { get; } = $"{RepositoryConstant.GitHubRepositoryUrl}/issues/{RepositoryConstant.FeedbackIssueNumber}";
        private async void Git_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var uriBing = new Uri(FeedbackUrl);
                var success = await Launcher.LaunchUriAsync(uriBing);
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }
    }
}
