using HTools;
using Lavcode.Common;
using Lavcode.Service.BaseGit;
using Lavcode.Service.BaseGit.Models;
using Newtonsoft.Json;
using Octokit;
using OneOf;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lavcode.Service.GitHub
{
    public class GitHubConService : BaseGitConService
    {
        internal GitHubClient Client { get; private set; }

        protected override async Task<string> GetUserLogin()
        {
            var user = await Client.User.Current();
            return user.Login;
        }

        protected async override Task<RepositoryItem> GetRepository()
        {
            Repository repository;
            try
            {
                repository = await Client.Repository.Get(UserLogin, RepositoryConstant.GitStorageRepos);
            }
            catch (NotFoundException)
            {
                repository = await Client.Repository.Create(new NewRepository(RepositoryConstant.GitStorageRepos)
                {
                    Private = true,
                    Homepage = CommonConstant.HomeUrl,
                    Description = "Lavcode 存储密码的仓库，请勿手动修改",
                    HasIssues = true,
                    HasDownloads = false,
                    HasWiki = false,
                    Visibility = RepositoryVisibility.Private,
                    AutoInit = true,
                });
            }
            return new RepositoryItem()
            {
                Id = repository.Id,
                Name = repository.Name,
            };
        }

        protected override async Task DeleteComment(int commentId)
        {
            await Client.Issue.Comment.Delete(Repository.Id, commentId);
        }

        protected override async Task<CommentItem<T>> UpdateComment<T>(int commentId, T value)
        {
            var newComment = await Client.Issue.Comment.Update(Repository.Id, commentId, JsonConvert.SerializeObject(value));
            return new CommentItem<T>()
            {
                Id = newComment.Id,
                Value = JsonConvert.DeserializeObject<T>(newComment.Body),
            };
        }

        protected override async Task<CommentItem<T>> CreateComment<T>(OneOf<int, string> issueNumber, T value)
        {
            var newComment = await Client.Issue.Comment.Create(Repository.Id, issueNumber.AsT0, JsonConvert.SerializeObject(value));
            return new CommentItem<T>()
            {
                Id = newComment.Id,
                Value = JsonConvert.DeserializeObject<T>(newComment.Body),
            };
        }

        protected override async Task<IReadOnlyList<CommentItem<T>>> GetPageComments<T>(int page, OneOf<int, string> issueNumber)
        {
            return (await Client.Issue.Comment.GetAllForIssue(UserLogin, Repository.Name, issueNumber.AsT0, new ApiOptions()
            {
                PageCount = 1,
                PageSize = PageSize,
                StartPage = page,
            }))
            .Select(item => new CommentItem<T>()
            {
                Id = item.Id,
                Value = JsonConvert.DeserializeObject<T>(item.Body)
            })
            .ToList();
        }

        protected override async Task<IReadOnlyList<IssueItem>> GetPageIssues(int page)
        {
            return (await Client.Issue.GetAllForRepository(UserLogin, Repository.Name, new ApiOptions()
            {
                PageCount = 1,
                PageSize = PageSize,
                StartPage = page,
            }))
            .Select(item => new IssueItem()
            {
                IssueId = item.Id,
                IssueNumber = item.Number,
                Title = item.Title,
            })
            .ToList();
        }

        protected override async Task<IssueItem<T>> CreateIssue<T>(string name)
        {
            var issue = await Client.Issue.Create(UserLogin, Repository.Name, new NewIssue(name));
            return new IssueItem<T>()
            {
                Title = issue.Title,
                IssueId = issue.Id,
                IssueNumber = issue.Number,
            };
        }

        protected override Task<bool> BeforeConnect(object args)
        {
            var token = DynamicHelper.ToExpandoObject(args).Token as string;

            var credentials = new Credentials(token, AuthenticationType.Oauth);
            Client = new GitHubClient(new ProductHeaderValue("Lavcode")) { Credentials = credentials };
            return Task.FromResult(true);
        }
    }
}
