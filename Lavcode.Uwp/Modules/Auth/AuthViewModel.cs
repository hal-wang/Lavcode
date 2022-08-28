using HTools.Uwp.Helpers;
using Lavcode.IService;
using Lavcode.Model;
using Lavcode.Uwp.Helpers;
using Lavcode.Uwp.Modules.Shell;
using Lavcode.Uwp.Modules.SqliteSync;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.Auth
{
    public class AuthViewModel : Microsoft.Toolkit.Mvvm.ComponentModel.ObservableObject
    {
        public Provider Provider => SettingHelper.Instance.Provider;

        private bool _supportWindowsHello = true;
        public bool SupportWindowsHello
        {
            get { return _supportWindowsHello; }
            set { SetProperty(ref _supportWindowsHello, value); }
        }

        private bool _loading = false;
        public bool Loading
        {
            get { return _loading; }
            set { SetProperty(ref _loading, value); }
        }

        public async void TryLogin()
        {
            if (Loading)
            {
                return;
            }

            Loading = true;
            try
            {
                await Login();
            }
            catch (HttpRequestException)
            {
                MessageHelper.ShowDanger("网络连接失败，请检查网络设置");
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
            finally
            {
                Loading = false;
            }
        }

        private async Task Login()
        {
#if WITHOUT_HELLO
#else
            if (ShouldAuthWindowsHello && !await WindowsHelloAuth())
            {
                return;
            }
#endif

            var provider = SettingHelper.Instance.Provider;
            object loginData = null;
            switch (provider)
            {
                case Provider.GitHub:
                case Provider.Gitee:
                    var gitToken = await OAuthLoginDialog.Login(provider);
                    if (string.IsNullOrEmpty(gitToken)) return;
                    loginData = new { Token = gitToken };
                    switch (provider)
                    {
                        case Provider.GitHub:
                            ServiceProvider.Register<Service.GitHub.GitHubConService>();
                            break;
                        case Provider.Gitee:
                            ServiceProvider.Register<Service.Gitee.GiteeConService>();
                            break;
                    }
                    break;
                case Provider.Sqlite:
                    ServiceProvider.Register<Service.Sqlite.ConService>();
                    loginData = new
                    {
                        FilePath = ServiceProvider.Services.GetService<SqliteFileService>().SqliteFilePath
                    };
                    break;
                case Provider.Api:
                    var apiToken = await ApiLoginDialog.Login();
                    if (string.IsNullOrEmpty(apiToken)) return;
                    ServiceProvider.Register<Service.Api.ConService>();
                    loginData = new
                    {
                        Token = apiToken,
                        Version = Global.Version,
                        BaseUrl = SettingHelper.Instance.ApiUrl,
                    };
                    break;
            }
            if (loginData == null) return;

            ServiceProvider.Services.GetService<IConService>().SetProxy(() => SettingHelper.Instance.UseProxy);
            var conResult = await ServiceProvider.Services.GetService<IConService>().Connect(loginData);
            if (!conResult) return;

            // Has navigated to another page
            if (App.Frame.CurrentSourcePageType != typeof(AuthPage))
            {
                return;
            }

            (Window.Current.Content as Frame)?.Navigate(typeof(ShellPage));
            SettingHelper.Instance.IsFirstInited = true;
        }

#if WITHOUT_HELLO
#else
        /// <summary>
        /// 是否需要验证 WindowsHello
        /// </summary>
        /// <returns></returns>
        private bool ShouldAuthWindowsHello
        {
            get
            {
                // 没有首次加载
                if (!SettingHelper.Instance.IsFirstInited) return false;

                // GitHub 没有登录过
                if (SettingHelper.Instance.Provider == Provider.GitHub && string.IsNullOrEmpty(SettingHelper.Instance.GitHubToken)) return false;

                // Gitee 没有登录过
                if (SettingHelper.Instance.Provider == Provider.Gitee && string.IsNullOrEmpty(SettingHelper.Instance.GiteeToken)) return false;

                // 云接口 没有登录过
                if (SettingHelper.Instance.Provider == Provider.Api && string.IsNullOrEmpty(SettingHelper.Instance.ApiToken)) return false;

                return SettingHelper.Instance.IsAuthOpen;
            }
        }

        private async Task<bool> WindowsHelloAuth()
        {
            if (await KeyCredentialManager.IsSupportedAsync())
            {
                var keyCreationResult = await KeyCredentialManager.RequestCreateAsync(SystemInformation.Instance.ApplicationName, KeyCredentialCreationOption.ReplaceExisting);
                return keyCreationResult.Status == KeyCredentialStatus.Success;
            }
            else
            {
                MessageHelper.ShowDanger("请在系统设置中打开Windows Hello");
                SupportWindowsHello = false;
                return false;
            }
        }
#endif
    }
}