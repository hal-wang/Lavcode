using HTools;
using Lavcode.IService;
using Lavcode.Model;
using Lavcode.Service.BaseGit.Models;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lavcode.Service.BaseGit
{
    public abstract class BaseGitConService : IConService
    {
        public Func<bool> UseProxy { get; private set; } = null;
        protected int PageSize { get; } = 20;

        internal IssueItem<Folder> FolderIssue { get; private set; }
        internal IssueItem<Password> PasswordIssue { get; private set; }
        internal IssueItem<Icon> IconIssue { get; private set; }
        internal IssueItem<DelectedItem> DelectedItemIssue { get; private set; }
        internal IssueItem<KeyValuePair> KeyValuePairIssue { get; private set; }

        private IssueItem<T> GetIssue<T>()
        {
            if (typeof(T) == typeof(Folder))
            {
                return FolderIssue as IssueItem<T>;
            }
            else if (typeof(T) == typeof(Password))
            {
                return PasswordIssue as IssueItem<T>;
            }
            else if (typeof(T) == typeof(Icon))
            {
                return IconIssue as IssueItem<T>;
            }
            else if (typeof(T) == typeof(DelectedItem))
            {
                return DelectedItemIssue as IssueItem<T>;
            }
            else if (typeof(T) == typeof(KeyValuePair))
            {
                return KeyValuePairIssue as IssueItem<T>;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        internal IList<T> GetComments<T, K>(K key, Func<T, K, bool> isEqual)
        {
            return GetIssue<T>().Comments.Where(item => isEqual(item.Value, key)).Select(item => item.Value).ToList();
        }

        internal async Task UpdateComment<T>(T value, Func<T, T, bool> isEqual)
        {
            var issues = GetIssue<T>();
            var existComment = issues.Comments.Where(item => isEqual(item.Value, value)).FirstOrDefault();
            if (existComment == default) return;
            var newFolderComment = await UpdateComment(existComment.Id, value);
            var index = issues.Comments.IndexOf(existComment);
            issues.Comments.RemoveAt(index);
            issues.Comments.Insert(index, new CommentItem<T>()
            {
                Id = newFolderComment.Id,
                Value = value,
            });
        }

        internal async Task DeleteComment<T, K>(K key, Func<T, K, bool> isEqual)
        {
            var issues = GetIssue<T>();
            var delectedItems = issues.Comments.Where(item => isEqual(item.Value, key)).ToList();
            if (delectedItems == default) return;
            foreach (var di in delectedItems)
            {
                await DeleteComment(di.Id);
                issues.Comments.Remove(di);
            }
        }

        internal async Task CreateComment<T>(T value)
        {
            var issues = GetIssue<T>();
            var newComment = await CreateComment(issues.IssueNumber, value);
            issues.Comments.Add(new CommentItem<T>()
            {
                Id = newComment.Id,
                Value = value
            });
        }

        internal async Task UpsertComment<T>(T value, Func<T, T, bool> isEqual)
        {
            var issues = GetIssue<T>();
            if (issues.Comments.Any(item => isEqual(item.Value, value)))
            {
                await UpdateComment(value, isEqual);
            }
            else
            {
                await CreateComment(value);
            }
        }

        public virtual async Task<bool> Connect(object args)
        {
            if (!await BeforeConnect(args)) return false;

            UserLogin = await GetUserLogin();
            Repository = await GetRepository();

            var issues = await CreateTable(await GetAllIssues());
            FolderIssue = await GetIssueTableItems<Folder>(issues);
            PasswordIssue = await GetIssueTableItems<Password>(issues);
            IconIssue = await GetIssueTableItems<Icon>(issues);
            DelectedItemIssue = await GetIssueTableItems<DelectedItem>(issues);
            KeyValuePairIssue = await GetIssueTableItems<KeyValuePair>(issues);
            return true;
        }

        private async Task<IssueItem<T>> GetIssueTableItems<T>(IList<IssueItem> issues)
        {
            var issue = issues.First(item => item.Title == typeof(T).Name);
            var comments = await GetAllComments<T>(issue.IssueNumber);
            return new IssueItem<T>()
            {
                IssueId = issue.IssueId,
                Comments = comments,
                Title = issue.Title,
                IssueNumber = issue.IssueNumber,
            };
        }

        private async Task<IList<IssueItem>> CreateTable(IList<IssueItem> issues)
        {
            await CreateIssueTable<Folder>(issues);
            await CreateIssueTable<Password>(issues);
            await CreateIssueTable<Icon>(issues);
            await CreateIssueTable<DelectedItem>(issues);
            await CreateIssueTable<KeyValuePair>(issues);
            return issues;
        }

        private async Task CreateIssueTable<T>(IList<IssueItem> issues)
        {
            if (!issues.Any(issue => issue.Title == typeof(T).Name))
            {
                var newIssue = await CreateIssue<T>(typeof(T).Name);
                issues.Add(new IssueItem()
                {
                    IssueId = newIssue.IssueId,
                    IssueNumber = newIssue.IssueNumber,
                    Title = newIssue.Title,
                });
            }
        }

        private async Task<IList<IssueItem>> GetAllIssues()
        {
            var result = new List<IssueItem>();
            IReadOnlyList<IssueItem> issues;
            var page = 1;
            do
            {
                issues = await GetPageIssues(page);
                result.AddRange(issues);
                page++;
            } while (issues.Count >= PageSize);
            return result;
        }

        private async Task<IList<CommentItem<T>>> GetAllComments<T>(OneOf<int, string> issueNumber)
        {
            var result = new List<CommentItem<T>>();
            IReadOnlyList<CommentItem<T>> comments;
            var page = 1;
            do
            {
                comments = await GetPageComments<T>(page, issueNumber);
                result.AddRange(comments);
                page++;
            } while (comments.Count >= PageSize);
            return result;
        }

        public void SetProxy(Func<bool> useProxy)
        {
            UseProxy = useProxy;
        }

        public void Dispose()
        {

        }

        protected RepositoryItem Repository { get; set; }
        protected string UserLogin { get; set; }
        protected abstract Task<bool> BeforeConnect(object args);
        protected abstract Task<string> GetUserLogin();
        protected abstract Task<RepositoryItem> GetRepository();
        protected abstract Task DeleteComment(int commentId);
        protected abstract Task<CommentItem<T>> UpdateComment<T>(int commentId, T value);
        protected abstract Task<IReadOnlyList<CommentItem<T>>> GetPageComments<T>(int page, OneOf<int, string> issueNumber);
        protected abstract Task<CommentItem<T>> CreateComment<T>(OneOf<int, string> issueNumber, T value);
        protected abstract Task<IReadOnlyList<IssueItem>> GetPageIssues(int page);
        protected abstract Task<IssueItem<T>> CreateIssue<T>(string name);
    }
}
