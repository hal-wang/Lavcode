using HTools;
using HTools.Uwp.Helpers;
using Lavcode.Common;
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
        private string _count = "";
        public string Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }

        private IncrementalLoadingCollection<IssueSource, Issue> _feedbacks = null;
        public IncrementalLoadingCollection<IssueSource, Issue> Feedbacks
        {
            get { return _feedbacks; }
            set { SetProperty(ref _feedbacks, value); }
        }

        public async void HandleRefresh()
        {
            await Init();
        }

        public async Task Init()
        {
            LoadingHelper.Show();
            await TaskExtend.SleepAsync(100);
            try
            {
                Feedbacks = new IncrementalLoadingCollection<IssueSource, Issue>(new IssueSource(new string[] { RepositoryConstant.FeedbackIssueTag }));
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

            await InitRepo();
        }

        private async Task InitRepo()
        {
            var github = GitHubHelper.GetBaseClient(RepositoryConstant.Repos);
            var repo = await github.Repository.Get(RepositoryConstant.GitAccount, RepositoryConstant.Repos);
            Count = repo.OpenIssuesCount.ToString();
        }

        public async void HandleFeedback()
        {
            var newIssue = await new FeedbackDialog().QueueAsync<Issue>();
            if (newIssue == null) return;
            if (Feedbacks == null)
            {
                await Init();
                return;
            }

            if (int.TryParse(Count, out int count))
            {
                Count = (count + 1).ToString();
            }
            else
            {
                Count = "1";
            }
        }
    }
}
