using Lavcode.Common;
using Lavcode.Model;
using Lavcode.Uwp.Helpers;
using System;
using System.Threading.Tasks;
using System.Web;
using Windows.ApplicationModel;
using Windows.System;

namespace Lavcode.Uwp.Modules.Auth
{
    public static class OAuthLogin
    {
        private static Uri GetNavUrl(Provider provider)
        {
            var version = $"{Package.Current.Id.Version.Major}.{Package.Current.Id.Version.Minor}.{Package.Current.Id.Version.Build}";
            var notify = HttpUtility.UrlEncode($"{CommonConstant.ToolsApiUrl}/oauth/notify/{provider.ToString().ToLower()}?protocol=lavcode&app=Lavcode&platform=UWP&version={version}");
            var state = Guid.NewGuid().ToString().Substring(0, 12);

            return provider switch
            {
                Provider.GitHub => new Uri($"https://github.com/login/oauth/authorize?allow_signup=true&client_id=f3f682dda48d939fba2c&redirect_uri={notify}&state={state}"),
                Provider.Gitee => new Uri($"https://gitee.com/oauth/authorize?client_id=ab5b9883b6444750ff708e47d98a04f57ffc033306ec89271325356e5b5d8312&response_type=code&redirect_uri={notify}&state={state}"),
                _ => throw new NotSupportedException(),
            };
        }

        public static async Task<string> Login(Provider provider)
        {
            var token = SettingHelper.Instance.Get<string>(null, $"{provider}Token");
            if (string.IsNullOrEmpty(token))
            {
                await Launcher.LaunchUriAsync(GetNavUrl(provider));
                var loadingDialog = new OAuthLoadingDialog($"On{provider}Notify");
                await loadingDialog.ShowAsync();
                if (loadingDialog.Result == null) return null;

                token = loadingDialog.Result["token"];
                SettingHelper.Instance.Set(token, $"{provider}Token");
                if (provider == Provider.Gitee)
                {
                    SettingHelper.Instance.GiteeRefreeToken = loadingDialog.Result["refresh_token"];
                }
            }
            return token;
        }
    }
}
