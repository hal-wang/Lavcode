using Hubery.Lavcode.Uwp.Helpers.Api;
using Hubery.Tools.Uwp.Helpers;
using Hubery.Yt.Uwp.Helpers;
using Microsoft.Toolkit.Collections;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hubery.Lavcode.Uwp.Controls.Comment
{
    /// <summary>
    /// 自动加载更多 Comments
    /// </summary>
    public class CommentSource : IIncrementalSource<Model.Api.Comment>
    {
        private readonly string _issueNumber;
        public CommentSource(string issueNumber)
        {
            _issueNumber = issueNumber;
        }

        public async Task<IEnumerable<Model.Api.Comment>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            try
            {
                var res = await ApiHelper.Repos.Get($"{Global.GiteeAccount}/{Global.Repos}/issues/{_issueNumber}/comments?page={pageIndex + 1}&per_page={pageSize}&order=desc");
                if (!await res.IsSuccess())
                {
                    return new List<Model.Api.Comment>();
                }

                return await res.GetContent<List<Model.Api.Comment>>();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex, 0);
                return new List<Model.Api.Comment>();
            }
        }
    }
}
