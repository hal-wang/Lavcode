using GalaSoft.MvvmLight;
using HTools;
using HTools.Uwp.Helpers;
using Lavcode.Uwp.Helpers;
using Octokit;
using System;
using System.Threading.Tasks;

namespace Lavcode.Uwp.Modules
{
    public class GitInfoViewModel : ViewModelBase
    {
        private bool _loading = false;
        public bool Loading
        {
            get { return _loading; }
            set { Set(ref _loading, value); }
        }

        private Repository _repository = null;
        public Repository Repository
        {
            get { return _repository; }
            set { Set(ref _repository, value); }
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
                var github = GitHubHelper.GetBaseClient(Global.Repos);
                Repository = await github.Repository.Get(Global.GitAccount, Global.Repos);
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
