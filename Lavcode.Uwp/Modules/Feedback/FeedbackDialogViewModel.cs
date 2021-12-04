using GalaSoft.MvvmLight;
using HTools.Uwp.Helpers;
using Lavcode.Common;
using Lavcode.Uwp.Helpers;
using Lavcode.Uwp.Modules.Auth;
using Octokit;
using System;
using System.Threading.Tasks;

namespace Lavcode.Uwp.Modules.Feedback
{
    public class FeedbackDialogViewModel : ViewModelBase
    {
        private string _content = string.Empty;
        public string Content
        {
            get { return _content; }
            set { Set(ref _content, value); }
        }

        public IssueComment CommentResult { get; private set; } = null;

        public async Task<bool> Feedback()
        {
            if (string.IsNullOrEmpty(Content))
            {
                MessageHelper.ShowWarning("请输入反馈内容");
                return false;
            }

            try
            {
                var token = await OAuthLogin.Login(Model.Provider.GitHub);
                if (string.IsNullOrEmpty(token)) return false;

                var client = GitHubHelper.GetAuthClient(RepositoryConstant.Repos, token);
                CommentResult = await client.Issue.Comment.Create(RepositoryConstant.GitAccount, RepositoryConstant.Repos, RepositoryConstant.FeedbackIssueNumber, Content);
            }
            catch (System.Net.Http.HttpRequestException)
            {
                MessageHelper.ShowDanger("提交失败");
                return false;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowDanger(ex.Message);
                return false;
            }

            MessageHelper.ShowPrimary("反馈成功");
            return true;
        }

    }
}
