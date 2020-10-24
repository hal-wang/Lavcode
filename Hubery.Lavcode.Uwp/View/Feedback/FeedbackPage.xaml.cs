using Hubery.Tools.Uwp.Helpers;
using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Hubery.Lavcode.Uwp.View.Feedback
{
    public sealed partial class FeedbackPage : Page
    {
        public FeedbackPage()
        {
            this.InitializeComponent();

            Loaded += FeedbackPage_Loaded;
        }

        private async void FeedbackPage_Loaded(object sender, RoutedEventArgs e)
        {
            await this.VM.Init();
        }

        public string Email { get; } = Global.Email;

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

        public string FeedbackUrl { get; } = $"{Global.ReposUrl}/issues/{Global.FeedbackIssueId}";
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
