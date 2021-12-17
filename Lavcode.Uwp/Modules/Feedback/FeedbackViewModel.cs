using HTools;
using HTools.Uwp.Helpers;
using Lavcode.Common;
using Lavcode.Uwp.Controls.Comment;
using Lavcode.Uwp.Helpers;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Uwp;
using Octokit;
using System;
using System.Threading.Tasks;

namespace Lavcode.Uwp.Modules.Feedback
{
    public class FeedbackViewModel : ObservableObject
    {
        private Issue _issue = null;
        public Issue Issue
        {
            get => _issue;
            set
            {
                SetProperty(ref _issue, value);
                _addedCount = 0;
                OnPropertyChanged(nameof(Count));
            }
        }

        private int _addedCount = 0;
        public int Count => (Issue == null ? 0 : Issue.Comments) + _addedCount;

        private IncrementalLoadingCollection<CommentSource, IssueComment> _feedbacks = null;
        public IncrementalLoadingCollection<CommentSource, IssueComment> Feedbacks
        {
            get { return _feedbacks; }
            set { SetProperty(ref _feedbacks, value); }
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

                Feedbacks = new IncrementalLoadingCollection<CommentSource, IssueComment>(new CommentSource(RepositoryConstant.FeedbackIssueNumber, Issue.Comments));
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
            var _client = GitHubHelper.GetBaseClient(RepositoryConstant.Repos);
            Issue = await _client.Issue.Get(RepositoryConstant.GitAccount, RepositoryConstant.Repos, RepositoryConstant.FeedbackIssueNumber);
        }

        public async void HandleFeedback()
        {
            var fbDialog = new FeedbackDialog();
            if (!await fbDialog.QueueAsync<bool>()) return;

            if (Feedbacks == null || Feedbacks.HasMoreItems) return;

            Feedbacks.Insert(0, fbDialog.CommentResult);
            _addedCount++;
            OnPropertyChanged(nameof(Count));
        }
    }
}
