using HTools;
using HTools.Uwp.Helpers;
using Lavcode.Common;
using Lavcode.Uwp.Helpers;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Octokit;
using System;
using System.Threading.Tasks;

namespace Lavcode.Uwp.Modules
{
    public class GitInfoViewModel : ObservableObject
    {
        private bool _loading = false;
        public bool Loading
        {
            get { return _loading; }
            set { SetProperty(ref _loading, value); }
        }

        private Repository _repository = null;
        public Repository Repository
        {
            get { return _repository; }
            set { SetProperty(ref _repository, value); }
        }

        public async Task Init()
        {
            if (Repository != null)
            {
                return;
            }

            Loading = true;
            await TaskExtend.SleepAsync(100);

            try
            {
                var github = GitHubHelper.GetBaseClient(RepositoryConstant.Repos);
                Repository = await github.Repository.Get(RepositoryConstant.GitAccount, RepositoryConstant.Repos);
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex, 0);
                return;
            }
            finally
            {
                Loading = false;
            }
        }
    }
}
