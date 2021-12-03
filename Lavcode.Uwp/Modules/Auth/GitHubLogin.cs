using GalaSoft.MvvmLight.Messaging;
using Lavcode.Uwp.Helpers;
using System;
using System.Threading.Tasks;
using System.Web;
using Windows.System;

namespace Lavcode.Uwp.Modules.Auth
{
    public class GitHubLogin
    {
        ~GitHubLogin()
        {
            Messenger.Default.Unregister(this);
        }

        private Uri GetNavUrl()
        {
            var notify = HttpUtility.UrlEncode($"{Global.ToolsApiUrl}/github/oauth/notify?protocol=lavcode&app=Lavcode&platform=UWP&version={Global.Version}");
            var state = Guid.NewGuid().ToString().Substring(0, 12);
            var url = $"https://github.com/login/oauth/authorize?allow_signup=true&client_id=f3f682dda48d939fba2c&redirect_uri={notify}&state={state}";
            return new Uri(url);
        }

        public async Task<string> Login()
        {
            var token = SettingHelper.Instance.GitHubToken;
            if (string.IsNullOrEmpty(token))
            {
                await Launcher.LaunchUriAsync(GetNavUrl());
                var loadingDialog = new OAuthLoadingDialog("OnGithubNotify");
                await loadingDialog.ShowAsync();
                if (loadingDialog.Result == null) return null;

                token = loadingDialog.Result["token"];
                SettingHelper.Instance.GitHubToken = token;
            }
            return token;
        }
    }
}
