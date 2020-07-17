using Hubery.Lavcode.Uwp.Helpers;
using Microsoft.Toolkit.Collections;
using Octokit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hubery.Lavcode.Uwp.Controls.Comment
{
    /// <summary>
    /// 自动加载更多 Comments
    /// </summary>
    public class CommentSource : IIncrementalSource<IssueComment>
    {
        private readonly int _issueNumber;
        public CommentSource(int issueNumber)
        {
            _issueNumber = issueNumber;
        }

        private readonly GitHubClient _client = GitHubHelper.GetBaseClient();
        public async Task<IEnumerable<IssueComment>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _client.Issue.Comment.GetAllForIssue(Global.GitHubAccount, Global.Repos, _issueNumber,
                new ApiOptions()
                {
                    PageCount = 1,
                    PageSize = pageSize,
                    StartPage = pageIndex + 1,
                });
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex, 0);
                return new List<IssueComment>();
            }
        }
    }
}
