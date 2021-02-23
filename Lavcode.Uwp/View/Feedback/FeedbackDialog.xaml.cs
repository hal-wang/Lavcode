using HTools.Uwp.Controls;
using Octokit;
using System;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.View.Feedback
{
    public sealed partial class FeedbackDialog : LayoutDialog
    {
        public FeedbackDialog()
        {
            this.InitializeComponent();
        }

        private async void LayoutDialog_PrimaryButtonClick(LayoutDialog sender, LayoutDialogButtonClickEventArgs args)
        {
            args.Cancel = true;

            IsLoading = true;
            if (await VM.Feedback())
            {
                this.Result = ContentDialogResult.Primary;
                this.IsOpen = false;
            }
            IsLoading = false;
        }

        private async void Preview_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await new MarkdownPreview(VM.Content).ShowAsync();
        }

        private async void Signup_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(Global.GitUrl, UriKind.Absolute));
        }

        public IssueComment CommentResult => VM.CommentResult;
    }
}
