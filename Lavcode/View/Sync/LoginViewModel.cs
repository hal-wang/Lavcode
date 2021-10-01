using GalaSoft.MvvmLight;
using Lavcode.Helpers;
using HTools.Uwp;
using HTools.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;

namespace Lavcode.View.Sync
{
    public class LoginViewModel : ViewModelBase, IElementViewModel
    {
        public FrameworkElement View { get; set; }

        public LoginViewModel()
        {
            SelectedCloud = CloudItems.Where((item) => item.CloudType == SettingHelper.Instance.DavCloudType).First();
        }

        private string _account = SettingHelper.Instance.DavAccount;
        public string Account
        {
            get { return _account; }
            set { Set(ref _account, value); }
        }

        private string _password = SettingHelper.Instance.DavPassword;
        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value); }
        }

        private string _url = SettingHelper.Instance.DavCustomUrl;
        public string Url
        {
            get { return _url; }
            set { Set(ref _url, value); }
        }

        public IReadOnlyList<CloudItem> CloudItems { get; } = Global.CloudItems;

        private CloudItem _selectedCloud = null;
        public CloudItem SelectedCloud
        {
            get { return _selectedCloud; }
            set { Set(ref _selectedCloud, value); }
        }

        public bool Finish()
        {
            if (string.IsNullOrEmpty(Account))
            {
                MessageHelper.ShowSticky(View.FindName("AccountTextBox") as FrameworkElement, "请输入账号");
                return false;
            }

            if (string.IsNullOrEmpty(Password))
            {
                MessageHelper.ShowSticky(View.FindName("AccountPasswordBox") as FrameworkElement, "请输入密码");
                return false;
            }

            if (SelectedCloud.CloudType == CloudType.Other && !Uri.IsWellFormedUriString(Url, UriKind.Absolute))
            {
                MessageHelper.ShowSticky(View.FindName("UrlTextBox") as FrameworkElement, "WebDAV地址格式不正确");
                return false;
            }

            SettingHelper.Instance.DavAccount = Account;
            SettingHelper.Instance.DavPassword = Password;
            SettingHelper.Instance.DavCloudType = SelectedCloud.CloudType;
            SettingHelper.Instance.DavCustomUrl = Url?.TrimEnd('/');
            return true;
        }
    }
}
