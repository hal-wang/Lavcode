using HTools;
using Lavcode.IService;
using Lavcode.Service.BaseGit.Entities;
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

        internal IssueItem<FolderEntity> FolderIssue { get; private set; }
        internal IssueItem<PasswordEntity> PasswordIssue { get; private set; }
        internal IssueItem<IconEntity> IconIssue { get; private set; }
        internal IssueItem<KeyValuePairEntity> KeyValuePairIssue { get; private set; }

        private IssueItem<T> GetIssue<T>() where T : IEntity
        {
            if (typeof(T) == typeof(FolderEntity))
            {
                return FolderIssue as IssueItem<T>;
            }
            else if (typeof(T) == typeof(PasswordEntity))
            {
                return PasswordIssue as IssueItem<T>;
            }
            else if (typeof(T) == typeof(IconEntity))
            {
                return IconIssue as IssueItem<T>;
            }
            else if (typeof(T) == typeof(KeyValuePairEntity))
            {
                return KeyValuePairIssue as IssueItem<T>;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private string GetTableName<T>() where T : IEntity
        {
            if (typeof(T) == typeof(FolderEntity))
            {
                return "Folder";
            }
            else if (typeof(T) == typeof(PasswordEntity))
            {
                return "Password";
            }
            else if (typeof(T) == typeof(IconEntity))
            {
                return "Icon";
            }
            else if (typeof(T) == typeof(KeyValuePairEntity))
            {
                return "KeyValuePair";
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        internal IList<T> GetComments<T, K>(K key, Func<T, K, bool> isEqual) where T : IEntity
        {
            return GetIssue<T>().Comments.Where(item => isEqual(item.Value, key)).Select(item => item.Value).ToList();
        }

        internal async Task UpdateComment<T>(T value, Func<T, T, bool> isEqual) where T : IEntity
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

        internal async Task DeleteComment<T, K>(K key, Func<T, K, bool> isEqual) where T : IEntity
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

        internal async Task CreateComment<T>(T value) where T : IEntity
        {
            var issues = GetIssue<T>();
            var newComment = await CreateComment(issues.IssueNumber, value);
            issues.Comments.Add(new CommentItem<T>()
            {
                Id = newComment.Id,
                Value = value
            });
        }

        internal async Task UpsertComment<T>(T value, Func<T, T, bool> isEqual) where T : IEntity
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

        private object _args;
        public virtual async Task<bool> Connect(object args)
        {
            if (!await BeforeConnect(args)) return false;

            _args = args;

            UserLogin = await GetUserLogin();
            Repository = await GetRepository();

            var issues = await CreateTable(await GetAllIssues());
            FolderIssue = await GetIssueTableItems<FolderEntity>(issues);
            PasswordIssue = await GetIssueTableItems<PasswordEntity>(issues);
            IconIssue = await GetIssueTableItems<IconEntity>(issues);
            KeyValuePairIssue = await GetIssueTableItems<KeyValuePairEntity>(issues);
            return true;
        }

        public virtual async Task<bool> Refresh()
        {
            return await Connect(_args);
        }

        private async Task<IssueItem<T>> GetIssueTableItems<T>(IList<IssueItem> issues) where T : IEntity
        {
            var issue = issues.First(item => item.Title == GetTableName<T>());
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
            await CreateIssueTable<FolderEntity>(issues);
            await CreateIssueTable<PasswordEntity>(issues);
            await CreateIssueTable<IconEntity>(issues);
            await CreateIssueTable<KeyValuePairEntity>(issues);
            return issues;
        }

        private async Task CreateIssueTable<T>(IList<IssueItem> issues) where T : IEntity
        {
            var name = GetTableName<T>();
            if (!issues.Any(issue => issue.Title == name))
            {
                var newIssue = await CreateIssue<T>(name);
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

        private async Task<IList<CommentItem<T>>> GetAllComments<T>(OneOf<int, string> issueNumber) where T : IEntity
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
        protected abstract Task DeleteComment(long commentId);
        protected abstract Task<CommentItem<T>> UpdateComment<T>(long commentId, T value) where T : IEntity;
        protected abstract Task<IReadOnlyList<CommentItem<T>>> GetPageComments<T>(int page, OneOf<int, string> issueNumber) where T : IEntity;
        protected abstract Task<CommentItem<T>> CreateComment<T>(OneOf<int, string> issueNumber, T value) where T : IEntity;
        protected abstract Task<IReadOnlyList<IssueItem>> GetPageIssues(int page);
        protected abstract Task<IssueItem<T>> CreateIssue<T>(string name) where T : IEntity;
    }
}
