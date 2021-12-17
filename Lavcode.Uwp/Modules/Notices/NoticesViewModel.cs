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

namespace Lavcode.Uwp.Modules.Notices
{
    public class NoticesViewModel : ObservableObject
    {
        public string Author => RepositoryConstant.GitAccount;

        private Issue _issue = null;
        public Issue Issue
        {
            get { return _issue; }
            set { SetProperty(ref _issue, value); }
        }

        private IncrementalLoadingCollection<CommentSource, IssueComment> _notices = null;
        public IncrementalLoadingCollection<CommentSource, IssueComment> Notices
        {
            get { return _notices; }
            set { SetProperty(ref _notices, value); }
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

                Notices = new IncrementalLoadingCollection<CommentSource, IssueComment>(new CommentSource(RepositoryConstant.NoticeIssueNumber, Issue.Comments));
                Notices.OnEndLoading += () =>
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
            Issue = await _client.Issue.Get(RepositoryConstant.GitAccount, RepositoryConstant.Repos, RepositoryConstant.NoticeIssueNumber);
        }
    }
}
