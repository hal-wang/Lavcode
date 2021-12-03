using HTools.Uwp.Helpers;
using Lavcode.Uwp.Helpers;
using Microsoft.Toolkit.Collections;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lavcode.Uwp.Controls.Comment
{
    /// <summary>
    /// 自动加载更多 Comments
    /// </summary>
    public class CommentSource : IIncrementalSource<IssueComment>
    {
        private readonly int _issueNumber;
        private readonly int _total;

        public CommentSource(int issueNumber, int total)
        {
            _issueNumber = issueNumber;
            _total = total;
        }

        private readonly GitHubClient _client = GitHubHelper.GetBaseClient(Global.Repos);
        public async Task<IEnumerable<IssueComment>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            try
            {
                var page = GetPagesCount(pageSize) - pageIndex;
                if (page <= 0) return new List<IssueComment>();

                var result = await _client.Issue.Comment.GetAllForIssue(Global.GitAccount, Global.Repos, _issueNumber,
                    new IssueCommentRequest()
                    {
                        Since = Global.PublishTime,
                    },
                    new ApiOptions()
                    {
                        PageCount = 1,
                        PageSize = pageSize,
                        StartPage = page,
                    });

                return result.Reverse();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex, 0);
                return new List<IssueComment>();
            }
        }

        private int GetPagesCount(int pageSize)
        {
            return (int)Math.Ceiling(_total / (double)pageSize);
        }
    }
}
