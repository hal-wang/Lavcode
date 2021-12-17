using HTools;
using HTools.Uwp.Helpers;
using Lavcode.Common;
using Lavcode.Model;
using Lavcode.Uwp.Helpers;
using Microsoft.Toolkit.Mvvm.Messaging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.Auth
{
    public sealed partial class OAuthLoginDialog : ContentDialog, IResultDialog<string>
    {
        public Provider Provider { get; private set; }

        public OAuthLoginDialog(Provider provider)
        {
            this.Provider = provider;
            this.InitializeComponent();
            StrongReferenceMessenger.Default.Register<NameValueCollection, string>(this, MessageStr, (a, b) => OnRsvLoginMsg(b));
            this.Closed += OAuthLoadingDialog_Closed;
            Loaded += OAuthLoadingDialog_Loaded;
        }

        private async void OAuthLoadingDialog_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(NavUrl);
        }

        private string MessageStr => $"On{Provider}Notify";

        private Uri NavUrl
        {
            get
            {
                var notify = HttpUtility.UrlEncode($"{CommonConstant.ToolsApiUrl}/oauth/notify/{Provider.ToString().ToLower()}?protocol=lavcode&app=Lavcode&platform=UWP&version={Global.Version}");
                var state = Guid.NewGuid().ToString().Substring(0, 12);

                return Provider switch
                {
                    Provider.GitHub => new Uri($"https://github.com/login/oauth/authorize?allow_signup=true&client_id=f3f682dda48d939fba2c&redirect_uri={notify}&state={state}&scope=repo%20read:user"),
                    Provider.Gitee => new Uri($"https://gitee.com/oauth/authorize?client_id=ab5b9883b6444750ff708e47d98a04f57ffc033306ec89271325356e5b5d8312&response_type=code&redirect_uri={notify}&state={state}"),
                    _ => throw new NotSupportedException(),
                };
            }
        }

        public string Result { get; set; }

        private void OAuthLoadingDialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            StrongReferenceMessenger.Default.UnregisterAll(this);
        }

        private void OnRsvLoginMsg(NameValueCollection collection)
        {
            Result = collection["token"];
            SettingHelper.Instance.Set(Result, $"{Provider}Token");
            if (Provider == Provider.Gitee)
            {
                SettingHelper.Instance.GiteeRefreeToken = collection["refresh_token"];
            }
            this.Hide();
        }

        #region Static
        private static async Task RefreshGiteeToken()
        {
            var res = await new HttpClient().PostAsync("https://gitee.com/oauth/token", query: new
            {
                grant_type = "refresh_token",
                refresh_token = SettingHelper.Instance.GiteeRefreeToken
            });
            res.EnsureSuccessStatusCode();
            var data = await res.GetContent<JObject>();
            SettingHelper.Instance.GiteeRefreeToken = data.Value<string>("refresh_token");
            SettingHelper.Instance.GiteeToken = data.Value<string>("access_token");
        }

        public static async Task<string> Login(Provider provider)
        {
            var token = SettingHelper.Instance.Get<string>(null, $"{provider}Token");
            if (string.IsNullOrEmpty(token))
            {
                var loginDialog = new OAuthLoginDialog(provider);
                token = await loginDialog.QueueAsync<string>();
            }
            else if (provider == Provider.Gitee)
            {
                await RefreshGiteeToken();
                token = SettingHelper.Instance.GiteeToken;
            }
            return token;
        }
        #endregion
    }
}
