using GalaSoft.MvvmLight;
using Hubery.Lavcode.Uwp.Controls.Comment;
using Hubery.Lavcode.Uwp.Helpers;
using Microsoft.Toolkit.Uwp;
using Octokit;
using System;
using System.Threading.Tasks;

namespace Hubery.Lavcode.Uwp.View.Feedback
{
    class FeedbackViewModel : ViewModelBase
    {
        private Issue _issue = null;
        public Issue Issue
        {
            get { return _issue; }
            set { Set(ref _issue, value); }
        }

        private IncrementalLoadingCollection<CommentSource, IssueComment> _feedbacks = null;
        public IncrementalLoadingCollection<CommentSource, IssueComment> Feedbacks
        {
            get { return _feedbacks; }
            set { Set(ref _feedbacks, value); }
        }

        public async void HandleRefresh()
        {
            Issue = null;
            await Init();
        }

        public async Task Init()
        {
            if (Issue != null)
            {
                return;
            }

            LoadingHelper.Show();
            await TaskExtend.SleepAsync(100);
            try
            {
                await GetIssueInfo();

                Feedbacks = new IncrementalLoadingCollection<CommentSource, IssueComment>(new CommentSource(Global.FeedbackIssueNumber));
                Feedbacks.OnEndLoading += () =>
                {
                    LoadingHelper.Hide();
                };
            }
            catch (Exception ex)
            {
                LoadingHelper.Hide();
                MessageHelper.ShowError(ex, 0);
            }
        }

        private async Task GetIssueInfo()
        {
            GitHubClient _client = GitHubHelper.GetBaseClient();
            Issue = await _client.Issue.Get(Global.GitHubAccount, Global.Repos, Global.FeedbackIssueNumber);
        }

        public async void HandleFeedback()
        {
            var fbDialog = new FeedbackDialog();
            if (await fbDialog.ShowAsync() != Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                return;
            }

            if (Feedbacks == null || Feedbacks.HasMoreItems)
            {
                return;
            }

            Feedbacks.Add(fbDialog.CommentResult);
        }
    }
}
