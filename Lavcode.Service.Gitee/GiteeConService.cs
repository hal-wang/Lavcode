using Lavcode.Service.BaseGit;
using Lavcode.Service.BaseGit.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavcode.Service.Gitee
{
    public class GiteeConService : BaseGitConService
    {
        protected override Task<BaseGit.Models.CommentItem<T>> CreateComment<T>(int issueNumber, T value)
        {
            throw new NotImplementedException();
        }

        protected override Task<IssueItem<T>> CreateIssue<T>(string name)
        {
            throw new NotImplementedException();
        }

        protected override Task DeleteComment(int commentId)
        {
            throw new NotImplementedException();
        }

        protected override Task<IReadOnlyList<BaseGit.Models.CommentItem<T>>> GetPageComments<T>(int page, int issueNumber)
        {
            throw new NotImplementedException();
        }

        protected override Task<IReadOnlyList<IssueItem>> GetPageIssues(int page)
        {
            throw new NotImplementedException();
        }

        protected override Task<BaseGit.Models.CommentItem<T>> UpdateComment<T>(int commentId, T value)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> BeforeConnect(object args)
        {
            throw new NotImplementedException();
        }

        protected override Task<RepositoryItem> GetRepository()
        {
            throw new NotImplementedException();
        }

        protected override Task<string> GetUserLogin()
        {
            throw new NotImplementedException();
        }
    }
}
