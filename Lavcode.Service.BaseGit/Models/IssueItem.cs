using System.Collections.Generic;

namespace Lavcode.Service.BaseGit.Models
{
    public class IssueItem<T>
    {
        public int IssueId { get; set; }
        public int IssueNumber { get; set; }
        public string Title { get; set; }
        public IList<CommentItem<T>> Comments { get; set; }
    }

    public class IssueItem : IssueItem<object>
    {

    }
}
