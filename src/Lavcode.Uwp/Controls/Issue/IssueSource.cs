using HTools.Uwp.Helpers;
using Lavcode.Common;
using Lavcode.Uwp.Helpers;
using Microsoft.Toolkit.Collections;
using Octokit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lavcode.Uwp.Modules.Feedback
{
    public class IssueSource : IIncrementalSource<Issue>
    {
        private readonly string[] _labels;
        private readonly ItemStateFilter _state;
        public IssueSource(string[] labels, ItemStateFilter state)
        {
            _labels = labels;
            _state = state;
        }

        private readonly GitHubClient _client = GitHubHelper.GetBaseClient(RepositoryConstant.Repos);
        public async Task<IEnumerable<Issue>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            try
            {
                var issueRequest = new RepositoryIssueRequest()
                {
                    SortDirection = SortDirection.Descending,
                    SortProperty = IssueSort.Updated,
                    State = _state,
                };
                if (_labels != null)
                {
                    foreach (var label in _labels)
                    {
                        issueRequest.Labels.Add(label);
                    }
                }

                return await _client.Issue.GetAllForRepository(
                    RepositoryConstant.GitAccount,
                    RepositoryConstant.Repos,
                    issueRequest,
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
                return new List<Issue>();
            }
        }
    }
}
