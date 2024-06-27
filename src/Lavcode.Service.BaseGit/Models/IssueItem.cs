using OneOf;
using System.Collections.Generic;

namespace Lavcode.Service.BaseGit.Models
{
    public class IssueItem<T>
    {
        public long IssueId { get; set; }
        public OneOf<int, string> IssueNumber { get; set; }
        public string Title { get; set; }
        public IList<CommentItem<T>> Comments { get; set; }
    }

    public class IssueItem : IssueItem<object>
    {

    }
}
