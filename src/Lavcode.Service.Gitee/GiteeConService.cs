using HTools;
using Lavcode.Common;
using Lavcode.Service.BaseGit;
using Lavcode.Service.BaseGit.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Lavcode.Service.Gitee
{
    public class GiteeConService : BaseGitConService
    {
        private string _token = string.Empty;

        private HttpClient HttpClient => new(new HttpClientHandler()
        {
            UseProxy = UseProxy?.Invoke() ?? false,
        })
        {
            BaseAddress = new Uri("https://gitee.com/api/v5/"),
            Timeout = TimeSpan.FromSeconds(5),
        };

        protected async override Task<CommentItem<T>> CreateComment<T>(OneOf<int, string> issueNumber, T value)
        {
            var res = await HttpClient.PostAsync($"repos/:owner/:repo/issues/:number/comments",
                query: new
                {
                    access_token = _token,
                    body = HttpUtility.UrlEncode(JsonConvert.SerializeObject(value)),
                },
                param: new
                {
                    owner = UserLogin,
                    number = issueNumber.AsT1,
                    repo = Repository.Name,
                });
            res.EnsureSuccessStatusCode();
            var comment = await res.GetContent<JObject>();

            return new CommentItem<T>()
            {
                Id = comment.Value<int>("id"),
                Value = JsonConvert.DeserializeObject<T>(comment.Value<string>("body")),
            };
        }

        protected async override Task<IssueItem<T>> CreateIssue<T>(string name)
        {
            var res = await HttpClient.PostAsync($"repos/:owner/issues",
                query: new
                {
                    access_token = _token,
                    title = name,
                    repo = Repository.Name,
                },
                param: new
                {
                    owner = UserLogin
                });
            res.EnsureSuccessStatusCode();
            var issue = await res.GetContent<JObject>();

            return new IssueItem<T>()
            {
                IssueId = issue.Value<int>("id"),
                IssueNumber = issue.Value<string>("number"),
                Title = issue.Value<string>("title"),
            };
        }

        protected async override Task DeleteComment(long commentId)
        {
            var res = await HttpClient.DeleteAsync($"repos/:owner/:repo/issues/comments/:id",
                query: new
                {
                    access_token = _token,
                },
                param: new
                {
                    owner = UserLogin,
                    id = commentId,
                    repo = Repository.Name,
                });
            res.EnsureSuccessStatusCode();
        }

        protected async override Task<IReadOnlyList<CommentItem<T>>> GetPageComments<T>(int page, OneOf<int, string> issueNumber)
        {
            var res = await HttpClient.GetAsync($"repos/:owner/:repo/issues/:number/comments",
                param: new
                {
                    owner = UserLogin,
                    repo = Repository.Name,
                    number = issueNumber.AsT1,
                },
                query: new
                {
                    access_token = _token,
                    page,
                    per_page = PageSize,
                });
            res.EnsureSuccessStatusCode();

            return (await res.GetContent<JObject[]>())
                .Select(item => new CommentItem<T>()
                {
                    Id = item.Value<int>("id"),
                    Value = JsonConvert.DeserializeObject<T>(item.Value<string>("body"))
                })
                .ToList();
        }

        protected async override Task<IReadOnlyList<IssueItem>> GetPageIssues(int page)
        {
            var res = await HttpClient.GetAsync($"repos/:owner/:repo/issues",
                param: new
                {
                    owner = UserLogin,
                    repo = Repository.Name,
                },
                query: new
                {
                    access_token = _token,
                    page,
                    per_page = PageSize,
                });
            res.EnsureSuccessStatusCode();

            return (await res.GetContent<JObject[]>())
                .Select(item => new IssueItem()
                {
                    IssueId = item.Value<int>("id"),
                    IssueNumber = item.Value<string>("number"),
                    Title = item.Value<string>("title"),
                })
                .ToList();
        }

        protected async override Task<CommentItem<T>> UpdateComment<T>(long commentId, T value)
        {
            var res = await HttpClient.PatchAsync($"repos/:owner/:repo/issues/comments/:id",
                query: new
                {
                    access_token = _token,
                    body = HttpUtility.UrlEncode(JsonConvert.SerializeObject(value)),
                },
                param: new
                {
                    owner = UserLogin,
                    id = commentId,
                    repo = Repository.Name,
                });
            res.EnsureSuccessStatusCode();
            var comment = await res.GetContent<JObject>();

            return new CommentItem<T>()
            {
                Id = comment.Value<int>("id"),
                Value = JsonConvert.DeserializeObject<T>(comment.Value<string>("body")),
            };
        }

        protected override Task<bool> BeforeConnect(object args)
        {
            _token = DynamicHelper.ToExpandoObject(args).Token as string;
            return Task.FromResult(true);
        }

        protected async override Task<RepositoryItem> GetRepository()
        {
            var res = await HttpClient.GetAsync($"repos/:owner/:repo", param: new
            {
                owner = UserLogin,
                repo = RepositoryConstant.GitStorageRepos,
            },
            query: new
            {
                access_token = _token,
            });
            if (res.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                res = await HttpClient.PostAsync($"user/repos", query: new
                {
                    access_token = _token,
                    repo = RepositoryConstant.GitStorageRepos,
                    @private = true,
                    homepage = CommonConstant.HomeUrl,
                    description = "Lavcode 存储密码的仓库，请勿手动修改",
                    has_issues = true,
                    has_wiki = false,
                    auto_init = true,
                });
            }
            res.EnsureSuccessStatusCode();

            var repo = await res.GetContent<JObject>();
            return new RepositoryItem()
            {
                Id = repo.Value<long>("id"),
                Name = repo.Value<string>("name"),
            };
        }

        protected override async Task<string> GetUserLogin()
        {
            var res = await HttpClient.GetAsync("user", query: new
            {
                access_token = _token
            });
            res.EnsureSuccessStatusCode();
            var user = await res.GetContent<JObject>();
            return user.Value<string>("login");
        }
    }
}
