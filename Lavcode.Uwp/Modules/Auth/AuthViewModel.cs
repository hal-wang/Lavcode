﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using HTools.Uwp.Helpers;
using Lavcode.IService;
using Lavcode.Model;
using Lavcode.Uwp.Common;
using Lavcode.Uwp.Modules.Shell;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.Auth
{
    public class AuthViewModel : ViewModelBase
    {
        public Provider Provider => SettingHelper.Instance.Provider;

        private bool _supportWindowsHello = true;
        public bool SupportWindowsHello
        {
            get { return _supportWindowsHello; }
            set { Set(ref _supportWindowsHello, value); }
        }

        private bool _loading = false;
        public bool Loading
        {
            get { return _loading; }
            set { Set(ref _loading, value); }
        }

        public async Task Init()
        {
            try
            {
                await Login();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
            finally
            {
                LoadingHelper.Hide();
            }
        }

        public async void LoginCommand()
        {
            try
            {
                await Login();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }

        private async Task Login()
        {
            if (Loading)
            {
                return;
            }

            Loading = true;
            try
            {
                if (ShouldAuthWindowsHello && !await WindowsHelloAuth())
                {
                    return;
                }

                object loginData = null;
                switch (SettingHelper.Instance.Provider)
                {
                    case Provider.GitHub:
                        var githubToken = await new GitHubLogin().Login();
                        if (string.IsNullOrEmpty(githubToken)) return;
                        loginData = new { Token = githubToken };
                        ViewModelLocator.Register<Service.GitHub.ConService>();
                        break;
                    case Provider.Sqlite:
                        ViewModelLocator.Register<Service.Sqlite.ConService>();
                        loginData = new { FilePath = Global.SqliteFilePath };
                        break;
                }

                if (loginData == null) return;
                var conResult = await SimpleIoc.Default.GetInstance<IConService>().Connect(loginData);
                if (!conResult) return;

                (Window.Current.Content as Frame)?.Navigate(typeof(ShellPage));
                SettingHelper.Instance.IsFirstInited = true;
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
                if (SettingHelper.Instance.Provider == Provider.GitHub && string.IsNullOrEmpty(SettingHelper.Instance.GitHubToken)) return false;

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
    }

}