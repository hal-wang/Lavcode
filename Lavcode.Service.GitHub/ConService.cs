using HTools;
using Lavcode.Common;
using Lavcode.IService;
using Lavcode.Model;
using Newtonsoft.Json;
using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Lavcode.Service.GitHub
{
    public class ConService : IConService
    {
        private static readonly int _pageSize = 20;

        internal GitHubClient Client { get; private set; }
        internal User User { get; private set; }
        internal Repository Repository { get; private set; }

        internal (Issue Issue, IList<CommentItem<Folder>> Comments) FolderIssue { get; private set; }
        internal (Issue Issue, IList<CommentItem<Password>> Comments) PasswordIssue { get; private set; }
        internal (Issue Issue, IList<CommentItem<Icon>> Comments) IconIssue { get; private set; }
        internal (Issue Issue, IList<CommentItem<DelectedItem>> Comments) DelectedItemIssue { get; private set; }
        internal (Issue Issue, IList<CommentItem<KeyValuePair>> Comments) KeyValuePairIssue { get; private set; }
        internal (Issue Issue, IList<CommentItem<Config>> Comments) ConfigIssue { get; private set; }

        private (Issue Issue, IList<CommentItem<T>> Comments) GetIssues<T>()
        {
            if (typeof(T) == typeof(Folder))
            {
                return ((Issue Issue, IList<CommentItem<T>> Comments))FolderIssue;
            }
            else if (typeof(T) == typeof(Password))
            {
                return ((Issue Issue, IList<CommentItem<T>> Comments))PasswordIssue;
            }
            else if (typeof(T) == typeof(Icon))
            {
                return ((Issue Issue, IList<CommentItem<T>> Comments))IconIssue;
            }
            else if (typeof(T) == typeof(DelectedItem))
            {
                return ((Issue Issue, IList<CommentItem<T>> Comments))DelectedItemIssue;
            }
            else if (typeof(T) == typeof(KeyValuePair))
            {
                return ((Issue Issue, IList<CommentItem<T>> Comments))KeyValuePairIssue;
            }
            else if (typeof(T) == typeof(Config))
            {
                return ((Issue Issue, IList<CommentItem<T>> Comments))ConfigIssue;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        internal IList<T> GetComments<T, K>(K key, Func<T, K, bool> isEqual)
        {
            var issues = GetIssues<T>();
            return issues.Comments.Where(item => isEqual(item.Value, key)).Select(item => item.Value).ToList();
        }

        internal async Task UpdateComment<T>(T value, Func<T, T, bool> isEqual)
        {
            var issues = GetIssues<T>();
            var existComment = issues.Comments.Where(item => isEqual(item.Value, value)).FirstOrDefault();
            if (existComment == default) return;
            var newFolderComment = await Client.Issue.Comment.Update(Repository.Id, existComment.Comment.Id, JsonConvert.SerializeObject(value));
            var index = issues.Comments.IndexOf(existComment);
            issues.Comments.RemoveAt(index);
            issues.Comments.Insert(index, new CommentItem<T>()
            {
                Comment = newFolderComment,
                Value = value,
            });
        }

        internal async Task DeleteComment<T, K>(K key, Func<T, K, bool> isEqual)
        {
            var issues = GetIssues<T>();
            var delectedItems = issues.Comments.Where(item => isEqual(item.Value, key)).ToList();
            if (delectedItems == default) return;
            foreach (var di in delectedItems)
            {
                await Client.Issue.Comment.Delete(Repository.Id, di.Comment.Id);
                issues.Comments.Remove(di);
            }
        }

        internal async Task CreateComment<T>(T value)
        {
            var issues = GetIssues<T>();
            var newComment = await Client.Issue.Comment.Create(Repository.Id, issues.Issue.Number, JsonConvert.SerializeObject(value));
            issues.Comments.Add(new CommentItem<T>()
            {
                Comment = newComment,
                Value = value
            });
        }

        internal async Task UpsertComment<T>(T value, Func<T, T, bool> isEqual)
        {
            var issues = GetIssues<T>();
            if (issues.Comments.Any(item => isEqual(item.Value, value)))
            {
                await UpdateComment(value, isEqual);
            }
            else
            {
                await CreateComment(value);
            }
        }

        public async Task<bool> Connect(object args)
        {
            var token = DynamicHelper.ToExpandoObject(args).Token as string;

            var credentials = new Credentials(token, AuthenticationType.Oauth);
            Client = new GitHubClient(new ProductHeaderValue("Lavcode")) { Credentials = credentials };

            try
            {
                User = await Client.User.Current();
                Repository = await GetRepository();

                var issues = await CreateTable(await GetAllIssues());
                FolderIssue = await GetIssueTableItems<Folder>(issues);
                PasswordIssue = await GetIssueTableItems<Password>(issues);
                IconIssue = await GetIssueTableItems<Icon>(issues);
                DelectedItemIssue = await GetIssueTableItems<DelectedItem>(issues);
                KeyValuePairIssue = await GetIssueTableItems<KeyValuePair>(issues);
                ConfigIssue = await GetIssueTableItems<Config>(issues);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        private async Task<Repository> GetRepository()
        {
            try
            {
                return await Client.Repository.Get(User.Login, RepositoryConstant.GitStorageRepos);
            }
            catch (NotFoundException)
            {
                return await Client.Repository.Create(new NewRepository(RepositoryConstant.GitStorageRepos)
                {
                    Private = true,
                    Homepage = CommonConstant.HomeUrl,
                    Description = "Lavcode 存储密码的仓库，请勿手动修改",
                    HasIssues = true,
                    HasDownloads = false,
                    HasWiki = false,
                    Visibility = RepositoryVisibility.Private,
                });
            }
        }

        private async Task<(Issue Issue, IList<CommentItem<T>> Comments)> GetIssueTableItems<T>(IList<Issue> issues)
        {
            var issue = issues.First(item => item.Title == typeof(T).Name);
            var comments = await GetAllComments(issue.Number);
            var items = comments.Select(item => new CommentItem<T>()
            {
                Comment = item,
                Value = JsonConvert.DeserializeObject<T>(item.Body),
            }).ToList();
            return (issue, items);
        }

        private async Task<IList<Issue>> CreateTable(IList<Issue> issues)
        {
            await CreateIssueTable(issues, nameof(Folder));
            await CreateIssueTable(issues, nameof(Password));
            await CreateIssueTable(issues, nameof(Icon));
            await CreateIssueTable(issues, nameof(DelectedItem));
            await CreateIssueTable(issues, nameof(KeyValuePair));
            await CreateIssueTable(issues, nameof(Config));
            return issues;
        }

        private async Task CreateIssueTable(IList<Issue> issues, string name)
        {
            if (!issues.Any(issue => issue.Title == name))
            {
                var newIssue = await Client.Issue.Create(User.Login, Repository.Name, new NewIssue(name));
                issues.Add(newIssue);
            }
        }

        private async Task<IReadOnlyList<Issue>> GetIssues(int page)
        {
            return await Client.Issue.GetAllForRepository(User.Login, Repository.Name, new ApiOptions()
            {
                PageCount = 1,
                PageSize = _pageSize,
                StartPage = page,
            });
        }
        private async Task<IList<Issue>> GetAllIssues()
        {
            var result = new List<Issue>();
            IReadOnlyList<Issue> issues;
            var page = 1;
            do
            {
                issues = await GetIssues(page);
                result.AddRange(issues);
                page++;
            } while (issues.Count >= _pageSize);
            return result;
        }

        private async Task<IReadOnlyList<IssueComment>> GetCommments(int issueNumber, int page)
        {
            return await Client.Issue.Comment.GetAllForIssue(User.Login, Repository.Name, issueNumber, new ApiOptions()
            {
                PageCount = 1,
                PageSize = _pageSize,
                StartPage = page,
            });
        }
        private async Task<IList<IssueComment>> GetAllComments(int issueNumber)
        {
            var result = new List<IssueComment>();
            IReadOnlyList<IssueComment> comments;
            var page = 1;
            do
            {
                comments = await GetCommments(issueNumber, page);
                result.AddRange(comments);
                page++;
            } while (comments.Count >= _pageSize);
            return result;
        }

        public void Dispose()
        {

        }
    }
}
