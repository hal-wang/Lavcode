using GalaSoft.MvvmLight;
using HTools.Uwp.Helpers;
using Lavcode.GitTools;
using Lavcode.Uwp.Common;
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
                var token = await new GitHubLogin().Login();
                if (string.IsNullOrEmpty(token)) return false;

                var client = GitHubHelper.GetAuthClient(Global.Repos, token);
                CommentResult = await client.Issue.Comment.Create(Global.GitAccount, Global.Repos, Global.FeedbackIssueNumber, Content);
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
