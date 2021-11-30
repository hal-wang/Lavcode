using HTools;
using HTools.Uwp.Helpers;
using Lavcode.Model;
using Lavcode.Uwp.Common;
using Lavcode.Uwp.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using Windows.System;

namespace Lavcode.Uwp.Modules.Feedback
{
    public class GitHubLogin
    {
        public async Task<string> Login()
        {
            var isLocalTokenValid = await IsLocalTokenValid();
            if (isLocalTokenValid == null) return null;
            if (isLocalTokenValid == true) return SettingHelper.Instance.GitHubToken;

            var oauth = await GetGitHubOauth();
            if (oauth == null) return null;

            if (!await Launcher.LaunchUriAsync(new Uri(oauth.Url)))
            {
                MessageHelper.ShowDanger("GitHub 登录失败");
                return null;
            }
            await TaskExtend.SleepAsync(300);
            if (await PopupHelper.ShowDialog("是否已在浏览器成功登录？", "GitHub登录", "是", "否", true, true) != Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                return null;
            }

            return await GetToken(oauth.Id);
        }

        private async Task<GitHubOauth> GetGitHubOauth()
        {
            var res = await Helpers.HttpClientExtend.HttpClient.GetAsync($"{Global.ToolsApiUrl}/github/oauth/url");
            if (res.IsResErr()) return null;
            return await res.GetContent<GitHubOauth>();
        }

        private async Task<string> GetToken(string id)
        {
            var res = await Helpers.HttpClientExtend.HttpClient.GetAsync($"{Global.ToolsApiUrl}/github/oauth/{id}");
            if (res.IsResErr()) return null;
            var obj = await res.GetContent<JObject>();
            var tokenField = "token";
            if (!obj.ContainsKey(tokenField) && !string.IsNullOrEmpty(obj.Value<string>(tokenField)))
            {
                MessageHelper.ShowDanger("GitHub 登录失败");
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

            var res = await Helpers.HttpClientExtend.HttpClient.GetAsync($"{Global.ToolsApiUrl}/github/oauth/{token}/valid");
            if (res.IsResErr()) return null;
            var obj = await res.GetContent<JObject>();
            var validField = "valid";
            if (!obj.ContainsKey(validField))
            {
                MessageHelper.ShowDanger("GitHub 登录失败");
                return null;
            }
            return obj.Value<bool>(validField);
        }
    }
}
