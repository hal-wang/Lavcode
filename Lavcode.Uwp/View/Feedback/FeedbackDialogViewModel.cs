using GalaSoft.MvvmLight;
using HTools;
using HTools.Uwp.Helpers;
using Lavcode.Model;
using Lavcode.Uwp.Common;
using Lavcode.Uwp.Helpers;
using Newtonsoft.Json.Linq;
using Octokit;
using System;
using System.Threading.Tasks;
using Windows.System;

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

            try
            {
                var token = await Login();
                if (string.IsNullOrEmpty(token)) return false;

                GitHubClient client = GitHubHelper.GetAuthClient(token);
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

        private async Task<string> Login()
        {
            var isLocalTokenValid = await IsLocalTokenValid();
            if (isLocalTokenValid == null) return null;
            if (isLocalTokenValid == true) return SettingHelper.Instance.GitHubToken;

            var oauth = await GetGitHubOauth();
            if (oauth == null) return null;

            if (!await Launcher.LaunchUriAsync(new Uri(oauth.Url)))
            {
                MessageHelper.ShowDanger("GitHub登录失败");
                return null;
            }
            await TaskExtend.SleepAsync(3);
            if (await PopupHelper.ShowDialog("是否已成功登录？", "GitHub登录", "是", "否", true, true) != Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                return null;
            }

            return await GetToken(oauth.Id);
        }

        private async Task<GitHubOauth> GetGitHubOauth()
        {
            var res = await GitHubHelper.HttpClient.GetAsync($"{Global.ToolsApiUrl}/github/oauth/url");
            if (res.IsResErr()) return null;
            return await res.GetContent<GitHubOauth>();
        }

        private async Task<string> GetToken(string id)
        {
            var res = await GitHubHelper.HttpClient.GetAsync($"{Global.ToolsApiUrl}/github/oauth/{id}");
            if (res.IsResErr()) return null;
            var obj = await res.GetContent<JObject>();
            var tokenField = "token";
            if (!obj.ContainsKey(tokenField) && !string.IsNullOrEmpty(obj.Value<string>(tokenField)))
            {
                MessageHelper.ShowDanger("GitHub登录失败");
                return null;
            }
            var token = obj.Value<string>(tokenField);
            SettingHelper.Instance.GitHubToken = token;
            return token;
        }

        private async Task<bool?> IsLocalTokenValid()
        {
            var token = SettingHelper.Instance.GitHubToken;
            if (string.IsNullOrEmpty(token)) return false;

            var res = await GitHubHelper.HttpClient.GetAsync($"{Global.ToolsApiUrl}/github/oauth/{token}/valid");
            if (res.IsResErr()) return null;
            var obj = await res.GetContent<JObject>();
            var validField = "valid";
            if (!obj.ContainsKey(validField))
            {
                MessageHelper.ShowDanger("GitHub登录失败");
                return null;
            }
            return obj.Value<bool>(validField);
        }
    }
}
