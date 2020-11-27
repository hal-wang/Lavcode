using GalaSoft.MvvmLight;
using Hubery.Lavcode.Uwp.Helpers;
using Hubery.Tools.Uwp.Helpers;
using Octokit;
using System;
using System.Threading.Tasks;

namespace Hubery.Lavcode.Uwp.View.Feedback
{
    class FeedbackDialogViewModel : ViewModelBase
    {
        private string _account = SettingHelper.Instance.GitAccount;
        public string Account
        {
            get { return _account; }
            set { Set(ref _account, value); }
        }

        private string _password = SettingHelper.Instance.GitPassword;
        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value); }
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return _content; }
            set { Set(ref _content, value); }
        }

        private bool _remember = !string.IsNullOrEmpty(SettingHelper.Instance.GitPassword);
        public bool Remember
        {
            get { return _remember; }
            set { Set(ref _remember, value); }
        }

        public IssueComment CommentResult { get; private set; } = null;

        public async Task<bool> Feedback()
        {
            if (!IsValid())
            {
                return false;
            }

            GitHubClient client = GitHubHelper.GetAuthClient(Account, Password);
            try
            {
                CommentResult = await client.Issue.Comment.Create(Global.GitAccount, Global.Repos, Global.FeedbackIssueNumber, Content);
                SettingHelper.Instance.GitAccount = Account;
                SettingHelper.Instance.GitPassword = Remember ? Password : null;
            }
            catch (System.Net.Http.HttpRequestException)
            {
                MessageHelper.ShowDanger("提交失败，请检查密码，或网络设置。");
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

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(Content))
            {
                MessageHelper.ShowWarning("请输入反馈内容");
                return false;
            }
            if (string.IsNullOrEmpty(Account))
            {
                MessageHelper.ShowWarning("请输入GitHub账号");
                return false;
            }
            if (string.IsNullOrEmpty(Password))
            {
                MessageHelper.ShowWarning("请输入GitHub密码");
                return false;
            }

            return true;
        }
    }
}
