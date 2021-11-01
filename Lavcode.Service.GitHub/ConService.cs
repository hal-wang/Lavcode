using HTools;
using Lavcode.IService;
using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Lavcode.Service.GitHub
{
    public class ConService : IConService
    {
        private static readonly int _pageSize = 20;

        public GitHubClient Client { get; private set; }
        public Repository Repository { get; private set; }

        public async Task<bool> Connect(object args)
        {
            var token = DynamicHelper.ToExpandoObject(args).Token as string;
            var repos = DynamicHelper.ToExpandoObject(args).Repos as string;
            var account = DynamicHelper.ToExpandoObject(args).Account as string;

            var credentials = new Credentials(token, AuthenticationType.Oauth);
            Client = new GitHubClient(new ProductHeaderValue(repos)) { Credentials = credentials };

            try
            {
                Repository = await Client.Repository.Get(account, repos);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        public async Task<IReadOnlyList<Issue>> GetIssues(int page)
        {
            return await Client.Issue.GetAllForRepository(Repository.Owner.Login, Repository.Name, new ApiOptions()
            {
                PageCount = 1,
                PageSize = _pageSize,
                StartPage = page,
            });
        }

        public async Task CreateIssue(NewIssue newIssue)
        {
            await Client.Issue.Create(Repository.Owner.Login, Repository.Name, newIssue);
        }

        public async Task UpdateIssue(int issueNumber, IssueUpdate issue)
        {
            await Client.Issue.Update(Repository.Owner.Login, Repository.Name, issueNumber, issue);
        }

        public Task RemoveIssue(int issueNumber, IssueUpdate issue)
        {
            throw new Exception("GiHub 不支持以 API 方式删除 Issue，请访问 github.com 以删除该 Issue");
        }

        public async Task<IReadOnlyList<IssueComment>> GetCommments(int issueNumber, int page)
        {
            return await Client.Issue.Comment.GetAllForIssue(Repository.Owner.Login, Repository.Name, issueNumber, new ApiOptions()
            {
                PageCount = 1,
                PageSize = _pageSize,
                StartPage = page,
            });
        }

        public void Dispose()
        {

        }
    }
}
