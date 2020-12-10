using GalaSoft.MvvmLight;
using Lavcode.Uwp.Helpers;
using Hubery.Tools.Uwp.Helpers;
using Octokit;
using System;
using System.Threading.Tasks;

namespace Lavcode.Uwp.View.Feedback
{
    class FeedbackDialogViewModel : ViewModelBase
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

            var url = await GitHubHelper.GetLoginUrl();
            if (string.IsNullOrEmpty(url))
            {
                MessageHelper.Show("认证信息获取失败");
                return false;
            };
            var ghld = new GitHubLoginDialog(new Uri(url));
            if (await ghld.ShowAsync() != Windows.UI.Xaml.Controls.ContentDialogResult.Primary) return false;

            GitHubClient client = GitHubHelper.GetAuthClient(ghld.Token);
            try
            {
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
