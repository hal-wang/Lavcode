using GalaSoft.MvvmLight;
using HTools;
using HTools.Uwp.Helpers;
using Lavcode.GitTools;
using Lavcode.Uwp.Common.View.Comment;
using Microsoft.Toolkit.Uwp;
using Octokit;
using System;
using System.Threading.Tasks;

namespace Lavcode.Uwp.Modules.Notices
{
    public class NoticesViewModel : ViewModelBase
    {
        public string Author => Global.GitAccount;

        private Issue _issue = null;
        public Issue Issue
        {
            get { return _issue; }
            set { Set(ref _issue, value); }
        }

        private IncrementalLoadingCollection<CommentSource, IssueComment> _notices = null;
        public IncrementalLoadingCollection<CommentSource, IssueComment> Notices
        {
            get { return _notices; }
            set { Set(ref _notices, value); }
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

                Notices = new IncrementalLoadingCollection<CommentSource, IssueComment>(new CommentSource(Global.NoticeIssueNumber, Issue.Comments));
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
            var _client = GitHubHelper.GetBaseClient(Global.Repos);
            Issue = await _client.Issue.Get(Global.GitAccount, Global.Repos, Global.NoticeIssueNumber);
        }
    }
}
