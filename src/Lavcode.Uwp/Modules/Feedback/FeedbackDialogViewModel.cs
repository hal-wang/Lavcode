using HTools.Uwp.Helpers;
using Lavcode.Common;
using Lavcode.Uwp.Helpers;
using Lavcode.Uwp.Modules.Auth;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using Octokit;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Lavcode.Uwp.Modules.Feedback
{
    public class FeedbackDialogViewModel : ObservableObject
    {
        private string _content = string.Empty;
        public string Content
        {
            get { return _content; }
            set { SetProperty(ref _content, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public Issue IssueResult { get; private set; } = null;

        public async Task<bool> Feedback()
        {
            if (string.IsNullOrEmpty(Title))
            {
                MessageHelper.ShowWarning("请输入标题");
                return false;
            }

            try
            {
                var token = await OAuthLoginDialog.Login(Model.Provider.GitHub);
                if (string.IsNullOrEmpty(token)) return false;

                var client = GitHubHelper.GetAuthClient(RepositoryConstant.Repos, token);
                var newIssue = new NewIssue("From client, DO NOT EDIT!")
                {
                    Body = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new
                    {
                        title = Title,
                        body = Content,
                        platform = "uwp",
                        version = Global.Version,
                        provider = SettingHelper.Instance.Provider.ToString().ToLower(),
                    }))),
                };
                IssueResult = await client.Issue.Create(RepositoryConstant.GitAccount, RepositoryConstant.Repos, newIssue);
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

            Title = string.Empty;
            Content = string.Empty;
            MessageHelper.ShowPrimary("反馈成功，请等待短暂时间后刷新");
            return true;
        }

    }
}
