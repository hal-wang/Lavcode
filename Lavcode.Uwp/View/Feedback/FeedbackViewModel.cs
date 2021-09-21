using GalaSoft.MvvmLight;
using Lavcode.Uwp.Controls.Comment;
using Lavcode.Uwp.Helpers;
using HTools;
using HTools.Uwp.Helpers;
using Microsoft.Toolkit.Uwp;
using Octokit;
using System;
using System.Threading.Tasks;

namespace Lavcode.Uwp.View.Feedback
{
    class FeedbackViewModel : ViewModelBase
    {
        private Issue _issue = null;
        public Issue Issue
        {
            get => _issue;
            set
            {
                Set(ref _issue, value);
                _addedCount = 0;
                RaisePropertyChanged(nameof(Count));
            }
        }

        private int _addedCount = 0;
        public int Count => (Issue == null ? 0 : Issue.Comments) + _addedCount;

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

                Feedbacks = new IncrementalLoadingCollection<CommentSource, IssueComment>(new CommentSource(Global.FeedbackIssueNumber, Issue.Comments));
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
            Issue = await _client.Issue.Get(Global.GitAccount, Global.Repos, Global.FeedbackIssueNumber);
        }

        public async void HandleFeedback()
        {
            var fbDialog = new FeedbackDialog();
            if (!await fbDialog.QueueAsync<bool>()) return;

            if (Feedbacks == null || Feedbacks.HasMoreItems) return;

            Feedbacks.Insert(0, fbDialog.CommentResult);
            _addedCount++;
            RaisePropertyChanged(nameof(Count));
        }
    }
}
